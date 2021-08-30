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
using Db4objects.Db4o.Constraints;

namespace Db4objects.Db4o.Constraints
{
	/// <summary>
	/// db4o-specific exception.<br /><br />
	/// This exception can be thrown by a
	/// <see cref="UniqueFieldValueConstraint">UniqueFieldValueConstraint</see>
	/// on commit.
	/// </summary>
	/// <seealso cref="Db4objects.Db4o.Config.IObjectField.Indexed(bool)">Db4objects.Db4o.Config.IObjectField.Indexed(bool)
	/// 	</seealso>
	/// <seealso cref="Db4objects.Db4o.Config.IConfiguration.Add(Db4objects.Db4o.Config.IConfigurationItem)
	/// 	">Db4objects.Db4o.Config.IConfiguration.Add(Db4objects.Db4o.Config.IConfigurationItem)
	/// 	</seealso>
	[System.Serializable]
	public class UniqueFieldValueConstraintViolationException : ConstraintViolationException
	{
		/// <summary>
		/// Constructor with a message composed from the class and field
		/// name of the entity causing the exception.
		/// </summary>
		/// <remarks>
		/// Constructor with a message composed from the class and field
		/// name of the entity causing the exception.
		/// </remarks>
		/// <param name="className">class, which caused the exception</param>
		/// <param name="fieldName">field, which caused the exception</param>
		public UniqueFieldValueConstraintViolationException(string className, string fieldName
			) : base("class: " + className + " field: " + fieldName)
		{
		}
	}
}
