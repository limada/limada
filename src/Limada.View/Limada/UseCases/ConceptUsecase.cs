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

using Limada.IO;
using Limada.Usecases.Contents;
using Limada.View.VisualThings;
using Limaki.Common;
using Limaki.Drawing;
using Limaki.Graphs;
using Limaki.Usecases;
using Limaki.View;
using Limaki.View.GraphScene;
using Limaki.View.Vidgets;
using Limaki.View.Visuals;
using Limaki.View.Viz;
using Limaki.View.Viz.Modelling;
using System;
using System.Linq;
using Xwt;
using Limaki.Usecases.Vidgets;
using Limaki.View.Common;
using Limaki.View.Viz.Mapping;
using Limaki.View.Viz.Visualizers.Toolbars;
using Limaki.View.Viz.Visualizers;
using Xwt.Drawing;

namespace Limada.Usecases {
	
    public class ConceptUsecase : IDisposable, IProgress, IConceptUsecase {

        public string UseCaseTitle { get; set; } = "limada::concept";

        public virtual IVindow MainWindow { get; set; }

        public Menu Menu { get; set; }

        public ISplitView SplitView { get; set; }

        public VisualsDisplayHistory VisualsDisplayHistory { get; set; }
        public IVisualSceneStoreInteractor SceneManager { get; set; }
        public IFavoriteManager FavoriteManager { get; set; }

        public ToolbarPanel Toolbar { get; set; }
        public ArrangerToolbar ArrangerToolbar { get; set; }
        public DisplayModeToolbar DisplayModeToolbar { get; set; }
        public SplitViewToolbar SplitViewToolbar { get; set; }

        public LayoutToolbar LayoutToolbar { get; set; }
        public MarkerToolbar MarkerToolbar { get; set; }

        public IGraphSceneMapDisplayOrganizer<IVisual, IVisualEdge> Organizer { get { return Registry.Pooled<IGraphSceneMapDisplayOrganizer<IVisual, IVisualEdge>>(); }}

		public Func<IVidget> GetCurrentVidget { get; set; }
        
        public Func<IGraphSceneDisplay<IVisual, IVisualEdge>> GetCurrentDisplay { get; set; }

        public Action<object, EventArgs<IStyle>> DisplayStyleChanged { get; set; }

        public Func<string, string, MessageBoxButtons, DialogResult> MessageBoxShow { get; set; }

        public Action<string, int, int> Progress { get; set; }

        public Action ApplicationQuit { get; set; }
        public bool ApplicationQuitted { get; set; }

        About _about = null;
        public virtual About About { get { return _about ?? (_about = Registry.Pooled<About> ()); } }

#region Open/Save

        public IGraphSceneUiManager GraphSceneUiManager { get; set; }

        public Func<FileDialogMemento, bool, DialogResult> FileDialogShow { get; set; }
        public Action<string> DataPostProcess { get; set; }

        public virtual void Start () {

            GraphSceneUiManager.OpenFileDialog.InitialDirectory = Environment.SpecialFolder.MyDocuments.ToString ();
            GraphSceneUiManager.SaveFileDialog.InitialDirectory = Environment.SpecialFolder.MyDocuments.ToString ();

            if (!GraphSceneUiManager.OpenCommandLine ()) {
                GraphSceneUiManager.ShowEmptyScene ();
            }
        }

        protected bool closeDone = false;
        public virtual void Close () {
            if (!closeDone) {
                SaveChanges ();
                GraphSceneUiManager.Close ();
                closeDone = true;
            }
        }

        public virtual void OpenFile () {
            SaveChanges ();
            GraphSceneUiManager.Open ();
        }

        public virtual void SaveFile () {
            SaveChanges ();
            GraphSceneUiManager.Save ();
        }

        public virtual void SaveAsFile () {
            SaveChanges ();
            GraphSceneUiManager.SaveAs ();
        }

        public virtual void ExportCurrentView () {
            var display = GetCurrentDisplay ();
            if (display != null) {
                GraphSceneUiManager.ExportSceneView (display.Data);
            }
        }
        
        public virtual void ImportThingGraphRaw () {
            SaveChanges ();
            GraphSceneUiManager.ShowEmptyScene ();
            GraphSceneUiManager.ImportRawSource ();
        }

        public bool AskForVisualsDisplayHistorySaveChanges { get; set; }

        public virtual void SaveChanges () {
			var displays = Organizer.Displays;
            VisualsDisplayHistory.SaveChanges (displays, SceneManager, AskForVisualsDisplayHistorySaveChanges);
            FavoriteManager.SaveChanges (displays);
        }

#endregion

#region Content

        public StreamContentUiManager StreamContentUiManager { get; set; }

        public virtual void ExportThings () {
            var display = GetCurrentDisplay ();
            if (display != null) {
                StreamContentUiManager.WriteThings (display.Data);
            }
        }

