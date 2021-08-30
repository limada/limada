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
#if !CF && !SILVERLIGHT

using System;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;

namespace Db4objects.Db4o.CS.Foundation
{
	internal class SslSocket : ISocket4
	{
		private SslSocket(ISocket4 socket)
		{
			_socket = socket;
		}

		public SslSocket(ISocket4 socket, string hostName, RemoteCertificateValidationCallback certificateValidationCallback) : this(socket)
		{
			_sslStream = new SslStream(new Socket4Stream(socket), false, certificateValidationCallback);
			_sslStream.AuthenticateAsClient(hostName);
		}

		public SslSocket(ISocket4 socket, X509Certificate2 certificate) : this(socket)
		{
			_sslStream = new SslStream(new Socket4Stream(socket), false);
			_sslStream.AuthenticateAsServer(certificate);
		}

		public void Close()
		{
			_sslStream.Close();
		}

		public void Flush()
		{
			_sslStream.Flush();
		}

		public void SetSoTimeout(int timeout)
		{
			_socket.SetSoTimeout(timeout);
		}

		public bool IsConnected()
		{
			return _socket.IsConnected();
		}

		public int Read(byte[] buffer, int offset, int count)
		{
			return _sslStream.Read(buffer, offset, count);
		}

		public void Write(byte[] bytes, int offset, int count)
		{
			_sslStream.Write(bytes, offset, count);
		}

		public ISocket4 OpenParallelSocket()
		{
			throw new NotImplementedException();
		}

		private readonly SslStream _sslStream;
		private readonly ISocket4 _socket;
	}
}

#endif