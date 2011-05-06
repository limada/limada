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
using System.Diagnostics;
using System.Security.Principal;
using System.Threading;
using Db4objects.Db4o.Monitoring;
using Db4oUnit;

namespace Db4oTool.Tests.Core
{
	class InstallPerformanceCountersTestCase : ITestCase
	{
		public void Test()
		{
			if (!IsCurrentUserAnAdministrator() && IsLenientPerformanceCounterInstallTest())
			{
				Console.Error.WriteLine("WARNING: {0} requires administrator access rights to run.", GetType());
				return;
			}

			if (Db4oCategoryExists())
			{
				PerformanceCounterCategory.Delete(Db4oPerformanceCounters.CategoryName);
			}

			ProgramOptions options = new ProgramOptions();
			options.InstallPerformanceCounters = true;

			Db4oTool.Program.Run(options);

			Assert.IsTrue(Db4oCategoryExists());
		}

		private static bool IsLenientPerformanceCounterInstallTest()
		{
			return !string.IsNullOrEmpty(Environment.GetEnvironmentVariable("LENIENT_PERFCOUNTER_INSTALL_TEST"));
		}

		private static bool Db4oCategoryExists()
		{
			return PerformanceCounterCategory.Exists(Db4oPerformanceCounters.CategoryName);
		}

		private static bool IsCurrentUserAnAdministrator()
		{
			AppDomain.CurrentDomain.SetPrincipalPolicy(PrincipalPolicy.WindowsPrincipal);
			WindowsPrincipal principal = (WindowsPrincipal)Thread.CurrentPrincipal;
			return principal.IsInRole(WindowsBuiltInRole.Administrator);
		}
	}
}