        public virtual void ImportContent () {
            StreamContentUiManager.ContentIn = content => {
                var display = GetCurrentDisplay ();
                if (display != null) {
                    display.Data.AddContent (content, display.Layout);
                    display.Perform ();
                }
            };
            StreamContentUiManager.Read ();
        }

        public virtual void ImportGraphCursor () {
            var display = GetCurrentDisplay ();

            if (display != null) {
                var scene = display.Data;
                StreamContentUiManager.ThingGraphCursorIoManager.SinkIn = graphCursor => {
                    scene.AddVisual (scene.Graph.VisualOf (graphCursor.Cursor), display.Layout);
                    display.Perform ();
                };
                StreamContentUiManager.ReadThingGraphCursor (display.Data);
                display.Perform ();
            }
        }

        public virtual void ExportContent () {
            StreamContentUiManager.ContentOut = () => {
                var display = GetCurrentDisplay ();
                if (display != null) {
                    return display.Data.ContentOfFocused ();
                }
                return null;
            };
            StreamContentUiManager.Save ();
        }

#endregion

        public void Search () {
            this.SplitView.DoSearch ();
        }

        public virtual void MergeVisual () {

            try {
                var display = GetCurrentDisplay ();
                if (display != null) {
                    if (MessageBoxShow ("Are you shure?", "Merge", MessageBoxButtons.OkCancel) == DialogResult.Ok) {
                        Registry.Create<ISceneInteractor<IVisual, IVisualEdge>> ().MergeVisual (display.Data);
                        display.Perform ();
                    }
                }
            } catch (Exception ex) {
                Registry.Pooled<IExceptionHandler> ().Catch (ex, MessageType.OK);
            }
        }

        public virtual void RevertEdges () {

            try {
                var display = GetCurrentDisplay ();
                if (display != null) {
                    
                    Registry.Create<ISceneInteractor<IVisual, IVisualEdge>> ().RevertEdges (display.Data);
                    display.Perform ();

                }
            } catch (Exception ex) {
                Registry.Pooled<IExceptionHandler> ().Catch (ex, MessageType.OK);
            }
        }

        public virtual void RefreshCompression () {
            try {
                var rfcThingGraph = GetCurrentDisplay ().Data.Graph.ThingGraph ();
                if (rfcThingGraph == null)
                    throw new ArgumentException ("ThingGraphMaintenance only works with Thing-backed graphs");

                var graph = rfcThingGraph.Unwrap ();
                var maint = new ThingGraphMaintenance ();

                maint.RefreshCompression (graph, true);
            } catch (Exception e) {
                Registry.Pooled<IExceptionHandler> ().Catch (e, MessageType.OK);
            }
        }

        public virtual void TimelineSheet () {
            try {
                var thingGraph = GetCurrentDisplay ().Data.Graph.ThingGraph ();
                if (thingGraph == null)
                    throw new ArgumentException ("TimelineSheet only works with Thing-backed graphs");

                var view = SplitView;
                var display = view.AdjacentDisplay (view.CurrentDisplay);
                var oldScene = display.Data;

                Organizer.RemoveScene (oldScene);

                var scene = Organizer.CreateSinkScene (oldScene.Graph);
                display.Data = scene;

                var visuals = new ThingGraphTimelineInteractor ()
                    .TimeLine (thingGraph)
                    .Select (t => scene.Graph.VisualOf (t)).ToArray ();

                new GraphSceneFacade<IVisual, IVisualEdge> (() => scene, display.Layout)
                    .Add (visuals, true, false);

                Organizer.AddScene (scene);

                var aligner = new Aligner<IVisual, IVisualEdge> (scene, display.Layout);
                aligner.OneColumn (visuals, (Point) display.Layout.Border, display.Layout.Options ());
                aligner.Locator.Commit (scene.Requests);

                display.Perform ();
            } catch (Exception e) {
                Registry.Pooled<IExceptionHandler> ().Catch (e, MessageType.OK);
            }
        }

        public Action ShowAboutWindow { get;set;}
        public Action ShowLayoutEditor { get; set; }
        public Action ShowStyleEditor { get; set; }
        public Action ExportAsImage { get; set; }
               
        public void ExportAsImageImpl (ConceptUsecase useCase) {
            var currentDisplay = useCase.GetCurrentDisplay ();
            if (currentDisplay != null && currentDisplay.Data != null) {
                var saveFileDialog = new FileDialogMemento {
                    DefaultExt = "png",
                    Filter = "PNG-Image|*.png|All Files|*.*",
                };

                if (useCase.FileDialogShow (saveFileDialog, false) == DialogResult.Ok) {
                    var image =
                        new ImageExporter (currentDisplay.Data, currentDisplay.Layout) { StyleSheet = currentDisplay.StyleSheet }
                            .ExportImage ();
                    if (image != null) {
                        image.Save (saveFileDialog.FileName, ImageFileType.Png);
                        image.Dispose ();
                    }
                }
            }
        }

        public virtual void Dispose () {
            this.SplitView.Dispose ();
        }

    }
}