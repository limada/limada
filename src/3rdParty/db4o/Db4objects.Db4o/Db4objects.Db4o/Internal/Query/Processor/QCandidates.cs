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
using System.Text;
using Db4objects.Db4o;
using Db4objects.Db4o.Foundation;
using Db4objects.Db4o.Internal;
using Db4objects.Db4o.Internal.Classindex;
using Db4objects.Db4o.Internal.Diagnostic;
using Db4objects.Db4o.Internal.Fieldindex;
using Db4objects.Db4o.Internal.Handlers;
using Db4objects.Db4o.Internal.Marshall;
using Db4objects.Db4o.Internal.Query.Processor;
using Db4objects.Db4o.Typehandlers;

namespace Db4objects.Db4o.Internal.Query.Processor
{
	/// <summary>
	/// Holds the tree of
	/// <see cref="QCandidate">QCandidate</see>
	/// objects and the list of
	/// <see cref="QCon">QCon</see>
	/// during query evaluation.
	/// The query work (adding and removing nodes) happens here.
	/// Candidates during query evaluation.
	/// <see cref="QCandidate">QCandidate</see>
	/// objects are stored in i_root
	/// </summary>
	/// <exclude></exclude>
	public sealed class QCandidates : IVisitor4
	{
		public readonly LocalTransaction i_trans;

		public Tree i_root;

		private List4 _constraints;

		internal ClassMetadata i_classMetadata;

		private QField _field;

		internal QCon i_currentConstraint;

		private IDGenerator _idGenerator;

		private bool _loadedFromClassIndex;

		internal QCandidates(LocalTransaction a_trans, ClassMetadata a_classMetadata, QField
			 a_field)
		{
			// Transaction necessary as reference to stream
			// root of the QCandidate tree
			// collection of all constraints
			// possible class information
			// possible field information
			// current executing constraint, only set where needed
			i_trans = a_trans;
			i_classMetadata = a_classMetadata;
			_field = a_field;
			if (a_field == null || a_field._fieldMetadata == null || !(a_field._fieldMetadata
				.GetHandler() is StandardReferenceTypeHandler))
			{
				return;
			}
			ClassMetadata yc = ((StandardReferenceTypeHandler)a_field._fieldMetadata.GetHandler
				()).ClassMetadata();
			if (i_classMetadata == null)
			{
				i_classMetadata = yc;
			}
			else
			{
				yc = i_classMetadata.GetHigherOrCommonHierarchy(yc);
				if (yc != null)
				{
					i_classMetadata = yc;
				}
			}
		}

		public QCandidate Add(QCandidate candidate)
		{
			i_root = Tree.Add(i_root, candidate);
			if (candidate._size == 0)
			{
				// This means that the candidate was already present
				// and QCandidate does not allow duplicates.
				// In this case QCandidate#isDuplicateOf will have
				// placed the existing QCandidate in the i_root
				// variable of the new candidate. We return it here: 
				return candidate.GetRoot();
			}
			return candidate;
		}

		internal void AddConstraint(QCon a_constraint)
		{
			_constraints = new List4(_constraints, a_constraint);
		}

		public QCandidate ReadSubCandidate(QueryingReadContext context, ITypeHandler4 handler
			)
		{
			ObjectID objectID = ObjectID.NotPossible;
			try
			{
				int offset = context.Offset();
				if (handler is IReadsObjectIds)
				{
					objectID = ((IReadsObjectIds)handler).ReadObjectID(context);
				}
				if (objectID.IsValid())
				{
					return new QCandidate(this, null, objectID._id);
				}
				if (objectID == ObjectID.NotPossible)
				{
					context.Seek(offset);
					object obj = context.Read(handler);
					if (obj != null)
					{
						QCandidate candidate = new QCandidate(this, obj, context.Container().GetID(context
							.Transaction(), obj));
						candidate.ClassMetadata(context.Container().ClassMetadataForObject(obj));
						return candidate;
					}
				}
			}
			catch (Exception)
			{
			}
			// FIXME: Catchall
			return null;
		}

		internal void Collect(Db4objects.Db4o.Internal.Query.Processor.QCandidates a_candidates
			)
		{
			IEnumerator i = IterateConstraints();
			while (i.MoveNext())
			{
				QCon qCon = (QCon)i.Current;
				SetCurrentConstraint(qCon);
				qCon.Collect(a_candidates);
			}
			SetCurrentConstraint(null);
		}

