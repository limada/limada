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
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Limada.IO;
using Limada.Model;
using Limada.View.VisualThings;
using Limaki.Common;
using Limaki.Contents;
using Limaki.Contents.IO;
using Limaki.Data;
using Limaki.Usecases;
using Limaki.View;
using Limaki.View.Common;
using Limaki.View.GraphScene;
using Limaki.View.Vidgets;
using Limaki.View.Visuals;
using Limaki.View.Viz.Mapping;
using Mono.Options;

namespace Limada.Usecases.Contents {

    public class ThingGraphUiManager : IoUiManager, IGraphSceneUiManager {

        protected ThingGraphContent Data { get; set; }

        public Action DataBound { get; set; }
        public Action<string> DataPostProcess { get; set; }


        /// <summary>
        /// called after ThingGraph is read
        /// </summary>
        public Action<ThingGraphContent> ThingGraphIn { get { return ThingGraphIoManager.SinkIn; } set { ThingGraphIoManager.SinkIn = value; } }

        /// <summary>
        /// called after ThingGraph is closed
        /// </summary>
        /// <value>The ThingGraph closed.</value>
        public Action<ThingGraphContent> ThingGraphClosed { get; set; }

        /// <summary>
        /// TODO: called to get the ThingGraph to be written
        /// </summary>
        public Func<ThingGraphContent> ThingGraphOut { get { return ThingGraphIoManager.SinkOut; } set { ThingGraphIoManager.SinkOut = value; } }

        public string ReadFilter { get { return ThingGraphIoManager.ReadFilter; } }
        public string WriteFilter { get { return ThingGraphIoManager.WriteFilter; } }

        private ThingGraphIoManager _thingGraphIoManager = null;
        public ThingGraphIoManager ThingGraphIoManager {
            get {
                return _thingGraphIoManager ?? (_thingGraphIoManager =
                                                new ThingGraphIoManager {
                                                    Progress = this.Progress,
                                                    DefaultExtension = "limo",
                                                });
            }
        }

		private IGraphSceneMapDisplayOrganizer<IVisual, IVisualEdge> _organizer = null;
		public IGraphSceneMapDisplayOrganizer<IVisual, IVisualEdge> Organizer {
			get { return _organizer ?? (_organizer = Registry.Pooled<IGraphSceneMapDisplayOrganizer<IVisual, IVisualEdge>> ()); }
			set { _organizer = value; }
		}

        #region Open

        public void Open () {
            if (this.IsUnsaved(this.Data)) {
                if (MessageBoxShow("You have an unsaved document. Do you want to save it?", "", MessageBoxButtons.YesNo) ==
                    DialogResult.Yes) {
                    SaveAs();
                }
            }

            DefaultDialogValues(OpenFileDialog, ReadFilter);
            if (FileDialogShow(OpenFileDialog, true) == DialogResult.Ok) {
                Open(Iori.FromFileName(OpenFileDialog.FileName));
                OpenFileDialog.ResetFileName();
            }

        }

        public bool Open (Iori iori) {
			
            ThingGraphContent source = null;
            var sinkIo = ThingGraphIoManager.GetSinkIO(iori, IoMode.Read) as ThingGraphIo;

            try {
                // TODO: check if iori already in use
				
                source = sinkIo.Open(iori);
                OnProgress("", -1, -1);
                AttachCurrent(source, iori.Name);
                return true;

            } catch (Exception ex) {
                Registry.Pooled<IExceptionHandler>()
                    .Catch(new Exception("Open failed: " + ex.Message, ex), MessageType.OK);
                try {
                    if (source != null)
                        sinkIo.Close(source);
                } catch { }

                return false;

            }
        }

        public void ShowEmptyScene () {
            
            OnProgress("", -1, -1);
            var source = new MemoryThingGraphIo().Open(null);
            AttachCurrent(source, "unknown");
           
        }

		public bool AttachCurrent (ThingGraphContent source, string message) {

            Close (Data);

			Data = source;

			ThingGraphIn?.Invoke (source);

		    Organizer.ApplyBackGraph (source.Data);
		    
            DataBound?.Invoke ();

			DataPostProcess?.Invoke (message);

			return true;
		}

        public void SetContent (Content content) {
            var tgc = content as ThingGraphContent;
            if (tgc != null) {
                Data = tgc;
            }
        }

        #endregion

