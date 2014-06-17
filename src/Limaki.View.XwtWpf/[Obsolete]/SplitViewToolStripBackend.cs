/*
 * Limaki 
 * 
 * This code is free software; you can redistribute it and/or modify it
 * under the terms of the GNU General Public License version 2 only, as
 * published by the Free Software Foundation.
 * 
 * Author: Lytico
 * Copyright (C) 2014 Lytico
 *
 * http://www.limada.org
 * 
 */

using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Limada.View.Vidgets;
using Limaki.Common.Linqish;
using Limaki.Usecases.Vidgets;
using Limaki.View.Vidgets;
using SWC= System.Windows.Controls;

namespace Limaki.View.WpfBackend {

    public class SplitViewToolStripBackend0 : ToolStripBackend, ISplitViewToolStripBackend0 {

        public override void InitializeBackend (Limaki.View.IVidget frontend, VidgetApplicationContext context) {
            this.Frontend = (SplitViewToolStrip0)frontend;
            Compose();
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public SplitViewToolStrip0 Frontend { get; protected set; }

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

        private ToolStripButton0 GraphStreamViewButton { get; set; }
        private ToolStripButton0 GraphGraphViewButton { get; set; }

        private ToolStripButton0 GoBackButton { get; set; }
        private ToolStripButton0 GoForwardButton { get; set; }

        private SWC.ComboBox SheetCombo;

        protected override void Compose () {
            base.Compose ();

            GraphStreamViewButton = new ToolStripButton0 {Command = Frontend.GraphStreamViewCommand, IsCheckable = true};
            GraphGraphViewButton = new ToolStripButton0 {Command = Frontend.GraphGraphViewCommand, IsCheckable = true};
            var toggleViewButton = new ToolStripButton0 {Command = Frontend.ToggleViewCommand};
            var viewVisualNote = new ToolStripButton0 {Command = Frontend.OpenNewWindowCommand};
            GoBackButton = new ToolStripButton0 {Command = Frontend.GoBackCommand};
            GoForwardButton = new ToolStripButton0 {Command = Frontend.GoForwardCommand};
            var goHomeButton = new ToolStripButton0 {Command = Frontend.GoHomeCommand};
            var newSheetButton = new ToolStripButton0 {Command = Frontend.NewSheetCommand};
            var newNoteButton = new ToolStripButton0 {Command = Frontend.NewNoteCommand};
            var saveSheetButton = new ToolStripButton0 {Command = Frontend.SaveSheetCommand};

            SheetCombo = new SWC.ComboBox { Width = 100 };
            SheetCombo.Items.Clear ();

            var sepStyle = (Style) FindResource (ToolBar.SeparatorStyleKey);
            this.AddItems (
                GraphStreamViewButton,
                GraphGraphViewButton,
                toggleViewButton,
                viewVisualNote,
                new Separator {Style = sepStyle},
                GoBackButton,
                GoForwardButton,
                goHomeButton,
                new Separator {Style = sepStyle},
                newSheetButton,
                newNoteButton,
                saveSheetButton,
                SheetCombo
                );
        }

        public void CheckBackForward (ISplitView splitView) {
            GoForwardButton.IsEnabled = splitView.CanGoBackOrForward (true);
            GoBackButton.IsEnabled = splitView.CanGoBackOrForward (false);
        }

        private IList<SceneInfo> _sheets = new List<SceneInfo> ();
        public virtual void AttachSheets () {
            SheetCombo.Items.Clear ();
            var name = Frontend.AttachSheets (_sheets);

            if (name != null) {
                SheetCombo.Text = name;
            }
            _sheets.ForEach (i => SheetCombo.Items.Add (i.Name));

        }

        protected virtual void SelectSheet (object sender, System.EventArgs e) {
            if (SheetCombo.SelectedIndex != -1) {
                Frontend.SelectSheet (_sheets[SheetCombo.SelectedIndex]);
            }
        }
    }
}