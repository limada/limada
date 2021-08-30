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
using System.Text;
using Db4objects.Db4o.Foundation;
using Db4objects.Db4o.Internal;
using Sharpen;

namespace Db4objects.Db4o.Internal
{
	/// <exclude>
	/// Use the methods in this class for system classes only, since they
	/// are not ClassLoader or Reflector-aware.
	/// TODO: this class should go to foundation.reflect, along with ReflectException and ReflectPlatform
	/// </exclude>
	public class Reflection4
	{
		public static object InvokeStatic(Type clazz, string methodName)
		{
			return Invoke(clazz, methodName, null, null, null);
		}

		/// <exception cref="Db4objects.Db4o.Internal.ReflectException"></exception>
		public static object Invoke(object obj, string methodName)
		{
			return Invoke(obj.GetType(), methodName, null, null, obj);
		}

		/// <exception cref="Db4objects.Db4o.Internal.ReflectException"></exception>
		public static object Invoke(object obj, string methodName, object[] @params)
		{
			Type[] paramClasses = new Type[@params.Length];
			for (int i = 0; i < @params.Length; i++)
			{
				paramClasses[i] = @params[i].GetType();
			}
			return Invoke(obj.GetType(), methodName, paramClasses, @params, obj);
		}

		/// <exception cref="Db4objects.Db4o.Internal.ReflectException"></exception>
		public static object Invoke(object obj, string methodName, Type[] paramClasses, object
			[] @params)
		{
			return Invoke(obj.GetType(), methodName, paramClasses, @params, obj);
		}

		/// <exception cref="Db4objects.Db4o.Internal.ReflectException"></exception>
		public static object Invoke(Type clazz, string methodName, Type[] paramClasses, object
			[] @params)
		{
			return Invoke(clazz, methodName, paramClasses, @params, null);
		}

		private static object Invoke(Type clazz, string methodName, Type[] paramClasses, 
			object[] @params, object onObject)
		{
			return Invoke(@params, onObject, GetMethod(clazz, methodName, paramClasses));
		}

		/// <exception cref="Db4objects.Db4o.Internal.ReflectException"></exception>
		public static object Invoke(string className, string methodName, Type[] paramClasses
			, object[] @params, object onObject)
		{
			MethodInfo method = GetMethod(className, methodName, paramClasses);
			return Invoke(@params, onObject, method);
		}

		/// <exception cref="Db4objects.Db4o.Internal.ReflectException"></exception>
		public static object Invoke(object[] @params, object onObject, MethodInfo method)
		{
			if (method == null)
			{
				return null;
			}
			Platform4.SetAccessible(method);
			try
			{
				return method.Invoke(onObject, @params);
			}
			catch (TargetInvocationException e)
			{
				throw new ReflectException(e.InnerException);
			}
			catch (ArgumentException e)
			{
				throw new ReflectException(e);
			}
			catch (MemberAccessException e)
			{
				throw new ReflectException(e);
			}
		}

		/// <summary>calling this method "method" will break C# conversion with the old converter
		/// 	</summary>
		public static MethodInfo GetMethod(string className, string methodName, Type[] paramClasses
			)
		{
			Type clazz = ReflectPlatform.ForName(className);
			if (clazz == null)
			{
				return null;
			}
			return GetMethod(clazz, methodName, paramClasses);
		}

		public static MethodInfo GetMethod(Type clazz, string methodName, Type[] paramClasses
			)
		{
			Type curclazz = clazz;
			while (curclazz != null)
			{
				try
				{
					return Sharpen.Runtime.GetDeclaredMethod(curclazz, methodName, paramClasses);
				}
				catch (Exception)
				{
				}
				curclazz = curclazz.BaseType;
			}
			return null;
		}

		/// <exception cref="Db4objects.Db4o.Internal.ReflectException"></exception>
		public static object Invoke(object obj, string methodName, Type signature, object
			 value)
		{
			return Invoke(obj, methodName, new Type[] { signature }, new object[] { value });
		}

		public static FieldInfo GetField(Type clazz, string name)
		{
			Type curclazz = clazz;
			while (curclazz != null)
			{
				try
				{
					FieldInfo field = Sharpen.Runtime.GetDeclaredField(curclazz, name);
					Platform4.SetAccessible(field);
					if (field != null)
					{
						return field;
					}
				}
				catch (Exception)
				{
				}
				curclazz = curclazz.BaseType;
			}
			return null;
		}

		/// <exception cref="Db4objects.Db4o.Internal.ReflectException"></exception>
		public static object GetFieldValue(object obj, string fieldName)
		{
			try
			{
				return GetField(obj.GetType(), fieldName).GetValue(obj);
			}
			catch (Exception e)
			{
				throw new ReflectException(e);
			}
		}

		public static object NewInstance(object template)
		{
			try
			{
				return System.Activator.CreateInstance(template.GetType());
			}
			catch (Exception e)
			{
				throw new ReflectException(e);
			}
		}

		public static string Dump(object obj)
		{
			return DumpPreventRecursion(obj, new IdentitySet4(), 2);
		}

		private static string DumpPreventRecursion(object obj, IdentitySet4 dumped, int stackLimit
			)
		{
			stackLimit--;
			if (obj == null)
			{
				return "null";
			}
			Type clazz = obj.GetType();
			if (Platform4.IsSimple(clazz))
			{
				return obj.ToString();
			}
			StringBuilder sb = new StringBuilder();
			sb.Append(clazz.FullName);
			sb.Append(" (");
			sb.Append(Runtime.IdentityHashCode(obj));
			sb.Append(")");
			if (dumped.Contains(obj) || stackLimit <= 0)
			{
				return sb.ToString();
			}
			dumped.Add(obj);
			FieldInfo[] fields = Sharpen.Runtime.GetDeclaredFields(clazz);
			for (int fieldIndex = 0; fieldIndex < fields.Length; ++fieldIndex)
			{
				FieldInfo field = fields[fieldIndex];
				Platform4.SetAccessible(field);
				try
				{
					if (field.GetValue(null) == field.GetValue(obj))
					{
						continue;
					}
				}
				catch (Exception)
				{
				}
				// static field.getModifiers() wouldn't sharpen 
				sb.Append("\n");
				sb.Append("\t");
				sb.Append(field.Name);
				sb.Append(": ");
				try
				{
					sb.Append(DumpPreventRecursion(field.GetValue(obj), dumped, stackLimit));
				}
				catch (Exception e)
				{
					sb.Append("Exception caught: ");
					sb.Append(e);
				}
			}
			return sb.ToString();
		}
	}
}
