using System.IO;
using Limaki.Contents;
using Limaki.Graphs;

namespace Limaki.View.Visuals {

    public interface IVisualContentViz {
        /// <summary>
        /// creates a visual, created and assigned with content
        /// </summary>
        /// <param name="graph">can be null</param>
        /// <param name="content">can be null</param>
        /// <returns>can be null</returns>
        IVisual VisualOfContent(IGraph<IVisual, IVisualEdge> graph, Content<Stream> content);


        /// <summary>
        /// gives back the content of the
        /// visual if this is backed by a StreamThing
        /// </summary>
        /// <param name="graph"></param>
        /// <param name="visual"></param>
        /// <returns></returns>
        Content<Stream> ContentOf(IGraph<IVisual, IVisualEdge> graph, IVisual visual);

    }

    public interface IVisualContentViz<TStore>:IVisualContentViz {
        TStore AssignContent(IGraph<IVisual, IVisualEdge> graph, TStore store, Content<Stream> content);
    }
}