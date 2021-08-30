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
using Db4objects.Db4o.Ext;
using Db4objects.Db4o.Internal;
using Db4objects.Db4o.Internal.Activation;
using Db4objects.Db4o.Internal.Marshall;
using Db4objects.Db4o.Marshall;
using Db4objects.Db4o.Typehandlers;

namespace Db4objects.Db4o.Internal.Marshall
{
	/// <summary>Wraps the low-level details of reading a Buffer, which in turn is a glorified byte array.
	/// 	</summary>
	/// <remarks>Wraps the low-level details of reading a Buffer, which in turn is a glorified byte array.
	/// 	</remarks>
	/// <exclude></exclude>
	public class UnmarshallingContext : ObjectReferenceContext, IHandlerVersionContext
		, IReferenceActivationContext
	{
		private object _object;

		private int _addToIDTree;

		private bool _checkIDTree;

		public UnmarshallingContext(Transaction transaction, ByteArrayBuffer buffer, ObjectReference
			 @ref, int addToIDTree, bool checkIDTree) : base(transaction, buffer, null, @ref
			)
		{
			_addToIDTree = addToIDTree;
			_checkIDTree = checkIDTree;
		}

		public UnmarshallingContext(Transaction transaction, ObjectReference @ref, int addToIDTree
			, bool checkIDTree) : this(transaction, null, @ref, addToIDTree, checkIDTree)
		{
		}

		public virtual object Read()
		{
			if (!BeginProcessing())
			{
				return _object;
			}
			ReadBuffer(ObjectId());
			if (Buffer() == null)
			{
				EndProcessing();
				return _object;
			}
			ClassMetadata classMetadata = ReadObjectHeader();
			if (classMetadata == null)
			{
				InvalidSlot();
				EndProcessing();
				return _object;
			}
			_reference.ClassMetadata(classMetadata);
			AdjustActivationDepth();
			if (_checkIDTree)
			{
				object objectInCacheFromClassCreation = Transaction().ObjectForIdFromCache(ObjectId
					());
				if (objectInCacheFromClassCreation != null)
				{
					_object = objectInCacheFromClassCreation;
					EndProcessing();
					return _object;
				}
			}
			if (PeekPersisted())
			{
				_object = ClassMetadata().InstantiateTransient(this);
			}
			else
			{
				_object = ClassMetadata().Instantiate(this);
			}
			EndProcessing();
			return _object;
		}

		private void InvalidSlot()
		{
			if (Container().Config().RecoveryMode())
			{
				return;
			}
			throw new InvalidSlotException("id: " + ObjectId());
		}

		private void AdjustActivationDepth()
		{
			if (UnknownActivationDepth.Instance == _activationDepth)
			{
				_activationDepth = Container().DefaultActivationDepth(ClassMetadata());
			}
		}

		private IActivationDepthProvider ActivationDepthProvider()
		{
			return Container().ActivationDepthProvider();
		}

		public virtual object ReadFullyActivatedObjectForKeys(ITypeHandler4 handler)
		{
			object obj = ReadObject(handler);
			if (obj == null)
			{
				return obj;
			}
			IActivationDepth activationDepth = ActivationDepthProvider().ActivationDepth(int.MaxValue
				, ActivationMode.Activate);
			Container().Activate(Transaction(), obj, activationDepth);
			return obj;
		}

		public virtual object ReadFieldValue(FieldMetadata field)
		{
			ReadBuffer(ObjectId());
			if (Buffer() == null)
			{
				return null;
			}
			ClassMetadata classMetadata = ReadObjectHeader();
			if (classMetadata == null)
			{
				return null;
			}
			return ReadFieldValue(classMetadata, field);
		}

		private object ReadFieldValue(ClassMetadata classMetadata, FieldMetadata field)
		{
			if (!classMetadata.SeekToField(this, field))
			{
				return null;
			}
			return field.Read(this);
		}

		private ClassMetadata ReadObjectHeader()
		{
			_objectHeader = new ObjectHeader(Container(), ByteArrayBuffer());
			ClassMetadata classMetadata = _objectHeader.ClassMetadata();
			if (classMetadata == null)
			{
				return null;
			}
			return classMetadata;
		}

		private void ReadBuffer(int id)
		{
			if (Buffer() == null && id > 0)
			{
				Buffer(Container().ReadBufferById(Transaction(), id));
			}
		}

		private bool BeginProcessing()
		{
			return _reference.BeginProcessing();
		}

		private void EndProcessing()
		{
			_reference.EndProcessing();
		}

		public virtual void SetStateClean()
		{
			_reference.SetStateClean();
		}

		public virtual object PersistentObject()
		{
			return _object;
		}

		public virtual void SetObjectWeak(object obj)
		{
			_reference.SetObjectWeak(Container(), obj);
		}

		protected override bool PeekPersisted()
		{
			return _addToIDTree == Const4.Transient;
		}

		public virtual Config4Class ClassConfig()
		{
			return ClassMetadata().Config();
		}

		public virtual void PersistentObject(object obj)
		{
			_object = obj;
		}
	}
}
