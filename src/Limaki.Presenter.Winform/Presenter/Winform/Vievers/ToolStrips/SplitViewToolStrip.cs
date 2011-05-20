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
using Limaki.UseCases.Viewers.ToolStrips;
using Limaki.UseCases.Winform.Viewers.ToolStrips;
using Limaki.UseCases.Viewers;
using Limaki.Presenter.Visuals;
using System.ComponentModel;

namespace Limaki.UseCases.Winform.Viewers.ToolStrips {
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

        public void Attach(object sender, EventArgs e) {
            var splitView = sender as ISplitView;
            if (splitView != null) {
                CheckBackForward(splitView);
                this.ViewMode = splitView.ViewMode;
            }
        }

        public SplitViewMode _viewMode = SplitViewMode.GraphStream;
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
    }
}