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

using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using Limaki.Drawing;
using Limaki.UseCases.Viewers;
using Limaki.UseCases.Viewers.ToolStripViewers;

namespace Limaki.UseCases.Winform.Viewers.ToolStripViewers {
    public partial class SplitViewToolStrip : ToolStrip, ISplitViewTool {
        public SplitViewToolStrip() {
            InitializeComponent();
            Compose();
        }
        SplitViewToolController _controller = null;
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public SplitViewToolController Controller {
            get { return _controller ?? (_controller = new SplitViewToolController { Tool = this }); }
        }

        public virtual void Compose() {
            this.graphStreamViewButton.Click += (s,e)=>Controller.GraphStreamView();
            this.graphGraphViewButton.Click += (s,e)=>Controller.GraphGraphView();
            this.toggleViewButton.Click += (s,e)=>Controller.ToggleView();
            this.goBackButton.Click += (s,e)=>Controller.GoBack();
            this.newSheetButton.Click += (s,e)=>Controller.NewSheet();
            this.goForwardButton.Click += (s,e)=>Controller.GoForward();
            this.goHomeButton.Click += (s,e)=>Controller.GoHome();
            this.newNoteButton.Click += (s,e)=>Controller.NewNote();
            this.saveSheetButton.Click += (s,e)=>Controller.SaveDocument();
            this.sheetCombo.SelectedIndexChanged += SelectSheet;
        }


        protected SplitViewMode _viewMode = SplitViewMode.GraphStream;
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public virtual SplitViewMode ViewMode {
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

        public void CheckBackForward(ISplitView splitView) {
            goForwardButton.Enabled = splitView.CanGoBackOrForward(true);
            goBackButton.Enabled = splitView.CanGoBackOrForward(false);
        }

       

        private IList<SceneInfo> _sheets = new List<SceneInfo>();
        public virtual void AttachSheets() {
            sheetCombo.Items.Clear();
            var name = Controller.AttachSheets(_sheets);

            if (name != null) {
                sheetCombo.Text = name;
            }
            sheetCombo.Items.AddRange(_sheets.Select(i => i.Name).ToArray());
        }

        protected virtual void SelectSheet(object sender, System.EventArgs e) {
            if (sheetCombo.SelectedIndex != -1) {
                Controller.SelectSheet(_sheets[sheetCombo.SelectedIndex]);
            }
        }
    }

    
}