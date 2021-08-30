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
using Db4objects.Db4o.Config;
using Db4objects.Db4o.Internal;
using Db4objects.Db4o.Internal.Activation;
using Db4objects.Db4o.TA;

namespace Db4objects.Db4o.TA
{
	/// <summary>
	/// Enables Transparent Persistence and Transparent Activation behaviours for
	/// the current session.
	/// </summary>
	/// <remarks>
	/// Enables Transparent Persistence and Transparent Activation behaviours for
	/// the current session.
	/// <br/><br/>
	/// commonConfiguration.Add(new TransparentPersistenceSupport());
	/// </remarks>
	/// <seealso cref="Db4objects.Db4o.TA.TransparentActivationSupport">Db4objects.Db4o.TA.TransparentActivationSupport
	/// </seealso>
	public class TransparentPersistenceSupport : TransparentActivationSupport
	{
		private readonly IRollbackStrategy _rollbackStrategy;

		/// <summary>Creates a new instance of TransparentPersistenceSupport class</summary>
		/// <param name="rollbackStrategy">
		/// RollbackStrategy interface implementation, which
		/// defines the actions to be taken on the object when the transaction is rolled back.
		/// </param>
		public TransparentPersistenceSupport(IRollbackStrategy rollbackStrategy)
		{
			_rollbackStrategy = rollbackStrategy;
		}

		/// <summary>
		/// Creates a new instance of TransparentPersistenceSupport class
		/// with no rollback strategies defined.
		/// </summary>
		/// <remarks>
		/// Creates a new instance of TransparentPersistenceSupport class
		/// with no rollback strategies defined.
		/// </remarks>
		public TransparentPersistenceSupport() : this(null)
		{
		}

		/// <summary>Configures current ObjectContainer to support Transparent Activation and Transparent Persistence
		/// 	</summary>
		/// <seealso cref="TransparentActivationSupport.Apply(Db4objects.Db4o.Internal.IInternalObjectContainer)
		/// 	"></seealso>
		public override void Apply(IInternalObjectContainer container)
		{
			base.Apply(container);
			EnableTransparentPersistenceFor(container);
		}

		private void EnableTransparentPersistenceFor(IInternalObjectContainer container)
		{
			ITransparentActivationDepthProvider provider = (ITransparentActivationDepthProvider
				)ActivationProvider(container);
			provider.EnableTransparentPersistenceSupportFor(container, _rollbackStrategy);
		}

		public override void Prepare(IConfiguration configuration)
		{
			base.Prepare(configuration);
			((Config4Impl)configuration).UpdateDepthProvider(new TPUpdateDepthProvider());
		}
	}
}
