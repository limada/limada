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
using Limaki.UseCases.Winform.Viewers.ToolStrips;

namespace Limaki.UseCases.Winform {
    public class UseCaseWinformComposer : IComposer<UseCase> {
        
        public Form Mainform { get; set; }
        public ToolStripContainer ToolStripContainer { get; set; }
        public MenuStrip MenuStrip { get; set; }
        
        public WinformSplitView SplitView { get; set; }

        public DisplayToolStrip DisplayToolStrip { get; set; }
        public SplitViewToolStrip SplitViewToolStrip { get; set; }
        public MarkerToolStrip MarkerToolStrip { get; set; }
        public LayoutToolStrip LayoutToolStrip { get; set; }

        public ToolStripStatusLabel StatusLabel { get; set; }
        public StatusStrip StatusStrip { get; set; }

        ICollection<StreamViewerController> StreamViewers = new List<StreamViewerController>();

        public void Factor(UseCase useCase) {
            ToolStripContainer = new ToolStripContainer();
            

            StatusStrip = new StatusStrip();
            StatusLabel = new ToolStripStatusLabel();
            
            MenuStrip = new MenuStrip();

            SplitView = new WinformSplitView(ToolStripContainer.ContentPanel);

            DisplayToolStrip = new DisplayToolStrip ();
            SplitViewToolStrip = new SplitViewToolStrip ();
            LayoutToolStrip = new LayoutToolStrip ();
            MarkerToolStrip = new MarkerToolStrip ();

            var htmlViewer = new HTMLViewerController ();
            htmlViewer.Viewer = new HTMLViewer();
            StreamViewers.Add(htmlViewer);
            StreamViewers.Add (new ImageViewerController ());
            StreamViewers.Add (new TextViewerWithToolstripController ());
            StreamViewers.Add (new SheetViewerController ());
            
        }

        public void Compose(UseCase useCase) {
            ToolStripContainer.Dock = DockStyle.Fill;
            Mainform.Controls.Add(ToolStripContainer);

            ToolStripContainer.BottomToolStripPanel.Controls.Add(StatusStrip);
            this.StatusStrip.Items.Add(StatusLabel);


            useCase.SplitView = SplitView.View;
            useCase.SplitView.StreamViewProvider.StreamViewers = this.StreamViewers;

            useCase.DisplayToolController = DisplayToolStrip.Controller;
            useCase.LayoutToolController = LayoutToolStrip.Controller;
            useCase.MarkerToolController = MarkerToolStrip.Controller;

            SplitViewToolStrip.SplitView = useCase.SplitView;

            useCase.DataPostProcess = 
                dataName => Mainform.Text = dataName + " - " + useCase.UseCaseTitle;

            useCase.MessageBoxShow = this.MessageBoxShow;
            useCase.FileDialogShow = this.FileDialogShow;

            Mainform.FormClosing += (s, e) => useCase.Close ();
            Application.ApplicationExit += (s, e) => {
                useCase.Close();
                useCase.Dispose ();
            };
            
            InstrumentMenus(useCase);
            

            InitializeToolstripPositions ();
        }


        public void InstrumentMenus(UseCase useCase) {
			this.MenuStrip.Items.AddRange(new ToolStripMenuItem[] {
            
            new ToolStripMenuItem("File", null, new ToolStripMenuItem[] {
                new ToolStripMenuItem("Open ...", null, (s, e) => { useCase.OpenFile(); }),
                new ToolStripMenuItem("Save", null, (s, e) => { useCase.SaveFile(); }),
                new ToolStripMenuItem("SaveAs ...", null, (s, e) => { useCase.SaveAsFile(); }),
                new ToolStripMenuItem("Export", null, new ToolStripMenuItem[] {
                    new ToolStripMenuItem("current view ...", null, (s, e) => { useCase.ExportCurrentView(); }),
                    new ToolStripMenuItem("as image ...", null, (s, e) => { this.ExportAsImage(useCase); }),
                    new ToolStripMenuItem("Content...", null, (s, e) => { useCase.ExportContent(); }),
                }),
				new ToolStripMenuItem("Import", null, new ToolStripMenuItem[] { 
					new ToolStripMenuItem("Content ...", null, (s, e) => { useCase.ImportContent(); })
				}),
                new ToolStripMenuItem("Print ...", null, (s, e) => { this.Print(useCase); }),
                new ToolStripMenuItem("PrintPreview ...", null, (s, e) => { this.PrintPreview(useCase); }),
                new ToolStripMenuItem("Exit", null, (s, e) => { Application.Exit();}),
            }),

            new ToolStripMenuItem("Edit", null, new ToolStripMenuItem[] {
                new ToolStripMenuItem("Copy", null, (s, e) => {
                    var display = useCase.GetCurrentDisplay();
                    if (display != null) ((IDragDropAction)display.EventControler).Copy();
                }),
                new ToolStripMenuItem("Paste", null, (s, e) => {
                    var display = useCase.GetCurrentDisplay();
                    if (display != null) ((IDragDropAction)display.EventControler).Paste();
                }),
                new ToolStripMenuItem("Search", null, (s, e) => { useCase.Search(); }),
            }),

            new ToolStripMenuItem("Format", null, new ToolStripMenuItem[] {
                new ToolStripMenuItem("Layout", null, (s, e) => { this.ShowLayoutEditor(useCase); }),
                new ToolStripMenuItem("Style", null, (s, e) => { this.ShowStyleEditor(useCase); }),
            }),

            new ToolStripMenuItem("Favorites", null, new ToolStripMenuItem[] {
                new ToolStripMenuItem("Add to favorites", null, (s, e) 
                => { new FavoriteManager().AddToFavorites(useCase.GetCurrentDisplay().Data);}),
                new ToolStripMenuItem("View on open ", null, (s, e) 
                => { new FavoriteManager().ViewOnOpen(useCase.GetCurrentDisplay().Data);}),
            }),

            new ToolStripMenuItem("About", null, (s, e) => {
                if(About == null) About = new About();
                    About.ShowDialog();
                })
        });

		var font = SystemFonts.MenuFont;
		MenuStrip.Font = font;
		
		Action<ToolStripMenuItem> setFont = null;
		setFont = (item)=>{
				item.Font = font;
				foreach(ToolStripMenuItem sub in item.DropDownItems){
					sub.Font = font;
					setFont(sub);
				}
			};
		foreach(ToolStripMenuItem item in MenuStrip.Items)
				setFont(item);
        }

