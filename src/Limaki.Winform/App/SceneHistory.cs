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
using Limada.View;
using Limaki.Common.Collections;
using Limaki.Widgets;
using Limaki.Winform.Displays;
using Id = System.Int64;

namespace Limaki.App {
    public class SceneHistory {
        public class SceneStream {
            public Id Id = 0;
            public Stream Stream = null;
        }

        private History<SceneStream> _history = null;
        protected History<SceneStream> history {
            get {
                if (_history == null) {
                    _history = new History<SceneStream>();
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
            foreach (var p in history) {
                p.Stream.Dispose();
            }
            _history = null;
        }

        protected SceneStream Save(WidgetDisplay display, Id id) {
            var sceneStream = history.GetItem(p => p.Id == id);
            if (display.Data.Count > 0) {
                if (sceneStream == null) {
                    sceneStream = new SceneStream();
                    sceneStream.Id = id;
                    history.Add(sceneStream);
                }

                var stream = new MemoryStream();
                new Sheet(display.Data, display.DataLayout).Save(stream);
                stream.Position = 0;

                sceneStream.Stream = stream;
                display.SceneId = sceneStream.Id;
            } else {
                if (sceneStream == null) 
                    sceneStream = new SceneStream();
                sceneStream.Stream = null;
            }
            return sceneStream;
        }

        public void Save(WidgetDisplay display, ISheetManager sheetManager, bool makeNew) {
            if (display != null && display.Data != null) {
                var id = sheetManager.RegisterSheet (display.SceneId, display.Text).Id;

                Save (display, id);
                if (makeNew) {
                    display.SceneId = 0;
                }
            }
        }

        protected void Load(WidgetDisplay display, SceneStream sceneStream) {
            if (sceneStream.Id == 0 || sceneStream.Stream == null)
                return;
            sceneStream.Stream.Position = 0;
            new SheetManager ().LoadSheet (display.Data, display.DataLayout, sceneStream.Stream);
            display.SceneId = sceneStream.Id;
            display.ResetScroll ();
            display.Invalidate ();

        }

        public void Navigate(WidgetDisplay display, ISheetManager sheetManager, bool forward) {
            var currSceneStream = Save(display, sheetManager.RegisterSheet(display.SceneId, display.Text).Id);

            SceneStream sceneStream = null;
            if (forward)
                sceneStream = history.Forward();
            else
                sceneStream = history.Back();

            if (sceneStream != null && sceneStream != currSceneStream) {
                Load (display, sceneStream);
            }

            if (currSceneStream.Stream == null)
                history.Remove(p => p.Id == currSceneStream.Id);

            var info = sheetManager.GetSheetInfo(display.SceneId);
            display.Text = info.Name;
        }
    }
}