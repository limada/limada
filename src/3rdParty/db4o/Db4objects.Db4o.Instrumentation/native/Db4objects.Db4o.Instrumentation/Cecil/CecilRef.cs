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
using Db4objects.Db4o.Instrumentation.Api;
using Mono.Cecil;

namespace Db4objects.Db4o.Instrumentation.Cecil
{
	public class CecilRef<T> where T : MemberReference
	{
		public static T GetReference(object type)
		{
			return ((CecilRef<T>)type).Reference;
		}

		private readonly CecilReferenceProvider _provider;
		protected T _reference;

	    public CecilRef(CecilReferenceProvider provider, T reference)
		{
			_provider = provider;
			_reference = reference;
		}

		protected ITypeRef TypeRef(TypeReference type)
		{
			return _provider.ForCecilType(type);
		}

		public T Reference
		{
			get { return _reference; }
		    set { _reference = value; }
		}

		public virtual string Name
		{
			get { return _reference.Name;  }
		}

		public override string ToString()
		{
			return Name;
		}
	}
}
