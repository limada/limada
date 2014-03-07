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

#define ShowHiddensNo

using System.Collections.Generic;
using Limada.Model;
using Limaki.Common.Collections;
using Limaki.Data;
using Limaki.Graphs;
using Id = System.Int64;
using System.Linq.Expressions;
using System.Linq;
using Limaki.Common;

namespace Limada.Schemata {

    public class SchemaThingGraph:FilteredGraph<IThing,ILink>,IThingGraph {
        public SchemaThingGraph(IThingGraph source):base(source) {
            this.EdgeFilter = SchemaEdgeFilter;
            this.ItemFilter = SchemaItemFilter;
            Initialize ();
        }

        static bool useHiddens = 
#if ShowHiddens
            false;
#else
        true;
#endif
        public void Initialize() {
            
            SchemaFacade.InitSchemata ();
            descriptions.Clear ();
            hiddens.Clear ();
            describedMarkers.Clear ();

            descriptions.Add(CommonSchema.DescriptionMarker.Id);
            hiddens.Add (CommonSchema.DescriptionMarker.Id);
            

            IThingGraph markerGraph = new ThingGraph ();
            var markers = Markers ();
            GraphExtensions.MergeInto(Schema.IdentityGraph, markerGraph);
            ThingGraph.DeepCopy(markers, markerGraph);
            markerGraph.DeepCopy(markers, ThingGraph);

            foreach(IThing marker in markerGraph) {
                var markerId = marker.Id;
                foreach(ILink link in markerGraph.Edges(marker)) {
                    if (link.Marker == null)
                        continue;
                    
                    var idLink = (ILink<Id>)link;

                    if (useHiddens && 
                        idLink.Marker == ViewMetaSchema.Hide.Id &&
                        idLink.Leaf == markerId) {
                        hiddens.Add(markerId);    
                    }

                    if (idLink.Marker == MetaSchema.DescriptionMarker.Id) {
                        if (idLink.Leaf == markerId) {
                            hiddens.Add(markerId);
                            descriptions.Add(markerId);
                        } else {
                            describedMarkers[markerId] = idLink.Leaf;
                        }
                    }
                }
            }
        }

        private ICollection<Id> hiddens = new Set<Id> ();
        private ICollection<Id> descriptions = new Set<Id>();

		public ICollection<Id> Hiddens {
			get {return hiddens;}
		}
		
        /// <summary>
        /// Markers that have a description (= a link with a marker of MetaSchema.DescriptionMarker.Id)
        /// </summary>
        private IDictionary<Id,Id> describedMarkers = new Dictionary<Id,Id>();

        public virtual bool SchemaEdgeFilter(ILink link) {
            if (link == null) return false;
            var idLink = (ILink<Id>)link;
            return !hiddens.Contains(idLink.Marker);
                
            //if (idLink.Marker == 0 || 
            //    (! descriptions.Contains(idLink.Marker) && ! (Source.RootIsEdge(link))))
            //    return true;
            //return !hiddens.Contains(idLink.Marker);
        }

        public virtual bool SchemaItemFilter(IThing thing) {
            if (thing == null) return false;
            bool result = true;
            foreach (ILink link in Source.Edges(thing)) {
                var idLink = (ILink<Id>)link;
                if (idLink.Marker != 0) {
                    if (idLink.Leaf == thing.Id && descriptions.Contains(idLink.Marker)) {
                        return !SchemaFacade.DescriptionableThing(link.Root);
                    }
                }
            }
            return result;
        }

        public virtual IEnumerable<ILink> ThingToDisplayPath(IThing item) {
            // REMARK: this is duplicate code with ThingToDisplay!!
            var path = new Stack<ILink> ();
            IThing result = item;
            if (item == null)
                return null;
            ILink linkResult = null;
            ICollection<IThing> itemDone = new Set<IThing>();
            var stack = new Stack<IThing>();
            if (SchemaFacade.DescriptionableThing(item))
                stack.Push(item);
            while (stack.Count > 0) {
                result = stack.Pop();
                if (itemDone.Contains(result))
                    continue;
                itemDone.Add(result);
                foreach (ILink link in Source.Edges(result)) {
                    var resultId = result.Id;
                    var idLink = (ILink<Id>)link;
                    if (idLink.Marker == 0)
                        continue;
                    if (idLink.Root == resultId) {
                        if (descriptions.Contains(idLink.Marker)) {
                            result = link.Leaf;
                            path.Push (link);
                            stack.Clear();
                            break;
                        }
                    } else if (idLink.Leaf == resultId) {
                        if (describedMarkers.ContainsKey(idLink.Marker)) {
                            path.Push(link);
                            stack.Push(link);
                            linkResult = link;
                        }
                    }
                }
                if (SchemaFacade.DescriptionableThing(result))
                    stack.Push(result);
            }
            if (result == linkResult)
                return new EmptyCollection<ILink>();
            return path;

        }

