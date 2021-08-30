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
namespace Db4objects.Db4o.Reflect.Net
{

	/// <remarks>Reflection implementation for Constructor to map to .NET reflection.</remarks>
	public class NetConstructor : Db4objects.Db4o.Reflect.Core.IReflectConstructor
	{
		private readonly Db4objects.Db4o.Reflect.IReflector reflector;

		private readonly System.Reflection.ConstructorInfo constructor;

		public NetConstructor(Db4objects.Db4o.Reflect.IReflector reflector, System.Reflection.ConstructorInfo
			 constructor)
		{
			this.reflector = reflector;
			this.constructor = constructor;
			Db4objects.Db4o.Internal.Platform4.SetAccessible(constructor);
		}

		public virtual Db4objects.Db4o.Reflect.IReflectClass[] GetParameterTypes()
		{
			return Db4objects.Db4o.Reflect.Net.NetReflector.ToMeta(reflector, Sharpen.Runtime.GetParameterTypes(constructor));
		}

		public virtual object NewInstance(object[] parameters)
		{
			try
			{
				return constructor.Invoke(parameters);
			}
			catch
			{
				return null;
			}
		}
	}
}
