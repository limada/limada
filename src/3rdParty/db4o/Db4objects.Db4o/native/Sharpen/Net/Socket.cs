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
using System.IO;
using System.Net;
using Sharpen.IO;
using NativeSocket=System.Net.Sockets.Socket;
using System.Net.Sockets;

namespace Sharpen.Net
{
	public class Socket : SocketWrapper
	{
#if SILVERLIGHT
		public Socket(string hostName, int port)
		{
		}
	}
#else
		public Socket(string hostName, int port)
		{
		    NativeSocket socket = new NativeSocket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
			socket.Connect(new IPEndPoint(Resolve(hostName), port));
			Initialize(socket);
			_toString = StringRepresentation();
		}

	    private static IPAddress Resolve(string hostName) {
	        try {
                var adr = IPAddress.Parse (hostName);
	            return adr;
	        } catch (Exception e) { }

	        IPHostEntry found = Dns.GetHostEntry(hostName);
	        foreach (IPAddress address in found.AddressList)
	        {
                if (address.AddressFamily == AddressFamily.InterNetwork)
                {
                    return address;
                }
	        }
	        throw new IOException("couldn't find suitable address for name '" + hostName + "'");
	    }

	    public Socket(NativeSocket socket)
		{
			Initialize(socket);
		}

		public IInputStream GetInputStream()
		{
			return _in;
		}

		public IOutputStream GetOutputStream()
		{
			return _out;
		}

		public int GetPort() 
		{
			return ((IPEndPoint) _delegate.RemoteEndPoint).Port;
		}

		override protected void Initialize(NativeSocket socket)
		{
			base.Initialize(socket);

			NetworkStream stream = new NetworkStream(_delegate);

#if CF
			_in = new SocketInputStream(this);
#else
			_in = new InputStream(stream);
#endif
			_out = new OutputStream(stream);
		}

		public override string ToString()
		{
			return _toString;
		}

		private string StringRepresentation()
		{
			return ((IPEndPoint)_delegate.LocalEndPoint).Port + " => "+ UnderlyingSocket.RemoteEndPoint;
		}

		private IInputStream _in;
		private IOutputStream _out;
		private readonly string _toString;
	}
#if CF
	internal class SocketInputStream : IInputStream
    {
    	private readonly Socket _socket;

    	public SocketInputStream(Socket socket)
        {
    		_socket = socket;
        }

    	public int Read()
    	{
			byte[] buffer = new byte[1];
    		if (1 != Read(buffer))
    		{
    			return -1;
    		}
    		return (int) buffer[0];
    	}

    	public int Read(byte[] bytes)
    	{
    		return Read(bytes, 0, bytes.Length);
    	}

    	public int Read(byte[] bytes, int offset, int length)
    	{
			try
			{
				if (_socket.SoTimeout > 0)
				{
					if (!UnderlyingSocket.Poll(_socket.SoTimeout*1000, SelectMode.SelectRead))
					{
						throw new IOException("read timeout");
					}
				}
				return InputStream.TranslateReadReturnValue(
					UnderlyingSocket.Receive(bytes, offset, length, SocketFlags.None));
			}
			catch (ObjectDisposedException x)
			{
				throw new IOException(x.Message, x);
			}
			catch (SocketException x)
			{
				throw new IOException(x.Message, x);
			}
    	}

    	public void Close()
    	{
    		// nothing to do
    	}

    	private System.Net.Sockets.Socket UnderlyingSocket
    	{
			get { return _socket.UnderlyingSocket;  }
    	}
	}
#endif // CF
#endif // SILVERLIGHT
}
