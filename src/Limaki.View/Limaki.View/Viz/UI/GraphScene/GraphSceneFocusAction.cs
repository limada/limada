
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


using Limaki.Actions;
using Limaki.Common;
using Limaki.Drawing;
using Limaki.Graphs;
using Limaki.View.Vidgets;
using Xwt;
using System;
using Limaki.View.GraphScene;

namespace Limaki.View.Viz.UI.GraphScene {
    /// <summary>
    /// Selects an item on mouse down or mouse move
    /// Sets scene.focused and scene.selected or scene.hovered
    /// </summary>
    public class GraphSceneFocusAction<TItem, TEdge> : MouseActionBase, ICheckable
    where TEdge : TItem, IEdge<TItem> {

        public GraphSceneFocusAction()
            : base() {
            this.Priority = ActionPriorities.SelectionPriority - 100;
        }

        public Func<ICamera> CameraHandler { get; set; }
        public Func<IGraphScene<TItem, TEdge>> SceneHandler { get; set; }

        public IGraphScene<TItem, TEdge> Scene {
            get { return SceneHandler(); }
        }

        protected ICamera Camera {
            get { return CameraHandler (); }
        }

        protected int _hitSize = 5;
        /// <summary>
        /// has to be the same as in GraphItemResizer
        /// </summary>
        public int HitSize {
            get { return _hitSize; }
            set { _hitSize = value; }
        }


        public TItem Current { get; protected set; }

        TItem HitTest(Point p) {
            var result = default(TItem);
            var sp = Camera.ToSource(p);

            result = Scene.Hit(sp, HitSize);

            return result;
        }

        public override bool Exclusive { get; protected set; }

        public virtual bool MultiSelect { get; set; }

        public override void OnMouseDown(MouseActionEventArgs e) {
            if (Scene == null)
                return;
            base.OnMouseDown(e);
            if (e.Button == MouseActionButtons.Left || e.IsTouchEvent) {
                var last = Scene.Focused;
                Current = HitTest(e.Location);

                Resolved = (Current != null) && (!Current.Equals(Scene.Focused));

                var multiSelect = e.Modifiers == ModifierKeys.Control || MultiSelect;
                if (Current != null && Current.Equals(last)
                    && multiSelect) {
                    Scene.Focused = default(TItem);
                    Scene.Selected.Remove(last);
                    Current = default(TItem);
                } else if (!multiSelect) {
                    Scene.Focused = Current;
                }

                if (last != null && !last.Equals(Current)) {
                    if (!multiSelect) {
                        Scene.ClearSelection(Current);
                    }
                    Scene.Requests.Add(
                        new StateChangeCommand<TItem>(last,
                            new Pair<UiState>(UiState.Selected, UiState.None))
                        );
                }

                if (Scene.Focused != null) {
                    var sp = Camera.ToSource(e.Location);
                    if (!multiSelect
                        &&
                        ! Scene.ItemShape(Scene.Focused).IsBorderHit(sp, HitSize)) {

                        Scene.ClearSelection(Current);
                        Scene.Requests.Add(
                            new StateChangeCommand<TItem>(Scene.Focused,
                                                          new Pair<UiState>(UiState.None, UiState.Selected))
                            );
                    }
                }

                if (Resolved && Current != null) {
                    if (!Scene.Selected.Contains(Current)) {
                        if (!multiSelect)
                            Scene.ClearSelection (Current);
                        Scene.Selected.Add(Current);
                    }

                    Scene.Requests.Add(
                        new StateChangeCommand<TItem>(Current,
                                                      new Pair<UiState>(UiState.None, UiState.Selected))
                        );
                }

                if (Current == null && !multiSelect) {
                    Scene.ClearSelection ();
                }
            }
        }


        public override void OnMouseMove(MouseActionEventArgs e) {

            if (Scene == null)
                return;
            
            Exclusive = false;
            var before = Scene.Hovered;

            Current = HitTest(e.Location);

            if (Current != null && Scene.Selected.Contains(Current)
                && (e.Button == MouseActionButtons.None || e.IsTouchEvent)) {
                if (Scene.Focused != null && !Current.Equals(Scene.Focused)) {
                    Scene.Requests.Add(
                        new StateChangeCommand<TItem>(Scene.Focused,
                                                      new Pair<UiState>(UiState.None, UiState.Focus))
                        );
                }
                Scene.Focused = Current;
                Scene.Hovered = default(TItem);
            } else {
                Scene.Hovered = Current;
            }

            if (!object.ReferenceEquals(before,Current)) {
                
                if (before != null) {
                    Scene.Requests.Add(
                        new StateChangeCommand<TItem>(before,
                                                      new Pair<UiState>(UiState.Hovered, UiState.None)));

                }
                if (Current != null) {
                    Scene.Requests.Add(
                        new StateChangeCommand<TItem>(Current,
                                                      new Pair<UiState>(UiState.None, UiState.Hovered))
                        );
                }
            }
            //if (before != null && !before.Equals(Current))
            //    Scene.Requests.Add(
            //        new StateChangeCommand<TItem>(before,
            //                                      new Pair<UiState>(UiState.Hovered, UiState.None)));

            //if (Current != null && !Current.Equals(before) && !sameFocus) 
            //    Scene.Requests.Add(
            //        new StateChangeCommand<TItem>(Current,
            //                                      new Pair<UiState>(UiState.None, UiState.Hovered))
            //        );


            Resolved = false;
            
        }
        
        #region ICheckable Member

        public bool Check() {
            if (this.CameraHandler == null) {
                throw new CheckFailedException(this.GetType(), typeof(ICamera));
            }
            if (this.SceneHandler == null) {
                throw new CheckFailedException(this.GetType(), typeof(IGraphScene<TItem,TEdge>));
            }
            return true;
        }

        #endregion

    }
}