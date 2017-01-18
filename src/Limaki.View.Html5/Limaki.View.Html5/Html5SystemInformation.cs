using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xwt;

namespace Limaki.View.Html5 {
    
    public class Html5SystemInformation : IUISystemInformation {
        public Size DragSize {
            get { return new Size(1,1); }
        }

        public int DoubleClickTime {
            get { return 2; }
        }

        public int VerticalScrollBarWidth {
            get { return 10; }
        }

        public int HorizontalScrollBarHeight {
            get { return 10; }
        }

        public IEnumerable<string> ImageFormats {
            get {
                yield break;
            }
        }
    }
}
