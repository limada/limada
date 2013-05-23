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
using Limada.VisualThings;
using Limaki;
using Limaki.Common;
using Limaki.Drawing;
using Limaki.Model.Content;
using Limaki.View.Visuals.UI;
using Limaki.Viewers;
using Limaki.Visuals;
using Limaki.Model.Content.IO;
using Limaki.Usecases;
using Limada.Model;
using Limaki.Graphs;
using Limaki.Graphs.Extensions;
using System.Collections.Generic;
using System.Linq;
using Limada.Schemata;
using Limaki.Reporting;
using Limaki.Data;


namespace Limada.Usecases {

    public class ContentStreamUiManager : IoUiManager {

        /// <summary>
        /// called after Content is read
        /// </summary>
        public Action<Content<Stream>> ContentIn { get { return ContentStreamIoManager.SinkIn; } set { ContentStreamIoManager.SinkIn = value; } }

        /// <summary>
        /// called to get the Content to be written
        /// </summary>
        public Func<Content<Stream>> ContentOut { get { return ContentStreamIoManager.SinkOut; } set { ContentStreamIoManager.SinkOut = value; } }


        public string ReadFilter { get { return ContentStreamIoManager.ReadFilter; } }
        public string WriteFilter { get { return ContentStreamIoManager.WriteFilter; } }

        private IoManager<Stream, Content<Stream>> _contentStreamManager = null;
        public IoManager<Stream, Content<Stream>> ContentStreamIoManager { get { return _contentStreamManager ?? (_contentStreamManager = new IoManager<Stream, Content<Stream>> { Progress = this.Progress }); } }

        /// <summary>
        /// reads a content from a file
        /// and provides it in ContentIn-Action
        /// </summary>
        public void ReadFile () {

            DefaultDialogValues(OpenFileDialog, ReadFilter);

            if (FileDialogShow(OpenFileDialog, true) == DialogResult.OK) {
                var content = ContentStreamIoManager.ReadSink(IOUtils.UriFromFileName(OpenFileDialog.FileName));
                if (content != null && content.Data != null)
                    ContentStreamIoManager.Close = () => content.Data.Close();
            }
        }

        /// <summary>
        /// gets a content from ContentOut-Func
        /// and writes it into a file
        /// </summary>
        public void SaveFile () {
            if (ContentOut == null)
                return;

            try {
                DefaultDialogValues(SaveFileDialog, WriteFilter);
                var content = ContentOut();
                if (content != null) {
                    var info = ContentStreamIoManager.GetContentInfo(content);
                    if (info != null) {
                        string ext = null;
                        SaveFileDialog.Filter = ContentStreamIoManager.GetFilter(info, out ext) + "All Files|*.*";
                        SaveFileDialog.DefaultExt = ext;
                        SaveFileDialog.FileName = content.Description.ToString();
                        if (FileDialogShow(SaveFileDialog, true) == DialogResult.OK) {
                            ContentStreamIoManager.WriteSink(content, IOUtils.UriFromFileName(SaveFileDialog.FileName));
                        }
                    }
                }
            } catch (Exception ex) {
                Registry.Pool.TryGetCreate<IExceptionHandler>().Catch(ex, MessageType.OK);
            }
        }

        #region Things-Export

        private IoManager<Stream, GraphFocus<IThing, ILink>> _thingGraphFocusIoManager = null;
        public IoManager<Stream, GraphFocus<IThing, ILink>> ThingGraphFocusIoManager { get { return _thingGraphFocusIoManager ?? (_thingGraphFocusIoManager = new IoManager<Stream, GraphFocus<IThing, ILink>> { Progress = this.Progress }); } }

        protected IoManager<Stream, IEnumerable<IThing>> _thingsIoManager = null;
        protected IoManager<Stream, IEnumerable<IThing>> ThingsIoManager { get { return _thingsIoManager ?? (_thingsIoManager = new IoManager<Stream, IEnumerable<IThing>> { Progress = this.Progress }); } }

        public void ConfigureSink<TSource, TSink> (ISink<TSource, TSink> sink) {
            var report = sink as IReport;
            if (report != null) {
                report.Options.MarginBottom = 0;
                report.Options.MarginLeft = 0;
                report.Options.MarginTop = 0;
                report.Options.MarginRight = 0;
            }
        }

        public IEnumerable<IThing> ThingsOut (IGraphScene<IVisual, IVisualEdge> scene) {
            var visuals = scene.Selected.Elements;
            if (visuals.Count() == 0)
                visuals = scene.Graph.Where(v => !(v is IVisualEdge));
            if (visuals.Count() == 0)
                return null;
            IEnumerable<IThing> things = null;
            if (visuals.Count() == 1) {
                var thing = scene.Graph.ThingOf(visuals.First());
                var schema = new DocumentSchema(scene.Graph.ThingGraph(), thing);
                if (schema.HasPages())
                    things = schema.OrderedPages();
                
            }
            if (things == null) {
                things = visuals
                    .OrderBy(v => v.Location, new PointComparer { Delta = 20 })
                    .Select(v => scene.Graph.ThingOf(v));
            }
            return things;

        }

        public void WriteThings (IGraphScene<IVisual, IVisualEdge> scene) {
            try {
                DefaultDialogValues(SaveFileDialog, ThingsIoManager.WriteFilter);
                 if (scene != null && scene.HasThingGraph()) {
                     SaveFileDialog.DefaultExt = "pdf";
                     if (FileDialogShow(SaveFileDialog, false) == DialogResult.OK) {
                         var uri = IOUtils.UriFromFileName(SaveFileDialog.FileName);
                         ThingsIoManager.SinkOut = ()=> ThingsOut(scene);
                         ThingsIoManager.ConfigureSinkIo = s => ConfigureSink(s);
                         ThingsIoManager.WriteSink(uri);
                     }
                 }
            } catch (Exception ex) {
                Registry.Pool.TryGetCreate<IExceptionHandler>().Catch(ex, MessageType.OK);
            }
        }
        #endregion
    }
}