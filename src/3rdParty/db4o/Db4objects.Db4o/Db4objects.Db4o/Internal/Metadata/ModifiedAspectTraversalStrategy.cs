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
using Db4objects.Db4o.Internal;
using Db4objects.Db4o.Internal.Metadata;

namespace Db4objects.Db4o.Internal.Metadata
{
	/// <exclude></exclude>
	public class ModifiedAspectTraversalStrategy : IAspectTraversalStrategy
	{
		private readonly IList _classDiffs;

		public ModifiedAspectTraversalStrategy(ClassMetadata classMetadata, IList ancestors
			)
		{
			_classDiffs = new ArrayList();
			_classDiffs.Add(new HierarchyAnalyzer.Same(classMetadata));
			Sharpen.Collections.AddAll(_classDiffs, ancestors);
		}

		public virtual void TraverseAllAspects(ITraverseAspectCommand command)
		{
			int currentSlot = 0;
			for (IEnumerator diffIter = _classDiffs.GetEnumerator(); diffIter.MoveNext(); )
			{
				HierarchyAnalyzer.Diff diff = ((HierarchyAnalyzer.Diff)diffIter.Current);
				ClassMetadata classMetadata = diff.ClassMetadata();
				if (diff.IsRemoved())
				{
					currentSlot = SkipAspectsOf(classMetadata, command, currentSlot);
					continue;
				}
				currentSlot = TraverseAspectsOf(classMetadata, command, currentSlot);
				if (command.Cancelled())
				{
					return;
				}
			}
		}

		internal interface ITraverseAspectCommandProcessor
		{
			void Process(ITraverseAspectCommand command, ClassAspect currentAspect, int currentSlot
				);
		}

		private int TraverseAspectsOf(ClassMetadata classMetadata, ITraverseAspectCommand
			 command, int currentSlot)
		{
			return ProcessAspectsOf(classMetadata, command, currentSlot, new _ITraverseAspectCommandProcessor_49
				());
		}

		private sealed class _ITraverseAspectCommandProcessor_49 : ModifiedAspectTraversalStrategy.ITraverseAspectCommandProcessor
		{
			public _ITraverseAspectCommandProcessor_49()
			{
			}

			public void Process(ITraverseAspectCommand command, ClassAspect currentAspect, int
				 currentSlot)
			{
				command.ProcessAspect(currentAspect, currentSlot);
			}
		}

		private int ProcessAspectsOf(ClassMetadata classMetadata, ITraverseAspectCommand 
			command, int currentSlot, ModifiedAspectTraversalStrategy.ITraverseAspectCommandProcessor
			 processor)
		{
			int aspectCount = command.DeclaredAspectCount(classMetadata);
			for (int i = 0; i < aspectCount && !command.Cancelled(); i++)
			{
				processor.Process(command, classMetadata._aspects[i], currentSlot);
				currentSlot++;
			}
			return currentSlot;
		}

		private int SkipAspectsOf(ClassMetadata classMetadata, ITraverseAspectCommand command
			, int currentSlot)
		{
			return ProcessAspectsOf(classMetadata, command, currentSlot, new _ITraverseAspectCommandProcessor_70
				());
		}

		private sealed class _ITraverseAspectCommandProcessor_70 : ModifiedAspectTraversalStrategy.ITraverseAspectCommandProcessor
		{
			public _ITraverseAspectCommandProcessor_70()
			{
			}

			public void Process(ITraverseAspectCommand command, ClassAspect currentAspect, int
				 currentSlot)
			{
				command.ProcessAspectOnMissingClass(currentAspect, currentSlot);
			}
		}
	}
}
