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
using Limaki.Widgets;
using Limada.App;
using Limaki.Model.Streams;

namespace Limaki.App {
    public class FileManager {
        IThingGraphProvider _thingGraphProvider = null;
        public IThingGraphProvider ThingGraphProvider {
            get {
                if (_thingGraphProvider == null) {
                    _thingGraphProvider = new MemoryThingGraphProvider();
                }
                return _thingGraphProvider;
            }
            set { _thingGraphProvider = value; }
        }

        public void Close() {
            if (_thingGraphProvider != null) {
                _thingGraphProvider.Close();
            }           
        }

        public Action<Scene> DataBound = null;
        public Action<string> DataPostProcess = null;

        IThingGraphProvider GetProvider(DataBaseInfo info) {
            var providers = Registry.Pool.TryGetCreate<DataProviders<IThingGraph>>();
            return providers.Find(info) as IThingGraphProvider;
        }

        string _fileProviderFilter = null;
        public string FileProviderFilter {
            get {
                if (_fileProviderFilter == null) {
                    var providers = Registry.Pool.TryGetCreate<DataProviders<IThingGraph>>();
                    foreach(var provider in providers) {
                        if (provider.Saveable) {
                            _fileProviderFilter +=
                                provider.Description + "|*" + provider.Extension+"|";
                        }
                    }
                }
                return _fileProviderFilter;
            }
        }

        public bool OpenFile(DataBaseInfo fileName) {
            ISceneProvider handler = new SceneProvider();
            IThingGraphProvider provider = GetProvider(fileName);
            bool result = false;
            if (provider != null) {
                handler.Provider = provider;
                handler.DataBound = this.DataBound;
                this.ThingGraphProvider.Close();
                if (handler.Open(fileName)) {
                    this.ThingGraphProvider = provider;
                    DataPostProcess(fileName.Name);
                    result = true;
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

        public bool SaveAs(DataBaseInfo fileName) {

            ISceneProvider handler = new SceneProvider();
            IThingGraphProvider provider = GetProvider(fileName);
            bool result = false;
            if (provider != null) {
                this.ThingGraphProvider.Save();
                provider.Data = this.ThingGraphProvider.Data;
                handler.Provider = provider;
                handler.DataBound = this.DataBound;
                if (handler.SaveAs(fileName)) {
                    this.ThingGraphProvider.Close();
                    this.ThingGraphProvider = provider;
                    DataPostProcess(fileName.Name);
                    result = true;
                } else {
                    provider.Data = null;
                    provider.Close ();
                }

            }
            return result;
        }

        public bool IsSceneExportable(Scene scene) {
            if (scene != null) {
                var graph = new GraphPairFacade<IWidget, IEdgeWidget> ()
                    .Source<IThing, ILink> (scene.Graph);

                return graph != null;
            }
            return false;
        }

        public void ExportAs(DataBaseInfo fileName, Scene scene) {
            var graph = new GraphPairFacade<IWidget, IEdgeWidget>()
                .Source<IThing, ILink>(scene.Graph);

            if (graph != null) {
                var handler = new SceneProvider();
                handler.Provider = GetProvider(fileName);
                handler.ExportAs (scene, fileName);
                handler.Provider.Close ();
            }
        }

        public void Save() {
            if (_thingGraphProvider != null) {
                _thingGraphProvider.Save();
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