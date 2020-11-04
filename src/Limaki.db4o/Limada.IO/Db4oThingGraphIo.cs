/*
 * Limada 
 * 
 * This code is free software; you can redistribute it and/or modify it
 * under the terms of the GNU General Public License version 2 only, as
 * published by the Free Software Foundation.
 * 
 * Author: Lytico
 * Copyright (C) 2013 Lytico
 *
 * http://www.limada.org
 * 
 */

using Db4objects.Db4o.Ext;
using Limada.IO.db4o;
using Limada.Model;
using Limaki.Data;
using Limaki.Contents.IO;
using System;
using System.Linq;
using Limaki.Common;
using ThingGraph = Limada.IO.db4o.ThingGraph;

namespace Limada.IO {

    public class Db4oThingGraphIo : ThingGraphIo {

        public Db4oThingGraphIo (): base(new Db4oThingGraphSpot()) {
            this.IoMode = IoMode.ReadWrite;
        }

        protected override ThingGraphContent OpenInternal (Iori source) {
            var gateway = new Limaki.Data.db4o.Gateway();
            try {
                gateway.Open(source);

                var sink = new ThingGraphContent {
                    Data = new ThingGraph (gateway),
                    Source = source,
                    ContentType = Db4oThingGraphSpot.Db4oThingGraphContentType,
                };
                return sink;
            } catch (DatabaseFileLockedException ex) {
                throw;
            } catch (Exception ex) {
                if (ex.InnerException != null && ex.InnerException is DatabaseFileLockedException)
                    throw ex.InnerException;
                else {
                    var olderVersion = ProveIfOlderVersion(gateway);
                    if (olderVersion != null)
                        throw new NotSupportedException(olderVersion, ex);
                    else
                        throw;
                }
            }
        }

        public override void Close (ThingGraphContent sink) {
            Flush(sink);
            var graph = sink.Data as ThingGraph;
            if (graph != null) {
                graph.Close();
            }
        }

        public override void Flush (ThingGraphContent sink) {
            if (sink.Data != null) {
                var graph = sink.Data as ThingGraph;
                if (graph != null) {
                    graph.Flush();
                }
            }
        }

        protected virtual string ProveIfOlderVersion (Limaki.Data.db4o.Gateway gateway) {
            var repairer = new Db4oRepairer();
            var clazzes = repairer.ClazzNames(gateway);
            var name = (from clazz in clazzes
                        from field in clazz.Item2
                        where field == "_writeDate"
                        select field).FirstOrDefault();

            if (name != null) {
                return "Database seems created with an older version. Please import into a new database.";
            }
            return null;
        }

        public override Iori Use (ThingGraphContent source) {
            var result =  base.Use (source);
            var thingGraph = source.Data as ThingGraph;
            if (thingGraph !=null) {
                result = thingGraph.Gateway.Iori;
            }
            return result;
        }
    }
}