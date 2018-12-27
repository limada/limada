using Xwt.Backends;
using Xwt.Drawing;

namespace Xwt.Headless.Backend {
    public static class HeadlessConverter {

         public static Font ToXwt (this FontData backend) {
            return CreateFrontend<Font>(backend);
        }


        public static T CreateFrontend<T> (object backend) {
            return ToolkitEngineBackend.GetToolkitBackend<HeadlessEngine>().CreateFrontend<T>(backend);
        }
        
    }
}