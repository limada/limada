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
namespace Db4objects.Db4o.Config
{
	/// <summary>configuration interface for fields of classes.</summary>
	/// <remarks>
	/// configuration interface for fields of classes.
	/// <br/><br/>
	/// Use
	/// <see cref="IObjectClass.ObjectField(string)">IObjectClass.ObjectField(string)</see>
	/// to access this setting.<br/><br/>
	/// </remarks>
	public interface IObjectField
	{
		/// <summary>sets cascaded activation behaviour.</summary>
		/// <remarks>
		/// sets cascaded activation behaviour.
		/// <br/><br/>
		/// Setting cascadeOnActivate to true will result in the activation
		/// of the object attribute stored in this field if the parent object
		/// is activated.
		/// <br/><br/>
		/// The default setting is <b>false</b>.<br/><br/>
		/// In client-server environment this setting should be used on both
		/// client and server. <br/><br/>
		/// This setting can be applied to an open object container. <br/><br/>
		/// </remarks>
		/// <param name="flag">whether activation is to be cascaded to the member object.</param>
		/// <seealso cref="IConfiguration.ActivationDepth()">Why activation?</seealso>
		/// <seealso cref="IObjectClass.CascadeOnActivate(bool)">IObjectClass.CascadeOnActivate(bool)
		/// 	</seealso>
		/// <seealso cref="Db4objects.Db4o.IObjectContainer.Activate(object, int)">Db4objects.Db4o.IObjectContainer.Activate(object, int)
		/// 	</seealso>
		/// <seealso cref="Db4objects.Db4o.Ext.IObjectCallbacks">Using callbacks</seealso>
		void CascadeOnActivate(bool flag);

		/// <summary>sets cascaded delete behaviour.</summary>
		/// <remarks>
		/// sets cascaded delete behaviour.
		/// <br/><br/>
		/// Setting cascadeOnDelete to true will result in the deletion of
		/// the object attribute stored in this field on the parent object
		/// if the parent object is passed to
		/// <see cref="Db4objects.Db4o.IObjectContainer.Delete(object)">Db4objects.Db4o.IObjectContainer.Delete(object)
		/// 	</see>
		/// .
		/// <br/><br/>
		/// <b>Caution !</b><br/>
		/// This setting will also trigger deletion of the old member object, on
		/// calls to
		/// <see cref="Db4objects.Db4o.IObjectContainer.Store(object)"></see>
		/// .
		/// An example of the behaviour can be found in
		/// <see cref="IObjectClass.CascadeOnDelete(bool)">IObjectClass.CascadeOnDelete(bool)
		/// 	</see>
		/// <br/><br/>
		/// The default setting is <b>false</b>.<br/><br/>
		/// In client-server environment this setting should be used on both
		/// client and server. <br/><br/>
		/// This setting can be applied to an open object container. <br/><br/>
		/// </remarks>
		/// <param name="flag">whether deletes are to be cascaded to the member object.</param>
		/// <seealso cref="IObjectClass.CascadeOnDelete(bool)">IObjectClass.CascadeOnDelete(bool)
		/// 	</seealso>
		/// <seealso cref="Db4objects.Db4o.IObjectContainer.Delete(object)">Db4objects.Db4o.IObjectContainer.Delete(object)
		/// 	</seealso>
		/// <seealso cref="Db4objects.Db4o.Ext.IObjectCallbacks">Using callbacks</seealso>
		void CascadeOnDelete(bool flag);

		/// <summary>sets cascaded update behaviour.</summary>
		/// <remarks>
		/// sets cascaded update behaviour.
		/// <br/><br/>
		/// Setting cascadeOnUpdate to true will result in the update
		/// of the object attribute stored in this field if the parent object
		/// is passed to
		/// <see cref="Db4objects.Db4o.IObjectContainer.Store(object)">Db4objects.Db4o.IObjectContainer.Store(object)
		/// 	</see>
		/// .
		/// <br/><br/>
		/// The default setting is <b>false</b>.<br/><br/>
		/// In client-server environment this setting should be used on both
		/// client and server. <br/><br/>
		/// This setting can be applied to an open object container. <br/><br/>
		/// </remarks>
		/// <param name="flag">whether updates are to be cascaded to the member object.</param>
		/// <seealso cref="com.db4o.ObjectContainer#set">com.db4o.ObjectContainer#set</seealso>
		/// <seealso cref="IObjectClass.CascadeOnUpdate(bool)">IObjectClass.CascadeOnUpdate(bool)
		/// 	</seealso>
		/// <seealso cref="IObjectClass.UpdateDepth(int)">IObjectClass.UpdateDepth(int)</seealso>
		/// <seealso cref="Db4objects.Db4o.Ext.IObjectCallbacks">Using callbacks</seealso>
		void CascadeOnUpdate(bool flag);

		/// <summary>turns indexing on or off.</summary>
		/// <remarks>
		/// turns indexing on or off.
		/// <br/><br/>Field indices dramatically improve query performance but they may
		/// considerably reduce storage and update performance.<br/>The best benchmark whether
		/// or not an index on a field achieves the desired result is the completed application
		/// - with a data load that is typical for it's use.<br/><br/>This configuration setting
		/// is only checked when the
		/// <see cref="Db4objects.Db4o.IObjectContainer">Db4objects.Db4o.IObjectContainer</see>
		/// is opened. If the
		/// setting is set to <code>true</code> and an index does not exist, the index will be
		/// created. If the setting is set to <code>false</code> and an index does exist the
		/// index will be dropped.<br/><br/>
		/// In client-server environment this setting should be used on both
		/// client and server. <br/><br/>
		/// If this setting is applied to an open ObjectContainer it will take an effect on the next
		/// time ObjectContainer is opened.<br/><br/>
		/// </remarks>
		/// <param name="flag">
		/// specify <code>true</code> or <code>false</code> to turn indexing on for
		/// this field
		/// </param>
		void Indexed(bool flag);

		/// <summary>renames a field of a stored class.</summary>
		/// <remarks>
		/// renames a field of a stored class.
		/// <br/><br/>Use this method to refactor classes.
		/// <br/><br/>
		/// In client-server environment this setting should be used on both
		/// client and server. <br/><br/>
		/// This setting can NOT be applied to an open object container. <br/><br/>
		/// </remarks>
		/// <param name="newName">the new field name.</param>
		void Rename(string newName);
	}
}
