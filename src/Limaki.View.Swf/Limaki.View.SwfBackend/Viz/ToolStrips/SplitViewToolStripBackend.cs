/*
 * Limaki 
 * 
 * This code is free software; you can redistribute it and/or modify it
 * under the terms of the GNU General Public License version 2 only, as
 * published by the Free Software Foundation.
 * 
 * Author: Lytico
 * Copyright (C) 2006-2013 Lytico
 *
 * http://www.limada.org
 * 
 */

using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using Limada.View.Vidgets;
using Limaki.Usecases.Vidgets;
using Limaki.View;
using Limaki.View.SwfBackend.VidgetBackends;
using Limaki.View.Vidgets;

namespace Limaki.View.SwfBackend.Viz.ToolStrips {

    public partial class SplitViewToolStripBackend : ToolStripBackend, ISplitViewToolStripBackend {

        public SplitViewToolStripBackend () {
            InitializeComponent();
           
        }

        public override void InitializeBackend (Limaki.View.IVidget frontend, VidgetApplicationContext context) {
            this.Frontend = (SplitViewToolStrip)frontend;
            Compose();
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public SplitViewToolStrip Frontend { get; protected set; }

        private ToolStripButtonEx graphStreamViewButton;
        private ToolStripButtonEx graphGraphViewButton;

        private ToolStripButtonEx goBackButton;
        private ToolStripButtonEx goForwardButton;


        public virtual void Compose () {

            graphStreamViewButton = new ToolStripButtonEx { Command = Frontend.GraphStreamViewCommand };
            graphGraphViewButton = new ToolStripButtonEx { Command = Frontend.GraphGraphViewCommand };
            var toggleViewButton = new ToolStripButtonEx { Command = Frontend.ToggleViewCommand };
            var viewVisualNote = new ToolStripButtonEx { Command = Frontend.OpenNewWindowCommand };
            goBackButton = new ToolStripButtonEx { Command = Frontend.GoBackCommand };
            goForwardButton = new ToolStripButtonEx { Command = Frontend.GoForwardCommand };
            var goHomeButton = new ToolStripButtonEx { Command = Frontend.GoHomeCommand };
            var newSheetButton = new ToolStripButtonEx { Command = Frontend.NewSheetCommand };
            var newNoteButton = new ToolStripButtonEx { Command = Frontend.NewNoteCommand };
            var saveSheetButton = new ToolStripButtonEx { Command = Frontend.SaveSheetCommand };

            this.Items.AddRange(new ToolStripItem[] {
                                                        graphStreamViewButton,
                                                        graphGraphViewButton,
                                                        toggleViewButton,
                                                        viewVisualNote,
                                                        new System.Windows.Forms.ToolStripSeparator {Size = new System.Drawing.Size(6, 27)},
                                                        goBackButton,
                                                        goForwardButton,
                                                        goHomeButton,
                                                        new System.Windows.Forms.ToolStripSeparator {Size = new System.Drawing.Size(6, 27)},
                                                        newSheetButton,
                                                        newNoteButton,
                                                        saveSheetButton,
                                                        sheetCombo
                                                    });

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

        public void CheckBackForward (ISplitView splitView) {
            goForwardButton.Enabled = splitView.CanGoBackOrForward(true);
            goBackButton.Enabled = splitView.CanGoBackOrForward(false);
        }

        private IList<SceneInfo> _sheets = new List<SceneInfo>();
        public virtual void AttachSheets () {
            sheetCombo.Items.Clear();
            var name = Frontend.AttachSheets(_sheets);

            if (name != null) {
                sheetCombo.Text = name;
            }
            sheetCombo.Items.AddRange(_sheets.Select(i => i.Name).ToArray());
        }

        protected virtual void SelectSheet (object sender, System.EventArgs e) {
            if (sheetCombo.SelectedIndex != -1) {
                Frontend.SelectSheet(_sheets[sheetCombo.SelectedIndex]);
            }
        }
    }


}