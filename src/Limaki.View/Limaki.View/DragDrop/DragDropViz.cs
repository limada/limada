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
using Limaki.Model.Content;
using Limaki.Common;
using Limaki.View.DragDrop;
using Limaki.Contents.IO;
using Limaki.Contents;
using System.Diagnostics;

namespace Limaki.View.DragDrop {

    public class DragDropViz {

        TransferDataManager _transferDataManager = null;
        public virtual TransferDataManager DataManager { get { return _transferDataManager ?? (_transferDataManager = Registry.Pool.TryGetCreate <TransferDataManager>()); } }

        ContentDiggPool _contentDiggPool = null;
        public virtual ContentDiggPool ContentDiggPool { get { return _contentDiggPool ?? (_contentDiggPool = Registry.Pool.TryGetCreate<ContentDiggPool>()); } }

        IVisualContentViz _visualContentViz = null;
        public IVisualContentViz VisualContentViz { get { return _visualContentViz ?? (_visualContentViz = Registry.Pool.TryGetCreate<IVisualContentViz>()); } }

        public virtual IVisual VisualOfTransferData (IGraph<IVisual, IVisualEdge> graph, ITransferData data) {
            var value = data.GetValue(TransferDataType.FromType(typeof(IVisual)));
            var bytes = value as byte[];
            if (bytes != null) {
                return TransferDataSource.DeserializeValue(bytes) as IVisual;
            }

            foreach (var s in DataManager.SinksOf(data.DataTypes)) {
#if TRACE
            var dt = "";
            data.DataTypes.ForEach (d => dt += d.Id+" | ");
            Trace.WriteLine (dt);
#endif
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
                    var info = sink.Use(stream);
                    if (info != null) {
                        var content = new Content<Stream> { Data = stream, ContentType = info.ContentType, Compression = info.Compression };

                        ContentDiggPool.Use(content);

                        var result =  VisualContentViz.VisualOfContent(graph, content);
                       

                        return result;
                    }
                }
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