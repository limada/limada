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

using System;
using System.IO;
using System.Linq;
using Limada.Model;
using Limaki.Common;
using Limaki.Data;
using Limaki.Graphs;
using Limaki.Contents.IO;

namespace Limada.IO {

    public class XmlThingGraphIo : ThingGraphIo {

        public XmlThingGraphIo (): base(new XmlThingGraphSpot()) {
            this.IoMode = Limaki.Contents.IO.IoMode.ReadWrite;
        }

        protected override ThingGraphContent OpenInternal (Iori source) {
            try {
                IThingGraph thingGraph = null;
                var fileName = source.ToFileName();
                if (File.Exists(fileName)) {
                    using (var file = new FileStream(fileName, FileMode.Open))
                        thingGraph = Open(file);
                } else {
                    thingGraph = new ThingGraph();
                }
                return new ThingGraphContent { Data = thingGraph, Source = source, ContentType = XmlThingGraphSpot.ContentType };

            } catch (Exception ex) {
                Registry.Pooled<IExceptionHandler>()
                    .Catch(new Exception("File load failed: " + ex.Message, ex), MessageType.OK);
            }
            return null;
        }

        public virtual IThingGraph Open (Stream stream) {

            var sink = new ThingGraph();

            if (stream != null && stream.Length > 0) {
                try {
                    var serializer = new ThingXmlSerializer { Graph = sink };

                    serializer.Read(stream);
                    sink.AddRange(serializer.Things);

                } catch (Exception ex) {
                    Registry.Pooled<IExceptionHandler>()
                        .Catch(new Exception("File load failed: " + ex.Message, ex), MessageType.OK);
                } finally {
                    stream.Close();
                }
            }
            return sink;
        }

        public virtual void Save (ThingGraphContent source, Iori sinkInfo) {
            if (source == null)
                return;

            using (var sink = new FileStream(sinkInfo.ToFileName(), FileMode.Create)) {

                var serializer = new ThingXmlSerializer { Graph = source.Data, Things = source.Data.Elements().ToList() };

                serializer.Write(sink);

                sink.Flush();
                sink.Close();
            }
            source.Source = sinkInfo;
        }

        public override void Close (ThingGraphContent sink) {
            Flush(sink);
        }

        public override void Flush (ThingGraphContent sink) {
            Save(sink, sink.Source as Iori);
        }
    }
}