/*
 * Limaki 
 * 
 * This code is free software; you can redistribute it and/or modify it
 * under the terms of the GNU General Public License version 2 only, as
 * published by the Free Software Foundation.
 * 
 * Author: Lytico
 * Copyright (C) 2008-2013 Lytico
 *
 * http://www.limada.org
 * 
 */

using System;
using System.Collections.Generic;
using Limaki.Common.Collections;
using Limaki.Iconerias;
using Limaki.Usecases.Vidgets;
using Limaki.View;
using Limaki.View.Vidgets;
using Limaki.View.Visuals;
using Limaki.View.Viz;
using Limaki.View.Viz.Visualizers;
using Limaki.View.Viz.Visualizers.ToolStrips;
using Limaki.View.Properties;
using Xwt.Backends;

namespace Limada.View.Vidgets {

    [BackendType(typeof(ISplitViewToolStripBackend))]
    public class SplitViewToolStrip0 : DisplayToolStrip0<IGraphSceneDisplay<IVisual, IVisualEdge>, ISplitViewToolStripBackend> {

        public ToolStripCommand0 GraphStreamViewCommand { get; set; }
        public ToolStripCommand0 GraphGraphViewCommand { get; set; }
        public ToolStripCommand0 ToggleViewCommand { get; set; }

        public ToolStripCommand0 OpenNewWindowCommand { get; set; }

        public ToolStripCommand0 GoBackCommand { get; set; }
        public ToolStripCommand0 GoForwardCommand { get; set; }
        public ToolStripCommand0 GoHomeCommand { get; set; }

        public ToolStripCommand0 NewSheetCommand { get; set; }
        public ToolStripCommand0 NewNoteCommand { get; set; }
        public ToolStripCommand0 SaveSheetCommand { get; set; }

        public SplitViewToolStrip0 () {
            Compose();
        }

        protected virtual void Compose () {
            var size = new Xwt.Size(36, 36);

            GraphStreamViewCommand = new ToolStripCommand0 {
                Action = s => Backend.ViewMode = SplitView.ViewMode = SplitViewMode.GraphStream,
                Image = Iconery.GraphContentView,
                Size = size,
                ToolTipText = "show contents"
            };
            GraphGraphViewCommand = new ToolStripCommand0 {
                Action = s => Backend.ViewMode = SplitView.ViewMode = SplitViewMode.GraphGraph,
                Image = Iconery.GraphGraphView,
                Size = size,
                ToolTipText = "show tiled graph"
            };

            ToggleViewCommand = new ToolStripCommand0 {
                Action = s => SplitView.ToggleView(),
                Image = Iconery.ToggleView,
                Size = size,
                ToolTipText = "toogle view"
            };

            OpenNewWindowCommand = new ToolStripCommand0 {
                Action = s => SplitView.ShowInNewWindow (),
                Image = Iconery.NewViewVisualNote,
                Size = size,
                ToolTipText = "open new window"
            };

            Action<bool> goBackOrForward = backOrForward => {
                            if (SplitView.CanGoBackOrForward(backOrForward)) {
                                SplitView.GoBackOrForward(backOrForward);
                                Backend.CheckBackForward(SplitView);
                            }
                        };
            GoBackCommand = new ToolStripCommand0 {
                Action = s => goBackOrForward(false),
                Image = Iconery.GoPrevious,
                Size = size,
                ToolTipText = "navigate back"
            };
            GoForwardCommand = new ToolStripCommand0 {
                Action = s => goBackOrForward(true),
                Image = Iconery.GoNext,
                Size = size,
                ToolTipText = "navigate forward"
            };
            GoHomeCommand = new ToolStripCommand0 {
                Action = s => SplitView.GoHome(),
                Image = Iconery.GoHome,
                Size = size,
                ToolTipText = "go to favorites"
            };

            NewSheetCommand = new ToolStripCommand0 {
                Action = s => SplitView.NewSheet(),
                Image = Iconery.NewSheet,
                Size = size,
                ToolTipText = "new sheet"
            };

            NewNoteCommand = new ToolStripCommand0 {
                Action = s => SplitView.NewNote(),
                Image = Iconery.NewNote,
                Size = size,
                ToolTipText = "new note"
            };

            SaveSheetCommand = new ToolStripCommand0 {
                Action = s => SplitView.SaveDocument(),
                Image = Iconery.SaveContent,
                Size = size,
                ToolTipText = "save content"
            };
        }

        ISplitView _splitView = null;
        public ISplitView SplitView {
            get {
                if (_splitView == null) {
                    _splitView = new SplitViewDumnmy();
                }
                return _splitView;
            }
            set {
                if (_splitView != value) {
                    if (_splitView != null) {
                        _splitView.ViewChanged -= this.ViewChanged;
                    }
                    if (value != null) {
                        value.ViewChanged += this.ViewChanged;
                    }
                }
                _splitView = value;
            }
        }
        
        protected virtual void ViewChanged (object sender, EventArgs e) { Attach(sender); }

        public ISheetManager SheetManager { get; set; }

        public override void Detach (object sender) { }

        public override void Attach (object sender) {
            var display = sender as IGraphSceneDisplay<IVisual, IVisualEdge>;
            if (display != null)
                CurrentDisplay = display;

            if (SplitView != null) {
                Backend.CheckBackForward(SplitView);
                Backend.ViewMode = SplitView.ViewMode;
            }
            Backend.AttachSheets();
        }

        public virtual string AttachSheets (IList<SceneInfo> _sheets) {
            string result = null;

            var display = CurrentDisplay;
            var dontCare = new Set<long>();

            if (display != null) {
                result = display.Info.Name;
                dontCare.Add(display.DataId);
                var adj = SplitView.AdjacentDisplay(display);
                if (adj != null)
                    dontCare.Add(adj.DataId);
            }
            _sheets.Clear();

            SheetManager.VisitRegisteredSheets(s => {
                if (SheetManager.StoreContains(s.Id) && !dontCare.Contains(s.Id))
                    _sheets.Add(s);
            });

            return result;

        }

        public void SelectSheet (SceneInfo info) { SplitView.LoadSheet(info); }

        
    }

    public interface ISplitViewToolStripBackend : IDisplayToolStripBackend {
        SplitViewMode ViewMode { get; set; }
        void CheckBackForward (ISplitView splitView);
        void AttachSheets ();
    }
}