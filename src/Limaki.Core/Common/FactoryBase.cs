/*
 * Limaki 
 * Version 0.08
 * 
 * This code is free software; you can redistribute it and/or modify it
 * under the terms of the GNU General Public License version 2 only, as
 * published by the Free Software Foundation.
 * 
 * Author: Lytico
 * Copyright (C) 2006-2008 Lytico
 *
 * http://limada.sourceforge.net
 * 
 */

using System;
using System.Collections.Generic;

namespace Limaki.Common {
    public abstract class FactoryBase : IFactory {
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

        public object One(Type type) {
            object result = null;
            Type clazzType = Clazz(type);
            if (clazzType.IsClass) {
                result = Activator.CreateInstance(clazzType);
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
        public T One<T>() {
            T result = default(T);
            Type type = typeof(T);
            Type clazzType = Clazz<T>();
            if (clazzType.IsClass) {
                result = (T)Activator.CreateInstance(clazzType);
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
        public TOuter One<TInner,TOuter>() {
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

        protected abstract void InstrumentClazzes();
    }
}