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
using Db4objects.Db4o;
using Db4objects.Db4o.Config;
using Db4objects.Db4o.Foundation;
using Db4objects.Db4o.Internal;
using Db4objects.Db4o.Internal.Handlers;
using Db4objects.Db4o.Internal.Handlers.Array;
using Db4objects.Db4o.Internal.Marshall;
using Db4objects.Db4o.Internal.Query.Processor;
using Db4objects.Db4o.Marshall;
using Db4objects.Db4o.Query;
using Db4objects.Db4o.Reflect;
using Db4objects.Db4o.Typehandlers;

namespace Db4objects.Db4o.Internal.Query.Processor
{
	/// <summary>Represents an actual object in the database.</summary>
	/// <remarks>
	/// Represents an actual object in the database. Forms a tree structure, indexed
	/// by id. Can have dependents that are doNotInclude'd in the query result when
	/// this is doNotInclude'd.
	/// </remarks>
	/// <exclude></exclude>
	public class QCandidate : TreeInt, ICandidate
	{
		internal ByteArrayBuffer _bytes;

		internal readonly QCandidates _candidates;

		private List4 _dependants;

		internal bool _include = true;

		private object _member;

		private Tree _pendingJoins;

		private Db4objects.Db4o.Internal.Query.Processor.QCandidate _root;

		private Db4objects.Db4o.Internal.ClassMetadata _classMetadata;

		private FieldMetadata _fieldMetadata;

		private int _handlerVersion;

		private QCandidate(QCandidates qcandidates) : base(0)
		{
			// db4o ID is stored in _key;
			// db4o byte stream storing the object
			// Dependent candidates
			// whether to include in the result set
			// may use id for optimisation ???
			// Possible pending joins on children
			// The evaluation root to compare all ORs
			// the ClassMetadata of this object
			// temporary field and member for one field during evaluation
			// null denotes null object
			_candidates = qcandidates;
		}

		public QCandidate(QCandidates candidates, object member, int id) : base(id)
		{
			if (DTrace.enabled)
			{
				DTrace.CreateCandidate.Log(id);
			}
			_candidates = candidates;
			_member = member;
			_include = true;
			if (id == 0)
			{
				_key = candidates.GenerateCandidateId();
			}
		}

		public override object ShallowClone()
		{
			Db4objects.Db4o.Internal.Query.Processor.QCandidate qcan = new Db4objects.Db4o.Internal.Query.Processor.QCandidate
				(_candidates);
			qcan.SetBytes(_bytes);
			qcan._dependants = _dependants;
			qcan._include = _include;
			qcan._member = _member;
			qcan._pendingJoins = _pendingJoins;
			qcan._root = _root;
			qcan._classMetadata = _classMetadata;
			qcan._fieldMetadata = _fieldMetadata;
			return base.ShallowCloneInternal(qcan);
		}

		internal virtual void AddDependant(Db4objects.Db4o.Internal.Query.Processor.QCandidate
			 a_candidate)
		{
			_dependants = new List4(_dependants, a_candidate);
		}

		private void CheckInstanceOfCompare()
		{
			if (_member is ICompare)
			{
				_member = ((ICompare)_member).Compare();
				LocalObjectContainer stream = Container();
				_classMetadata = stream.ClassMetadataForReflectClass(stream.Reflector().ForObject
					(_member));
				_key = stream.GetID(Transaction(), _member);
				if (_key == 0)
				{
					SetBytes(null);
				}
				else
				{
					SetBytes(stream.ReadBufferById(Transaction(), _key));
				}
			}
		}

