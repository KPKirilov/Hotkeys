using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Interop;

namespace HotKeys
{
	public class HotKeyHost : IDisposable
	{
		// Constants
		private const int WM_HOTKEY = 0x0312;

		// Fields
		private Window hotKeyDetector;
		private IntPtr handle;
		private HwndSource source;
		private HashSet<HotKey> hotKeys;
		private int firstFreeId;

		// Constructors
		public HotKeyHost()
		{
			this.hotKeyDetector = new Window();
			this.handle = new WindowInteropHelper(this.hotKeyDetector).EnsureHandle();
			this.source = HwndSource.FromHwnd(handle);
			this.source.AddHook(HwndHook);
			this.hotKeys = new HashSet<HotKey>();
			this.firstFreeId = 0;
		}

		// Methods
		[DllImport("user32.dll")]
		private static extern bool RegisterHotKey(IntPtr hWnd, int id, int fsModifiers, int vlc);

		[DllImport("user32.dll")]
		private static extern bool UnregisterHotKey(IntPtr hWnd, int id);

		public void RegisterHotKey(HotKey hotKey)
		{
			if (hotKey.IsRegistered)
			{
				throw new HotKeyRegistrationException("The given HotKey is already registered.");
			}

			if (hotKey.Key == 0)
			{
				throw new HotKeyRegistrationException("Key is missing.");
			}

			RegisterHotKey(this.handle, firstFreeId, (int)hotKey.Modifiers, (int)hotKey.Key);
			hotKey.Id = firstFreeId;
			hotKey.IsRegistered = true;
			firstFreeId++;
			this.hotKeys.Add(hotKey);
		}

		public void UnregisterHotKey(HotKey hotKey)
		{
			if (!hotKey.IsRegistered)
			{ 
				throw new HotKeyRegistrationException("The given HotKey is not registered.");
			}
			
			if (!this.hotKeys.Contains(hotKey))
			{
				throw new HotKeyRegistrationException("The given HotKey is not registered here.");
			}

			UnregisterHotKey(handle, (int)hotKey.Id);
			hotKey.Id = null;
			hotKey.IsRegistered = false;
			this.hotKeys.Remove(hotKey);
		}

		public IReadOnlyCollection<HotKey> HotKeys
		{
			get
			{
				return this.hotKeys as IReadOnlyCollection<HotKey>;
			}
		}

		private IntPtr HwndHook(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
		{
			handled = false;

			if (msg == WM_HOTKEY)
			{
				var firedId = wParam.ToInt32();
				foreach (var hk in hotKeys)
				{
					if (hk.Id == firedId)
					{
						hk.OnFired();
						break;
					}
				}
			}

			handled = true;
			return IntPtr.Zero;
		}

		public void Dispose()
		{
			foreach (var hk in this.hotKeys)
			{
				UnregisterHotKey(handle, (int)hk.Id);
			}

			this.source.RemoveHook(HwndHook);
			this.hotKeyDetector.Dispatcher.Invoke(() => hotKeyDetector.Close());
		}
	}
}
