/*
 * Limaki 
 * 
 * This code is free software; you can redistribute it and/or modify it
 * under the terms of the GNU General Public License version 2 only, as
 * published by the Free Software Foundation.
 * 
 * Author: Lytico
 * Copyright (C) 2006-2015 Lytico
 *
 * http://www.limada.org
 * 
 */


using System;
using System.Diagnostics;
using Limaki.Common;
using Limaki.Graphs;
using Limaki.View.GraphScene;
using Limaki.View.Vidgets;
using Limaki.View.Visuals;
using Limaki.View.Viz.Rendering;
using Xwt;

namespace Limaki.View.Viz.UI.GraphScene {

    public class GraphSceneFolding<TItem, TEdge> : ICheckable, IGraphSceneFolding<TItem, TEdge>
        where TItem : class
        where TEdge : TItem, IEdge<TItem> {

        public GraphSceneFolding () : base () { }

        public IBackendRenderer BackendRenderer { get; set; }
        public virtual ISelectionRenderer MoveResizeRenderer { get; set; }

        public Func<IGraphSceneLayout<TItem, TEdge>> Layout { get; set; }
        public Func<IGraphScene<TItem, TEdge>> SceneHandler { get; set; }

        private GraphSceneFacade<TItem, TEdge> _folder = null;

        public virtual GraphSceneFacade<TItem, TEdge> Folder {
            get {
                if (_folder == null) {
                    _folder = new GraphSceneFacade<TItem, TEdge> (this.SceneHandler, Layout ());
                }
                _folder.Layout = Layout ();
                _folder.OrderBy = this.OrderBy;
                _folder.RemoveOrphans = this.RemoveOrphans;
                return _folder;
            }
            set { _folder = value; }
        }

        public DataComparer<TItem> OrderBy { get; set; }

        public bool RemoveOrphans { get; set; }

        public virtual void Clear () {
            Folder.Clear ();
        }

        public virtual void Hide () {
            var wasFiltered = Folder.IsFiltered;
            if (MoveResizeRenderer != null) {
                MoveResizeRenderer.Shape = null;
                MoveResizeRenderer.ShowGrips = false;
            }
            Folder.Hide ();
            if (wasFiltered != Folder.IsFiltered)
                BackendRenderer.Render ();
        }

        public virtual void Delete () {
            Folder.Delete ();
            if (MoveResizeRenderer != null) {
                MoveResizeRenderer.Shape = null;
                MoveResizeRenderer.ShowGrips = false;
            }
        }

        public virtual void Fold (Action fold) {

            var wasFiltered = Folder.IsFiltered;
            var act = !(Folder.Scene.Focused is TEdge); // currently folding on edges is not supported
            if (act) {
                fold ();
                if (wasFiltered != Folder.IsFiltered)
                    BackendRenderer.Render ();
            }
        }

        #region ICheckable Member

        public bool Check () {
            if (this.SceneHandler == null) {
                throw new CheckFailedException (this.GetType (), typeof (IGraphScene<TItem, TEdge>));
            }
            if (this.Layout == null) {
                throw new CheckFailedException (this.GetType (), typeof (IGraphSceneLayout<TItem, TEdge>));
            }
            if (this.BackendRenderer == null) {
                throw new CheckFailedException (this.GetType (), typeof (IBackendRenderer));
            }
            return true;

        }

        #endregion
    }

    public interface IGraphSceneFolding<TItem, TEdge> 
        where TItem : class
        where TEdge : TItem, IEdge<TItem> {

        IBackendRenderer BackendRenderer { get; set; }
        ISelectionRenderer MoveResizeRenderer { get; set; }
        Func<IGraphSceneLayout<TItem, TEdge>> Layout { get; set; }
        Func<IGraphScene<TItem, TEdge>> SceneHandler { get; set; }
        GraphSceneFacade<TItem, TEdge> Folder { get; set; }
        DataComparer<TItem> OrderBy { get; set; }
        bool RemoveOrphans { get; set; }
        void Clear ();
        bool Check ();
        void Fold (Action fold);
        void Hide ();
        void Delete ();
    }

