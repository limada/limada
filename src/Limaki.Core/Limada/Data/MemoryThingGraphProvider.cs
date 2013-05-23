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

namespace Limada.Data {

    public class MemoryThingGraphProvider:ThingGraphProvider {

        public override IThingGraph Data {
            get {
                if (base.Data == null) {
                    base.Data = new ThingGraph ();
                }
                return base.Data;
            }
            set {
                base.Data = value;
            }
        }
        public override string Extension {
            get { return "memory thing graph"; }
        }

        public override string Description {
            get { return "memory thing graph"; }
        }

        public override bool Saveable {
            get { return false; }
        }

        public override bool Readable {
            get { return false; }
        }

        public override void Open() {
            
        }

        public override void Open(IoInfo fileName) {
            Registry.Pool.TryGetCreate<IExceptionHandler>()
                .Catch(new Exception(this.Description+" opening currently not implemented"), MessageType.OK);
        }

        public override void Save() {
            Registry.Pool.TryGetCreate<IExceptionHandler>()
                .Catch(new Exception(this.Description + " not possible. Use SaveAs "), MessageType.OK);
        }

        public override void SaveAs(IThingGraph source, IoInfo fileName) {
            Registry.Pool.TryGetCreate<IExceptionHandler>()
                .Catch(new Exception(this.Description + " saving currently not implemented"), MessageType.OK);
        }

        public override void Close() {
            this.Data = null;
        }

        public override void SaveCurrent() {
            
        }

        public override IDataProvider<IThingGraph> Clone() {
            return new MemoryThingGraphProvider { Progress = this.Progress };
            
        }
    }
}