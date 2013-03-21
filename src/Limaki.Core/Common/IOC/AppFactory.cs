using Limada.Usecases;
using Limaki.IOC;
using System.Reflection;
using System.Linq;
using System;
using System.IO;
using Limaki.Common.Collections;

namespace Limaki.Common.IOC {
    public class AppFactory<T>
    where T : ContextRecourceLoader {
        public AppFactory() { }

        bool backendApplied = false;
        public AppFactory(IBackendContextRecourceLoader backendContextRecourceLoader) {
            
            var resourceLoader = Activator.CreateInstance(typeof(T),new object[]{backendContextRecourceLoader}) as T;
            Registry.ConcreteContext = resourceLoader.CreateContext();
            resourceLoader.ApplyResources(Registry.ConcreteContext);
            backendApplied = true;

            ResolveDirectory ("", "Lima*.dll");
        }

        public void ResolveAssembly(Assembly assembly) {
            foreach (var type in assembly.GetTypes().Where(
                t => t.IsClass && 
                     ! t.IsAbstract &&
                     t.GetInterface(typeof(IContextRecourceLoader).FullName) != null)) {

                // only one IBackendContextRecourceLoader allowed:
                if (type.GetInterface(typeof(IBackendContextRecourceLoader).FullName) != null && backendApplied)
                    return;

                var loader = Activator.CreateInstance(type) as IContextRecourceLoader;
                loader.ApplyResources(Registry.ConcreteContext);

            }
        }

        public void ResolveDirectory(string path, string filter) {
            var assemlies = new Set<Assembly>(AppDomain.CurrentDomain.GetAssemblies ());
            path = GetFullPath (path);
            string[] files = Directory.GetFiles(path, filter);
            foreach(var file in files) {
                
                var ass = LoadAssembly (file);
                if (ass != null && ! assemlies.Contains(ass)) {
                    ResolveAssembly (ass);
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