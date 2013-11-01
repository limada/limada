using Xwt.Backends;
using Xwt.Drawing;
namespace Xwt.Blind.Backend {
    public static class BlindConverter {

         public static Font ToXwt (this FontData backend) {
            return CreateFrontend<Font>(backend);
        }


        public static T CreateFrontend<T> (object backend) {
            return ToolkitEngineBackend.GetToolkitBackend<BlindEngine>().CreateFrontend<T>(backend);
        }
        
    }
}