		internal void Execute()
		{
			if (DTrace.enabled)
			{
				DTrace.QueryProcess.Log();
			}
			FieldIndexProcessorResult result = ProcessFieldIndexes();
			if (result.FoundIndex())
			{
				i_root = result.ToQCandidate(this);
			}
			else
			{
				LoadFromClassIndex();
			}
			Evaluate();
		}

		public IEnumerator ExecuteSnapshot(Collection4 executionPath)
		{
			IIntIterator4 indexIterator = new IntIterator4Adaptor(IterateIndex(ProcessFieldIndexes
				()));
			Tree idRoot = TreeInt.AddAll(null, indexIterator);
			IEnumerator snapshotIterator = new TreeKeyIterator(idRoot);
			IEnumerator singleObjectQueryIterator = SingleObjectSodaProcessor(snapshotIterator
				);
			return MapIdsToExecutionPath(singleObjectQueryIterator, executionPath);
		}

		private IEnumerator SingleObjectSodaProcessor(IEnumerator indexIterator)
		{
			return Iterators.Map(indexIterator, new _IFunction4_159(this));
		}

		private sealed class _IFunction4_159 : IFunction4
		{
			public _IFunction4_159(QCandidates _enclosing)
			{
				this._enclosing = _enclosing;
			}

			public object Apply(object current)
			{
				int id = ((int)current);
				QCandidate candidate = new QCandidate(this._enclosing, null, id);
				this._enclosing.i_root = candidate;
				this._enclosing.Evaluate();
				if (!candidate.Include())
				{
					return Iterators.Skip;
				}
				return current;
			}

			private readonly QCandidates _enclosing;
		}

		public IEnumerator ExecuteLazy(Collection4 executionPath)
		{
			IEnumerator indexIterator = IterateIndex(ProcessFieldIndexes());
			IEnumerator singleObjectQueryIterator = SingleObjectSodaProcessor(indexIterator);
			return MapIdsToExecutionPath(singleObjectQueryIterator, executionPath);
		}

		private IEnumerator IterateIndex(FieldIndexProcessorResult result)
		{
			if (result.NoMatch())
			{
				return Iterators.EmptyIterator;
			}
			if (result.FoundIndex())
			{
				return result.IterateIDs();
			}
			if (!i_classMetadata.HasClassIndex())
			{
				return Iterators.EmptyIterator;
			}
			return BTreeClassIndexStrategy.Iterate(i_classMetadata, i_trans);
		}

		private IEnumerator MapIdsToExecutionPath(IEnumerator singleObjectQueryIterator, 
			Collection4 executionPath)
		{
			if (executionPath == null)
			{
				return singleObjectQueryIterator;
			}
			IEnumerator res = singleObjectQueryIterator;
			IEnumerator executionPathIterator = executionPath.GetEnumerator();
			while (executionPathIterator.MoveNext())
			{
				string fieldName = (string)executionPathIterator.Current;
				res = Iterators.Concat(Iterators.Map(res, new _IFunction4_205(this, fieldName)));
			}
			return res;
		}

		private sealed class _IFunction4_205 : IFunction4
		{
			public _IFunction4_205(QCandidates _enclosing, string fieldName)
			{
				this._enclosing = _enclosing;
				this.fieldName = fieldName;
			}

			public object Apply(object current)
			{
				int id = ((int)current);
				CollectIdContext context = CollectIdContext.ForID(this._enclosing.i_trans, id);
				if (context == null)
				{
					return Iterators.Skip;
				}
				context.ClassMetadata().CollectIDs(context, fieldName);
				return new TreeKeyIterator(context.Ids());
			}

			private readonly QCandidates _enclosing;

			private readonly string fieldName;
		}

		public ObjectContainerBase Stream()
		{
			return i_trans.Container();
		}

		public int ClassIndexEntryCount()
		{
			return i_classMetadata.IndexEntryCount(i_trans);
		}

		private FieldIndexProcessorResult ProcessFieldIndexes()
		{
			if (_constraints == null)
			{
				return FieldIndexProcessorResult.NoIndexFound;
			}
			return new FieldIndexProcessor(this).Run();
		}

