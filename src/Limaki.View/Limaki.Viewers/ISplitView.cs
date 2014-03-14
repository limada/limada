/*
 * Limaki 
 * 
 * This code is free software; you can redistribute it and/or modify it
 * under the terms of the GNU General Public License version 2 only, as
 * published by the Free Software Foundation.
 * 
 * Author: Lytico
 * Copyright (C) 2006-2010 Lytico
 *
 * http://www.limada.org
 * 
 */

using System;
using Limaki.Drawing;
using Limaki.View.Visualizers;
using Limaki.Visuals;

namespace Limaki.Viewers {

    public interface ISplitView {

        SplitViewMode ViewMode { get; set; }
        void ToggleView();

        bool CanGoBackOrForward(bool forward);
        void GoBackOrForward(bool forward);
        void GoHome();

        void NewSheet();
        void NewNote();
        void SaveDocument();

        event EventHandler ViewChanged;

        void LoadSheet(SceneInfo sceneInfo);

        IGraphSceneDisplay<IVisual, IVisualEdge> AdjacentDisplay(IGraphSceneDisplay<IVisual, IVisualEdge> display);

        /// <summary>
        /// shows a new IGraphSceneDisplay{Visual, IVisualEdge} in a new Window
        /// 
        /// </summary>
        void ShowInNewWindow ();
    }

    public enum SplitViewMode {
        /// <summary>
        /// shows two VisualsDisplays
        /// </summary>
        GraphGraph,
        /// <summary>
        /// shows one VisualsDisplay and one StreamViewer
        /// </summary>
        GraphStream
    }
}