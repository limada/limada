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


using Limaki.Common;
using Limaki.Drawing;
using Limaki.Graphs;

namespace Limaki.Presenter {
    // NOT USED!
    // Presentation Manager ??
    // Bridge?? 
    public class ReceiverInstrumentation<TItem, TEdge> 
    where TEdge:TItem,IEdge<TItem> {

        public virtual IModelReceiver<TItem> ModelReceiver { get; set; }

        ISceneReceiver<TItem, TEdge> receiver = null;
        public virtual ISceneReceiver<TItem, TEdge> Receiver {
            get {
                if (receiver != null) {
                    receiver.GraphScene = this.GraphScene;
                    receiver.Layout = this.Layout;
                    receiver.Camera = this.Camera;

                    receiver.ModelReceiver = () => this.ModelReceiver;
                    receiver.Clipper = this.Clipper;
                }
                return receiver;
            }
            set { receiver = value; }
        }

        public virtual Get<IClipper> Clipper { get; set; }
        public virtual Get<ICamera> Camera { get; set; }
        public virtual Get<IGraphScene<TItem, TEdge>> GraphScene { get; set; }
        public virtual Get<IGraphLayout<TItem,TEdge>> Layout { get; set; }
    }
}