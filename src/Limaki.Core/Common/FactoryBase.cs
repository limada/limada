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
 * http://limada.sourceforge.net
 * 
 */

using System;
using System.Linq;
using System.Collections.Generic;

namespace Limaki.Common {
    public class FactoryBase : IFactory {
        private Dictionary<Type, Type> _clazzes = null;
        protected Dictionary<Type, Type> Clazzes {
            get {
                if(_clazzes == null) {
                    _clazzes = new Dictionary<Type, Type>();
                    InstrumentClazzes();
                }
                return _clazzes;
            }
        }

        private IDictionary<Type, Delegate> _creators = null;
        protected IDictionary<Type, Delegate> Creators {
            get {
                if (_creators == null) {
                    _creators = new Dictionary<Type, Delegate>();
                    InstrumentClazzes();
                }
                return _creators;
            }
        }
        public IEnumerable<Type> KnownClasses {
            get { return Clazzes.Values.Distinct<Type>(); }
        }

        public Type Clazz<T>() {
            return Clazz(typeof(T));
        }

        public bool Contains<T>() {
            return Clazzes.ContainsKey (typeof (T));
        }

        /// <summary>
        /// returns the associated Type of type
        /// Association is in a Dictionary<Type, Type> 
        /// where key=interface and value=class
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public virtual Type Clazz(Type type) {
            Type result = null;
            if (!Clazzes.TryGetValue(type, out result)) {
                result = type;
            }
            if (result.IsInterface) {
                throw new ArgumentException(result.FullName + " not supported");
            }
            return result;
        }

        public virtual object Create(Type type) {
            object result = null;
            Type clazzType = null;

            if (Clazzes.TryGetValue(type, out clazzType)) {
                if (clazzType.IsClass) {
                    result = Activator.CreateInstance(clazzType);
                }
            } else {
                Delegate d = null;
                if (Creators.TryGetValue(type, out d)) {
                    return d.DynamicInvoke(null);
                }
            }
            return result;
        }

        /// <summary>
        /// calls the default-construtor of the class
        /// which is associated with interface T
        /// <code>T o = TranslatedType.New();</code>
        /// <seealso cref="Clazz"/>
        /// </summary>
        /// <typeparam name="T">interface to translate</typeparam>
        /// <returns>new object of translated class</returns>
        public virtual T Create<T>() {
            T result = default(T);
            Type type = typeof(T);
            var clazzType = Clazz<T>();
            if (clazzType != null) {
                if (clazzType.IsClass) {
                    result = (T)Activator.CreateInstance(clazzType);
                }
            } else {
                Func<T> func = null;
                Delegate d = null;
                if (Creators.TryGetValue(typeof(T), out d)) {
                    func = d as Func<T>;
                    if (func != null) {
                        return func();
                    }
                }
            }
            return result;
        }
        /// <summary>
        /// calls the default-construtor of the class TOuter#TInner#
        /// which is associated with type T
        /// <seealso cref="Clazz"/>
        /// </summary>
        /// <typeparam name="TInner">type to translate</typeparam>
        /// <returns>new object of TOuter{TInner}</returns>
        public virtual TOuter Create<TInner,TOuter>() {
            TOuter result = default(TOuter);
            Type clazzType = Clazz<TInner>();
            if (clazzType.IsClass) {
                result = (TOuter)Activator.CreateInstance(clazzType);

            }
            return result;
        }
        
        public virtual void Add(Type source, Type target) {
            Clazzes[source]=target;
        }

        public virtual void Add<T1,T2>() {
            Clazzes[typeof(T1)] = typeof(T2);
        }

        protected bool instrumented = false;
        protected virtual void InstrumentClazzes() {
            instrumented = true;
        }

        public virtual void Add<T>(Func<T> creator) {
            Creators[typeof(T)] = creator;
        }


    }
}