/*
 * Limaki 
 * 
 * This code is free software; you can redistribute it and/or modify it
 * under the terms of the GNU General Public License version 2 only, as
 * published by the Free Software Foundation.
 * 
 * Author: Lytico
 * Copyright (C) 2010-2013 Lytico
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
using Limaki.Swf.Backends.Viewers;
using Limaki.Swf.Backends.Viewers.Content;
using Limaki.Swf.Backends.Viewers.ToolStrips;
using Limaki.Usecases.Concept;
using Limaki.View.Swf;
using Limaki.View.Swf.Backends;
using Limaki.View.Swf.Visuals;
using Limaki.View.Visualizers;
using Limaki.Viewers;
using Limaki.Viewers.StreamViewers;
using DialogResult = Limaki.Viewers.DialogResult;
using MessageBoxButtons = Limaki.Viewers.MessageBoxButtons;

namespace Limaki.Swf.Backends.UseCases {

    public class ConceptUseCaseComposer : IComposer<ConceptUsecase> {

        public Form Mainform { get; set; }
        public ToolStripContainer ToolStripContainer { get; set; }
        public MenuStrip MenuStrip { get; set; }

        public SwfSplitViewBackend SplitViewBackend { get; set; }

        public DisplayToolStripBackend DisplayToolStrip { get; set; }
        public SplitViewToolStripBackend SplitViewToolStripBackend { get; set; }
        public MarkerToolStripBackend MarkerToolStrip { get; set; }
        public LayoutToolStripBackend LayoutToolStripBackend { get; set; }
        public ArrangerToolStripBackend ArrangerToolStripBackend { get; set; }

        public ToolStripStatusLabel StatusLabel { get; set; }
        public StatusStrip StatusStrip { get; set; }

        public void Factor (ConceptUsecase useCase) {
            ToolStripContainer = new ToolStripContainer ();

            StatusStrip = new StatusStrip ();
            StatusLabel = new ToolStripStatusLabel ();

            MenuStrip = new MenuStrip ();

            SplitViewBackend = new SwfSplitViewBackend (ToolStripContainer.ContentPanel);

            DisplayToolStrip = new DisplayToolStripBackend ();
            SplitViewToolStripBackend = new SplitViewToolStripBackend ();
            LayoutToolStripBackend = new LayoutToolStripBackend ();
            MarkerToolStrip = new MarkerToolStripBackend ();
            ArrangerToolStripBackend = new ArrangerToolStripBackend ();

            //TODO: move this to UseCaseContextResourceLoader
            Registry.Factory.Add<ContentViewerProvider, ContentVisualViewerProvider> ();

            var viewProviders = Registry.Pool.TryGetCreate<ContentViewerProvider> ();

            viewProviders.Add (new HtmlViewer {BackendHandler = new WebBrowserBackendHandler ()});
            viewProviders.Add(new SwfDocumentSchemaViewer());
            viewProviders.Add (new ImageViewer ());
            viewProviders.Add (new TextViewerWithToolstrip ());
            viewProviders.Add (new SheetViewer ());

        }

        public void Compose (ConceptUsecase useCase) {
            ToolStripContainer.Dock = DockStyle.Fill;
            Mainform.Controls.Add (ToolStripContainer);

            ToolStripContainer.BottomToolStripPanel.Controls.Add (StatusStrip);
            this.StatusStrip.Items.Add (StatusLabel);

            useCase.SplitView = SplitViewBackend.Frontend;

            useCase.DisplayToolStrip = DisplayToolStrip.Strip;
            useCase.LayoutToolController = LayoutToolStripBackend.Frontend;
            useCase.MarkerToolStrip = MarkerToolStrip.Frontend;
            useCase.SplitViewToolStrip = SplitViewToolStripBackend.Frontend;
            useCase.ArrangerToolStrip = ArrangerToolStripBackend.Frontend;

            useCase.DataPostProcess =
                dataName => Mainform.Text = dataName + " - " + useCase.UseCaseTitle;

            useCase.MessageBoxShow = this.MessageBoxShow;
            useCase.FileDialogShow = this.FileDialogShow;
            useCase.Progress = (m, i, count) => {
                this.StatusLabel.Text = m;
                Application.DoEvents ();
            };

            Mainform.FormClosing += (s, e) => useCase.Close ();
            Application.ApplicationExit += (s, e) => {
                useCase.Close ();
                useCase.Dispose ();
            };

            useCase.ApplicationQuit = () => useCase.ApplicationQuitted = true;
            
            InstrumentMenus (useCase);

            var utils = new SwfUtils {
                //ToolStripBackground = Color.FromArgb(0xEF,0xEF,0xF2), //WhiteSmoke
                //ToolStripForeground = Color.White, // does nothing
                ToolStripItemSelectedColor = Color.White,
            };

            utils.InitializeToolstrips (
                this.ToolStripContainer.TopToolStripPanel,
                this.MenuStrip,
                new ToolStrip[] {
                    ArrangerToolStripBackend,
                    SplitViewToolStripBackend,
                    MarkerToolStrip,
                    LayoutToolStripBackend,
                    DisplayToolStrip,
                });

        }

        public void InstrumentMenus (ConceptUsecase useCase) {
            var l = new Localizer ();
            this.MenuStrip.Items.AddRange (new ToolStripMenuItem[] {
            
            new ToolStripMenuItem(l["File"], null, new ToolStripMenuItem[] {
                new ToolStripMenuItem(l["Open ..."], null, (s, e) => { useCase.OpenFile(); }),
                new ToolStripMenuItem(l["Save"], null, (s, e) => { useCase.SaveFile(); }),
                new ToolStripMenuItem(l["SaveAs ..."], null, (s, e) => { useCase.SaveAsFile(); }),
                new ToolStripMenuItem(l["Export"], null, new ToolStripMenuItem[] {
                    new ToolStripMenuItem(l["current view ..."], null, (s, e) => { useCase.ExportCurrentView(); }),
                    new ToolStripMenuItem(l["view as image ..."], null, (s, e) => { this.ExportAsImage(useCase); }),
                    new ToolStripMenuItem(l["Content ..."], null, (s, e) => { useCase.ExportContent(); }),
                    new ToolStripMenuItem(l["Report (pdf)..."], null, (s, e) => { useCase.ExportThings(); }),
                }),
				new ToolStripMenuItem(l["Import"], null, new ToolStripMenuItem[] { 
					new ToolStripMenuItem(l["Content ..."], null, (s, e) => { useCase.ImportContent(); }),
                    new ToolStripMenuItem(l["multi content ..."], null, (s, e) => { useCase.ImportGraphFocus(); }),
                    new ToolStripMenuItem(l["File from previous version ..."], null, (s, e) => { useCase.ImportThingGraphRaw(); })
				}),
                new ToolStripMenuItem(l["Print ..."], null, (s, e) => { this.Print(useCase); }),
                new ToolStripMenuItem(l["PrintPreview ..."], null, (s, e) => { this.PrintPreview(useCase); }),
                new ToolStripMenuItem(l["Exit"], null, (s, e) => { Application.Exit();}),
            }),

            new ToolStripMenuItem(l["Edit"], null, new ToolStripMenuItem[] {
                new ToolStripMenuItem(l["Copy"], null, (s, e) => {
                    var display = useCase.GetCurrentDisplay();
                    if (display != null) ((IDragDropAction)display.EventControler).Copy();
                }),
                new ToolStripMenuItem(l["Paste"], null, (s, e) => {
                    var display = useCase.GetCurrentDisplay();
                    if (display != null) ((IDragDropAction)display.EventControler).Paste();
                }),
                new ToolStripMenuItem(l["Search"], null, (s, e) => { useCase.Search(); }),
            }),

            new ToolStripMenuItem(l["Style"], null, new ToolStripMenuItem[] {
                new ToolStripMenuItem(l["Layout"], null, (s, e) => { this.ShowLayoutEditor(useCase); }),
                new ToolStripMenuItem(l["StyleSheet"], null, (s, e) => { this.ShowStyleEditor(useCase); }),
            }),

            new ToolStripMenuItem(l["Favorites"], null, new ToolStripMenuItem[] {
                new ToolStripMenuItem(l["Add to favorites"], null, (s, e) => 
                     useCase.FavoriteManager.AddToFavorites(useCase.GetCurrentDisplay().Data)),
                new ToolStripMenuItem(l["View on open "], null, (s, e) => 
                    useCase.FavoriteManager.SetAutoView(useCase.GetCurrentDisplay().Data)),
            }),

            new ToolStripMenuItem(l["About"], null, (s, e) => {
                if(About == null) About = new About();
                    About.ShowDialog();
                })
            });

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

        Form About = null;

        public DialogResult MessageBoxShow (string text, string title, MessageBoxButtons buttons) {
            return Converter.Convert (MessageBox.Show (Mainform, text, title, Converter.Convert (buttons)));
        }

        public DialogResult FileDialogShow (FileDialogMemento value, bool open) {
            FileDialog fileDialog = null;
            if (open)
                fileDialog = new OpenFileDialog ();
            else
                fileDialog = new SaveFileDialog ();

            Converter.FileDialogSetValue (fileDialog, value);
            Application.DoEvents ();

            var result = fileDialog.ShowDialog (this.Mainform);

            Application.DoEvents ();

            Converter.FileDialogSetValue (value, fileDialog);
            return Converter.Convert (result);
        }

        #region Menu - Format

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
                this.DisplayToolStrip.Strip.Layout ();
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
                    AddExtension = true,
                    ValidateNames = true,
                };

                if (useCase.FileDialogShow (saveFileDialog, false) == DialogResult.OK) {
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