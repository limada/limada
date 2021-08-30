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

using System.Security.Cryptography.X509Certificates;

namespace Db4objects.Db4o.CS.Foundation
{
	internal class ServerSslSocket : ServerSocket4Decorator
	{
		public ServerSslSocket(IServerSocket4 socket, X509Certificate2 certificate) : base(socket)
		{
			_certificate = certificate;
		}

		public override ISocket4 Accept()
		{
			ISocket4 socket = base.Accept();
			return new SslSocket(socket, _certificate);
		}
		
		private readonly X509Certificate2 _certificate;
	}
}

#endif