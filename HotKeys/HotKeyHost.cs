using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Interop;

namespace Hotkeys
{
	public class HotkeyHost : IDisposable
	{
		// Constants
		private const int WM_HOTKEY = 0x0312;

		// Fields
		private Window hotkeyDetector;
		private IntPtr handle;
		private HwndSource source;
		private HashSet<Hotkey> hotkeys;
		private int firstFreeId;

		// Constructors
		public HotkeyHost()
		{
			this.hotkeyDetector = new Window();
			this.handle = new WindowInteropHelper(this.hotkeyDetector).EnsureHandle();
			this.source = HwndSource.FromHwnd(handle);
			this.source.AddHook(HwndHook);
			this.hotkeys = new HashSet<Hotkey>();
			this.firstFreeId = 0;
		}

		// Methods
		[DllImport("user32.dll")]
		private static extern bool RegisterHotkey(IntPtr hWnd, int id, int fsModifiers, int vlc);

		[DllImport("user32.dll")]
		private static extern bool UnregisterHotkey(IntPtr hWnd, int id);

		public void RegisterHotkey(Hotkey hotkey)
		{
			if (hotkey.IsRegistered)
			{
				throw new HotkeyRegistrationException("The given Hotkey is already registered.");
			}

			if (hotkey.Key == 0)
			{
				throw new HotkeyRegistrationException("Key is missing.");
			}

			RegisterHotkey(this.handle, firstFreeId, (int)hotkey.Modifiers, (int)hotkey.Key);
			hotkey.Id = firstFreeId;
			hotkey.IsRegistered = true;
			firstFreeId++;
			this.hotkeys.Add(hotkey);
		}

		public void UnregisterHotkey(Hotkey hotkey)
		{
			if (!hotkey.IsRegistered)
			{ 
				throw new HotkeyRegistrationException("The given Hotkey is not registered.");
			}
			
			if (!this.hotkeys.Contains(hotkey))
			{
				throw new HotkeyRegistrationException("The given Hotkey is not registered here.");
			}

			UnregisterHotkey(handle, (int)hotkey.Id);
			hotkey.Id = null;
			hotkey.IsRegistered = false;
			this.hotkeys.Remove(hotkey);
		}

		public IReadOnlyCollection<Hotkey> Hotkeys
		{
			get
			{
				return this.hotkeys as IReadOnlyCollection<Hotkey>;
			}
		}

		private IntPtr HwndHook(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
		{
			handled = false;

			if (msg == WM_HOTKEY)
			{
				var firedId = wParam.ToInt32();
				foreach (var hk in hotkeys)
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
			foreach (var hk in this.hotkeys)
			{
				UnregisterHotkey(handle, (int)hk.Id);
			}

			this.source.RemoveHook(HwndHook);
			this.hotkeyDetector.Dispatcher.Invoke(() => hotkeyDetector.Close());
		}
	}
}
