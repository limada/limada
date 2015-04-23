using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Limaki.Playground.Text {

    public interface IMarkDownEditor {

    }

    public interface IMarkDownViewer {

    }

    public class MarkDownEdit {
        public IMarkDownEditor Editor { get; set; }
        public IMarkDownViewer Viewer { get; set; }

        public void StartEdit () {
            // hide the viewer and show the editor
        }
        public void EndEdit () {
            // show the viewer and hide the editor
        }

    }
}
