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
using Db4objects.Db4o.Foundation;
using Db4objects.Db4o.Internal;
using Db4objects.Db4o.Internal.Delete;
using Db4objects.Db4o.Internal.Marshall;
using Db4objects.Db4o.Typehandlers;

namespace Db4objects.Db4o.Internal
{
	/// <exclude></exclude>
	public class TypeHandlerAspect : ClassAspect
	{
		public readonly ITypeHandler4 _typeHandler;

		private readonly ClassMetadata _ownerMetadata;

		public TypeHandlerAspect(ClassMetadata classMetadata, ITypeHandler4 typeHandler)
		{
			if (Handlers4.IsValueType(typeHandler))
			{
				throw new InvalidOperationException();
			}
			_ownerMetadata = classMetadata;
			_typeHandler = typeHandler;
		}

		public override bool Equals(object obj)
		{
			if (obj == this)
			{
				return true;
			}
			if (obj == null || obj.GetType() != GetType())
			{
				return false;
			}
			Db4objects.Db4o.Internal.TypeHandlerAspect other = (Db4objects.Db4o.Internal.TypeHandlerAspect
				)obj;
			return _typeHandler.Equals(other._typeHandler);
		}

		public override int GetHashCode()
		{
			return _typeHandler.GetHashCode();
		}

		public override string GetName()
		{
			return _typeHandler.GetType().FullName;
		}

		public override void CascadeActivation(IActivationContext context)
		{
			if (!Handlers4.IsCascading(_typeHandler))
			{
				return;
			}
			Handlers4.CascadeActivation(context, _typeHandler);
		}

		public override void CollectIDs(CollectIdContext context)
		{
			if (!Handlers4.IsCascading(_typeHandler))
			{
				IncrementOffset(context);
				return;
			}
			context.SlotFormat().DoWithSlotIndirection(context, new _IClosure4_58(this, context
				));
		}

		private sealed class _IClosure4_58 : IClosure4
		{
			public _IClosure4_58(TypeHandlerAspect _enclosing, CollectIdContext context)
			{
				this._enclosing = _enclosing;
				this.context = context;
			}

			public object Run()
			{
				QueryingReadContext queryingReadContext = new QueryingReadContext(context.Transaction
					(), context.HandlerVersion(), context.Buffer(), 0, context.Collector());
				((ICascadingTypeHandler)this._enclosing._typeHandler).CollectIDs(queryingReadContext
					);
				return null;
			}

			private readonly TypeHandlerAspect _enclosing;

			private readonly CollectIdContext context;
		}

		public override void DefragAspect(IDefragmentContext context)
		{
			context.SlotFormat().DoWithSlotIndirection(context, new _IClosure4_68(this, context
				));
		}

		private sealed class _IClosure4_68 : IClosure4
		{
			public _IClosure4_68(TypeHandlerAspect _enclosing, IDefragmentContext context)
			{
				this._enclosing = _enclosing;
				this.context = context;
			}

			public object Run()
			{
				this._enclosing._typeHandler.Defragment(context);
				return null;
			}

			private readonly TypeHandlerAspect _enclosing;

			private readonly IDefragmentContext context;
		}

		public override int LinkLength()
		{
			return Const4.IndirectionLength;
		}

		public override void Marshall(MarshallingContext context, object obj)
		{
			context.CreateIndirectionWithinSlot();
			if (IsNotHandlingConcreteType(context))
			{
				_typeHandler.Write(context, obj);
				return;
			}
			if (_typeHandler is IInstantiatingTypeHandler)
			{
				IInstantiatingTypeHandler instantiating = (IInstantiatingTypeHandler)_typeHandler;
				instantiating.WriteInstantiation(context, obj);
				instantiating.Write(context, obj);
			}
			else
			{
				_typeHandler.Write(context, obj);
			}
		}

		private bool IsNotHandlingConcreteType(MarshallingContext context)
		{
			return context.ClassMetadata() != _ownerMetadata;
		}

		public override Db4objects.Db4o.Internal.Marshall.AspectType AspectType()
		{
			return Db4objects.Db4o.Internal.Marshall.AspectType.Typehandler;
		}

		public override void Activate(UnmarshallingContext context)
		{
			if (!CheckEnabled(context))
			{
				return;
			}
			context.SlotFormat().DoWithSlotIndirection(context, new _IClosure4_110(this, context
				));
		}

		private sealed class _IClosure4_110 : IClosure4
		{
			public _IClosure4_110(TypeHandlerAspect _enclosing, UnmarshallingContext context)
			{
				this._enclosing = _enclosing;
				this.context = context;
			}

			public object Run()
			{
				Handlers4.Activate(context, this._enclosing._typeHandler);
				return null;
			}

			private readonly TypeHandlerAspect _enclosing;

			private readonly UnmarshallingContext context;
		}

		public override void Delete(DeleteContextImpl context, bool isUpdate)
		{
			context.SlotFormat().DoWithSlotIndirection(context, new _IClosure4_119(this, context
				));
		}

		private sealed class _IClosure4_119 : IClosure4
		{
			public _IClosure4_119(TypeHandlerAspect _enclosing, DeleteContextImpl context)
			{
				this._enclosing = _enclosing;
				this.context = context;
			}

			public object Run()
			{
				this._enclosing._typeHandler.Delete(context);
				return null;
			}

			private readonly TypeHandlerAspect _enclosing;

			private readonly DeleteContextImpl context;
		}

		public override void Deactivate(IActivationContext context)
		{
			CascadeActivation(context);
		}

		public override bool CanBeDisabled()
		{
			return true;
		}
	}
}
