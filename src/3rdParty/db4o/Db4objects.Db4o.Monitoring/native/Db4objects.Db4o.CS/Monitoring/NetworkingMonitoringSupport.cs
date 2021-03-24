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

using Db4objects.Db4o.Config;
using Db4objects.Db4o.CS.Config;
using Db4objects.Db4o.CS.Foundation;
using Db4objects.Db4o.CS.Internal.Config;
using Db4objects.Db4o.Internal;

namespace Db4objects.Db4o.CS.Monitoring
{
    public class NetworkingMonitoringSupport:IConfigurationItem
    {
        public void Prepare(IConfiguration configuration)
        {
            INetworkingConfiguration networkConfig = Db4oClientServerLegacyConfigurationBridge.AsNetworkingConfiguration(configuration);
            ISocket4Factory currentSocketFactory = networkConfig.SocketFactory;
            networkConfig.SocketFactory = new MonitoredSocket4Factory(currentSocketFactory);
        }

        public void Apply(IInternalObjectContainer container)
        {

        }
    }
}
#endif