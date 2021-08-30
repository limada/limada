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
using Limaki.View.Viz.Mapping;
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
using Limaki.View.Common;

namespace Limada.Usecases {

    public class GraphSceneDisplayXmlSerializer : DrawingXmlSerializer {
		
        public static class NodeNames {
            public const string Display = "display";

            public const string Source = "source";
            public const string File = "file";

            public const string Layout = "layout";
            public const string StyleSheet = "name";

			public const string Viewport = "viewport";
			public const string Offset = "offset";
			public const string ZoomState = "zoom_state";
			public const string Zoom = "zoom_factor";

            public const string Scene = "scene";
            public const string Focused = "focused";

            // TODO:
            // selected, focused
        }

        private IGraphSceneMapDisplayOrganizer<IVisual, IVisualEdge> _organizer = null;

		public IGraphSceneMapDisplayOrganizer<IVisual, IVisualEdge> Organizer {
			get { return _organizer ?? (_organizer = Registry.Pooled<IGraphSceneMapDisplayOrganizer<IVisual, IVisualEdge>> ()); }
			set { _organizer = value; }
		}

		public virtual XElement Write (GraphSceneDisplayMemento display) {

			var result = new XElement (NodeNames.Display);

			if (display.Iori != null) {
				result.Add (new XElement (NodeNames.Source, Write (NodeNames.File, display.Iori.ToString ())));
			} else {
				// TODO: write MemoryThingGraph
			}

			var graph = display.Scene.Graph.Source<IVisual, IVisualEdge, IThing, ILink> ();
			if (graph != null) {
				var serializer = new VisualThingXmlSerializer ();
				serializer.VisualThingGraph = graph;
				serializer.Layout = display.Layout;
				serializer.Write (display.Scene.Graph);
				result.Add (serializer.XThings);
			}

			var sceneInfoSerializer = new SceneInfoXmlSerializer ();
			result.Add (sceneInfoSerializer.Write (display.Info));

			result.Add (new XElement (NodeNames.Viewport,
				Write (NodeNames.Offset, display.Offset),
				Write (NodeNames.ZoomState, display.ZoomState),
				Write (NodeNames.Zoom, display.Zoom)));

			// TODO: serialize Layout
			result.Add (new XElement (NodeNames.Layout,
				Write (NodeNames.StyleSheet, display.Layout.StyleSheet.Name)));

		    var focused = display.Scene.Graph.ThingOf (display.Scene.Focused);
		    if (focused != null) {
		        result.Add(new XElement (NodeNames.Scene, Write (NodeNames.Focused, focused.Id.ToString("X"))));
		    }
			return result;
		}

        public virtual GraphSceneDisplayMemento ReadDisplay (XElement node) {
			
			var memento = new GraphSceneDisplayMemento ();

			var iori = ReadElement (node, NodeNames.Source);
			if (iori != null)
				memento.Iori = Iori.FromFileName (Read<string> (iori, NodeNames.File));

            memento.Info = new SceneInfoXmlSerializer ().ReadSceneInfo (node);;

			var vp = ReadElement (node, NodeNames.Viewport);
            memento.Offset = Read<Point> (vp, NodeNames.Offset);
            memento.ZoomState = Read<ZoomState> (vp, NodeNames.ZoomState);
            memento.Zoom = Read<double> (vp, NodeNames.Zoom);

			var thingGraph = Organizer.MappedGraph(memento.Iori);

			memento.Scene = Organizer.MapInteractor<IThing,ILink>().CreateScene (thingGraph);

			var layout = ReadElement (node, NodeNames.Layout);
			memento.StyleSheetName = Read<string> (layout, NodeNames.StyleSheet);
			var styleSheet = Registry.Pooled<StyleSheets> ()[memento.StyleSheetName];

			// NEVER use the tempLayout in memento-Layout; this brings troubles if data changes
			// creating layout is done in memento.Restore
			Func<IGraphScene<IVisual, IVisualEdge>> scene = () => memento.Scene;
			var tempLayout = Registry.Create<IGraphSceneLayout<IVisual, IVisualEdge>> (scene, styleSheet);

			var serializer = new VisualThingXmlSerializer ();
			serializer.VisualThingGraph = memento.Scene.Graph.Source<IVisual, IVisualEdge, IThing, ILink> ();
			serializer.Layout = tempLayout;

			serializer.XThings = ReadElement (node, VisualThingXmlSerializer.NodeNames.Things);

			serializer.ReadXThings ();

			new GraphSceneFacade<IVisual, IVisualEdge> (() => memento.Scene, tempLayout)
				.Add (serializer.VisualsCollection, true, false);

            var sceneDetails = ReadElement (node, NodeNames.Scene);
            var foc = Read<long> (sceneDetails, NodeNames.Focused);
            if (foc != 0) {
                var focThing = thingGraph.GetById (foc);
                if (focThing != null) {
                    memento.Scene.Focused = memento.Scene.Graph.VisualOf (focThing);
                }
            }
			memento.Scene.ClearSpatialIndex ();

			return memento;
		}
    }
	
}