using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hotkeys
{
	public class HotkeyModificationException : Exception
	{
		public HotkeyModificationException()
			: base()
		{
		}

		public HotkeyModificationException(string message)
			: base(message)
		{
		}
	}
}
