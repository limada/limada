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
using Db4objects.Db4o.Foundation;
using Mono.Cecil;

namespace Db4oTool.Core
{
	public class CecilReflector
	{
		private readonly InstrumentationContext _context;
		private readonly IAssemblyResolver _resolver;

		public CecilReflector(InstrumentationContext context)
		{
			_context = context;
			_resolver = new RelativeAssemblyResolver(_context);

			if (_context.AlternateAssemblyLocation != null)
			{
				_resolver = new CompositeAssemblyResolver(
										new RelativeAssemblyResolver(_context.AlternateAssemblyLocation),
										_resolver);
			}
		}

		public bool Implements(TypeDefinition type, Type interfaceType)
		{
			return Implements(type, interfaceType.FullName);
		}

		private bool Implements(TypeDefinition type, string interfaceName)
		{
			if (Contains(type.Interfaces, interfaceName)) return true;
			if (null == type.BaseType) return false;

			TypeDefinition baseType = ResolveTypeReference(type.BaseType);
			if (null != baseType) return Implements(baseType, interfaceName);

			return false;
		}

		public TypeDefinition ResolveTypeReference(TypeReference typeRef)
		{
			if (null == typeRef) throw new ArgumentNullException("typeRef");

			TypeDefinition type = typeRef as TypeDefinition;
			if (null != type) return type;

			GenericParameter parameter = typeRef as GenericParameter;
			if (parameter != null) return null;

			TypeSpecification typeSpecification = typeRef as TypeSpecification;
			if (typeSpecification != null) return ResolveTypeReference(typeSpecification.ElementType);
            
			AssemblyDefinition assembly = ResolveAssembly(typeRef.Scope as AssemblyNameReference);
			if (null == assembly) return null;

			return FindType(assembly, typeRef);
		}

		private AssemblyDefinition ResolveAssembly(AssemblyNameReference assemblyRef)
		{
			return _resolver.Resolve(assemblyRef);
		}

		private TypeDefinition FindType(AssemblyDefinition assembly, TypeReference typeRef)
		{
			foreach (ModuleDefinition m in assembly.Modules)
			{
				foreach (TypeDefinition t in m.Types)
				{
					if (t.FullName == typeRef.FullName) return t;
				}
			}
			return null;
		}

		private static bool Contains(IEnumerable<TypeReference> collection, string fullName)
		{
			foreach (TypeReference typeRef in collection)
			{
				if (typeRef.FullName == fullName) return true;
			}
			return false;
		}

		public static MethodDefinition GetMethod(TypeDefinition type, string methodName)
		{
			if (type == null) throw new ArgumentNullException("type");
			if (methodName == null) throw new ArgumentNullException("methodName");

			foreach (MethodDefinition method in type.Methods)
			{
				if (method.Name == methodName) return method;
			}

			return null;
		}

		public static MethodDefinition GetMethod(TypeDefinition type, MethodReference template)
		{
			return GetMethodInternal(type, template.Name, template.Parameters);
		}

		public static MethodDefinition GetMethod(TypeDefinition type, string methodName, Type [] parameterTypes)
		{
			return GetMethodInternal(type, methodName, parameterTypes);
		}

		static MethodDefinition GetMethodInternal(TypeDefinition type, string name, IList parameters)
		{
			if (type == null) throw new ArgumentNullException ("type");

			foreach (MethodDefinition method in type.Methods)
			{
				if (method.Name != name) continue;
				if (!ParametersMatch(method.Parameters, parameters)) continue;

				return method;
			}

			return null;
		}

		static bool ParametersMatch(IList<ParameterDefinition> parameters, IList candidates)
		{
			if (parameters.Count != candidates.Count) return false;

			for (int i = 0; i < parameters.Count; i++) {
				string candidateTypeName;
				object candidate = candidates[i];

				if (candidate is Type)
					candidateTypeName = (candidate as Type).FullName.Replace('+', '/');
				else if (candidate is TypeReference)
					candidateTypeName = (candidate as TypeReference).FullName;
				else if (candidate is ParameterDefinition)
					candidateTypeName = (candidate as ParameterDefinition).ParameterType.FullName;
				else
					return false;

				if (parameters[i].ParameterType.FullName != candidateTypeName) return false;
			}

			return true;
		}

		public static FieldDefinition GetField(TypeDefinition type, string fieldName)
		{
			if (type == null) throw new ArgumentNullException("type");
			if (fieldName == null) throw new ArgumentNullException("fieldName");

			foreach (FieldDefinition field in type.Fields)
			{
				if (field.Name == fieldName) return field;
			}

			return null;
		}
	}

	internal class CompositeAssemblyResolver : IAssemblyResolver
	{
		private readonly IAssemblyResolver[] _resolvers;

		public CompositeAssemblyResolver(params IAssemblyResolver[] resolvers)
		{
			_resolvers = resolvers;
		}

		public AssemblyDefinition Resolve(string fullName)
		{
			return InternalResolve(delegate(IAssemblyResolver resolver)
			                       	{
			                       		return resolver.Resolve(fullName);
			                       	});
		}

		public AssemblyDefinition Resolve(AssemblyNameReference name)
		{
			return InternalResolve(delegate(IAssemblyResolver resolver)
									{
										return resolver.Resolve(name);
									});
		}

		private AssemblyDefinition InternalResolve(Function<IAssemblyResolver, AssemblyDefinition> @delegate)
		{
			foreach (IAssemblyResolver resolver in _resolvers)
			{
				AssemblyDefinition assemblyDefinition = @delegate(resolver);
				if (assemblyDefinition != null) return assemblyDefinition;
			}

			return null;
		}
	}
}
