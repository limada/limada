using System;
using System.Collections.Generic;
using System.Linq;
using Xwt.Backends;

namespace Xwt.Backends
{
	public abstract partial class ClipboardBackend : BackendHandler
	{
		public abstract IEnumerable<TransferDataType> GetTypesAvailable ();
	}

	public partial class TransferDataStore {
		
		public DataRequestDelegate DataRequestCallback { get; set; }
		
		public IEnumerable<TransferDataType> DataTypes {
			get { return data.Keys; }
		}

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
		
		public static ITransferData GetTransferData (IEnumerable<TransferDataType> dataTypes) {
			var result = new TransferDataStore();
			result.DataRequestCallback = t => Clipboard.GetData(t);
			foreach (var dt in dataTypes.Where (t => Clipboard.ContainsData (t)))
				result.AddValue(dt, (object) null);
			return result;
		}

		public static void SetTransferData (TransferDataSource source) {
			foreach (var t in source.DataTypes)
				if (source.DataRequestCallback != null)
					Clipboard.SetData(t, source.DataRequestCallback);
				else
					Clipboard.SetData (t, source.GetValue (t));
		}
	}

	public partial interface ITransferData {
		IEnumerable<TransferDataType> DataTypes { get; }

	}

}
