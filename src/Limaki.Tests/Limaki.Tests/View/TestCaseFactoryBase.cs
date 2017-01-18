using System;
using System.Text;
using Limada.Schemata;
using Limada.UseCases;
using Limada.View.VisualThings;
using Limaki.Common;
using Limaki.Common.Linqish;
using Limaki.Tests.View;
using Limaki.Tests.View.Display;
using Limaki.Usecases;
using Limaki.Usecases.Vidgets;
using Limaki.View;
using Limaki.View.Vidgets;
using Limaki.View.Visuals;
using Limaki.View.Viz;
using Limaki.View.Viz.Mesh;
using Limaki.View.Viz.Rendering;
using Limaki.View.Viz.Visuals;
using Xwt;

namespace Limaki.Tests.UseCases {

    public class TestCaseFactoryBase : UsecaseFactory<ConceptUsecase> {

        public Func<BenchmarkOneTests> DisplayTest { get; protected set; }
        public MessageEventHandler TestMessage { get; protected set; }

        public override void Compose (ConceptUsecase useCase) {

            this.TestMessage = (s, m) => useCase.Progress (m, -1, -1);

            this.DisplayTest = () => {
                var test = new BenchmarkOneTests ();
                var testVidget = new VisualsDisplayTest {
                    CreateDisplay = () => new VisualsDisplay ()
                };

                test.WriteDetail += TestMessage;
                test.TestWindow = useCase.MainWindow;
                test.Display = useCase.SplitView.Display1;
                test.Setup ();

                return test;
            };
        }

        public void ShowQuadTree (IGraphScene<IVisual, IVisualEdge> scene) {
            var vindow = new Vindow { Size = new Size (800,600) };
            var display = new VisualsDisplay();
            vindow.Content = display;

            var quadTreeVisualizer = new QuadTreeVisualizer();
            quadTreeVisualizer.VisualsDisplay = display;
            quadTreeVisualizer.Data = (scene.SpatialIndex as VisualsQuadTreeIndex).GeoIndex;

            vindow.Show();
        }

        public void ShowFileExplorer (ConceptUsecase useCase) {
            var vindow = new Vindow { Size = new Size (800, 600) };
            var fileDisplay = new VisualsDisplay ();
            vindow.Content = fileDisplay;

            var fileExplorer = new FileExplorerComposer ();
            fileExplorer.ComposeDisplay (fileDisplay);
            fileExplorer.FileSelected = f => {
                
            };
            var docFolder = Environment.GetFolderPath (Environment.SpecialFolder.Personal);
            fileExplorer.FileFilter = "*.limo";
            fileExplorer.ShowCurrent = true;
            fileExplorer.ShowDir (docFolder);

            fileDisplay.DataChanged ();

            vindow.Show ();
        }

        public void ShowClipboardContent (ConceptUsecase useCase) {
            var vindow = new Vindow { Size = new Size (800, 600) };
            var textbox = new PlainTextBox ();
            vindow.Content = textbox;
            var cbcontent = new StringBuilder ();
            var data = Clipboard.GetTransferData (Clipboard.GetTypesAvailable ());
            foreach (var f in data.DataTypes){
                cbcontent.AppendLine ();
                cbcontent.AppendLine (f.Id);
                var ct = data.GetValue (f);
                var text = ct as string;
                if (text != null) {
                    cbcontent.AppendLine (text);
                    continue;
                } 
                var bytes = ct as byte [];
                if (bytes != null) {
                    for (var i = 0; i < bytes.Length; i++) {
                        if (i % (4 * 8)==0) {
                            cbcontent.AppendLine ();
                        }
                        cbcontent.Append ($"{bytes[i]:N0} ");
                    }
                    continue;
                }
                cbcontent.AppendLine ($"{ct!=null?ct.GetType():null}");
            }
            textbox.Text = cbcontent.ToString ();
            vindow.Show ();
        }

        public void WCFServiceTest (ConceptUsecase usecase) {
#if WCF
            DataBaseInfo info = new DataBaseInfo();
            info.Server = "http://localhost";
            info.Port = 8000;
            info.Path = "Limada";
            info.Name = "ThingGraphService";
            var handler = new SceneProvider();
            var provider = new WCFThingGraphClientProvider();

            handler.Provider = provider;
            handler.DataBound = usecase.GraphSceneUiManager.DataBound;
            if (handler.Open(info)) {
                usecase.DataPostProcess (provider.host.baseAddress.AbsoluteUri);
            }
#endif
        }

        public void InstrumentLayer (IRenderAction renderAction, IGraphSceneDisplay<IVisual, IVisualEdge> display) {
            var layer = renderAction as ILayer<IGraphScene<IVisual, IVisualEdge>>;
            if (layer != null) {
                layer.Data = () => display.Data;
                layer.Camera = () => display.Viewport.Camera;
            }
            var graphLayer = layer as GraphSceneLayer<IVisual, IVisualEdge>;
            if (graphLayer != null) {
                graphLayer.Layout = () => display.Layout;

            }
            display.ActionDispatcher.Add (renderAction);
        }

        public void NoSchemaThingGraph (ConceptUsecase useCase) {
            var display = useCase.GetCurrentDisplay();
            var thingGraph = display.Data.Graph.ThingGraph();
            var schemaGraph = thingGraph as SchemaThingGraph;
            if (schemaGraph != null) {
                schemaGraph.EdgeFilter = e => true;
                schemaGraph.ItemFilter = e => true;
            }
        }

        protected virtual void SetExampleScene (ConceptUsecase useCase, IGraphScene<IVisual, IVisualEdge> scene) {
            useCase.Close ();

            var mesh = Registry.Pooled<IGraphSceneDisplayMesh<IVisual, IVisualEdge>> ();
            mesh.AddScene (scene);

            mesh.Displays.ForEach (d => {
                var s2 = mesh.CreateSinkScene (scene.Graph);
                mesh.AddScene (s2);
                d.Data = s2;
            });
            useCase.SplitView.SetScene (scene, nameof (ISplitView.Display1).ToLower ());
            useCase.FavoriteManager.GoHome (useCase.SplitView.Display1, true);
        }

        public void CurrentProblem (ConceptUsecase usecase) {
            try {

                //var test = new WebProxyTest ();
                //test.TestInfinitLoopIfHtmlContentIsFocused (usecase.GetCurrentDisplay ());

                var test = new UsecaseSerializerTest ();
                test.WriteUsecase (usecase);

            } catch (Exception e) {
                Registry.Pooled<IExceptionHandler> ().Catch (e, MessageType.OK);
            } finally {

            }
        }

  
    }
}