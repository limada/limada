using System;
using System.Collections.Generic;

namespace Xwt.Backends
{
	public abstract partial class ClipboardBackend : BackendHandler
	{
		public abstract IEnumerable<TransferDataType> GetTypesAvailable ();
	}
}

namespace Xwt
{
	public static partial class Clipboard
	{
		public static IEnumerable<TransferDataType> GetTypesAvailable ()
		{
			return Backend.GetTypesAvailable ();
		}
	}
}
