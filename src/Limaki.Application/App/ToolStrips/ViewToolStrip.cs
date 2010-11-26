/*
 * Limaki 
 * Version 0.081
 * 
 * This code is free software; you can redistribute it and/or modify it
 * under the terms of the GNU General Public License version 2 only, as
 * published by the Free Software Foundation.
 * 
 * Author: Lytico
 * Copyright (C) 2006-2008 Lytico
 *
 * http://limada.sourceforge.net
 * 
 */

using System;
using System.Windows.Forms;
using Limaki.Common;
using Limaki.Winform.Displays;
using Limaki.Winform.Viewers;

namespace Limaki.Winform.Controls.ToolStrips {
    public partial class ViewToolStrip : ToolStrip, IToolView {
        public ViewToolStrip() {
            InitializeComponent();
        }

        ISplitView _controller = null;
        public ISplitView Controller {
            get {
                if (_controller == null) {
                    _controller = new ViewToolsController(this);
                }
                return _controller;
            }
            set {
                if(_controller != value) {
                    if (_controller != null) {
                        _controller.RefreshToolViewer -= this.Attach;
                    }
                    if (value != null) {
                        value.RefreshToolViewer += this.Attach;
                    }
                }
                _controller = value;
            }
        }

        public void Attach(object sender, EventArgs<ISplitView> e) {
            CheckBackForward (e.Arg);
            this.ViewMode = e.Arg.ViewMode;
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
            Controller.ViewMode = this.ViewMode;
        }

        private void GraphStreamView(object sender, EventArgs e) {
            this.ViewMode = SplitViewMode.GraphStream;
            Controller.ViewMode = this.ViewMode;
        }

        private void ToggleView(object sender, EventArgs e) {
            Controller.ToggleView ();
        }

        public void CheckBackForward(ISplitView controller) {
            goForwardButton.Enabled = controller.CanGoBackOrForward(true);
            goBackButton.Enabled = controller.CanGoBackOrForward(false);   
        }
        private void GoBack(object sender, EventArgs e) {
            if (Controller.CanGoBackOrForward(false)) {
                Controller.GoBackOrForward (false);
                CheckBackForward(Controller);
            }
        }
        private void GoForward(object sender, EventArgs e) {
            if (Controller.CanGoBackOrForward(true)) {
                Controller.GoBackOrForward(true);
                CheckBackForward(Controller);
            }
        }

        private void GoHome(object sender, EventArgs e) {
            Controller.GoHome ();
        }

        private void NewSheet(object sender, EventArgs e) {
            Controller.NewSheet ();
        }

        private void NewNote(object sender, EventArgs e) {
            Controller.NewNote ();
        }

        private void SaveDocument(object sender, EventArgs e) {
            Controller.SaveDocument ();
        }

    }


    public class ViewToolsController : ToolsController<WidgetDisplay, object>, ISplitView {
        ViewToolStrip toolStrip = null;
        public ViewToolsController(){}
        public ViewToolsController(ViewToolStrip editor) {
            this.toolStrip = editor;
        }

        public event EventHandler<EventArgs<ISplitView>> RefreshToolViewer;

        public override void Attach() {
            var currentDisplay = this.CurrentDisplay;
            if (currentDisplay == null || toolStrip == null) {
                return;
            }
            //toolStrip.Attach(


            //    );
        }

        public virtual void ToggleView() {
            
        }

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

        public virtual void SetGraphGraphView() {
            
        }
        public virtual void SetGraphStreamView() {

        }

        public virtual bool CanGoBackOrForward(bool forward) {
            return false;
        }

        public virtual void GoHome() {

        }

        public virtual void GoBackOrForward(bool forward) {

        }
        public virtual void NewSheet() {

        }
        public virtual void NewNote() {

        }
        public virtual void SaveDocument() {

        }
    }
}
