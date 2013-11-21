using System;
using System.Threading;
using Gecko.IO;
using Gecko.Interop;

namespace Gecko
{
	/// <summary>
	/// Pipe transfers data from nsIAsyncOutputStream to nsIAsyncInputStream
	/// Can be used for transfer data between threads
	/// WARNING it seems that this class can be created only one time :(
	/// </summary>
	public sealed class Pipe
		:IDisposable 
	{
		internal ComPtr<nsIPipe> _pipe;

		public Pipe()
		{
			_pipe = Xpcom.CreateInstance2<nsIPipe>( Contracts.Pipe );
			_pipe.Instance.Init(true, true,0, 0, null);
		}

		~Pipe()
		{
			Release();
		}

		public void Dispose()
		{
			Release();
			GC.SuppressFinalize( this );
		}

		private void Release()
		{
			if (_pipe == null) return;
			Xpcom.DisposeObject( ref _pipe );
		}

		public InputStream InputStream
		{
			get { return IO.InputStream.Create(_pipe.Instance.GetInputStreamAttribute()); }
		}


		public OutputStream OutputStream
		{
			get
			{
				return _pipe.Instance.GetOutputStreamAttribute()
							.Wrap( x => new OutputStream( x ) );
			}
		}


	}
}