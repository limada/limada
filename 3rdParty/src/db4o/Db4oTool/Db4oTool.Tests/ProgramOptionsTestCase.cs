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
using Db4oUnit;

namespace Db4oTool.Tests
{
	class ProgramOptionsTestCase : ITestCase
	{	
		public void TestTpSameAsTA()
		{
			AssertTransparentPersistence("-tp");
			AssertTransparentPersistence("-ta");
		}

		public void TestInstallPerformanceCounters()
		{
			ProgramOptions options = new ProgramOptions();
			Assert.IsFalse(options.InstallPerformanceCounters);
			options.ProcessArgs(new string[] { "--install-performance-counters"});
			Assert.IsTrue(options.InstallPerformanceCounters);
		}

        public void TestInvalidOptionsCombination()
        {
            AssertInvalidOptionsCombination("-nq -fileusage mytarget");
            AssertInvalidOptionsCombination("-nq -check mytarget");
            AssertInvalidOptionsCombination("-ta -fileusage mytarget");
            AssertInvalidOptionsCombination("-ta -check mytarget");
            AssertInvalidOptionsCombination("-tp -fileusage mytarget");
        }

	    private void AssertInvalidOptionsCombination(string arguments)
	    {
            ProgramOptions options = new ProgramOptions();
            options.ProcessArgs(arguments.Split(' '));
	        Assert.IsFalse(options.IsValid);
	    }

	    private static void AssertTransparentPersistence(string arg)
		{
			ProgramOptions options = new ProgramOptions();
			Assert.IsFalse(options.TransparentPersistence);
			options.ProcessArgs(new string[] { arg });
			Assert.IsTrue(options.TransparentPersistence);
		}
	}
}
