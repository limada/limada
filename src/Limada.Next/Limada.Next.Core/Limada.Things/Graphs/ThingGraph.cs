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

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Limada.Things.Model;
using Limaki.Common.Collections;
using Limaki.Graphs;
using Limaki.UnitsOfWork.Content;

namespace Limada.Things.Graphs {

    public class ThingGraph : Graph<IThing, ILink>, IThingGraph {

        #region ID-Handling

        private IDictionary<Guid, IThing> _ids = new Dictionary<Guid, IThing> ();

        public virtual IThing GetById (Guid id) {
            _ids.TryGetValue (id, out var result);

            return result;
        }

        protected virtual void AddId (IThing thing) {
            if (thing != null && !_ids.Keys.Contains (thing.Id)) {
                _ids.Add (thing.Id, thing);
            }
        }

        protected virtual bool RemoveId (IThing thing) {
            if (thing != null) {
                return _ids.Remove (thing.Id);
            }

            return false;
        }

        protected virtual void ReplaceId (IThing thing) {
            if (thing != null) {
                _ids[thing.Id] = thing;
            }
        }

        #endregion

        public override void Clear () {
            base.Clear ();
            _ids.Clear ();
            _markerIds = null;
        }

        protected override void AddEdge (ILink edge, IThing item) {
            base.AddEdge (edge, item);
            AddId (item);
        }

        public override void Add (ILink edge) {
            Add (edge.Marker);
            base.Add (edge);
            AddId (edge);
            AddMarker (edge.Marker);
        }

        public override void DoChangeData (IThing item, object data) {
            base.DoChangeData (item, data);
            this.EnsureChangeData (item, data);
        }

        public override bool Remove (ILink edge) {
            var result = base.Remove (edge);
            RemoveId (edge);
            RemoveMarker (edge.Marker);

            return result;
        }

        public override void Add (IThing item) {
            base.Add (item);
            AddId (item);

            if (item is IContentThing ct) {
                // TODO: add content?
            }

        }

        public override bool Remove (IThing item) {
            if (item is IContentThing ct) {
                ContentService.Remove (ct.ContentId);
            }

            var result = base.Remove (item);
            RemoveId (item);

            return result;
        }

        public override void OnGraphChange (object sender, GraphChangeArgs<IThing, ILink> args) {
            if (args.EventType == GraphEventType.Update) {
                if (args.Item is ILink link) {
                    AddMarker (link.Marker);
                }
            }

            base.OnGraphChange (sender, args);
        }

        #region Marker-Handling

        private ICollection<Guid> _markerIds = null;

        protected ICollection<Guid> MarkerIds {
            get {
                if (_markerIds != null) return _markerIds;

                _markerIds = new Set<Guid> ();

                foreach (var link in Edges ()) {
                    AddMarker (link.Marker);
                }

                return _markerIds;
            }
            set => _markerIds = value;
        }

        public virtual void AddMarker (IThing marker) {
            if (marker == null) return;

            if (!MarkerIds.Contains (marker.Id)) {
                MarkerIds.Add (marker.Id);
            }
        }

        public virtual void RemoveMarker (IThing marker) {
            if (marker == null) return;

            MarkerIds.Remove (marker.Id);
        }

        #endregion

        #region IThingGraph Member

        public virtual IEnumerable<IThing> GetByData (object data) {
            foreach (var thing in this) {
                if (thing.Data != null && thing.Data.Equals (data)) {
                    yield return thing;
                }
            }
        }

        public virtual IEnumerable<IThing> GetByData (object data, bool exact) {
            var search = data.ToString ().ToLower ();

            foreach (var thing in this) {
                if (thing.Data != null && thing.Data.ToString ().ToLower ().StartsWith (search)) {
                    yield return thing;
                }
            }
        }

        public virtual bool IsMarker (IThing thing) {
            if (thing == null) return false;

            return MarkerIds.Contains (thing.Id);
        }

        public virtual ICollection<IThing> Markers () {
            ICollection<IThing> result = new Set<IThing> ();

            foreach (var id in MarkerIds)
                result.Add (GetById (id));

            return result;
        }

        protected virtual void Replace (IThing oldThing, IThing newThing) {
            var replaceLinks = Edges (oldThing);

            if (!items.Contains (oldThing) && replaceLinks.Count == 0 && items.Contains (newThing))
                return;

            foreach (var link in replaceLinks) {
                var llink = (ILink<Guid>)link;

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

            var newLink = newThing as ILink;

            if (oldThing is ILink oldLink) {
                edges.Remove (oldLink);

                if (!edges.Contains (newLink))
                    edges.Add (newLink);
            }

            items.Remove (oldThing);

            var links = Edges (newThing);

            if (links.Count == 0) {
                if (replaceLinks.Count > 0) {
                    items[newThing] = replaceLinks;
                } else {
                    items[newThing] = null;
                }
            } else {
                foreach (var link in replaceLinks) {
                    Add (link);
                }
            }

            if (MarkerIds.Contains (oldThing.Id)) {
                foreach (var link in edges) {
                    if (((ILink<Guid>)link).Marker == oldThing.Id) {
                        link.Marker = newThing;
                    }
                }
            }

            ReplaceId (newThing);
        }

        public virtual IThing UniqueThing (IThing thing) {
            var result = thing;

            if (thing == null) return result;

            var stored = GetById (thing.Id);

            if (stored != null && !ReferenceEquals (stored, thing)) {
                Replace (stored, thing);
            }

            return result;
        }

        #endregion

        #region Proxy-Handling

        private IContentService<Guid, Stream> _contentService = null;

        public virtual IContentService<Guid, Stream> ContentService { get => _contentService ??= new ContentContainer<Guid, Stream> (); set => _contentService = value; }

        #endregion

        #region Linqish

        public IEnumerable<T> WhereQ<T> (System.Linq.Expressions.Expression<Func<T, bool>> predicate) where T : IThing {
            if (typeof(T).GetInterface (typeof(ILink).FullName) != null)
                return edges.OfType<T> ().Where (predicate.Compile ());

            return items.OfType<T> ().Where (predicate.Compile ());
        }

        #endregion

    }

}