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
using Limaki.View;
using Limaki.View.Visuals;
using Limaki.View.Viz;

namespace Limada.View.Vidgets {

    public class SceneStream {
        public long Id = 0;
        public Stream Stream = null;
    }

    public class VisualsDisplayHistory {
        
        private History<long> _history = null;
        protected History<long> History {
            get { return _history ?? (_history = new History<long> ()); }
        }


        public bool CanGoForward () {
            return History.HasForward ();
        }

        public bool CanGoBack () {
            return History.HasBack ();
        }

        public void Clear () {
            _history = null;
        }

        public void Store (SceneInfo info) {
            History.Add (info.Id);
        }

        public void Store (IGraphSceneDisplay<IVisual, IVisualEdge> display, ISceneManager sceneManager) {
            sceneManager.Store (display);
            History.Add (display.DataId);
        }


        protected void Load (IGraphSceneDisplay<IVisual, IVisualEdge> display, ISceneManager sceneManager, Int64 id) {
            sceneManager.Load (display, id);
        }

        public void Navigate (IGraphSceneDisplay<IVisual, IVisualEdge> display, ISceneManager sceneManager, bool forward) {
            
            Store (display, sceneManager);

            var info = display.Info;
            var currSheedId = info != null ? info.Id : 0;

            var sheetId = forward ? History.Forward () : History.Back ();

            if (sheetId != currSheedId) {
                Load (display, sceneManager, sheetId);
            }

            if (currSheedId == 0)
                History.Remove (p => p == currSheedId);
            
        }

        public void SaveChanges (IEnumerable<IGraphSceneDisplay<IVisual, IVisualEdge>> displays, ISceneManager sceneManager, bool ask) {
            sceneManager.SaveChanges (displays, ask);
        }
    }
}