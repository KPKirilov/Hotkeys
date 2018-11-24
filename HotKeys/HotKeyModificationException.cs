using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotKeys
{
	public class HotKeyModificationException : Exception
	{
		public HotKeyModificationException()
			: base()
		{
		}

		public HotKeyModificationException(string message)
			: base(message)
		{
		}
	}
}
