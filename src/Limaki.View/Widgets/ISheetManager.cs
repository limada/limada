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
        SheetInfo SaveToThing ( Scene scene, ILayout<Scene, IWidget> layout, SheetInfo info );
        SheetInfo SaveToThing ( Scene scene, ILayout<Scene, IWidget> layout, IThing thing, string name );
        bool IsSaveable ( Scene scene );
        void LoadSheet ( Scene scene, ILayout<Scene, IWidget> layout, Stream stream );
        SheetInfo LoadSheet(Scene scene, ILayout<Scene, IWidget> layout, StreamInfo<Stream> info);
        SheetInfo RegisterSheet ( Id id, string name );
    }

    public struct SheetInfo {
        public Id Id;
        public string Name;
        public bool Persistent;
    }
}