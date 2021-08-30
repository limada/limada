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


using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Limada.Model;
using Limaki.Common;
using Id = System.Int64;
using Limaki.Common.Collections;
using System.Linq;
using Limaki.Graphs;

namespace Limada.Schemata {

    public class Schema {

        public Schema() {
            Init ();
            //if (!IsIdentityDone) { // does not work; must be per schema!
                PopulateGraph (ref _identityGraph);
                if (( _identityGraph.Count != 0 ) && ( _identityMap.Count == 0 )) {
                    throw new ArgumentException (SchemaErrorBuilder (_identityMap.Values));
                }
                //IsIdentityDone = true;
            //}
        }

        public Schema (IThingGraph graph, IThing target):this() {
            this.Graph = graph;
            this.Subject = target;
        }

        # region Identity-Handling (static methods and fields)

        static IThingFactory _factory = null;
        public static IThingFactory Factory {
            get {
                if (_factory == null) {
                    _factory = new ThingFactory ();
                }
                return _factory;
            }
        }

        public static bool IsIdentityDone = false;

        public static DateTime CreationDate { get; set; }

        //TODO: remove? as ThingGraph has an id-dictionary; use IdentityGraph instead?

        protected static IDictionary<Id, IThing> _identityMap = new Dictionary<Id, IThing>();
        /// <summary>
        /// contains all IThings of all instantiated Schema's
        /// </summary>
        public static IDictionary<Id, IThing> IdentityMap {
            get { return _identityMap; }
        }

        protected static IThingGraph _identityGraph = null;
        /// <summary>
        /// contains all IThings of all instantiated Schema's
        /// </summary>
        public static IThingGraph IdentityGraph {
            get { return _identityGraph; }
        }

        /// <summary>
        /// creates a IThing with the id and stores the IThing in the IdentityMap
        /// or
        /// gives back the ITing with the id from IdentityMap
        /// </summary>
        protected static IThing Thing(ulong id) {
            IThing result = null;
            var mId = unchecked((Id)id);
            _identityMap.TryGetValue(mId, out result);
            if (result == null) {
                result = Factory.CreateIdItem(mId);
                result.SetCreationDate(CreationDate);
                _identityMap.Add(result.Id, result);
            }
            return result;
        }

        /// <summary>
        /// creates a IThing with the id and data
        /// and stores the IThing in the Schema.IdentityMap
        /// or
        /// gives back the ITing with the id from Schema.IdentityMap
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        protected static IThing Thing<T>(T data, ulong id) {
            IThing result = null;
            var mId = unchecked((Id)id);
            _identityMap.TryGetValue(mId, out result);
            if (result == null) {
                result = Factory.CreateItem<T> (mId,data);
                result.SetCreationDate(CreationDate);
                _identityMap.Add(result.Id, result);
            }
            return result;
        }

        /// <summary>
        /// creates a ILink with the id and stores the IThing in the Schema.IdentityMap
        /// or
        /// gives back the ILink with the id from IdentityMap
        protected static ILink Link(IThing root, IThing leaf, IThing marker, ulong id) {
            IThing result = null;
            var mId = unchecked((Id)id);
            _identityMap.TryGetValue(mId, out result);
            if (result == null) {
                result = Factory.CreateEdge(mId,root,leaf,marker);
                result.SetCreationDate(CreationDate);
                _identityMap.Add(result.Id, result);
            }
            return (ILink)result;
        }

        protected virtual string SchemaErrorBuilder(IEnumerable<IThing> things) {
            var result = new StringWriter();
            result.WriteLine("Something is wrong with the structure in Schema\t" + this.GetType().FullName);
            foreach (var eThing in things) {
                result.WriteLine(eThing.ToString());
            }
            return result.ToString();
        }

        protected virtual void Init () { }

