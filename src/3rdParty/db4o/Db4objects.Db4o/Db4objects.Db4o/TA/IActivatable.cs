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
using Db4objects.Db4o.Activation;

namespace Db4objects.Db4o.TA
{
	/// <summary>
	/// IActivatable must be implemented by classes in order to support
	/// Transparent Activation.
	/// <br/>
	/// <br/>
	/// The IActivatable interface may be added to persistent classes by hand
	/// or by using the db4o instrumentation (Db4oTools).
	/// </summary>
	/// <remarks>
	/// IActivatable must be implemented by classes in order to support
	/// Transparent Activation.
	/// <br/>
	/// <br/>
	/// The IActivatable interface may be added to persistent classes by hand
	/// or by using the db4o instrumentation (Db4oTools). For further
	/// information on the enhancer see:
	/// <br/>
	/// <br/>
	/// http://developer.db4o.com/Resources/view.aspx/Reference/Implementation_Strategies/Enhancement_Tools/Enhancement_For_.NET.
	/// <br/>
	/// <br/>
	/// The basic idea for Transparent Activation is as follows:
	/// <br/>
	/// Objects have an activation depth of 0, i.e. by default they are not
	/// activated at all. Whenever a method is called on such an object, the
	/// first thing to do before actually executing the method body is to
	/// activate the object to level 1, i.e. populating its direct members.
	/// <br/>
	/// <br/>
	/// To illustrate this approach, we will use the following simple class.
	/// <br/>
	/// <br/>
	/// <code>
	/// public class Item {
	/// <br/>   private Item _next;<br/><br/>
	///    public Item(Item next) {<br/>
	///       _next = next;<br/>
	///    }<br/><br/>
	///    public Item Next {<br/>
	///      get {<br/>
	///       return _next;<br/>
	///      }<br/>
	///    }<br/>
	/// }<br/><br/></code>
	/// The basic sequence of actions to get the above scheme to work is the
	/// following:<br/>
	/// <br/>
	/// - Whenever an object is instantiated from db4o, the database registers an
	/// activator for this object. To enable this, the object has to implement the
	/// IActivatable interface and provide the according Bind(IActivator) method. The
	/// default implementation of the bind method will simply store the given
	/// activator reference for later use.<br/>
	/// <br/>
	/// <code>
	/// public class Item implements IActivatable {<br/>
	///    transient IActivator _activator;<br/><br/>
	///    public void Bind(IActivator activator) {<br/>
	///       if (null != _activator) {<br/>
	///          throw new IllegalStateException();<br/>
	///       }<br/>
	///       _activator = activator;<br/>
	///    }<br/><br/>
	///    // ...<br/>
	/// }<br/><br/></code>
	/// - The first action in every method body of an activatable object should be a
	/// call to the corresponding IActivator's Activate() method. (Note that this is
	/// not enforced by any interface, it is rather a convention, and other
	/// implementations are possible.)<br/>
	/// <br/>
	/// <code>
	/// public class Item implements IActivatable {<br/>
	///    public void Activate() {<br/>
	///       if (_activator == null) return;<br/>
	///       _activator.Activate();<br/>
	///    }<br/><br/>
	///    public Item Next() {<br/>
	///      get {<br/>
	///       Activate();<br/>
	///       return _next;<br/>
	///      }<br/>
	///    }<br/>
	/// }<br/><br/></code>
	/// - The Activate() method will check whether the object is already activated.
	/// If this is not the case, it will request the container to activate the object
	/// to level 1 and set the activated flag accordingly.<br/>
	/// <br/>
	/// To instruct db4o to actually use these hooks (i.e. to register the database
	/// when instantiating an object), TransparentActivationSupport has to be
	/// registered with the db4o configuration.<br/>
	/// <br/>
	/// <code>
	/// ICommonConfiguration config = ...<br/>
	/// config.Add(new TransparentActivationSupport());<br/><br/>
	/// </code>
	/// </remarks>
	public interface IActivatable
	{
		/// <summary>called by db4o upon instantiation.</summary>
		/// <remarks>
		/// called by db4o upon instantiation. <br />
		/// <br />
		/// The recommended implementation of this method is to store the passed
		/// <see cref="Db4objects.Db4o.Activation.IActivator">Db4objects.Db4o.Activation.IActivator
		/// 	</see>
		/// in a transient field of the object.
		/// </remarks>
		/// <param name="activator">the Activator</param>
		void Bind(IActivator activator);

		/// <summary>should be called by every reading field access of an object.</summary>
		/// <remarks>
		/// should be called by every reading field access of an object. <br />
		/// <br />
		/// The recommended implementation of this method is to call
		/// <see cref="Db4objects.Db4o.Activation.IActivator.Activate(Db4objects.Db4o.Activation.ActivationPurpose)
		/// 	">Db4objects.Db4o.Activation.IActivator.Activate(Db4objects.Db4o.Activation.ActivationPurpose)
		/// 	</see>
		/// on the
		/// <see cref="Db4objects.Db4o.Activation.IActivator">Db4objects.Db4o.Activation.IActivator
		/// 	</see>
		/// that was previously passed to
		/// <see cref="Bind(Db4objects.Db4o.Activation.IActivator)">Bind(Db4objects.Db4o.Activation.IActivator)
		/// 	</see>
		/// .
		/// </remarks>
		/// <param name="purpose">TODO</param>
		void Activate(ActivationPurpose purpose);
	}
}
