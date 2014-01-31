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

using Limaki.Common;
using Limaki.Usecases.Concept;
using Limaki.View.Visualizers;
using Xwt;
using Xwt.Drawing;


namespace Limaki.View.XwtBackend {

    public class XwtConceptUseCaseComposer : IComposer<ConceptUsecase> {

        public Window MainWindow { get; set; }
        public Menu Menu { get; set; }

        public void Factor (ConceptUsecase useCase) {
            MainWindow.MainMenu = CreateMenu(useCase);

            this.Menu = MainWindow.MainMenu;
           
            var splitViewBackend = useCase.SplitView.Backend as Widget;

            StatusLabel = new Label {
                //HorizontalPlacement = WidgetPlacement.End,
                //VerticalPlacement = WidgetPlacement.Fill,
                HeightRequest = 20,
                Text = "hello",
                TextColor = Colors.Black,
            };
            var box = new VBox {
                //VerticalPlacement = WidgetPlacement.Fill,
                //HorizontalPlacement = WidgetPlacement.Fill
                
            };

            box.PackStart(splitViewBackend,true);
            box.PackEnd(StatusLabel);

            MainWindow.Content = box;

           
 
        }

       

        public void Compose (ConceptUsecase useCase) {

            useCase.DataPostProcess =
               dataName => MainWindow.Title = dataName + " - " + useCase.UseCaseTitle;
            useCase.MessageBoxShow = (text, title, buttons) => 
                new XwtMessageBoxShow().Show(text, title, buttons);


            useCase.Progress = (m, i, count) => {
                if (i == -1 && count == -1)
                    StatusLabel.Text = m;
                else
                    StatusLabel.Text = string.Format(m, i, count);
                Application.MainLoop.DispatchPendingEvents();
            };

            MainWindow.CloseRequested += (s, e) => {
                e.AllowClose = MessageDialog.Confirm("Close?", Command.Ok);
                if (e.AllowClose) {
                    useCase.Close();
                    Application.Exit();
                }
            };

        }

        private Menu CreateMenu (ConceptUsecase useCase) {
            var menu = new Menu();

            var l = new Localizer();
            menu.AddItems(

            new MenuItem(l["File"], null, null,new MenuItem[] {
                new MenuItem(l["Open ..."], null, (s, e) => { useCase.OpenFile(); }),
                new MenuItem(l["Save"], null, (s, e) => { useCase.SaveFile(); }),
                new MenuItem(l["SaveAs ..."], null, (s, e) => { useCase.SaveAsFile(); }),
                new MenuItem(l["Export"], null,null, new MenuItem[] {
                    new MenuItem(l["current view ..."], null, (s, e) => { useCase.ExportCurrentView(); }),
                    new MenuItem(l["view as image ..."], null, (s, e) => { this.ExportAsImage(useCase); }),
                    new MenuItem(l["Content ..."], null, (s, e) => { useCase.ExportContent(); }),
                    new MenuItem(l["Report (pdf)..."], null, (s, e) => { useCase.ExportThings(); }),
                }),
				new MenuItem(l["Import"], null, null,new MenuItem[] { 
					new MenuItem(l["Content ..."], null, (s, e) => { useCase.ImportContent(); }),
                    new MenuItem(l["multi content ..."], null, (s, e) => { useCase.ImportGraphFocus(); }),
                    new MenuItem(l["File from previous version ..."], null, (s, e) => { useCase.ImportThingGraphRaw(); })
				}),
                new MenuItem(l["Print ..."], null, (s, e) => { this.Print(useCase); }),
                new MenuItem(l["PrintPreview ..."], null, (s, e) => { this.PrintPreview(useCase); }),
                new MenuItem(l["Exit"], null, (s, e) => { Application.Exit();}),
            }),

            new MenuItem(l["Edit"], null,null, new MenuItem[] {
                new MenuItem(l["Copy"], null, (s, e) => {
                    var display = useCase.GetCurrentDisplay();
                    if (display != null) display.EventControler.Copy();
                }),
                new MenuItem(l["Paste"], null, (s, e) => {
                    var display = useCase.GetCurrentDisplay();
                    if (display != null) display.EventControler.Paste();
                }),
                new MenuItem(l["Search"], null, (s, e) => { useCase.Search(); }),
            }),

            new MenuItem(l["Style"], null, null,new MenuItem[] {
                new MenuItem(l["Layout"], null, (s, e) => { this.ShowLayoutEditor(useCase); }),
                new MenuItem(l["StyleSheet"], null, (s, e) => { this.ShowStyleEditor(useCase); }),
            }),

            new MenuItem(l["Favorites"], null, null,new MenuItem[] {
                new MenuItem(l["Add to favorites"], null, (s, e) => 
                     useCase.FavoriteManager.AddToFavorites(useCase.GetCurrentDisplay().Data)),
                new MenuItem(l["View on open "], null, (s, e) => 
                    useCase.FavoriteManager.SetAutoView(useCase.GetCurrentDisplay().Data)),
            }),

            new MenuItem(l["About"], null, (s, e) => {
                if (About == null) About = new About();
                About.Show();
            })



                );
            return menu;
        }

        private void ExportAsImage (ConceptUsecase useCase) {
            throw new System.NotImplementedException();
        }

        private void Print (ConceptUsecase useCase) {
            throw new System.NotImplementedException();
        }

        private void PrintPreview (ConceptUsecase useCase) {
            throw new System.NotImplementedException();
        }

        private void ShowStyleEditor (ConceptUsecase useCase) {
            
        }

        private void ShowLayoutEditor (ConceptUsecase useCase) {
            
        }

        About About { get; set; }
        public Label StatusLabel { get; set; }
    }
}