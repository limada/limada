/*
 * Limada 
 * 
 * This code is free software; you can redistribute it and/or modify it
 * under the terms of the GNU General Public License version 2 only, as
 * published by the Free Software Foundation.
 * 
 * Author: Lytico
 * Copyright (C) 2006-2017 Lytico
 *
 * http://www.limada.org
 * 
 */

using System;
using System.Collections.Generic;
using System.IO;
using Limada.View.VisualThings;
using Limaki.Common;
using Limaki.Contents;
using Limaki.Graphs;
using Limaki.View;
using Limaki.View.GraphScene;
using Limaki.View.Vidgets;
using Limaki.View.Visuals;
using Limaki.View.Viz;
using Limaki.View.Viz.Mapping;
using System.Linq;
using System.Diagnostics;
using Limaki.View.Common;
using Xwt.Drawing;

namespace Limaki.View.Visuals {

    public static class VisualSceneStoreExtensions {

        static IGraphSceneMapDisplayOrganizer<IVisual, IVisualEdge>  _organizer = null;
        static IGraphSceneMapDisplayOrganizer<IVisual, IVisualEdge> Organizer { get { return _organizer ?? (_organizer = Registry.Pooled<IGraphSceneMapDisplayOrganizer<IVisual, IVisualEdge>> ()); } }

        public static long IdOf (this IVisualSceneStoreInteractor interactor, Content<Stream> content) {
            return content.Source is Int64 ? (Int64)content.Source : 0;
        }

        public static void Store (this IVisualSceneStoreInteractor interactor, IGraphSceneDisplay<IVisual, IVisualEdge> display) {
            if (display != null && display.Data != null && display.Data.Count > 0) {
                if (display.DataId == 0)
                    display.DataId = Isaac.Long;

                display.Perform ();
                if (interactor.SaveInStore (display.Data, display.Layout, display.DataId)) {
                    interactor.SheetStore.RegisterSceneInfo (display.Info);
                }
            }
        }

        public static bool IsSceneOpen (this IVisualSceneStoreInteractor interactor, long id, bool showMessage = false, bool activateOpenDisplay = false) {

            var display = Organizer.Displays.Where (d => d.DataId == id).FirstOrDefault ();
            if (display == null)
                return false;
            var result = true;
            try {
                if (activateOpenDisplay) {
                    display.Wink ();
                }

                if (showMessage) {
                    var name = interactor.SheetStore.GetSheetInfo (id)?.Name;
                    Xwt.Application.MainLoop.QueueExitAction (() => {
                        Registry.Factory.Create<IMessageBoxShow> ().Show ("Warning", $"Sheet {name ?? ""} is already in use", MessageBoxButtons.None);
                    });
                }
            } finally {
                
            }
            return result;

        }

        public static bool DoLoadDirtyScene (this IVisualSceneStoreInteractor interactor, long id, bool showMessage = false) {

            if (id == 0)
                return false;
            
            var stored = interactor.SheetStore.GetSheetInfo (id);
            var dirty = stored != null && !stored.State.Clean;
            if (dirty && showMessage) {
                var dialog = Registry.Factory.Create<IMessageBoxShow> ();
                if (dialog.Show ("Warning",
                    $"You try to load the a changed sheet again.\r\nYou'll lose all changes on sheet {stored.Name}",
                    MessageBoxButtons.OkCancel)
                    == DialogResult.Cancel) {
                    dirty = false;
                }
            }

            return !dirty;
        }

        public static void AssignScene (this IGraphSceneDisplay<IVisual, IVisualEdge> display, IGraphScene<IVisual, IVisualEdge> scene, SceneInfo info) {
            if (scene == null || display == null || scene == display.Data)
                return;
            Trace.WriteLine ($"{nameof (VisualSceneStoreExtensions)}.{nameof (AssignScene)}:{display.Info} => {info}");

            Organizer.RemoveScene (display.Data);
            Organizer.AddScene (scene);

            display.Data = scene;
            display.Info = info;
            display.Data.CreateMarkers ();
            display.QueueDraw ();
        }

        public static void AssignScene (this IVisualSceneStoreInteractor interactor, IGraphSceneDisplay<IVisual, IVisualEdge> display, IGraphScene<IVisual, IVisualEdge> scene, SceneInfo info) {
            if (display == null || scene == null)
                return;

            AssignScene (display, scene, info);
            interactor.SheetStore.RegisterSceneInfo (info);
        }

        public static void Load (this IVisualSceneStoreInteractor interactor, IGraphSceneDisplay<IVisual, IVisualEdge> display, Int64 id, bool checkOpen = true) {
            
            if (id == 0)
                return;

            if (checkOpen && interactor.IsSceneOpen (id, true, true))
                return;
            
            var scene = interactor.Load (display.Data.Graph, display.Layout, id);
            var info = interactor.SheetStore.GetSheetInfo (id);

            display.AssignScene (scene, info);
        }

        public static void Load (this IVisualSceneStoreInteractor interactor, IGraphSceneDisplay<IVisual, IVisualEdge> display, Content<Stream> content, bool checkOpen = true) {

            if (content == null)
                return;

            var id = interactor.IdOf (content);

            if (checkOpen && interactor.IsSceneOpen (id, true, true))
                return;
            
            var scene = interactor.LoadFromContent (content, display.Data.Graph, display.Layout);
            var info = interactor.SheetStore.GetSheetInfo (id);

            display.AssignScene (scene, info);
        }

        public static void SaveChanges (this IVisualSceneStoreInteractor interactor, IEnumerable<IGraphSceneDisplay<IVisual, IVisualEdge>> displays, bool ask) {

            // TODO: use mesh instead of displays

            IGraph<IVisual, IVisualEdge> graph = null;

            foreach (var display in displays) {
                if (graph == null)
                    graph = display.Data.Graph;
                
                if (display.State.Dirty && !display.State.Hollow && display.Data != null) {
                    var info = interactor.SheetStore.GetSheetInfo (display.DataId) ?? display.Info;
                    interactor.SaveInStore (display.Data, display.Layout, info.Id);
                    display.State.CopyTo (info.State);
                }
            }

            Action<SceneInfo> sheetVisitor = (info) => {
                if (info.State.Dirty && !info.State.Hollow) {
                    var saveSheet = !ask || Registry.Pooled<IMessageBoxShow> ().Show ("This sheet has been changed. Do you want to save it?", "Sheet " + info.Name, MessageBoxButtons.YesNo) == DialogResult.Yes;
                    if (saveSheet) {
                        var sheet = interactor.StreamFromStore (info.Id);
                        if (sheet != null) {
                            interactor.SaveStreamInGraph (sheet, graph, info);
                            var display = displays.FirstOrDefault (d => d.DataId == info.Id);
                            if (display != null)
                                info.State.CopyTo (display.State);
                        }
                    }
                }
            };

            if (graph != null)
                interactor.SheetStore.VisitRegisteredSheetInfos (sheetVisitor);
        }
    }
}
