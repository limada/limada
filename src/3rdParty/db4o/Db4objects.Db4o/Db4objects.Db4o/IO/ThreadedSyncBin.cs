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
using Db4objects.Db4o.Foundation;
using Db4objects.Db4o.IO;
using Sharpen.Lang;

namespace Db4objects.Db4o.IO
{
	public class ThreadedSyncBin : BinDecorator
	{
		private const int OneSecond = 1000;

		private volatile IRunnable _syncRunnable;

		private volatile bool _closed;

		private readonly Thread _thread;

		private readonly Lock4 _lock = new Lock4();

		public ThreadedSyncBin(IBin bin) : base(bin)
		{
			_thread = new Thread(new _IRunnable_23(this), "ThreadedSyncBin");
			_thread.Start();
		}

		private sealed class _IRunnable_23 : IRunnable
		{
			public _IRunnable_23(ThreadedSyncBin _enclosing)
			{
				this._enclosing = _enclosing;
			}

			public void Run()
			{
				IClosure4 closure = new _IClosure4_25(this);
				while (true)
				{
					this._enclosing._lock.Run(closure);
					if (this._enclosing._closed)
					{
						return;
					}
				}
			}

			private sealed class _IClosure4_25 : IClosure4
			{
				public _IClosure4_25(_IRunnable_23 _enclosing)
				{
					this._enclosing = _enclosing;
				}

				public object Run()
				{
					this._enclosing._enclosing.RunSyncRunnable();
					this._enclosing._enclosing._lock.Snooze(Db4objects.Db4o.IO.ThreadedSyncBin.OneSecond
						);
					return null;
				}

				private readonly _IRunnable_23 _enclosing;
			}

			private readonly ThreadedSyncBin _enclosing;
		}

		public override void Close()
		{
			WaitForPendingSync();
			_closed = true;
			_lock.Run(new _IClosure4_46(this));
			base.Close();
		}

		private sealed class _IClosure4_46 : IClosure4
		{
			public _IClosure4_46(ThreadedSyncBin _enclosing)
			{
				this._enclosing = _enclosing;
			}

			public object Run()
			{
				this._enclosing._lock.Awake();
				return null;
			}

			private readonly ThreadedSyncBin _enclosing;
		}

		private void WaitForPendingSync()
		{
			while (_syncRunnable != null)
			{
				if (Thread.CurrentThread() == _thread)
				{
					return;
				}
			}
		}

		public override long Length()
		{
			WaitForPendingSync();
			return base.Length();
		}

		public override int Read(long position, byte[] buffer, int bytesToRead)
		{
			WaitForPendingSync();
			return base.Read(position, buffer, bytesToRead);
		}

		public override void Write(long position, byte[] bytes, int bytesToWrite)
		{
			WaitForPendingSync();
			base.Write(position, bytes, bytesToWrite);
		}

		public override void Sync()
		{
			WaitForPendingSync();
			base.Sync();
		}

		public override void Sync(IRunnable runnable)
		{
			WaitForPendingSync();
			_lock.Run(new _IClosure4_85(this, runnable));
		}

		private sealed class _IClosure4_85 : IClosure4
		{
			public _IClosure4_85(ThreadedSyncBin _enclosing, IRunnable runnable)
			{
				this._enclosing = _enclosing;
				this.runnable = runnable;
			}

			public object Run()
			{
				this._enclosing._syncRunnable = runnable;
				this._enclosing._lock.Awake();
				return null;
			}

			private readonly ThreadedSyncBin _enclosing;

			private readonly IRunnable runnable;
		}

		internal void RunSyncRunnable()
		{
			IRunnable runnable = _syncRunnable;
			if (runnable != null)
			{
				base.Sync();
				runnable.Run();
				base.Sync();
				_syncRunnable = null;
			}
		}
	}
}