        protected virtual void PopulateGraph(ref IThingGraph graph) {
            if (graph == null) {
                graph = new ThingGraph();
            }

            var things = new Queue<IThing>();
            var type = this.GetType();

            foreach (var info in type.GetFields()) {
                var attr = Attribute.GetCustomAttribute(info, typeof(UniqueThingAttribute));
                if (attr is UniqueThingAttribute) {
                    var value = info.GetValue(this) as IThing;
                    if (value != null) {
                        things.Enqueue(value);
                    }
                }
            }

            // activating properties with UniqueThingAttribute
            // they should call Thing() or Link(), otherwise it is no garantie of uniqueness
            foreach (var info in type.GetProperties()) {
                var attr = Attribute.GetCustomAttribute(info, typeof(UniqueThingAttribute));
                if (attr is UniqueThingAttribute) {
                    var value = info.GetValue(this, null) as IThing;
                    if (value != null) {
                        things.Enqueue(value);
                    }
                }
            }
            foreach (var thing in IdentityMap.Values) {
                things.Enqueue (thing);
            }

            int tries = things.Count * 1000;
            while (things.Count != 0) {
                var thing = things.Dequeue();
                if (!(thing is ILink)) {
                    graph.Add(thing);
                } else {
                    var link = (ILink)thing;
                    if (graph.Contains(link.Root)
                        && graph.Contains(link.Leaf)
                        && graph.Contains(link.Marker)) {
                        graph.Add(link);
                    } else {
                        things.Enqueue (thing);
                    }
                }
                tries--;
                if (tries < 0) {
                    throw new ArgumentException(SchemaErrorBuilder(things));
                }
            }

        }
        #endregion

        /// <summary>
        /// Gives back a Graph filled with the predefined Things of the Schema
        /// </summary>
        public IThingGraph SchemaGraph {
            get {
                IThingGraph result = null;
                PopulateGraph(ref result);
                return result;
            }
        }

        #region Data-Handling (Schema as an instance)
        
        #region marker-cache

        private IDictionary<Id, IMultiDictionary<Id, Id>> markerCache = new Dictionary<Id, IMultiDictionary<Id, Id>>();
        /// <summary>
        /// cache[markerId,targetId].Add(valueId)
        /// </summary>
        /// <param name="markerId"></param>
        /// <param name="targetId"></param>
        /// <param name="valueId"></param>
        protected void AddInCache(Id markerId, Id targetId, Id valueId) {
            IMultiDictionary<Id, Id> targets = null;
            markerCache.TryGetValue (markerId, out targets);
            if (targets == null) {
                targets = new MultiDictionary<Id, Id>();
                markerCache[markerId] = targets;
            }
            targets.Add (targetId, valueId);
            
        }

        /// <summary>
        /// ICollection#id of cache[markerId,targetId]
        /// </summary>
        /// <param name="markerId"></param>
        /// <param name="targetId"></param>
        /// <returns></returns>
        protected ICollection<Id> GetCached(Id markerId, Id targetId) {
            IMultiDictionary<Id, Id> targets = null;
            markerCache.TryGetValue(markerId, out targets);
            if (targets != null) {
                return targets[targetId];
            }
            return new EmptyCollection<Id>();
        }

        /// <summary>
        /// look up to the first foundId in
        /// ICollection#id cache[markerId,targetId]
        /// returns Graph.GetById(foundId)
        /// </summary>
        /// <param name="markerId"></param>
        /// <param name="targetId"></param>
        /// <returns></returns>
        protected IThing GetTheCached(Id markerId, Id targetId) {
            ICollection<Id> cache = GetCached(markerId, targetId);
            if (cache.Count != 0) {
                foreach (Id id in cache) {
                    return Graph.GetById(id);
                }
            }
            return null;
        }

        /// <summary>
        /// sets cache[markerId,targetId] to valueId
        /// all other cache[markerId,targetId] are deleted in the cache
        /// </summary>
        /// <param name="markerId"></param>
        /// <param name="targetId"></param>
        /// <param name="valueId"></param>
        protected void SetTheCached(Id markerId, Id targetId, Id valueId) {
            ICollection<Id> cache = GetCached(markerId, targetId);
            if (cache.Count == 0) {
                AddInCache(markerId, targetId, valueId);
            } else {
                cache.Clear();
                cache.Add(valueId);
            }
        }

        #endregion

        public virtual IThing GetTheLeaf(IGraph<IThing, ILink> graph, IThing thing, IThing marker) {
            var link = GetTheLeafEdge (graph, thing, marker);
            if (link != null)
                return link.Leaf;
            return null;
        }

