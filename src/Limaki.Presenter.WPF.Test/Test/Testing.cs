using System.Windows.Media;
using Limaki.Common;

using Limaki.Graphs;
using Limaki.Graphs.Extensions;
using Limaki.Visuals;

using Limada.View;
using System.Diagnostics;
using System;
using System.IO;
using Limada.UseCases;
using System.Net;
using System.Threading;
using Limaki.Drawing;
using Limaki.Presenter.WPF.Display;
using Limaki.Presenter.Visuals;

namespace Limaki.WPF008 {
    public class Testing {

        public WPFVisualsDisplay CreateDisplay() {
            WPFVisualsDisplay display = new WPFVisualsDisplay();
            display.Background = new SolidColorBrush(Colors.White);
            return display;
        }



        public void GenerateTestData(WPFVisualsDisplay display) {
#if SILVERLIGHT
            WebClient client = new WebClient ();

            client.AllowReadStreamBuffering = true;

            Uri uri = new Uri(client.BaseAddress);
            string suri = uri.ToString ();
            suri = suri.Remove(suri.LastIndexOf('/'))+"/thinggraph.xml";

            string error = "File "+suri+" load failed: ";
            LoadGraphIntoDisplay(display, MessageGraph("Loading ", suri, null));

            ThreadPool.QueueUserWorkItem(
                delegate {
                try {
                    client.OpenReadCompleted += (sender, e) => {
                        if (e.Error == null) {
                            Stream file = e.Result;


                            display.Dispatcher.BeginInvoke(() => {
                                var ser = new XMLFileHandler();
                                ser.Display = display;
                                ser.Open(file);
                                ( (IControl) display ).Invalidate ();
                            });


                        } else {
                            string errmess = e.Error.Message;
                            if (e.Error.InnerException != null &&
                                errmess != e.Error.InnerException.Message) {
                                errmess += e.Error.InnerException.Message;
                            }

                            display.Dispatcher.BeginInvoke(() => {
                                LoadGraphIntoDisplay(display,
                                    MessageGraph(error, errmess, null));
                            
                            });
                        
                        }
                    };

                    client.OpenReadAsync(new Uri(suri));

                } catch (Exception ex) {
                    string exmess = ex.Message;
                    if (ex.InnerException != null)
                        exmess += ex.InnerException.Message;

                    display.Dispatcher.BeginInvoke(() => {
                        LoadGraphIntoDisplay(display, MessageGraph(error, exmess, null));
                    });

                    Registry.Pool.TryGetCreate<IExceptionHandler>()
                       .Catch(new Exception(error + ex.Message, ex));
                    
                }
                });
#else
            LoadGraphIntoDisplay(display, GenerateTestGraph ());
#endif

        }

        public VisualGraph MessageGraph(string message1, string message2,string message3) {

            IVisualFactory factory = Registry.Factory.Create<IVisualFactory>();
            VisualGraph graph = new VisualGraph();

            if (message1 != null) {
                IVisual visual = factory.CreateItem (message1);
                graph.Add (visual);

                if (message2 != null) {
                    var leaf = factory.CreateItem (message2);
                    graph.Add (visual);


                    var edge = factory.CreateEdge (visual, leaf,"");
                    graph.Add (edge);
                }
            }

            return graph;

        }

        public VisualGraph GenerateTestGraph() {
            IVisualFactory factory = Registry.Factory.Create<IVisualFactory>();
            VisualGraph graph = new VisualGraph();

            IVisual visual = factory.CreateItem("Limada");
            graph.Add(visual);


            var leaf = factory.CreateItem("on");
            graph.Add(visual);


            var edge = factory.CreateEdge(visual, leaf,"connected");
            graph.Add(edge);
#if SILVERLIGHT
            leaf = factory.CreateItem("Silverlight");
#else
            leaf = factory.CreateItem("WPF009");
#endif
            graph.Add(visual);

            edge = factory.CreateEdge(edge, leaf,"connected");
            graph.Add(edge);
            return graph;
        }

        public void LoadGraphIntoDisplay(WPFVisualsDisplay display, VisualGraph graph) {
            
            //display.Layout.StyleSheet.DefaultStyle.AutoSize =
            //    new Limaki.Toolkit.Drawing.Size (50, 50);


            var layout = (display.Display as VisualsDisplay).Layout;
            layout.Centered = true;
            layout.Orientation = Orientation.TopBottom;

            var view = 
            	new GraphView<IVisual,IVisualEdge>(graph,new VisualGraph());
            
            var fac = new GraphViewFacade<IVisual, IVisualEdge>(view);
            
            fac.Expand(graph,false);
            
            Scene scene = new Scene();
            scene.Graph = view;
            display.Display.Data = scene;
            
            display.Display.Invoke();

        }


#if FALSE//!SILVERLIGHT
        public void GetDataOverWCFService(WPFWidgetDisplay display) {
            var host = new Limaki.WCF.Client.ThingGraphClientHost();
            try {
                host.OpenClient();

                var testResult = host.ClientService.AllItems();
                foreach (var id in testResult) {
                    Debug.WriteLine(id);
                }

                var thingGraph =
                    new Limaki.WCF.Client.ClientThingGraph(host.ClientService);

                var graph = new WidgetThingGraph(new WidgetGraph(), thingGraph);

                var view =
                    new GraphView<IWidget, IEdgeWidget>(graph, new WidgetGraph());

                var fac = new GraphViewFacade<IWidget, IEdgeWidget>(view);

                fac.Expand(graph, false);

                Scene scene = new Scene();
                scene.Graph = view;
                display.Data = scene;
            } catch (Exception e) {
                Debug.WriteLine(e.Message);
            }
        }
#endif
    }


}
