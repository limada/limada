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
using Id = System.Int64;
using Limaki.Common;
using Limaki.Graphs;
using System.Runtime.Serialization;

namespace Limada.Model {

    [DataContract]
    public class Link : Thing, IThing<IThing>, ILink, ILink<Id> {

        public Link () { }

        public Link (IThing marker): base () {
            this.Marker = marker;
        }

        public Link (Id id, IThing marker): base (id) {
            this.Marker = marker;
        }

        public Link (Id id, Id marker): base (id) {
            this.MarkerId = marker;
        }

        public Link (IThing root, IThing leaf, IThing marker) : this (marker) {
            this.Root = root;
            this.Leaf = leaf;
        }

        public Link (Id root, Id leaf, Id marker) {
            this.RootId = root;
            this.LeafId = leaf;
            this.MarkerId = marker;
        }

        public Link (Id id, IThing root, IThing leaf, IThing marker) : this (id, marker) {
            this.Root = root;
            this.Leaf = leaf;
        }

        public Link (Id id, Id root, Id leaf, Id marker) : this (id, marker) {
            this.RootId = root;
            this.LeafId = leaf;
        }

        IThing _root = null;
        [Transient]
        public IThing Root {
            get { return GetThing (_rootId, ref _root); }
            set { SetThing (ref _rootId, ref _root, value); }
        }


        IThing _leaf = null;
        [Transient]
        public IThing Leaf {
            get { return GetThing (_leafId, ref _leaf); }
            set { SetThing (ref _leafId, ref _leaf, value); }
        }


        IThing _marker = null;
        [Transient]
        public IThing Marker {
            get { return GetThing (_markerId, ref _marker); }
            set { SetThing (ref _markerId, ref _marker, value); }
        }

        [Transient]
        IThing IThing<IThing>.Data {
            get { return Marker; }
            set { Marker = value; }
        }

        public override void MakeEqual (IThing thing) {
            if (thing is ILink) {
                base.MakeEqual (thing);
                var idLink = ((ILink<Id>) thing);
                var idThis = ((ILink<Id>) this);
                idThis.Root = idLink.Root;
                idThis.Leaf = idLink.Leaf;
                idThis.Marker = idLink.Marker;
                this.Root = ((ILink) thing).Root;
                this.Leaf = ((ILink) thing).Leaf;

            }
        }

        protected void SetThing (ref Id id, ref IThing thing, IThing value) {
            if (value == null) {
                thing = value;
                this.State.Setter (ref id, default (Id));
            } else if (value != thing) {
                this.State.Setter (ref id, value.Id);
                thing = value;
            }
        }

        public Func<Id, IThing> GetByID { get; set; }

        protected IThing GetThing (Id id, ref IThing data) {
            if (id == 0) {
                data = null;
            } else if ((GetByID != null) && (data == null)) {
                data = GetByID (id);
            }
            return data;
        }

        #region IEdge<Id> Member

        protected void SetId (ref Id id, ref IThing thing, Id value) {
            if (value != id) {
                thing = null;
                this.State.Setter (ref id, value);
            }
        }

        protected Id _leafId = 0;
        [DataMember (Name = "LeafId")]
        public Id LeafId {
            get { return _leafId; }
            set { SetId (ref _leafId, ref _leaf, value); }
        }

        Id IEdge<Id>.Leaf {
            get { return _leafId; }
            set { SetId (ref _leafId, ref _leaf, value); }
        }

        protected Id _markerId = 0;
        [DataMember (Name = "MarkerId")]
        public Id MarkerId {
            get { return _markerId; }
            set { SetId (ref _markerId, ref _marker, value); }
        }

        Id ILink<Id>.Marker {
            get { return _markerId; }
            set { SetId (ref _markerId, ref _marker, value); }
        }

        protected Id _rootId = 0;
        [DataMember (Name = "RootId")]
        public Id RootId {
            get { return _rootId; }
            set { SetId (ref _rootId, ref _root, value); }
        }

        Id IEdge<Id>.Root {
            get { return _rootId; }
            set { SetId (ref _rootId, ref _root, value); }
        }
        #endregion

        public override string ToString () {
            return String.Format ("[{0}->{1},{2}]", this.Root, this.Leaf, this.Marker);
        }

    }
}