        /// <summary>
        /// gives back the leaf of a link with link.Marker==marker 
        /// in graph.Edges(thing)
        /// </summary>
        /// <param name="graph"></param>
        /// <param name="thing"></param>
        /// <param name="marker"></param>
        /// <returns></returns>
        public virtual ILink GetTheLeafEdge (IGraph<IThing, ILink> graph, IThing thing, IThing marker) {
            if (graph != null && thing != null && marker != null)
                foreach (var link in graph.Edges(thing)) {
                    if ((link.Marker != null && link.Marker.Id == marker.Id) && (link.Root == thing)) {
                        return link;
                    }
                }
            return null;
        }

        public virtual ILink GetTheRootEdge(IThingGraph graph, IThing thing, IThing marker) {
            if (graph != null && thing != null && marker != null)
                foreach (var link in graph.Edges(thing)) {
                    if ((link.Marker!=null)&&(link.Marker.Id == marker.Id) && (link.Leaf == thing)) {
                        return link;
                    }
                }
            return null;
        }

        public virtual IThing GetTheRoot(IThingGraph graph, IThing thing, IThing marker) {
            var link = GetTheRootEdge(graph, thing, marker);
            if (link != null)
                return link.Root;
            return null;
        }

        /// <summary>
        /// returns the first link.leaf with link.Marker == marker
        /// in this.Graph.Edges(this.Target)
        /// </summary>
        /// <param name="marker"></param>
        /// <returns></returns>
        public virtual IThing GetTheLeaf(IThing marker) {
            var result = GetTheCached(marker.Id, Subject.Id);
            if (result != null) {
                return result;
            } else {
                result = GetTheLeaf(this.Graph, this.Subject, marker);
                if (result != null)
                    AddInCache(marker.Id, Subject.Id, result.Id);
                return result;
            }
        }

        /// <summary>
        /// Sets the leaf of the first link with link.Marker==marker
        /// if there is already a link with link.Marker==marker && link.Root==this.Thing,  
        /// change the link.Leaf to value
        /// if the old link.Leaf is an Orphan, delete it
        /// in graph.Edges(thing)
        /// </summary>
        /// <param name="graph"></param>
        /// <param name="thing"></param>
        /// <param name="marker"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public virtual ILink SetTheLeaf(IThingGraph graph, IThing thing, IThing marker, IThing value) {
            if (graph != null && thing != null && marker != null && value != null) {
                foreach (var link in graph.Edges (thing)) {
                    if (( link.Marker.Id == marker.Id ) && ( link.Root == thing )) {
                        var old = link.Leaf;
                        graph.ChangeEdge (link, value, false);
                        // test if old is an orphan; 
                        // if so, and old is a string, delete it:
                        if ((old is IThing<string>) && graph.Edges(old).Count == 0) {
                            graph.Remove(old);
                        }
                        return link;
                    }
                }
                var newLink = new Link (thing, value, marker);
                graph.Add(newLink);
                return newLink;
            }
            return null;
        }

        /// <summary>
        /// Sets the first link.Leaf with link.Marker == marker
        /// in this.Graph.Edges(this.Target)        
        /// </summary>
        /// <param name="marker"></param>
        /// <param name="value"></param>
        public virtual void SetTheLeaf(IThing marker, IThing value) {
            var result = GetTheCached(marker.Id, Subject.Id);
            if (result == null || result != value) {
                SetTheLeaf(this.Graph, this.Subject, marker, value);
                SetTheCached(marker.Id, Subject.Id, value.Id);
            }
        }

        /// <summary>
        /// the Graph on which the operations are performed
        /// </summary>
        public virtual IThingGraph Graph { get; set; }

        /// <summary>
        /// the thing on which the operations are performed
        /// </summary>
        public IThing Subject { get; set; }


        #endregion


        #region UUID-Helper
        /// <summary>
        /// Gives a list of fresh generated Id's out to the console
        /// usefull if you need constant Id's for schemata
        /// </summary>
        public static string GenerateIDs() {
            int count = 20;
            var s = new StringWriter();
            for (int i = 1; i <= count; i++) {
                s.Write("0x" + Isaac.Asaac.longval().ToString("X16") + "\t");
                if ((i % 5) == 0) {
                    s.WriteLine();
                }
            }
            return s.ToString();
        }

        #endregion

    }
}