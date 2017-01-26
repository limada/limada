/*
 * Limaki 
 * 
 * This code is free software; you can redistribute it and/or modify it
 * under the terms of the GNU General Public License version 2 only, as
 * published by the Free Software Foundation.
 * 
 * Author: Lytico
 * Copyright (C) 2010-2017 Lytico
 *
 * http://www.limada.org
 * 
 */

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;
using Limaki.Common;
using Limaki.Drawing;
using Limada.UseCases;
using Limada.View.ContentViewers;
using Limaki.View.ContentViewers;
using Limaki.View.SwfBackend;
using Limaki.View.SwfBackend.Controls;
using Limaki.View.SwfBackend.VidgetBackends;
using Limaki.View.Vidgets;
using Limaki.View.Viz.Modelling;
using Limaki.View.Viz.UI;
using DialogResult = Limaki.View.Vidgets.DialogResult;
using ImageExporter = Limaki.View.Viz.ImageExporter;
using MessageBoxButtons = Limaki.View.Vidgets.MessageBoxButtons;
using ToolStrip = System.Windows.Forms.ToolStrip;
using System.Linq;
using Xwt.Backends;

namespace Limada.Usecases {

    public class SwfConceptUseCaseComposer : IBackendConceptUseCaseComposer {

        public Form Mainform { get { return MainWindow.Backend.ToSwf() as Form; } }

        public ToolStripContainer ToolStripContainer { get; set; }
        public MenuStrip MenuStrip { get; set; }

        public ToolStripStatusLabel StatusLabel { get; set; }
        public StatusStrip StatusStrip { get; set; }

        public Vindow MainWindow { get; set; }

        public Xwt.Size WindowSize { get; set; }

        public Action OnShow { get; set; }

        public Xwt.Menu Menu { get; set; }

        public void Factor (ConceptUsecase useCase) {

            if (MainWindow == null) {
                this.MainWindow = new Vindow ();
            }

            useCase.MainWindow = this.MainWindow;
           
            ToolStripContainer = new ToolStripContainer ();

            StatusStrip = new StatusStrip ();
            StatusLabel = new ToolStripStatusLabel ();

            var splitViewBackend = useCase.SplitView.Backend.ToSwf() as SplitContainer;
            ToolStripContainer.SuspendLayout();
            ToolStripContainer.ContentPanel.Controls.Add(splitViewBackend);
            ToolStripContainer.ResumeLayout();
            ToolStripContainer.PerformLayout();
            
            splitViewBackend.SplitterDistance = (int)(ToolStripContainer.Width / 2);

            var viewerProvider = Registry.Pooled<ContentViewerProvider>();

            viewerProvider.Add(new HtmlContentViewer());
            viewerProvider.Add(new DigidocContentViewer());
            viewerProvider.Add(new TextContentViewerWithToolstrip0());
            viewerProvider.Add(new SheetViewer());

            if (PdfContentViewer.Available ())
                viewerProvider.Add (new PdfContentViewer ());

            if (MarkdownContentViewer.Available ())
                viewerProvider.Add (new MarkdownContentViewer ());

            viewerProvider.Add(new SdImageContentViewer());

            if (MarkdownContentViewer.Available ())
                viewerProvider.Add (new MarkdownContentViewer ());

        }

        public void Compose (ConceptUsecase useCase) {
            
            ToolStripContainer.Dock = DockStyle.Fill;
            Mainform.Controls.Add (ToolStripContainer);

            ToolStripContainer.BottomToolStripPanel.Controls.Add (StatusStrip);
            this.StatusStrip.Items.Add (StatusLabel);

            useCase.DataPostProcess =
                dataName => Mainform.Text = dataName + " - " + useCase.UseCaseTitle;

            useCase.MessageBoxShow = this.MessageBoxShow;
           
            useCase.Progress = (m, i, count) => {
                if (i == -1 && count == -1)
                    this.StatusLabel.Text = m;
                else
                    this.StatusLabel.Text = string.Format(m, i, count);
                Application.DoEvents ();
            };

            Mainform.FormClosing += (s, e) => useCase.Close ();
            Application.ApplicationExit += (s, e) => {
                useCase.Close ();
                useCase.Dispose ();
            };

            useCase.ApplicationQuit = () => useCase.ApplicationQuitted = true;

            useCase.ShowAboutWindow = () => {
                if (AboutForm == null) AboutForm = new AboutForm ();
                AboutForm.ShowDialog ();
            };

        }

