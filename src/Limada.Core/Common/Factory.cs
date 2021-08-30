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
using System.Linq;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using Limaki.Common.Linqish;
using System.Linq.Expressions;

namespace Limaki.Common {

    public abstract class FactoryBase  {

        public abstract bool Contains<T>();
        public abstract T Create<T>();
        public abstract T Create<T>(params object[] args);
        public abstract IFactory Clone();

        protected static IDictionary<int, ConstructorInfo> _constructors = new Dictionary<int, ConstructorInfo>();
        public T Ctor<T>(params object[] args) {
            //where T : new()
            //if (args == null || args.Length == 0)
            //    return new T();

            var clazz = typeof(T);
            var argTypes = args.Select(e => e.GetType()).ToArray();
            var key = KeyMaker.AddHashCode<Type>(clazz, KeyMaker.GetHashCode(argTypes));
            ConstructorInfo constructor = null;
            if (!_constructors.TryGetValue(key, out constructor)) {
                constructor = clazz.GetConstructor(argTypes);
                _constructors.Add(key, constructor);
            }
            if (constructor == null) {
                StringBuilder argMsg = new StringBuilder("(");
                argTypes.ForEach(t => argMsg.Append(t.Name + ","));
                if (argMsg.Length > 0)
                    argMsg.Replace(',', ')', argMsg.Length - 1, 1);
                throw new ArgumentException("typeof(" + clazz.Name + ") has no constructor with argumtens " + argMsg);
            }
            T result = (T)constructor.Invoke(args);
            return result;
        }
    }


    public class Factory : FactoryBase,IFactory {

        public IEnumerable<Type> KnownTypes {
            get { return Clazzes.Keys; }
        }

        protected IDictionary<Type, Type> _knownClazzes=null;
        protected IDictionary<Type,Type> knownClazzes {
            get {
                if (_knownClazzes == null) {
                    _knownClazzes = new Dictionary<Type, Type> (); 
                    if (_clazzes == null)
                        InstrumentClazzes ();
                }
                return _knownClazzes;
            }
        }
        public IEnumerable<Type> KnownClasses {
            get {
                if (_clazzes == null)
                    InstrumentClazzes();
                return knownClazzes.Values;
            }
        }

        protected IDictionary<Type, Delegate> _clazzes = null;
        protected IDictionary<Type, Delegate> Clazzes {
            get {
                if (_clazzes == null) {
                    _clazzes = new Dictionary<Type, Delegate>();
                    InstrumentClazzes();
                }
                return _clazzes;
            }
        }

        public virtual void Clear () {
            _clazzes = null;
            _knownClazzes = null;
        }

        public override bool Contains<T>() {
            return Clazzes.ContainsKey(typeof(T));
        }
        public virtual bool Contains(Type type) {
            return Clazzes.ContainsKey(type);
        }
        public virtual Func<T> Func<T>() {
            var type = typeof(T);
            Delegate result = null;
            if (Clazzes.TryGetValue(type, out result)) {
                return result as Func<T>;
            }

            return null;
        }

        public Delegate Func(Type type) {

            Delegate result = null;
            if (Clazzes.TryGetValue(type, out result)) {
                return result;
            }

            return null;
        }
        public Type Clazz<T>() {
            var result = default(Type);
            knownClazzes.TryGetValue(typeof(T), out result) ;
            return result;

        }
        public override T Create<T>(params object[] args) {
            Delegate d = null;
            var type = typeof(T);
            if (Clazzes.TryGetValue(type, out d)) {
                //if (args == null) {
                //    var func = d as Func<T>;
                //    if (func != null) {
                //        return func();
                //    }
                //}
                var funcA = d as Func<object[], T>;
                if (funcA != null) {
                    return funcA(args);
                }
            }
            if (type.IsClass)
                return (T)System.Activator.CreateInstance(typeof(T),args);
            return default(T);
        }

        public override T Create<T>() {
            Delegate d = null;
            var type = typeof (T);
            if (Clazzes.TryGetValue(type, out d)) {

                var func = d as Func<T>;
                if (func != null) {
                    return func();
                }
                var funcA = d as Func<object[], T>;
                if (funcA != null) {
                    return funcA(null);
                }
               
            }
            if (type.IsClass)
                return System.Activator.CreateInstance<T>();
            return default(T);
        }

        protected static MethodInfo GenericCreateMethod = null;
        public virtual object Create(Type type) {
            Delegate d = null;
            if (Clazzes.TryGetValue(type, out d)) {
                if (GenericAddMethod == null) {
                    Expression<Action> genexp = () => this.Create<object>();
                    GenericCreateMethod = ((MethodCallExpression)((LambdaExpression)genexp).Body).Method.GetGenericMethodDefinition();
                }
                // TODO: cache delegate here
                return GenericCreateMethod.MakeGenericMethod(type).Invoke(this, null);
            }
            return null;
        }

        protected static MethodInfo GenericAddMethod = null;
        public virtual void Add(Type t1, Type t2) {
            if (GenericAddMethod == null) {
                Expression<Action> genexp =() => this.Add<object, object>();
                GenericAddMethod = ((MethodCallExpression)((LambdaExpression)genexp).Body).Method.GetGenericMethodDefinition();
            }
            // TODO: cache delegate here
            GenericAddMethod.MakeGenericMethod(t1, t2).Invoke(this, null);
            knownClazzes[t1] = t2;
        }

        public virtual void Add<T1, T2>() where T2 : T1 {
            Type clazzType = typeof(T2);
            var canActivate = clazzType.IsClass || clazzType.IsValueType;
            var argsAllowed = clazzType.IsClass;
            Func<object[], T1> creator = args => {
                if (args != null)
                    if (!argsAllowed)
                        throw new ArgumentException(clazzType.Name + "does't have a contstrutor with arguments");
                    else
                        return Ctor<T2>(args);

                T1 result = default(T1);

                if (canActivate) {
                    result = (T1)Activator.CreateInstance(clazzType);
                }
                return result;
            };

            Clazzes[typeof(T1)] = creator;
            AddKnown<T1, T2>();
        }

        public virtual void Add<T>(Func<object[], T> creator) {
            Clazzes[typeof(T)] = creator;
        }
        
        public virtual void Add<T>(Func<T> creator) {
            Clazzes[typeof(T)] = creator;
        }

        public virtual void Add<T1,T2>(Func<object[], T1> creator)where T2:T1 {
            Clazzes[typeof(T1)] = creator;
            AddKnown<T1, T2>();   
        }

        protected virtual void AddKnown<T1,T2>() {
            knownClazzes[typeof(T1)] = typeof(T2);
        }

        public virtual void Add<T1,T2>(Func<T1> creator) where T2 : T1 {
            Clazzes[typeof(T1)] = creator;
            AddKnown<T1, T2>();
        }
        protected void Add(Type type, Delegate creator) {
            Clazzes[type] = creator;
        }

        protected virtual void InstrumentClazzes() { }

        public override IFactory Clone() {
            var result = new Factory();
            result._clazzes = new Dictionary<Type, Delegate>();
            foreach (var item in this.Clazzes)
                result._clazzes.Add(item);
            return result;
        }

        public virtual void MergeWith(Factory other) {
            foreach (var type in other.KnownTypes)
                this.Add(type, other.Func(type));
        }

    }
}