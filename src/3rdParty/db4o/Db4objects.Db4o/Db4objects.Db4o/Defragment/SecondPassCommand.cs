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
using Db4objects.Db4o.Defragment;
using Db4objects.Db4o.Internal;
using Db4objects.Db4o.Internal.Btree;

namespace Db4objects.Db4o.Defragment
{
	/// <summary>
	/// Second step in the defragmenting process: Fills in target file pointer slots, copies
	/// content slots from source to target and triggers ID remapping therein by calling the
	/// appropriate db4o/marshaller defrag() implementations.
	/// </summary>
	/// <remarks>
	/// Second step in the defragmenting process: Fills in target file pointer slots, copies
	/// content slots from source to target and triggers ID remapping therein by calling the
	/// appropriate db4o/marshaller defrag() implementations. During the process, the actual address
	/// mappings for the content slots are registered for use with string indices.
	/// </remarks>
	/// <exclude></exclude>
	internal sealed class SecondPassCommand : IPassCommand
	{
		protected readonly int _objectCommitFrequency;

		protected int _objectCount = 0;

		public SecondPassCommand(int objectCommitFrequency)
		{
			_objectCommitFrequency = objectCommitFrequency;
		}

		/// <exception cref="Db4objects.Db4o.CorruptionException"></exception>
		/// <exception cref="System.IO.IOException"></exception>
		public void ProcessClass(DefragmentServicesImpl services, ClassMetadata classMetadata
			, int id, int classIndexID)
		{
			if (services.MappedID(id, -1) == -1)
			{
				Sharpen.Runtime.Err.WriteLine("MAPPING NOT FOUND: " + id);
			}
			DefragmentContextImpl.ProcessCopy(services, id, new _ISlotCopyHandler_34(classMetadata
				, classIndexID));
		}

		private sealed class _ISlotCopyHandler_34 : ISlotCopyHandler
		{
			public _ISlotCopyHandler_34(ClassMetadata classMetadata, int classIndexID)
			{
				this.classMetadata = classMetadata;
				this.classIndexID = classIndexID;
			}

			public void ProcessCopy(DefragmentContextImpl context)
			{
				classMetadata.DefragClass(context, classIndexID);
			}

			private readonly ClassMetadata classMetadata;

			private readonly int classIndexID;
		}

		/// <exception cref="Db4objects.Db4o.CorruptionException"></exception>
		/// <exception cref="System.IO.IOException"></exception>
		public void ProcessObjectSlot(DefragmentServicesImpl services, ClassMetadata classMetadata
			, int id)
		{
			ByteArrayBuffer sourceBuffer = services.SourceBufferByID(id);
			DefragmentContextImpl.ProcessCopy(services, id, new _ISlotCopyHandler_43(this, services
				), sourceBuffer);
		}

		private sealed class _ISlotCopyHandler_43 : ISlotCopyHandler
		{
			public _ISlotCopyHandler_43(SecondPassCommand _enclosing, DefragmentServicesImpl 
				services)
			{
				this._enclosing = _enclosing;
				this.services = services;
			}

			public void ProcessCopy(DefragmentContextImpl context)
			{
				ClassMetadata.DefragObject(context);
				if (this._enclosing._objectCommitFrequency > 0)
				{
					this._enclosing._objectCount++;
					if (this._enclosing._objectCount == this._enclosing._objectCommitFrequency)
					{
						services.TargetCommit();
						services.Mapping().Commit();
						this._enclosing._objectCount = 0;
					}
				}
			}

			private readonly SecondPassCommand _enclosing;

			private readonly DefragmentServicesImpl services;
		}

		/// <exception cref="Db4objects.Db4o.CorruptionException"></exception>
		/// <exception cref="System.IO.IOException"></exception>
		public void ProcessClassCollection(DefragmentServicesImpl services)
		{
			DefragmentContextImpl.ProcessCopy(services, services.SourceClassCollectionID(), new 
				_ISlotCopyHandler_59(services));
		}

		private sealed class _ISlotCopyHandler_59 : ISlotCopyHandler
		{
			public _ISlotCopyHandler_59(DefragmentServicesImpl services)
			{
				this.services = services;
			}

			public void ProcessCopy(DefragmentContextImpl context)
			{
				int acceptedClasses = 0;
				int numClassesOffset = context.TargetBuffer().Offset();
				acceptedClasses = this.CopyAcceptedClasses(context, acceptedClasses);
				this.WriteIntAt(context.TargetBuffer(), numClassesOffset, acceptedClasses);
			}

			private int CopyAcceptedClasses(DefragmentContextImpl context, int acceptedClasses
				)
			{
				int numClasses = context.ReadInt();
				for (int classIdx = 0; classIdx < numClasses; classIdx++)
				{
					int classId = context.SourceBuffer().ReadInt();
					if (!this.Accept(classId))
					{
						continue;
					}
					++acceptedClasses;
					context.WriteMappedID(classId);
				}
				return acceptedClasses;
			}

			private void WriteIntAt(ByteArrayBuffer target, int offset, int value)
			{
				int currentOffset = target.Offset();
				target.Seek(offset);
				target.WriteInt(value);
				target.Seek(currentOffset);
			}

			private bool Accept(int classId)
			{
				return services.Accept(services.ClassMetadataForId(classId));
			}

			private readonly DefragmentServicesImpl services;
		}

		/// <exception cref="Db4objects.Db4o.CorruptionException"></exception>
		/// <exception cref="System.IO.IOException"></exception>
		public void ProcessBTree(DefragmentServicesImpl context, BTree btree)
		{
			btree.DefragBTree(context);
		}

		public void Flush(DefragmentServicesImpl context)
		{
		}
	}
}
