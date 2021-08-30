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
using Db4objects.Db4o.Ext;
using Db4objects.Db4o.Foundation;

namespace Db4objects.Db4o.Internal.Handlers
{
    internal class WeakReferenceHandlerQueue
	{
        private List4 _list;

        internal void Add(WeakReferenceHandler reference) {
            lock(this){
                _list = new List4(_list, reference);
            }
        }

        internal void Poll(ObjectContainerBase objectContainer) {
            List4 remove = null;
            lock(this){
                System.Collections.IEnumerator i = new Iterator4Impl(_list);
                _list = null;
                while(i.MoveNext()){
					WeakReferenceHandler refHandler = (WeakReferenceHandler)i.Current;
                    if(refHandler.IsAlive){
                        _list = new List4(_list, refHandler);
                    }else{
                        remove = new List4(remove, refHandler.ObjectReference);
                    }
                }
            }
            System.Collections.IEnumerator j = new Iterator4Impl(remove);
            while (j.MoveNext())
            {
                lock (objectContainer.Lock())
                {
                    if (objectContainer.IsClosed())
                    {
                        return;
                    }
                    objectContainer.RemoveFromAllReferenceSystems(j.Current);
                }
            }
        }
    }
}