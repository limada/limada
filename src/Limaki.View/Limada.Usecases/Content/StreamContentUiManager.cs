/*
 * Limada 
 * 
 * This code is free software; you can redistribute it and/or modify it
 * under the terms of the GNU General Public License version 2 only, as
 * published by the Free Software Foundation.
 * 
 * Author: Lytico
 * Copyright (C) 2010-2013 Lytico
 *
 * http://www.limada.org
 */

using System;
using System.IO;
using Limada.Data;
using Limada.Model;
using Limada.VisualThings;
using Limaki;
using Limaki.Common;
using Limaki.Contents;
using Limaki.Contents.IO;
using Limaki.Drawing;
using Limaki.Graphs;
using Limaki.Graphs.Extensions;
using Limaki.Reporting;
using Limaki.Usecases;
using Limaki.Viewers;
using Limaki.Visuals;

namespace Limada.Usecases {

    public class StreamContentUiManager : IoUiManager {

        /// <summary>
        /// called after Content is read
        /// </summary>
        public Action<Content<Stream>> ContentIn { get { return StreamContentIoManager.SinkIn; } set { StreamContentIoManager.SinkIn = value; } }

        /// <summary>
        /// called to get the Content to be written
        /// </summary>
        public Func<Content<Stream>> ContentOut { get { return StreamContentIoManager.SinkOut; } set { StreamContentIoManager.SinkOut = value; } }

        public string ReadFilter { get { return StreamContentIoManager.ReadFilter; } }
        public string WriteFilter { get { return StreamContentIoManager.WriteFilter; } }

        private StreamContentIoManager _streamContentIoManager = null;
        public StreamContentIoManager StreamContentIoManager {
            get {
                return _streamContentIoManager ?? 
                    (_streamContentIoManager = new StreamContentIoManager { Progress = this.Progress });
            }
        }

        /// <summary>
        /// reads a content from a file
        /// and provides it in ContentIn-Action
        /// </summary>
        public void Read () {

            DefaultDialogValues(OpenFileDialog, ReadFilter);

            if (FileDialogShow(OpenFileDialog, true) == DialogResult.Ok) {
                StreamContentIoManager.ConfigureSinkIo = s => ConfigureSink(s);
                var content = StreamContentIoManager.ReadSink(IoUtils.UriFromFileName(OpenFileDialog.FileName));
                if (content != null && content.Data != null)
                    StreamContentIoManager.Close = () => content.Data.Close();
                OpenFileDialog.ResetFileName();
            }
        }

        /// <summary>
        /// gets a content from ContentOut-Func
        /// and writes it into a file
        /// </summary>
        public void Save () {
            if (ContentOut == null)
                return;

            try {
                DefaultDialogValues(SaveFileDialog, WriteFilter);
                var content = ContentOut();
                if (content != null) {
                    var info = StreamContentIoManager.GetContentInfo(content);
                    if (info != null) {
                        string ext = null;
                        SaveFileDialog.Filter = StreamContentIoManager.GetFilter(info, out ext) + "All Files|*.*";
                        SaveFileDialog.DefaultExt = ext;
                        SaveFileDialog.SetFileName(content.Source.ToString());
                        if (FileDialogShow(SaveFileDialog, false) == DialogResult.Ok) {
                            StreamContentIoManager.ConfigureSinkIo = s => ConfigureSink(s);
                            StreamContentIoManager.WriteSink(content, IoUtils.UriFromFileName(SaveFileDialog.FileName));
                            SaveFileDialog.ResetFileName();
                        }
                    }
                }
            } catch (Exception ex) {
                Registry.Pool.TryGetCreate<IExceptionHandler>().Catch(ex, MessageType.OK);
            }
        }

        #region Things-Import-Export

        private ThingGraphCursorIoManager _thingGraphCursorIoManager = null;
        public ThingGraphCursorIoManager ThingGraphCursorIoManager { get {
            return _thingGraphCursorIoManager ?? (_thingGraphCursorIoManager = new ThingGraphCursorIoManager { Progress = this.Progress });
        } }

        protected ThingsStreamIoManager _thingsStreamIoManager = null;
        protected ThingsStreamIoManager ThingsStreamIoManager { get { return _thingsStreamIoManager ?? (_thingsStreamIoManager = new ThingsStreamIoManager { Progress = this.Progress }); } }

        public void ConfigureSink<TSource, TSink> (IPipe<TSource, TSink> sink) {
            var report = sink as IReport;
            if (report != null) {
                report.Options.MarginBottom = 0;
                report.Options.MarginLeft = 0;
                report.Options.MarginTop = 0;
                report.Options.MarginRight = 0;
            }
        }

        public void WriteThings (IGraphScene<IVisual, IVisualEdge> scene) {
            try {
                DefaultDialogValues(SaveFileDialog, ThingsStreamIoManager.WriteFilter);
                if (scene != null && scene.HasThingGraph()) {
                    SaveFileDialog.DefaultExt = "pdf";
                    if (FileDialogShow(SaveFileDialog, false) == DialogResult.Ok) {
                        ThingsStreamIoManager.SinkOut = () => new VisualThingsSceneViz().SelectedThings(scene);
                        ThingsStreamIoManager.ConfigureSinkIo = s => ConfigureSink(s);
                        ThingsStreamIoManager.WriteSink(IoUtils.UriFromFileName(SaveFileDialog.FileName));
                        SaveFileDialog.ResetFileName();
                    }
                }
            } catch (Exception ex) {
                Registry.Pool.TryGetCreate<IExceptionHandler>().Catch(ex, MessageType.OK);
            }
        }

        public void ReadThingGraphCursor (IGraphScene<IVisual, IVisualEdge> scene) {
            try {
                DefaultDialogValues(OpenFileDialog, ThingGraphCursorIoManager.ReadFilter);
                if (scene != null && scene.HasThingGraph()) {
                    if (FileDialogShow(OpenFileDialog, true) == DialogResult.Ok) {

                        var graphCursor = new GraphCursor<IThing, ILink>(scene.Graph.Source<IVisual, IVisualEdge, IThing, ILink>().Source);
                        var uri = IoUtils.UriFromFileName(OpenFileDialog.FileName);
                        ThingGraphCursorIoManager.ConfigureSinkIo = s => ConfigureSink(s);
                        
                        graphCursor = ThingGraphCursorIoManager.ReadSink(uri, graphCursor);
                        new VisualThingsSceneViz().SetDescription(scene, graphCursor.Cursor, OpenFileDialog.FileName);
                        
                        OpenFileDialog.ResetFileName();
                    }
                }
            } catch (Exception ex) {
                Registry.Pool.TryGetCreate<IExceptionHandler>().Catch(ex, MessageType.OK);
            }
        }
       
        #endregion
    }
}