        public virtual IThing ThingToDisplay(IThing item) {
            var result = item;
            if (item == null)
                return null;
            ILink linkResult = null;
            ICollection<IThing> itemDone = new Set<IThing>();
            var stack = new Stack<IThing> ();
            if (SchemaFacade.DescriptionableThing(item))
                stack.Push (item);
            while (stack.Count > 0) {
                result = stack.Pop ();
                if (itemDone.Contains(result))
                    continue;
                itemDone.Add(result);
                foreach (var link in Source.Edges(result)) {
                    var resultId = result.Id;
                    var idLink = (ILink<Id>)link;
                    if (idLink.Marker == 0)
                        continue;
                    if (idLink.Root == resultId) {
                        if (descriptions.Contains(idLink.Marker)) {
                            result = link.Leaf;
                            stack.Clear ();
                            break;
                        }
                    } else if (idLink.Leaf == resultId) {
                        if (describedMarkers.ContainsKey(idLink.Marker)) {
                            stack.Push (link);
                            linkResult = link;
                        }
                    }
                }
                if (SchemaFacade.DescriptionableThing(result))
                    stack.Push(result);
            }
            if (result == linkResult)
                return item;
            return result;

        }

        public virtual IThing DescribedThing(IThing item) {
            IThing result = item;
            if (item == null )
                return null;
            ILink linkResult = null;
            ICollection<IThing> itemDone = new Set<IThing>();
            var stack = new Stack<IThing>();
            stack.Push(item);
            while (stack.Count > 0 ) {
                result = stack.Pop ();
                if (itemDone.Contains(result))
                    break;
                itemDone.Add(result);
                foreach (var link in Source.Edges(result)) {
                    var resultId = result.Id;
                    var idLink = (ILink<Id>)link;
                    if (idLink.Marker == 0)
                        continue;
                    if (idLink.Leaf == resultId) {
                        if (descriptions.Contains(idLink.Marker) && SchemaFacade.DescriptionableThing(link.Root)) {
                            result = link.Root;
                            stack.Clear();
                            break;
                        }
                    } else if (idLink.Root == resultId) { 
                        if (describedMarkers.ContainsKey(idLink.Marker) && SchemaFacade.DescriptionableThing(link.Root)) {
                            stack.Push (link);
                            linkResult = link;
                        }
                    }
                }

            }

            while (result is ILink) {
                result = ( (ILink) result ).Leaf;
            }

            if (result == linkResult)
                return item;
            return result;

        }

        public override void DoChangeData(IThing item, object data) {
            var itemToChange = ThingToDisplay(item);
            base.DoChangeData(itemToChange, data);
        }

        #region IThingGraph Member

        public IThingGraph ThingGraph {
            get { return Source as IThingGraph; }
        }

        //public IThingGraph SchemaGraph {
        //    get { return Schema.IdentityGraph; }
        //}

        public IThing GetById(long id) {
            return ThingGraph.GetById (id);
        }

        public bool IsMarker(IThing thing) {
            bool result = ThingGraph.IsMarker (thing);
            return result;
        }

        public ICollection<IThing> Markers() {
            return ThingGraph.Markers ();
        }

        public IEnumerable<IThing> GetByData(object data) {
            return ThingGraph.GetByData (data);
        }

        public IEnumerable<IThing> GetByData(object data, bool exact) {
            return ThingGraph.GetByData(data, exact);
        }

        public virtual IDataContainer<Id> DataContainer {
            get { return ThingGraph.DataContainer; }
            set { ThingGraph.DataContainer = value; }
        }

        public virtual IThing UniqueThing(IThing thing) {
            return ThingGraph.UniqueThing(thing);
        }

        public virtual void AddMarker(IThing thing) {
            ThingGraph.AddMarker (thing);
        }

        #endregion

        public override void Add(ILink edge) {
            Source.Add(edge);
        }
        public override void Add(IThing item) {
            Source.Add(item);
        }

        

        public virtual bool RemoveThingToDisplay(IThing item) {
            var thingToDisplay = ThingToDisplay (item);
            if (thingToDisplay != item) {
                if (Source.HasSingleEdge(thingToDisplay))
                    return Remove(thingToDisplay);
            }
            return false;
        }

        public override bool Remove(ILink edge) {
            RemoveThingToDisplay (edge);
            return Source.Remove(edge);
        }

        public override bool Remove (IThing item) {
            SchemaFacade.Dependencies.VisitItems(
                GraphCursor.Create(this, item),
                d => {
                    RemoveThingToDisplay(d);
                    OnGraphChange(d, GraphEventType.Remove);
                    Source.Remove(d);
                },
                GraphEventType.Remove);
            RemoveThingToDisplay(item);
            return Source.Remove(item);
        }

        
        public IEnumerable<T> Where<T>(Expression<System.Func<T, bool>> predicate) where T : IThing {
            return (Source as IThingGraph).Where<T>(predicate);
        }

        

        #region IEnumerable Member

        public new System.Collections.IEnumerator GetEnumerator() {
            throw new System.NotImplementedException();
        }

        #endregion
    }
}