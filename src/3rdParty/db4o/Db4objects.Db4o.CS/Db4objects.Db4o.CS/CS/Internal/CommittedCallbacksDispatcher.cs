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
using Db4objects.Db4o.Foundation;
using Sharpen.Lang;

namespace Db4objects.Db4o.CS.Internal
{
	public class CommittedCallbacksDispatcher : IRunnable
	{
		private bool _stopped;

		private readonly BlockingQueue _committedInfosQueue;

		private readonly ObjectServerImpl _server;

		public CommittedCallbacksDispatcher(ObjectServerImpl server, BlockingQueue committedInfosQueue
			)
		{
			_server = server;
			_committedInfosQueue = committedInfosQueue;
		}

		public virtual void Run()
		{
			SetThreadName();
			MessageLoop();
		}

		private void MessageLoop()
		{
			while (!_stopped)
			{
				MCommittedInfo committedInfos;
				try
				{
					committedInfos = (MCommittedInfo)_committedInfosQueue.Next();
				}
				catch (BlockingQueueStoppedException)
				{
					break;
				}
				_server.BroadcastMsg(committedInfos, new _IBroadcastFilter_33());
			}
		}

		private sealed class _IBroadcastFilter_33 : IBroadcastFilter
		{
			public _IBroadcastFilter_33()
			{
			}

			public bool Accept(IServerMessageDispatcher dispatcher)
			{
				return dispatcher.CaresAboutCommitted();
			}
		}

		private void SetThreadName()
		{
			Thread.CurrentThread().SetName("committed callback thread");
		}

		public virtual void Stop()
		{
			_committedInfosQueue.Stop();
			_stopped = true;
		}
	}
}
