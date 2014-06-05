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
using ToolStrip = System.Windows.Forms.ToolStrip;
using ToolStripItem = System.Windows.Forms.ToolStripItem;

namespace Limaki.View.SwfBackend.Viz.ToolStrips {

    public partial class SplitViewToolStripBackend : ToolStripBackend, ISplitViewToolStripBackend {

        public SplitViewToolStripBackend () {
            InitializeComponent();
           
        }

        public override void InitializeBackend (Limaki.View.IVidget frontend, VidgetApplicationContext context) {
            base.InitializeBackend (frontend, context);
            this.Frontend = (SplitViewToolStrip)frontend;
            Compose();
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public new SplitViewToolStrip Frontend { get; protected set; }

        private ToolStripButtonBackend graphStreamViewButton;
        private ToolStripButtonBackend graphGraphViewButton;

        private ToolStripButtonBackend goBackButton;
        private ToolStripButtonBackend goForwardButton;


        public virtual void Compose () {

            graphStreamViewButton = new ToolStripButtonBackend { Command = Frontend.GraphStreamViewCommand };
            graphGraphViewButton = new ToolStripButtonBackend { Command = Frontend.GraphGraphViewCommand };
            var toggleViewButton = new ToolStripButtonBackend { Command = Frontend.ToggleViewCommand };
            var viewVisualNote = new ToolStripButtonBackend { Command = Frontend.OpenNewWindowCommand };
            goBackButton = new ToolStripButtonBackend { Command = Frontend.GoBackCommand };
            goForwardButton = new ToolStripButtonBackend { Command = Frontend.GoForwardCommand };
            var goHomeButton = new ToolStripButtonBackend { Command = Frontend.GoHomeCommand };
            var newSheetButton = new ToolStripButtonBackend { Command = Frontend.NewSheetCommand };
            var newNoteButton = new ToolStripButtonBackend { Command = Frontend.NewNoteCommand };
            var saveSheetButton = new ToolStripButtonBackend { Command = Frontend.SaveSheetCommand };

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