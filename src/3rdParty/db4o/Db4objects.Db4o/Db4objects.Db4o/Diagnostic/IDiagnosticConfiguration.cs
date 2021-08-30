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
	/// <summary>provides methods to configure the behaviour of db4o
	/// diagnostics.</summary>
	/// <remarks>
	/// provides methods to configure the behaviour of db4o diagnostics.
	/// <br/>
	/// <br/>
	/// Diagnostic system can be enabled on a running db4o database to
	/// notify a user about possible problems or misconfigurations.
	/// Diagnostic listeners can be be added and removed with calls to this
	/// interface. To install the most basic listener call:
	/// <br/>
	/// <code>commonConfig.Diagnostic.AddListener(new
	/// DiagnosticToConsole());</code>
	/// </remarks>
	/// <seealso cref="IConfiguration.Diagnostic">IConfiguration.Diagnostic
	/// </seealso>
	/// <seealso cref="IDiagnosticListener">IDiagnosticListener
	/// </seealso>
	public interface IDiagnosticConfiguration
	{
		/// <summary>adds a DiagnosticListener to listen to Diagnostic messages.</summary>
		/// <remarks>adds a DiagnosticListener to listen to Diagnostic messages.</remarks>
		void AddListener(IDiagnosticListener listener);

		/// <summary>removes all DiagnosticListeners.</summary>
		/// <remarks>removes all DiagnosticListeners.</remarks>
		void RemoveAllListeners();
	}
}
