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
using System.Reflection;

namespace Limada.Usecases {

	public class UsecaseXmlSerializer : XmlSerializerBase {

	    public static class NodeNames {
	        public const string Usecase = "usecase";

	        public const string Name = "name";
	        public const string Displays = "displays";

	        public const string SplitView = "splitview";
            public const string SplitViewDisplay1 = SplitView + "." + nameof (Limaki.Usecases.Vidgets.ISplitView.Display1);
            public const string SplitViewDisplay2 = SplitView + "." + nameof (Limaki.Usecases.Vidgets.ISplitView.Display2);

            public const string Favorites = "favorites";
	        public const string HomeId = "homeid";
            
	    }

        public Action<IGraphSceneDisplay<IVisual, IVisualEdge>, string> BeforeSave { get; set; }
        public Action<IGraphSceneDisplay<IVisual, IVisualEdge>, string> AfterRestore { get; set; }

        private IGraphSceneMapDisplayOrganizer<IVisual, IVisualEdge> _organizer = null;
        public IGraphSceneMapDisplayOrganizer<IVisual, IVisualEdge> Organizer {
			get { return _organizer ?? (_organizer = Registry.Pooled<IGraphSceneMapDisplayOrganizer<IVisual, IVisualEdge>> ()); }
			set { _organizer = value; }
		}

		public virtual XElement Write (IConceptUsecase usecase) {

			var result = new XElement (NodeNames.Usecase);

			result.Add (new GraphSceneDisplayOrganizerXmlSerializer ().Write (Organizer));

			var splitView = new XElement (NodeNames.SplitView);
			var displays = new XElement (NodeNames.Displays);

			Action<IGraphSceneDisplay<IVisual, IVisualEdge>, string> writeSplitDisplays = (d, n) => {
			    BeforeSave?.Invoke (d, n);
				var display = new XElement (GraphSceneDisplayXmlSerializer.NodeNames.Display);
				display.Add (Write (NodeNames.Name, n));
				display.Add (Write (SceneInfoXmlSerializer.NodeNames.Id, d.DataId.ToString ("X")));
				displays.Add (display);
			};

            writeSplitDisplays (usecase.SplitView.Display1, NodeNames.SplitViewDisplay1);
            writeSplitDisplays (usecase.SplitView.Display2, NodeNames.SplitViewDisplay2);

            splitView.Add (displays);
			result.Add (splitView);

			var favorites = new XElement (NodeNames.Favorites);
			favorites.Add (Write (NodeNames.HomeId, usecase.FavoriteManager.HomeId.ToString ("X")));
			result.Add (favorites);

			return result;
		}

		public IEnumerable<string> FileNames { get; protected set;}
	    public virtual void Read (XElement node, IConceptUsecase usecase) {

	        var ms = new GraphSceneDisplayOrganizerXmlSerializer ();
	        ms.Read (node, Organizer);

			FileNames = ms.FileNames;

            var backHandler = Organizer.MapInteractor<IThing, ILink> ();
            var filesOpen = backHandler
                .MappedGraphs
                .Select (g => new {
                    Iori = ThingMapHelper.GetIori (g),
                    Graph = g
                })
                .Where (i => i.Iori != null)
                .ToDictionary (k => k.Iori.ToString (), e => e.Graph);

            
            foreach (var file in ms.FileNames) {

                var io = Registry.Pooled<ThingGraphIoPool> ().Find (Path.GetExtension (file), IoMode.Read)
                 as ThingGraphIo;

                if (!filesOpen.ContainsKey (file)) {
                    try {
                        var content = io.Open (Iori.FromFileName (file));
                        var g = backHandler.WrapGraph (content.Data);
                        backHandler.RegisterMappedGraph (g);
                    } catch (Exception e) {

                    }
                }
            }
            
            var displays = ReadElement (ReadElement (node, NodeNames.SplitView), NodeNames.Displays);

	        foreach (var display in ReadElements (displays, GraphSceneDisplayXmlSerializer.NodeNames.Display)) {
	            var name = Read<string> (display, NodeNames.Name);
	            var id = Read<long> (display, SceneInfoXmlSerializer.NodeNames.Id);
	            var d = Organizer.Displays.Where (disp => disp.DataId == id).FirstOrDefault ();
	            if (d == null) {
	                var dm = ms.Displays.Where (disp => disp.Info.Id == id).FirstOrDefault ();
	                if (name == NodeNames.SplitViewDisplay1 && dm != null) {
                        dm.Restore (usecase.SplitView.Display1);
	                    AfterRestore?.Invoke (d, name);
	                }
	                if (name == NodeNames.SplitViewDisplay2 && dm != null) {
	                    dm.Restore (usecase.SplitView.Display2);
                        AfterRestore?.Invoke (d, name);
                    }
	            }
	        }

            var homeId = Read<long> (ReadElement (node, NodeNames.Favorites), NodeNames.HomeId);
	        usecase.FavoriteManager.HomeId = homeId;

        }
	}

}