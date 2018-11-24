using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotKeys
{
	public class HotKeyRegistrationException : Exception
	{
		public HotKeyRegistrationException()
			: base()
		{
		}

		public HotKeyRegistrationException(string message)
			: base(message)
		{				
		}
	}
}
