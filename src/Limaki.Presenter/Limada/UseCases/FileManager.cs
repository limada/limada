/*
 * Limada 
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
 */

using System;
using System.IO;
using Limada.Data;
using Limada.Model;
using Limada.View;
using Limaki.Common;
using Limaki.Data;
using Limaki.Graphs.Extensions;
using Limaki.Model.Streams;
using Limaki.Visuals;
using Limaki.UseCases.Viewers;

namespace Limada.UseCases {
	public class FileManager:FileManagerBase {
   
        public Action<Scene> DataBound = null;
        public Action<string> DataPostProcess = null;

        public string DefaultExtension = "limo";
        string _fileProviderFilter = null;
        public string FileProviderFilter {
            get {
                if (_fileProviderFilter == null) {
                    var providers = Registry.Pool.TryGetCreate<DataProviders<IThingGraph>>();
                    string defaultFilter = null;
                    foreach(var provider in providers) {
                        if (provider.Saveable) {
                            string filter = provider.Description + "|*" + provider.Extension+"|";
                            if (provider.Extension == "." + DefaultExtension)
                                defaultFilter = filter;
                            else 
                                _fileProviderFilter += filter;
                                
                        }
                    }
                    if (defaultFilter != null) {
                        _fileProviderFilter = defaultFilter + _fileProviderFilter;
                    }
                }
                return _fileProviderFilter;
            }
        }

        public override bool OpenFile(DataBaseInfo fileName) {
            var handler = new SceneProvider();
            var provider = GetProvider(fileName);
            bool result = false;
            if (provider != null) {
                handler.Provider = provider;
                handler.DataBound = this.DataBound;

                if (handler.Open(fileName)) {
                    this.ThingGraphProvider.Close();
                    this.ThingGraphProvider = provider;
                    DataPostProcess(fileName.Name);
                    result = true;
                }


            }
            return result;
        }

        public bool SaveAs(DataBaseInfo fileName) {

            var provider = GetProvider(fileName);
            bool result = false;
            if (provider != null) {
                if (this.ThingGraphProvider.Saveable) {
                    this.ThingGraphProvider.Save();
                }
                
                
                try {
                    provider.SaveAs(this.ThingGraphProvider.Data, fileName);
                    this.ThingGraphProvider.Close ();
                    this.ThingGraphProvider = provider;
                    
                    ISceneProvider handler = new SceneProvider();
                    handler.DataBound = this.DataBound;
                    handler.Provider = provider;
                    handler.Open (()=> { });
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

        public bool OpenCommandLine() {
            var args = Environment.GetCommandLineArgs();
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

        public void ShowEmptyThingGraph() {
            ISceneProvider handler = new SceneProvider();
            IThingGraphProvider provider = new MemoryThingGraphProvider();
            this.ThingGraphProvider.Close();
            this.ThingGraphProvider = provider;
            handler.Provider = provider;
            handler.DataBound = this.DataBound;
            handler.Open();

            DataPostProcess("unknown");
        }

        public bool HasUnsavedData() {
            if (_thingGraphProvider is MemoryThingGraphProvider) {
                if (this.ThingGraphProvider.Data.Count > 0) {
                    return true;
                }
            }
            return false;
        }

        

        public bool IsSceneExportable(Scene scene) {
            if (scene != null) {
                var graph = GraphPairExtension<IVisual, IVisualEdge>.Source<IThing, ILink> (scene.Graph);

                return graph != null;
            }
            return false;
        }

        public void ExportAs(DataBaseInfo fileName, Scene scene) {
            var graph = GraphPairExtension<IVisual, IVisualEdge>
                .Source<IThing, ILink>(scene.Graph);

            if (graph != null) {
                var handler = new SceneProvider();
                handler.Provider = GetProvider(fileName);
                handler.ExportAs (scene, fileName);
                handler.Provider.Close ();
            }
        }

        public void Save() {
            if (this.HasUnsavedData()) {
                SaveAsFile();
            } else {
                if (_thingGraphProvider != null && _thingGraphProvider.Saveable) {
                    _thingGraphProvider.Save ();
                }
            }
        }

       

        public FileDialogMemento OpenFileDialog { get; set; }
        public FileDialogMemento SaveFileDialog { get; set; }

        public Func<string, string, MessageBoxButtons, DialogResult> MessageBoxShow { get; set; }
        public Func<FileDialogMemento, bool, DialogResult> FileDialogShow { get; set; }

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

        public void ImportRawFile() {
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
                            var targetProvider = GetProvider(DataBaseInfo.FromFileName(target));
                            var sourceProvider = GetProvider(DataBaseInfo.FromFileName(source));

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

        public void DefaultDialogValues(FileDialogMemento dialog) {
            dialog.Filter = this.FileProviderFilter + "All Files|*.*";
            dialog.DefaultExt = "limo";
            dialog.AddExtension = true;
            dialog.CheckFileExists = false;
            dialog.CheckPathExists = true;
            dialog.AutoUpgradeEnabled = true;
            dialog.SupportMultiDottedExtensions = true;
            // Important! under windows this adds the fileextension!
            dialog.ValidateNames = true;

        }
       
		public void SaveAsFile() {
            DefaultDialogValues (SaveFileDialog);
            if (FileDialogShow(SaveFileDialog, true) == DialogResult.OK) {
                this.SaveAs(DataBaseInfo.FromFileName(SaveFileDialog.FileName));
            }
        }

        public void ExportAsFile(Scene scene) {
            DefaultDialogValues(SaveFileDialog);
            if (scene != null && this.IsSceneExportable(scene)) {

                SaveFileDialog.Filter = this.FileProviderFilter + "All Files|*.*";
                SaveFileDialog.DefaultExt = "limo";

                if (FileDialogShow(SaveFileDialog, true) == DialogResult.OK) {
                    this.ExportAs(
                        DataBaseInfo.FromFileName(SaveFileDialog.FileName),
                        scene);
                }
            }
        }
		
		
	    public bool DocumentHasPages(Scene scene) {
            var graph = scene.Graph;
            var document = scene.Focused;
            var documentSchemaManager = new DocumentSchemaManager();

            return documentSchemaManager.HasPages(graph, document);

        }

        public void ExportPages(string dir, Scene scene) {
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
                                  StreamTypes.Extension(streamThing.StreamType);

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
    }
}