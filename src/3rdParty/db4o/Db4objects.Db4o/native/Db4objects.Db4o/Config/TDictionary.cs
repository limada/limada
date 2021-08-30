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
using Sharpen.Lang;
using Db4objects.Db4o;

namespace Db4objects.Db4o.Config {

	/// <exclude />
    public class TDictionary : IObjectTranslator {

        public void OnActivate(IObjectContainer objectContainer, object obj, object members){
            IDictionary dict = (IDictionary)obj;
            dict.Clear();
            if(members != null){
                Entry[] entries = (Entry[]) members;
                for(int i = 0; i < entries.Length; i++){
                    if(entries[i].key != null && entries[i].value != null){
                        dict[entries[i].key] =  entries[i].value;
                    }
                }
            }
        }

        public Object OnStore(IObjectContainer objectContainer, object obj){
            IDictionary dict = (IDictionary)obj;
            Entry[] entries = new Entry[dict.Count];
            IDictionaryEnumerator e = dict.GetEnumerator();
            e.Reset();
            for(int i = 0; i < dict.Count; i++){
                e.MoveNext();
                entries[i] = new Entry();
                entries[i].key = e.Key;
                entries[i].value = e.Value;
            }
            return entries;
        }

        public System.Type StoredClass(){
            return typeof(Entry[]);
        }
    }
}