        /// <summary>
        /// runs after usecasefactory.compose
        /// </summary>
        /// <param name="useCase">Use case.</param>
        public void Compose2 (ConceptUsecase useCase) {

            ComposeMenus (useCase);

            if (useCase.Toolbar == null)
                return;

            var toolStrips = useCase.Toolbar.Items
                .Cast<Limaki.View.IVidget> ()
                .Select (v => v.ToSwf () as ToolStrip).ToArray ();

            var utils = new ToolStripUtils {
                ToolStripItemSelectedColor = Color.White,
            };

            utils.InitializeToolstrips (
                this.ToolStripContainer.TopToolStripPanel,
                this.MenuStrip,
                toolStrips
                );
        }

        public void ComposeAdditionalMenus (ConceptUsecase useCase) {
            var l = new Localizer ();
            var addToFile = new ToolStripMenuItem[] {
                new ToolStripMenuItem (l["Print ..."], null, (s, e) => { this.Print (useCase); }),
                new ToolStripMenuItem (l["PrintPreview ..."], null, (s, e) => { this.PrintPreview (useCase); }),
            };
#if DEBUG
            var addToEdit = new ToolStripMenuItem[] {
                new ToolStripMenuItem (l["Merge"], null, (s, e) => { useCase.MergeVisual (); }),
            };
#endif
            var addToStyle = new ToolStripMenuItem[] {
                new ToolStripMenuItem (l["Layout"], null, (s, e) => { this.ShowLayoutEditor (useCase); }),
                new ToolStripMenuItem (l["StyleSheet"], null, (s, e) => { this.ShowStyleEditor (useCase); }),
            };

            var addToCurrentView = new ToolStripMenuItem (l["view as image ..."], null,
                (s, e) => { this.ExportAsImage (useCase); });
        }

        public void ComposeMenus (ConceptUsecase useCase) {

            if (useCase.Menu == null)
                return;

            ComposeAdditionalMenus (useCase);

            var menuBackend = useCase.Menu.GetBackend () as Xwt.Swf.Xwt.SwfBackend.MenuBackend;
            if (menuBackend != null) {
                MenuStrip = menuBackend.Menu;
            }
            
            var font = SystemFonts.MenuFont;
            MenuStrip.Font = font;

            Action<ToolStripMenuItem> setFont = null;
            setFont = (item) => {
                item.Font = font;
                foreach (ToolStripMenuItem sub in item.DropDownItems) {
                    sub.Font = font;
                    setFont (sub);
                }
            };

            foreach (ToolStripMenuItem item in MenuStrip.Items)
                setFont (item);
        }

        Form AboutForm = null;

        public DialogResult MessageBoxShow (string text, string title, MessageBoxButtons buttons) {
            return Converter.Convert (MessageBox.Show (Mainform, text, title, Converter.Convert (buttons)));
        }
        
        #region StyleEditors

        Rectangle ControlSize (Control control) {
            Rectangle result = Rectangle.Empty;
            foreach (Control c in control.Controls) {
                result.Location = new Point (
                    Math.Min (c.Left, result.Location.X),
                    Math.Min (c.Top, result.Location.Y));
                result.Size = new Size (
                    Math.Max (c.Left + c.Width, result.Width),
                    c.Height + result.Height
                    );

            }
            return result;
        }

