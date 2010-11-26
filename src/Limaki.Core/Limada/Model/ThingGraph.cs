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

using Limaki.Graphs;
using Id = System.Int64;
using System.Collections.Generic;
using Limaki.Common.Collections;
using Limaki.Model.Streams;
using Limaki.Data;

namespace Limada.Model {
    public class ThingGraph:Graph<IThing,ILink>,IThingGraph {

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

        #endregion


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

        public override bool Remove(ILink edge) {
            bool result = base.Remove(edge);
            RemoveId(edge);
            RemoveMarker (edge.Marker);
            return result;
        }

        public override void Add(IThing item) {
            base.Add(item);
            AddId (item);
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
        protected ICollection<long> markerIds {
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



        protected virtual void AddMarker(IThing marker) {
            if (marker == null) return;
            if (!markerIds.Contains(marker.Id)) {
                markerIds.Add(marker.Id);
            }
        }

        protected virtual void RemoveMarker(IThing marker) {
            if (marker == null) return;
            markerIds.Remove(marker.Id);
        }

        #endregion

        #region IThingGraph Member

        public virtual IEnumerable<IThing> GetByData(object data) {
            foreach (IThing thing in this) {
                if (thing.Data.Equals(data)) {
                    yield return thing;
                }
            }
        }
        public virtual IEnumerable<IThing> GetByData(object data, bool exact) {
            string search = data.ToString ().ToLower ();
            foreach (IThing thing in this) {
                if (thing.Data.ToString().ToLower().StartsWith(search)) {
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
            foreach (Id id in markerIds)
                result.Add(GetById (id));
            return result;
        }

        public virtual IThing UniqueThing(IThing thing) {
            return thing;
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

    }

    public class DataContainer:IDataContainer<Id>{
        private IDictionary<Id, IRealData<Id>> dataList = 
            new Dictionary<Id, IRealData<Id>>();
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
            return dataList[id];
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