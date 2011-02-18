using System;
using System.IO;
using Limaki.Common;
using Limaki.Drawing;
using Limaki.Graphs;
using Limaki.Model.Streams;
using Id = System.Int64;

namespace Limaki.Widgets{
    public interface ISheetManager {
        SheetInfo GetSheetInfo ( Int64 id );
        SheetInfo RegisterSheet(Id id, string name);
        void VisitRegisteredSheets(Action<SheetInfo> visitor);

        bool IsSaveable(IGraphScene<IWidget, IEdgeWidget> scene);
        SheetInfo LoadFromStreamInfo(StreamInfo<Stream> source, IGraphScene<IWidget, IEdgeWidget> target, IGraphLayout<IWidget, IEdgeWidget> layout);
        SheetInfo SaveInGraph(IGraphScene<IWidget, IEdgeWidget> scene, IGraphLayout<IWidget, IEdgeWidget> layout, SheetInfo info);
        bool SaveStreamInGraph(Stream source, IGraph<IWidget, IEdgeWidget> target, SheetInfo info);

        bool StoreInStreams(IGraphScene<IWidget, IEdgeWidget> scene, IGraphLayout<IWidget, IEdgeWidget> layout, Id id);
        bool LoadFromStreams(IGraphScene<IWidget, IEdgeWidget> scene, IGraphLayout<IWidget, IEdgeWidget> layout, Id id);
        Stream GetFromStreams(Id id);
        
        void Clear();
    }

    public class SheetInfo {
        public Id Id;
        public string Name;
        private State _state=null;
        public State State { get { return _state ?? (_state = new State{Hollow=true}); } }
    }
}