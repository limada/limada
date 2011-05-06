using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Db4objects.Db4o.Config;
using Db4objects.Db4o.Reflect;
using Db4objects.Db4o.Reflect.Generic;
using Limada.Model;
using Limada.Schemata;
using Limaki.Common;
using Limaki.Data;
using Limaki.Data.db4o;
using Limaki.Model.Streams;

namespace Limada.Data.db4o {
    public class Db4oRepairer {

        public event MessageEventHandler WriteDetail = null;

        public void ReportDetail(string message) {
            if (WriteDetail != null) {
                WriteDetail(this, message);
            }
        }

        public void ReadAndSaveAs(DataBaseInfo file, DataBaseInfo newDb, bool repair) {
            IThingGraph graph = null;
            var links = new List<ILink>();
            if (repair) {
                var newFile = DataBaseInfo.ToFileName(newDb);
                if (File.Exists(newFile))
                    File.Delete(newFile);

                var p = Registry.Pool.TryGetCreate<DataProviders<IThingGraph>>().Find(newDb);
                //new Limaki.Data.
                //new Limaki.Data.dbLinq.Parts.Model1.PartsThingGraphProvider();
                p.Open(newDb);
                graph = p.Data;

                ReportClazzes((graph as Limada.Data.db4o.ThingGraph).Gateway as Gateway);
            }

            var gateway = new Gateway();
            if (!repair) {
                gateway.Configuration.AllowVersionUpdates(true);
                gateway.Configuration.DetectSchemaChanges(true);
                gateway.Configuration.RecoveryMode(true);
            }

            ConfigureAliases(gateway.Configuration);

            gateway.Open(file);
            var session = gateway.Session;
            ReportClazzes(gateway);

            var cache = new Dictionary<IReflectClass, Tuple<IReflectClass, List<IReflectField>, Type>>();
            foreach (var item in session.Query<object>()) {
                var thing = item as IThing;
                if (thing != null)
                    ReportDetail(thing.Id.ToString());
                var go = item as GenericObject;
                if (go != null) {
                    IReflectClass clazz = go.GetGenericClass();
                    Tuple<IReflectClass, List<IReflectField>, Type> defs = null;
                    if (!cache.TryGetValue(clazz, out defs)) {
                        var name = clazz.GetName();
                        name = name.Substring(0, name.LastIndexOf(','));

                        var newType = typeof(IThing).Assembly.GetType(name);

                        var fields = new List<IReflectField>();
                        var iClazz = clazz;
                        while (iClazz != null) {
                            fields.AddRange(iClazz.GetDeclaredFields());
                            iClazz = iClazz.GetSuperclass();
                        }
                        defs = Tuple.Create(clazz, fields, newType);
                        cache.Add(clazz, defs);
                    }

                    var newThing = Activator.CreateInstance(defs.Item3, new object[] { null });

                    foreach (var field in defs.Item2) {
                        var val = field.Get(go);
                        FieldInfo info = null;
                        var type = defs.Item3;
                        while (info == null && type != null) {
                            info = type.GetField(field.GetName(), BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy);
                            if (info == null)
                                info = type.GetField(field.GetName(), BindingFlags.Instance | BindingFlags.Public | BindingFlags.FlattenHierarchy);
                            if (info == null && field.GetName() == "_writeDate") {
                                info = type.GetField("_creationDate", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy);
                            }
                            type = type.BaseType;
                        }
                        if (info != null && val != null && val.GetType() == info.FieldType) {
                            info.SetValue(newThing, val);
                        }
                    }
                    ReportDetail(newThing.ToString());
                    if (repair) {
                        var link = newThing as Link;
                        if (link != null) {
                            link.GetByID = (id) => graph.GetById(id);
                            links.Add(newThing as ILink);
                        } else if (newThing is IThing) {
                            graph.Add(newThing as IThing);
                        } else if (newThing is IRealData<long>) {
                            graph.DataContainer.Add(newThing as IRealData<long>);
                        }
                    }
                }
            }
            if (repair) {
                ReportDetail("write links...");
                foreach (var link in links) {
                    if (link.Marker == null)
                        link.Marker = CommonSchema.EmptyMarker;
                    if (link.Root != null && link.Leaf != null)
                        graph.Add(link);
                }
                (graph as DbGraph<IThing, ILink>).Close();
            }
            gateway.Close();
            ReportDetail("done:\t"+(repair?DataBaseInfo.ToFileName(newDb):""));
        }

        public void ReportClazzes(Gateway gateway) {
            if (gateway == null)
                return;

            ReportDetail(gateway.DataBaseInfo.ToString());

            var session = gateway.Session;

            foreach (var clazz in session.Ext().StoredClasses()) {
                ReportDetail(clazz.GetName());
                foreach (var field in clazz.GetStoredFields())
                    ReportDetail("\t" + field.GetName());
            }
        }

        protected virtual void ConfigureAliases(IConfiguration configuration) {
            configuration.MarkTransient(typeof(Limaki.Common.TransientAttribute).FullName);
            return;
            var TypesToConfigure = new List<Type>();
            TypesToConfigure.Add(typeof(IThing));
            TypesToConfigure.Add(typeof(ILink));
            TypesToConfigure.Add(typeof(Thing));
            TypesToConfigure.Add(typeof(Thing<string>));
            TypesToConfigure.Add(typeof(Link));
            TypesToConfigure.Add(typeof(StreamThing));

            TypesToConfigure.Add(typeof(RealData));
            TypesToConfigure.Add(typeof(RealData<byte[]>));

            foreach (var type in TypesToConfigure)
                ConfigureAlias(configuration, type);
        }

        protected virtual void ConfigureAlias(IConfiguration configuration, Type type) {
            string ass = type.Assembly.FullName;
            ass = ass.Substring(0, ass.IndexOf(","));
            string namespc = type.Namespace;
            configuration.AddAlias(
                new WildcardAlias(
                    namespc + ".*, " + namespc + ".007",
                    namespc + ".*, " + ass));

            IObjectClass clazz = configuration.ObjectClass(type);
            var changeDate = clazz.ObjectField("_changeDate");

            var writeDate = clazz.ObjectField("_writeDate");
            if (writeDate != null && changeDate == null) {
                writeDate.Rename("_changeDate");
            }
        }
    }
}