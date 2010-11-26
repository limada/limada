/*
 * Limada 
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

#define ShowHiddens

using System.Collections.Generic;
using Limada.Model;
using Limaki.Common.Collections;
using Limaki.Graphs;
using Id = System.Int64;
using Limaki.Data;



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

            descriptions.Add(CommonSchema.DescriptionMarker.Id);
            hiddens.Add (CommonSchema.DescriptionMarker.Id);
            

            IThingGraph markers = new ThingGraph ();
            GraphUtils.MergeGraphs<IThing, ILink> (Schema.IdentityGraph, markers);
            ThingGraphUtils.DeepCopy (ThingGraph, Markers (), markers);

            foreach(IThing marker in markers) {
                var markerId = marker.Id;
                foreach(ILink link in markers.Edges(marker)) {
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
                            descriptedMarkers[markerId] = idLink.Leaf;
                        }
                    }
                }
            }
        }

        private ICollection<Id> hiddens = new Set<Id> ();
        private ICollection<Id> descriptions = new Set<Id>();
        private IDictionary<Id,Id> descriptedMarkers = new Dictionary<Id,Id>();

        public virtual bool SchemaEdgeFilter(ILink link) {
            if (link == null) return false;
            var idLink = (ILink<Id>)link;
            if (idLink.Marker == 0 || 
                (! descriptions.Contains(idLink.Marker) && ! (Source.RootIsEdge(link))))
                return true;
            return !hiddens.Contains(idLink.Marker);
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

        public virtual IThing ThingToDisplay(IThing item) {
            IThing result = item;
            if (item == null)
                return null;
            ILink linkResult = null;
            ICollection<IThing> itemDone = new Set<IThing>();
            while (SchemaFacade.DescriptionableThing(result)||(result is ILink)) {
                if (itemDone.Contains(result))
                    break;
                itemDone.Add(result);
                foreach (ILink link in Source.Edges(result)) {
                    var resultId = result.Id;
                    var idLink = (ILink<Id>)link;
                    if (idLink.Marker == 0)
                        continue;
                    if (idLink.Root == resultId) {
                        if (descriptions.Contains(idLink.Marker)) {
                            result = link.Leaf;
                            break;
                        }
                    } else if (idLink.Leaf == resultId) {
                        if (descriptedMarkers.ContainsKey(idLink.Marker)) {
                            result = link;
                            linkResult = link;
                            break;
                        }
                    }
                }
            }
            if (result == linkResult)
                return item;
            return result;

        }

        public override void OnChangeData(IThing item, object data) {
            IThing itemToChange = ThingToDisplay(item);
            base.OnChangeData(itemToChange, data);
        }

        #region IThingGraph Member

        public IThingGraph ThingGraph {
            get { return Source as IThingGraph; }
        }

        public IThingGraph SchemaGraph {
            get { return Schema.IdentityGraph; }
        }

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

        #endregion

        public override void Add(ILink edge) {
            Source.Add(edge);
        }
        public override void Add(IThing item) {
            Source.Add(item);
        }

        public virtual bool RemoveThingToDisplay(IThing item) {
            IThing description = ThingToDisplay (item);
            if (description != item) {
                int i = 0;
                foreach(ILink link in Source.Edges(description)) {
                    if ( i>0 ) break;
                    i++;
                }
                if (i <= 1)
                    return Remove(description);
            }
            return false;
        }
        public override bool Remove(ILink edge) {
            RemoveThingToDisplay (edge);
            return Source.Remove(edge);
        }

        public override bool Remove(IThing item) {
            RemoveThingToDisplay(item);
            return Source.Remove(item);
        }
    }
}