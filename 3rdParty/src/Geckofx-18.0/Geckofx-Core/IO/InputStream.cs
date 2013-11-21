using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using Gecko.Interop;

namespace Gecko.IO
{
	public class InputStream
		:System.IO.Stream
	{
		private readonly bool _seekable;
		internal ComPtr<nsIInputStream> _inputStream;
		private ComPtr<nsISeekableStream> _seekableStream;

		internal InputStream(nsIInputStream inputStream)
		{
			_inputStream = new ComPtr<nsIInputStream>( inputStream );
			var seekableStream = Xpcom.QueryInterface<nsISeekableStream>(inputStream);
			if ( _seekable = seekableStream != null )
			{
				_seekableStream = new ComPtr<nsISeekableStream>( seekableStream );
			}
		}

		public override void Flush()
		{
		}

		public override void Close()
		{
			// this method is automatically called by Dispose
			if (_inputStream != null)
			{
				_inputStream.Instance.Close();
				Xpcom.DisposeObject(ref _seekableStream);
				Xpcom.DisposeObject(ref _inputStream);
			}
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
			var position = _seekableStream.Instance.Tell();
			_seekableStream.Instance.Seek(0, (int)value);
			_seekableStream.Instance.SetEOF();
			if (position < value)
			{
				// Returning to old position
				_seekableStream.Instance.Seek(0, position);
			}

		}

		public unsafe override int Read( byte[] buffer, int offset, int count )
		{
			uint ret;
			// strict values & buffer size check before using pointers
			if ((offset<0)||(count<=0)) return 0;
			// offset >= 0 count>0
			if ((offset+count)>buffer.Length) return 0;
			fixed (byte* bufferPtr =buffer)
			{
				byte* writePtr = bufferPtr + offset;
				ret = _inputStream.Instance.Read(new IntPtr(writePtr), (uint)count);
			}
			return ( int ) ret;
		}

		public unsafe override int ReadByte()
		{
			byte ret;
			byte* ptr = &ret;
			uint count = _inputStream.Instance.Read(new IntPtr(ptr), 1);
			return count == 0 ? -1 : ret;
		}

		/// <summary>
		/// Warning: .NET have another stream model. DON'T USE THIS FUNCTION
		/// InputStream can only read data
		/// </summary>
		/// <param name="buffer"></param>
		/// <param name="offset"></param>
		/// <param name="count"></param>
		public override void Write( byte[] buffer, int offset, int count )
		{
			throw new NotSupportedException("InputStream can only read data :)");
		}

		public override bool CanRead
		{
			get { return true; }
		}

		public override bool CanSeek
		{
			get { return _seekable; }
		}

		public override bool CanWrite
		{
			get { return false; }
		}

		/// <summary>
		/// Warning: .NET have another stream model. DON'T USE THIS PROPERTY
		/// </summary>
		public override long Length
		{
			get
			{
				return _seekableStream.Instance.Tell() + _inputStream.Instance.Available();
			}
		}

		public long Available
		{
			get { return _inputStream.Instance.Available(); }
		}

		public override long Position
		{
			get { return _seekableStream.Instance.Tell(); }
			set { _seekableStream.Instance.Seek(0, (int)value); }
		}



		public static InputStream Create(nsIInputStream stream)
		{
			var mimeInputStream= Xpcom.QueryInterface<nsIMIMEInputStream>(stream);
			if (mimeInputStream != null)
			{
				Marshal.ReleaseComObject( stream );
				return new MimeInputStream( mimeInputStream );
			}
			var stringInputStream = Xpcom.QueryInterface<nsIStringInputStream>( stream );
			if (stringInputStream != null)
			{
				Marshal.ReleaseComObject( stream );
				return new StringInputStream( stringInputStream );
			}
			return new InputStream(stream);
		}

		/// <summary>
		/// Method is useful when reading headers
		/// </summary>
		/// <returns></returns>
		public string ReadLine()
		{
			StringBuilder ret = new StringBuilder(64);
			var count = _inputStream.Instance.Available();
			for (var i = 0; i < count; i++)
			{
				var character = ReadByte();
				if (character < 0) break;
				char test = (char)(byte)character;
				if (test == '\r')
				{
					// nothing
				}
				else
				{
					if (test == '\n')
					{
						break;
					}
					ret.Append(test);
				}
			}
			return ret.ToString();
		}
	}
}
