using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotKeys
{
	public class HotKeyFiredEventArgs : EventArgs
	{
		public HotKeyFiredEventArgs()
		{
		}

		public HotKeyFiredEventArgs(int hotKeyId, Modifiers modifiers, Key key)
		{
			HotKeyId = hotKeyId;
			Modifiers = Modifiers;
			Key = key;
		}

		//public IntPtr DummyWindowHandle { get; set; }
		public int HotKeyId { get; set; }
		public Modifiers Modifiers { get; set; }
		public Key Key { get; set; }
	}
}
