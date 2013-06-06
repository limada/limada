using Limaki.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xwt.Backends;

namespace Limaki.View {

    public class VidgetToolkit {

        public VidgetToolkitEngineBackend Backend { get; set; }
        public static VidgetToolkit CurrentEngine { get; set; }

        VidgetApplicationContext _context = null;
        public VidgetApplicationContext Context {
            get { return _context ?? (_context = new VidgetApplicationContext(this)); }
        }
    }

    public class VidgetApplicationContext {
        internal VidgetApplicationContext (VidgetToolkit toolkit) { this.Toolkit = toolkit; }
        public VidgetToolkit Toolkit { get; protected set; }
    }

    public class VidgetToolkitEngineBackend {

        Dictionary<Type, Type> backendTypes;
        Dictionary<Type, Type> backendTypesByFrontend;

        public void Initialize () {
            if (backendTypes == null) {
                backendTypes = new Dictionary<Type, Type>();
                backendTypesByFrontend = new Dictionary<Type, Type>();
                InitializeBackends();
            }
        }

        /// <summary>
        /// Initializes the widget registry used by the application.
        /// </summary>
        /// <remarks>
        /// Don't do any toolkit initialization there, do them in InitializeApplication.
        /// Override to register the backend classes, by calling RegisterBackend() methods.
        /// </remarks>
        public virtual void InitializeBackends () {}
        public IVidgetBackend CreateBackendForFrontend<T> () {
            return CreateBackendForFrontend(typeof (T));
        }

        public IVidgetBackend CreateBackendForFrontend (Type frontendType) {
            Type bt = null;
            if (!backendTypesByFrontend.TryGetValue(frontendType, out bt)) {
                var attr = (BackendTypeAttribute)Attribute.GetCustomAttribute(frontendType, typeof(BackendTypeAttribute), true);
                if (attr == null || attr.Type == null)
                    throw new InvalidOperationException("Backend type not specified for type: " + frontendType);
                if (!typeof(IVidgetBackend).IsAssignableFrom(attr.Type))
                    throw new InvalidOperationException("Backend type for frontend '" + frontendType + "' is not a IVidgetBackend implementation");
                backendTypes.TryGetValue(attr.Type, out bt);
                backendTypesByFrontend[frontendType] = bt;
            }
            if (bt == null)
                return null;
            return (IVidgetBackend)Activator.CreateInstance(bt);
        }

        internal object CreateBackend (Type backendType) {
            CheckInitialized();
            Type bt = null;

            if (!backendTypes.TryGetValue(backendType, out bt))
                return null;
            var res = Activator.CreateInstance(bt);
            if (!backendType.IsInstanceOfType(res))
                throw new InvalidOperationException("Invalid backend type. Expected '" + backendType + "' found '" + res.GetType() + "'");

            return res;
        }

        internal T CreateBackend<T> () {
            return (T)CreateBackend(typeof(T));
        }

        public void RegisterBackend<Backend, Implementation> () where Implementation : Backend {
            CheckInitialized();
            backendTypes[typeof(Backend)] = typeof(Implementation);
        }

        void CheckInitialized () {
            if (backendTypes == null)
                throw new InvalidOperationException("XWT toolkit not initialized");
        }
    }

  
}
