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
using System.Collections.Generic;
using System.Diagnostics;

namespace Db4oTool.Core
{
	public class Configuration
	{
		private bool _caseSensitive;
		private readonly string _assemblyLocation;
		private readonly TraceSwitch _traceSwitch = new TraceSwitch("Db4oTool", "Db4oTool tracing level");
	    private readonly List<ITypeFilter> _filters = new List<ITypeFilter>();
		private bool _preserveDebugInfo;
		
		public Configuration(string assemblyLocation)
		{
			_assemblyLocation = assemblyLocation;
			_traceSwitch.Level = TraceLevel.Warning;
		}

		public bool CaseSensitive
		{
			get { return _caseSensitive; }
			set { _caseSensitive = value; }
		}

		public string AssemblyLocation
		{
			get { return _assemblyLocation; }
		}

		public TraceSwitch TraceSwitch
		{
			get { return _traceSwitch; }
		}

		public bool PreserveDebugInfo
		{
			get { return _preserveDebugInfo; }
			set { _preserveDebugInfo = value; }
		}

		public void AddFilter(ITypeFilter filter)
	    {
            if (null == filter) throw new ArgumentNullException("filter");
	        _filters.Add(filter);
	    }

        public bool Accept(Mono.Cecil.TypeDefinition typedef)
        {
            if (_filters.Count == 0) return true;
            foreach (ITypeFilter filter in _filters)
            {
                if (filter.Accept(typedef)) return true;
            }
            return false;
        }
    }
}
