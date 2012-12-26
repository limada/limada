/*
 * Limada 
 * 
 * This code is free software; you can redistribute it and/or modify it
 * under the terms of the GNU General Public License version 2 only, as
 * published by the Free Software Foundation.
 * 
 * Author: Lytico
 * Copyright (C) 2006-2011 Lytico
 *
 * http://www.limada.org
 */

using System;
using System.IO;
using Limada.Data;
using Limada.Model;
using Limada.UseCases;
using Limaki.Common;
using Limaki.Data;
using Limaki.Drawing;
using Limada.View;
using Limada.VisualThings;
using Limaki.Model.Streams;
using System.Collections.Generic;
using Limaki.Graphs.Extensions;
using Limaki.Viewers;
using Limaki.Visuals;
using Mono.Options;

namespace Limada.UseCases {
	public class FileManager:FileManagerBase {

        public Action<IGraphScene<IVisual, IVisualEdge>> DataBound = null;
        public Action<string> DataPostProcess = null;
        public Action ApplicationQuit = null;
        #region ThingGraph Open 

        public override bool OpenFile(DataBaseInfo fileName) {
            var provider = GetThingGraphProvider(fileName);
            bool result = false;
            if (provider != null) {
                var sceneProvider = new SceneProvider {
                    Provider = provider,
                    DataBound = this.DataBound
                };

                this.ThingGraphProvider.Close();

                if (sceneProvider.Open(fileName)) {
                    this.ThingGraphProvider = provider;
                    DataPostProcess(fileName.Name);
                    result = true;
                }


            }
            return result;
        }

        public void OpenFile() {
            if (this.HasUnsavedData()) {
                if (MessageBoxShow("You have an unsaved document. Do you want to save it?", "", MessageBoxButtons.YesNo) ==
                    DialogResult.Yes) {
                    SaveAsFile();
                }
            }

            DefaultDialogValues(OpenFileDialog);
            if (FileDialogShow(OpenFileDialog, true) == DialogResult.OK) {
                this.OpenFile(DataBaseInfo.FromFileName(OpenFileDialog.FileName));
            }
        }

