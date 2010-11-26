/*
 * Limaki 
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
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Forms;
using Limada.View;
using Limaki.Drawing.Imaging;
using Limaki.Graphs;
using Limaki.Model.Streams;
using Limaki.Widgets;


namespace Limaki.Presenter.Winform.DragDrop {
    public class WidgetImageDataObjectHandler: 
        IDataObjectHandler<IGraph<IWidget,IEdgeWidget>,IWidget>, IWidgetDataObjectHandler{

        public virtual string[] DataFormats {
            get { return new string[] { System.Windows.Forms.DataFormats.Dib }; }
        }

        public Type HandledType {
            get { return typeof (IWidget); }
        }

        public void SetData(IDataObject data, IGraph<IWidget, IEdgeWidget> container, IWidget value) {
            
        }

        public IWidget GetData(IDataObject data, IGraph<IWidget, IEdgeWidget> container) {
            DataObject dataObject = data as DataObject;
            Image image = null;
            
            if (dataObject != null) {
                if (dataObject.ContainsImage()) {
                    image = dataObject.GetImage();
                } else {
                    Stream stream = dataObject.GetData(DataFormats[0]) as Stream;
                    BitmapFromDibStream bmpStream = new BitmapFromDibStream(stream);
                    image = new Bitmap(bmpStream);
                }
            }

            if (image != null) {
                StreamInfo<Stream> streamInfo = new StreamInfo<Stream> ();
                streamInfo.Data = new MemoryStream();

                image.Save(streamInfo.Data, ImageFormat.Jpeg);
                streamInfo.StreamType = StreamTypes.JPG;
                streamInfo.Compression = CompressionType.neverCompress;

                image.Dispose();

                Uri uri = DragDropFacade.GetUri (data);
                if (uri != null) {
                    streamInfo.Source = uri.AbsoluteUri;
                    streamInfo.Description = uri.Segments[uri.Segments.Length-1];
                }
                return new WidgetThingStreamHelper ().CreateFromStream (container, streamInfo);

            } else {
                return null;
            }
        }


    }
}