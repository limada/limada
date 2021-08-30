/* This file is part of the db4o object database http://www.db4o.com

Copyright (C) 2004 - 2010  Versant Corporation http://www.versant.com

db4o is free software; you can redistribute it and/or modify it under
the terms of version 3 of the GNU General Public License as published
by the Free Software Foundation.

db4o is distributed in the hope that it will be useful, but WITHOUT ANY
WARRANTY; without even the implied warranty of MERCHANTABILITY or
FITNESS FOR A PARTICULAR PURPOSE.  See the GNU General Public License
for more details.

You should have received a copy of the GNU General Public License along
with this program.  If not, see http://www.gnu.org/licenses/. */
using System.IO;
using Db4objects.Db4o.CS.Foundation;
using Db4objects.Db4o.Ext;

namespace Db4objects.Db4o.CS.Internal
{
	public class Socket4Adapter
	{
		private readonly ISocket4 _delegate;

		public Socket4Adapter(ISocket4 delegate_)
		{
			_delegate = delegate_;
		}

		public Socket4Adapter(ISocket4Factory socketFactory, string hostName, int port)
		{
			try
			{
				_delegate = socketFactory.CreateSocket(hostName, port);
			}
			catch (IOException e)
			{
				throw new Db4oIOException(e);
			}
		}

		/// <exception cref="Db4objects.Db4o.Ext.Db4oIOException"></exception>
		public virtual void Close()
		{
			try
			{
				_delegate.Close();
			}
			catch (IOException e)
			{
				throw new Db4oIOException(e);
			}
		}

		/// <exception cref="Db4objects.Db4o.Ext.Db4oIOException"></exception>
		public virtual void Flush()
		{
			try
			{
				_delegate.Flush();
			}
			catch (IOException e)
			{
				throw new Db4oIOException(e);
			}
		}

		public virtual bool IsConnected()
		{
			return _delegate.IsConnected();
		}

		/// <exception cref="Db4objects.Db4o.Ext.Db4oIOException"></exception>
		public virtual Db4objects.Db4o.CS.Internal.Socket4Adapter OpenParalellSocket()
		{
			try
			{
				return new Db4objects.Db4o.CS.Internal.Socket4Adapter(_delegate.OpenParallelSocket
					());
			}
			catch (IOException e)
			{
				throw new Db4oIOException(e);
			}
		}

		/// <exception cref="Db4objects.Db4o.Ext.Db4oIOException"></exception>
		public virtual int Read(byte[] buffer, int bufferOffset, int byteCount)
		{
			try
			{
				return _delegate.Read(buffer, bufferOffset, byteCount);
			}
			catch (IOException e)
			{
				throw new Db4oIOException(e);
			}
		}

		public virtual void SetSoTimeout(int timeout)
		{
			_delegate.SetSoTimeout(timeout);
		}

		/// <exception cref="Db4objects.Db4o.Ext.Db4oIOException"></exception>
		public virtual void Write(byte[] bytes, int offset, int count)
		{
			try
			{
				_delegate.Write(bytes, offset, count);
			}
			catch (IOException e)
			{
				throw new Db4oIOException(e);
			}
		}

		/// <exception cref="Db4objects.Db4o.Ext.Db4oIOException"></exception>
		public virtual void Write(byte[] bytes)
		{
			try
			{
				_delegate.Write(bytes, 0, bytes.Length);
			}
			catch (IOException e)
			{
				throw new Db4oIOException(e);
			}
		}

		public override string ToString()
		{
			return _delegate.ToString();
		}
	}
}
