using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;
using Limada.Presenter;
using Limaki.Common;
using Limaki.Drawing;
using Limaki.Presenter.Display;
using Limaki.Presenter.Viewers.Winform;
using Limaki.Presenter.Winform;
using Limaki.UseCases.Viewers;
using Limaki.UseCases.Viewers.StreamViewers;
using Limaki.UseCases.Winform.Viewers;
using Limaki.UseCases.Winform.Viewers.StreamViewers;
using Limaki.UseCases.Winform.Viewers.ToolStripViewers;
using Limaki.Presenter.Winform.Controls;
using Limaki.Presenter.Layout;
using Limaki.Visuals;
using System.Linq;
using LinqKit;

namespace Limaki.UseCases.Winform {
    public class WinformUseCaseComposer : IComposer<UseCase> {

        public Form Mainform { get; set; }
        public ToolStripContainer ToolStripContainer { get; set; }
        public MenuStrip MenuStrip { get; set; }

        public WinformSplitView SplitView { get; set; }

        public DisplayToolStrip DisplayToolStrip { get; set; }
        public SplitViewToolStrip SplitViewToolStrip { get; set; }
        public MarkerToolStrip MarkerToolStrip { get; set; }
        public LayoutToolStrip LayoutToolStrip { get; set; }
        public ArrangerToolStrip ArrangerToolStrip { get; set; }

        public ToolStripStatusLabel StatusLabel { get; set; }
        public StatusStrip StatusStrip { get; set; }

        public void Factor(UseCase useCase) {
            ToolStripContainer = new ToolStripContainer();


            StatusStrip = new StatusStrip();
            StatusLabel = new ToolStripStatusLabel();

            MenuStrip = new MenuStrip();

            SplitView = new WinformSplitView(ToolStripContainer.ContentPanel);

            DisplayToolStrip = new DisplayToolStrip();
            SplitViewToolStrip = new SplitViewToolStrip();
            LayoutToolStrip = new LayoutToolStrip();
            MarkerToolStrip = new MarkerToolStrip();
            ArrangerToolStrip = new ArrangerToolStrip();

            //TODO: move this to UserCaseContextResourceLoader
            Registry.Factory.Add<ContentViewProviders, ThingContentViewProviders>();

            var viewProviders = Registry.Pool.TryGetCreate<ContentViewProviders>();

            var htmlViewer = new HTMLViewerController();
            htmlViewer.Viewer = new HTMLViewer();
            viewProviders.Add(htmlViewer);
            viewProviders.Add(new ImageViewerController());
            viewProviders.Add(new TextViewerWithToolstripController());
            viewProviders.Add(new SheetViewerController());
            viewProviders.Add(new DocumentSchemaController());

        }

        public void Compose(UseCase useCase) {
            ToolStripContainer.Dock = DockStyle.Fill;
            Mainform.Controls.Add(ToolStripContainer);

            ToolStripContainer.BottomToolStripPanel.Controls.Add(StatusStrip);
            this.StatusStrip.Items.Add(StatusLabel);

            useCase.SplitView = SplitView.View;

            useCase.DisplayToolController = DisplayToolStrip.Controller;
            useCase.LayoutToolController = LayoutToolStrip.Controller;
            useCase.MarkerToolController = MarkerToolStrip.Controller;
            useCase.SplitViewToolController = SplitViewToolStrip.Controller;
            useCase.ArrangerToolController = ArrangerToolStrip.Controller;

            useCase.DataPostProcess =
                dataName => Mainform.Text = dataName + " - " + useCase.UseCaseTitle;

            useCase.MessageBoxShow = this.MessageBoxShow;
            useCase.FileDialogShow = this.FileDialogShow;
            useCase.Progress = (m,i,count) => {
                this.StatusLabel.Text = m;
                Application.DoEvents();
            };
            Mainform.FormClosing += (s, e) => useCase.Close();
            Application.ApplicationExit += (s, e) => {
                useCase.Close();
                useCase.Dispose();
            };

            InstrumentMenus(useCase);

            var utils = new WinformUtils();
            utils.InitializeToolstripPositions(
                this.ToolStripContainer.TopToolStripPanel,
                this.MenuStrip, new ToolStrip[] {
                    DisplayToolStrip,
                    MarkerToolStrip,
                    ArrangerToolStrip,
                    SplitViewToolStrip,
                    LayoutToolStrip,
                });
            
        }