        public bool OpenCommandLineOptions () {
            var result = false;
            var filesToAdd = new List<string>();
            string fileToOpen = null;
            var exitAfterImport = false;

            var p = new OptionSet() {
                                        { "add=", a => filesToAdd .Add(a)}, 
                                        { "file=", a => fileToOpen=a },
                                        { "exit", a => exitAfterImport=a!=null },
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
                            var db = DataBaseInfo.FromFileName(fileToAdd);
                            var provider = GetThingGraphProvider(db);
                            if (provider != null) {
                                try {
                                    provider.Open(db);
                                    provider.Merge(provider.Data, this.ThingGraphProvider.Data);
                                } catch (Exception ex) {
                                    Registry.Pool.TryGetCreate<IExceptionHandler>()
                                        .Catch(new Exception("Add file failed: " + ex.Message, ex), MessageType.OK);
                                } finally {
                                    provider.Close();
                                    provider.Data = null;
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
            if (exitAfterImport && ApplicationQuit!=null)
                ApplicationQuit();
            return result;
        }

        public bool OpenCommandLine (string [] args) {

            string fileName = null;
            if (args.Length > 1) {
                if (File.Exists(args[1])) {
                    fileName = args[1];
                }
            }
            if (fileName != null) {
                return OpenFile(DataBaseInfo.FromFileName(fileName));
            }
            return false;
        }

	    public bool OpenCommandLine() {
            return OpenCommandLine(Environment.GetCommandLineArgs());
        }

        public void ShowEmptyThingGraph() {
            var provider = new MemoryThingGraphProvider();
            this.ThingGraphProvider.Close();
            this.ThingGraphProvider = provider;

            var handler = new SceneProvider {
                Provider = provider,
                DataBound = this.DataBound
            };
            handler.Open();

            DataPostProcess("unknown");
        }

        #endregion

        #region Thinggraph Save 

        public bool SaveAs(DataBaseInfo fileName) {

            var provider = GetThingGraphProvider(fileName);
            bool result = false;
            if (provider != null) {
                if (this.ThingGraphProvider.Saveable) {
                    this.ThingGraphProvider.Save();
                }
                try {
                    provider.SaveAs(this.ThingGraphProvider.Data, fileName);
                    this.ThingGraphProvider.Close();
                    this.ThingGraphProvider = provider;

                    ISceneProvider handler = new SceneProvider();
                    handler.DataBound = this.DataBound;
                    handler.Provider = provider;
                    handler.Open(() => { });
                    DataPostProcess(fileName.Name);
                    result = true;
                } catch (Exception ex) {
                    Registry.Pool.TryGetCreate<IExceptionHandler>()
                        .Catch(new Exception("Save as failed: " + ex.Message, ex), MessageType.OK);
                    provider.Data = null;
                    provider.Close();
                }

            }
            return result;
        }

        public void Save() {
            if (this.HasUnsavedData()) {
                SaveAsFile();
            } else {
                if (_thingGraphProvider != null && _thingGraphProvider.Saveable) {
                    _thingGraphProvider.Save();
                }
            }
        }

        public void SaveAsFile() {
            DefaultDialogValues(SaveFileDialog);
            if (FileDialogShow(SaveFileDialog, true) == DialogResult.OK) {
                this.SaveAs(DataBaseInfo.FromFileName(SaveFileDialog.FileName));
            }
        }

        public bool HasUnsavedData() {
            if (_thingGraphProvider is MemoryThingGraphProvider) {
                if (this.ThingGraphProvider.Data.Count > 0) {
                    return true;
                }
            }
            return false;
        }

        public bool IsSceneExportable(IGraphScene<IVisual, IVisualEdge> scene) {
            if (scene != null) {
                var graph = scene.Graph.Source<IVisual, IVisualEdge, IThing, ILink>();
                return graph != null;
            }
            return false;
        }

        public void ExportAsThingGraph(DataBaseInfo fileName, IGraphScene<IVisual, IVisualEdge> scene) {
            if (IsSceneExportable(scene)) {
                var provider = new SceneProvider();
                provider.Provider = GetThingGraphProvider(fileName);
                provider.ExportAsThingGraph (scene, fileName);
                provider.Provider.Close ();
            }
        }

        public void ExportAsThingGraph(IGraphScene<IVisual, IVisualEdge> scene) {
            DefaultDialogValues(SaveFileDialog);
            if (scene != null && this.IsSceneExportable(scene)) {
                if (FileDialogShow(SaveFileDialog, true) == DialogResult.OK) {
                    this.ExportAsThingGraph(
                        DataBaseInfo.FromFileName(SaveFileDialog.FileName),
                        scene);
                }
            }
        }

        #endregion

	    #region Dialog-Handling
        public FileDialogMemento OpenFileDialog { get; set; }
        public FileDialogMemento SaveFileDialog { get; set; }

        public Func<string, string, MessageBoxButtons, DialogResult> MessageBoxShow { get; set; }
        public Func<FileDialogMemento, bool, DialogResult> FileDialogShow { get; set; }

        public void DefaultDialogValues(FileDialogMemento dialog) {
            dialog.Filter = this.ThingGraphFileProviderFilter + "All Files|*.*";
            dialog.DefaultExt = DefaultExtension;
            dialog.AddExtension = true;
            dialog.CheckFileExists = false;
            dialog.CheckPathExists = true;
            dialog.AutoUpgradeEnabled = true;
            dialog.SupportMultiDottedExtensions = true;
            // Important! under windows this adds the fileextension!
            dialog.ValidateNames = true;

        }

        public string DefaultExtension = "limo";
        string _thingGraphFileProviderFilter = null;
        public string ThingGraphFileProviderFilter {
            get {
                return _thingGraphFileProviderFilter ?? (_thingGraphFileProviderFilter = GetFileProviderFilter<IThingGraph>());
            }
        }

        string _thingsProviderFilter = null;
        public string ThingsProviderFilter {
            get {
                return _thingsProviderFilter ?? (_thingsProviderFilter = GetFileProviderFilter<IEnumerable<IThing>>());
            }
        }

        public string GetFileProviderFilter<T>() {
            string _fileProviderFilter = "";

            var providers = Registry.Pool.TryGetCreate<DataProviders<T>>();
            string defaultFilter = null;
            foreach (var provider in providers) {
                if (provider.Saveable) {
                    string filter = provider.Description + "|*" + provider.Extension + "|";
                    if (provider.Extension == "." + DefaultExtension)
                        defaultFilter = filter;
                    else
                        _fileProviderFilter += filter;

                }
            }
            if (defaultFilter != null) {
                _fileProviderFilter = defaultFilter + _fileProviderFilter;
            }

            return _fileProviderFilter;

        }
        #endregion

        #region Import

        public void ImportThingGraphRaw() {
            Save();
            Close();
            bool tryIt = true;
            while (tryIt && MessageBoxShow("Open a new, non exisiting file", "RawImport", MessageBoxButtons.OKCancel) == DialogResult.OK) {
                var fileDialog = new FileDialogMemento();
                DefaultDialogValues(fileDialog);
                DefaultDialogValues(OpenFileDialog);
                if (FileDialogShow(fileDialog, true) == DialogResult.OK) {
                    var target = fileDialog.FileName;
                    if (File.Exists(target)) {
                        continue;
                    }
                    if (tryIt = MessageBoxShow("Open the file to import", "RawImport", MessageBoxButtons.OKCancel) == DialogResult.OK) {
                        if (FileDialogShow(OpenFileDialog, true) == DialogResult.OK) {

                            var source = OpenFileDialog.FileName;
                            if (!File.Exists(source)) {
                                MessageBoxShow("File does not exist", "RawImport", MessageBoxButtons.OK);
                                break;
                            }
                            tryIt = false;
                            var targetProvider = GetThingGraphProvider(DataBaseInfo.FromFileName(target));
                            var sourceProvider = GetThingGraphProvider(DataBaseInfo.FromFileName(source));

                            try {
                                targetProvider.Open(DataBaseInfo.FromFileName(target));
                                sourceProvider.RawImport(DataBaseInfo.FromFileName(source), targetProvider);
                            } catch (Exception ex) {
                                Registry.Pool.TryGetCreate<IExceptionHandler>()
                                    .Catch(new Exception("Raw import failed: " + ex.Message, ex), MessageType.OK);
                                targetProvider.Data = null;
                                targetProvider.Close();
                                File.Delete(target);
                            }
                            targetProvider.Close();
                            MessageBoxShow("Import successfull", "RawImport", MessageBoxButtons.OK);
                            this.OpenFile(DataBaseInfo.FromFileName(target));
                        }
                    }
                }
            }

        }
        
        #endregion 

        #region Export

        public void ExportThingsAs(DataBaseInfo fileName, IGraphScene<IVisual, IVisualEdge> scene) {
            if (IsSceneExportable(scene)) {
                var thingsProvider = GetThingsProvider(fileName);
                if (thingsProvider == null)
                    return;

                var provider = new SceneProvider() { Progress = this.Progress };
                try {
                    provider.ExportTo(scene, thingsProvider, fileName);
                    thingsProvider.Close();
                } catch(Exception ex) {
                    Message(ex.Message,-1,-1);
                }
            }
        }

        public void ExportThingsAs(IGraphScene<IVisual, IVisualEdge> scene) {
            DefaultDialogValues(SaveFileDialog);
            if (scene != null && this.IsSceneExportable(scene)) {

                SaveFileDialog.Filter = this.ThingsProviderFilter + "All Files|*.*";
                SaveFileDialog.DefaultExt = "pdf";

                if (FileDialogShow(SaveFileDialog, true) == DialogResult.OK) {
                    var fileName = DataBaseInfo.FromFileName(SaveFileDialog.FileName);
                    this.ExportThingsAs(fileName,scene);
                }
            }
        }

        public bool DocumentHasPages(IGraphScene<IVisual, IVisualEdge> scene) {
            var graph = scene.Graph;
            var document = scene.Focused;
            var documentSchemaManager = new DocumentSchemaManager();

            return documentSchemaManager.HasPages(graph, document);

        }

        public void ExportPages(string dir, IGraphScene<IVisual, IVisualEdge> scene) {
            var graph = scene.Graph;
            var document = scene.Focused;
            var documentSchemaManager = new DocumentSchemaManager();
            if (documentSchemaManager.HasPages(graph, document)) {
                int i = 0;
                foreach (var streamThing in documentSchemaManager.PageStreams(graph, document)) {
                    string pageName = i.ToString().PadLeft(5, '0');
                    if (streamThing.Description != null)
                        pageName = streamThing.Description.ToString().PadLeft(5, '0');

                    string name = dir + Path.DirectorySeparatorChar +
                                  scene.Focused.Data.ToString() + " " +
                                  pageName +
                                  ContentTypes.Extension(streamThing.StreamType);

                    streamThing.Data.Position = 0;
                    using (FileStream fileStream = new FileStream(name, FileMode.Create)) {
                        var buff = new byte[streamThing.Data.Length];
                        streamThing.Data.Read(buff, 0, (int)streamThing.Data.Length);
                        fileStream.Write(buff, 0, (int)streamThing.Data.Length);
                        fileStream.Flush();
                        fileStream.Close();
                    }
                    streamThing.Data.Dispose();
                    streamThing.Data = null;
                }
            }
        }
        
        #endregion
    }
}