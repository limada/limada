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
using System.Collections;
using Db4objects.Db4o.Events;
using Db4objects.Db4o.Ext;
using Db4objects.Db4o.Foundation;
using Db4objects.Db4o.Internal;
using Db4objects.Db4o.Internal.Btree;
using Db4objects.Db4o.Internal.Handlers;
using Db4objects.Db4o.Marshall;

namespace Db4objects.Db4o.Internal
{
	public class CommitTimestampSupport
	{
		private BTree _idToTimestamp;

		private BTree _timestampToId;

		private readonly LocalObjectContainer _container;

		public CommitTimestampSupport(LocalObjectContainer container)
		{
			_container = container;
		}

		public virtual void EnsureInitialized()
		{
			IdToTimestamp();
		}

		public virtual BTree IdToTimestamp()
		{
			if (_idToTimestamp != null)
			{
				return _idToTimestamp;
			}
			if (!_container.Config().GenerateCommitTimestamps().DefiniteYes())
			{
				return null;
			}
			Initialize();
			return _idToTimestamp;
		}

		public virtual BTree TimestampToId()
		{
			if (_timestampToId != null)
			{
				return _timestampToId;
			}
			if (!_container.Config().GenerateCommitTimestamps().DefiniteYes())
			{
				return null;
			}
			Initialize();
			return _timestampToId;
		}

		private void Initialize()
		{
			int idToTimestampIndexId = _container.SystemData().IdToTimestampIndexId();
			int timestampToIdIndexId = _container.SystemData().TimestampToIdIndexId();
			_idToTimestamp = new BTree(_container.SystemTransaction(), idToTimestampIndexId, 
				new CommitTimestampSupport.TimestampEntryById());
			_timestampToId = new BTree(_container.SystemTransaction(), timestampToIdIndexId, 
				new CommitTimestampSupport.IdEntryByTimestamp());
			if (idToTimestampIndexId != _idToTimestamp.GetID())
			{
				StoreBtreesIds();
			}
			EventRegistryFactory.ForObjectContainer(_container).Committing += new System.EventHandler<Db4objects.Db4o.Events.CommitEventArgs>
				(new _IEventListener4_69(this).OnEvent);
		}

		private sealed class _IEventListener4_69
		{
			public _IEventListener4_69(CommitTimestampSupport _enclosing)
			{
				this._enclosing = _enclosing;
			}

			public void OnEvent(object sender, Db4objects.Db4o.Events.CommitEventArgs args)
			{
				LocalTransaction trans = (LocalTransaction)((CommitEventArgs)args).Transaction();
				long transactionTimestamp = trans.Timestamp();
				long commitTimestamp = (transactionTimestamp > 0) ? transactionTimestamp : this._enclosing
					._container.GenerateTimeStampId();
				Transaction sysTrans = trans.SystemTransaction();
				this.AddTimestamp(sysTrans, ((CommitEventArgs)args).Added.GetEnumerator(), commitTimestamp
					);
				this.AddTimestamp(sysTrans, ((CommitEventArgs)args).Updated.GetEnumerator(), commitTimestamp
					);
				this.AddTimestamp(sysTrans, ((CommitEventArgs)args).Deleted.GetEnumerator(), 0);
			}

			private void AddTimestamp(Transaction trans, IEnumerator it, long commitTimestamp
				)
			{
				while (it.MoveNext())
				{
					IObjectInfo objInfo = (IObjectInfo)it.Current;
					CommitTimestampSupport.TimestampEntry te = new CommitTimestampSupport.TimestampEntry
						((int)objInfo.GetInternalID(), commitTimestamp);
					CommitTimestampSupport.TimestampEntry oldEntry = (CommitTimestampSupport.TimestampEntry
						)this._enclosing._idToTimestamp.Remove(trans, te);
					if (oldEntry != null)
					{
						this._enclosing._timestampToId.Remove(trans, oldEntry);
					}
					if (commitTimestamp != 0)
					{
						this._enclosing._idToTimestamp.Add(trans, te);
						this._enclosing._timestampToId.Add(trans, te);
					}
				}
			}

			private readonly CommitTimestampSupport _enclosing;
		}

