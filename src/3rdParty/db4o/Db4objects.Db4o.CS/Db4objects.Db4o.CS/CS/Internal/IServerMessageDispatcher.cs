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
using Db4objects.Db4o.CS.Internal;
using Db4objects.Db4o.CS.Internal.Messages;
using Db4objects.Db4o.Internal;

namespace Db4objects.Db4o.CS.Internal
{
	/// <exclude></exclude>
	public interface IServerMessageDispatcher : IClientConnection, IMessageDispatcher
		, ICommittedCallbackDispatcher
	{
		void QueryResultFinalized(int queryResultID);

		Socket4Adapter Socket();

		int DispatcherID();

		LazyClientObjectSetStub QueryResultForID(int queryResultID);

		void SwitchToMainFile();

		void SwitchToFile(MSwitchToFile file);

		void UseTransaction(MUseTransaction transaction);

		void MapQueryResultToID(LazyClientObjectSetStub stub, int queryResultId);

		ObjectServerImpl Server();

		void Login();

		bool Close();

		bool Close(ShutdownMode mode);

		void CloseConnection();

		void CaresAboutCommitted(bool care);

		bool CaresAboutCommitted();

		bool Write(Msg msg);

		CallbackObjectInfoCollections CommittedInfo();

		Db4objects.Db4o.CS.Internal.ClassInfoHelper ClassInfoHelper();

		bool ProcessMessage(Msg message);

		/// <exception cref="System.Exception"></exception>
		void Join();

		void SetDispatcherName(string name);
	}
}
