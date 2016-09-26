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

namespace Limaki.Common {

    public class Pool:IPool {

        private IDictionary<Type,object> _pooledObjects = 
            new Dictionary<Type,object> ();

        IFactory _poolableObjectFactory = null;
        public virtual IFactory PoolFactory {
            set { _poolableObjectFactory = value; }
        }
 
        public virtual object TryGetCreate(Type type) {
            object result = null;
            if (!_pooledObjects.TryGetValue(type, out result)) {
                if (_poolableObjectFactory.Contains(type)) {
                    result = _poolableObjectFactory.Create (type);
                } else {
                    result = Activator.CreateInstance(type);
                }
                _pooledObjects.Add (type, result);
            }
            return result;
        }

        public virtual T TryGetCreate<T>() {
            object result = default(T);
            Type type = typeof (T);
            if (!_pooledObjects.TryGetValue(type, out result)) {
                if (_poolableObjectFactory.Contains<T>()) {
                    result = _poolableObjectFactory.Create<T>();
                } else {
                    result = Activator.CreateInstance<T> ();
                }
                _pooledObjects.Add(type, result);
            }
            return (T)result;
        }

        public virtual bool Remove<T>() {
            Type type = typeof(T);
            return _pooledObjects.Remove (type);
        }

        public void Register<T> (T subject) {
            _pooledObjects[typeof (T)] = subject;
        }

        public virtual void Clear() {
            _pooledObjects.Clear();
        }
    }
}