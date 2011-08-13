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
 * http://limada.sourceforge.net
 * 
 */

using System;
using System.Windows.Forms;
using Limaki.Common;
using Limaki.UseCases.Winform.Viewers.ToolStripViewers;
using Limaki.UseCases.Viewers;
using Limaki.Presenter.Visuals;
using System.ComponentModel;
using System.Linq;
using System.Collections.Generic;
using Limaki.Drawing;
using Limaki.Presenter.Display;
using Limaki.Visuals;

namespace Limaki.UseCases.Winform.Viewers.ToolStripViewers {
    [TODO("make a controller")]
    public partial class SplitViewToolStrip : ToolStrip {
        public SplitViewToolStrip() {
            InitializeComponent();
        }

        ISplitView _splitView = null;
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public ISplitView SplitView {
            get {
                if (_splitView == null) {
                    _splitView = new SplitViewDumnmy();
                }
                return _splitView;
            }
            set {
                if(_splitView != value) {
                    if (_splitView != null) {
                        _splitView.ViewChanged -= this.Attach;
                    }
                    if (value != null) {
                        value.ViewChanged += this.Attach;
                    }
                }
                _splitView = value;
            }
        }
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public ISheetManager SheetManager { get; set; }

        public void Attach(object sender, EventArgs e) {
            var splitView = sender as ISplitView;
            if (splitView != null) {
                CheckBackForward(splitView);
                this.ViewMode = splitView.ViewMode;
            }
            AttachSheets();
        }

        public SplitViewMode _viewMode = SplitViewMode.GraphStream;
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public SplitViewMode ViewMode {
            get {
                if (graphStreamViewButton.Checked)
                    _viewMode = SplitViewMode.GraphStream;
                else 
                    _viewMode = SplitViewMode.GraphGraph;
                return _viewMode;

            }
            set {
                _viewMode = value;
                if (value == SplitViewMode.GraphStream) {
                    graphStreamViewButton.Checked = true;
                    graphGraphViewButton.Checked = false;
                    
                } else {
                    graphStreamViewButton.Checked = false;
                    graphGraphViewButton.Checked = true;
                }
                
            }
        }
        
        private void GraphGraphView(object sender, EventArgs e) {
            this.ViewMode = SplitViewMode.GraphGraph;
            SplitView.ViewMode = this.ViewMode;
        }

        private void GraphStreamView(object sender, EventArgs e) {
            this.ViewMode = SplitViewMode.GraphStream;
            SplitView.ViewMode = this.ViewMode;
        }

        private void ToggleView(object sender, EventArgs e) {
            SplitView.ToggleView ();
        }

        public void CheckBackForward(ISplitView splitView) {
            goForwardButton.Enabled = splitView.CanGoBackOrForward(true);
            goBackButton.Enabled = splitView.CanGoBackOrForward(false);   
        }

        private void GoBack(object sender, EventArgs e) {
            if (SplitView.CanGoBackOrForward(false)) {
                SplitView.GoBackOrForward (false);
                CheckBackForward(SplitView);
            }
        }

        private void GoForward(object sender, EventArgs e) {
            if (SplitView.CanGoBackOrForward(true)) {
                SplitView.GoBackOrForward(true);
                CheckBackForward(SplitView);
            }
        }

        private void GoHome(object sender, EventArgs e) {
            SplitView.GoHome ();
        }

        private void NewSheet(object sender, EventArgs e) {
            SplitView.NewSheet ();
        }

        private void NewNote(object sender, EventArgs e) {
            SplitView.NewNote ();
        }

        private void SaveDocument(object sender, EventArgs e) {
            SplitView.SaveDocument ();
        }
        private List<SceneInfo> _sheets = new List<SceneInfo>();
        public void AttachSheets() {
            sheetCombo.Items.Clear();
            var display = GetCurrentDisplay();
            if(display!=null) {
                sheetCombo.Text =display.Info.Name;
            }
            _sheets.Clear();
            SheetManager.VisitRegisteredSheets(s => _sheets.Add(s));
            sheetCombo.Items.AddRange(_sheets.Select(i=>i.Name).ToArray());


        }

        void SelectSheet(object sender, System.EventArgs e) {
            if (sheetCombo.SelectedIndex != -1) {
                SplitView.LoadSheet(_sheets[sheetCombo.SelectedIndex]);
            }
        }


        public Get<IGraphSceneDisplay<IVisual, IVisualEdge>> GetCurrentDisplay { get; set; }
    }

    public class SplitViewDumnmy : ISplitView {
        public event EventHandler ViewChanged;
        public virtual void ToggleView() {}

        SplitViewMode _viewMode = SplitViewMode.GraphStream;
        public SplitViewMode ViewMode {
            get { return _viewMode; }
            set {
                if (_viewMode != value) {
                    if (value == SplitViewMode.GraphStream)
                        this.SetGraphStreamView();
                    else if (value == SplitViewMode.GraphGraph)
                        this.SetGraphGraphView();
                }
                _viewMode = value;
            }
        }

        public virtual void SetGraphGraphView() {}
        public virtual void SetGraphStreamView() {}

        public virtual bool CanGoBackOrForward(bool forward) {
            return false;
        }

        public virtual void GoHome() {}
        public virtual void GoBackOrForward(bool forward) {}
        public virtual void NewSheet() {}
        public virtual void NewNote() {}
        public virtual void SaveDocument() {}
        public virtual void LoadSheet(SceneInfo info) { }
    }
}