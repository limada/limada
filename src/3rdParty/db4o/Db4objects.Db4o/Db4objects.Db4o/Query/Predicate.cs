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
using Db4objects.Db4o.Internal;
using Db4objects.Db4o.Query;

namespace Db4objects.Db4o.Query
{
	/// <summary>Base class for native queries.</summary>
	/// <remarks>
	/// Base class for native queries.
	/// <br /><br />Native Queries provide the ability to run one or more lines
	/// of code against all instances of a class. Native query expressions should
	/// return true to mark specific instances as part of the result set.
	/// db4o will  attempt to optimize native query expressions and run them
	/// against indexes and without instantiating actual objects, where this is
	/// possible.<br /><br />
	/// The syntax of the enclosing object for the native query expression varies
	/// slightly, depending on the language version used. Here are some examples,
	/// how a simple native query will look like in some of the programming languages and
	/// dialects that db4o supports:<br /><br />
	/// <code>
	/// <b>// C# .NET 2.0</b><br />
	/// IList &lt;Cat&gt; cats = db.Query &lt;Cat&gt; (delegate(Cat cat) {<br />
	/// &#160;&#160;&#160;return cat.Name == "Occam";<br />
	/// });<br />
	/// <br />
	/// <br />
	/// <b>// Java JDK 5</b><br />
	/// List &lt;Cat&gt; cats = db.query(new Predicate&lt;Cat&gt;() {<br />
	/// &#160;&#160;&#160;public boolean match(Cat cat) {<br />
	/// &#160;&#160;&#160;&#160;&#160;&#160;return cat.getName().equals("Occam");<br />
	/// &#160;&#160;&#160;}<br />
	/// });<br />
	/// <br />
	/// <br />
	/// <b>// Java JDK 1.2 to 1.4</b><br />
	/// List cats = db.query(new Predicate() {<br />
	/// &#160;&#160;&#160;public boolean match(Cat cat) {<br />
	/// &#160;&#160;&#160;&#160;&#160;&#160;return cat.getName().equals("Occam");<br />
	/// &#160;&#160;&#160;}<br />
	/// });<br />
	/// <br />
	/// <br />
	/// <b>// Java JDK 1.1</b><br />
	/// ObjectSet cats = db.query(new CatOccam());<br />
	/// <br />
	/// public static class CatOccam extends Predicate {<br />
	/// &#160;&#160;&#160;public boolean match(Cat cat) {<br />
	/// &#160;&#160;&#160;&#160;&#160;&#160;return cat.getName().equals("Occam");<br />
	/// &#160;&#160;&#160;}<br />
	/// });<br />
	/// <br />
	/// <br />
	/// <b>// C# .NET 1.1</b><br />
	/// IList cats = db.Query(new CatOccam());<br />
	/// <br />
	/// public class CatOccam : Predicate {<br />
	/// &#160;&#160;&#160;public boolean Match(Cat cat) {<br />
	/// &#160;&#160;&#160;&#160;&#160;&#160;return cat.Name == "Occam";<br />
	/// &#160;&#160;&#160;}<br />
	/// });<br />
	/// </code>
	/// <br />
	/// Summing up the above:<br />
	/// In order to run a Native Query, you can<br />
	/// - use the delegate notation for .NET 2.0.<br />
	/// - extend the Predicate class for all other language dialects<br /><br />
	/// A class that extends Predicate is required to
	/// implement the #match() / #Match() method, following the native query
	/// conventions:<br />
	/// - The name of the method is "#match()" (Java) / "#Match()" (.NET).<br />
	/// - The method must be public public.<br />
	/// - The method returns a boolean.<br />
	/// - The method takes one parameter.<br />
	/// - The Type (.NET) / Class (Java) of the parameter specifies the extent.<br />
	/// - For all instances of the extent that are to be included into the
	/// resultset of the query, the match method should return true. For all
	/// instances that are not to be included, the match method should return
	/// false.<br /><br />
	/// </remarks>
	[System.Serializable]
	public abstract class Predicate
	{
		/// <summary>public for implementation reasons, please ignore.</summary>
		/// <remarks>public for implementation reasons, please ignore.</remarks>
		public static readonly string PredicatemethodName = "match";

		private Type _extentType;

		[System.NonSerialized]
		private MethodInfo cachedFilterMethod = null;

		public Predicate() : this(null)
		{
		}

		public Predicate(Type extentType)
		{
			_extentType = extentType;
		}

		public virtual MethodInfo GetFilterMethod()
		{
			if (cachedFilterMethod != null)
			{
				return cachedFilterMethod;
			}
			MethodInfo[] methods = GetType().GetMethods();
			for (int methodIdx = 0; methodIdx < methods.Length; methodIdx++)
			{
				MethodInfo method = methods[methodIdx];
				if ((!method.Name.Equals(PredicatePlatform.PredicatemethodName)) || Sharpen.Runtime.GetParameterTypes
					(method).Length != 1)
				{
					continue;
				}
				cachedFilterMethod = method;
				string targetName = Sharpen.Runtime.GetParameterTypes(method)[0].FullName;
				if (!"java.lang.Object".Equals(targetName))
				{
					break;
				}
			}
			if (cachedFilterMethod == null)
			{
				throw new ArgumentException("Invalid predicate.");
			}
			return cachedFilterMethod;
		}

		/// <summary>public for implementation reasons, please ignore.</summary>
		/// <remarks>public for implementation reasons, please ignore.</remarks>
		public virtual Type ExtentType()
		{
			if (_extentType == null)
			{
				_extentType = FilterParameterType();
			}
			return _extentType;
		}

		private Type FilterParameterType()
		{
			return (Type)Sharpen.Runtime.GetParameterTypes(GetFilterMethod())[0];
		}

		/// <summary>public for implementation reasons, please ignore.</summary>
		/// <remarks>public for implementation reasons, please ignore.</remarks>
		public virtual bool AppliesTo(object candidate)
		{
			try
			{
				MethodInfo filterMethod = GetFilterMethod();
				Platform4.SetAccessible(filterMethod);
				object ret = filterMethod.Invoke(this, new object[] { candidate });
				return ((bool)ret);
			}
			catch (Exception)
			{
				// TODO: log this exception somewhere?
				//			e.printStackTrace();
				return false;
			}
		}
	}
}
