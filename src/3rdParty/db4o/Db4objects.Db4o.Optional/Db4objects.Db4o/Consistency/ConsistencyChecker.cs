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
using System.Collections;
using System.Text;
using Db4objects.Db4o;
using Db4objects.Db4o.Consistency;
using Db4objects.Db4o.Ext;
using Db4objects.Db4o.Foundation;
using Db4objects.Db4o.Internal;
using Db4objects.Db4o.Internal.Btree;
using Db4objects.Db4o.Internal.Classindex;
using Db4objects.Db4o.Internal.Ids;
using Db4objects.Db4o.Internal.Slots;

namespace Db4objects.Db4o.Consistency
{
	public class ConsistencyChecker
	{
		private readonly LocalObjectContainer _db;

		private readonly IList bogusSlots = new ArrayList();

		private TreeIntObject mappings;

		public class SlotSource
		{
			public static readonly ConsistencyChecker.SlotSource IdSystem = new ConsistencyChecker.SlotSource
				("IdSystem");

			public static readonly ConsistencyChecker.SlotSource Freespace = new ConsistencyChecker.SlotSource
				("Freespace");

			private readonly string _name;

			private SlotSource(string name)
			{
				_name = name;
			}

			public override string ToString()
			{
				return _name;
			}
		}

		public class SlotWithSource
		{
			public readonly Slot slot;

			public readonly ConsistencyChecker.SlotSource source;

			public SlotWithSource(Slot slot, ConsistencyChecker.SlotSource source)
			{
				this.slot = slot;
				this.source = source;
			}

			public override string ToString()
			{
				return slot + "(" + source + ")";
			}
		}

		public class ConsistencyReport
		{
			private const int MaxReportedItems = 50;

			internal readonly IList bogusSlots;

			internal readonly IList overlaps;

			internal readonly IList invalidObjectIds;

			internal readonly IList invalidFieldIndexEntries;

			public ConsistencyReport(IList bogusSlots, IList overlaps, IList invalidClassIds, 
				IList invalidFieldIndexEntries)
			{
				this.bogusSlots = bogusSlots;
				this.overlaps = overlaps;
				this.invalidObjectIds = invalidClassIds;
				this.invalidFieldIndexEntries = invalidFieldIndexEntries;
			}

			public virtual bool Consistent()
			{
				return bogusSlots.Count == 0 && overlaps.Count == 0 && invalidObjectIds.Count == 
					0 && invalidFieldIndexEntries.Count == 0;
			}

			public override string ToString()
			{
				if (Consistent())
				{
					return "no inconsistencies detected";
				}
				StringBuilder message = new StringBuilder("INCONSISTENCIES DETECTED\n").Append(overlaps
					.Count + " overlaps\n").Append(bogusSlots.Count + " bogus slots\n").Append(invalidObjectIds
					.Count + " invalid class ids\n").Append(invalidFieldIndexEntries.Count + " invalid field index entries\n"
					);
				message.Append("(slot lengths are non-blocked)\n");
				AppendInconsistencyReport(message, "OVERLAPS", overlaps);
				AppendInconsistencyReport(message, "BOGUS SLOTS", bogusSlots);
				AppendInconsistencyReport(message, "INVALID OBJECT IDS", invalidObjectIds);
				AppendInconsistencyReport(message, "INVALID FIELD INDEX ENTRIES", invalidFieldIndexEntries
					);
				return message.ToString();
			}

			private void AppendInconsistencyReport(StringBuilder str, string title, IList entries
				)
			{
				if (entries.Count != 0)
				{
					str.Append(title + "\n");
					int count = 0;
					for (IEnumerator entryIter = entries.GetEnumerator(); entryIter.MoveNext(); )
					{
						object entry = entryIter.Current;
						str.Append(entry).Append("\n");
						count++;
						if (count > MaxReportedItems)
						{
							str.Append("and more...\n");
							break;
						}
					}
				}
			}
		}

		public static void Main(string[] args)
		{
			IEmbeddedObjectContainer db = Db4oEmbedded.OpenFile(args[0]);
			try
			{
				Sharpen.Runtime.Out.WriteLine(new ConsistencyChecker(db).CheckSlotConsistency());
			}
			finally
			{
				db.Close();
			}
		}