		internal virtual bool CreateChild(QCandidates a_candidates)
		{
			if (!_include)
			{
				return false;
			}
			if (_fieldMetadata != null)
			{
				ITypeHandler4 handler = _fieldMetadata.GetHandler();
				if (handler != null)
				{
					QueryingReadContext queryingReadContext = new QueryingReadContext(Transaction(), 
						MarshallerFamily().HandlerVersion(), _bytes, _key);
					ITypeHandler4 arrayElementHandler = Handlers4.ArrayElementHandler(handler, queryingReadContext
						);
					if (arrayElementHandler != null)
					{
						int offset = queryingReadContext.Offset();
						bool outerRes = true;
						// The following construct is worse than not ideal.
						// For each constraint it completely reads the
						// underlying structure again. The structure could b
						// kept fairly easy. TODO: Optimize!
						IEnumerator i = a_candidates.IterateConstraints();
						while (i.MoveNext())
						{
							QCon qcon = (QCon)i.Current;
							QField qf = qcon.GetField();
							if (qf == null || qf.Name().Equals(_fieldMetadata.GetName()))
							{
								QCon tempParent = qcon.Parent();
								qcon.SetParent(null);
								QCandidates candidates = new QCandidates(a_candidates.i_trans, null, qf);
								candidates.AddConstraint(qcon);
								qcon.SetCandidates(candidates);
								ReadArrayCandidates(handler, queryingReadContext.Buffer(), arrayElementHandler, candidates
									);
								queryingReadContext.Seek(offset);
								bool isNot = qcon.IsNot();
								if (isNot)
								{
									qcon.RemoveNot();
								}
								candidates.Evaluate();
								ByRef pending = ByRef.NewInstance();
								bool[] innerRes = new bool[] { isNot };
								candidates.Traverse(new _IVisitor4_160(innerRes, isNot, pending));
								// Collect all pending subresults.
								// We need to change
								// the
								// constraint here, so
								// our
								// pending collector
								// uses
								// the right
								// comparator.
								// We only keep one
								// pending result
								// for
								// all array
								// elements.
								// and memorize,
								// whether we had a
								// true or a false
								// result.
								// or both.
								if (isNot)
								{
									qcon.Not();
								}
								// In case we had pending subresults, we
								// need to communicate
								// them up to our root.
								if (((Tree)pending.value) != null)
								{
									((Tree)pending.value).Traverse(new _IVisitor4_229(this));
								}
								if (!innerRes[0])
								{
									// Again this could be double triggering.
									// 
									// We want to clean up the "No route"
									// at some stage.
									qcon.Visit(GetRoot(), qcon.Evaluator().Not(false));
									outerRes = false;
								}
								qcon.SetParent(tempParent);
							}
						}
						return outerRes;
					}
					// We may get simple types here too, if the YapField was null
					// in the higher level simple evaluation. Evaluate these
					// immediately.
					if (Handlers4.IsQueryLeaf(handler))
					{
						a_candidates.i_currentConstraint.Visit(this);
						return true;
					}
				}
			}
			if (_fieldMetadata == null)
			{
				return false;
			}
			if (_fieldMetadata is NullFieldMetadata)
			{
				return false;
			}
			_classMetadata.SeekToField(Transaction(), _bytes, _fieldMetadata);
			Db4objects.Db4o.Internal.Query.Processor.QCandidate candidate = ReadSubCandidate(
				a_candidates);
			if (candidate == null)
			{
				return false;
			}
			// fast early check for ClassMetadata
			if (a_candidates.i_classMetadata != null && a_candidates.i_classMetadata.IsStronglyTyped
				())
			{
				ITypeHandler4 handler = _fieldMetadata.GetHandler();
				if (Handlers4.IsUntyped(handler))
				{
					handler = TypeHandlerFor(candidate);
				}
				if (handler == null)
				{
					return false;
				}
			}
			AddDependant(a_candidates.Add(candidate));
			return true;
		}

		private sealed class _IVisitor4_160 : IVisitor4
		{
			public _IVisitor4_160(bool[] innerRes, bool isNot, ByRef pending)
			{
				this.innerRes = innerRes;
				this.isNot = isNot;
				this.pending = pending;
			}

			public void Visit(object obj)
			{
				Db4objects.Db4o.Internal.Query.Processor.QCandidate cand = (Db4objects.Db4o.Internal.Query.Processor.QCandidate
					)obj;
				if (cand.Include())
				{
					innerRes[0] = !isNot;
				}
				if (cand._pendingJoins != null)
				{
					cand._pendingJoins.Traverse(new _IVisitor4_173(pending));
				}
			}

			private sealed class _IVisitor4_173 : IVisitor4
			{
				public _IVisitor4_173(ByRef pending)
				{
					this.pending = pending;
				}

				public void Visit(object a_object)
				{
					QPending newPending = ((QPending)a_object).InternalClonePayload();
					newPending.ChangeConstraint();
					QPending oldPending = (QPending)Tree.Find(((Tree)pending.value), newPending);
					if (oldPending != null)
					{
						if (oldPending._result != newPending._result)
						{
							oldPending._result = QPending.Both;
						}
					}
					else
					{
						pending.value = Tree.Add(((Tree)pending.value), newPending);
					}
				}

				private readonly ByRef pending;
			}

			private readonly bool[] innerRes;

			private readonly bool isNot;

			private readonly ByRef pending;
		}

		private sealed class _IVisitor4_229 : IVisitor4
		{
			public _IVisitor4_229(QCandidate _enclosing)
			{
				this._enclosing = _enclosing;
			}

			public void Visit(object a_object)
			{
				this._enclosing.GetRoot().Evaluate((QPending)a_object);
			}

