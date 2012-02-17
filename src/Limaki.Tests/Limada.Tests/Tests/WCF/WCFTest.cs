using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Limaki.Graphs;
using Limaki.Graphs.Extensions;
using Limada.Model;
using Limaki.WCF;
using System.ServiceModel;
using System.Threading;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;
using System.Diagnostics;
using Limada.Tests.Model;
using Limaki.Tests.Graph.Model;
using Limaki.WCF.Client;

namespace Limaki.Test.WCF {
    public class WCFClientTest : ThingGraphClientHost {
        protected ThingFactory Factory = new ThingFactory();

        public void Run() {
            //StartService();

            for (int i = 0; i < 1; i++) {
                Debug.WriteLine ("*** iteration " + i);
                TestClient ();
            }
            //var clientTest = new WCFTest();
            //clientTest.Source = this.Source;
            //var clientThread = new Thread(new ThreadStart(clientTest.TestClient));
            //clientThread.Start();
        }




        void ReportThing(IThing clThing) {
            if (clThing != null)
                Debug.WriteLine(clThing.ToString());
            else
                Debug.WriteLine("Thing is null");
        }

        public void TestPureClient() {
            try {
                var clThing = ClientService.GetById(0);
                ReportThing(clThing);
            } catch (Exception e) {
                Debug.WriteLine(e.Message);
            }

            Debug.WriteLine("**** Markers of Service ****");
            try {
                foreach (var id in ClientService.Markers()) {
                    var clThing = ClientService.GetById (id);
                    ReportThing(clThing);
                }
            } catch (Exception e) {
                Debug.WriteLine(e.Message);
            }

            Debug.WriteLine("**** all items of Service ****");
            try {
                foreach (var id in ClientService.AllItems()) {
                    IThing clThing = ClientService.GetById (id);
                    if (clThing != null) {
                        Debug.WriteLine(clThing.ToString());
                        foreach (ILink link in ClientService.Edges(clThing)) {
                            ReportThing(link);
                        }
                    } else {
                        Debug.WriteLine("ERROR: Thing is null");
                    }
                }
            } catch (Exception e) {
                Debug.WriteLine(e.Message);
            }  
        }

        void TestReadWriteClient(IThingGraph thingGraph) {
            var GetByIdCount = ClientService.GetByIdCount;
            

            var thing1 = Factory.CreateItem("hello service");
            thingGraph.Add(thing1);

            var thing2 = Factory.CreateItem("a child");
            thingGraph.Add(thing2);

            thingGraph.Add(Factory.CreateEdge(thing1, thing2, Factory.CreateItem("marker")));

            var graph = ModelHelper.GetSourceGraph<ProgrammingLanguageFactory>().Two;
            foreach (var thing in graph) {
                thingGraph.Add(thing);
            }
            foreach (var thing in graph.Edges()) {
                thingGraph.Add(thing);
            }
            thingGraph.Clear();

            Debug.WriteLine("**** all items of Service ****");
            try {
                int thingCount = 0;
                foreach (IThing clThing in thingGraph) {
                    ReportThing(clThing);
                    thingCount++;
                }
                Debug.WriteLine("** GetByIdCount = " + (ClientService.GetByIdCount - GetByIdCount));
                foreach (IThing clThing in thingGraph) {
                    foreach (ILink link in thingGraph.Edges(clThing)) {
                        ReportThing(link);
                    }
                }
                Debug.WriteLine("** GetByIdCount = " + (ClientService.GetByIdCount - GetByIdCount));

            } catch (Exception e) {
                Debug.WriteLine(e.Message);
            }
        }

        public void TestClient() {
            try {
                OpenClient ();
 
            } catch (Exception e) {
                Debug.WriteLine (e.Message);
            } finally {
                //client.Close ();
                //client = null;
            }
        }
    }
}