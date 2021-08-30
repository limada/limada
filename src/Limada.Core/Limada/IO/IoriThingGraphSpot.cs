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

using Limada.Model;
using Limaki.Data;
using Limaki.Contents.IO;
using System.IO;
using System.Runtime.Serialization;
using System.Xml;
using System.Xml.Serialization;
using System.Linq;
using Limaki.Contents;

namespace Limada.IO {

    public class IoriThingGraphSpot : ContentDetector {
        public IoriThingGraphSpot (): base(
            new ContentInfo[] {
                                  new ContentInfo(
                                      "Connection to Limada Things",
                                      ContentType,
                                      "limiori",
                                      "application/limiori",
                                      CompressionType.None
                                      )
                              }
            ) { }

        public static long ContentType = unchecked((long) 0x05de188316ec4301);
    }

    /// <summary>
    /// supports files containing a connection information
    /// like file, path, server etc.
    /// </summary>
    public class IoriThingGraphIo : ThingGraphIo {

        public IoriThingGraphIo (): base(new IoriThingGraphSpot()) {
            this.IoMode = Limaki.Contents.IO.IoMode.Read;
        }

        private ThingGraphIoManager _thingGraphIoManager = null;
        public ThingGraphIoManager ThingGraphIoManager {
            get {
                return _thingGraphIoManager ?? (_thingGraphIoManager =
                                                new ThingGraphIoManager {
                                                    Progress = this.Progress,
                                                    DefaultExtension = this.Detector.ContentSpecs.First ().Extension,
                                                });
            }
        }

        private const string RootElement = "Limioris";

        public virtual Iori Read (Stream source) => new Iori ().FromXmlStream (RootElement, source);

        public virtual void Write (Iori source, Stream sink) => source.ToXmlStream (RootElement, sink);

        public virtual void Write (Iori source, Iori sink) {
            using (var file = File.Open (sink.ToFileName (), FileMode.Create)) {
                Write (source, file);
            }
        }

        protected override ThingGraphContent OpenInternal (Iori source) {
            Iori sourceInfo = null;
            using (var file = File.OpenRead(source.ToFileName())) {
                sourceInfo = Read(file);
                if (sourceInfo != null) {
                    if (string.IsNullOrEmpty(sourceInfo.Path)) {
                        sourceInfo.Path = source.Path;
                    }
                }
            }
            var sinkIo = ThingGraphIoManager.GetSinkIO(sourceInfo, IoMode.Read) as ThingGraphIo;
            var sourceContent = sinkIo.Open(sourceInfo);
            
            return sourceContent;
        }

        public override void Close (ThingGraphContent sink) {
            var sinkIo = ThingGraphIoManager.GetSinkIO(sink.Source as Iori, IoMode.Write) as ThingGraphIo;
            Close(sink);
        }

        public override void Flush (ThingGraphContent sink) {
            var sinkIo = ThingGraphIoManager.GetSinkIO(sink.Source as Iori, IoMode.Write) as ThingGraphIo;
            Flush(sink);
        }
    }
}