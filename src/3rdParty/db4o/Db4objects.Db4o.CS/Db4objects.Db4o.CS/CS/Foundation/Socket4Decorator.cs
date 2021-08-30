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
using Db4objects.Db4o.CS.Foundation;

namespace Db4objects.Db4o.CS.Foundation
{
	/// <since>7.12</since>
	public class Socket4Decorator : ISocket4
	{
		public Socket4Decorator(ISocket4 socket)
		{
			_socket = socket;
		}

		/// <exception cref="System.IO.IOException"></exception>
		public virtual void Close()
		{
			_socket.Close();
		}

		/// <exception cref="System.IO.IOException"></exception>
		public virtual void Flush()
		{
			_socket.Flush();
		}

		public virtual bool IsConnected()
		{
			return _socket.IsConnected();
		}

		/// <exception cref="System.IO.IOException"></exception>
		public virtual ISocket4 OpenParallelSocket()
		{
			return _socket.OpenParallelSocket();
		}

		/// <exception cref="System.IO.IOException"></exception>
		public virtual int Read(byte[] buffer, int offset, int count)
		{
			return _socket.Read(buffer, offset, count);
		}

		public virtual void SetSoTimeout(int timeout)
		{
			_socket.SetSoTimeout(timeout);
		}

		/// <exception cref="System.IO.IOException"></exception>
		public virtual void Write(byte[] bytes, int offset, int count)
		{
			_socket.Write(bytes, offset, count);
		}

		public override string ToString()
		{
			return _socket.ToString();
		}

		protected ISocket4 _socket;
	}
}
