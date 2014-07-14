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
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Limaki.Common;
using Limaki.Common.Collections;
using Limaki.Graphs;
using Limaki.View;
using Limaki.View.Vidgets;
using Limaki.View.Visuals;
using Limaki.View.Viz;
using Limaki.View.Viz.Visualizers;

namespace Limada.View.Vidgets {

    public class SceneStream {
        public Int64 Id = 0;
        public Stream Stream = null;
    }

    public class VisualsDisplayHistory {
        private History<long> _history = null;
        protected History<long> History {
            get {
                if (_history == null) {
                    _history = new History<long>();
                }
                return _history;
            }
        }


        public bool CanGoForward() {
            return History.HasForward();
        }

        public bool CanGoBack() {
            return History.HasBack();
        }

        public void Clear() {
            _history = null;
        }

        public void Store(SceneInfo info) {
            History.Add(info.Id);
        }

        public void Store(IGraphSceneDisplay<IVisual, IVisualEdge> display, ISheetManager sheetManager) {
            if (display != null && display.Data != null && display.Data.Count > 0) {
                if (display.DataId == default(Int64))
                    display.DataId = Isaac.Long;
                if (sheetManager.SaveInStore(display.Data, display.Layout, display.DataId)) {
                    sheetManager.RegisterSheet(display.Info);
                    History.Add(display.DataId);
                }
            }
        }
        
        protected void Load(IGraphSceneDisplay<IVisual, IVisualEdge> display, ISheetManager sheetManager, Int64 id) {
            if (id == 0)
                return;
            var info = sheetManager.GetSheetInfo(id);
            if (info != null) {
                if (sheetManager.LoadFromStore(display.Data, display.Layout, info.Id)) {
                    display.Info = info;
                    display.Viewport.Reset();
                    display.BackendRenderer.Render();
                }
            }
        }

        public void Navigate(IGraphSceneDisplay<IVisual, IVisualEdge> display, ISheetManager sheetManager, bool forward) {
            var info = display.Info;
            Store(display, sheetManager);
            var currSheedId = default(Int64);

            if (info != null)
                currSheedId = info.Id;

            Int64 sheetId = default(Int64);
            if (forward)
                sheetId = History.Forward();
            else
                sheetId = History.Back();

            if (sheetId != default(Int64) && sheetId != currSheedId) {
                Load(display, sheetManager, sheetId);
            }

            if (currSheedId == default(Int64))
                History.Remove(p => p == currSheedId);

            
        }

        public void SaveChanges(IEnumerable<IGraphSceneDisplay<IVisual, IVisualEdge>> displays, ISheetManager sheetManager, Func<string, string, MessageBoxButtons, DialogResult> MessageBoxShow) {
            IGraph<IVisual, IVisualEdge> graph = null;
            foreach (var display in displays) {
                if (graph == null)
                    graph = display.Data.Graph;
                if (display.State.Dirty && !display.State.Hollow && display.Data != null) {
                    var info = sheetManager.GetSheetInfo(display.DataId) ?? display.Info;
                    sheetManager.SaveInStore(display.Data, display.Layout, info.Id);
                    display.State.CopyTo(info.State);

                }
            }

            Action<SceneInfo> sheetVisitor = (info) => {
                if (info.State.Dirty && !info.State.Hollow) {
                    if (MessageBoxShow("This sheet has been changed. Do you want to save it?", "Sheet " + info.Name, MessageBoxButtons.YesNo) == DialogResult.Yes) {
                        var sheet = sheetManager.GetFromStore(info.Id);
                        if (sheet != null) {
                            sheetManager.SaveStreamInGraph(sheet, graph, info);
                            var display = displays.FirstOrDefault(d => d.DataId == info.Id);
                            if (display != null)
                                info.State.CopyTo(display.State);
                        }
                    }
                }
            };

            if (graph != null)
                sheetManager.VisitRegisteredSheets(sheetVisitor);
        }

    }
}