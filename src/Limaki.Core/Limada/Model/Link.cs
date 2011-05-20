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
 * http://limada.sourceforge.net
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
        protected Link() { }

        public Link(IThing marker):base() {
            this.Marker = marker;
        }

        public Link(Id id, IThing marker) : base(id) {
            this.Marker = marker;
        }

        public Link(Id id, Id marker): base(id) {
            ((ILink<Id>)this).Marker = marker;
        }

        public Link(IThing root, IThing leaf, IThing marker):this(marker) {
            this.Root = root;
            this.Leaf = leaf;
        }

        public Link(Id root, Id leaf, Id marker) {
            ((ILink<Id>)this).Marker = marker;
            ((ILink<Id>)this).Root = root ;
            ((ILink<Id>)this).Leaf = leaf;
        }

        public Link(Id id, IThing root, IThing leaf, IThing marker): this(id,marker) {
            this.Root = root;
            this.Leaf = leaf;
        }

        public Link(Id id, Id root, Id leaf, Id marker) : this(id, marker) {
            ((ILink<Id>)this).Root = root;
            ((ILink<Id>)this).Leaf = leaf;
        }

        
        IThing _root = null;
        [Transient]
        public IThing Root {
            get { return getThing (_rootId,ref _root); }
            set { setThing (ref _rootId, ref _root, value); }
        }

       
        IThing _leaf = null;
        [Transient]
        public IThing Leaf {
            get { return getThing(_leafId, ref _leaf); }
            set { setThing(ref _leafId, ref _leaf, value); }
        }

        
        IThing _marker = null;
        [Transient]
        public IThing Marker {
            get { return getThing(_markerId, ref _marker); }
            set { setThing(ref _markerId, ref _marker, value); }
        }

        [Transient]
        IThing IThing<IThing>.Data {
            get { return Marker; }
            set { Marker = value; }
        }


        public override void MakeEqual(IThing thing) {
            if (thing is ILink) {
                base.MakeEqual(thing);
                var idLink = ( (ILink<Id>) thing );
                var idThis = ((ILink<Id>)this);
                idThis.Root = idLink.Root;
                idThis.Leaf = idLink.Leaf;
                idThis.Marker = idLink.Marker;
                this.Root = ( (ILink) thing ).Root;
                this.Leaf = ((ILink)thing).Leaf;
                
            }
        }
        //public virtual IThing Adjacent(IThing thing) {
        //    if (thing.Equals(this.Root)) {
        //        return this.Leaf;
        //    } else if (thing.Equals(this.Leaf)) {
        //        return this.Root;
        //    }
        //    return WalkerBase<IThing,ILink>.Adjacent(this, thing); ;
        //}

        protected void setThing(ref Id id, ref IThing data, IThing value) {
            if (value == null) {
                data = value;
                this.State.Setter (ref id, default( Id ));
            } else if (value != data) {
                this.State.Setter(ref id, value.Id);
                data = value;
            }
        }

        public Func<Id,IThing> GetByID = null;

        protected IThing getThing(Id id, ref IThing data) {
           if (id == 0) {
               data = null;
           } else if ((GetByID != null)&&(data==null)) {
               data = GetByID (id);
           }
           return data;
        }

        #region IEdge<Id> Member

        protected void setId(ref Id id, ref IThing data, Id value) {
            if (value != id) {
                data = null;
                this.State.Setter(ref id, value);
            } 
        }

        protected Id _leafId = 0;
        [DataMember(Name = "LeafId")]
#if SILVERLIGHT
        public Id leafId {
            get { return _leafId; }
            set { setId(ref _leafId, ref _leaf, value); }
        }
#endif
        Id IEdge<Id>.Leaf {
            get { return _leafId; }
            set { setId(ref _leafId, ref _leaf, value); }
        }

        protected Id _markerId = 0;
        [DataMember(Name = "MarkerId")]
#if SILVERLIGHT        
        public Id narkerId {
            get { return _markerId; }
            set { setId(ref _markerId, ref _marker, value); }
        }
#endif
        Id ILink<Id>.Marker {
            get { return _markerId; }
            set { setId(ref _markerId, ref _marker, value); }
        }

        protected Id _rootId = 0;
        [DataMember(Name = "RootId")]
#if SILVERLIGHT        
        public Id rootId {
            get { return _rootId; }
            set { setId(ref _rootId, ref _root, value); }
        }
#endif

        Id IEdge<Id>.Root {
            get { return _rootId; }
            set { setId(ref _rootId, ref _root, value); }
        }
        #endregion

        public override string ToString() {
            return String.Format("[{0}->{1},{2}]", this.Root, this.Leaf, this.Marker);
        }

        

        
    }
}