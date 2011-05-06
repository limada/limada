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
using System.IO;
using Db4oTool.Tests.Core;
using Db4objects.Db4o.Internal.Query;
using Db4oUnit;

namespace Db4oTool.Tests.NQ
{
	// TODO: generate evaluation based queries for unoptimizable predicates
	// so they don't need to be analyzed again in runtime
	// TODO: report unoptimizable through the API as a Warning object
    public class UnoptimizablePredicatesTestCase : SingleResourceTestCase
	{
		protected override string ResourceName
		{
			get { return "UnoptimizablePredicateSubject"; }
		}

		protected override string CommandLine
		{
			get { return "-nq"; }
		}

		protected override void CheckInstrumentationOutput(ShellUtilities.ProcessOutput output)
		{
			base.CheckInstrumentationOutput(output);
			Assert.AreEqual(
				"WARNING: Predicate 'System.Boolean ByUpperNameUnoptimizable::Match(Item)' could not be optimized. Unsupported expression: candidate.get_Name().ToUpper",
				output.StdErr.Trim());
			Assert.AreEqual("", output.StdOut);
		}

		protected override void OnQueryExecution(object sender, QueryExecutionEventArgs args)
		{
			if (args.Predicate.GetType().Name.EndsWith("Unoptimizable"))
			{
				Assert.AreEqual(QueryExecutionKind.Unoptimized, args.ExecutionKind);
			}
			else
			{
				Assert.AreEqual(QueryExecutionKind.PreOptimized, args.ExecutionKind);
			}
		}

		protected override void OnQueryOptimizationFailure(object sender, QueryOptimizationFailureEventArgs args)
		{
			// do nothing as we expect some predicate to fail
		}
	}
}
