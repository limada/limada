/*
 * Limaki 
 * 
 * This code is free software; you can redistribute it and/or modify it
 * under the terms of the GNU General Public License version 2 only, as
 * published by the Free Software Foundation.
 * 
 * Author: Lytico
 * Copyright (C) 2008-2017 Lytico
 *
 * http://www.limada.org
 * 
 */

using System;
using System.Collections.Generic;
using Limaki.Iconerias;
using Limaki.Usecases.Vidgets;
using Limaki.View.Vidgets;
using Limaki.View.Visuals;
using Limaki.View.Viz;
using Limaki.View.Viz.Visualizers.Toolbars;
using Xwt.Backends;
using System.ComponentModel;
using System.Linq;

namespace Limaki.View.Vidgets {

    [BackendType (typeof (ISplitViewToolbarBackend))]
    public class SplitViewToolbar : DisplayToolbar<IGraphSceneDisplay<IVisual, IVisualEdge>> {

        public IToolbarCommand GraphStreamViewCommand { get; set; }
        public IToolbarCommand GraphGraphViewCommand { get; set; }
        public IToolbarCommand ToggleViewCommand { get; set; }

        public IToolbarCommand OpenNewWindowCommand { get; set; }

        public IToolbarCommand GoBackCommand { get; set; }
        public IToolbarCommand GoForwardCommand { get; set; }
        public IToolbarCommand GoHomeCommand { get; set; }

        public IToolbarCommand NewSheetCommand { get; set; }
        public IToolbarCommand NewNoteCommand { get; set; }
        public IToolbarCommand SaveSheetCommand { get; set; }

        protected ToolbarButton GraphStreamViewButton { get; set; }
        protected ToolbarButton GraphGraphViewButton { get; set; }
        
        protected ToolbarButton GoBackButton { get; set; }
        protected ToolbarButton GoForwardButton { get; set; }
        
        protected ComboBox SheetCombo;

        public SplitViewToolbar () {
            Compose ();
        }

        protected virtual void Compose () {
            var size = new Xwt.Size (36, 36);

            GraphStreamViewCommand = new ToolbarCommand {
                Action = s => ViewMode = SplitView.ViewMode = SplitViewMode.GraphContent,
                Image = Iconery.GraphContentView,
                Size = size,
                ToolTipText = "show contents"
            };
            GraphGraphViewCommand = new ToolbarCommand {
                Action = s => ViewMode = SplitView.ViewMode = SplitViewMode.GraphGraph,
                Image = Iconery.GraphGraphView,
                Size = size,
                ToolTipText = "show tiled graph"
            };

            ToggleViewCommand = new ToolbarCommand {
                Action = s => SplitView.ToggleView (),
                Image = Iconery.ToggleView,
                Size = size,
                ToolTipText = "toogle view"
            };

            OpenNewWindowCommand = new ToolbarCommand {
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

            GoBackCommand = new ToolbarCommand {
                Action = s => goBackOrForward (false),
                Image = Iconery.GoPrevious,
                Size = size,
                ToolTipText = "navigate back"
            };

            GoForwardCommand = new ToolbarCommand {
                Action = s => goBackOrForward (true),
                Image = Iconery.GoNext,
                Size = size,
                ToolTipText = "navigate forward"
            };

            GoHomeCommand = new ToolbarCommand {
                Action = s => SplitView.GoHome (),
                Image = Iconery.GoHome,
                Size = size,
                ToolTipText = "go to favorites"
            };

            NewSheetCommand = new ToolbarCommand {
                Action = s => SplitView.NewSheet (),
                Image = Iconery.NewSheet,
                Size = size,
                ToolTipText = "new sheet"
            };

            NewNoteCommand = new ToolbarCommand {
                Action = s => SplitView.NewNote (),
                Image = Iconery.NewNote,
                Size = size,
                ToolTipText = "new note"
            };

            SaveSheetCommand = new ToolbarCommand {
                Action = s => SplitView.SaveDocument (),
                Image = Iconery.SaveContent,
                Size = size,
                ToolTipText = "save content"
            };

            GraphStreamViewButton = new ToolbarButton (GraphStreamViewCommand){ IsCheckable = true };
            GraphGraphViewButton = new ToolbarButton (GraphGraphViewCommand){ IsCheckable = true };
            var toggleViewButton = new ToolbarButton (ToggleViewCommand );
            var newWindowButton = new ToolbarButton (OpenNewWindowCommand );
            GoBackButton = new ToolbarButton (GoBackCommand );
            GoForwardButton = new ToolbarButton (GoForwardCommand );
            var goHomeButton = new ToolbarButton (GoHomeCommand );
            var newSheetButton = new ToolbarButton (NewSheetCommand );
            var newNoteButton = new ToolbarButton (NewNoteCommand );
            var saveSheetButton = new ToolbarButton (SaveSheetCommand );

            SheetCombo = new ComboBox { Width = 100 };

            var sheetComboHost = new ToolbarItemHost () { Child = SheetCombo };

            this.AddItems (
                sheetComboHost,
                saveSheetButton,

                new ToolbarSeparator (),
                GraphStreamViewButton,
                GraphGraphViewButton,
                toggleViewButton,

                new ToolbarSeparator (),
                GoBackButton,
                GoForwardButton,
                goHomeButton,
                new ToolbarSeparator (),
                newSheetButton,
                newNoteButton,
                newWindowButton,
                new ToolbarSeparator ()
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

        SheetComboComposer sheetComboComposer = new SheetComboComposer ();
        public override void Attach (object sender) {
            var display = sender as IGraphSceneDisplay<IVisual, IVisualEdge>;
            if (display != null)
                CurrentDisplay = display;

            if (SplitView != null) {
                CheckBackForward (SplitView);
                ViewMode = SplitView.ViewMode;
            }

            sheetComboComposer.SceneManager = this.SceneManager;
            sheetComboComposer.SheetCombo = this.SheetCombo;
            sheetComboComposer.AttachSheets (display, SplitView);
        }


        public void CheckBackForward (ISplitView splitView) {
            GoForwardButton.IsEnabled = splitView.CanGoBackOrForward (true);
            GoBackButton.IsEnabled = splitView.CanGoBackOrForward (false);
        }
    }

    public interface ISplitViewToolbarBackend : IDisplayToolbarBackend {
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

            Func<long, bool> isinSplitView = id => SplitView.Display1.DataId == id || SplitView.Display2.DataId == id;
            Action<long> activateInSplitView = id => {
                SplitView.CurrentDisplay.Data.Focused = null;
                var d = SplitView.Display1.DataId == id ? SplitView.Display1 : SplitView.Display2;
                if (SplitView.CurrentVidget != SplitView.Display1 || SplitView.CurrentVidget != SplitView.Display2) {
                    SplitView.ViewMode = SplitViewMode.GraphGraph;
                    SplitView.ViewMode = SplitViewMode.GraphContent;
                }
                d.Wink ();
                d.QueueDraw ();
            };

            SelectSheet = (s, e) => {
                var selectedIndex = SheetCombo.SelectedIndex;
                if (selectedIndex != -1) {
                    var info = _sheets [selectedIndex];
                    if (isinSplitView(info.Id)) {
                        activateInSplitView (info.Id);
                        resetSelected ();
                        return;
                    }
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