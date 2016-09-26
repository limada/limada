using System;
using System.Collections.Generic;
using System.Linq;
using Limaki.Common;
using Limaki.Common.Linqish;
using Limaki.Drawing;
using Limaki.View;
using Limaki.View.ContentViewers;
using Limaki.View.Visuals;
using Limaki.View.Viz;
using Limaki.View.Viz.Mesh;
using Xwt;
using System.Xml.Linq;
using Limada.Model;
using Limaki.Graphs;
using Limada.View.VisualThings;
using Limaki.Data;
using Limada.IO;
using Limaki.View.GraphScene;
using System.IO;
using Limaki.Contents;
using Limaki.Drawing.Styles;
using System.Diagnostics;

namespace Limada.UseCases {

	public class GraphSceneDisplayMemento {

		public IGraphScene<IVisual, IVisualEdge> Scene { get; set; }

		public IGraphSceneLayout<IVisual, IVisualEdge> Layout { get; set; }

		SceneInfo _info = null;
		public SceneInfo Info { get { return _info ?? (_info = new SceneInfo ());} set { _info = value; } }

		public Point Offset { get; set; }

		public ZoomState ZoomState { get; set; }

		public double Zoom { get; set; }

		public Iori Iori { get; set; }

		public string StyleSheetName { get; set; }

		public GraphSceneDisplayMemento (IGraphSceneDisplay<IVisual, IVisualEdge> display) {
			Save (display);
		}

		public GraphSceneDisplayMemento () {
		}

		public void Save (IGraphSceneDisplay<IVisual, IVisualEdge> display) {
			
			Scene = display.Data;
			Layout = display.Layout;
			Info = SceneInfo.FromInfo (display.Info);

			Offset = display.Viewport.ClipOrigin;
			Zoom = display.Viewport.ZoomFactor;
			ZoomState = display.Viewport.ZoomState;

		}

		public void Restore (IGraphSceneDisplay<IVisual, IVisualEdge> display) {
			
            display.Data = Scene;

			display.Info = SceneInfo.FromInfo (Info);

			var layout = Layout;
			if (layout == null) {
				var styleSheet = Registry.Pooled<StyleSheets>()[StyleSheetName];
				Func<IGraphScene<IVisual, IVisualEdge>> scene = () => display.Data;
				layout = Registry.Create<IGraphSceneLayout<IVisual, IVisualEdge>>(scene, styleSheet);
			}
			display.Layout = layout;
			display.Viewport.ClipOrigin = Offset;
			display.Viewport.ZoomState = ZoomState;
			if (ZoomState == ZoomState.Custom)
				display.Viewport.ZoomFactor = Zoom;
		}
	}
	
}