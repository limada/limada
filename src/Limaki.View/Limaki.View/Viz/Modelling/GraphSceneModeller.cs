/*
 * Limaki 
 * 
 * This code is free software; you can redistribute it and/or modify it
 * under the terms of the GNU General Public License version 2 only, as
 * published by the Free Software Foundation.
 * 
 * Author: Lytico
 * Copyright (C) 2006-2011 Lytico
 *
 * http://www.limada.org
 * 
 */


using System;
using Limaki.Actions;
using Limaki.Drawing;
using Limaki.Graphs;
using Xwt.Drawing;
using Limaki.View.GraphScene;

namespace Limaki.View.Viz.Modelling {
    /// <summary>
    /// modells the scene
    /// updates clipper
    /// uses a ModelReceiver 
    /// </summary>
    public class GraphSceneModeller<TItem, TEdge> : ActionBase, IGraphSceneModeller<TItem, TEdge>
        where TEdge : TItem, IEdge<TItem> {

        protected int tolerance = 5;

        public virtual void Perform () {
            var data = GraphScene;
            var camera = this.Camera;
            var clipper = this.Clipper;
            var modeller = this.Modeller;

            if (data != null && data.Requests.Count != 0 && modeller != null && camera != null) {
                bool clipChanged = false;
                var matrix = new Matrix(camera.Matrix);

                if (clipper != null) {
                    Action<ICommand<TItem>> before = (request) => {
                        clipChanged = true;
                        var subject = request.Subject;
                        var shape = data.ItemShape (subject);
                        if (shape != null) {
                            var hull = shape.Hull (matrix, tolerance, true);
                            clipper.Add (hull);
                        }

                        if (request is StateChangeCommand<TItem>) {
                            var hull = Layout.GetDataHull (
                                subject, ((StateChangeCommand<TItem>) request).Parameter.One,
                                matrix, tolerance, true);
                            clipper.Add (hull);
                        } else {
                            var hull = Layout.GetDataHull (subject, matrix, tolerance, true);
                            clipper.Add (hull);
                        }
                    };

                    Action<ICommand<TItem>> after = (request) => {
                        clipChanged = true;
                        var subject = request.Subject;
                        var shape = data.ItemShape (subject);
                        if (shape != null) {
                            var hull = shape.Hull (matrix, tolerance, true);
                            clipper.Add (hull);
                        }

                        if (request is StateChangeCommand<TItem>) {
                            var hull = Layout.GetDataHull (
                                subject, ((StateChangeCommand<TItem>) request).Parameter.Two,
                                matrix, tolerance, true);
                            clipper.Add (hull);
                        } else {
                            var hull = Layout.GetDataHull (subject, matrix, tolerance, true);
                            clipper.Add (hull);
                        }
                    };

                    modeller.BeforePerform = before;
                    modeller.AfterPerform = after;

                } else {
                    modeller.BeforePerform = null;
                    modeller.AfterPerform = null;
                }

                modeller.Data = this.GraphScene;
                modeller.Layout = this.Layout;


                modeller.Perform (data.Requests);
            }
        }

        public virtual void Finish () {
            var data = this.GraphScene;
            if (data != null)
                data.Requests.Clear ();
        }

        public virtual void Reset () {
            var layout = this.Layout;
            var data = this.GraphScene;

            if (layout != null && data != null) {
                data.SpatialIndex.BoundsDirty = true;
                layout.Reset ();
            }
        }

        # region Properties

        public virtual GraphItemModeller<TItem, TEdge> Modeller {
            get {
                if (_modeller != null) {
                    return _modeller () as GraphItemModeller<TItem, TEdge>;
                }
                return null;
            }
        }

        public virtual IGraphScene<TItem, TEdge> GraphScene {
            get {
                if (_graphScene != null) {
                    return _graphScene ();
                }
                return null;
            }
        }

        public virtual IGraphSceneLayout<TItem, TEdge> Layout {
            get {
                if (_layout != null) {
                    return _layout ();
                }
                return null;
            }
        }


        public virtual IClipper Clipper {
            get {
                if (_clipper != null) {
                    return _clipper ();
                }
                return null;
            }
        }

        public virtual ICamera Camera {
            get {
                if (_camera != null) {
                    return _camera ();
                }
                return null;
            }
        }
        #endregion

        #region IGraphSceneReceiver<TItem, TEdge> Member

        protected Func<IGraphScene<TItem, TEdge>> _graphScene = null;
        Func<IGraphScene<TItem, TEdge>> IGraphSceneModeller<TItem, TEdge>.GraphScene {
            get { return _graphScene; }
            set { _graphScene = value; }
        }

        protected Func<IGraphSceneLayout<TItem, TEdge>> _layout = null;
        Func<IGraphSceneLayout<TItem, TEdge>> IGraphSceneModeller<TItem, TEdge>.Layout {
            get { return _layout; }
            set { _layout = value; }
        }

        protected Func<ICamera> _camera = null;
        Func<ICamera> IGraphSceneModeller<TItem, TEdge>.Camera {
            get { return _camera; }
            set { _camera = value; }
        }

        protected Func<IClipper> _clipper = null;
        Func<IClipper> IGraphSceneModeller<TItem, TEdge>.Clipper {
            get { return _clipper; }
            set { _clipper = value; }
        }

        protected Func<ICommandModeller<TItem>> _modeller = null;
        Func<ICommandModeller<TItem>> IGraphSceneModeller<TItem, TEdge>.Modeller {
            get { return _modeller; }
            set { _modeller = value; }
        }

        #endregion


    }
}