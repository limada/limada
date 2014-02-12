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

using System;
using System.IO;
using Limada.Usecases;
using Limada.View;
using Limaki.Common;
using Limaki.Drawing;
using Limaki.Model.Content;
using Limaki.View.Visualizers;
using Limaki.Viewers;
using Limaki.Viewers.ToolStripViewers;
using Limaki.Visuals;
using Limada.VisualThings;

namespace Limaki.Usecases.Concept {

    public class ConceptUsecase:IDisposable, IProgress {

        protected string _useCaseTitle = "limada::concept";
        public string UseCaseTitle {
            get { return _useCaseTitle; }
            set { _useCaseTitle = value; }
        }

        public void Start () {

            GraphSceneUiManager.OpenFileDialog.InitialDirectory = Environment.SpecialFolder.MyDocuments.ToString();
            GraphSceneUiManager.SaveFileDialog.InitialDirectory = Environment.SpecialFolder.MyDocuments.ToString();

            if (!GraphSceneUiManager.ProcessCommandLine()) {
                GraphSceneUiManager.ShowEmptyScene();
            }
        }

        bool closeDone = false;
        public void Close() {
            if (!closeDone) {
                SaveChanges();
                GraphSceneUiManager.Close();
                closeDone = true;
            }
        }

        public SplitView0 SplitView { get; set; }
        public VisualsDisplayHistory VisualsDisplayHistory { get; set; }
        public ISheetManager SheetManager {get;set;}
        public FavoriteManager FavoriteManager { get; set; }

        public ArrangerToolStrip ArrangerToolStrip { get; set; }
        public DisplayModeToolStrip DisplayToolStrip { get; set; }
        public SplitViewToolStrip SplitViewToolStrip { get; set; }
        public LayoutToolStrip LayoutToolStrip { get; set; }
        public MarkerToolStrip MarkerToolStrip { get; set; }
        
        public Func<object> GetCurrentControl { get; set; }
        public Func<IGraphSceneDisplay<IVisual, IVisualEdge>> GetCurrentDisplay { get; set; }


        public Func<string, string, MessageBoxButtons, DialogResult> MessageBoxShow { get; set; }
        public Func<FileDialogMemento, bool, DialogResult> FileDialogShow { get; set; }

        public IGraphSceneUiManager GraphSceneUiManager { get; set; }
        public Action<string> DataPostProcess { get; set; }

        public void OpenFile() {
            SaveChanges();
            GraphSceneUiManager.Open ();
        }

        public virtual void SaveFile() {
            SaveChanges();
            GraphSceneUiManager.Save();
        }

        public void SaveAsFile() {
            SaveChanges();
            GraphSceneUiManager.SaveAs ();
        }

        public void ExportCurrentView() {
            var display = GetCurrentDisplay ();
            if (display != null) {
                GraphSceneUiManager.ExportSceneView (display.Data);
            }
        }

        
        public void ImportThingGraphRaw() {
            SaveChanges();
            GraphSceneUiManager.ShowEmptyScene();
            GraphSceneUiManager.ImportRawSource();
        }

        public void Search() {
            this.SplitView.DoSearch ();
        }

        public void Dispose() {
            this.SplitView.Dispose ();
            
        }

        public event EventHandler<EventArgs<IStyle>> DisplayStyleChanged = null;
        public void OnDisplayStyleChanged(object sender, EventArgs<IStyle> arg) {
            if (DisplayStyleChanged != null) {
                DisplayStyleChanged(sender, arg);
            }
        }

		public StreamContentUiManager StreamContentUiManager { get; set; }

        public void ExportThings () {
            var display = GetCurrentDisplay();
            if (display != null) {
                StreamContentUiManager.WriteThings(display.Data);
            }
        }

        public void ImportContent () {
            StreamContentUiManager.ContentIn = content => {
                var display = GetCurrentDisplay();
                if (display != null) {
                    display.Data.AddContent(content, display.Layout);
                    display.Perform();
                }
            };
            StreamContentUiManager.Read();
        }

        public void ImportGraphFocus () {
            var display = GetCurrentDisplay();

            if (display != null) {
                var scene = display.Data;
                StreamContentUiManager.ThingGraphCursorIoManager.SinkIn = graphFocus => {
                    scene.AddVisual(scene.Graph.VisualOf(graphFocus.Cursor), display.Layout);
                    display.Perform();
                };
                StreamContentUiManager.ReadThingGraphFocus(display.Data);
            }
        }

        public void ExportContent () {
            StreamContentUiManager.ContentOut = () => {
                var display = GetCurrentDisplay();
                if (display != null) {
                    return display.Data.ContentOfFocused();
                }
                return null;
            };
            StreamContentUiManager.Save();
        }

        public void SaveChanges() {
            var displays = new IGraphSceneDisplay<IVisual, IVisualEdge>[] { SplitView.Display1, SplitView.Display2 };
            VisualsDisplayHistory.SaveChanges(displays, SheetManager, MessageBoxShow);
            FavoriteManager.SaveChanges(displays);
        }

        public Action<string,int,int> Progress {get; set;}
        public Action ApplicationQuit { get; set; }
        public bool ApplicationQuitted { get; set; }



        
    }
}