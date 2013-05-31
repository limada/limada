/*
 * Limada 
 * 
 * This code is free software; you can redistribute it and/or modify it
 * under the terms of the GNU General Public License version 2 only, as
 * published by the Free Software Foundation.
 * 
 * Author: Lytico
 * Copyright (C) 2006-2011 Lytico
 *
 * http://www.limada.org
 * 
 */

using System;
using Limada.Model;
using Limaki.Common;
using Limaki.Data;
using Limaki.Graphs;
using System.Linq;
using System.Text;
using Limaki.Model.Content.IO;

namespace Limada.Data {
    public class Db4oThingGraphProvider : ThingGraphProvider {

        public override string Extension {
            get { return ".limo"; }
        }

        public override string Description {
            get { return "Limada ObjectBase"; }
        }

        public override bool Readable {
            get { return false; }
        }

        public override bool Saveable {
            get { return true; }
        }

        public override void Open(IoInfo fileName) {
            Close();
            var gateway = new Limaki.Data.db4o.Gateway();
            try {
                gateway.Open(fileName);

                this.Data = new Limada.Data.db4o.ThingGraph(gateway);

            } catch (Exception ex) {
                var olderVersion = ProveIfOlderVersion(gateway);
                if (olderVersion != null)
                    throw new NotSupportedException(olderVersion, ex);
                else
                    throw;
            }
        }
        
        protected virtual string ProveIfOlderVersion(Limaki.Data.db4o.Gateway gateway) {
            var repairer = new Limada.Data.db4o.Db4oRepairer();
            var clazzes = repairer.ClazzNames(gateway);
            var name = (from clazz in clazzes
                       from field in clazz.Item2
                       where field=="_writeDate"
                       select field).FirstOrDefault();
                       
            if (name != null) {
                return "Database seems created with an older version. Please import into a new database.";
            }
            return null;
        }

        public override void Open() {
            Registry.Pool.TryGetCreate<IExceptionHandler>()
                .Catch(new Exception(this.Description + " opening without filename currently not implemented"), MessageType.OK);

        }

        public override void SaveCurrent() {
            if (Data != null) {
                var graph = this.Data as Limada.Data.db4o.ThingGraph;
                if (graph!=null) {
                    graph.Flush();
                }
            }
        }

        public override void Close() {
            SaveCurrent();
            var graph = this.Data as Limada.Data.db4o.ThingGraph;
            if (graph != null) {
                graph.Close();
            }

            this.Data = null;
        }

        public override IDataProvider<IThingGraph> Clone () {
            return new Db4oThingGraphProvider() { Progress = this.Progress };
        }

        public override void RawImport (IoInfo source, IDataProvider<IThingGraph> target) {
            var repairer = new Limada.Data.db4o.Db4oRepairer { Progress = this.Progress };
            repairer.Use(source, target.Data);
        }

    }
}