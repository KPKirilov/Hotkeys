using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hotkeys
{
	public class HotkeyRegistrationException : Exception
	{
		public HotkeyRegistrationException()
			: base()
		{
		}

		public HotkeyRegistrationException(string message)
			: base(message)
		{				
		}
	}
}
