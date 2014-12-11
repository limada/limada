using System;
using System.Collections.Generic;
using System.IO;
using Xwt;
using Xwt.Drawing;

namespace Limaki.View.Vidgets {

    public interface ITextViewerBackend {

        bool ReadOnly { get; set; }
        bool Modified { get; set; }
        Point AutoScrollOffset { get; set; }

        void Save (Stream stream, TextViewerTextType textType);
        void Load (Stream stream, TextViewerTextType textType);

        void Clear ();

        void SetAttribute (TextAttribute attribute);

        IEnumerable<TextAttribute> GetAttributes ();

        event EventHandler SelectionChanged;
    }
}