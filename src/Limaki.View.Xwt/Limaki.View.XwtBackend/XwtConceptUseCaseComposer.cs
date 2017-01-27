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

using System;
using Limada.Usecases;
using Limaki.Common;
using Limada.UseCases;
using Limaki.View.ContentViewers;
using Limaki.View.Vidgets;
using Limaki.View.Viz.Visualizers;
using Xwt;
using Xwt.Drawing;
using Limada.View.ContentViewers;
using Limaki.Usecases;
using System.Linq;

namespace Limaki.View.XwtBackend {
   
    public class XwtConceptUseCaseComposer : IXwtBackendConceptUseCaseComposer {

		public IVindow MainWindow { get; set; }

        public Label StatusLabel { get; set; }
        public Action OnShow { get; set; }

        public Vindow AboutWindow { get; set; }

        public Action FinalizeCompose { get; set; }

        public virtual void Factor (ConceptUsecase useCase) {

            if (AboutWindow == null) { 
                AboutWindow = new Vindow ();
            }
                
            var splitViewBackend = useCase.SplitView.Backend.ToXwt();

            StatusLabel = new Label {
                HeightRequest = 20,
                Text = "starting...",
                TextColor = Colors.Black,
                TextAlignment = Alignment.Start,
            };

            var box = new VBox {
                Spacing = 2
            };

            box.PackStart(splitViewBackend, true);
            box.PackEnd(StatusLabel);

            (MainWindow.Backend as Window).Content = box;

            OnShow += () => (splitViewBackend as Paned).PositionFraction = 0.50;// WindowSize.Width / 2;
        }

        public virtual void Compose (ConceptUsecase useCase) {

            var mainWindowBackend = MainWindow.Backend as Window;
            mainWindowBackend.MainMenu = useCase.Menu;
            useCase.DataPostProcess =
               dataName => mainWindowBackend.Title = dataName + " - " + useCase.UseCaseTitle;

            useCase.MessageBoxShow = (text, title, buttons) =>
                new XwtMessageBoxShow().Show(text, title, buttons);


            useCase.Progress = (m, i, count) => {
                if (i == -1 && count == -1)
                    StatusLabel.Text = m;
                else
                    StatusLabel.Text = string.Format(m, i, count);
                Application.MainLoop.DispatchPendingEvents();
            };

            mainWindowBackend.CloseRequested += (s, e) => {
                useCase.Close ();
                useCase.Dispose ();
                Application.Exit ();
            };
            
            var viewerProvider = Registry.Pooled<ContentViewerProvider>();

            viewerProvider.Add(new SheetViewer());
            viewerProvider.Add(new ImageContentViewer());
            viewerProvider.Add (new DigidocContentViewer ());
            viewerProvider.Add(new HtmlContentViewer());

            if (PdfContentViewer.Available ())
                viewerProvider.Add (new PdfContentViewer ());
            if (TextContentViewerWithToolbar.Available ())
                viewerProvider.Add(new TextContentViewerWithToolbar());
            else
                if (TextContentViewer.Available ())
                    viewerProvider.Add (new TextContentViewer ());

            if (MarkdownContentViewer.Available ())
                viewerProvider.Add (new MarkdownContentViewer ());
            
            useCase.ShowAboutWindow = () => {
                ComposeAbout (AboutWindow);
                AboutWindow.Show ();
            };

        }

        About _about = null;
        public virtual About About { get { return _about ?? (_about = Registry.Pooled<About> ()); } }

        void ComposeAbout (IVindow window) {
            
            var backend = window.Backend as Window;
            backend.Title = $"about {About.ApplicationName}";

            VBox box = null;
            Label aboutLabel = null;

            if (backend.Content != null) {
                box = backend.Content as VBox;
                if (box != null) {
                    aboutLabel = box.Children.FirstOrDefault (c => c.Name == nameof (aboutLabel)) as Label;
                    if (aboutLabel != null) {
                        aboutLabel.Text = About.ToString ();
                    }
                }
                return;
            };

            backend.CloseRequested += (s, e)=>{
                e.AllowClose = false;
                backend.Hide ();
            };

            backend.Padding = 2;
            backend.TransientFor = MainWindow.Backend as Window;

            aboutLabel = new Label {
                Name = nameof(aboutLabel),
                Text = About.ToString(),
                TextColor = Colors.Black,
                TextAlignment = Alignment.Start,
            };

            box = new VBox {
                Spacing = 2
            };

            box.PackStart (aboutLabel);
            backend.Content = box;

        }

        [TODO]
        private void Print (ConceptUsecase useCase) {
            using (var printDialog = new PrintDialog()) {
                var currentDisplay = useCase.GetCurrentDisplay();
                var man = new PrintManager();
                using (var doc = man.CreatePrintDocument(currentDisplay.Data, currentDisplay.Layout)) {
                    printDialog.Document = doc;
                    if (printDialog.Show() == DialogResult.Ok) {
                        doc.Print();
                    }
                    printDialog.Document = null;
                }
            }
        }

        [TODO]
        private void PrintPreview (ConceptUsecase useCase) {
            throw new System.NotImplementedException();
        }

        [TODO]
        private void ShowStyleEditor (ConceptUsecase useCase) {
            var editor = new StyleEditor();
        }

        private void ShowLayoutEditor (ConceptUsecase useCase) {
            var editor = new LayoutEditor();
        }


	}
}