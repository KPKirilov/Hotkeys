using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hotkeys
{
	public class HotkeyFiredEventArgs : EventArgs
	{
		public HotkeyFiredEventArgs()
		{
		}

		public HotkeyFiredEventArgs(int hotkeyId, Modifiers modifiers, Key key)
		{
			HotkeyId = hotkeyId;
			Modifiers = Modifiers;
			Key = key;
		}

		//public IntPtr DummyWindowHandle { get; set; }
		public int HotkeyId { get; set; }
		public Modifiers Modifiers { get; set; }
		public Key Key { get; set; }
	}
}
