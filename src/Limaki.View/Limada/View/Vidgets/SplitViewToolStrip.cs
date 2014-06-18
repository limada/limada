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
using Limaki.Common.Linqish;

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
                Action = s => ViewMode = SplitView.ViewMode = SplitViewMode.GraphStream,
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
            var viewVisualNote = new ToolStripButton (OpenNewWindowCommand );
            GoBackButton = new ToolStripButton (GoBackCommand );
            GoForwardButton = new ToolStripButton (GoForwardCommand );
            var goHomeButton = new ToolStripButton (GoHomeCommand );
            var newSheetButton = new ToolStripButton (NewSheetCommand );
            var newNoteButton = new ToolStripButton (NewNoteCommand );
            var saveSheetButton = new ToolStripButton (SaveSheetCommand );

            SheetCombo = new ComboBox { Width = 100 };
            SheetCombo.Items.Clear ();
            SheetCombo.SelectionChanged += SelectSheet;

            var comboHost = new ToolStripItemHost () { Child = SheetCombo };

            this.AddItems (
                GraphStreamViewButton,
                GraphGraphViewButton,
                toggleViewButton,
                viewVisualNote,
                new ToolStripSeparator (),
                GoBackButton,
                GoForwardButton,
                goHomeButton,
                new ToolStripSeparator (),
                newSheetButton,
                newNoteButton,
                saveSheetButton,
                comboHost
                );
        }

        protected SplitViewMode _viewMode = SplitViewMode.GraphStream;
        [Browsable (false)]
        [DesignerSerializationVisibility (DesignerSerializationVisibility.Hidden)]
        public virtual SplitViewMode ViewMode {
            get {
                if (GraphStreamViewButton.IsChecked.Value)
                    _viewMode = SplitViewMode.GraphStream;
                else
                    _viewMode = SplitViewMode.GraphGraph;
                return _viewMode;

            }
            set {
                _viewMode = value;
                if (value == SplitViewMode.GraphStream) {
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

        public ISheetManager SheetManager { get; set; }

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
            string result = null;

            var display = CurrentDisplay;
            var dontCare = new Set<long> ();

            if (display != null) {
                result = display.Info.Name;
                dontCare.Add (display.DataId);
                var adj = SplitView.AdjacentDisplay (display);
                if (adj != null)
                    dontCare.Add (adj.DataId);
            }
            _sheets.Clear ();

            SheetManager.VisitRegisteredSheets (s => {
                if (SheetManager.StoreContains (s.Id) && !dontCare.Contains (s.Id))
                    _sheets.Add (s);
            });

            SheetCombo.Items.Clear ();

            if (result != null) {
                SheetCombo.SelectedItem = result;
            }
            _sheets.ForEach (i => SheetCombo.Items.Add (i.Name));

        }


        protected virtual void SelectSheet (object sender, System.EventArgs e) {
            if (SheetCombo.SelectedIndex != -1) {
                SplitView.LoadSheet (_sheets[SheetCombo.SelectedIndex]);
            }
        }

        public void CheckBackForward (ISplitView splitView) {
            GoForwardButton.IsEnabled = splitView.CanGoBackOrForward (true);
            GoBackButton.IsEnabled = splitView.CanGoBackOrForward (false);
        }
    }

    public interface ISplitViewToolStripBackend : IDisplayToolStripBackend {
    }
}