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

        protected SheetInfo Store(VisualsDisplay display, ISheetManager sheetManager, Id id) {
            var result = default(SheetInfo);
            if (display != null && display.Data != null && display.Data.Count>0) {
                if(id == default(Id))
                    id = Limada.Common.Isaac.Long;
                if (sheetManager.StoreInStreams(display.Data, display.Layout, id)) {
                    history.Add(id);
                    var info = sheetManager.RegisterSheet(id, display.Text);
                    display.State.CopyTo(info.State);
                    display.DataId = info.Id;
                    display.Text = info.Name;
                    return info;
                }
            }
            return result;
        }

        public SheetInfo Store(VisualsDisplay display, ISheetManager sheetManager, bool makeNew) {
            var result = Store(display, sheetManager, display.DataId);
            return result;
        }

        protected void Load(VisualsDisplay display, ISheetManager sheetManager, Id id) {
            if (id == 0)
                return;

            if (sheetManager.LoadFromStreams(display.Data, display.Layout, id)) {
                display.DataId = id;
                display.Viewport.Reset();
                display.DeviceRenderer.Render();
            }
        }

        public void Navigate(VisualsDisplay display, ISheetManager sheetManager, bool forward) {
            var info = Store(display, sheetManager, display.DataId);
            var currSheedId = default(Id);
            
            if(info!=null)
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

            info = sheetManager.GetSheetInfo(display.DataId);
            if (info != null)
                display.Text = info.Name;
        }

        public void SaveChanges(IEnumerable<IGraphSceneDisplay<IVisual, IVisualEdge>> displays, ISheetManager sheetManager, Func<string, string, MessageBoxButtons, DialogResult> MessageBoxShow) {
            IGraph<IVisual, IVisualEdge> graph = null;
            foreach (var display in displays)
                if (display.State.Dirty && !display.State.Hollow) {
                    var info = sheetManager.GetSheetInfo(display.DataId) ?? new SheetInfo { Id = display.DataId };
                    display.State.CopyTo(info.State);
                    sheetManager.StoreInStreams(display.Data, display.Layout, info.Id);
                    if (graph == null)
                        graph = display.Data.Graph;
                }

            Action<SheetInfo> sheetVisitor = (info) => {
                if (info.State.Dirty && !info.State.Hollow) {
                    if (MessageBoxShow("This sheet has been changed. Do you want to save it?", "Sheet " + info.Name, MessageBoxButtons.YesNo) == DialogResult.Yes) {
                        var sheet = sheetManager.GetFromStreams(info.Id);
                        if (sheet != null) {
                            sheetManager.SaveStreamInGraph(sheet, graph, info);
                            var display = displays.FirstOrDefault(d => d.DataId == info.Id);
                            if (display!=null)
                                info.State.CopyTo(display.State);
                        }
                    }
                }
            };
            sheetManager.VisitRegisteredSheets(sheetVisitor);
        }

    }
}