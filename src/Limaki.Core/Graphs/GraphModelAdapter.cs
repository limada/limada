/*
 * Limaki 
 * Version 0.081
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


namespace Limaki.Graphs {
    public abstract class GraphModelAdapter<TItemOne, TItemTwo, TEdgeOne, TEdgeTwo>
        where TEdgeOne : IEdge<TItemOne>, TItemOne
        where TEdgeTwo : IEdge<TItemTwo>, TItemTwo {
        public abstract TItemOne CreateItemOne(IGraph<TItemTwo, TEdgeTwo> sender,
            IGraph<TItemOne, TEdgeOne> target, TItemTwo item);
        public abstract TEdgeOne CreateEdgeOne(IGraph<TItemTwo, TEdgeTwo> sender,
            IGraph<TItemOne, TEdgeOne> target, TEdgeTwo item);

        public abstract TItemTwo CreateItemTwo(IGraph<TItemOne, TEdgeOne> sender, 
            IGraph<TItemTwo, TEdgeTwo> target, TItemOne item);
        public abstract TEdgeTwo CreateEdgeTwo(IGraph<TItemOne, TEdgeOne> sender, 
            IGraph<TItemTwo, TEdgeTwo> target, TEdgeOne item);

        public abstract void ChangeData ( IGraph<TItemOne, TEdgeOne> sender, TItemOne item, object data );
        public abstract void ChangeData ( IGraph<TItemTwo, TEdgeTwo> sender, TItemTwo item, object data);
        public virtual void EdgeCreated ( TEdgeOne one, TEdgeTwo two) {}
        public virtual void EdgeCreated ( TEdgeTwo two, TEdgeOne one) {}

        public virtual GraphModelAdapter<TItemTwo, TItemOne, TEdgeTwo, TEdgeOne> ReverseAdapter() {
            return
                new ReverseGraphModelAdapter<TItemTwo, TItemOne, TEdgeTwo, TEdgeOne>(this);
        }
    }

    public class ReverseGraphModelAdapter<TItemOne, TItemTwo, TEdgeOne, TEdgeTwo> : 
        GraphModelAdapter<TItemOne, TItemTwo, TEdgeOne, TEdgeTwo>
        where TEdgeOne : IEdge<TItemOne>, TItemOne
        where TEdgeTwo : IEdge<TItemTwo>, TItemTwo {

        GraphModelAdapter<TItemTwo, TItemOne, TEdgeTwo, TEdgeOne> source = null;

        public ReverseGraphModelAdapter(GraphModelAdapter<TItemTwo, TItemOne, TEdgeTwo, TEdgeOne> source) {
            this.source = source;
        }

        public override TItemOne CreateItemOne(IGraph<TItemTwo, TEdgeTwo> sender,
            IGraph<TItemOne, TEdgeOne> target, TItemTwo item) {
            return source.CreateItemTwo(sender,target,item);
        }

        public override TEdgeOne CreateEdgeOne(IGraph<TItemTwo, TEdgeTwo> sender,
            IGraph<TItemOne, TEdgeOne> target, TEdgeTwo item) {
            return source.CreateEdgeTwo(sender,target,item);
        }

        public override TItemTwo CreateItemTwo(IGraph<TItemOne, TEdgeOne> sender,
            IGraph<TItemTwo, TEdgeTwo> target, TItemOne item) {
            return source.CreateItemOne(sender,target,item);
        }

        public override TEdgeTwo CreateEdgeTwo(IGraph<TItemOne, TEdgeOne> sender,
            IGraph<TItemTwo, TEdgeTwo> target, TEdgeOne item) {
            return source.CreateEdgeOne(sender,target,item);
        }
        public override void EdgeCreated(TEdgeOne one, TEdgeTwo two) {
            source.EdgeCreated (one, two);
        }
        public override void EdgeCreated(TEdgeTwo two, TEdgeOne one) {
            source.EdgeCreated (two, one);
        }
        public override void ChangeData(IGraph<TItemOne, TEdgeOne> sender, TItemOne item, object data) {
            source.ChangeData (sender, item, data);
        }
        public override void ChangeData(IGraph<TItemTwo, TEdgeTwo> sender, TItemTwo item, object data) {
            source.ChangeData(sender, item, data);
        }

        public override GraphModelAdapter<TItemTwo, TItemOne, TEdgeTwo, TEdgeOne> ReverseAdapter() {
            return source;
        }
    }
}