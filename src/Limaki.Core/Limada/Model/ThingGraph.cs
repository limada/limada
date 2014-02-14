/*
 * Limada 
 * 
 * This code is free software; you can redistribute it and/or modify it
 * under the terms of the GNU General Public License version 2 only, as
 * published by the Free Software Foundation.
 * 
 * Author: Lytico
 * Copyright (C) 2006-2013 Lytico
 *
 * http://www.limada.org
 * 
 */

using Limaki.Graphs;
using Id = System.Int64;
using System.Collections.Generic;
using Limaki.Common.Collections;
using Limaki.Model.Content;
using Limaki.Data;
using System.Linq;
using Limaki.Common;

namespace Limada.Model {

    public class ThingGraph : Graph<IThing, ILink>, IThingGraph {

        #region ID-Handling
        private IDictionary<Id, IThing> _ids = new Dictionary<Id, IThing> ();

        public virtual IThing GetById(Id id) {
            IThing result = null;
            _ids.TryGetValue(id, out result);
            return result;
        }

        protected virtual void AddId(IThing thing) {
            if (thing != null && !_ids.Keys.Contains(thing.Id)) {
                _ids.Add(thing.Id, thing);
            }
        }
        protected virtual bool RemoveId(IThing thing) {
            if (thing != null) {
                return _ids.Remove(thing.Id);
            }
            return false;
        }
        protected virtual void ReplaceId(IThing thing) {
            if (thing != null) {
                _ids[thing.Id]=thing;
            }
        }
        #endregion

        public override void Clear() {
            base.Clear();
            _ids.Clear ();
        }

        protected override void AddEdge(ILink edge, IThing item) {
            base.AddEdge(edge, item);
            AddId (item);
        }

        public override void Add(ILink edge) {
            this.Add (edge.Marker);
            base.Add(edge);
            AddId (edge);
            AddMarker (edge.Marker);
        }

        public override void DoChangeData (IThing item, object data) {
            base.DoChangeData(item, data);
            this.EnsureChangeData(item, data);
        }

        public override bool Remove(ILink edge) {
            bool result = base.Remove(edge);
            RemoveId(edge);
            RemoveMarker (edge.Marker);
            return result;
        }

        public override void Add(IThing item) {
            base.Add(item);
            AddId(item);
            var streamThing = item as IStreamThing;
            if (streamThing!=null && streamThing.DataContainer==null) {
                streamThing.DataContainer = this.DataContainer;
            }
        }

        public override bool Remove(IThing item) {
            if (item is IProxy) {
                DataContainer.Remove (item.Id);
            }
            bool result = base.Remove(item);
            RemoveId (item);
            return result;
        }

        public override void OnDataChanged(IThing item) {
            if (item is ILink) {
                AddMarker (( (ILink) item ).Marker);
            }
            base.OnDataChanged(item);
        }

        #region Marker-Handling

        private ICollection<Id> _markerIds = null;
        protected ICollection<Id> markerIds {
            get {
                if (_markerIds == null) {
                    _markerIds = new Set<Id>();
                    foreach(ILink link in this.Edges()) {
                        AddMarker (link.Marker);
                    }
                }
                return _markerIds;
            }
            set { _markerIds = value; }
        }



        public virtual void AddMarker(IThing marker) {
            if (marker == null) return;
            if (!markerIds.Contains(marker.Id)) {
                markerIds.Add(marker.Id);
            }
        }

        public virtual void RemoveMarker(IThing marker) {
            if (marker == null) return;
            markerIds.Remove(marker.Id);
        }

        #endregion

        #region IThingGraph Member

        public virtual IEnumerable<IThing> GetByData(object data) {
            foreach (IThing thing in this) {
                if (thing.Data != null && thing.Data.Equals(data)) {
                    yield return thing;
                }
            }
        }
        public virtual IEnumerable<IThing> GetByData(object data, bool exact) {
            string search = data.ToString ().ToLower ();
            foreach (IThing thing in this) {
                if (thing.Data != null && thing.Data.ToString().ToLower().StartsWith(search)) {
                    yield return thing;
                }
            }
        }

        public virtual bool IsMarker(IThing thing) {
            if (thing == null) return false;
            return markerIds.Contains (thing.Id);
        }

        public virtual ICollection<IThing> Markers() {
            ICollection<IThing> result = new Set<IThing>();
            foreach (var id in markerIds)
                result.Add(GetById (id));
            return result;
        }

        protected virtual void Replace (IThing oldThing, IThing newThing) {
            var replaceLinks = Edges(oldThing);
            if (!items.Contains(oldThing) && replaceLinks.Count == 0 && items.Contains(newThing))
                return;

            foreach (var link in replaceLinks) {
                var llink = (ILink<Id>) link;
                if (llink.Root == oldThing.Id) {
                    link.Root = newThing;
                }
                if (llink.Leaf == oldThing.Id) {
                    link.Leaf = newThing;
                }
                if (llink.Marker == oldThing.Id) {
                    link.Marker = newThing;
                }
            }
            var oldLink = oldThing as ILink;
            var newLink = newThing as ILink;
            if (oldLink != null) {
                edges.Remove(oldLink);
                if(!edges.Contains(newLink))
                    edges.Add(newLink);
            }
            items.Remove(oldThing);

            var links = Edges(newThing);
            if (links.Count == 0) {
                if (replaceLinks.Count > 0) {
                    items[newThing] = replaceLinks;
                } else {
                    items[newThing] = null;
                }
            } else {
                foreach (var link in replaceLinks) {
                    Add(link);
                }
            }

            if (markerIds.Contains(oldThing.Id)) {
                foreach (var link in this.edges) {
                    if (((ILink<Id>)link).Marker == oldThing.Id) {
                        link.Marker = newThing;
                    }
                }
            }

            ReplaceId(newThing);
        }

        public virtual IThing UniqueThing (IThing thing) {
            IThing result = thing;
            if (thing != null) {
                var stored = this.GetById(thing.Id);
                if (stored != null && !object.ReferenceEquals(stored, thing)) {
                    Replace(stored, thing);
                }
            }
            return result;
        }
        #endregion

        #region Proxy-Handling
        
        private IDataContainer<Id> _dataContainer = null;
        public virtual IDataContainer<Id> DataContainer {
            get {
                if (_dataContainer == null) {
                    _dataContainer = new DataContainer ();
                }
                return _dataContainer;
            }
            set { _dataContainer = value; }
        }

        #endregion


        #region Linqish


        public IEnumerable<T> Where<T>(System.Linq.Expressions.Expression<System.Func<T, bool>> predicate) where T : IThing {
            if (typeof(T).GetInterface(typeof(ILink).FullName) != null)
                return edges.OfType<T>().Where(predicate.Compile());
            else {
                return items.OfType<T>().Where(predicate.Compile());
            }
        }

        #endregion

        
    }

    public class DataContainer:IDataContainer<Id>{

        private IDictionary<Id, IRealData<Id>> dataList = new Dictionary<Id, IRealData<Id>>();
       
        #region IDataContainer Member

        public bool Contains(IRealData<Id> item) {
            return dataList.ContainsKey (item.Id);
        }

        public void Add(IRealData<Id> item) {
            dataList[item.Id] = item;
        }

        public bool Contains(long id) {
            return dataList.ContainsKey(id);
        }

        public IRealData<Id> GetById(Id id) {
            var result = default(IRealData<Id>);
            dataList.TryGetValue (id, out result);
            return result;
        }

        public bool Remove(long id) {
            return dataList.Remove (id);
        }

        public bool Remove(IRealData<Id> item) {
            return Remove (item.Id);
        }

        #endregion
    }
}