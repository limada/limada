using System.IO;
using Limada.Schemata;
using Limaki.Drawing;
using Limaki.Model.Content;
using Limaki.Visuals;

namespace Limada.Usecases {

    public class DocumentSchemaDataProvider {
        public bool DocumentHasPages (IGraphScene<IVisual, IVisualEdge> scene) {
            var graph = scene.Graph;
            var document = scene.Focused;
            var documentSchemaManager = new DocumentSchemaManager();

            return documentSchemaManager.HasPages(graph, document);

        }

        public void ExportPages (string dir, IGraphScene<IVisual, IVisualEdge> scene) {
            var graph = scene.Graph;
            var document = scene.Focused;
            var documentSchemaManager = new DocumentSchemaManager();
            if (documentSchemaManager.HasPages(graph, document)) {
                int i = 0;
                foreach (var streamThing in documentSchemaManager.PageStreams(graph, document)) {
                    var pageName = i.ToString().PadLeft(5, '0');
                    if (streamThing.Description != null)
                        pageName = streamThing.Description.ToString().PadLeft(5, '0');
                    var s = scene.Focused.Data == null ? CommonSchema.NullString : scene.Focused.Data.ToString();
                    var name = dir + Path.DirectorySeparatorChar +
                               s + " " +
                               pageName +
                               ContentTypes.Extension(streamThing.StreamType);

                    streamThing.Data.Position = 0;
                    using (var fileStream = new FileStream(name, FileMode.Create)) {
                        var buff = new byte[streamThing.Data.Length];
                        streamThing.Data.Read(buff, 0, (int) streamThing.Data.Length);
                        fileStream.Write(buff, 0, (int) streamThing.Data.Length);
                        fileStream.Flush();
                        fileStream.Close();
                    }
                    streamThing.Data.Dispose();
                    streamThing.Data = null;
                }
            }
        }
    }
}