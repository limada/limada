using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Limaki.View.Headless.VidgetBackends {
    public class HeadlessCursorHandlerBackend:ICursorHandler {
        private DisplayBackend Backend;

        public HeadlessCursorHandlerBackend (DisplayBackend Backend) {
            this.Backend = Backend;
        }

        public void SetCursor (Drawing.Anchor anchor, bool hasHit) {

        }

        public void SetEdgeCursor (Drawing.Anchor anchor) {

        }

        public void SaveCursor () {

        }

        public void RestoreCursor () {

        }
    }
}
