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
using Limaki.Model.Content.IO;

namespace Limada.Data {

    public class XmlThingGraphIo : ThingGraphIo {

        public XmlThingGraphIo (): base(new XmlThingGraphInfo()) {
            this.IoMode = InOutMode.ReadWrite;
        }

        protected override ThingGraphContent OpenInternal (IoInfo source) {
            try {
                var file = new FileStream(IoInfo.ToFileName(source), FileMode.Open);
                var thingGraph = Open(file);
                return new ThingGraphContent { Data = new ThingGraph(), Source = source, ContentType = XmlThingGraphInfo.ContentType };

            } catch (Exception ex) {
                Registry.Pool.TryGetCreate<IExceptionHandler>()
                    .Catch(new Exception("File load failed: " + ex.Message, ex), MessageType.OK);
            }
            return null;
        }

        public virtual IThingGraph Open (Stream stream) {

            var sink = new ThingGraph();

            if (stream != null && stream.Length > 0) {
                try {
                    var serializer = new ThingSerializer { Graph = sink };

                    serializer.Read(stream);
                    sink.AddRange(serializer.ThingCollection);
                } catch (Exception ex) {
                    Registry.Pool.TryGetCreate<IExceptionHandler>()
                        .Catch(new Exception("File load failed: " + ex.Message, ex), MessageType.OK);
                } finally {
                    stream.Close();
                }
            }
            return sink;
        }

        public virtual void Save (ThingGraphContent source, IoInfo sinkInfo) {
            if (source == null)
                return;

            var sink = new FileStream(IoInfo.ToFileName(sinkInfo), FileMode.Create);

            var serializer = new ThingSerializer { Graph = source.Data, ThingCollection = source.Data.Elements().ToList() };

            serializer.Write(sink);

            sink.Flush();
            sink.Close();

            source.Source = sinkInfo;
        }

        public override void Close (ThingGraphContent sink) {
            Flush(sink);
        }

        public override void Flush (ThingGraphContent sink) {
            Save(sink, sink.Source as IoInfo);
        }
    }
}