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
namespace Db4objects.Db4o.Diagnostic
{
	/// <summary>
	/// Marker interface for Diagnostic messages<br /><br />
	/// Diagnostic system can be enabled on a running db4o database
	/// to notify a user about possible problems or misconfigurations.
	/// </summary>
	/// <remarks>
	/// Marker interface for Diagnostic messages<br /><br />
	/// Diagnostic system can be enabled on a running db4o database
	/// to notify a user about possible problems or misconfigurations. Diagnostic
	/// messages must implement this interface and are usually derived from
	/// <see cref="DiagnosticBase">DiagnosticBase</see>
	/// class. A separate Diagnostic implementation
	/// should be used for each problem.
	/// </remarks>
	/// <seealso cref="DiagnosticBase">DiagnosticBase</seealso>
	/// <seealso cref="IDiagnosticConfiguration">IDiagnosticConfiguration</seealso>
	public interface IDiagnostic
	{
	}
}
