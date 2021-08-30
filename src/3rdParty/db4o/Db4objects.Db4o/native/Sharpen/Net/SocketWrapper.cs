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
using System.Net.Sockets;
using NativeSocket=System.Net.Sockets.Socket;

namespace Sharpen.Net
{
	public class SocketWrapper
	{
		protected NativeSocket _delegate;

#if CF || SILVERLIGHT
	    private int _soTimeout = 0;

        public int SoTimeout
        {
            get { return _soTimeout; }
        }
#endif

		public NativeSocket UnderlyingSocket
		{
			get { return _delegate;  }
		}

	    protected virtual void Initialize(NativeSocket socket)
		{
			_delegate = socket;
		}

		public void SetSoTimeout(int timeout)
		{
#if !CF && !SILVERLIGHT
			_delegate.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReceiveTimeout, timeout);
			_delegate.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.SendTimeout, timeout);
#else
			_soTimeout = timeout;
#endif
		}

		public void Close()
		{
			if (_delegate.Connected)
			{
				try
				{
					_delegate.Shutdown(SocketShutdown.Both);
				}
				catch (Exception)
				{	
				}
			}
			_delegate.Close();
		}

        public bool IsConnected() 
        {
            return _delegate.Connected;
        }
	}
}