  		public void SetFont(System.Drawing.Font font, Control control){
			control.Font = font;
			foreach(Control child in control.Controls){
				SetFont(font, child);
			}
		}
		
		Form About = null;

        void InitializeToolstripPositions() {
            this.ToolStripContainer.TopToolStripPanel.SuspendLayout();

            Point location = new Point ();

            this.ToolStripContainer.TopToolStripPanel.Controls.Clear();
            this.ToolStripContainer.TopToolStripPanel.ResumeLayout(true);
            Application.DoEvents();

            this.ToolStripContainer.TopToolStripPanel.SuspendLayout();

            if (this.MenuStrip != null) {
                MenuStrip.Location = new Point ();
                this.ToolStripContainer.TopToolStripPanel.Controls.Add (MenuStrip);
                location = this.MenuStrip.Location + new Size(0, this.MenuStrip.Size.Height+3);
            }

            this.DisplayToolStrip.Location = location;
            this.ToolStripContainer.TopToolStripPanel.Controls.Add(DisplayToolStrip);
            this.ToolStripContainer.TopToolStripPanel.Controls.Add(LayoutToolStrip);
            this.ToolStripContainer.TopToolStripPanel.Controls.Add(MarkerToolStrip);
            this.ToolStripContainer.TopToolStripPanel.Controls.Add(SplitViewToolStrip);

            location = new Point(this.DisplayToolStrip.Bounds.Right+3, this.DisplayToolStrip.Bounds.Top);
            this.LayoutToolStrip.Location = location;

            location = new Point(this.LayoutToolStrip.Bounds.Right+3, this.LayoutToolStrip.Bounds.Top);
            this.MarkerToolStrip.Location = location;

            location = new Point(this.MarkerToolStrip.Bounds.Right+3, this.MarkerToolStrip.Bounds.Top);
            this.SplitViewToolStrip.Location = location;


            this.ToolStripContainer.TopToolStripPanel.ResumeLayout(false);
            this.ToolStripContainer.TopToolStripPanel.PerformLayout();


        }

        public Limaki.UseCases.Viewers.DialogResult MessageBoxShow(string text, string title, Limaki.UseCases.Viewers.MessageBoxButtons buttons) {
            return Converter.Convert(MessageBox.Show (Mainform, text, title, Converter.Convert(buttons)));
        }

        
        public Limaki.UseCases.Viewers.DialogResult FileDialogShow(FileDialogMemento value, bool open) {
            FileDialog fileDialog = null;
            if (open)
                fileDialog = new OpenFileDialog();
            else
                fileDialog = new SaveFileDialog ();
            
            Converter.FileDialogSetValue(fileDialog, value);
            Application.DoEvents();

            var result = fileDialog.ShowDialog (this.Mainform);
            
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
            options.applyButton.Click += (s1, e1) => {
                this.DisplayToolStrip.Controller.Layout();
            };

            var editor = new LayoutEditor();
            editor.Dock = DockStyle.Fill;
            editor.SelectedObject = useCase.GetCurrentDisplay ().Layout;
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

        private void ShowStyleEditor(UseCase useCase) {
            ICollection<IStyle> optionObject = useCase.GetCurrentDisplay ().StyleSheet.Styles;
            options = new Options();
            options.applyButton.Click += (s1, e1) => {
                this.DisplayToolStrip.Controller.Layout();
            };

            var editor = new StyleEditor();
            editor.Dock = DockStyle.Fill;

            editor.PropertyValueChanged += useCase.OnDisplayStyleChanged;
            
            options.OptionChanger = editor;

            options.SuspendLayout();
            options.ContentPanel.Size = editor.Size;
            options.ContentPanel.Controls.Add(editor);

            options.OptionList.SelectedItem = null;

            options.OptionList.Items.Clear();
            editor.Top = options.OptionList.Bottom + 1;

            IList<IStyle> optionList = new List<IStyle>(optionObject);
            foreach (object o in optionObject) {
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

                if (useCase.FileDialogShow(saveFileDialog,false)==UseCases.Viewers.DialogResult.OK) {
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
                var currentDisplay = useCase.GetCurrentDisplay ();
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