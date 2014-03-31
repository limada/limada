using Limada.Usecases;
using System.Reflection;
using System.Linq;
using System;
using System.IO;
using Limaki.Common.Collections;
using System.Diagnostics;

namespace Limaki.Common.IOC {

    public class AppFactory<T> where T : ContextResourceLoader {
        protected AppFactory() { }

        bool backendApplied = false;
        public AppFactory (IBackendContextResourceLoader backendContextResourceLoader) {
            Create(backendContextResourceLoader);
        }

        protected virtual void Create(IBackendContextResourceLoader backendContextResourceLoader){ 
   
            var resourceLoader = Activator.CreateInstance(typeof(T),new object[]{backendContextResourceLoader}) as T;
            Registry.ConcreteContext = resourceLoader.CreateContext();
            resourceLoader.ApplyResources(Registry.ConcreteContext);
            backendApplied = true;

            ResolveDirectory ("", "Lima*.dll");
        }

        public virtual bool TakeType (Type type) {
            // only one IBackendContextRecourceLoader allowed
            if (type.GetInterface(typeof(IBackendContextResourceLoader).FullName) != null && backendApplied)
                return false;
            return true;
        }

        public virtual void ResolveAssembly (Assembly assembly) {
            Trace.WriteLine(string.Format("loading assembly {0}", assembly.FullName));
            try {
                foreach (var type in assembly.GetTypes().Where(
                    t =>
                        t.IsClass &&
                         !t.IsAbstract &&
                         t.GetInterface(typeof(IContextResourceLoader).FullName) != null &&
                         TakeType(t))) {

                    if (type.GetConstructors().Any(tc => tc.GetParameters().Length == 0)) {
                        var loader = Activator.CreateInstance(type) as IContextResourceLoader;
                        loader.ApplyResources(Registry.ConcreteContext);
                    }

                }
            } catch (Exception ex) {
                Trace.WriteLine(string.Format("Error loading assembly {0}:{1}", assembly.FullName, ex.Message));
            }
        }

        public void ResolveDirectory(string path, string filter) {
            var assemlies = new Set<Assembly>(AppDomain.CurrentDomain.GetAssemblies ());
            path = GetFullPath (path);
            var files = Directory.GetFiles(path, filter);
            foreach(var file in files) {
                try {
                    var ass = LoadAssembly(file);
                    if (ass != null && !assemlies.Contains(ass)) {
                        ResolveAssembly(ass);
                    }
                } catch(Exception ex) {
                    Trace.WriteLine(string.Format("Error loading assembly {0}:{1}", file, ex.Message));
                }
            }
        }

        private string GetFullPath(string path) {
            if (!Path.IsPathRooted(path) && AppDomain.CurrentDomain.BaseDirectory != null) {
                path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, path);
            }

            return Path.GetFullPath(path);
        }

        private Assembly LoadAssembly(string codeBase) {
            try {
                var assemblyName = AssemblyName.GetAssemblyName(codeBase);
                return Assembly.Load(assemblyName);
            } catch  {
                return null;    
            }

        }
    }
}