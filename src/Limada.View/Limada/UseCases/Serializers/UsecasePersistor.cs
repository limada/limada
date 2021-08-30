using System;
using System.Collections.Generic;
using System.Linq;
using Limaki.Common;
using Limaki.Common.Linqish;
using Limaki.View;
using Limaki.View.ContentViewers;
using Limaki.View.Visuals;
using Limaki.View.Viz;
using Limaki.View.Viz.Mapping;
using System.Xml.Linq;
using Limada.Model;
using Limaki.Graphs;
using Limada.View.VisualThings;
using Limaki.Data;
using Limada.IO;
using Limaki.View.GraphScene;
using System.IO;
using Limaki.Contents;
using System.Diagnostics;

namespace Limada.Usecases {

    public class UsecasePersistor {

        public XElement XmlUsecase { get; set; }

        private IGraphSceneMapDisplayOrganizer<IVisual, IVisualEdge> _organizer = null;

        public IGraphSceneMapDisplayOrganizer<IVisual, IVisualEdge> Organizer {
            get { return _organizer ?? (_organizer = Registry.Pooled<IGraphSceneMapDisplayOrganizer<IVisual, IVisualEdge>> ()); }
        }

        public Action<IGraphSceneDisplay<IVisual, IVisualEdge>, string> BeforeSave { get; set; }
        public Action<IGraphSceneDisplay<IVisual, IVisualEdge>, string> AfterRestore { get; set; }
        
        public virtual void Save (IConceptUsecase usecase) {

            if (usecase.SplitView == null || usecase.GraphSceneUiManager == null)
                return;

            var displays = Organizer.Displays;
            usecase.VisualsDisplayHistory.SaveChanges (displays, usecase.SceneManager, false);
            usecase.FavoriteManager.SaveChanges (displays);
            usecase.GraphSceneUiManager.Save ();

            usecase.SplitView.ContentViewManager.SaveContentOfViewers ();

            XmlUsecase = new UsecaseXmlSerializer ()
                      .Write (usecase);

            StoreContentViewerTypes ();

        }

        public IList<Type> ContentViewerTypes = new List<Type> ();

        public virtual void StoreContentViewerTypes () {
            ContentViewerTypes.Clear ();
            var provider = Registry.Pooled<ContentViewerProvider> ();
            provider.Viewers.ForEach (viewer => ContentViewerTypes.Add (viewer.GetType ()));
            //provider.Clear ();
        }

        public virtual void ComposeContentViewers () {
            var provider = Registry.Pooled<ContentViewerProvider> ();
            var types = provider.Viewers.Select (p => p.GetType ()).ToArray ();
            ContentViewerTypes
                .Where (type=>!types.Contains (type))
                .ForEach (type => provider.Add (Activator.CreateInstance (type) as ContentViewer));
        }

        public virtual void Resume (IConceptUsecase usecase) {

            if (XmlUsecase == null || usecase.FavoriteManager == null)
                return;
            
            ComposeContentViewers ();

            Organizer.ClearDisplays ();

			var backHandler = Organizer.MapInteractor<IThing, ILink>();
			var backGraphs = backHandler.MappedGraphs.ToArray();

            var ser = new UsecaseXmlSerializer ();
            ser.Read (XmlUsecase, usecase);

			var graphChanged = backGraphs.Distinct().Any();
            
			// close others
            foreach (var g in backHandler.MappedGraphs
                //.Where(g=>backGraphs.Contains(g)
                .Select (g => new {iori = ThingMapHelper.GetIori (g), graph = g})
                .Where (g => g.iori == null || !ser.FileNames.Contains (g.iori.ToString ()))
                .Select (j => j.graph)
                .ToArray ()) {

                backHandler.UnregisterMappedGraph (g);

                usecase.GraphSceneUiManager.SetContent (ThingMapHelper.GetContent (g));
                usecase.GraphSceneUiManager.Close ();
            }

            if (graphChanged) {

				usecase.FavoriteManager.Clear();
                usecase.SceneManager.Clear();
				usecase.VisualsDisplayHistory.Clear();

                foreach (var d in Organizer.Displays) {
                    if (d.Info.Id != usecase.FavoriteManager.HomeId)
                        usecase.VisualsDisplayHistory.Store (d, usecase.SceneManager);
                }
			}
			var thingGraph = backHandler.MappedGraphs.FirstOrDefault() as IThingGraph;
            if (thingGraph != null) {
				usecase.GraphSceneUiManager.SetContent (ThingMapHelper.GetContent(thingGraph));
            }

            var focused = usecase.SplitView.Display1.Data.Focused;
            usecase.SplitView.Display1.Data.FocusChanged?.Invoke (usecase.SplitView.Display1.Data,focused);
            usecase.SplitView.Display1.OnSceneFocusChanged ();

        }

    }