		public ConsistencyChecker(IObjectContainer db)
		{
			_db = (LocalObjectContainer)db;
		}

		public virtual ConsistencyChecker.ConsistencyReport CheckSlotConsistency()
		{
			MapIdSystem();
			MapFreespace();
			return new ConsistencyChecker.ConsistencyReport(bogusSlots, CollectOverlaps(), CheckClassIndices
				(), CheckFieldIndices());
		}

		private IList CheckClassIndices()
		{
			IList invalidIds = new ArrayList();
			IIdSystem idSystem = _db.IdSystem();
			if (!(idSystem is BTreeIdSystem))
			{
				return invalidIds;
			}
			ClassMetadataIterator clazzIter = _db.ClassCollection().Iterator();
			while (clazzIter.MoveNext())
			{
				ClassMetadata clazz = clazzIter.CurrentClass();
				if (!clazz.HasClassIndex())
				{
					continue;
				}
				BTreeClassIndexStrategy index = (BTreeClassIndexStrategy)clazz.Index();
				index.TraverseAll(_db.SystemTransaction(), new _IVisitor4_143(this, invalidIds, clazz
					));
			}
			return invalidIds;
		}

		private sealed class _IVisitor4_143 : IVisitor4
		{
			public _IVisitor4_143(ConsistencyChecker _enclosing, IList invalidIds, ClassMetadata
				 clazz)
			{
				this._enclosing = _enclosing;
				this.invalidIds = invalidIds;
				this.clazz = clazz;
			}

			public void Visit(object id)
			{
				if (!this._enclosing.IdIsValid((((int)id))))
				{
					invalidIds.Add(new Pair(clazz.GetName(), ((int)id)));
				}
			}

			private readonly ConsistencyChecker _enclosing;

			private readonly IList invalidIds;

			private readonly ClassMetadata clazz;
		}

		private IList CheckFieldIndices()
		{
			IList invalidIds = new ArrayList();
			ClassMetadataIterator clazzIter = _db.ClassCollection().Iterator();
			while (clazzIter.MoveNext())
			{
				ClassMetadata clazz = clazzIter.CurrentClass();
				clazz.TraverseDeclaredFields(new _IProcedure4_159(this, invalidIds, clazz));
			}
			return invalidIds;
		}

		private sealed class _IProcedure4_159 : IProcedure4
		{
			public _IProcedure4_159(ConsistencyChecker _enclosing, IList invalidIds, ClassMetadata
				 clazz)
			{
				this._enclosing = _enclosing;
				this.invalidIds = invalidIds;
				this.clazz = clazz;
			}

			public void Apply(object field)
			{
				if (!((FieldMetadata)field).HasIndex())
				{
					return;
				}
				BTree fieldIndex = ((FieldMetadata)field).GetIndex(this._enclosing._db.SystemTransaction
					());
				fieldIndex.TraverseKeys(this._enclosing._db.SystemTransaction(), new _IVisitor4_165
					(this, invalidIds, clazz, field));
			}

			private sealed class _IVisitor4_165 : IVisitor4
			{
				public _IVisitor4_165(_IProcedure4_159 _enclosing, IList invalidIds, ClassMetadata
					 clazz, object field)
				{
					this._enclosing = _enclosing;
					this.invalidIds = invalidIds;
					this.clazz = clazz;
					this.field = field;
				}

				public void Visit(object fieldIndexKey)
				{
					int parentID = ((IFieldIndexKey)fieldIndexKey).ParentID();
					if (!this._enclosing._enclosing.IdIsValid(parentID))
					{
						invalidIds.Add(new Pair(clazz.GetName() + "#" + ((FieldMetadata)field).GetName(), 
							parentID));
					}
				}

				private readonly _IProcedure4_159 _enclosing;

				private readonly IList invalidIds;

				private readonly ClassMetadata clazz;

				private readonly object field;
			}

			private readonly ConsistencyChecker _enclosing;

			private readonly IList invalidIds;

			private readonly ClassMetadata clazz;
		}

