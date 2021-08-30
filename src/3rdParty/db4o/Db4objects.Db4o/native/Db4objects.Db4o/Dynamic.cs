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
using System.Reflection;

namespace Db4objects.Db4o {

	/// <exclude />
    public class Dynamic {

		private const BindingFlags AllMembers = BindingFlags.Static | BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public;

        public static object GetProperty(object obj, string prop){
            if(obj != null){
                Type type = TypeForObject(obj);
                try {
                    PropertyInfo pi = type.GetProperty(prop, AllMembers);
                    return pi.GetValue(obj,null);
                } catch {
                }
            }
            return null;
        }

        public static void SetProperty(object obj, string prop, object val){
            if(obj != null){
                Type type = TypeForObject(obj);
                try {
                    PropertyInfo pi = type.GetProperty(prop, AllMembers);
                    pi.SetValue(obj, val, null);
                } catch {
                }
            }
        }

        private static Type TypeForObject(object obj){
            Type type = obj as Type;
            if(type != null){
                return type;
            }
            return obj.GetType();
        }
    }
}
