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

using Db4objects.Db4o;
using Db4objects.Db4o.Config;
using Db4objects.Db4o.Linq;
using Db4objects.Db4o.Query;
using Limaki.Common.Collections;
using Limaki.Common.Reflections;
using Limaki.Graphs;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Limaki.Data.db4o {

    public class Graph<TItem, TEdge> : DbGraph<TItem, TEdge>, IGatewayConnection
        where TEdge : IEdge<TItem> {

        #region IGraph<TItem,TEdge> Member

        public override bool Contains(TEdge edge) {
            bool result = false;
            try {
                result = Session.Ext().IsStored(edge);
            } catch (Exception e) {
                throw e;
            } finally { }
            return result;
        }

        protected override void Store(TItem item) {
            Session.Store(item);
        }

        protected override void Store(TEdge edge) {
            Session.Store(edge);
        }

        protected override void Delete(TItem item) {
            Session.Delete(item);
        }

        protected override void Delete(TEdge edge) {
            Session.Delete(edge);
        }

        
        public override int EdgeCount(TItem item) {
            return Edges(item).Count;
        }


        protected override ICollection<TEdge> EdgesOf(TItem item) {
            ICollection<TEdge> result = null;
            try {
                // this doesn't optimize; it seems that the easiest way is to 
                // override Graphs with concrete classes
                //result = _session.Query<TEdge>(delegate(TEdge e) {
                //    return e.Leaf.Equals(item) || e.Root.Equals(item);
                //});

                IQuery query = Session.Query();
                query.Constrain(typeof(TEdge));
                IConstraint constr = query.Descend("_root").Constrain(item);
                constr.Or(query.Descend("_leaf").Constrain(item));


                result = new NativeQueryWrapper<TEdge>(query.Execute());

            } catch (Exception e) {
                throw e;
            } finally { }
            return result;
        }

        public override IEnumerable<TEdge> Edges() {
            IEnumerable<TEdge> result = null;
            try {
                result = Session.Query<TEdge>();
            } catch (Exception e) {
                throw e;
            } finally { }
            return result;
        }

        public override bool Contains(TItem item) {
            bool result = false;
            try {
                if (ItemIsStorableClazz) {
                    result = Session.Ext().IsStored(item);
                } else {
                    result = EdgeCount(item) != 0;
                }
            } catch (Exception e) {
                throw e;
            } finally { }
            return result;
        }

        public override void Clear() {
            foreach (TItem item in this) {
                Remove(item);
            }
        }

        public override void CopyTo(TItem[] array, int arrayIndex) {
            throw new Exception("The method or operation is not implemented.");
        }

        public override int Count {
            get {
                int result = 0;
                foreach (var clazz in Session.Ext().StoredClasses()) {
                    result += clazz.InstanceCount();
                }
                return result;
            }
        }

        public override bool IsReadOnly {
            get { return Session != null; }
        }

        public override IEnumerator<TItem> GetEnumerator() {
            return Items.GetEnumerator();
        }

        protected override IEnumerable<TItem> Items {
            get {
                if (!ItemIsStorableClazz) {
                    Set<TItem> done = new Set<TItem>();
                    foreach (TEdge edge in Edges()) {
                        if (!done.Contains(edge.Root)) {
                            done.Add(edge.Root);
                        }
                        if (!done.Contains(edge.Leaf)) {
                            done.Add(edge.Leaf);
                        }
                    }
                    return done;
                } else {
                    IList<TItem> list = Session.Query<TItem>();
                    return list;
                }
            }
        }

        public override void OnDataChanged(TItem item) {
            base.OnDataChanged(item);
            this.Add(item);
        }

        public override void DoChangeData(TItem item, object data) {
            base.DoChangeData(item, data);
            this.Add(item);
        }


        #endregion

        # region Session

        Gateway _gateway = null;
        public IGateway Gateway {
            get { return _gateway; }
        }

        public IObjectContainer Session {
            get {
                if (!_gateway.HasSession() && !_gateway.IsClosed) {
                    ConfigureSession(_gateway.Session);
                }
                return _gateway.Session;
            }
        }

        public Graph(IGateway gateway) {
            this._gateway = gateway as Gateway;
            if (this._gateway == null) {
                throw new ArgumentException("Wrong Gateway");
            }
            DeclareTypesToConfigure();
            //Refactor();
            Configure();
            ConfigureSession (_gateway.Session);
        }

        public override void EvictItem(TItem item) {
            Session.Ext().Purge(item);
        }

        public override void Flush() {
            try {
                if (Session != null) {
                    Session.Commit ();
                }
            } catch (Db4objects.Db4o.Ext.Db4oException e) {
                // TODO: a curios exception is thrown here:
                // "This functionality is only available for indexed fields."
                // see also: Gateway.Close()
                throw e;
            }
        }
        public override void Close() {
            Flush();
            this._gateway.Close();
        }
        #endregion 

        #region Configuration

        /// <summary>
        /// Add classes to be configured to a 
        /// </summary>
        protected virtual void DeclareTypesToConfigure() {
            TypesToConfigure.Add(typeof(TItem));
            TypesToConfigure.Add(typeof(TEdge));
        }

        /// <summary>
        /// Configure a class
        /// this means setting the indices, UpdateDepth etc. etc.
        /// </summary>
        /// <param name="type"></param>
        protected virtual void ConfigureType(Type type) {
            var clazz = Configuration.ObjectClass(type);
            clazz.CascadeOnUpdate(true);
            clazz.CascadeOnActivate(true);
            clazz.CascadeOnDelete(false);
            clazz.UpdateDepth(5);
            if (Reflector.Implements(type, typeof(IEdge<TItem>))) {
                clazz.ObjectField("_root").Indexed(true);
                clazz.ObjectField("_leaf").Indexed(true);
            }
        }

        protected virtual bool RefactorType(Type type) {
            return false;
        }

        public ICommonConfiguration Configuration {
            get { return _gateway.Configuration; }
        }


        private IList<Type> _typesToConfigure = new List<Type>();
        /// <summary>
        /// a list of known types
        /// use DeclareTypes to add items to the list
        /// </summary>
        public virtual IList<Type> TypesToConfigure {
            get { return _typesToConfigure; }
            set { _typesToConfigure = value; }
        }

        /// <summary>
        /// returns true if a type is configurable
        /// </summary>
        /// <param name="type"></param>
        public virtual bool IsClassConfigurable(Type type) {
            return
                (type != null && Reflector.IsStorable(type) && !type.IsGenericTypeDefinition) &&
                (Reflector.Implements(type, typeof(TEdge)) || Reflector.Implements(type, typeof(TItem))
                  )
            ;
        }

        /// <summary>
        /// Calls configureclass for every type in typesToConfigure
        /// calls ConfigureAliases
        /// this is called before opening the database; no valid session here
        /// </summary>
        protected void Configure() {
            ConfigureAliases ();
            var typesDone = new Set<Type>();
            
            foreach (Type type in TypesToConfigure) {
                try {
                    if (!typesDone.Contains(type) && IsClassConfigurable(type)) {
                        ConfigureType(type);
                    }
                } catch (Exception e) {
                    Trace.WriteLine("Error with " + type.FullName + "\t" + e.Message);
                    Trace.WriteLine(e.StackTrace);
                }
                typesDone.Add(type);
            }

            //foreach (Assembly ass in AppDomain.CurrentDomain.GetAssemblies()) {
            //    try {
            //        foreach (Type type in ass.GetTypes()) {
            //            if (!typesDone.Contains(type) && IsClassConfigurable(type)) {
            //                ConfigureType(type);
            //                typesDone.Add(type);
            //            }
            //        }
            //    } catch (Exception e) {
            //        System.Console.WriteLine("Error with " + ass.FullName + "\t" + e.Message);
            //        System.Console.WriteLine(e.StackTrace);
            //    }
            //}
        }

        /// <summary>
        /// this is called after opening the database
        /// and having a valid session
        /// register events to the IEventRegistry here
        /// </summary>
        /// <param name="session"></param>
        protected virtual void ConfigureSession(IObjectContainer session) { }

        protected virtual void ConfigureAlias(Type type) {
            string ass = type.Assembly.FullName;
            ass = ass.Substring(0, ass.IndexOf(","));
            string namespc = type.Namespace;
            Configuration.AddAlias(
              new WildcardAlias(
              namespc + ".*, "+namespc+".007",
              namespc + ".*, " + ass));
        }

        protected virtual void ConfigureAliases() {
            Set<string> assembliesDone = new Set<string> ();
            Set<string> namespacesDone = new Set<string> ();
            foreach (Type type in TypesToConfigure) {
                if (!namespacesDone.Contains(type.Namespace) || !
                    assembliesDone.Contains(type.Assembly.FullName)) {
                    ConfigureAlias (type);
                    namespacesDone.Add (type.Namespace);
                    assembliesDone.Add (type.Assembly.FullName);
                }
            }
        }

        #endregion


        public override IEnumerable<TItem> Where(System.Linq.Expressions.Expression<Func<TItem, bool>> predicate) {
            return Session.AsQueryable<TItem>().Where(predicate);
        }

    }

}
