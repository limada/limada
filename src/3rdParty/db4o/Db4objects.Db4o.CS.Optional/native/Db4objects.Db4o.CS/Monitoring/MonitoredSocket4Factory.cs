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

using Db4objects.Db4o.CS.Foundation;

namespace Db4objects.Db4o.CS.Monitoring
{
    public class MonitoredSocket4Factory : ISocket4Factory
    {
        private readonly ISocket4Factory _socketFactory;

        public MonitoredSocket4Factory(ISocket4Factory socketFactory)
        {
            _socketFactory = socketFactory;
        }

        public ISocket4 CreateSocket(string hostName, int port)
        {
            return new MonitoredClientSocket4(_socketFactory.CreateSocket(hostName, port));
        }

        public IServerSocket4 CreateServerSocket(int port)
        {
            return new MonitoredServerSocket4(_socketFactory.CreateServerSocket(port));
        }
    }
}
#endif