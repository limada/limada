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
using System.Collections.Generic;
using System.IO;

using Db4objects.Db4o;
using Db4objects.Db4o.Internal;
using Db4objects.Db4o.Internal.Query;

namespace Db4oShell.Example
{
	class Item
	{
		string _name;
		
		public Item(string name)
		{
			_name = name;
		}
		
		public string Name
		{
			get { return _name;  }
		}
	}
	
	
	/// <summary>
	/// IMPORTANT: Look at Project Properties/Build Events. The whole point of this example is
	/// to show how to configure a CompactFramework project to be instrumented.
	/// </summary>
	class Program
	{
		static void Main(string[] args)
		{
			string dataFile = GetDataFilePath();
			if (File.Exists(dataFile)) File.Delete(dataFile);
			
			using (IObjectContainer container = Db4oFactory.OpenFile(dataFile))
			{
				((ObjectContainerBase)container).GetNativeQueryHandler().QueryExecution += new 
					QueryExecutionHandler(Program_QueryExecution);
				container.Set(new Item("Foo"));
				container.Set(new Item("Bar"));
				
				IList<Item> found = container.Query<Item>(delegate(Item candidate) { return candidate.Name.StartsWith("F"); });
				AssertEquals(1, found.Count);
				AssertEquals("Foo", found[0].Name);
			};
		}

		static void Program_QueryExecution(object sender, QueryExecutionEventArgs args)
		{
			using (StreamWriter writer = File.AppendText(GetPersonalFilePath("CFNativeQueriesEnabler.Example.txt")))
			{
				writer.WriteLine("{0} - {1}: {2}", DateTime.Now, args.ExecutionKind, args.Predicate);
			}
		}
		
		private static void AssertEquals(object expected, object actual)
		{
			if (object.Equals(expected, actual)) return;
			throw new ApplicationException(string.Format("'{0}' != '{1}'", expected, actual));
		}

		private static string GetDataFilePath()
		{
			return GetPersonalFilePath("example.yap");
		}

		private static string GetPersonalFilePath(string fname)
		{
			return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), fname);
		}
	}
}