			private readonly QCandidate _enclosing;
		}

		private ITypeHandler4 TypeHandlerFor(Db4objects.Db4o.Internal.Query.Processor.QCandidate
			 candidate)
		{
			Db4objects.Db4o.Internal.ClassMetadata classMetadata = candidate.ReadClassMetadata
				();
			if (classMetadata != null)
			{
				return classMetadata.TypeHandler();
			}
			return null;
		}

		private void ReadArrayCandidates(ITypeHandler4 typeHandler, IReadBuffer buffer, ITypeHandler4
			 arrayElementHandler, QCandidates candidates)
		{
			if (!Handlers4.IsCascading(arrayElementHandler))
			{
				return;
			}
			SlotFormat slotFormat = SlotFormat.ForHandlerVersion(_handlerVersion);
			slotFormat.DoWithSlotIndirection(buffer, typeHandler, new _IClosure4_318(this, arrayElementHandler
				, buffer, candidates));
		}

		private sealed class _IClosure4_318 : IClosure4
		{
			public _IClosure4_318(QCandidate _enclosing, ITypeHandler4 arrayElementHandler, IReadBuffer
				 buffer, QCandidates candidates)
			{
				this._enclosing = _enclosing;
				this.arrayElementHandler = arrayElementHandler;
				this.buffer = buffer;
				this.candidates = candidates;
			}

			public object Run()
			{
				QueryingReadContext context = null;
				if (Handlers4.HandleAsObject(arrayElementHandler))
				{
					// TODO: Code is similar to FieldMetadata.collectIDs. Try to refactor to one place.
					int collectionID = buffer.ReadInt();
					ByteArrayBuffer arrayElementBuffer = this._enclosing.Container().ReadBufferById(this
						._enclosing.Transaction(), collectionID);
					ObjectHeader objectHeader = ObjectHeader.ScrollBufferToContent(this._enclosing.Container
						(), arrayElementBuffer);
					context = new QueryingReadContext(this._enclosing.Transaction(), candidates, this
						._enclosing._handlerVersion, arrayElementBuffer, collectionID);
					objectHeader.ClassMetadata().CollectIDs(context);
				}
				else
				{
					context = new QueryingReadContext(this._enclosing.Transaction(), candidates, this
						._enclosing._handlerVersion, buffer, 0);
					((ICascadingTypeHandler)arrayElementHandler).CollectIDs(context);
				}
				Tree.Traverse(context.Ids(), new _IVisitor4_336(candidates));
				IEnumerator i = context.ObjectsWithoutId();
				while (i.MoveNext())
				{
					object obj = i.Current;
					candidates.Add(new Db4objects.Db4o.Internal.Query.Processor.QCandidate(candidates
						, obj, 0));
				}
				return null;
			}

			private sealed class _IVisitor4_336 : IVisitor4
			{
				public _IVisitor4_336(QCandidates candidates)
				{
					this.candidates = candidates;
				}

				public void Visit(object obj)
				{
					TreeInt idNode = (TreeInt)obj;
					candidates.Add(new Db4objects.Db4o.Internal.Query.Processor.QCandidate(candidates
						, null, idNode._key));
				}

				private readonly QCandidates candidates;
			}

			private readonly QCandidate _enclosing;

			private readonly ITypeHandler4 arrayElementHandler;

			private readonly IReadBuffer buffer;

			private readonly QCandidates candidates;
		}

		internal virtual void DoNotInclude()
		{
			Include(false);
			if (_dependants != null)
			{
				IEnumerator i = new Iterator4Impl(_dependants);
				_dependants = null;
				while (i.MoveNext())
				{
					((Db4objects.Db4o.Internal.Query.Processor.QCandidate)i.Current).DoNotInclude();
				}
			}
		}

		internal virtual bool Evaluate(QConObject a_constraint, QE a_evaluator)
		{
			if (a_evaluator.Identity())
			{
				return a_evaluator.Evaluate(a_constraint, this, null);
			}
			if (_member == null)
			{
				_member = Value();
			}
			return a_evaluator.Evaluate(a_constraint, this, a_constraint.Translate(_member));
		}

		internal virtual bool Evaluate(QPending a_pending)
		{
			QPending oldPending = (QPending)Tree.Find(_pendingJoins, a_pending);
			if (oldPending == null)
			{
				a_pending.ChangeConstraint();
				_pendingJoins = Tree.Add(_pendingJoins, a_pending.InternalClonePayload());
				return true;
			}
			_pendingJoins = _pendingJoins.RemoveNode(oldPending);
			oldPending._join.EvaluatePending(this, oldPending, a_pending._result);
			return false;
		}

