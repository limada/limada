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

using Limada.Data;
using Limada.Model;
using Limada.VisualThings;
using Limaki.Common;
using Limaki.Data;
using Limaki.Drawing;
using Limaki.Model.Content.IO;
using Limaki.Usecases;
using Limaki.Viewers;
using Limaki.Visuals;
using Mono.Options;
using System;
using System.Collections.Generic;
using System.IO;

namespace Limada.Usecases {

    public class ThingGraphUiManager : IoUiManager, IFileManager {

        public ThingGraphContent Current { get; set; }

        public Action<IGraphScene<IVisual, IVisualEdge>> DataBound { get; set; }
        public Action<string> DataPostProcess { get; set; }

        /// <summary>
        /// called after Content is read
        /// </summary>
        public Action<ThingGraphContent> ThingGraphIn { get { return ThingGraphIoManager.SinkIn; } set { ThingGraphIoManager.SinkIn = value; } }

        /// <summary>
        /// called to get the Content to be written
        /// </summary>
        public Func<ThingGraphContent> ThingGraphOut { get { return ThingGraphIoManager.SinkOut; } set { ThingGraphIoManager.SinkOut = value; } }

        public string ReadFilter { get { return ThingGraphIoManager.ReadFilter; } }
        public string WriteFilter { get { return ThingGraphIoManager.WriteFilter; } }

        private IoManager<IoInfo, ThingGraphContent> _thingGraphIoManager = null;
        public IoManager<IoInfo, ThingGraphContent> ThingGraphIoManager { get { return _thingGraphIoManager ?? (_thingGraphIoManager = new IoManager<IoInfo, ThingGraphContent> { Progress = this.Progress }); } }

        #region Open

        public void OpenFile () {
            if (this.HasUnsavedData(this.Current)) {
                if (MessageBoxShow("You have an unsaved document. Do you want to save it?", "", MessageBoxButtons.YesNo) ==
                    DialogResult.Yes) {
                    SaveAsFile();
                }
            }

            DefaultDialogValues(OpenFileDialog, ReadFilter);
            if (FileDialogShow(OpenFileDialog, true) == DialogResult.OK) {
                OpenFile(IoInfo.FromFileName(OpenFileDialog.FileName));
                OpenFileDialog.ResetFileName();
            }
        }

        public bool OpenFile (IoInfo sourceInfo) {
            ThingGraphContent source = null;
            var sinkIo = ThingGraphIoManager.GetSinkIO(sourceInfo, InOutMode.Read) as ThingGraphIo;

            try {
                source = sinkIo.Open(sourceInfo);
                AttachCurrent(source, sourceInfo.Name);
                return true;
            } catch (Exception ex) {
                Registry.Pool.TryGetCreate<IExceptionHandler>()
                    .Catch(new Exception("Open failed: " + ex.Message, ex), MessageType.OK);
                try {
                    sinkIo.Close(source);
                } catch { }

                return false;

            }
        }

        public void ShowEmptyThingGraph () {
            var source = new MemoryThingGraphIo().Open(null);
            AttachCurrent(source, "unknown");
        }

        public bool AttachCurrent (ThingGraphContent source, string message) {

            Close(this.Current);
            this.Current = source;

            var scene = new SceneIo().CreateScene(source.Data);
            if (this.DataBound != null)
                this.DataBound(scene);

            DataPostProcess(message);

            return true;
        }
        #endregion

        public void Save () {
            if (this.HasUnsavedData(this.Current)) {
                SaveAsFile();
            } else {
                var sinkIo = ThingGraphIoManager.GetSinkIO(this.Current.ContentType, InOutMode.Write) as ThingGraphIo;
                if (sinkIo != null)
                    sinkIo.Use(this.Current);
            }
        }

