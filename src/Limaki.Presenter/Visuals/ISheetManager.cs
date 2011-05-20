using System;
using System.IO;
using Limaki.Common;
using Limaki.Drawing;
using Limaki.Graphs;
using Limaki.Model.Streams;
using Id = System.Int64;

namespace Limaki.Visuals{
    public interface ISheetManager {
        SheetInfo GetSheetInfo ( Int64 id );
        SheetInfo RegisterSheet(Id id, string name);
        void VisitRegisteredSheets(Action<SheetInfo> visitor);

        bool IsSaveable(IGraphScene<IVisual, IVisualEdge> scene);
        SheetInfo LoadFromStreamInfo(StreamInfo<Stream> source, IGraphScene<IVisual, IVisualEdge> target, IGraphLayout<IVisual, IVisualEdge> layout);
        SheetInfo SaveInGraph(IGraphScene<IVisual, IVisualEdge> scene, IGraphLayout<IVisual, IVisualEdge> layout, SheetInfo info);
        bool SaveStreamInGraph(Stream source, IGraph<IVisual, IVisualEdge> target, SheetInfo info);

        bool StoreInStreams(IGraphScene<IVisual, IVisualEdge> scene, IGraphLayout<IVisual, IVisualEdge> layout, Id id);
        bool LoadFromStreams(IGraphScene<IVisual, IVisualEdge> scene, IGraphLayout<IVisual, IVisualEdge> layout, Id id);
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