		internal virtual IReflectClass ClassReflector()
		{
			ReadClassMetadata();
			if (_classMetadata == null)
			{
				return null;
			}
			return _classMetadata.ClassReflector();
		}

		internal virtual bool FieldIsAvailable()
		{
			return ClassReflector() != null;
		}

		// / ***<Candidate interface code>***
		public virtual IObjectContainer ObjectContainer()
		{
			return Container();
		}

		public virtual object GetObject()
		{
			object obj = Value(true);
			if (obj is ByteArrayBuffer)
			{
				ByteArrayBuffer reader = (ByteArrayBuffer)obj;
				int offset = reader._offset;
				obj = ReadString(reader);
				reader._offset = offset;
			}
			return obj;
		}

		public virtual string ReadString(ByteArrayBuffer buffer)
		{
			return StringHandler.ReadString(Transaction().Context(), buffer);
		}

		internal virtual Db4objects.Db4o.Internal.Query.Processor.QCandidate GetRoot()
		{
			return _root == null ? this : _root;
		}

		internal LocalObjectContainer Container()
		{
			return Transaction().LocalContainer();
		}

		internal LocalTransaction Transaction()
		{
			return _candidates.i_trans;
		}

		public virtual bool Include()
		{
			return _include;
		}

		/// <summary>For external interface use only.</summary>
		/// <remarks>
		/// For external interface use only. Call doNotInclude() internally so
		/// dependancies can be checked.
		/// </remarks>
		public virtual void Include(bool flag)
		{
			// TODO:
			// Internal and external flag may need to be handled seperately.
			_include = flag;
		}

		public override Tree OnAttemptToAddDuplicate(Tree oldNode)
		{
			_size = 0;
			_root = (Db4objects.Db4o.Internal.Query.Processor.QCandidate)oldNode;
			return oldNode;
		}

		private IReflectClass MemberClass()
		{
			return Transaction().Reflector().ForObject(_member);
		}

		internal virtual IPreparedComparison PrepareComparison(ObjectContainerBase container
			, object constraint)
		{
			IContext context = container.Transaction.Context();
			if (_fieldMetadata != null)
			{
				return _fieldMetadata.PrepareComparison(context, constraint);
			}
			if (_classMetadata != null)
			{
				return _classMetadata.PrepareComparison(context, constraint);
			}
			IReflector reflector = container.Reflector();
			Db4objects.Db4o.Internal.ClassMetadata classMetadata = null;
			if (_bytes != null)
			{
				classMetadata = container.ProduceClassMetadata(reflector.ForObject(constraint));
			}
			else
			{
				if (_member != null)
				{
					classMetadata = container.ClassMetadataForReflectClass(reflector.ForObject(_member
						));
				}
			}
			if (classMetadata != null)
			{
				if (_member != null && _member.GetType().IsArray)
				{
					ITypeHandler4 arrayElementTypehandler = classMetadata.TypeHandler();
					if (reflector.Array().IsNDimensional(MemberClass()))
					{
						MultidimensionalArrayHandler mah = new MultidimensionalArrayHandler(arrayElementTypehandler
							, false);
						return mah.PrepareComparison(context, _member);
					}
					ArrayHandler ya = new ArrayHandler(arrayElementTypehandler, false);
					return ya.PrepareComparison(context, _member);
				}
				return classMetadata.PrepareComparison(context, constraint);
			}
			return null;
		}

		private void Read()
		{
			if (_include)
			{
				if (_bytes == null)
				{
					if (_key > 0)
					{
						if (DTrace.enabled)
						{
							DTrace.CandidateRead.Log(_key);
						}
						SetBytes(Container().ReadBufferById(Transaction(), _key));
						if (_bytes == null)
						{
							Include(false);
						}
					}
					else
					{
						Include(false);
					}
				}
			}
		}

		private int CurrentOffSet()
		{
			return _bytes._offset;
		}

		private Db4objects.Db4o.Internal.Query.Processor.QCandidate ReadSubCandidate(QCandidates
			 candidateCollection)
		{
			Read();
			if (_bytes == null || _fieldMetadata == null)
			{
				return null;
			}
			int offset = CurrentOffSet();
			QueryingReadContext context = NewQueryingReadContext();
			ITypeHandler4 handler = HandlerRegistry.CorrectHandlerVersion(context, _fieldMetadata
				.GetHandler());
			Db4objects.Db4o.Internal.Query.Processor.QCandidate subCandidate = candidateCollection
				.ReadSubCandidate(context, handler);
			Seek(offset);
			if (subCandidate != null)
			{
				subCandidate._root = GetRoot();
				return subCandidate;
			}
			return null;
		}

