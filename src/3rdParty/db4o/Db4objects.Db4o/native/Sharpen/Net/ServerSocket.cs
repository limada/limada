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
using System;
using System.Net;
using System.Net.Sockets;
using NativeSocket=System.Net.Sockets.Socket;

namespace Sharpen.Net
{
	public class ServerSocket : SocketWrapper
	{
		public ServerSocket(int port)
		{
#if !SILVERLIGHT
			try
            {
				NativeSocket socket = new NativeSocket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                socket.Bind(new IPEndPoint(IPAddress.Any, port));

                int maxPendingConnections = 42;
                socket.Listen(maxPendingConnections);
                Initialize(socket);
            }
            catch (SocketException e)
            {
                throw new System.IO.IOException(e.Message);
            }
#endif
		}

#if !SILVERLIGHT
		public Socket Accept()
		{
			return new Socket(_delegate.Accept());
		}

		public int GetLocalPort()
		{
			return ((IPEndPoint)_delegate.LocalEndPoint).Port;
		}
#endif
	}
}
