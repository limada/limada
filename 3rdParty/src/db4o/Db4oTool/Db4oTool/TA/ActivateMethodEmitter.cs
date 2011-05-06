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
using Db4oTool.Core;
using Db4objects.Db4o.Activation;
using Db4objects.Db4o.TA;
using Mono.Cecil;
using Mono.Cecil.Cil;

namespace Db4oTool.TA
{
	class ActivateMethodEmitter : MethodEmitter
	{
		public ActivateMethodEmitter(InstrumentationContext context, FieldDefinition field) : base(context, field)
		{
		}

		public MethodDefinition Emit()
		{
			MethodDefinition activate = NewExplicitMethod(typeof(IActivatable).GetMethod("Activate", new Type[] { typeof(ActivationPurpose) }));

			ILProcessor cil = activate.Body.GetILProcessor ();
			cil.Emit(OpCodes.Ldarg_0);
			cil.Emit(OpCodes.Ldfld, _activatorField);

			Instruction ret = cil.Create(OpCodes.Ret);

			cil.Emit(OpCodes.Brfalse, ret);

			cil.Emit(OpCodes.Ldarg_0);
			cil.Emit(OpCodes.Ldfld, _activatorField);
			cil.Emit(OpCodes.Ldarg_1);
			cil.Emit(OpCodes.Callvirt, _context.Import(typeof(IActivator).GetMethod("Activate", new Type[] { typeof(ActivationPurpose) })));

			cil.Append(ret);

			return activate;
		}
	}
}
