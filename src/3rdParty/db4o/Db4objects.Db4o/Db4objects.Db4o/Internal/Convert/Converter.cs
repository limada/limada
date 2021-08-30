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
using System.Collections;
using Db4objects.Db4o.Internal.Convert;
using Db4objects.Db4o.Internal.Convert.Conversions;

namespace Db4objects.Db4o.Internal.Convert
{
	/// <exclude></exclude>
	public class Converter
	{
		public const int Version = VersionNumberToCommitTimestamp_8_0.Version;

		public static bool Convert(ConversionStage stage)
		{
			if (!NeedsConversion(stage.ConverterVersion()))
			{
				return false;
			}
			return Instance().RunConversions(stage);
		}

		private static Db4objects.Db4o.Internal.Convert.Converter _instance;

		private IDictionary _conversions;

		private int _minimumVersion = int.MaxValue;

		private Converter()
		{
			_conversions = new Hashtable();
			// TODO: There probably will be Java and .NET conversions
			//       Create Platform4.registerConversions() method ann
			//       call from here when needed.
			CommonConversions.Register(this);
		}

		public static Db4objects.Db4o.Internal.Convert.Converter Instance()
		{
			if (_instance == null)
			{
				_instance = new Db4objects.Db4o.Internal.Convert.Converter();
			}
			return _instance;
		}

		public virtual Conversion ConversionFor(int version)
		{
			return ((Conversion)_conversions[version]);
		}

		private static bool NeedsConversion(int converterVersion)
		{
			return converterVersion < Version;
		}

		public virtual void Register(int introducedVersion, Conversion conversion)
		{
			if (_conversions.Contains(introducedVersion))
			{
				throw new InvalidOperationException();
			}
			if (introducedVersion < _minimumVersion)
			{
				_minimumVersion = introducedVersion;
			}
			_conversions[introducedVersion] = conversion;
		}

		public virtual bool RunConversions(ConversionStage stage)
		{
			int startingVersion = Math.Max(stage.ConverterVersion() + 1, _minimumVersion);
			for (int version = startingVersion; version <= Version; version++)
			{
				Conversion conversion = ConversionFor(version);
				if (conversion == null)
				{
					throw new InvalidOperationException("Could not find a conversion for version " + 
						version);
				}
				stage.Accept(conversion);
			}
			return true;
		}
	}
}
