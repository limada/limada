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
using Limaki.View;
using Limaki.View.Visuals;
using Limaki.View.Viz;

namespace Limaki.Usecases.Vidgets {

    public interface ISplitView {

        IGraphSceneDisplay<IVisual, IVisualEdge> CurrentDisplay { get; }

        IGraphSceneDisplay<IVisual, IVisualEdge> Display1 { get; }
        IGraphSceneDisplay<IVisual, IVisualEdge> Display2 { get; }

        SplitViewMode ViewMode { get; set; }
        void ToggleView();

        bool CanGoBackOrForward(bool forward);
        void GoBackOrForward(bool forward);
        void GoHome();

        void Search (string name);
        void DoSearch ();

        void NewSheet();
        void NewNote();
        void SaveDocument();

        event EventHandler ViewChanged;

        void ChangeData (IGraphScene<IVisual, IVisualEdge> scene);

        void LoadSheet(SceneInfo sceneInfo);

        IGraphSceneDisplay<IVisual, IVisualEdge> AdjacentDisplay(IGraphSceneDisplay<IVisual, IVisualEdge> display);

        /// <summary>
        /// shows a new IGraph
        /// SceneDisplay{Visual, IVisualEdge} in a new Window
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
        GraphContent
    }
}