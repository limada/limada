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
using System.Net.Sockets;
using Db4objects.Db4o.CS.Foundation;
using Sharpen.Net;

namespace Db4objects.Db4o.CS.Foundation
{
	public abstract class NetworkServerSocketBase : IServerSocket4
	{
		protected abstract ServerSocket Socket();

		public virtual void SetSoTimeout(int timeout)
		{
			try
			{
				Socket().SetSoTimeout(timeout);
			}
			catch (SocketException e)
			{
				Sharpen.Runtime.PrintStackTrace(e);
			}
		}

		public virtual int GetLocalPort()
		{
			return Socket().GetLocalPort();
		}

		/// <exception cref="System.IO.IOException"></exception>
		public virtual ISocket4 Accept()
		{
			Sharpen.Net.Socket sock = Socket().Accept();
			// TODO: check connection permissions here
			return new NetworkSocket(sock);
		}

		/// <exception cref="System.IO.IOException"></exception>
		public virtual void Close()
		{
			Socket().Close();
		}
	}
}
