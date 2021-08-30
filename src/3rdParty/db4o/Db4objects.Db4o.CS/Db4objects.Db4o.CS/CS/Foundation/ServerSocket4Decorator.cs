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
	public class ServerSocket4Decorator : IServerSocket4
	{
		public ServerSocket4Decorator(IServerSocket4 serverSocket)
		{
			_serverSocket = serverSocket;
		}

		/// <exception cref="System.IO.IOException"></exception>
		public virtual ISocket4 Accept()
		{
			return _serverSocket.Accept();
		}

		/// <exception cref="System.IO.IOException"></exception>
		public virtual void Close()
		{
			_serverSocket.Close();
		}

		public virtual int GetLocalPort()
		{
			return _serverSocket.GetLocalPort();
		}

		public virtual void SetSoTimeout(int timeout)
		{
			_serverSocket.SetSoTimeout(timeout);
		}

		protected IServerSocket4 _serverSocket;
	}
}
