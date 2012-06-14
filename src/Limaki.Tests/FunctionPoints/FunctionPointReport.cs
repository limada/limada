using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.IO;
using System.Diagnostics;

namespace Limaki.Common.Reflections {
    public class FunctionPointReport {
        #region Write
        public TextWriter Writer { get; set; }
        public void WriteLine(string message, params object[] args) {
            if (Writer != null) {
                Writer.WriteLine(string.Format(message, args));
            } else {
                Trace.WriteLine(string.Format(message, args));
            }
        }

        public void Write(string message, params object[] args) {
            if (Writer != null) {
                Writer.Write(string.Format(message, args));
            } else {
                Trace.Write(string.Format(message, args));
            }
        }

        #endregion

        public IEnumerable<Type> GenericTypesOfProperty(Type type, Type memberType) {
            if (memberType.IsGenericTypeDefinition) {
                var genType = memberType.GetGenericTypeDefinition();
                foreach (var prop in type.GetProperties()) {
                    if (prop.PropertyType.IsGenericType) {
                        var gettype = prop.PropertyType.GetGenericTypeDefinition();
                        if (gettype.Equals(genType)) {
                            foreach (var gg in prop.PropertyType.GetGenericArguments())
                                yield return gg;
                        }
                    }
                }
            }
        }

        public IEnumerable<Type> TypesInNamespace(Type nameSpaceType) {
            return TypesInNamespace(Assembly.GetAssembly(nameSpaceType), nameSpaceType.Namespace);
        }

        public IEnumerable<Type> TypesInNamespace(params Type[] nameSpaceType) {
            foreach (var p in nameSpaceType)
                foreach (var t in TypesInNamespace(p))
                    yield return t;
        }

        public IEnumerable<Type> TypesInNamespace(Assembly assembly, string nameSpace) {
            return assembly.GetTypes().Where(t => string.Equals(t.Namespace, nameSpace, StringComparison.Ordinal));
        }

        public IEnumerable<Type> ImplementInterfaces(IEnumerable<Type> interfaces, Type nameSpaceType) {
            var namespaces = TypesInNamespace(nameSpaceType);
            var resultQ =
                namespaces.SelectMany(clazz => clazz.GetInterfaces(), (clazz, implements) => new { clazz, implements })
                    .Join(interfaces, join => join.implements, interfaze => interfaze, (join, interfaze) => new { join, interfaze });

            foreach (var result in resultQ)
                yield return result.interfaze;

            foreach (var result in resultQ)
                yield return result.join.clazz;
        }

        protected ICollection<Type> Done = new HashSet<Type>();

        public int Points { get; set; }
        public bool ReportDone { get; set; }
        public bool ReportMembers { get; set; }

        public void ReportClasses(string header, FunctionPointCounter counter, IEnumerable<Type> clazzes) {
            var oldWriter = counter.Writer;
            counter.Writer = this.Writer;
            WriteLine(header);
            foreach (var clazz in clazzes.Distinct().OrderBy(t => t.Name).OrderBy(t => !t.IsInterface)) {
                if (!Done.Contains(clazz)) {
                    var count = counter.Points;
                    Done.Add(clazz);
                    counter.ReportClass(clazz);
                    Points += (counter.Points - count);
                } else if(ReportDone) {
                    WriteLine("{0}\t * see above", counter.ClassName(clazz));
                }
            }
            counter.Writer = oldWriter;
        }
    }
}