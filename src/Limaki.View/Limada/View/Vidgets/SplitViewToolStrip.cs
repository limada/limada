/*
 * Limaki 
 * 
 * This code is free software; you can redistribute it and/or modify it
 * under the terms of the GNU General Public License version 2 only, as
 * published by the Free Software Foundation.
 * 
 * Author: Lytico
 * Copyright (C) 2008-2014 Lytico
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
using Limaki.View.Viz.Visualizers.ToolStrips;
using Xwt.Backends;
using System.ComponentModel;
using System.Linq;

namespace Limada.View.Vidgets {

    [BackendType (typeof (ISplitViewToolStripBackend))]
    public class SplitViewToolStrip : DisplayToolStrip<IGraphSceneDisplay<IVisual, IVisualEdge>> {

        public IToolStripCommand GraphStreamViewCommand { get; set; }
        public IToolStripCommand GraphGraphViewCommand { get; set; }
        public IToolStripCommand ToggleViewCommand { get; set; }

        public IToolStripCommand OpenNewWindowCommand { get; set; }

        public IToolStripCommand GoBackCommand { get; set; }
        public IToolStripCommand GoForwardCommand { get; set; }
        public IToolStripCommand GoHomeCommand { get; set; }

        public IToolStripCommand NewSheetCommand { get; set; }
        public IToolStripCommand NewNoteCommand { get; set; }
        public IToolStripCommand SaveSheetCommand { get; set; }

        protected ToolStripButton GraphStreamViewButton { get; set; }
        protected ToolStripButton GraphGraphViewButton { get; set; }
        
        protected ToolStripButton GoBackButton { get; set; }
        protected ToolStripButton GoForwardButton { get; set; }
        
        protected ComboBox SheetCombo;

        public SplitViewToolStrip () {
            Compose ();
        }

        protected virtual void Compose () {
            var size = new Xwt.Size (36, 36);

            GraphStreamViewCommand = new ToolStripCommand {
                Action = s => ViewMode = SplitView.ViewMode = SplitViewMode.GraphContent,
                Image = Iconery.GraphContentView,
                Size = size,
                ToolTipText = "show contents"
            };
            GraphGraphViewCommand = new ToolStripCommand {
                Action = s => ViewMode = SplitView.ViewMode = SplitViewMode.GraphGraph,
                Image = Iconery.GraphGraphView,
                Size = size,
                ToolTipText = "show tiled graph"
            };

            ToggleViewCommand = new ToolStripCommand {
                Action = s => SplitView.ToggleView (),
                Image = Iconery.ToggleView,
                Size = size,
                ToolTipText = "toogle view"
            };

            OpenNewWindowCommand = new ToolStripCommand {
                Action = s => SplitView.ShowInNewWindow (),
                Image = Iconery.NewViewVisualNote,
                Size = size,
                ToolTipText = "open new window"
            };

            Action<bool> goBackOrForward = backOrForward => {
                if (SplitView.CanGoBackOrForward (backOrForward)) {
                    SplitView.GoBackOrForward (backOrForward);
                    CheckBackForward (SplitView);
                }
            };

            GoBackCommand = new ToolStripCommand {
                Action = s => goBackOrForward (false),
                Image = Iconery.GoPrevious,
                Size = size,
                ToolTipText = "navigate back"
            };

            GoForwardCommand = new ToolStripCommand {
                Action = s => goBackOrForward (true),
                Image = Iconery.GoNext,
                Size = size,
                ToolTipText = "navigate forward"
            };

            GoHomeCommand = new ToolStripCommand {
                Action = s => SplitView.GoHome (),
                Image = Iconery.GoHome,
                Size = size,
                ToolTipText = "go to favorites"
            };

            NewSheetCommand = new ToolStripCommand {
                Action = s => SplitView.NewSheet (),
                Image = Iconery.NewSheet,
                Size = size,
                ToolTipText = "new sheet"
            };

            NewNoteCommand = new ToolStripCommand {
                Action = s => SplitView.NewNote (),
                Image = Iconery.NewNote,
                Size = size,
                ToolTipText = "new note"
            };

            SaveSheetCommand = new ToolStripCommand {
                Action = s => SplitView.SaveDocument (),
                Image = Iconery.SaveContent,
                Size = size,
                ToolTipText = "save content"
            };

            GraphStreamViewButton = new ToolStripButton (GraphStreamViewCommand){ IsCheckable = true };
            GraphGraphViewButton = new ToolStripButton (GraphGraphViewCommand){ IsCheckable = true };
            var toggleViewButton = new ToolStripButton (ToggleViewCommand );
            var newWindowButton = new ToolStripButton (OpenNewWindowCommand );
            GoBackButton = new ToolStripButton (GoBackCommand );
            GoForwardButton = new ToolStripButton (GoForwardCommand );
            var goHomeButton = new ToolStripButton (GoHomeCommand );
            var newSheetButton = new ToolStripButton (NewSheetCommand );
            var newNoteButton = new ToolStripButton (NewNoteCommand );
            var saveSheetButton = new ToolStripButton (SaveSheetCommand );

            SheetCombo = new ComboBox { Width = 100 };
            SheetCombo.SelectionChanged += DoSelect;

            var sheetComboHost = new ToolStripItemHost () { Child = SheetCombo };

            this.AddItems (
                sheetComboHost,
                saveSheetButton,

                new ToolStripSeparator (),
                GraphStreamViewButton,
                GraphGraphViewButton,
                toggleViewButton,

                new ToolStripSeparator (),
                GoBackButton,
                GoForwardButton,
                goHomeButton,
                new ToolStripSeparator (),
                newSheetButton,
                newNoteButton,
                newWindowButton,
                new ToolStripSeparator ()
                );
        }

        protected SplitViewMode _viewMode = SplitViewMode.GraphContent;
        [Browsable (false)]
        [DesignerSerializationVisibility (DesignerSerializationVisibility.Hidden)]
        public virtual SplitViewMode ViewMode {
            get {
                if (GraphStreamViewButton.IsChecked.Value)
                    _viewMode = SplitViewMode.GraphContent;
                else
                    _viewMode = SplitViewMode.GraphGraph;
                return _viewMode;

            }
            set {
                _viewMode = value;
                if (value == SplitViewMode.GraphContent) {
                    GraphStreamViewButton.IsChecked = true;
                    GraphGraphViewButton.IsChecked = false;

                } else {
                    GraphStreamViewButton.IsChecked = false;
                    GraphGraphViewButton.IsChecked = true;
                }

            }
        }

        ISplitView _splitView = null;
        public ISplitView SplitView {
            get {
                if (_splitView == null) {
                    _splitView = new SplitViewDumnmy ();
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

        protected virtual void ViewChanged (object sender, EventArgs e) { Attach (sender); }

        public ISceneManager SceneManager { get; set; }

        public override void Detach (object sender) { }

        public override void Attach (object sender) {
            var display = sender as IGraphSceneDisplay<IVisual, IVisualEdge>;
            if (display != null)
                CurrentDisplay = display;

            if (SplitView != null) {
                CheckBackForward (SplitView);
                ViewMode = SplitView.ViewMode;
            }
            AttachSheets ();
        }

        private IList<SceneInfo> _sheets = new List<SceneInfo> ();
        public virtual void AttachSheets () {
            var display = CurrentDisplay;

            if (display == null)
                return;


            Func<SceneInfo, bool> dontCare = i => {
                if (display.DataId == i.Id)
                    return true;
                var adj = SplitView.AdjacentDisplay (display);
                if (adj?.DataId == i.Id)
                    return true;
                return false;
            };

            _sheets.Clear ();
            SheetCombo.Items.Clear ();

            Action<SceneInfo> addInfo = s => {
                _sheets.Add (s);
                SheetCombo.Items.Add (s.Name);
            };

            SceneManager.SheetStore.VisitRegisteredSheetInfos (s => addInfo (s));

            var lastSelected = _sheets.FirstOrDefault (i => i.Id == display.Info.Id);
            if (lastSelected == null) {
                addInfo (display.Info);
                lastSelected = display.Info;
            }

            SelectSheet = (s, e) => {
                var selectedIndex = SheetCombo.SelectedIndex;
                if (selectedIndex != -1) {
                    var info = _sheets [selectedIndex];
                    if (info != lastSelected && !dontCare (info)) {
                        SplitView.LoadSheet (_sheets [selectedIndex]);
                        lastSelected = info;
                    } else {
                        SheetCombo.SelectionChanged -= DoSelect;
                        var idx = _sheets.IndexOf (lastSelected);
                        SheetCombo.SelectedIndex = idx;
                        SheetCombo.SelectionChanged += DoSelect;
                    }
                }
            };

            SheetCombo.SelectionChanged -= DoSelect;
            {
                var idx = _sheets.IndexOf (lastSelected);
                SheetCombo.SelectedIndex = idx;
                SheetCombo.SelectionChanged += DoSelect;
            }
        }

        Action<object, EventArgs> SelectSheet { get; set; }
        protected virtual void DoSelect (object sender, System.EventArgs e) {
            SelectSheet?.Invoke (sender, e);
        }

        public void CheckBackForward (ISplitView splitView) {
            GoForwardButton.IsEnabled = splitView.CanGoBackOrForward (true);
            GoBackButton.IsEnabled = splitView.CanGoBackOrForward (false);
        }
    }

    public interface ISplitViewToolStripBackend : IDisplayToolStripBackend {
    }

    public class SheetComboComposer { 

        public ISceneManager SceneManager { get; set; }

        public ComboBox SheetCombo { get; set; }

        public Action<object, EventArgs> SelectSheet { get; protected set; }

        public virtual void AttachSheets (IGraphSceneDisplay<IVisual, IVisualEdge> display, ISplitView SplitView) {
            
            if (display == null || SheetCombo == null)
                return;

            SheetCombo.Items.Clear ();
            var _sheets = new List<SceneInfo> ();

            Func<SceneInfo, bool> dontCare = i => {
                if (display.DataId == i.Id)
                    return true;
                var adj = SplitView.AdjacentDisplay (display);
                if (adj?.DataId == i.Id)
                    return true;
                return false;
            };

            Action<SceneInfo> addInfo = s => {
                _sheets.Add (s);
                SheetCombo.Items.Add (s.Name);
            };

            SceneManager.SheetStore.VisitRegisteredSheetInfos (s => addInfo (s));

            var lastSelected = _sheets.FirstOrDefault (i => i.Id == display.Info.Id);
            if (lastSelected == null) {
                addInfo (display.Info);
                lastSelected = display.Info;
            }

            Action resetSelected = () => { 
                SheetCombo.SelectionChanged -= DoSelect;
                var idx = _sheets.IndexOf (lastSelected);
                SheetCombo.SelectedIndex = idx;
                SheetCombo.SelectionChanged += DoSelect;
            };

            SelectSheet = (s, e) => {
                var selectedIndex = SheetCombo.SelectedIndex;
                if (selectedIndex != -1) {
                    var info = _sheets [selectedIndex];
                    if (info != lastSelected && !dontCare (info)) {
                        SplitView.LoadSheet (_sheets [selectedIndex]);
                        lastSelected = info;
                    } else {
                        resetSelected ();  
                    }
                }
            };

            resetSelected ();

        }

        protected virtual void DoSelect (object sender, System.EventArgs e) {
            SelectSheet?.Invoke (sender, e);
        }

    }
}