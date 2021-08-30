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

namespace Db4objects.Db4o.Foundation
{
	public interface IBlockingQueue4 : IQueue4
	{
		/// <summary>
		/// <p>
		/// Returns the next queued item or waits for it to be available for the
		/// maximum of <code>timeout</code> miliseconds.
		/// </summary>
		/// <remarks>
		/// <p>
		/// Returns the next queued item or waits for it to be available for the
		/// maximum of <code>timeout</code> miliseconds.
		/// </remarks>
		/// <param name="timeout">maximum time to wait for the next avilable item in miliseconds
		/// 	</param>
		/// <returns>
		/// the next item or <code>null</code> if <code>timeout</code> is
		/// reached
		/// </returns>
		/// <exception cref="BlockingQueueStoppedException">
		/// if the
		/// <see cref="Stop()">Stop()</see>
		/// is called.
		/// </exception>
		/// <exception cref="Db4objects.Db4o.Foundation.BlockingQueueStoppedException"></exception>
		object Next(long timeout);

		void Stop();

		/// <summary>
		/// <p>
		/// Removes all the available elements in the queue to the colletion passed
		/// as argument.
		/// </summary>
		/// <remarks>
		/// <p>
		/// Removes all the available elements in the queue to the colletion passed
		/// as argument.
		/// <p>
		/// It will block until at least one element is available.
		/// </remarks>
		/// <param name="list"></param>
		/// <returns>the number of elements added to the list.</returns>
		/// <exception cref="BlockingQueueStoppedException">
		/// if the
		/// <see cref="Stop()">Stop()</see>
		/// is called.
		/// </exception>
		/// <exception cref="Db4objects.Db4o.Foundation.BlockingQueueStoppedException"></exception>
		int DrainTo(Collection4 list);
	}
}
