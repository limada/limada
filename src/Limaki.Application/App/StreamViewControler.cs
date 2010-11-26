/*
 * Limada 
 * Version 0.08
 * 
 * This code is free software; you can redistribute it and/or modify it
 * under the terms of the GNU General Public License version 2 only, as
 * published by the Free Software Foundation.
 * 
 * Author: Lytico
 * Copyright (C) 2006-2008 Lytico
 *
 * http://limada.sourceforge.net
 */


using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using Limada.Model;
using Limada.View;
using Limaki.Common;
using Limaki.Common.Collections;
using Limaki.Drawing;
using Limaki.Graphs.Extensions;
using Limaki.Model.Streams;
using Limaki.Widgets;
using Limaki.Winform.Controls;
using Limaki.Winform.Displays;
using Id = System.Int64;

namespace Limaki.App {
    public class StreamViewControler:IDisposable {
        ImageDisplay imageDisplay = null;
        Control htmlControl = null;
        RichTextBox textControl = null;
        public WidgetDisplay sheetControl = null;
        public ISheetManager sheetManager = null;

        public System.Drawing.Color BackColor = SystemColors.Control;
        public Control StreamControl = null;
        public Control Parent = null;


        private ICollection<Id> _imageStreamTypes = null;
        public ICollection<Id> ImageStreamTypes {
            get {
                if (_imageStreamTypes==null) {
                    _imageStreamTypes = new Set<Id>();
                    _imageStreamTypes.Add(StreamTypes.TIF);
                    _imageStreamTypes.Add(StreamTypes.JPG);
                    _imageStreamTypes.Add(StreamTypes.GIF);
                    _imageStreamTypes.Add(StreamTypes.PNG);
                    _imageStreamTypes.Add(StreamTypes.BMP);
                    _imageStreamTypes.Add(StreamTypes.EMF);
                    _imageStreamTypes.Add(StreamTypes.WMF);
                }
                return _imageStreamTypes;
            }
            set { _imageStreamTypes = value; }
        }

        IStreamThing currentThing = null;

        void findControlForStreamThing(IStreamThing thing) {
            if (thing != null) {
                try {
                    if (ImageStreamTypes.Contains(thing.StreamType)){
                        thing.DeCompress();
                        if (imageDisplay == null) {
                            imageDisplay = new ImageDisplay();
                        }

                        imageDisplay.SelectAction.Enabled = false;
                        imageDisplay.ZoomAction.Enabled = true;
                        imageDisplay.ScrollAction.Enabled = true;
                        imageDisplay.BackColor = this.BackColor;
                        
                        imageDisplay.Data = Image.FromStream(thing.Data);
                        thing.ClearRealSubject();

                        imageDisplay.ZoomState = ZoomState.FitToScreen;
                        

                        DoShowStreamControl(imageDisplay);
                        currentThing = thing;

                    } else if (thing.StreamType == StreamTypes.HTML) {
                        thing.DeCompress();
                        if (htmlControl == null) {
//                            if (Commons.Mono) {
                                htmlControl = new Limaki.Winform.Controls.WebBrowser();
                                htmlControl.Parent = this.Parent;
//                            } else {
//                                htmlControl = new Limaki.ThirdPartyWrappers.GeckoWebBrowser ();
//                                htmlControl.Parent = this.Parent;
//                                Thread.Sleep (0);
//                            }
                        }

                        using (StreamReader reader = new StreamReader(thing.Data)) {
                            ((IWebBrowser)htmlControl).DocumentText = reader.ReadToEnd();
                            thing.ClearRealSubject();
                        }

                        DoShowStreamControl(htmlControl);
                        currentThing = thing;
                        
                    } else if (thing.StreamType == StreamTypes.RTF || thing.StreamType == StreamTypes.ASCII) {
                        thing.DeCompress();
                        if (textControl == null) {
                            textControl = new RicherTextBox();
                            textControl.Multiline = true;
                            textControl.BorderStyle = BorderStyle.None;
                            textControl.EnableAutoDragDrop = true;
                        }
                        using (StreamReader reader = new StreamReader(thing.Data)) {
                            textControl.Rtf = reader.ReadToEnd();
                            thing.ClearRealSubject();
                        }
                        DoShowStreamControl(textControl);
                        currentThing = thing;
                        
                    } else if (thing.StreamType == Sheet.StreamType) {
                        if (sheetControl == null) {
                            throw new ArgumentException ("sheetControl must not be null");
                        }

                        sheetControl.CommandsExecute();

                        var info = ((SheetManager)sheetManager).LoadSheet (sheetControl.Data, sheetControl.DataLayout, thing);
                        
                        sheetControl.Invalidate ();
                        
                        sheetControl.CommandsExecute();
                        sheetControl.Text = info.Name;
                        sheetControl.SceneId = thing.Id;
                        
                        Registry.ApplyProperties<MarkerContextProcessor, Scene>(sheetControl.Data);
                        
                        DoShowStreamControl(sheetControl);
                        currentThing = thing;
                        
                    }
                } catch (Exception ex) {
                    MessageBox.Show(ex.Message, "error", MessageBoxButtons.OK);
                }
                finally {
                    thing.ClearRealSubject ();
                }
            }
        }

        protected void DoShowStreamControl(Control control) {
            if (ShowStreamControl != null) {
                StreamControl = control;
                ShowStreamControl (control);
            }
        }


        public event Action<Control> ShowStreamControl = null;

        public void SaveStream() {
            if (currentThing != null && StreamControl !=null) {
                if (StreamControl is RichTextBox) {
                    StreamWriter writer = new StreamWriter (new MemoryStream ());
                    writer.Write (( (RichTextBox) StreamControl ).Rtf);
                    writer.Flush ();
                    writer.BaseStream.Position = 0;

                    StreamInfo<Stream> streamInfo = new StreamInfo<Stream> ();
                    streamInfo.StreamType = StreamTypes.RTF;
                    streamInfo.Compression = CompressionType.bZip2;
                    streamInfo.Data = writer.BaseStream;

                }
            }
        }

        public void FocusChanged(object sender, SceneEventArgs e) {
            var graph = new GraphPairFacade<IWidget, IEdgeWidget>().Source<IThing, ILink>(e.Scene.Graph);


            if (e.Widget != null && graph != null) {

                WidgetThingGraph widgetThingGraph = (WidgetThingGraph)graph;
                IStreamThing thing = widgetThingGraph.Get(e.Widget) as IStreamThing;
                findControlForStreamThing(thing);

            }
        }

        public void Dispose() {
            this.StreamControl = null;
            if (this.imageDisplay != null) {
                this.imageDisplay.Dispose ();
                this.imageDisplay= null;
            }
            if (this.textControl != null) {
                this.textControl.Dispose ();
                this.textControl = null;
            }
            if (this.htmlControl != null) {
                this.htmlControl.Dispose ();
                this.htmlControl = null;
            }
        }
    }
}