        Options options = null;
        private void ShowLayoutEditor (ConceptUsecase useCase) {
            options = new Options ();
            options.ApplyButton.Click += (s1, e1) => {
                useCase.ArrangerToolbar.FullLayout(useCase.GetCurrentDisplay().Layout.Options());
            };

            var editor = new LayoutEditor ();
            editor.Dock = DockStyle.Fill;
            editor.SelectedObject = useCase.GetCurrentDisplay ().Layout;
            editor.PropertyValueChanged += (s1, e1) => {
                useCase.OnDisplayStyleChanged (s1, new EventArgs<IStyle> (null));
            };

            options.SuspendLayout ();
            options.Controls.Remove (options.OptionList);

            options.OptionChanger = editor;
            options.ContentPanel.Size = editor.Size;
            options.ContentPanel.Controls.Add (editor);

            options.ClientSize = ControlSize (options).Size;
            options.ResumeLayout (true);
            Application.DoEvents ();
            options.ClientSize = ControlSize (options).Size;
            Application.DoEvents ();

            options.Show ();
        }

        public Options ComposeStyleEditor (IEnumerable<IStyle> styles, EventHandler<EventArgs<IStyle>> styleChanged) {
            options = new Options ();
            options.ApplyButton.Visible = false;

            var editor = new StyleEditor ();
            editor.Dock = DockStyle.Fill;

            editor.PropertyValueChanged += styleChanged;

            options.OptionChanger = editor;

            options.SuspendLayout ();
            options.ContentPanel.Size = editor.Size;
            options.ContentPanel.Controls.Add (editor);

            options.OptionList.SelectedItem = null;

            options.OptionList.Items.Clear ();
            editor.Top = options.OptionList.Bottom + 1;

            var optionList = new List<IStyle> (styles);
            foreach (object o in styles) {
                options.OptionList.Items.Add (o.ToString ());
            }
            options.OptionList.SelectedIndexChanged += (s, e) => {
                if (options.OptionList.SelectedIndex != -1)
                    editor.SelectedObject = optionList[options.OptionList.SelectedIndex];
            };
            options.OptionList.SelectedIndex = 0;

            options.ClientSize = ControlSize (options).Size;

            options.ResumeLayout (true);
            Application.DoEvents ();
            options.ClientSize = ControlSize (options).Size;
            Application.DoEvents ();
            return options;

        }

        private void ShowStyleEditor (ConceptUsecase useCase) {
            var options = ComposeStyleEditor (useCase.GetCurrentDisplay ().StyleSheet.Styles, useCase.OnDisplayStyleChanged);
            options.Show ();
        }


        #endregion

        private void ExportAsImage (ConceptUsecase useCase) {
            var currentDisplay = useCase.GetCurrentDisplay ();
            if (currentDisplay != null && currentDisplay.Data != null) {
                var saveFileDialog = new FileDialogMemento {
                    DefaultExt = "tif",
                    Filter = "TIF-Image|*.tif|All Files|*.*",
                };

                if (useCase.FileDialogShow (saveFileDialog, false) == DialogResult.Ok) {
                    var image =
                        new ImageExporter (currentDisplay.Data, currentDisplay.Layout){StyleSheet=currentDisplay.StyleSheet}
                            .ExportImage ();
                    if (image != null) {
                        image.Save (saveFileDialog.FileName, ImageFormat.Tiff);
                        image.Dispose ();
                    }
                }
            }
        }

        private void Print (ConceptUsecase useCase) {
            using (PrintDialog printDialog = new PrintDialog ()) {
                var currentDisplay = useCase.GetCurrentDisplay ();
                PrintManager man = new PrintManager ();
                using (var doc = man.CreatePrintDocument (currentDisplay.Data, currentDisplay.Layout)) {
                    printDialog.Document = doc;
                    if (printDialog.ShowDialog () == System.Windows.Forms.DialogResult.OK) {
                        doc.Print ();
                    }
                    printDialog.Document = null;
                }
            }
        }

        private void PrintPreview (ConceptUsecase useCase) {
            using (PrintPreviewDialog printDialog = new PrintPreviewDialog ()) {
                var currentDisplay = useCase.GetCurrentDisplay ();
                PrintManager man = new PrintManager ();
                using (var doc = man.CreatePrintDocument (currentDisplay.Data, currentDisplay.Layout)) {
                    printDialog.Document = doc;
                    if (printDialog.ShowDialog () == System.Windows.Forms.DialogResult.OK) {
                        doc.Print ();
                    }
                    printDialog.Document = null;
                }
            }
        }
    }
}