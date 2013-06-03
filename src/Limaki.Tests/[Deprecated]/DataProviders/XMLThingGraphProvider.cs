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
using System.IO;
using System.Linq;
using Limada.Data;
using Limada.Model;
using Limaki.Common;
using Limaki.Data;
using Limaki.Graphs;

namespace Limada.Data {

    public class XMLThingGraphProvider : ThingGraphProvider {

        public override string Extension {
            get { return ".limml"; }
        }

        public override string Description {
            get { return "Limada XML"; }
        }

        public override bool Saveable {
            get { return true; }
        }

        public override bool Readable {
            get { return true; }
        }

        public virtual void Open(Stream stream) {

            this.Data = new ThingGraph();

            if (stream != null && stream.Length > 0) {
                try {
                    var serializer = new ThingSerializer { Graph = this.Data };

                    serializer.Read(stream);
                    Data.AddRange(serializer.ThingCollection);
                } catch (Exception ex) {
                    Registry.Pool.TryGetCreate<IExceptionHandler>()
                        .Catch(new Exception("File load failed: " + ex.Message, ex), MessageType.OK);
                } finally {
                    stream.Close();
                }
            }

        }
        
        public override void Open() {
            this.Data = new ThingGraph ();
        }

        public override void Open(Iori fileName) {
            try {
                var file = new FileStream(Iori.ToFileName(fileName), FileMode.Open);
                Open(file);
                _lastFile = fileName;
            } catch (Exception ex) {
                Registry.Pool.TryGetCreate<IExceptionHandler>()
                    .Catch(new Exception("File load failed: " + ex.Message, ex),MessageType.OK);
            }
        }

        Iori _lastFile = null;
        public override void Save() {
            //Registry.Pool.TryGetCreate<IExceptionHandler>()
            //    .Catch(new Exception(Extension+"-Files can't be saved."),MessageType.OK);
            if (_lastFile != null)
                SaveAs (this.Data, this._lastFile);
        }

        public override void SaveAs(IThingGraph source, Iori fileName) {
            var file = new FileStream(Iori.ToFileName(fileName), FileMode.Create);

            var serializer = new ThingSerializer{Graph = source,ThingCollection = this.Data.Elements().ToList()};

            serializer.Write(file);

            file.Flush();
            file.Close();
            _lastFile = fileName;
        }

        public override void Close() {
            this._data = null;
        }

        public override void SaveCurrent() {
            Registry.Pool.TryGetCreate<IExceptionHandler>()
                .Catch(new Exception(this.Description + " saving currently not implemented"), MessageType.OK);

        }

        public override IDataProvider<IThingGraph> Clone() {
            return new XMLThingGraphProvider() { Progress = this.Progress };
        }
    }
}