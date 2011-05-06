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
using System.Diagnostics;
using System.IO;
using System.Text;
using Db4oUnit;

namespace Db4oTool.Tests.TA
{
	abstract class TAOutputListenerTestCaseBase : TATestCaseBase
	{
		protected void InstrumentAndAssert(string assemblyPath, bool shouldContain, params string[] expectedMessages)
		{
			InstrumentAndAssert(assemblyPath, "-ta -v", shouldContain, expectedMessages);
		}

		protected void InstrumentAndAssert(string assemblyPath, string db4oToolOptions, bool shouldContain, params string[] expectedMessages)
		{
			TraceListener listener = new TraceListener();
			Trace.Listeners.Add(listener);

			RunDb4oTool(db4oToolOptions, assemblyPath);

			Trace.Listeners.Remove(listener);
			Assert.AreEqual(expectedMessages.Length, listener.Contents.Count, Zip(listener.Contents));
			foreach (string message in expectedMessages)
			{
				Assert.AreEqual(shouldContain, Contains(listener.Contents, message));
			}
		}

		private static void RunDb4oTool(string db4oToolOptions, string assemblyPath)
		{
			TextWriter previousError = Console.Error;
			StringWriter error = new StringWriter();

			Console.SetError(error);
			Assert.AreEqual(0, Db4oTool.Program.Main(CreateOptionsArrayFor(db4oToolOptions, assemblyPath)), error.ToString());
			Console.SetError(previousError);
		}

		private static string[] CreateOptionsArrayFor(string db4oToolOptions, string assemblyPath)
		{
			List<string> options = new List<string>(db4oToolOptions.Split(' '));
			options.Add(assemblyPath);
			return options.ToArray();
		}

		private static string Zip(IEnumerable<string> contents)
		{
			StringBuilder sb = new StringBuilder();
			foreach (string item in contents)
			{
				sb.AppendLine(item);
			}

			return sb.ToString();
		}

		private static bool Contains(IEnumerable<string> contents, string message)
		{
			foreach (string item in contents)
			{
				if (item.Contains(message)) return true;
			}

			return false;
		}
	}
}