        public bool SaveAs (IoInfo sinkInfo) {
            // save current:
            var oldSource = this.Current.Source.ToString();
            var sinkIo = ThingGraphIoManager.GetSinkIO(this.Current.ContentType, InOutMode.Write) as ThingGraphIo;
            if (sinkIo != null) {
                sinkIo.Flush(this.Current);
            }

            // get the new:
            sinkIo = ThingGraphIoManager.GetSinkIO(sinkInfo, InOutMode.Write) as ThingGraphIo;
            if (sinkIo == null)
                return false;
            var sink = sinkIo.Open(sinkInfo);

            // saveAs:
            new ThingGraphMergeSink { Progress = this.Progress }.Use(this.Current.Data, sink.Data);
            sinkIo.Flush(sink);
            sink.Data.AttachThings(t => {});

            AttachCurrent(sink, sinkInfo.Name);

            OnProgress(string.Format("{0} saved as {1}",oldSource,sinkInfo.Name), -1, -1);
            return true;
        }

        public void SaveAsFile () {
            DefaultDialogValues(SaveFileDialog, WriteFilter);
            if (FileDialogShow(SaveFileDialog, false) == DialogResult.OK) {
                SaveAs(IoInfo.FromFileName(SaveFileDialog.FileName));
                SaveFileDialog.ResetFileName();
            }
        }

        public void ExportAsThingGraph (IoInfo sinkInfo, IGraphScene<IVisual, IVisualEdge> scene) {
            if (scene.HasThingGraph()) {
                var sinkIo = ThingGraphIoManager.GetSinkIO(sinkInfo, InOutMode.Write) as ThingGraphIo;
                if (sinkIo != null) {
                    var source = new SceneIo().CreateThingsView(scene);
                    var sink = sinkIo.Open(sinkInfo);
                    new ThingGraphExportSink { Progress = this.Progress }.Use(source, sink.Data);
                    sinkIo.Close(sink);
                }
            }
        }

        public void ExportAsThingGraph (IGraphScene<IVisual, IVisualEdge> scene) {
            DefaultDialogValues(SaveFileDialog, WriteFilter);
            if (scene != null && scene.HasThingGraph()) {
                if (FileDialogShow(SaveFileDialog, false) == DialogResult.OK) {
                    ExportAsThingGraph(IoInfo.FromFileName(SaveFileDialog.FileName), scene);
                    SaveFileDialog.ResetFileName();
                }
            }
        }

        protected void Close (ThingGraphContent data) {
            if (data == null)
                return;
            var sink = ThingGraphIoManager.GetSinkIO(data.ContentType, InOutMode.Write) as ThingGraphIo;
            if (sink != null)
                sink.Close(data);
        }


        public void Close () {
            Close(this.Current);
        }

        public bool HasUnsavedData (ThingGraphContent data) {
            var sink = ThingGraphIoManager.GetSinkIO(data.ContentType, InOutMode.None) as ThingGraphIo;
            if (sink is MemoryThingGraphIo) {
                if (data.Data.Count > 0) {
                    return true;
                }
            }
            return false;
        }

        #region command line

        public Action ApplicationQuit { get; set; }

        public bool OpenCommandLine (string[] args) {

            string fileName = null;
            if (args.Length > 1) {
                if (File.Exists(args[1])) {
                    fileName = args[1];
                }
            }
            if (fileName != null) {
                return OpenFile(IoInfo.FromFileName(fileName));
            }
            return false;
        }

        public bool OpenCommandLine () {
            return OpenCommandLine(Environment.GetCommandLineArgs());
        }

