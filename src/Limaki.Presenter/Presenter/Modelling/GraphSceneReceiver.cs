/*
 * Limaki 
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
 * 
 */


using System;
using Limaki.Actions;
using Limaki.Common;
using Limaki.Drawing;
using Limaki.Graphs;
using Limaki.Presenter.UI;

namespace Limaki.Presenter {
    /// <summary>
    /// modells the scene
    /// updates clipper
    /// uses a ModelReceiver 
    /// </summary>
    public class GraphSceneReceiver<TItem, TEdge> : ActionBase, ISceneReceiver<TItem, TEdge>
        where TEdge : TItem, IEdge<TItem> {


        protected int tolerance = 5;
        public virtual void Execute() {
            var data = GraphScene;
            var camera = this.Camera;
            var clipper = this.Clipper;
            var modeller = this.ModelReceiver;

            if (data != null && data.Requests.Count != 0 && modeller != null && camera != null) {
                bool clipChanged = false;
                Matrice matrix = camera.Matrice.Clone();

                if (clipper != null) {
                    Action<ICommand<TItem>> before = (request) => {
                        clipChanged = true;
                        var subject = request.Subject;
                        var shape = data.ItemShape(subject);
                        if (shape != null) {
                            var hull = shape.Hull(matrix, tolerance, true);
                            clipper.Add(hull);
                        }

                        if (request is StateChangeCommand<TItem>) {
                            var hull = Layout.GetDataHull(
                                subject, ((StateChangeCommand<TItem>)request).Parameter.One,
                                matrix, tolerance, true);
                            clipper.Add(hull);
                        } else {
                            var hull = Layout.GetDataHull(subject, matrix, tolerance, true);
                            clipper.Add(hull);
                        }
                    };

                    Action<ICommand<TItem>> after = (request) => {
                        clipChanged = true;
                        var subject = request.Subject;
                        var shape = data.ItemShape(subject);
                        if (shape != null) {
                            var hull = shape.Hull(matrix, tolerance, true);
                            clipper.Add(hull);
                        }

                        if (request is StateChangeCommand<TItem>) {
                            var hull = Layout.GetDataHull(
                                subject, ((StateChangeCommand<TItem>)request).Parameter.Two,
                                matrix, tolerance, true);
                            clipper.Add(hull);
                        } else {
                            var hull = Layout.GetDataHull(subject, matrix, tolerance, true);
                            clipper.Add(hull);
                        }
                    };

                    modeller.BeforeExecute = before;
                    modeller.AfterExecute = after;

                } else {
                    modeller.BeforeExecute = null;
                    modeller.AfterExecute = null;
                }

                modeller.Data = this.GraphScene;
                modeller.Layout = this.Layout;


                modeller.Execute(data.Requests);
            }
        }

        public virtual void Done() {
            var data = this.GraphScene;
            if (data != null)
                data.Requests.Clear();
        }

        public virtual void Invoke() {
            var layout = this.Layout;
            var data = this.GraphScene;
            if (layout != null && data != null) {
                ISpatialIndex<TItem> index = data.SpatialIndex;
                index.BoundsDirty = true;

                layout.Invoke();
            }
        }

        # region Properties
        public virtual GraphItemReceiver<TItem, TEdge> ModelReceiver {
            get {
                if (_modelReceiver != null) {
                    return _modelReceiver() as GraphItemReceiver<TItem, TEdge>;
                }
                return null;
            }
        }

        public virtual IGraphScene<TItem, TEdge> GraphScene {
            get {
                if (_graphScene != null) {
                    return _graphScene();
                }
                return null;
            }
        }

        public virtual IGraphLayout<TItem, TEdge> Layout {
            get {
                if (_layout != null) {
                    return _layout();
                }
                return null;
            }
        }


        public virtual IClipper Clipper {
            get {
                if (_clipper != null) {
                    return _clipper();
                }
                return null;
            }
        }

        public virtual ICamera Camera {
            get {
                if (_camera != null) {
                    return _camera();
                }
                return null;
            }
        }
        #endregion

        #region ISceneRealizer<TItem, TEdge> Member
        protected Get<IGraphScene<TItem, TEdge>> _graphScene = null;
        Get<IGraphScene<TItem, TEdge>> ISceneReceiver<TItem, TEdge>.GraphScene {
            get { return _graphScene; }
            set { _graphScene = value; }
        }

        protected Get<IGraphLayout<TItem, TEdge>> _layout = null;
        Get<IGraphLayout<TItem, TEdge>> ISceneReceiver<TItem, TEdge>.Layout {
            get { return _layout; }
            set { _layout = value; }
        }

        protected Get<ICamera> _camera = null;
        Get<ICamera> ISceneReceiver<TItem, TEdge>.Camera {
            get { return _camera; }
            set { _camera = value; }
        }

        protected Get<IClipper> _clipper = null;
        Get<IClipper> ISceneReceiver<TItem, TEdge>.Clipper {
            get { return _clipper; }
            set { _clipper = value; }
        }

        protected Get<IModelReceiver<TItem>> _modelReceiver = null;
        Get<IModelReceiver<TItem>> ISceneReceiver<TItem, TEdge>.ModelReceiver {
            get { return _modelReceiver; }
            set { _modelReceiver = value; }
        }

        #endregion


    }
}