		private void Seek(int offset)
		{
			_bytes._offset = offset;
		}

		private QueryingReadContext NewQueryingReadContext()
		{
			return new QueryingReadContext(Transaction(), _handlerVersion, _bytes, _key);
		}

		private void ReadThis(bool a_activate)
		{
			Read();
			ObjectContainerBase container = Transaction().Container();
			_member = container.TryGetByID(Transaction(), _key);
			if (_member != null && (a_activate || _member is ICompare))
			{
				container.Activate(Transaction(), _member);
				CheckInstanceOfCompare();
			}
		}

		internal virtual Db4objects.Db4o.Internal.ClassMetadata ReadClassMetadata()
		{
			if (_classMetadata == null)
			{
				Read();
				if (_bytes != null)
				{
					Seek(0);
					ObjectContainerBase stream = Container();
					ObjectHeader objectHeader = new ObjectHeader(stream, _bytes);
					_classMetadata = objectHeader.ClassMetadata();
					if (_classMetadata != null)
					{
						if (stream._handlers.IclassCompare.IsAssignableFrom(_classMetadata.ClassReflector
							()))
						{
							ReadThis(false);
						}
					}
				}
			}
			return _classMetadata;
		}

		public override string ToString()
		{
			string str = "QCandidate ";
			if (_classMetadata != null)
			{
				str += "\n   YapClass " + _classMetadata.GetName();
			}
			if (_fieldMetadata != null)
			{
				str += "\n   YapField " + _fieldMetadata.GetName();
			}
			if (_member != null)
			{
				str += "\n   Member " + _member.ToString();
			}
			if (_root != null)
			{
				str += "\n  rooted by:\n";
				str += _root.ToString();
			}
			else
			{
				str += "\n  ROOT";
			}
			return str;
		}

		internal virtual void UseField(QField a_field)
		{
			Read();
			if (_bytes == null)
			{
				_fieldMetadata = null;
				return;
			}
			ReadClassMetadata();
			_member = null;
			if (a_field == null)
			{
				_fieldMetadata = null;
				return;
			}
			if (_classMetadata == null)
			{
				_fieldMetadata = null;
				return;
			}
			_fieldMetadata = FieldMetadataFrom(a_field, _classMetadata);
			if (_fieldMetadata == null)
			{
				FieldNotFound();
				return;
			}
			HandlerVersion handlerVersion = _classMetadata.SeekToField(Transaction(), _bytes, 
				_fieldMetadata);
			if (handlerVersion == HandlerVersion.Invalid)
			{
				FieldNotFound();
				return;
			}
			_handlerVersion = handlerVersion._number;
		}

		private FieldMetadata FieldMetadataFrom(QField qField, Db4objects.Db4o.Internal.ClassMetadata
			 type)
		{
			FieldMetadata existingField = qField.GetFieldMetadata();
			if (existingField != null)
			{
				return existingField;
			}
			FieldMetadata field = type.FieldMetadataForName(qField.Name());
			if (field != null)
			{
				field.Alive();
			}
			return field;
		}

		private void FieldNotFound()
		{
			if (_classMetadata.HoldsAnyClass())
			{
				// retry finding the field on reading the value 
				_fieldMetadata = null;
			}
			else
			{
				// we can't get a value for the field, comparisons should definitely run against null
				_fieldMetadata = new NullFieldMetadata();
			}
			_handlerVersion = HandlerRegistry.HandlerVersion;
		}

		internal virtual object Value()
		{
			return Value(false);
		}

		// TODO: This is only used for Evaluations. Handling may need
		// to be different for collections also.
		internal virtual object Value(bool a_activate)
		{
			if (_member == null)
			{
				if (_fieldMetadata == null)
				{
					ReadThis(a_activate);
				}
				else
				{
					int offset = CurrentOffSet();
					_member = _fieldMetadata.Read(NewQueryingReadContext());
					Seek(offset);
					CheckInstanceOfCompare();
				}
			}
			return _member;
		}

		internal virtual void SetBytes(ByteArrayBuffer bytes)
		{
			_bytes = bytes;
		}

		private Db4objects.Db4o.Internal.Marshall.MarshallerFamily MarshallerFamily()
		{
			return Db4objects.Db4o.Internal.Marshall.MarshallerFamily.Version(_handlerVersion
				);
		}

		public override bool Duplicates()
		{
			return _root != null;
		}

		public virtual void ClassMetadata(Db4objects.Db4o.Internal.ClassMetadata classMetadata
			)
		{
			_classMetadata = classMetadata;
		}
	}
}
