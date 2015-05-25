using Limada.Model;
using Limada.Schemata;
using Limaki.Graphs;
using Limaki.Tests.Graph.Model;
using System.IO;
using Limaki.Common;
using Limaki.Contents.IO;
using System.Linq;
using Limaki;

namespace Limada.Tests.Model {
    
    public class DigidocSampleFactory : SampleGraphFactory<IThing, ILink> {

        public override string Name {
            get { return "DigidocSchema Things"; }
        }

        public override IGraph<IThing, ILink> Graph {
            get {
                if (_graph == null) {
                    _graph = new SchemaThingGraph (new ThingGraph ());
                }
                return _graph;
            }
            set {
                base.Graph = value;
                // _thingGraph = value as IThingGraph;
            }
        }

        public override void Populate(IGraph<IThing, ILink> graph) {
            Nodes[1] = Factory.CreateItem<object>(null);

            Nodes[2] = Factory.CreateItem("");
            Nodes[2].Data = "Document " + Nodes[2].Id.ToString ("X");

            Edges[1] = Factory.CreateEdge(Nodes[1], Nodes[2], DigidocSchema.DocumentTitle);
            var iEdge = 2;
            var iNode = 3;
            var pageNr = 1;

            for (int i = 0; i < 3; i++) {
                Nodes[iNode] = Factory.CreateItem<Stream> (null);
                Edges[iEdge] = Factory.CreateEdge (Nodes[1], Nodes[iNode], DigidocSchema.DocumentPage);
                iNode++;
                iEdge++;
                Nodes[iNode] = Factory.CreateItem<int> (pageNr);
                Edges[iEdge] = Factory.CreateEdge (Edges[iEdge - 1], Nodes[iNode], DigidocSchema.PageNumber);
                pageNr++;
                iNode++;
                iEdge++;
            }
            AddSamplesToGraph (graph);
        }

        public void CreateDocuments (IThingGraph graph, IThing root, int count) {
            var digidoc = new DigidocSchema ();
            for (int i = 0; i < count; i++) {

                var document = digidoc.CreateDocument (graph, null);
                digidoc.CreatePage (graph, document, null, 1);
                graph.Add (Factory.CreateEdge (root, document, DigidocSchema.Document));


            }
        }

        public void ReadPagesFromDir (IThingGraph graph, IThing document, string path) {
            var digidoc = new DigidocSchema ();
            var imageStreamProvider = new ImageStreamContentIo ();
            var nr = 1;

            foreach (var file in Directory.GetFiles (path).OrderBy (f => f)) {
                if (imageStreamProvider.Detector.Supports (Path.GetExtension (file))) {
                    var stream = imageStreamProvider.ReadContent (IoUtils.UriFromFileName (file));
                    if (stream != null && stream.Data != null) {
                        digidoc.CreatePage (graph, document, stream, nr);
                        nr++;
                    }
                }
            }
        }
    }


}