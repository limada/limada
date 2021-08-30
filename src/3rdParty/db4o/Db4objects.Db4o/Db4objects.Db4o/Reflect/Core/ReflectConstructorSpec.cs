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
using Db4objects.Db4o.Foundation;
using Db4objects.Db4o.Reflect.Core;

namespace Db4objects.Db4o.Reflect.Core
{
	/// <summary>
	/// a spec holding a constructor, it's arguments
	/// and information, if the constructor can instantiate
	/// objects.
	/// </summary>
	/// <remarks>
	/// a spec holding a constructor, it's arguments
	/// and information, if the constructor can instantiate
	/// objects.
	/// </remarks>
	public class ReflectConstructorSpec
	{
		private IReflectConstructor _constructor;

		private object[] _args;

		private TernaryBool _canBeInstantiated;

		public static readonly Db4objects.Db4o.Reflect.Core.ReflectConstructorSpec UnspecifiedConstructor
			 = new Db4objects.Db4o.Reflect.Core.ReflectConstructorSpec(TernaryBool.Unspecified
			);

		public static readonly Db4objects.Db4o.Reflect.Core.ReflectConstructorSpec InvalidConstructor
			 = new Db4objects.Db4o.Reflect.Core.ReflectConstructorSpec(TernaryBool.No);

		public ReflectConstructorSpec(IReflectConstructor constructor, object[] args)
		{
			_constructor = constructor;
			_args = args;
			_canBeInstantiated = TernaryBool.Yes;
		}

		private ReflectConstructorSpec(TernaryBool canBeInstantiated)
		{
			_canBeInstantiated = canBeInstantiated;
			_constructor = null;
		}

		/// <summary>creates a new instance.</summary>
		/// <remarks>creates a new instance.</remarks>
		/// <returns>the newly created instance.</returns>
		public virtual object NewInstance()
		{
			if (_constructor == null)
			{
				return null;
			}
			return _constructor.NewInstance(_args);
		}

		/// <summary>
		/// returns true if an instance can be instantiated
		/// with the constructor, otherwise false.
		/// </summary>
		/// <remarks>
		/// returns true if an instance can be instantiated
		/// with the constructor, otherwise false.
		/// </remarks>
		public virtual TernaryBool CanBeInstantiated()
		{
			return _canBeInstantiated;
		}
	}
}
