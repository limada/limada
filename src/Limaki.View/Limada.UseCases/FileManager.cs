/*
 * Limada 
 * 
 * This code is free software; you can redistribute it and/or modify it
 * under the terms of the GNU General Public License version 2 only, as
 * published by the Free Software Foundation.
 * 
 * Author: Lytico
 * Copyright (C) 2006-2013 Lytico
 *
 * http://www.limada.org
 */

using Limada.Data;
using Limada.VisualThings;
using Limaki.Common;
using Limaki.Data;
using Limaki.Drawing;
using Limaki.Viewers;
using Limaki.Visuals;
using Mono.Options;
using System;
using System.Collections.Generic;
using System.IO;

namespace Limada.Usecases {

    //TODO: refactor to ThingGraphUiManager and remove
    
    public class FileManager : FileManagerBase, IFileManager {

        public Action<IGraphScene<IVisual, IVisualEdge>> DataBound { get; set; }
        public Action<string> DataPostProcess { get; set; }
        public Action ApplicationQuit { get; set; }

        #region ThingGraph Open 

        //done
        public override bool OpenFile(IoInfo fileName) {
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

        //done
        public void OpenFile() {
            if (this.HasUnsavedData()) {
                if (MessageBoxShow("You have an unsaved document. Do you want to save it?", "", MessageBoxButtons.YesNo) ==
                    DialogResult.Yes) {
                    SaveAsFile();
                }
            }

            DefaultDialogValues(OpenFileDialog);
            if (FileDialogShow(OpenFileDialog, true) == DialogResult.OK) {
                this.OpenFile(IoInfo.FromFileName(OpenFileDialog.FileName));
            }
        }

        //done
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
                            var db = IoInfo.FromFileName(fileToAdd);
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
       
        //done
        public bool OpenCommandLine (string [] args) {

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
        //done
	    public bool OpenCommandLine() {
            return OpenCommandLine(Environment.GetCommandLineArgs());
        }
        //done
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

        //done
        public bool SaveAs(IoInfo fileName) {

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

                    var sceneProvider = new SceneProvider();
                    sceneProvider.DataBound = this.DataBound;
                    sceneProvider.Provider = provider;
                    sceneProvider.Open(() => { });
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
        //done
        public void Save() {
            if (this.HasUnsavedData()) {
                SaveAsFile();
            } else {
                if (_thingGraphProvider != null && _thingGraphProvider.Saveable) {
                    _thingGraphProvider.Save();
                }
            }
        }

        //done
        public void SaveAsFile() {
            DefaultDialogValues(SaveFileDialog);
            if (FileDialogShow(SaveFileDialog, false) == DialogResult.OK) {
                this.SaveAs(IoInfo.FromFileName(SaveFileDialog.FileName));
            }
        }

        //done
        public bool HasUnsavedData() {
            if (_thingGraphProvider is MemoryThingGraphProvider) {
                if (this.ThingGraphProvider.Data.Count > 0) {
                    return true;
                }
            }
            return false;
        }

        //done
        public void ExportAsThingGraph(IoInfo fileName, IGraphScene<IVisual, IVisualEdge> scene) {
            if (scene.HasThingGraph()) {
                var provider = new SceneProvider();
                provider.Provider = GetThingGraphProvider(fileName);
                provider.ExportAsThingGraph (scene, fileName);
                provider.Provider.Close ();
            }
        }

        //done
        public void ExportAsThingGraph(IGraphScene<IVisual, IVisualEdge> scene) {
            DefaultDialogValues(SaveFileDialog);
            if (scene != null && scene.HasThingGraph()) {
                if (FileDialogShow(SaveFileDialog, false) == DialogResult.OK) {
                    this.ExportAsThingGraph(
                        IoInfo.FromFileName(SaveFileDialog.FileName),
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
            dialog.Filter = ThingGraphProviderManager.SaveFilter + "All Files|*.*";
            dialog.DefaultExt = ThingGraphProviderManager.DefaultExtension;
            dialog.AddExtension = true;
            dialog.CheckFileExists = false;
            dialog.CheckPathExists = true;
            dialog.AutoUpgradeEnabled = true;
            dialog.SupportMultiDottedExtensions = true;
            // Important! under windows this adds the fileextension!
            dialog.ValidateNames = true;

        }

        #endregion

        #region Import
        //done
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
                            var targetProvider = GetThingGraphProvider(IoInfo.FromFileName(target));
                            var sourceProvider = GetThingGraphProvider(IoInfo.FromFileName(source));

                            try {
                                targetProvider.Open(IoInfo.FromFileName(target));
                                sourceProvider.RawImport(IoInfo.FromFileName(source), targetProvider);
                            } catch (Exception ex) {
                                Registry.Pool.TryGetCreate<IExceptionHandler>()
                                    .Catch(new Exception("Raw import failed: " + ex.Message, ex), MessageType.OK);
                                targetProvider.Data = null;
                                targetProvider.Close();
                                File.Delete(target);
                            }
                            targetProvider.Close();
                            MessageBoxShow("Import successfull", "RawImport", MessageBoxButtons.OK);
                            this.OpenFile(IoInfo.FromFileName(target));
                        }
                    }
                }
            }

        }
        
        #endregion 

        #region Export

       
	   

	    #endregion
    }
}