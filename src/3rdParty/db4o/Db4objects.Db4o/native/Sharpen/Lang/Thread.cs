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
using System.Threading;

namespace Sharpen.Lang
{
	public class Thread : IRunnable
	{
		private IRunnable _target;

		private string _name;

		private System.Threading.Thread _thread;

		private bool _isDaemon;

		public Thread()
		{
			_target = this;
		}

		public Thread(IRunnable target, string name)
		{
			_target = target;
			SetName(name);
		}

		public Thread(IRunnable target)
		{
			_target = target;
		}

		public Thread(System.Threading.Thread thread)
		{
			_thread = thread;
		}

		public static Thread CurrentThread()
		{
			return new Thread(System.Threading.Thread.CurrentThread);
		}

		public virtual void Run()
		{
		}

		public void SetName(string name)
		{
			_name = name;
#if !CF
			if (_thread != null && name != null)
			{
				try
				{
					_thread.Name = _name;
				}
				catch
				{
				}
			}
#endif
		}

		public string GetName()
		{
#if !CF
			return _thread != null ? _thread.Name : _name;
#else
			return "";
#endif
		}

		public static void Sleep(long milliseconds)
		{
			System.Threading.Thread.Sleep((int)milliseconds);
		}

		public void Start()
		{
			_thread = new System.Threading.Thread(EntryPoint);
			_thread.IsBackground = _isDaemon;
			if (_name != null)
			{
				SetName(_name);
			}
			_thread.Start();
		}

		public void Join() 
		{
			if (_thread == null)
				return;
			_thread.Join();
		}

		public void Join(int millisecondsTimeout)
		{
			if (_thread == null)
				return;
			_thread.Join(millisecondsTimeout);
		}
		
		public void SetDaemon(bool isDaemon)
		{
			_isDaemon = isDaemon;
		}

		public override bool Equals(object obj)
		{
			Thread other = (obj as Thread);
			if (other == null)
				return false;
			if (other == this)
				return true;
			if (_thread == null)
				return false;
			return _thread == other._thread;
		}

		public override int GetHashCode()
		{
			return _thread == null ? 37 : _thread.GetHashCode();
		}

		private void EntryPoint()
		{
			try
			{
				_target.Run();
			}
			catch (Exception e)
			{
				// don't let an unhandled exception bring
				// the process down
				Runtime.PrintStackTrace(e);
			}
		}

		public bool IsDaemon()
		{
			return _thread != null
				? _thread.IsBackground
				: _isDaemon;
		}
	}
}
