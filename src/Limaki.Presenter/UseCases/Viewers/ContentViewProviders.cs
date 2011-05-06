using System;
using System.Collections.Generic;
using System.Linq;

namespace Limaki.UseCases.Viewers {
    public class ContentViewProviders  {
        public ICollection<ViewerController> Viewers = new List<ViewerController>();
        public StreamViewerController Supports(Int64 streamType) {
            return Viewers.OfType<StreamViewerController>().Where(v => v.Supports(streamType)).FirstOrDefault();
        }
        public void Add(ViewerController controller) {
            this.Viewers.Add(controller);
        }
    }
}