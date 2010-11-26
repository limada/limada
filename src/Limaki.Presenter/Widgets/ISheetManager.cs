using System;
using System.IO;
using Limada.Model;
using Limaki.Drawing;
using Limaki.Widgets;
using Id = System.Int64;
using Limaki.Model.Streams;

namespace Limaki.Widgets{
    public interface ISheetManager {
        void Clear();
        SheetInfo GetSheetInfo ( Int64 id );
        SheetInfo SaveToThing(Scene scene, IGraphLayout<IWidget, IEdgeWidget> layout, SheetInfo info);
        SheetInfo SaveToThing(Scene scene, IGraphLayout<IWidget, IEdgeWidget> layout, IThing thing, string name);
        bool IsSaveable ( Scene scene );
        void LoadSheet(Scene scene, IGraphLayout<IWidget, IEdgeWidget> layout, Stream stream);
        SheetInfo LoadSheet(Scene scene, IGraphLayout<IWidget, IEdgeWidget> layout, StreamInfo<Stream> info);
        SheetInfo RegisterSheet ( Id id, string name );
    }

    public struct SheetInfo {
        public Id Id;
        public string Name;
        public bool Persistent;
    }
}