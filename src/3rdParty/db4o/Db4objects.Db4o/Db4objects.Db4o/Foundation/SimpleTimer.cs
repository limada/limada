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
using Sharpen.Lang;

namespace Db4objects.Db4o.Foundation
{
	/// <exclude></exclude>
	public sealed class SimpleTimer : IRunnable
	{
		private readonly IRunnable _runnable;

		private readonly long _interval;

		private Lock4 _lock;

		public volatile bool stopped = false;

		public SimpleTimer(IRunnable runnable, long interval)
		{
			_runnable = runnable;
			_interval = interval;
			_lock = new Lock4();
		}

		public void Stop()
		{
			stopped = true;
			_lock.Run(new _IClosure4_27(this));
		}

		private sealed class _IClosure4_27 : IClosure4
		{
			public _IClosure4_27(SimpleTimer _enclosing)
			{
				this._enclosing = _enclosing;
			}

			public object Run()
			{
				this._enclosing._lock.Awake();
				return null;
			}

			private readonly SimpleTimer _enclosing;
		}

		public void Run()
		{
			while (!stopped)
			{
				_lock.Run(new _IClosure4_37(this));
				if (!stopped)
				{
					_runnable.Run();
				}
			}
		}

		private sealed class _IClosure4_37 : IClosure4
		{
			public _IClosure4_37(SimpleTimer _enclosing)
			{
				this._enclosing = _enclosing;
			}

			public object Run()
			{
				this._enclosing._lock.Snooze(this._enclosing._interval);
				return null;
			}

			private readonly SimpleTimer _enclosing;
		}
	}
}
