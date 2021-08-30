/*
 * Limaki 
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

using Limaki.Common;
using Limaki.Graphs;
using Limaki.View.Vidgets;
using Xwt;
using System;
using Limaki.Actions;

namespace Limaki.View.Viz.UI.GraphScene {

	public class GraphItemMouseScrollAction<TItem, TEdge> : MouseScrollAction
		where TEdge : IEdge<TItem>, TItem {

		public GraphItemMouseScrollAction():base() {
			Priority = ActionPriorities.SelectionPriority - 500;
            MoveIfItemHit = false;
		}

		public Func<IGraphScene<TItem, TEdge>> SceneHandler { get; set; }

		public IGraphScene<TItem, TEdge> Scene {
			get { return SceneHandler(); }
		}

		public int HitSize { get; set; }

		protected virtual TItem Hit(MouseActionEventArgs e){
			if (Scene.Focused != null)
				return Scene.Focused;
			var zoomTarget = this.Viewport ();
			var p = zoomTarget.Camera.ToSource(e.Location);
			var item = Scene.Hit(p, HitSize);

			return item;
		}

        public bool MoveIfItemHit { get; set; }

		public override void OnMouseDown (MouseActionEventArgs e)
		{
			var item = Hit(e);
			if (MoveIfItemHit || item == null) {
				base.OnMouseDown (e);
			}
		}

		public override bool Check() {
			if (this.SceneHandler == null) {
				throw new CheckFailedException(this.GetType(), typeof(IGraphScene<TItem, TEdge>));
			}
			return base.Check();
		}
	}
}