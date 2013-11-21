using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gecko.Interop;

namespace Gecko
{
	public class nsCancelable
	{
		private ComPtr<nsICancelable> _cancelable;

		internal nsCancelable(nsICancelable cancelable)
		{
			_cancelable = new ComPtr<nsICancelable>( cancelable );
		}

		public void Cancel(int reason)
		{
			_cancelable.Instance.Cancel( reason );
		}
	}
}