		internal void Evaluate()
		{
			if (_constraints == null)
			{
				return;
			}
			ForEachConstraint(new _IProcedure4_243(this));
			ForEachConstraint(new _IProcedure4_251());
			ForEachConstraint(new _IProcedure4_257());
			ForEachConstraint(new _IProcedure4_263());
			ForEachConstraint(new _IProcedure4_269());
			ForEachConstraint(new _IProcedure4_275());
		}

		private sealed class _IProcedure4_243 : IProcedure4
		{
			public _IProcedure4_243(QCandidates _enclosing)
			{
				this._enclosing = _enclosing;
			}

			public void Apply(object arg)
			{
				QCon qCon = (QCon)arg;
				qCon.SetCandidates(this._enclosing);
				qCon.EvaluateSelf();
			}

			private readonly QCandidates _enclosing;
		}

		private sealed class _IProcedure4_251 : IProcedure4
		{
			public _IProcedure4_251()
			{
			}

			public void Apply(object arg)
			{
				((QCon)arg).EvaluateSimpleChildren();
			}
		}

		private sealed class _IProcedure4_257 : IProcedure4
		{
			public _IProcedure4_257()
			{
			}

			public void Apply(object arg)
			{
				((QCon)arg).EvaluateEvaluations();
			}
		}

		private sealed class _IProcedure4_263 : IProcedure4
		{
			public _IProcedure4_263()
			{
			}

			public void Apply(object arg)
			{
				((QCon)arg).EvaluateCreateChildrenCandidates();
			}
		}

		private sealed class _IProcedure4_269 : IProcedure4
		{
			public _IProcedure4_269()
			{
			}

			public void Apply(object arg)
			{
				((QCon)arg).EvaluateCollectChildren();
			}
		}

		private sealed class _IProcedure4_275 : IProcedure4
		{
			public _IProcedure4_275()
			{
			}

			public void Apply(object arg)
			{
				((QCon)arg).EvaluateChildren();
			}
		}

		private void ForEachConstraint(IProcedure4 proc)
		{
			IEnumerator i = IterateConstraints();
			while (i.MoveNext())
			{
				QCon constraint = (QCon)i.Current;
				if (!constraint.ProcessedByIndex())
				{
					proc.Apply(constraint);
				}
			}
		}

		internal bool IsEmpty()
		{
			bool[] ret = new bool[] { true };
			Traverse(new _IVisitor4_295(ret));
			return ret[0];
		}

		private sealed class _IVisitor4_295 : IVisitor4
		{
			public _IVisitor4_295(bool[] ret)
			{
				this.ret = ret;
			}

			public void Visit(object obj)
			{
				if (((QCandidate)obj)._include)
				{
					ret[0] = false;
				}
			}

			private readonly bool[] ret;
		}

		internal bool Filter(IVisitor4 a_host)
		{
			if (i_root != null)
			{
				i_root.Traverse(a_host);
				i_root = i_root.Filter(new _IPredicate4_308());
			}
			return i_root != null;
		}

		private sealed class _IPredicate4_308 : IPredicate4
		{
			public _IPredicate4_308()
			{
			}

			public bool Match(object a_candidate)
			{
				return ((QCandidate)a_candidate)._include;
			}
		}

		internal int GenerateCandidateId()
		{
			if (_idGenerator == null)
			{
				_idGenerator = new IDGenerator();
			}
			return -_idGenerator.Next();
		}

		public IEnumerator IterateConstraints()
		{
			if (_constraints == null)
			{
				return Iterators.EmptyIterator;
			}
			return new Iterator4Impl(_constraints);
		}

		internal sealed class TreeIntBuilder
		{
			public TreeInt tree;

			public void Add(TreeInt node)
			{
				tree = (TreeInt)((TreeInt)Tree.Add(tree, node));
			}
		}

		internal void LoadFromClassIndex()
		{
			if (!IsEmpty())
			{
				return;
			}
			QCandidates.TreeIntBuilder result = new QCandidates.TreeIntBuilder();
			IClassIndexStrategy index = i_classMetadata.Index();
			index.TraverseAll(i_trans, new _IVisitor4_346(this, result));
			i_root = result.tree;
			DiagnosticProcessor dp = i_trans.Container()._handlers.DiagnosticProcessor();
			if (dp.Enabled() && !IsClassOnlyQuery())
			{
				dp.LoadedFromClassIndex(i_classMetadata);
			}
			_loadedFromClassIndex = true;
		}

