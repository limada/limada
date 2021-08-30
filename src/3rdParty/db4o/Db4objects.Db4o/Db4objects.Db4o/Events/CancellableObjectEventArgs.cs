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
using Db4objects.Db4o.Events;
using Db4objects.Db4o.Ext;
using Db4objects.Db4o.Internal;

namespace Db4objects.Db4o.Events
{
	/// <summary>Argument for object related events which can be cancelled.</summary>
	/// <remarks>Argument for object related events which can be cancelled.</remarks>
	/// <seealso cref="IEventRegistry">IEventRegistry</seealso>
	/// <seealso cref="ICancellableEventArgs">ICancellableEventArgs</seealso>
	public class CancellableObjectEventArgs : ObjectInfoEventArgs, ICancellableEventArgs
	{
		private bool _cancelled;

		private object _object;

		/// <summary>Creates a new instance for the specified object.</summary>
		/// <remarks>Creates a new instance for the specified object.</remarks>
		public CancellableObjectEventArgs(Transaction transaction, IObjectInfo objectInfo
			, object obj) : base(transaction, objectInfo)
		{
			_object = obj;
		}

		/// <seealso cref="ICancellableEventArgs.Cancel()">ICancellableEventArgs.Cancel()</seealso>
		public virtual void Cancel()
		{
			_cancelled = true;
		}

		/// <seealso cref="ICancellableEventArgs.IsCancelled()">ICancellableEventArgs.IsCancelled()
		/// 	</seealso>
		public virtual bool IsCancelled
		{
			get
			{
				return _cancelled;
			}
		}

		public override object Object
		{
			get
			{
				return _object;
			}
		}

		public override IObjectInfo Info
		{
			get
			{
				IObjectInfo info = base.Info;
				if (null == info)
				{
					throw new InvalidOperationException();
				}
				return info;
			}
		}
	}
}