        public bool OpenCommandLineOptions () {
            var result = false;
            var filesToAdd = new List<string>();
            string fileToOpen = null;
            var exitAfterImport = false;

            var p = new OptionSet() {
                                        {"add=", a => filesToAdd.Add(a)},
                                        {"file=", a => fileToOpen = a},
                                        {"exit", a => exitAfterImport = a != null},
                                    };
            var options = p.Parse(Environment.GetCommandLineArgs());

            if (fileToOpen != null) {
                result = OpenCommandLine(new string[] { fileToOpen });
            } else {
                result = OpenCommandLine(options.ToArray());
            }
            if (result && filesToAdd.Count > 0) {
                var progressSaved = this.Progress;
                var progressHandler = Registry.Pool.TryGetCreate<IProgressHandler>();
                this.Progress = (m, i, max) => progressHandler.Write(m, i, max);
                try {
                    foreach (var fileToAdd in filesToAdd) {
                        if (File.Exists(fileToAdd)) {
                            progressHandler.Show("Importing file " + fileToAdd);
                            var sourceInfo = IoInfo.FromFileName(fileToAdd);
                            var sourceIo = ThingGraphIoManager.GetSinkIO(sourceInfo, InOutMode.Read) as ThingGraphIo;
                            if (sourceIo != null) {
                                ThingGraphContent source = null;
                                try {
                                    source = sourceIo.Open(sourceInfo);
                                    new ThingGraphMergeSink().Use(source.Data, this.Current.Data);
                                } catch (Exception ex) {
                                    Registry.Pool.TryGetCreate<IExceptionHandler>()
                                        .Catch(new Exception("Add file failed: " + ex.Message, ex), MessageType.OK);
                                } finally {
                                    sourceIo.Close(source);
                                }
                            }
                        }
                    }
                    this.Progress = null;
                    progressHandler.Close();
                } finally {
                    this.Progress = progressSaved;
                }
            }
            if (exitAfterImport && ApplicationQuit != null)
                ApplicationQuit();
            return result;
        }
        #endregion

        public void ImportThingGraphRaw () {
            Save();
            Close(this.Current);
            bool tryIt = true;

            while (tryIt && MessageBoxShow("Open a new, non exisiting file", "RawImport", MessageBoxButtons.OKCancel) == DialogResult.OK) {
                
                var fileDialog = new FileDialogMemento();
                DefaultDialogValues(fileDialog,WriteFilter);
                DefaultDialogValues(OpenFileDialog,ReadFilter);

                if (FileDialogShow(fileDialog, true) == DialogResult.OK) {
                    var sinkFile = fileDialog.FileName;
                    fileDialog.ResetFileName();
                    if (File.Exists(sinkFile)) {
                        continue;
                    }
                    if (tryIt = MessageBoxShow("Open the file to import", "RawImport", MessageBoxButtons.OKCancel) == DialogResult.OK) {
                        if (FileDialogShow(OpenFileDialog, true) == DialogResult.OK) {

                            var sourceFile = OpenFileDialog.FileName;
                            if (!File.Exists(sourceFile)) {
                                MessageBoxShow("File does not exist", "RawImport", MessageBoxButtons.OK);
                                break;
                            }

                            tryIt = false;
                            var sourceInfo = IoInfo.FromFileName(sourceFile);
                            var sinkIo = ThingGraphIoManager.GetSinkIO(IoInfo.FromFileName(sinkFile), InOutMode.Write) as ThingGraphIo;
                            ThingGraphContent sink = null;

                            try {

                                sink = sinkIo.Open(IoInfo.FromFileName(sinkFile));
                                var repairer = Registry.Pool.TryGetCreate<IoProvider<IThingGraphRepair, IoInfo>>()
                                    .Find(sourceInfo.Extension,InOutMode.Read) as ISink<IoInfo, IThingGraph>;
                                this.AttachProgress(repairer as IProgress);
                                repairer.Use(IoInfo.FromFileName(sourceFile), sink.Data);

                            } catch (Exception ex) {
                                Registry.Pool.TryGetCreate<IExceptionHandler>()
                                    .Catch(new Exception("Raw import failed: " + ex.Message, ex), MessageType.OK);
                                sinkIo.Close(sink);
                                File.Delete(sinkFile);
                            }
                            sinkIo.Close(sink);
                            MessageBoxShow("Import successfull", "RawImport", MessageBoxButtons.OK);
                            this.OpenFile(IoInfo.FromFileName(sinkFile));
                            OpenFileDialog.ResetFileName();
                        }
                    }
                }
            }

        }


    }
}