    public interface IGraphSceneKeyFolding<TItem, TEdge>: IKeyAction
        where TItem : class
        where TEdge : TItem, IEdge<TItem> {

        IGraphSceneFolding<TItem, TEdge> Folding { get; }
    }

    public class GraphSceneKeyFolding<TItem, TEdge> : KeyActionBase, ICheckable
        where TItem : class
        where TEdge : TItem, IEdge<TItem> {

        public IGraphSceneFolding<TItem, TEdge> Folding { get; protected set; }

        public GraphSceneKeyFolding (IGraphSceneFolding<TItem, TEdge> folding) : base () {
            this.Folding = folding;
        }

        public override void OnKeyPressed (KeyActionEventArgs e) {

            Trace.WriteLine (string.Format ("folding key {0} {1}", e.Key, e.Modifiers));

            base.OnKeyPressed (e);

            var folder = Folding.Folder;
            Action<Action> fold = a => Folding.Fold (a);

            if (e.Key == Key.Delete && e.Modifiers == ModifierKeys.None) {
                Folding.Hide ();

            } else if (e.Key == Key.Delete && (e.Modifiers == ModifierKeys.Control)) {
                Folding.Delete ();

            } else if ((e.Key == Key.NumPadAdd || e.Key == Key.Plus) && e.Modifiers == ModifierKeys.None) {
                fold (() => folder.Expand (false));

            } else if ((e.Key == Key.NumPadSubtract || e.Key == Key.Minus) && e.Modifiers == ModifierKeys.None) {
                fold (() => folder.Collapse ());

            } else if ((e.Key == Key.NumPadDivide || e.Key == Key.Slash)) {
                fold (() => folder.CollapseToFocused ());

            } else if ((e.Key == Key.NumPadMultiply || e.Key == Key.Asterisk) && e.Modifiers == ModifierKeys.Control) {
                fold (() => folder.ShowAllData ());

            } else if (((e.Key == Key.NumPadMultiply || e.Key == Key.Asterisk) && e.Modifiers == ModifierKeys.None)
                       || (e.Key == Key.Plus && e.Modifiers == ModifierKeys.Shift)) {
                fold (() => folder.Expand (true));

            } else if ((e.Key == Key.Space && e.Modifiers == ModifierKeys.None)) {
                fold (() => folder.Toggle ());
            }

        }

        public bool Check () {
            return Folding != null && Folding.Check ();
        }
    }

    public interface IGraphSceneMouseFolding<TItem, TEdge> : IMouseAction
        where TItem : class
        where TEdge : TItem, IEdge<TItem> {

        IGraphSceneFolding<TItem, TEdge> Folding { get; }
    }

    public class GraphSceneMouseFolding<TItem, TEdge> : MouseDragActionBase, ICheckable
        where TItem : class
        where TEdge : TItem, IEdge<TItem> {

        public IGraphSceneFolding<TItem, TEdge> Folding { get; protected set; }

        public int HitSize { get; set; }

        public GraphSceneMouseFolding (IGraphSceneFolding<TItem, TEdge> folding)
            : base () {
            // lower than MoveResize
            Priority = ActionPriorities.SelectionPriority - 80;
            this.Behaviour = DragBehaviour.DoubleClick;
            this.Folding = folding;
        }

        protected override bool CheckDoubleClickHit (double x, double y) {
            var scene = Folding.SceneHandler ();
            return scene.Hit (new Point (x, y), HitSize) == scene.Focused;
        }

        protected override void EndAction () {
            if (Resolved) {
                Folding.Fold (() => Folding.Folder.Toggle ());
                LastMouseTime = 0;
            }
            base.EndAction ();
        }

        public bool Check () {
            return Folding != null && Folding.Check ();
        }

    }

    public static class UiFoldingExtensions {
        public static IGraphSceneFolding<TItem, TEdge> Folding<TItem, TEdge> (this IDisplay<IGraphScene<TItem, TEdge>> display)
            where TItem : class
            where TEdge : TItem, IEdge<TItem> {
            var uiAction = display.ActionDispatcher.GetAction<GraphSceneKeyFolding<TItem, TEdge>> ();
            if (uiAction != null)
                return uiAction.Folding;
            return null;
        }
    }
}