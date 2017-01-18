/*
 * Limaki 
 * 
 * This code is free software; you can redistribute it and/or modify it
 * under the terms of the GNU General Public License version 2 only, as
 * published by the Free Software Foundation.
 * 
 * Author: Lytico
 * Copyright (C) 2010-2014 Lytico
 *
 * http://www.limada.org
 * 
 */

using Limada.Usecases;
using System.Reflection;
using System.Linq;
using System;
using System.Collections.Generic;
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


			ResolveAssemblys (AppDomain.CurrentDomain.GetAssemblies (), "Lima");
            var loc = new Uri (this.GetType ().Assembly.EscapedCodeBase).LocalPath;
            loc = Path.GetDirectoryName (loc);
            ResolveDirectory (loc, "Lima*.dll");
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
                Trace.WriteLine(string.Format("Error resolving assemblys {0}:{1}", assembly.FullName, ex.Message));
            }
        }

		Set<Assembly> _assemblies = null; 

		Set<Assembly> assemblies {
			get { return _assemblies ?? (_assemblies = new Set<Assembly> ()); }
		} 

		public virtual void ResolveAssemblys(IEnumerable<Assembly> currDomain, string filter) {

			foreach (var ass in currDomain) {
				try {
					if (ass != null && ass.GetName ().Name.StartsWith (filter) && !assemblies.Contains (ass)) {
						ResolveAssembly (ass);
						assemblies.Add (ass);
						var refasses = ass.GetReferencedAssemblies()
							.Select(a => Assembly.Load (a.FullName))
							.ToArray();

						ResolveAssemblys (refasses, filter);
					}
				} catch (Exception ex) {
					Trace.WriteLine (string.Format ("Error loading assembly {0}:{1}", ass, ex.Message));
				}
			}
		}

        public virtual void ResolveDirectory(string path, string filter) {
            path = GetFullPath (path);
            var files = Directory.GetFiles(path, filter);
            foreach(var file in files) {
                try {
                    var ass = LoadAssembly(file);
                    if (ass != null && !assemblies.Contains(ass)) {
                        ResolveAssembly(ass);
						assemblies.Add(ass);
                    }
                } catch(Exception ex) {
                    Trace.WriteLine(string.Format("Error loading assembly {0}:{1}", file, ex.Message));
                }
            }
        }

		protected string GetFullPath(string path) {
            if (!Path.IsPathRooted(path) && AppDomain.CurrentDomain.BaseDirectory != null) {
                path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, path);
            }

            return Path.GetFullPath(path);
        }

		protected Assembly LoadAssembly(string codeBase) {
            try {
                var assemblyName = AssemblyName.GetAssemblyName(codeBase);
                return Assembly.Load(assemblyName);
            } catch (Exception ex) {
                Trace.WriteLine(string.Format("Error loading assembly {0}:{1}", codeBase, ex.Message));
                return null;    
            }

        }
    }
}