    public class InMemUsecasePersistor {

		private Dictionary<string, GraphSceneDisplayMemento> SavedDisps = new Dictionary<string, GraphSceneDisplayMemento> ();

		private IGraphSceneMapDisplayOrganizer<IVisual, IVisualEdge> _organizer = null;

		public IGraphSceneMapDisplayOrganizer<IVisual, IVisualEdge> Organizer {
			get { return _organizer ?? (_organizer = Registry.Pooled<IGraphSceneMapDisplayOrganizer<IVisual, IVisualEdge>> ()); }
		}

		private Int64 HomeId = 0;

		public virtual void Save (string tag, IGraphSceneDisplay<IVisual, IVisualEdge> display) {
			SavedDisps[tag] = new GraphSceneDisplayMemento (display);
			display.Data = null;
			Organizer.RemoveDisplay (display);
		}

		public virtual void Restore (string tag, IGraphSceneDisplay<IVisual, IVisualEdge> display) {
			SavedDisps[tag].Restore (display);
			Organizer.AddDisplay (display);
		}

		public void Save (IConceptUsecase usecase) {

			if (usecase.SplitView == null || usecase.GraphSceneUiManager == null)
				return;

			HomeId = usecase.FavoriteManager.HomeId;
			var displays = Organizer.Displays;
            usecase.VisualsDisplayHistory.SaveChanges (displays, usecase.SceneManager, false);
			usecase.FavoriteManager.SaveChanges (displays);
			usecase.GraphSceneUiManager.Save ();

			Save ("SplitView.Display1", usecase.SplitView.Display1);
			Save ("SplitView.Display2", usecase.SplitView.Display2);

			ClearContentViewers ();

		}

		IList<Type> contentViewers = new List<Type> ();

		public void ClearContentViewers () {
			contentViewers.Clear ();
			var provider = Registry.Pooled<ContentViewerProvider> ();
			provider.Viewers.ForEach (viewer => contentViewers.Add (viewer.GetType ()));
			provider.Clear ();
		}
        
        public void ComposeContentViewers () {
            var provider = Registry.Pooled<ContentViewerProvider> ();
            contentViewers.ForEach (type => provider.Add (Activator.CreateInstance (type) as ContentViewer));
        }

        public void Resume (IConceptUsecase usecase) {

			if (SavedDisps == null || usecase.FavoriteManager == null)
				return;

			// there could be a resume without compose (only called in start)
			if (Registry.Pooled<ContentViewerProvider> ().Viewers.Count () == 0)
				ComposeContentViewers ();

			if (SavedDisps.Count > 0) {
				Restore ("SplitView.Display1", usecase.SplitView.Display1);
				Restore ("SplitView.Display2", usecase.SplitView.Display2);
				if (usecase.SplitView.CurrentDisplay != null && usecase.SplitView.CurrentDisplay.Data.Focused != null) {
					usecase.SplitView.CurrentDisplay.OnSceneFocusChanged ();
				}
			}
			usecase.FavoriteManager.HomeId = HomeId;

        }

	}


}