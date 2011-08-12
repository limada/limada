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
 * http://limada.sourceforge.net
 */

using System.IO;
using Limaki.Common.Collections;
using Limaki.Presenter.Visuals;
using Limaki.Visuals;
using Id = System.Int64;
using System;
using System.Collections.Generic;
using Limaki.Graphs;
using System.Linq;
using Limaki.UseCases.Viewers;
using Limaki.Presenter.Display;
using Limaki.Drawing;

namespace Limada.Presenter {
    public class SceneStream {
        public Id Id = 0;
        public Stream Stream = null;
    }

    public class SceneHistory {
        private History<Id> _history = null;
        protected History<Id> history {
            get {
                if (_history == null) {
                    _history = new History<Id>();
                }
                return _history;
            }
        }


        public bool CanGoForward() {
            return history.HasForward();
        }

        public bool CanGoBack() {
            return history.HasBack();
        }

        public void Clear() {
            _history = null;
        }

        public void Store(SceneInfo info) {
            history.Add(info.Id);
        }

        public void Store(IGraphSceneDisplay<IVisual, IVisualEdge> display, ISheetManager sheetManager) {
            if (display != null && display.Data != null && display.Data.Count > 0) {
                if (display.DataId == default(Id))
                    display.DataId = Limada.Common.Isaac.Long;
                if (sheetManager.SaveInStore(display.Data, display.Layout, display.DataId)) {
                    sheetManager.RegisterSheet(display.Info);
                    history.Add(display.DataId);
                }
            }
        }

        

        protected void Load(IGraphSceneDisplay<IVisual, IVisualEdge> display, ISheetManager sheetManager, Id id) {
            if (id == 0)
                return;
            var info = sheetManager.GetSheetInfo(id);
            if (info != null) {
                if (sheetManager.LoadFromStore(display.Data, display.Layout, info.Id)) {
                    display.Info = info;
                    display.Viewport.Reset();
                    display.DeviceRenderer.Render();
                }
            }
        }

        public void Navigate(IGraphSceneDisplay<IVisual, IVisualEdge> display, ISheetManager sheetManager, bool forward) {
            var info = display.Info;
            Store(display, sheetManager);
            var currSheedId = default(Id);

            if (info != null)
                currSheedId = info.Id;

            Id sheetId = default(Id);
            if (forward)
                sheetId = history.Forward();
            else
                sheetId = history.Back();

            if (sheetId != default(Id) && sheetId != currSheedId) {
                Load(display, sheetManager, sheetId);
            }

            if (currSheedId == default(Id))
                history.Remove(p => p == currSheedId);

            
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