		private sealed class _IVisitor4_346 : IVisitor4
		{
			public _IVisitor4_346(QCandidates _enclosing, QCandidates.TreeIntBuilder result)
			{
				this._enclosing = _enclosing;
				this.result = result;
			}

			public void Visit(object obj)
			{
				result.Add(new QCandidate(this._enclosing, null, ((int)obj)));
			}

			private readonly QCandidates _enclosing;

			private readonly QCandidates.TreeIntBuilder result;
		}

		internal void SetCurrentConstraint(QCon a_constraint)
		{
			i_currentConstraint = a_constraint;
		}

		internal void Traverse(IVisitor4 a_visitor)
		{
			if (i_root != null)
			{
				i_root.Traverse(a_visitor);
			}
		}

		// FIXME: This method should go completely.
		//        We changed the code to create the QCandidates graph in two steps:
		//        (1) call fitsIntoExistingConstraintHierarchy to determine whether
		//            or not we need more QCandidates objects
		//        (2) add all constraints
		//        This method tries to do both in one, which results in missing
		//        constraints. Not all are added to all QCandiates.
		//        Right methodology is in 
		//        QQueryBase#createCandidateCollection
		//        and
		//        QQueryBase#createQCandidatesList
		internal bool TryAddConstraint(QCon a_constraint)
		{
			if (_field != null)
			{
				QField qf = a_constraint.GetField();
				if (qf != null)
				{
					if (_field.Name() != null && !_field.Name().Equals(qf.Name()))
					{
						return false;
					}
				}
			}
			if (i_classMetadata == null || a_constraint.IsNullConstraint())
			{
				AddConstraint(a_constraint);
				return true;
			}
			ClassMetadata yc = a_constraint.GetYapClass();
			if (yc != null)
			{
				yc = i_classMetadata.GetHigherOrCommonHierarchy(yc);
				if (yc != null)
				{
					i_classMetadata = yc;
					AddConstraint(a_constraint);
					return true;
				}
			}
			AddConstraint(a_constraint);
			return false;
		}

		public void Visit(object a_tree)
		{
			QCandidate parent = (QCandidate)a_tree;
			if (parent.CreateChild(this))
			{
				return;
			}
			// No object found.
			// All children constraints are necessarily false.
			// Check immediately.
			IEnumerator i = IterateConstraints();
			while (i.MoveNext())
			{
				((QCon)i.Current).VisitOnNull(parent.GetRoot());
			}
		}

		public override string ToString()
		{
			StringBuilder sb = new StringBuilder();
			i_root.Traverse(new _IVisitor4_430(sb));
			return sb.ToString();
		}

		private sealed class _IVisitor4_430 : IVisitor4
		{
			public _IVisitor4_430(StringBuilder sb)
			{
				this.sb = sb;
			}

			public void Visit(object obj)
			{
				QCandidate candidate = (QCandidate)obj;
				sb.Append(" ");
				sb.Append(candidate._key);
			}

			private readonly StringBuilder sb;
		}

		public Transaction Transaction()
		{
			return i_trans;
		}

		public bool WasLoadedFromClassIndex()
		{
			return _loadedFromClassIndex;
		}

		public bool FitsIntoExistingConstraintHierarchy(QCon constraint)
		{
			if (_field != null)
			{
				QField qf = constraint.GetField();
				if (qf != null)
				{
					if (_field.Name() != null && !_field.Name().Equals(qf.Name()))
					{
						return false;
					}
				}
			}
			if (i_classMetadata == null || constraint.IsNullConstraint())
			{
				return true;
			}
			ClassMetadata classMetadata = constraint.GetYapClass();
			if (classMetadata == null)
			{
				return false;
			}
			classMetadata = i_classMetadata.GetHigherOrCommonHierarchy(classMetadata);
			if (classMetadata == null)
			{
				return false;
			}
			i_classMetadata = classMetadata;
			return true;
		}

		private bool IsClassOnlyQuery()
		{
			if (((List4)_constraints._next) != null)
			{
				return false;
			}
			if (!(_constraints._element is QConClass))
			{
				return false;
			}
			return !((QCon)_constraints._element).HasChildren();
		}
	}
}
