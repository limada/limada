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

        public new IThingFactory Factory { get { return base.Factory as IThingFactory; } }

        public override void Populate(IGraph<IThing, ILink> graph) {
            Nodes[1] = Factory.CreateItem();

            Nodes[2] = Factory.CreateItem("");
            Nodes[2].Data = "Document " + Nodes[2].Id.ToString ("X");

            Edges[1] = Factory.CreateEdge(Nodes[1], Nodes[2], DigidocSchema.DocumentTitle);

            Nodes[3] = Factory.CreateItem<Stream>(null);
            Edges[2] = Factory.CreateEdge(Nodes[1], Nodes[3], DigidocSchema.DocumentPage);

            Nodes[4] = Factory.CreateItem<int>(1);
            Edges[3] = Factory.CreateEdge(Edges[2], Nodes[4], DigidocSchema.PageNumber);

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