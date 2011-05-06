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
using System.Collections.Generic;

namespace Db4oTool.Core
{
	using Mono.Cecil;

	public abstract class AbstractAssemblyInstrumentation : IAssemblyInstrumentation
	{
		protected InstrumentationContext _context;

		public virtual void Run(InstrumentationContext context)
		{
			_context = context;
			try
			{
				BeforeAssemblyProcessing();
				ProcessAssembly();
				AfterAssemblyProcessing();
			}
			finally
			{
				_context = null;
			}
		}

		protected virtual void BeforeAssemblyProcessing()
		{
		}
		
		protected virtual void AfterAssemblyProcessing()
		{	
		}

		protected virtual void ProcessAssembly()
		{
			foreach (ModuleDefinition module in _context.Assembly.Modules)
			{
				TraceVerbose("Entering module '{0}'", module);
				ProcessModule(module);
				TraceVerbose("Leaving module '{0}'", module);
			}
		}

		protected virtual void ProcessModule(ModuleDefinition module)
		{
			ProcessTypes(module.Types, ProcessType);
		}

		protected virtual void ProcessTypes(IEnumerable<TypeDefinition> types, System.Action<TypeDefinition> action)
		{
			ProcessTypes(types, Accept, action);
		}

		protected bool Accept(TypeDefinition type)
		{
			return _context.Accept(type);
		}

		protected static bool NoFiltering(TypeDefinition type)
		{
			return true;
		}

		protected virtual void ProcessTypes (IEnumerable<TypeDefinition> types, System.Predicate<TypeDefinition> filter, System.Action<TypeDefinition> action)
		{
			foreach (TypeDefinition typedef in new List<TypeDefinition>(types))
			{
				ProcessTypes(typedef.NestedTypes, filter, action);

				if (!filter(typedef)) continue;

				TraceVerbose("Entering type '{0}'", typedef);
				action(typedef);
				TraceVerbose("Leaving type '{0}'", typedef);
			}
		}
		
		protected virtual void ProcessType(TypeDefinition type)
		{
			ProcessMethods(type.Methods);
		}
		
		protected virtual void ProcessMethod(MethodDefinition method)
		{	
		}

		protected void ProcessMethods(System.Collections.IEnumerable methods)
		{
			foreach (MethodDefinition methodef in methods)
			{
				TraceVerbose("Entering method '{0}'", methodef);
				ProcessMethod(methodef);
				TraceVerbose("Leaving method '{0}'", methodef);
			}
		}
		
		protected void TraceWarning(string format, params object[] args)
		{
			_context.TraceWarning(format, args);
		}

		protected void TraceVerbose(string format, params object[] args)
		{
			_context.TraceVerbose(format, args);
		}

		protected void TraceInfo(string format, params object[] args)
		{
			_context.TraceInfo(format, args);
		}

		protected void TraceMethodBody(MethodDefinition m)
		{
			if (_context.TraceSwitch.TraceVerbose)
			{
				TraceVerbose(Cecil.FlowAnalysis.Utilities.Formatter.FormatMethodBody(m));
			}
		}

		public TypeReference Import(System.Type type)
		{
			return _context.Import(type);
		}

		public MethodReference Import(System.Reflection.MethodBase method)
		{
			return _context.Import(method);
		}
	}
}