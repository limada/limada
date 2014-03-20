/*
 * Limaki 
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
using Limaki.Drawing;
using Limaki.Graphs;
using Limaki.Graphs.Extensions;
using System.Text;
using System.Linq;
using Limaki.Common.Linqish;
using Limaki.View.Visuals.UI;
using Limaki.Visuals;
using Xwt;
using Limaki.Common;
using Limaki.View.DragDrop;
using Limaki.Contents.IO;
using Limaki.Contents;
using System.Diagnostics;

namespace Limaki.View.DragDrop {

    public class DragDropViz {

        TransferDataManager _transferDataManager = null;
        public virtual TransferDataManager DataManager { get { return _transferDataManager ?? (_transferDataManager = Registry.Pooled<TransferDataManager>()); } }

        ContentDiggPool _contentDiggPool = null;
        public virtual ContentDiggPool ContentDiggPool { get { return _contentDiggPool ?? (_contentDiggPool = Registry.Pooled<ContentDiggPool>()); } }

        IVisualContentViz _visualContentViz = null;
        public IVisualContentViz VisualContentViz { get { return _visualContentViz ?? (_visualContentViz = Registry.Pooled<IVisualContentViz>()); } }

        public virtual IVisual VisualOfTransferData (IGraph<IVisual, IVisualEdge> graph, ITransferData data) {
            var value = data.GetValue(TransferDataType.FromType(typeof(IVisual)));
            var bytes = value as byte[];
            if (bytes != null) {
                return TransferDataSource.DeserializeValue(bytes) as IVisual;
            }
#if TRACE
            var dt = "";
            data.DataTypes.ForEach (d => dt += d.Id+" | ");
            Trace.WriteLine (dt);
#endif
            var dataTypes = data.DataTypes;
            Content<Stream> content = null;
            foreach (var s in DataManager.SinksOf(dataTypes)) {
                value = data.GetValue(s.Item1);
                var sink = s.Item2;
                var stream = value as Stream;
                bytes = value as byte[];
                if (bytes != null)
                    stream = new MemoryStream(bytes);
                var text = value as string;
                if (text != null)
                    stream = ByteUtils.AsUnicodeStream (text);
                if (stream != null) {
                    var info = sink.Use (stream);

                    if (info == null) {
                        info = DataManager.InfoOf (sink, dataTypes).FirstOrDefault();
                    }

                    if (info != null) {
                        bool newContent = content == null || stream != content.Data;

                        content = new Content<Stream> (content) {
                            Data = newContent ? stream : content.Data,
                            ContentType = newContent ? info.ContentType : content.ContentType,
                            Compression = newContent ? info.Compression : content.Compression,
                        };
                        

                        content = ContentDiggPool.Use (content);
                        // TODO: find a better handling of preferences; maybe MimeFingerPrints does the job?
                        if (content.Data == null && (string.IsNullOrEmpty(content.Description.ToString())))
                            continue;
                        else
                            break;
                    }
                }

            }

            if (content != null) {
                var result = VisualContentViz.VisualOfContent (graph, content);
                return result;
            }
            return null;
        }

        public virtual IVisual Paste (IGraph<IVisual, IVisualEdge> graph) {
            return null;
        }

        public virtual TransferDataSource TransferDataOfVisual (IGraph<IVisual, IVisualEdge> graph, IVisual visual) {
            if (graph == null || visual == null)
                return null;
            var result = new TransferDataSource ();
            result.AddValue<string> (visual.Data.ToString ());
            return result;
        }
    }
}