		private void StoreBtreesIds()
		{
			_container.SystemData().IdToTimestampIndexId(_idToTimestamp.GetID());
			_container.SystemData().TimestampToIdIndexId(_timestampToId.GetID());
			_container.GetFileHeader().WriteVariablePart(_container);
		}

		public class TimestampEntry : IFieldIndexKey
		{
			public readonly int objectId;

			public readonly long commitTimestamp;

			public override string ToString()
			{
				return "TimestampEntry [objectId=" + objectId + ", commitTimestamp=" + commitTimestamp
					 + "]";
			}

			public TimestampEntry(int objectId, long commitTimestamp)
			{
				this.objectId = objectId;
				this.commitTimestamp = commitTimestamp;
			}

			public virtual int ParentID()
			{
				return objectId;
			}

			public virtual long GetCommitTimestamp()
			{
				return commitTimestamp;
			}

			public virtual object Value()
			{
				return commitTimestamp;
			}
		}

		private class TimestampEntryById : IIndexable4
		{
			public virtual IPreparedComparison PrepareComparison(IContext context, object first
				)
			{
				return new _IPreparedComparison_139(first);
			}

			private sealed class _IPreparedComparison_139 : IPreparedComparison
			{
				public _IPreparedComparison_139(object first)
				{
					this.first = first;
				}

				public int CompareTo(object second)
				{
					return IntHandler.Compare(((CommitTimestampSupport.TimestampEntry)first).objectId
						, ((CommitTimestampSupport.TimestampEntry)second).objectId);
				}

				private readonly object first;
			}

			public virtual int LinkLength()
			{
				return Const4.IntLength + Const4.LongLength;
			}

			public virtual object ReadIndexEntry(IContext context, ByteArrayBuffer reader)
			{
				return new CommitTimestampSupport.TimestampEntry(reader.ReadInt(), reader.ReadLong
					());
			}

			public virtual void WriteIndexEntry(IContext context, ByteArrayBuffer writer, object
				 obj)
			{
				writer.WriteInt(((CommitTimestampSupport.TimestampEntry)obj).ParentID());
				writer.WriteLong(((CommitTimestampSupport.TimestampEntry)obj).GetCommitTimestamp(
					));
			}

			public virtual void DefragIndexEntry(DefragmentContextImpl context)
			{
				// we are storing ids in the btree, so the order will change when the ids change
				// to properly defrag the btree we need to readd all the entries
				throw new NotSupportedException();
			}
		}

		private sealed class IdEntryByTimestamp : CommitTimestampSupport.TimestampEntryById
		{
			public override IPreparedComparison PrepareComparison(IContext context, object first
				)
			{
				return new _IPreparedComparison_168(first);
			}

			private sealed class _IPreparedComparison_168 : IPreparedComparison
			{
				public _IPreparedComparison_168(object first)
				{
					this.first = first;
				}

				public int CompareTo(object second)
				{
					int result = LongHandler.Compare(((CommitTimestampSupport.TimestampEntry)first).commitTimestamp
						, ((CommitTimestampSupport.TimestampEntry)second).commitTimestamp);
					if (result != 0)
					{
						return result;
					}
					return IntHandler.Compare(((CommitTimestampSupport.TimestampEntry)first).objectId
						, ((CommitTimestampSupport.TimestampEntry)second).objectId);
				}

				private readonly object first;
			}
		}

		public virtual long VersionForId(int id)
		{
			if (IdToTimestamp() == null || id == 0)
			{
				return 0;
			}
			CommitTimestampSupport.TimestampEntry te = (CommitTimestampSupport.TimestampEntry
				)IdToTimestamp().Search(_container.SystemTransaction(), new CommitTimestampSupport.TimestampEntry
				(id, 0));
			if (te == null)
			{
				return 0;
			}
			return te.GetCommitTimestamp();
		}

		public virtual void Put(Transaction trans, int objectId, long version)
		{
			CommitTimestampSupport.TimestampEntry te = new CommitTimestampSupport.TimestampEntry
				(objectId, version);
			IdToTimestamp().Add(trans, te);
			TimestampToId().Add(trans, te);
		}
	}
}
