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

using Limaki.Common.IOC;

namespace Limaki.Common {
    
    /// <summary>
    /// The Registry gathers application wide used objects
    /// </summary>
    public class Registry:IApplicationContext {
        
        static IApplicationContext _concreteContext = null;
        public static IApplicationContext ConcreteContext {
            get { return _concreteContext; }
            set { _concreteContext = value; }
        }

        public static IPool Pool {
            get { return ConcreteContext.Pool; }
        }

        public static IFactory Factory {
            get { return ConcreteContext.Factory; }
        }

        /// <summary>
        /// looks for an ApplicationContextProcessor in the Pool
        /// and calls ApplicationContextProcessor.ApplyProperties(target)
        /// </summary>
        /// <typeparam name="TProcessor"></typeparam>
        /// <typeparam name="TTarget"></typeparam>
        /// <param name="target"></param>
        public static void ApplyProperties<T> (T target) {
            var processor = Pooled<ContextProcessor<T>> ();
            processor.ApplyProperties (ConcreteContext, target);
        }

        /// <summary>
        /// looks for an ApplicationContextProcessor in the Pool
        /// and calls ApplicationContextProcessor.ApplyProperties(target)
        /// </summary>
        /// <typeparam name="TProcessor"></typeparam>
        /// <typeparam name="TTarget"></typeparam>
        /// <param name="target"></param>
        public static void ApplyProperties<TProcessor, TTarget> (TTarget target)
            where TProcessor : ContextProcessor<TTarget> {
            if (target == null || ConcreteContext == null) return;
            var processor = Pooled<TProcessor> ();
            processor.ApplyProperties (ConcreteContext, target);

        }

        /// <summary>
        /// calls <see cref="IPool.TryGetCreate{T}()"/> of <see cref="Pool"/>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T Pooled<T>() {
            return Pool.TryGetCreate<T>();
        }

        public static T Create<T> () {
            return Factory.Create<T> ();
        }

        public static T Create<T> (params object[] args) {
            return Factory.Create<T> (args);
        }

        #region IApplicationContext Member

        IPool IApplicationContext.Pool {
            get { return ConcreteContext.Pool; }
        }

        IFactory IApplicationContext.Factory {
            get { return ConcreteContext.Factory; }
        }

        #endregion
    }
}