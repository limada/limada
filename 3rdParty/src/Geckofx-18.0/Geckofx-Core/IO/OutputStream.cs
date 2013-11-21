using System;
using System.IO;
using Gecko.Interop;

namespace Gecko.IO
{
	public sealed class OutputStream
		:Stream
	{
		private readonly bool _seekable;
		private ComPtr<nsISeekableStream> _seekableStream;
		internal ComPtr<nsIOutputStream> _outputStream;
		private ComPtr<nsIBinaryOutputStream> _binaryOutputStream;

		internal OutputStream(nsIOutputStream outputStream)
		{
			_outputStream = new ComPtr<nsIOutputStream>( outputStream );
			var seekableStream = Xpcom.QueryInterface<nsISeekableStream>( outputStream );
			if ( _seekable = (seekableStream != null) )
			{
				_seekableStream = new ComPtr<nsISeekableStream>( seekableStream );
			}
			_binaryOutputStream = Xpcom.CreateInstance2<nsIBinaryOutputStream>(Contracts.BinaryOutputStream);
			_binaryOutputStream.Instance.SetOutputStream( _outputStream.Instance );
		}

		public override void Close()
		{
			// this method is automatically called by Dispose
			if ( _outputStream != null )
			{
				_binaryOutputStream.Instance.Close();
				_outputStream.Instance.Close();
				Xpcom.DisposeObject(ref _binaryOutputStream);
				Xpcom.DisposeObject(ref _seekableStream);
				Xpcom.DisposeObject(ref _outputStream);
			}
		}

		public override void Flush()
		{
			_binaryOutputStream.Instance.Flush();
			_outputStream.Instance.Flush();
		}

		public override long Seek( long offset, SeekOrigin origin )
		{
			//NS_SEEK_SET 	0 	Specifiesthat the offset is relative to the start of the stream.
			//NS_SEEK_CUR 	1 	Specifies that the offset is relative to the current position in the stream.
			//NS_SEEK_END 	2 	Specifies that the offset is relative to the end of the stream.
			_seekableStream.Instance.Seek((int)origin, (int)offset);
			return _seekableStream.Instance.Tell();
		}

		public override void SetLength( long value )
		{
			int current = _seekableStream.Instance.Tell();
			_seekableStream.Instance.Seek(0, (int)value);
			_seekableStream.Instance.SetEOF();
			_seekableStream.Instance.Seek(0, current);
		}

		public override int Read( byte[] buffer, int offset, int count )
		{
			throw new NotImplementedException();
		}

		public override void WriteByte(byte value)
		{
			_binaryOutputStream.Instance.Write8( value );
		}

		public override void Write(byte[] buffer, int offset, int count)
		{
			if ( offset == 0 )
			{
				_binaryOutputStream.Instance.WriteByteArray(buffer, (uint)count);
				return;
			}
			byte[] newArray = new byte[count - offset];
			Array.Copy( buffer, offset, newArray, 0, newArray.Length );
			_binaryOutputStream.Instance.WriteByteArray(newArray, (uint)newArray.Length);
		}

		public override bool CanRead
		{
			get { return false; }
		}

		public override bool CanSeek
		{
			get { return _seekable; }
		}

		public override bool CanWrite
		{
			get { return true; }
		}

		public override long Length
		{
			get { return _seekableStream.Instance.Tell(); }
		}

		public override long Position
		{
			get { return _seekableStream.Instance.Tell(); }
			set { _seekableStream.Instance.Seek(0, (int)Position); }
		}
	}
}