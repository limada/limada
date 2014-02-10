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
using Limaki.View.Visuals.UI;
using Limaki.Visuals;
using Xwt;
using Limaki.Model.Content;
using Limaki.Common;
using Limaki.View.DragDrop;
using Limaki.Model.Content.IO;

namespace Limaki.View.DragDrop {

    public class DragDropViz {

        public virtual void LinkItem(IGraphScene<IVisual, IVisualEdge> scene, IVisual item, Point pt, int hitSize, bool itemIsRoot) {
            if (item != null) {
                var target = scene.Hovered;
                if (target == null && scene.Focused != null && scene.Focused.Shape.IsHit(pt, hitSize)) {
                    target = scene.Focused;
                }
                if (item != target) {
                    if (itemIsRoot)
                        SceneExtensions.CreateEdge(scene, item, target);
                    else
                        SceneExtensions.CreateEdge(scene, target, item);
                }
            }
        }

        TransferDataManager _transferDataManager = null;
        public virtual TransferDataManager DataManager { get { return _transferDataManager ?? (_transferDataManager = new TransferDataManager()); } }

        ContentDiggProvider _contentDiggProvider = null;
        public virtual ContentDiggProvider ContentDiggProvider { get { return _contentDiggProvider ?? (_contentDiggProvider = Registry.Pool.TryGetCreate<ContentDiggProvider>()); } }

        IVisualContentViz _visualContentViz = null;
        public IVisualContentViz VisualContentViz { get { return _visualContentViz ?? (_visualContentViz = Registry.Pool.TryGetCreate<IVisualContentViz>()); } }

        public virtual IVisual VisualOfTransferData (IGraph<IVisual, IVisualEdge> graph, ITransferData data) {
            var value = data.GetValue(TransferDataType.FromType(typeof(IVisual)));
            var bytes = value as byte[];
            if (bytes != null) {
                return TransferDataSource.DeserializeValue(bytes) as IVisual;
            }
            foreach (var s in DataManager.SinksOf(data.DataTypes)) {
                value = data.GetValue(s.Item1);
                var sink = s.Item2;
                var stream = value as Stream;
                bytes = value as byte[];
                if (bytes != null)
                    stream = new MemoryStream(bytes);
                var text = value as string;
                if (text != null)
                    stream = DataManager.AsUnicodeStream(text);
                if (stream != null) {
                    var info = sink.Use(stream);
                    if (info != null) {
                        var content = new Content<Stream> { Data = stream, ContentType = info.ContentType, Compression = info.Compression };

                        ContentDiggProvider.Use(content);

                        var result =  VisualContentViz.VisualOfRichContent(graph, content);
                       

                        return result;
                    }
                }
            }

            return null;
        }
    }
}