        public void Save () {
            if (Data == null)
                return;
            if (IsUnsaved(this.Data)) {
                SaveAs();
            } else {
                var sinkIo = ThingGraphIoManager.GetSinkIO(Data.ContentType, IoMode.Write) as ThingGraphIo;
                if (sinkIo != null) {
                    sinkIo.Flush(Data);
                }
            }
        }

        public bool SaveAs (Iori iori) {
            // save current:
            var oldSource = "";
            ThingGraphIo sinkIo = null;
            if (Data.Source != null) {
                oldSource = this.Data.Source.ToString ();

                sinkIo = ThingGraphIoManager.GetSinkIO (Data.ContentType, IoMode.Write) as ThingGraphIo;
                if (sinkIo != null) {
                    sinkIo.Flush (Data);
                }
            }

            // get the new:
            sinkIo = ThingGraphIoManager.GetSinkIO (iori, IoMode.Write) as ThingGraphIo;
            if (sinkIo == null)
                return false;
            var sink = sinkIo.Open (iori);

            // saveAs:
            var merger = new ThingGraphMerger { Progress = this.Progress };
            merger.Use (Data.Data, sink.Data);
            merger.AttachThings (sink.Data);

            // close and reopen; flush and attach does't work in all cases
            sinkIo.Close (sink);
            sink = sinkIo.Open (iori);

            AttachCurrent (sink, iori.Name);

            OnProgress ($"{Path.GetFileNameWithoutExtension (oldSource)} saved as {iori.Name}", -1, -1);
            return true;
        }

        public void SaveAs () {
            DefaultDialogValues(SaveFileDialog, WriteFilter);
            SaveFileDialog.OverwritePrompt = false;
            if (FileDialogShow(SaveFileDialog, false) == DialogResult.Ok) {
                SaveAs(Iori.FromFileName(SaveFileDialog.FileName));
                SaveFileDialog.ResetFileName();
            }
        }

        public void ExportSceneView (Iori iori, IGraphScene<IVisual, IVisualEdge> scene) {
            if (scene.HasThingGraph()) {
                var sinkIo = ThingGraphIoManager.GetSinkIO(iori, IoMode.Write) as ThingGraphIo;
                if (sinkIo != null) {
                    var source = Registry.Create<ISceneInteractor<IVisual, IThing, IVisualEdge, ILink>> ()
                        .CreateThingsView(scene);
                    var sink = sinkIo.Open(iori);
                    new ThingGraphExporter { Progress = this.Progress }.Use(source, sink.Data);
                    sinkIo.Close(sink);
                }
            }
        }

        public void ExportSceneView (IGraphScene<IVisual, IVisualEdge> scene) {
            DefaultDialogValues(SaveFileDialog, WriteFilter);
            if (scene != null && scene.HasThingGraph()) {
                SaveFileDialog.OverwritePrompt = false;
                if (FileDialogShow(SaveFileDialog, false) == DialogResult.Ok) {
                    ExportSceneView(Iori.FromFileName(SaveFileDialog.FileName), scene);
                    SaveFileDialog.ResetFileName();
                }
            }
        }

        protected void Close (ThingGraphContent data) {
            if (data == null)
                return;

            Organizer.RemoveMappedGraph (data.Data);
            Organizer.ClearDisplays ();

            var sink = ThingGraphIoManager.GetSinkIO(data.ContentType, IoMode.Write) as ThingGraphIo;
            if (sink != null) {
                sink.Close (data);
                ThingGraphClosed?.Invoke (data);
            }
        }

        public void Close () {
            Close(Data);
        }

        public bool IsUnsaved (ThingGraphContent data) {
            var sink = ThingGraphIoManager.GetSinkIO(data.ContentType, IoMode.None) as ThingGraphIo;
            if (sink is MemoryThingGraphIo) {
                if (data.Data.Count > 0) {
                    return true;
                }
            }
            return false;
        }

        #region command line

        public Action ApplicationQuit { get; set; }

