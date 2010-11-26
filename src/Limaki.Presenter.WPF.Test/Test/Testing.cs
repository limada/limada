using System.Windows.Media;
using Limaki.Common;

using Limaki.Graphs;
using Limaki.Graphs.Extensions;
using Limaki.Widgets;

using Limada.View;
using System.Diagnostics;
using System;
using System.IO;
using Limada.UseCases;
using System.Net;
using System.Threading;
using Limaki.Drawing;
using Limaki.Presenter.WPF.Display;
using Limaki.Presenter.Widgets;

namespace Limaki.WPF008 {
    public class Testing {

        public WPFWidgetDisplay CreateDisplay() {
            WPFWidgetDisplay display = new WPFWidgetDisplay();
            display.Background = new SolidColorBrush(Colors.White);
            return display;
        }



        public void GenerateTestData(WPFWidgetDisplay display) {
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

        public WidgetGraph MessageGraph(string message1, string message2,string message3) {

            IWidgetFactory factory = Registry.Factory.Create<IWidgetFactory>();
            WidgetGraph graph = new WidgetGraph();

            if (message1 != null) {
                IWidget widget = factory.CreateItem (message1);
                graph.Add (widget);

                if (message2 != null) {
                    var leaf = factory.CreateItem (message2);
                    graph.Add (widget);


                    var edge = factory.CreateEdge (widget, leaf,"");
                    graph.Add (edge);
                }
            }

            return graph;

        }

        public WidgetGraph GenerateTestGraph() {
            IWidgetFactory factory = Registry.Factory.Create<IWidgetFactory>();
            WidgetGraph graph = new WidgetGraph();

            IWidget widget = factory.CreateItem("Limada");
            graph.Add(widget);


            var leaf = factory.CreateItem("on");
            graph.Add(widget);


            var edge = factory.CreateEdge(widget, leaf,"connected");
            graph.Add(edge);
#if SILVERLIGHT
            leaf = factory.CreateWidget("Silverlight");
#else
            leaf = factory.CreateItem("WPF009");
#endif
            graph.Add(widget);

            edge = factory.CreateEdge(edge, leaf,"connected");
            graph.Add(edge);
            return graph;
        }

        public void LoadGraphIntoDisplay(WPFWidgetDisplay display, WidgetGraph graph) {
            
            //display.Layout.StyleSheet.DefaultStyle.AutoSize =
            //    new Limaki.Toolkit.Drawing.Size (50, 50);


            var layout = (display.Display as WidgetDisplay).Layout;
            layout.Centered = true;
            layout.Orientation = Orientation.TopBottom;

            var view = 
            	new GraphView<IWidget,IEdgeWidget>(graph,new WidgetGraph());
            
            var fac = new GraphViewFacade<IWidget, IEdgeWidget>(view);
            
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
