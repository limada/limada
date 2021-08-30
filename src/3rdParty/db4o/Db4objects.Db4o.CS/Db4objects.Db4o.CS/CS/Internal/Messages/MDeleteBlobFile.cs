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
using Db4objects.Db4o.CS.Internal;
using Db4objects.Db4o.CS.Internal.Messages;
using Db4objects.Db4o.Types;

namespace Db4objects.Db4o.CS.Internal.Messages
{
	public class MDeleteBlobFile : MsgBlob, IServerSideMessage
	{
		public virtual void ProcessAtServer()
		{
			try
			{
				IBlob blob = this.ServerGetBlobImpl();
				if (blob != null)
				{
					blob.DeleteFile();
				}
			}
			catch (Exception)
			{
			}
		}

		/// <exception cref="System.IO.IOException"></exception>
		public override void ProcessClient(Socket4Adapter sock)
		{
		}
		// nothing to do here
	}
}