        public bool ImportFiles (IEnumerable<string> sourceFiles) {

            var result = true;
            if (sourceFiles.Count () > 0) {
                var progressSaved = this.Progress;
                var progressHandler = Registry.Pooled<IProgressHandler> ();
                this.Progress = (m, i, max) => progressHandler.Write ( m, i, max );
                try {
                    foreach (var sourceFile in sourceFiles) {
                        if (File.Exists ( sourceFile )) {
                            progressHandler.Show ( "Importing file " + sourceFile );
                            var sourceIori = Iori.FromFileName ( sourceFile );
                            var sourceIo = ThingGraphIoManager.GetSinkIO ( sourceIori, IoMode.Read ) as ThingGraphIo;
                            if (sourceIo != null) {
                                ThingGraphContent source = null;
                                try {
                                    source = sourceIo.Open ( sourceIori );
                                    new ThingGraphMerger { Progress = this.Progress }.Use ( source.Data, Data.Data );
                                } catch (Exception ex) {
                                    Registry.Pooled<IExceptionHandler> ()
                                        .Catch ( new Exception ( "Add file failed: " + ex.Message, ex ), MessageType.OK );
                                    result = false;
                                } finally {
                                    sourceIo.Close ( source );
                                }
                            }
                        }
                    }
                    Progress = null;
                    progressHandler.Close ();
                } finally {
                    Progress = progressSaved;
                }
            }
            return result;
        }

        public bool OpenCommandLine () {
			
            var result = false;
            var sourceFiles = new List<string>();
            string fileToOpen = null;
            string resumeFile = null;
            var exitAfterImport = false;

            var p = new OptionSet() {
                                        {"add=", a => sourceFiles.Add(a)},
                                        {"file=", a => fileToOpen = a},
                                        {"resume=", a => resumeFile = a},
                                        {"exit", a => exitAfterImport = a != null},
                                    };
            var options = p.Parse(Environment.GetCommandLineArgs());

            if (fileToOpen == null) {
                if (options.Count > 1)
                    fileToOpen = options[1];
            }

            if (fileToOpen != null && File.Exists ( fileToOpen ))
                result = Open ( Iori.FromFileName ( fileToOpen ) );

            if (result)
                result = ImportFiles (sourceFiles);

            if (exitAfterImport && ApplicationQuit != null)
                ApplicationQuit();
            return result;
        }
        #endregion

        public void ImportRawSource () {
            Save();
            Close(Data);
            bool tryIt = true;

            while (tryIt && MessageBoxShow("Open a new, non exisiting file", "RawImport", MessageBoxButtons.OkCancel) == DialogResult.Ok) {
                
                var fileDialog = new FileDialogMemento();
                DefaultDialogValues(fileDialog,WriteFilter);
                DefaultDialogValues(OpenFileDialog,ReadFilter);

                if (FileDialogShow(fileDialog, true) == DialogResult.Ok) {
                    var sinkFile = fileDialog.FileName;
                    fileDialog.ResetFileName();
                    if (File.Exists(sinkFile)) {
                        continue;
                    }
                    if (tryIt = MessageBoxShow("Open the file to import", "RawImport", MessageBoxButtons.OkCancel) == DialogResult.Ok) {
                        if (FileDialogShow(OpenFileDialog, true) == DialogResult.Ok) {

                            var sourceFile = OpenFileDialog.FileName;
                            if (!File.Exists(sourceFile)) {
                                MessageBoxShow("File does not exist", "RawImport", MessageBoxButtons.Ok);
                                break;
                            }

                            tryIt = false;
                            var sourceInfo = Iori.FromFileName(sourceFile);
                            var sinkIo = ThingGraphIoManager.GetSinkIO(Iori.FromFileName(sinkFile), IoMode.Write) as ThingGraphIo;
                            ThingGraphContent sink = null;

                            try {

                                sink = sinkIo.Open(Iori.FromFileName(sinkFile));
                                var repairer = Registry.Pooled<ThingGraphRepairPool>()
                                    .Find(sourceInfo.Extension,IoMode.Read) as IPipe<Iori, IThingGraph>;
                                this.AttachProgress(repairer as IProgress);
                                repairer.Use(Iori.FromFileName(sourceFile), sink.Data);

                            } catch (Exception ex) {
                                Registry.Pooled<IExceptionHandler>()
                                    .Catch(new Exception("Raw import failed: " + ex.Message, ex), MessageType.OK);
                                sinkIo.Close(sink);
                                File.Delete(sinkFile);
                            }
                            sinkIo.Close(sink);
                            MessageBoxShow("Import successfull", "RawImport", MessageBoxButtons.Ok);
                            this.Open(Iori.FromFileName(sinkFile));
                            OpenFileDialog.ResetFileName();
                        }
                    }
                }
            }

        }


    }
}