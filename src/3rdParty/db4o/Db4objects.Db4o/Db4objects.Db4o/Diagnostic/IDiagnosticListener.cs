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
using Db4objects.Db4o.Diagnostic;

namespace Db4objects.Db4o.Diagnostic
{
	/// <summary>listens to Diagnostic messages.</summary>
	/// <remarks>
	/// listens to Diagnostic messages.
	/// <br/><br/>Create a class that implements this listener interface and add
	/// the listener by calling <code>commonConfig.Diagnostic.AddListener()</code>.
	/// </remarks>
	/// <seealso cref="Db4objects.Db4o.Diagnostic.IDiagnosticConfiguration">Db4objects.Db4o.Diagnostic.IDiagnosticConfiguration
	/// </seealso>
	public interface IDiagnosticListener
	{
		/// <summary>this method will be called with Diagnostic messages.</summary>
		/// <remarks>this method will be called with Diagnostic messages.</remarks>
		void OnDiagnostic(IDiagnostic d);
	}
}
