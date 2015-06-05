/*
 * Limada 
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
using Db4objects.Db4o.Constraints;
using Db4objects.Db4o.Events;
using Db4objects.Db4o.Ext;
using Db4objects.Db4o.Foundation;
using Db4objects.Db4o.Linq;
using Limada.Model;
using Limaki.Common.Collections;
using Limaki.Common.Reflections;
using Limaki.Contents;
using Limaki.Data;
using Limaki.Data.db4o;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Id = System.Int64;
using Limaki.Graphs;

namespace Limada.IO.db4o {

    public class ThingGraph : Limaki.Data.db4o.Graph<IThing, ILink>, IThingGraph {

        public ThingGraph(IGateway g) : base(g) { }

        #region Configuration

        public static readonly string RootIdField = "_rootId";
        public static readonly string LeafIdField = "_leafId";

        // Attention! Here we use Link and NOT ILink as of query-index-usage
        public static readonly Type LinkType = typeof (Link);

        protected override void DeclareTypesToConfigure() {
            
            //Configuration.BTreeCacheHeight(10);
            //Configuration.BTreeNodeSize(1000);

            base.DeclareTypesToConfigure();

            TypesToConfigure.Add (typeof (Thing));
            TypesToConfigure.Add (typeof (Thing<string>));
            TypesToConfigure.Add (typeof (StringThing));
            TypesToConfigure.Add (typeof (NodeThing));
            TypesToConfigure.Add (typeof (Link));
            TypesToConfigure.Add (typeof (StreamThing));

            TypesToConfigure.Add (typeof (RealData));
            TypesToConfigure.Add (typeof (RealData<byte[]>));


        }

        public override bool IsClassConfigurable(Type type) {
            bool result = base.IsClassConfigurable(type);
            return result || Reflector.Implements(type,typeof(IIdContent<Id>));
        }

        protected override bool RefactorType(Type type) {
            var result = false;
            var clazz = Configuration.ObjectClass(type);
            if (Reflector.Implements(type, typeof(IThing))) {
                var writeDate = clazz.ObjectField("_writeDate");
                var changeDate = clazz.ObjectField("_changeDate");
                if (writeDate != null) {
                    writeDate.Rename("_changeDate");
                    result = true;
                }
            }
            return result;
        }

        protected override void ConfigureType(Type type) {
            
            var clazz = Configuration.ObjectClass(type);
            clazz.MaximumActivationDepth(15);
            clazz.UpdateDepth(1);
            
            if ( Reflector.Implements(type, typeof(IThing)) || 
                 Reflector.Implements(type, typeof(IIdContent<Id>)) ) {

                clazz.ObjectField("_id").Indexed(true);
                
                // the following makes errors on closing (sometimes): see Gateway.Close(); this.Flush();
                // Configuration.Add(new UniqueFieldValueConstraint(type, "_id"));

            }

            if (Reflector.Implements(type, typeof(ILink))) {
                clazz.UpdateDepth(1);
                clazz.ObjectField(RootIdField).Indexed(true);
                clazz.ObjectField(LeafIdField).Indexed(true);
                clazz.ObjectField("_markerId").Indexed(true);
            } else {
                FieldInfo fieldType =
                    type.GetField("_data", BindingFlags.Instance | BindingFlags.NonPublic);
                
                if (fieldType != null && ! Reflector.IsStorable(fieldType.FieldType)) {
                    clazz.ObjectField("_data").Indexed(true);
                }
            }

        }

        protected override void ConfigureSession(IObjectContainer session) {
            base.ConfigureSession(session);

            var registry = EventRegistryFactory.ForObjectContainer(session);
            registry.Activated += new EventHandler<ObjectInfoEventArgs>(ActivateThing);
            registry.Created += new EventHandler<ObjectInfoEventArgs>(ThingCreated);
        }

  
        #endregion
        
        #region Graph

        public override void Add( IThing item ) {
            if (item == null)
                return;
            try {
                base.Add(this.UniqueThing(item));
                item.State.Clean = true;
                //Session.Commit();
            } catch (UniqueFieldValueConstraintViolationException ) {
                Session.Rollback();
                //Add(this.UniqueThing(item));
            }
        }

        public override void DoChangeData (IThing item, object data) {
            if (ChangeData != null) {
                ChangeData(this, item, data);
            }
            this.EnsureChangeData (item, data);
        }
        
        void CheckProxy(object o) {
            if (o is IStreamThing) {
                ((IStreamThing)o).ContentContainer = this.ContentContainer;
            }
        }

        protected virtual void ThingCreated(object sender, ObjectInfoEventArgs args) {
            CheckProxy (args.Object);
        }

        protected virtual void ActivateThing(object sender, ObjectInfoEventArgs args) {
            var thing = args.Object as IThing;
            if (thing != null) {
                thing.State.Clean = true;
            }
            if (args.Object is Link) {
                var link = (Link)args.Object;
                link.GetByID = GetById;
                //ILink<Id> idLink = (ILink<Id>)args.Object;
                //link.Leaf = GetById(idLink.Leaf);
                //link.Root = GetById(idLink.Root);
                //link.Marker = GetById(idLink.Marker);
            } else {
                CheckProxy(args.Object);    
            }
        }

        protected override ICollection<ILink> EdgesOf(IThing item) {
            NativeQueryWrapper<ILink> result = null;
            if (item != null) 
            try {

                var query = Session.Query();
                
                query.Constrain(LinkType);

                query.Descend(RootIdField).Constrain(item.Id);
                result = new NativeQueryWrapper<ILink>(query.Execute());
                
                query = Session.Query();
                query.Constrain (LinkType);
                query.Descend(LeafIdField).Constrain(item.Id);
                result.AddSet (query.Execute());
                

            } catch (Exception e) {
                throw e;
            } finally { }
            return result;
        }

        public override void OnGraphChange (object sender, GraphChangeArgs<IThing, ILink> args) {
            if (args.EventType == GraphEventType.Update) {
                if (args.Item is ILink)
                    // maybe this can be done better with
                    // IObjectTranslator..::OnStore or 
                    // with callbacks:
                    // IObjectCallbacks..::ObjectCanUpdate
                    // IObjectCallbacks..::ObjectOnUpdate
                    markerVisitor = null;
            }
            base.OnGraphChange (sender, args);
        }
    
        public override void Add( ILink edge ) {
            if (edge != null) {
                markerVisitor = null;
                Add(edge.Marker);
                
                try {
                    base.Add((ILink) this.UniqueThing(edge));
                    edge.State.Clean = true;
                    //Session.Commit();
                } catch (UniqueFieldValueConstraintViolationException ) {
                    Session.Rollback();
                } finally {}

                
            }
        }

        public override bool Remove(ILink edge) {
            markerVisitor = null;
            var result =  base.Remove(edge);
            edge.State.Hollow = true;
            return result;
        }

        public override bool Remove(IThing item) {
            if (item == null)
                return false;
            if (item is IProxy) {
                ContentContainer.Remove (item.Id);
            }
            var result = base.Remove(item);
            item.State.Hollow = true;
            return result;
        }

        #region performance enhancements

        public override IThing Adjacent(ILink edge, IThing item) {
            if (item != null && edge != null) {
                var idEdge = (ILink<Id>)edge;
                var idItem = item.Id;
                if (idEdge.Root.Equals(idItem))
                    return edge.Leaf;
                else if (idEdge.Leaf.Equals(idItem))
                    return edge.Root;
                else
                    return null;
            } else {
                return null;

            }
        }

        public override bool HasSingleEdge (IThing item) {
            var result = false;
            if (item != null) {
                var links = GetCached (item);
                if (links == null) {
                    try {
                        var query = Session.Query();

                        query.Constrain (LinkType);

                        query.Descend (LeafIdField).Constrain (item.Id);
                        var count = query.Execute().Count;
                        if (count <= 1) {
                            query.Descend (RootIdField).Constrain (item.Id);
                            count += query.Execute().Count;
                        }
                        result = count == 1;
                    } catch (Exception e) {
                        throw e;
                    } finally {}
                } else {
                    return links.Count == 1;
                }
            }
            return result;
        }
        //protected virtual bool IsEdge(bool root, ILink<Id> edge) {
        //    if (edge!= null)
        //        try {
        //            IQuery query = Session.Query();
        //            // Attention! Here we use Link and NOT ILink as of query-index-usage
        //            query.Constrain(typeof(Link));

        //            if (root) {
        //                query.Descend ("_id").Constrain (edge.Root);
        //            } else {
        //                query.Descend("_id").Constrain(edge.Leaf);
        //            }
        //            return query.Execute ().HasNext();

        //        } catch (Exception e) {
        //            throw e;
        //        } finally { }
        //    return false;
        //}

        //public override bool LeafIsEdge(ILink curr) {
        //    return IsEdge(false,(ILink<Id>)curr);
        //}

        //public override bool RootIsEdge(ILink curr) {
        //    return IsEdge(true, (ILink<Id>)curr);
        //}
        #endregion

        #endregion

        #region Marker-Handling
        public class IDVisitor : IVisitor4 {
            IThingGraph graph = null;
            public ICollection<Id> Ids = new Set<Id>();
            public IDVisitor(IThingGraph graph) {
                this.graph = graph;
            }
            public void Visit(object o) {
                if (o is Id) {
                    Id id = (Id)o;
                    if (!Ids.Contains(id))
                        Ids.Add(id);
                }
            }
        }

        private IDVisitor _markerVisitor=null;
        protected IDVisitor markerVisitor {
            get {
                if (_markerVisitor == null) {
                    _markerVisitor = new IDVisitor(this);
                    // Attention! Here we use Link and NOT ILink as of query-index-usage
                    IStoredClass storedClass = Session.Ext().StoredClass(typeof(Link));
                    if (storedClass != null) {
                        IStoredField storedField = storedClass.StoredField ("_markerId", typeof (Id));
                        if (storedField.HasIndex ())
                            storedField.TraverseValues (_markerVisitor);
                    }
                }
                return _markerVisitor;
            }
            set { _markerVisitor = value; }
        }
        
        #endregion

        #region IThingGraph Member

        public virtual bool IsMarker(IThing item) {
            bool result = false;
            if (item != null)
                try {
                    if (pureLinq) {
                        result = markerVisitor.Ids.Contains(item.Id);
                    } else {
                        var query = Session.Query();
                        // Attention! Here we use Link and NOT ILink as of query-index-usage
                        query.Constrain(typeof(Link));

                        query.Descend("_markerId").Constrain(item.Id);
                        var set = query.Execute();
                        result = set.Ext().HasNext();
                    }

                } catch (Exception e) {
                    throw e;
                } finally { }
            return result;
        }

        public virtual ICollection<IThing> Markers() {
            var result = new Set<IThing>();
            foreach (Id id in markerVisitor.Ids) {
                IThing marker = GetById (id);
                if (marker != null) {
                    result.Add (marker);
                }
            }
            return result;
        }
        
        public virtual void AddMarker(IThing thing) {
            this.Add (thing);
        }

        bool pureLinq = false;
        public virtual IThing GetById(Id id) {
            IThing result = null;
            try {
                if (pureLinq) {
                    result = Session.AsQueryable<IThing>().Where(e => e.Id == id).FirstOrDefault();
                } else {
                    var query = Session.Query();
                    //Attention! Here we use Thing and NOT IThing as of query-index-usage
                    query.Constrain(typeof(Thing));
                    query.Descend("_id").Constrain(id);
                    var set = query.Execute();
                    if (set.HasNext())
                        result = set.Next() as IThing;
                }
            } catch (Exception e) {
                throw e;
            } finally { }
            return result;
        }

        public virtual IThing UniqueThing(IThing thing) {
            IThing result = thing;
            if (thing != null && !Session.Ext().IsStored(thing)) {
                IThing stored = this.GetById(thing.Id);
                if (stored != null) {
                    long id = Session.Ext().GetID(stored);
                    Session.Ext().Bind(thing, id);
                }
            }
            return result;
        }

        public IEnumerable<IThing> GetByData(object data) {
            try {
                var query = Session.Query();
                // Attention! Here we use Thing and NOT IThing as of query-index-usage
                query.Constrain(typeof(Thing));
                query.Descend("_data").Constrain(data);
                var set = query.Execute();
                return set.Cast<IThing>();
                // not working:
                //var result = Session.AsQueryable<IThing>().WhereQ(e => e.Data != null && e.Data==data);
                //return result.Cast<IThing>();
            } catch (Exception e) {
                throw e;
            } finally { }
        }

        public IEnumerable<IThing> GetByData(object data, bool exact) {
            if (data == null)
                return new IThing[0];
            if (exact)
                return GetByData (data);
            else {
                if (data is string) {
                    string search = ((string)data).ToLower();
                    var result = Session.AsQueryable<IThing<string>>().Where(e => e.Data != null && e.Data.ToLower().StartsWith(search));
                    return result.Cast<IThing>();
                }
                throw new ArgumentException (data.ToString () + " is not searchable");
            }
            return new IThing[0];
        }

        public class DataVisitor<T> : IVisitor4 {
            IThingGraph graph = null;
            public ICollection<T> searchResults = new Set<T>();
            public Predicate<T> comparer  {get;set;} 
            public Type storedClass {get;set;}
            public IObjectContainer session {get;set;}

            public void Visit(object o) {
                if (o is T) {
                    T data = (T)o;
                    if (!searchResults.Contains(data) && comparer(data))
                        searchResults.Add(data);
                }
            }

            public bool Search() {
                IStoredClass storedClass = session.Ext().StoredClass(this.storedClass);
                if (storedClass != null) {
                    IStoredField storedField = storedClass.StoredField("_data", typeof(T));
                    if (storedField.HasIndex()) {
                        storedField.TraverseValues(this);
                        return true;
                    }
                }
                return false;
            }
        }


        #endregion

        private IContentContainer<Id> _contentContainer = null;
        public virtual IContentContainer<Id> ContentContainer {
            get {
                if (_contentContainer == null) {
                    _contentContainer = new ContentContainer(this.Gateway);
                }
                return _contentContainer;
            }
            set { _contentContainer = value; }
        }

        #region IThingGraph Member


        public IEnumerable<T> WhereQ<T>(System.Linq.Expressions.Expression<Func<T, bool>> predicate) where T : IThing {
            return Session.AsQueryable<T>().Where(predicate);
        }

        #endregion

       
    }
}