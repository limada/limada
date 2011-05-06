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
using Db4oUnit.Fixtures;

namespace Db4oTool.Tests.Core
{
	sealed class DebugInformationTestVariables
	{
		public static readonly FixtureVariable SourceAvailable = new FixtureVariable("Source");
		public static readonly FixtureVariable DebugSymbolsAvailable = new FixtureVariable("DebugSymbols");

		public static readonly IFixtureProvider SourceFixtureProvider = new SimpleFixtureProvider(SourceAvailable, 
																					new object[]
																						{
																							LabeledBool.Create(true, "Available"), 
																							LabeledBool.Create(false, "Unavailable") 
																						});

		public static readonly IFixtureProvider DebugSymbolsFixtureProvider = new SimpleFixtureProvider(DebugSymbolsAvailable, 
																					new object[]
																						{
																							LabeledBool.Create(true, "Available"), 
																							LabeledBool.Create(false, "Unavailable") 
																						});

		public static bool TestWithSourceAvailable()
		{
			return (LabeledBool) SourceAvailable.Value;
		}

		public static bool TestWithDebugSymbolsAvailable()
		{
			return (LabeledBool) DebugSymbolsAvailable.Value;
		}
	}

	sealed class LabeledBool : ILabeled
	{
		public static LabeledBool Create(bool value, string label)
		{
			return new LabeledBool(value, label);
		}

		public string Label()
		{
			return _label;
		}
		
		public static implicit operator bool(LabeledBool source)
		{
			return source._value;
		}

		private LabeledBool(bool value, string label)
		{
			_value = value;
			_label = label;
		}
	
		private readonly string _label;
		private readonly bool _value;
	}
}
