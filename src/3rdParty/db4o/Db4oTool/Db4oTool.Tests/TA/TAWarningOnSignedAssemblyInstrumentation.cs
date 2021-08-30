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
using Db4oTool.Tests.Core;
using Db4oUnit;
using Mono.Cecil;

namespace Db4oTool.Tests.TA
{
	class TAWarningOnSignedAssemblyInstrumentation : TAOutputListenerTestCaseBase
	{
		public void TestDelaySign()
		{
			AssertInstrumentingSignedAssembly(true, true);
		}

		public void TestSignedAssembly()
		{
			AssertInstrumentingSignedAssembly(true, false);
		}

		public void TestNoWarningForUnsignedAssemblies()
		{
			AssertInstrumentingSignedAssembly(false, false);
		}

		private void AssertInstrumentingSignedAssembly(bool sign, bool delaySign)
		{
			string signKeyPath = GenerateKeyToSign();
			AssemblyDefinition assembly = null;

			if (sign)
			{
				CompilationServices.KeyFile.Using(
					new SignConfiguration(signKeyPath, delaySign),
					delegate
						{
							assembly = GenerateAssembly(ResourceName);
						});
			}
			else
			{
				assembly = GenerateAssembly(ResourceName);
			}

			Assert.AreEqual(sign, assembly.Name.HasPublicKey);
			string[] messages = sign ? new string[] { "has been signed" } : new string[0];
			InstrumentAndAssert(assembly.MainModule.FullyQualifiedName, sign, messages);
		}

		private static string GenerateKeyToSign()
		{
			string keyPath = Path.Combine(Path.GetTempPath(), "db4otool-test.skn");
			if (!File.Exists(keyPath))
			{
				ProcessStartInfo psi = new ProcessStartInfo("sn.exe", "-k " + AppendDoubleQuotationMarks(keyPath));
				psi.RedirectStandardOutput = true;
				psi.RedirectStandardError = true;
				psi.UseShellExecute = false;

				Process sn = Process.Start(psi);

				if (sn == null)
				{
					Assert.Fail("Failed to generate a key for testing...");
				}
				else
				{
					sn.WaitForExit();
				}

				//string output = sn.StandardOutput.ReadToEnd();
			}
			return AppendDoubleQuotationMarks(keyPath);
		}

		private static string AppendDoubleQuotationMarks(string path)
		{
			return "\"" + path + "\"";
		}

		private const string ResourceName = "TASignedAssemblySubject";
	}

	internal class TraceListener : System.Diagnostics.TraceListener
	{
		private readonly IList<string> _messages = new List<string>();

		public override void Write(string message)
		{
			_messages.Add(message);
		}

		public override void WriteLine(string message)
		{
			Write(message + Environment.NewLine);
		}

		public IList<string> Contents
		{
			get { return _messages; }
		}
	}
}
