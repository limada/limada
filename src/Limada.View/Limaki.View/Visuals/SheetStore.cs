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
using Limaki.Common;

namespace Limaki.View.Visuals {

    public class SheetStore {
        
        private IDictionary<Int64, SceneInfo> _sceneInfos = null;
        /// <summary>
        /// names to thing.ids of loaded and saved sheets
        /// </summary>
        protected IDictionary<Int64, SceneInfo> SceneInfos {
            get { return _sceneInfos ?? (_sceneInfos = new Dictionary<Int64, SceneInfo> ()); }
            set { _sceneInfos = value; }
        }

        private IDictionary<long, byte []> _sheetsStreams = null;

        /// <summary>
        /// streams of sheets
        /// </summary>
        protected IDictionary<long, byte []> SheetStreams {
            get { return _sheetsStreams ?? (_sheetsStreams = new Dictionary<long, byte []> ()); }
            set { _sheetsStreams = value; }
        }

        public Action<SceneInfo> SceneInfoRegistered { get; set; }

        public void RegisterSceneInfo (SceneInfo info) {
            if (info.Id == 0) {
                info.Id = Isaac.Long;
            }
            SceneInfos [info.Id] = info;
        }

        public SceneInfo RegisterSceneInfo (Int64 id, string name) {
            var result = default (SceneInfo);
            if (id != 0 && SceneInfos.ContainsKey (id)) {
                result = SceneInfos [id];
                if (!string.IsNullOrEmpty (name))
                    result.Name = name;
            } else {
                if (id == 0) {
                    id = Isaac.Long;
                }
                result = new SceneInfo { Id = id, Name = name };
                result.State.Hollow = true;
                SceneInfos [id] = result;
                SceneInfoRegistered?.Invoke (result);
            }

            return result;
        }

        public SceneInfo GetSheetInfo (Int64 id) {
            var result = default (SceneInfo);
            SceneInfos.TryGetValue (id, out result);
            return result;
        }

        public SceneInfo CreateSceneInfo (string name = "") {
            var result = new SceneInfo { Id = Isaac.Long, Name = name };
            result.State.Hollow = true;
            return result;
        }

        public bool TryGetValue (long id, out byte [] buffer) {
            return SheetStreams.TryGetValue (id, out buffer);
        }

        public virtual void VisitRegisteredSheetInfos (Action<SceneInfo> visitor) {
            foreach (var sheetInfo in SceneInfos.Values) {
                visitor (sheetInfo);
            }
        }

        public void Add (long id, byte [] buffer) {
            SheetStreams [id] = buffer;
        }

        public void Remove (long id) {
            SheetStreams.Remove (id);
        }
        public void Clear () {
            SceneInfos = null;
            SheetStreams = null;
        }

        public bool Contains (long id) {
            return SceneInfos.ContainsKey (id);
        }
	}

}