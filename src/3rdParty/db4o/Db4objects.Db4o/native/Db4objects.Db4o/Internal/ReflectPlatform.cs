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
using Sharpen.Lang;

namespace Db4objects.Db4o.Internal
{
	public class ReflectPlatform
	{
		public static Type ForName(string typeName)
		{
			try
			{
				return TypeReference.FromString(typeName).Resolve();
			}
			catch
			{
				return null;
			}
		}

		public static object CreateInstance(string typeName)
		{
            return ReflectPlatform.CreateInstance(ForName(typeName));
		}

        public static object CreateInstance(Type type)
        {
            try
            {
                return Activator.CreateInstance(type);
            }
            catch
            {
                return null;
            }
        }

	    public static string FullyQualifiedName(Type type)
	    {
	        return TypeReference.FromType(type).GetUnversionedName();
	    }

	    public static bool IsNamedClass(Type type)
	    {
	        return true;
	    }

        public static string SimpleName(Type type)
        {
            return type.Name;
        }
    }
}
