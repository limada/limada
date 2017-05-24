using System;
using System.IO;
using System.Linq;
using Xwt.Backends;
using System.Collections.Generic;

#if MONOMAC
using nint = System.Int32;
using nfloat = System.Single;
using MonoMac.Foundation;
using MonoMac.AppKit;
#else
using Foundation;
using AppKit;
#endif

namespace Xwt.Mac
{
	public partial class MacClipboardBackend
	{

		public override IEnumerable<TransferDataType> GetTypesAvailable ()
		{
			foreach (var t in NSPasteboard.GeneralPasteboard.Types)
				yield return TransferDataType.FromId (t);
		}

	}
}