        public void InstrumentMenus(UseCase useCase) {
            var l = new Localizer();
            this.MenuStrip.Items.AddRange(new ToolStripMenuItem[] {
            
            new ToolStripMenuItem(l["File"], null, new ToolStripMenuItem[] {
                new ToolStripMenuItem(l["Open ..."], null, (s, e) => { useCase.OpenFile(); }),
                new ToolStripMenuItem(l["Save"], null, (s, e) => { useCase.SaveFile(); }),
                new ToolStripMenuItem(l["SaveAs ..."], null, (s, e) => { useCase.SaveAsFile(); }),
                new ToolStripMenuItem(l["Export"], null, new ToolStripMenuItem[] {
                    new ToolStripMenuItem(l["current view ..."], null, (s, e) => { useCase.ExportCurrentView(); }),
                    new ToolStripMenuItem(l["as image ..."], null, (s, e) => { this.ExportAsImage(useCase); }),
                    new ToolStripMenuItem(l["Content ..."], null, (s, e) => { useCase.ExportContent(); }),
                    new ToolStripMenuItem(l["Report ..."], null, (s, e) => { useCase.ExportThings(); }),
                }),
				new ToolStripMenuItem(l["Import"], null, new ToolStripMenuItem[] { 
					new ToolStripMenuItem(l["Content ..."], null, (s, e) => { useCase.ImportContent(); }),
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
                    setFont(sub);
                }
            };

            foreach (ToolStripMenuItem item in MenuStrip.Items)
                setFont(item);
        }

        Form About = null;

        public Limaki.UseCases.Viewers.DialogResult MessageBoxShow(string text, string title, Limaki.UseCases.Viewers.MessageBoxButtons buttons) {
            return Converter.Convert(MessageBox.Show(Mainform, text, title, Converter.Convert(buttons)));
        }

        public Limaki.UseCases.Viewers.DialogResult FileDialogShow(FileDialogMemento value, bool open) {
            FileDialog fileDialog = null;
            if (open)
                fileDialog = new OpenFileDialog();
            else
                fileDialog = new SaveFileDialog();

            Converter.FileDialogSetValue(fileDialog, value);
            Application.DoEvents();

            var result = fileDialog.ShowDialog(this.Mainform);

            Application.DoEvents();

            Converter.FileDialogSetValue(value, fileDialog);
            return Converter.Convert(result);
        }

        #region Menu - Format

        Rectangle ControlSize(Control control) {
            Rectangle result = Rectangle.Empty;
            foreach (Control c in control.Controls) {
                result.Location = new Point(
                    Math.Min(c.Left, result.Location.X),
                    Math.Min(c.Top, result.Location.Y));
                result.Size = new Size(
                    Math.Max(c.Left + c.Width, result.Width),
                    c.Height + result.Height
                    );

            }
            return result;
        }

        Options options = null;
        private void ShowLayoutEditor(UseCase useCase) {
            options = new Options();
            options.ApplyButton.Click += (s1, e1) => {
                this.DisplayToolStrip.Controller.Layout();
            };

            var editor = new LayoutEditor();
            editor.Dock = DockStyle.Fill;
            editor.SelectedObject = useCase.GetCurrentDisplay().Layout;
            editor.PropertyValueChanged += (s1, e1) => {
                useCase.OnDisplayStyleChanged(s1, new EventArgs<IStyle>(null));
            };

            options.SuspendLayout();
            options.Controls.Remove(options.OptionList);

            options.OptionChanger = editor;
            options.ContentPanel.Size = editor.Size;
            options.ContentPanel.Controls.Add(editor);

            options.ClientSize = ControlSize(options).Size;
            options.ResumeLayout(true);
            Application.DoEvents();
            options.ClientSize = ControlSize(options).Size;
            Application.DoEvents();

            options.Show();
        }

        public Options ComposeStyleEditor(IEnumerable<IStyle> styles, EventHandler<EventArgs<IStyle>> styleChanged) {
            options = new Options();
            options.ApplyButton.Visible = false;

            var editor = new StyleEditor();
            editor.Dock = DockStyle.Fill;

            editor.PropertyValueChanged += styleChanged;

            options.OptionChanger = editor;

            options.SuspendLayout();
            options.ContentPanel.Size = editor.Size;
            options.ContentPanel.Controls.Add(editor);

            options.OptionList.SelectedItem = null;

            options.OptionList.Items.Clear();
            editor.Top = options.OptionList.Bottom + 1;

            var optionList = new List<IStyle>(styles);
            foreach (object o in styles) {
                options.OptionList.Items.Add(o.ToString());
            }
            options.OptionList.SelectedIndexChanged += (s, e) => {
                if (options.OptionList.SelectedIndex != -1)
                    editor.SelectedObject = optionList[options.OptionList.SelectedIndex];
            };
            options.OptionList.SelectedIndex = 0;

            options.ClientSize = ControlSize(options).Size;

            options.ResumeLayout(true);
            Application.DoEvents();
            options.ClientSize = ControlSize(options).Size;
            Application.DoEvents();
            return options;

        }

        private void ShowStyleEditor(UseCase useCase) {
            var options = ComposeStyleEditor(useCase.GetCurrentDisplay().StyleSheet.Styles, useCase.OnDisplayStyleChanged);
            options.Show();
        }


        #endregion

        private void ExportAsImage(UseCase useCase) {
            var currentDisplay = useCase.GetCurrentDisplay();
            if (currentDisplay != null && currentDisplay.Data != null) {
                var saveFileDialog = new FileDialogMemento {
                    DefaultExt = "tif",
                    Filter = "TIF-Image|*.tif|All Files|*.*",
                    AddExtension = true,
                    ValidateNames = true,
                };

                if (useCase.FileDialogShow(saveFileDialog, false) == UseCases.Viewers.DialogResult.OK) {
                    var image =
                        new ImageExporter(currentDisplay.Data, currentDisplay.Layout)
                            .ExportImage();
                    if (image != null) {
                        image.Save(saveFileDialog.FileName, ImageFormat.Tiff);
                        image.Dispose();
                    }
                }
            }
        }


        private void Print(UseCase useCase) {
            using (PrintDialog printDialog = new PrintDialog()) {
                var currentDisplay = useCase.GetCurrentDisplay();
                PrintManager man = new PrintManager();
                using (var doc = man.CreatePrintDocument(currentDisplay.Data, currentDisplay.Layout)) {
                    printDialog.Document = doc;
                    if (printDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK) {
                        doc.Print();
                    }
                    printDialog.Document = null;
                }
            }
        }

        private void PrintPreview(UseCase useCase) {
            using (PrintPreviewDialog printDialog = new PrintPreviewDialog()) {
                var currentDisplay = useCase.GetCurrentDisplay();
                PrintManager man = new PrintManager();
                using (var doc = man.CreatePrintDocument(currentDisplay.Data, currentDisplay.Layout)) {
                    printDialog.Document = doc;
                    if (printDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK) {
                        doc.Print();
                    }
                    printDialog.Document = null;
                }
            }
        }
    }
}