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
using Db4objects.Db4o.Internal;
using Db4objects.Db4o.Reflect.Generic;

namespace Db4objects.Db4o.CS.Internal.Messages
{
	public class MClassMeta : MsgObject, IMessageWithResponse
	{
		public virtual Msg ReplyFromServer()
		{
			Unmarshall();
			try
			{
				lock (ContainerLock())
				{
					ClassInfo classInfo = (ClassInfo)ReadObjectFromPayLoad();
					ClassInfoHelper classInfoHelper = ServerMessageDispatcher().ClassInfoHelper();
					GenericClass genericClass = classInfoHelper.ClassMetaToGenericClass(Container().Reflector
						(), classInfo);
					if (genericClass != null)
					{
						Transaction trans = Container().SystemTransaction();
						ClassMetadata classMetadata = Container().ProduceClassMetadata(genericClass);
						if (classMetadata != null)
						{
							Container().CheckStillToSet();
							classMetadata.SetStateDirty();
							classMetadata.Write(trans);
							trans.Commit();
							StatefulBuffer returnBytes = Container().ReadStatefulBufferById(trans, classMetadata
								.GetID());
							return Msg.ObjectToClient.GetWriter(returnBytes);
						}
					}
				}
			}
			catch (Exception e)
			{
			}
			return Msg.Failed;
		}
	}
}
