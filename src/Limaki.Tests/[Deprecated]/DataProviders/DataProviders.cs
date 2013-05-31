/*
 * Limaki 
 * 
 * This code is free software; you can redistribute it and/or modify it
 * under the terms of the GNU General Public License version 2 only, as
 * published by the Free Software Foundation.
 * 
 * Author: Lytico
 * Copyright (C) 2006-2011 Lytico
 *
 * http://www.limada.org
 * 
 */

using System;
using System.Collections.Generic;
using Limaki.Common;
using Limaki.Common.Collections;

namespace Limaki.Data {
    public class DataProviders<T> : IEnumerable<IDataProvider<T>> {
        private ICollection<Type> providers = new Set<Type>();

        public virtual void Add(Type provider) {
            if (Reflector.Implements(provider, typeof(IDataProvider<T>))) {
                providers.Add (provider);
            }
        }
        public virtual void Remove(Type provider) {
            providers.Remove(provider);
        }

        public virtual IDataProvider<T> FindByExtension(string extension) {
            foreach (var type in providers) {
                var provider = Activator.CreateInstance(type) as IDataProvider<T>;
                if (provider.Extension == extension) {
                    return provider;
                }
            }
            return null;
        }

        public virtual IDataProvider<T> Find(IoInfo info) {
            return FindByExtension(info.Extension);
        }

        public IEnumerator<IDataProvider<T>> GetEnumerator() {
            foreach (var type in providers) {
                yield return Activator.CreateInstance(type) as IDataProvider<T>;
            }
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() {
            return this.GetEnumerator ();
        }


    }
}