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
using System.Collections.Generic;
using System.Reflection;
using Db4objects.Db4o.Instrumentation.Api;
using Mono.Cecil;

namespace Db4objects.Db4o.Instrumentation.Cecil
{
	public class CecilReferenceProvider : IReferenceProvider
	{
		public static CecilReferenceProvider ForModule(ModuleDefinition module)
		{
			return new CecilReferenceProvider(module);
		}

		private readonly ModuleDefinition _module;
		private readonly Dictionary<TypeReference, ITypeRef> _typeCache = new Dictionary<TypeReference, ITypeRef>();

		private CecilReferenceProvider(ModuleDefinition module)
		{
			if (null == module) throw new ArgumentNullException();
			_module = module;
		}

		public ITypeRef ForType(Type type)
		{
#if CF
			return ForCecilType(ImportType(type));
#else
			return ForCecilType(_module.Import(type));
#endif
		}

		public ITypeRef ForCecilType(TypeReference type)
		{
			ITypeRef typeRef;
			if (!_typeCache.TryGetValue(type, out typeRef))
			{
				typeRef = new CecilTypeRef(this, type);
				_typeCache.Add(type, typeRef);
			}
			return typeRef;
		}

		public IMethodRef ForMethod(MethodInfo method)
		{
#if CF
			return new CecilMethodRef(this, _module.Import(new MethodReference(method.Name, ImportType(method.ReturnType))));
#else
			return new CecilMethodRef(this, _module.Import(method));
#endif
		}

#if CF
		private TypeReference ImportType(Type type)
		{
			return _module.Import(new TypeReference(type.Namespace, type.Name, _module));
		}
#endif

		public IMethodRef ForMethod(ITypeRef declaringType, string methodName, ITypeRef[] parameterTypes, ITypeRef returnType)
		{
			throw new NotImplementedException();
		}

		public IFieldRef ForCecilField(FieldReference field)
		{
			return new CecilFieldRef(this, field);
		}
	}
}
