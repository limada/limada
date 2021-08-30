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
using System.Reflection;
#if !CF
using System.Reflection.Emit;
#endif
using System.Runtime.CompilerServices;

namespace Db4objects.Db4o.Internal.Reflect.Emitters
{
#if !CF
	class GetFieldEmitter : Emitter
	{
		public GetFieldEmitter(FieldInfo field) : base(field, typeof(object), new Type[] { typeof(object) })
		{
		}

		public Getter Emit()
		{
			EmitMethodBody();
			return (Getter)CreateDelegate <Getter>();
		}

		private void EmitMethodBody()
		{
			if (_field.IsStatic)
			{
				// make sure type is initialized before
				// accessing any static fields
				RuntimeHelpers.RunClassConstructor(_field.DeclaringType.TypeHandle);
				_il.Emit(OpCodes.Ldsfld, _field);
			}
			else
			{
				EmitLoadTargetObject(_field.DeclaringType);
				_il.Emit(OpCodes.Ldfld, _field);
			}

			EmitReturn();
		}

		protected void EmitReturn()
		{
			BoxIfNeeded(_field.FieldType);
			_il.Emit(OpCodes.Ret);
		}
	}
#endif
}
