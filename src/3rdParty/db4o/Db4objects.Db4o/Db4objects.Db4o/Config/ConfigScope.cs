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
using Db4objects.Db4o.Foundation;

namespace Db4objects.Db4o.Config
{
	/// <summary>
	/// Defines a scope of applicability of a config setting.<br /><br />
	/// Some of the configuration settings can be either: <br /><br />
	/// - enabled globally; <br />
	/// - enabled individually for a specified class; <br />
	/// - disabled.<br /><br />
	/// </summary>
	/// <seealso cref="IConfiguration.GenerateUUIDs(ConfigScope)">IConfiguration.GenerateUUIDs(ConfigScope)
	/// 	</seealso>
	/// <seealso cref="IConfiguration.GenerateVersionNumbers(ConfigScope)">IConfiguration.GenerateVersionNumbers(ConfigScope)
	/// 	</seealso>
	[System.Serializable]
	public sealed class ConfigScope
	{
		public const int DisabledId = -1;

		public const int IndividuallyId = 1;

		public const int GloballyId = int.MaxValue;

		private static readonly string DisabledName = "disabled";

		private static readonly string IndividuallyName = "individually";

		private static readonly string GloballyName = "globally";

		/// <summary>Marks a configuration feature as globally disabled.</summary>
		/// <remarks>Marks a configuration feature as globally disabled.</remarks>
		public static readonly Db4objects.Db4o.Config.ConfigScope Disabled = new Db4objects.Db4o.Config.ConfigScope
			(DisabledId, DisabledName);

		/// <summary>Marks a configuration feature as individually configurable.</summary>
		/// <remarks>Marks a configuration feature as individually configurable.</remarks>
		public static readonly Db4objects.Db4o.Config.ConfigScope Individually = new Db4objects.Db4o.Config.ConfigScope
			(IndividuallyId, IndividuallyName);

		/// <summary>Marks a configuration feature as globally enabled.</summary>
		/// <remarks>Marks a configuration feature as globally enabled.</remarks>
		public static readonly Db4objects.Db4o.Config.ConfigScope Globally = new Db4objects.Db4o.Config.ConfigScope
			(GloballyId, GloballyName);

		private readonly int _value;

		private readonly string _name;

		private ConfigScope(int value, string name)
		{
			_value = value;
			_name = name;
		}

		/// <summary>
		/// Checks if the current configuration scope is globally
		/// enabled or disabled.
		/// </summary>
		/// <remarks>
		/// Checks if the current configuration scope is globally
		/// enabled or disabled.
		/// </remarks>
		/// <param name="defaultValue">- default result</param>
		/// <returns>
		/// false if disabled, true if globally enabled, default
		/// value otherwise
		/// </returns>
		public bool ApplyConfig(TernaryBool defaultValue)
		{
			switch (_value)
			{
				case DisabledId:
				{
					return false;
				}

				case GloballyId:
				{
					return !defaultValue.DefiniteNo();
				}

				default:
				{
					return defaultValue.DefiniteYes();
					break;
				}
			}
		}

		public override bool Equals(object obj)
		{
			if (this == obj)
			{
				return true;
			}
			if (obj == null || GetType() != obj.GetType())
			{
				return false;
			}
			Db4objects.Db4o.Config.ConfigScope tb = (Db4objects.Db4o.Config.ConfigScope)obj;
			return _value == tb._value;
		}

		public override int GetHashCode()
		{
			return _value;
		}

		private object ReadResolve()
		{
			switch (_value)
			{
				case DisabledId:
				{
					return Disabled;
				}

				case IndividuallyId:
				{
					return Individually;
				}

				default:
				{
					return Globally;
					break;
				}
			}
		}

		public override string ToString()
		{
			return _name;
		}
	}
}