		private bool IdIsValid(int id)
		{
			try
			{
				return !Slot.IsNull(_db.IdSystem().CommittedSlot(id));
			}
			catch (InvalidIDException)
			{
				return false;
			}
		}

		private IList CollectOverlaps()
		{
			IBlockConverter blockConverter = _db.BlockConverter();
			IList overlaps = new ArrayList();
			ByRef prevSlot = ByRef.NewInstance();
			mappings.Traverse(new _IVisitor4_192(prevSlot, blockConverter, overlaps));
			return overlaps;
		}

		private sealed class _IVisitor4_192 : IVisitor4
		{
			public _IVisitor4_192(ByRef prevSlot, IBlockConverter blockConverter, IList overlaps
				)
			{
				this.prevSlot = prevSlot;
				this.blockConverter = blockConverter;
				this.overlaps = overlaps;
			}

			public void Visit(object obj)
			{
				ConsistencyChecker.SlotWithSource curSlot = (ConsistencyChecker.SlotWithSource)((
					TreeIntObject)obj)._object;
				if (((ConsistencyChecker.SlotWithSource)prevSlot.value) != null)
				{
					if (((ConsistencyChecker.SlotWithSource)prevSlot.value).slot.Address() + blockConverter
						.ToBlockedLength(((ConsistencyChecker.SlotWithSource)prevSlot.value).slot).Length
						() > curSlot.slot.Address())
					{
						overlaps.Add(new Pair(((ConsistencyChecker.SlotWithSource)prevSlot.value), curSlot
							));
					}
				}
				prevSlot.value = curSlot;
			}

			private readonly ByRef prevSlot;

			private readonly IBlockConverter blockConverter;

			private readonly IList overlaps;
		}

		private void MapFreespace()
		{
			_db.FreespaceManager().Traverse(new _IVisitor4_207(this));
		}

		private sealed class _IVisitor4_207 : IVisitor4
		{
			public _IVisitor4_207(ConsistencyChecker _enclosing)
			{
				this._enclosing = _enclosing;
			}

			public void Visit(object slot)
			{
				if (((Slot)slot).Address() < 0)
				{
					this._enclosing.bogusSlots.Add(new ConsistencyChecker.SlotWithSource(((Slot)slot)
						, ConsistencyChecker.SlotSource.Freespace));
				}
				this._enclosing.AddMapping(((Slot)slot), ConsistencyChecker.SlotSource.Freespace);
			}

			private readonly ConsistencyChecker _enclosing;
		}

		private void MapIdSystem()
		{
			IIdSystem idSystem = _db.IdSystem();
			if (idSystem is BTreeIdSystem)
			{
				((BTreeIdSystem)idSystem).TraverseIds(new _IVisitor4_220(this));
			}
		}

		private sealed class _IVisitor4_220 : IVisitor4
		{
			public _IVisitor4_220(ConsistencyChecker _enclosing)
			{
				this._enclosing = _enclosing;
			}

			public void Visit(object mapping)
			{
				if (((IdSlotMapping)mapping)._address < 0)
				{
					this._enclosing.bogusSlots.Add(new ConsistencyChecker.SlotWithSource(((IdSlotMapping
						)mapping).Slot(), ConsistencyChecker.SlotSource.IdSystem));
				}
				if (((IdSlotMapping)mapping)._address > 0)
				{
					this._enclosing.AddMapping(((IdSlotMapping)mapping).Slot(), ConsistencyChecker.SlotSource
						.IdSystem);
				}
			}

			private readonly ConsistencyChecker _enclosing;
		}

		private void AddMapping(Slot slot, ConsistencyChecker.SlotSource source)
		{
			mappings = ((TreeIntObject)Tree.Add(mappings, new ConsistencyChecker.MappingTree(
				slot, source)));
		}

		private class MappingTree : TreeIntObject
		{
			public MappingTree(Slot slot, ConsistencyChecker.SlotSource source) : base(slot.Address
				(), new ConsistencyChecker.SlotWithSource(slot, source))
			{
			}

			public override bool Duplicates()
			{
				return true;
			}
		}
	}
}
