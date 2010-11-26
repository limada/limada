/*
 * Limaki 
 * Version 0.071
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
    public abstract class FactoryBase {
        private Dictionary<Type, Type> _clazzes = null;
        protected Dictionary<Type, Type> Clazzes {
            get {
                if(_clazzes == null) {
                    _clazzes = new Dictionary<Type, Type>();
                    defaultClazzes();
                }
                return _clazzes;
            }
        }
        protected Type Clazz<T>() {
            return Clazz(typeof(T));
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
        /// calls the default-construtor of the class R_Of{T}
        /// which is associated with type T
        /// <seealso cref="Clazz"/>
        /// </summary>
        /// <typeparam name="T">type to translate</typeparam>
        /// <returns>new object of R_Of{T}</returns>
        public R_Of One<T,R_Of>() {
            R_Of result = default(R_Of);
            Type type = typeof(T);
            Type clazzType = Clazz<T>();
            if (clazzType.IsClass) {
                result = (R_Of)Activator.CreateInstance(clazzType);

            }
            return result;
        }

        protected abstract void defaultClazzes();
    }
}