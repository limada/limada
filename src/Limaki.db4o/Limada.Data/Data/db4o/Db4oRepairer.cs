/*
 * Limada 
 * 
 * This code is free software; you can redistribute it and/or modify it
 * under the terms of the GNU General Public License version 2 only, as
 * published by the Free Software Foundation.
 * 
 * Author: Lytico
 * Copyright (C) 2010-2013 Lytico
 *
 * http://www.limada.org
 */

using Db4objects.Db4o.Config;
using Db4objects.Db4o.Reflect;
using Db4objects.Db4o.Reflect.Generic;
using Limada.Model;
using Limada.Schemata;
using Limaki.Data;
using Limaki.Data.db4o;
using Limaki.Model.Content;
using Limaki.Model.Content.IO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Id = System.Int64;

namespace Limada.Data.db4o {

    public class Db4oRepairer : SinkIo<IThingGraphRepair>, ISink<IoInfo, IThingGraph> {

        protected StringWriter Log { get; set; }
        public void ReportDetail(string message) {
            if (Progress != null) {
                Progress(message, -1, -1);
            }
            if (Log != null)
                Log.WriteLine(message);
        }

        public IThingGraph RawImport (IoInfo source, IThingGraph sink, bool repair) {
            
            this.Log = new StringWriter();
            
            var links = new List<ILink>();
            if (repair) {
                var db4oGraph = sink as Limada.Data.db4o.ThingGraph;
                if (db4oGraph != null) {
                    ReportClazzes(db4oGraph.Gateway as Gateway);
                }
            }

            var gateway = new Gateway();
            
            if (!repair) {
                gateway.Configuration.AllowVersionUpdates=true;
                gateway.Configuration.DetectSchemaChanges = true;
                //gateway.Configuration.RecoveryMode(true);
            }

            ConfigureAliases(gateway.Configuration);

            gateway.Open(source);
            var session = gateway.Session;
            
            ReportClazzes(gateway);
            SchemaFacade.InitSchemata();

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
                                info = type.GetField("_changeDate", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy);
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
                            link.GetByID = (id) => sink.GetById(id);
                            links.Add(newThing as ILink);
                        } else if (newThing is IThing) {
                            sink.Add(newThing as IThing);
                        } else if (newThing is IRealData<long>) {
                            sink.DataContainer.Add(newThing as IRealData<long>);
                        }
                    }
                }
            }
            if (repair) {
                ReportDetail("write links...");
               
                foreach (var link in links) {
                    var idLink = link as ILink<Id>;
                    if (link.Marker == null && idLink.Marker != 0) {
                        IThing marker = null;
                        if (Schema.IdentityMap.TryGetValue(idLink.Marker, out marker))
                            link.Marker = marker;
                    }
                    if (link.Marker == null) {
                       link.Marker = CommonSchema.EmptyMarker;
                    }
                    if (link.Root != null && link.Leaf != null)
                        sink.Add(link);
                }
                
            }
            gateway.Close();
            ReportDetail("done:\t");
            if (this.Log != null) {
                var logfilename = IoInfo.ToFileName(source) + ".log";
                if (File.Exists(logfilename))
                    File.Delete(logfilename);
                var logfile = new StreamWriter(logfilename);
                logfile.Write(this.Log.ToString());
                logfile.Close();
            }
            return sink;
        }

        public IEnumerable<Tuple<string, IList<string>>> ClazzNames(IGateway gateway) {
            if (gateway == null)
                yield break;

            var session = (gateway as Gateway).Session;
            foreach (var clazz in session.Ext().StoredClasses()) {
                var result = Tuple.Create(clazz.GetName(), new List<string>() as IList<string>);
                foreach (var field in clazz.GetStoredFields())
                    result.Item2.Add(field.GetName());
                yield return result;
            }

        }

        public void ReportClazzes(Gateway gateway) {
            if (gateway == null)
                return;

            ReportDetail(gateway.IoInfo.ToString());

            var session = gateway.Session;

            foreach (var clazz in session.Ext().StoredClasses()) {
                ReportDetail(clazz.GetName());
                foreach (var field in clazz.GetStoredFields())
                    ReportDetail("\t" + field.GetName());
            }
        }

        protected virtual void ConfigureAliases(ICommonConfiguration configuration) {
            configuration.MarkTransient(typeof(Limaki.Common.TransientAttribute).FullName);
          
        }

        public Db4oRepairer (): base(new Db4oThingGraphInfo()) {
            this.IoMode = InOutMode.Read;
        }

        public override bool Supports (IThingGraphRepair source) {
            return this.InfoSink.Supports(source.IoInfo.Extension);
        }

        public override ContentInfo Use (IThingGraphRepair source) {
            if (Supports(source))
                return InfoSink.SupportedContents.First();
            return null;
        }

        public override ContentInfo Use (IThingGraphRepair source, ContentInfo sink) {
            if (Supports(source))
                return SinkExtensions.Use(source, sink, s => Use(s));
            return null;
        }

        public IThingGraph Use (IoInfo source) {
            throw new ArgumentException("Repairer needs a Graph to store into");
        }

        public IThingGraph Use (IoInfo source, IThingGraph sink) {
            return this.RawImport(source, sink, true);
        }
    }
}