using System;
using Xwt.Backends;

namespace Xwt {

    public sealed partial class Toolkit {

        public static Toolkit Engine<T> () where T : ToolkitEngineBackend {
            if (toolkits.ContainsKey (typeof (T)))
                return toolkits[typeof (T)];
            return null;
        }

        public static Toolkit CreateToolkit<T> (bool isGuest, ToolkitType type = ToolkitType.Other) where T : ToolkitEngineBackend {
            var t = typeof (T);
            var backend = (ToolkitEngineBackend)Activator.CreateInstance (t);
            var result = WithBackend (type, backend, isGuest);
            return result;
        }

        public T CreateFrontend<T> (object ob)
        {
            return Backend.CreateFrontend<T>(ob);
        }
        
        public T CreateBackendHandler<T> () {
            return Backend.CreateBackend<T> ();
        }

        public static Toolkit WithBackend (ToolkitType type, ToolkitEngineBackend backend, bool isGuest = false) {
            var t = new Toolkit {
                toolkitType = type,
                backend = backend
            };
            t.Initialize (isGuest);
            return t;
        }
 
    }

    public static partial class Application {

        public static void Initialize (ToolkitType type, ToolkitEngineBackend backend) {
            if (engine != null)
                return;

            toolkit = Toolkit.WithBackend (type, backend, false);
            toolkit.SetActive ();
            engine = toolkit.Backend;
            mainLoop = new UILoop (toolkit);

            UIThread = System.Threading.Thread.CurrentThread;

            toolkit.EnterUserCode ();
        }
    }
}
