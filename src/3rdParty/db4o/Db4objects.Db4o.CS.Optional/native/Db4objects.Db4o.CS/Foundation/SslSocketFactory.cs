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

using System.Net.Security;
using System.Security.Cryptography.X509Certificates;

namespace Db4objects.Db4o.CS.Foundation
{
	public class SslSocketFactory : ISocket4Factory
	{
		public SslSocketFactory(ISocket4Factory delegating, X509Certificate2 certificate)
		{
			_certificate = certificate;
			_delegating = delegating;
		}

		public SslSocketFactory(ISocket4Factory delegating, RemoteCertificateValidationCallback validationCallback)
		{
			_delegating = delegating;
			_validationCallback = validationCallback;
		}

		public ISocket4 CreateSocket(string hostName, int port)
		{
			ISocket4 clientSocket = _delegating.CreateSocket(hostName, port);
			return new SslSocket(clientSocket, hostName, _validationCallback);
		}

		public IServerSocket4 CreateServerSocket(int port)
		{
			IServerSocket4 serverSocket = _delegating.CreateServerSocket(port);
			return new ServerSslSocket(serverSocket, _certificate);
		}

		private readonly ISocket4Factory _delegating;
		private readonly X509Certificate2 _certificate;
		private readonly RemoteCertificateValidationCallback _validationCallback;
	}
}

#endif