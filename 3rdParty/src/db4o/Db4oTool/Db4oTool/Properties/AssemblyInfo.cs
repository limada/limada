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
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security;

[assembly: AssemblyTitle("Db4oTool")]
[assembly: AssemblyDescription("Db4oTool 8.0.184.15484 (.NET)")]
[assembly: AssemblyConfiguration(".NET")]
[assembly: AssemblyCompany("Versant Corp., Redwood City, CA, USA")]
[assembly: AssemblyProduct("db4o - database for objects")]
[assembly: AssemblyCopyright("Versant Corp. 2000 - 2011")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]

[assembly: ComVisible(false)]

[assembly: AssemblyVersion("8.0.184.15484")]

[assembly: Mono.Author("Jean Baptiste Evain")]
[assembly: Mono.Author("Rodrigo B. de Oliveira")]
[assembly: Mono.Author("Klaus Wuestefeld")]
[assembly: Mono.Author("Patrick Roemer")]

[assembly: Mono.About("")]
[assembly: Mono.UsageComplement("<target>")]

[assembly: AllowPartiallyTrustedCallers]

#if NET_4_0
[assembly: SecurityRules(SecurityRuleSet.Level1)]
#endif
