/*
 * Limada 
 * Version 0.081
 * 
 * This code is free software; you can redistribute it and/or modify it
 * under the terms of the GNU General Public License version 2 only, as
 * published by the Free Software Foundation.
 * 
 * Author: Lytico
 * Copyright (C) 2006-2008 Lytico
 *
 * http://limada.sourceforge.net
 * 
 */

using System;
using Limada.Data;
using Limada.Model;
using Limaki.Common;
using Limaki.Data;
using Limaki.Graphs;

namespace Limada.App {
    public class Db4oThingGraphProvider : ThingGraphProvider {
        public override string Extension {
            get { return ".limo"; }
        }

        public override string Description {
            get { return "Limada ObjectBase"; }
        }

        public override bool Saveable {
            get { return true; }
        }

        public override void Open(DataBaseInfo FileName) {
            Close();
            IGateway gateway = new Limaki.Data.db4o.Gateway();

            gateway.Open(FileName);

            this.Data = new Limada.Data.db4o.ThingGraph(gateway);
        }
        
        public override void Open() {
            throw new NotImplementedException();
        }

        public override void SaveCurrent() {
            if (Data != null) {
                if (this.Data is Limada.Data.db4o.ThingGraph) {
                    ((Limada.Data.db4o.ThingGraph)this.Data).Flush();
                }
            }
        }


        public override void Save() {
            SaveCurrent ();
        }

        public override void SaveAs(DataBaseInfo FileName) {
            var current = this.Data;
            this.Data = null;
            Open (FileName);
            ThingGraphUtils.MergeGraphs(current, this.Data);
        }

        public override void Close() {
            SaveCurrent();
            if (Data is Limada.Data.db4o.ThingGraph) {
                ((Limada.Data.db4o.ThingGraph)this.Data).Close();
            }

            this.Data = null;
        }
        
        public override IDataProvider<IThingGraph> Clone() {
            return new Db4oThingGraphProvider();
        }
    }
}