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
using Db4objects.Db4o.Diagnostic;

namespace Db4objects.Db4o.Diagnostic
{
	public class NativeQueryOptimizerNotLoaded : DiagnosticBase
	{
		private int _reason;

		private readonly Exception _details;

		public const int NqNotPresent = 1;

		public const int NqConstructionFailed = 2;

		public NativeQueryOptimizerNotLoaded(int reason, Exception details)
		{
			_reason = reason;
			_details = details;
		}

		public override string Problem()
		{
			return "Native Query Optimizer could not be loaded";
		}

		public override object Reason()
		{
			switch (_reason)
			{
				case NqNotPresent:
				{
					return AppendDetails("Native query not present.");
				}

				case NqConstructionFailed:
				{
					return AppendDetails("Native query couldn't be instantiated.");
				}

				default:
				{
					return AppendDetails("Reason not specified.");
					break;
				}
			}
		}

		public override string Solution()
		{
			return "If you to have the native queries optimized, please check that the native query jar is present in the class-path.";
		}

		private object AppendDetails(string reason)
		{
			if (_details == null)
			{
				return reason;
			}
			return reason + "\n" + _details.ToString();
		}
	}
}
