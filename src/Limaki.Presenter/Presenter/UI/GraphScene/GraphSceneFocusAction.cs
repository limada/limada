
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


using Limaki.Actions;
using Limaki.Common;
using Limaki.Drawing;
using Limaki.Graphs;

namespace Limaki.Presenter.UI {
    /// <summary>
    /// Selects a widget on mouse down or mouse move
    /// Sets scene.focused and scene.selected or scene.hovered
    /// </summary>
    public class GraphSceneFocusAction<TItem, TEdge> : MouseActionBase, ICheckable
    where TEdge : TItem, IEdge<TItem> {

        public GraphSceneFocusAction()
            : base() {
            this.Priority = ActionPriorities.SelectionPriority - 10;
        }

        public Get<ICamera> CameraHandler { get; set; }
        public Get<IGraphScene<TItem, TEdge>> SceneHandler { get; set; }

        public IGraphScene<TItem, TEdge> Scene {
            get { return SceneHandler(); }
        }

        protected ICamera Camera {
            get { return CameraHandler (); }
        }

        protected int _hitSize = 5;
        /// <summary>
        /// has to be the same as in WidgetResizer
        /// </summary>
        public int HitSize {
            get { return _hitSize; }
            set { _hitSize = value; }
        }

        private TItem _current = default(TItem);
        public TItem Current {
            get { return _current; }
            set { _current = value; }
        }

        TItem HitTest(PointI p) {
            TItem result = default(TItem);
            PointI sp = Camera.ToSource(p);

            result = Scene.Hit(sp, HitSize);

            return result;
        }

        bool _exclusive = false;
        public override bool Exclusive {
            get { return _exclusive; }
            protected set { _exclusive = value; }
        }

        void ClearSelection() {
            foreach (TItem w in Scene.Selected.Elements) {
                if (!w.Equals(Current))
                    Scene.Requests.Add(new StateChangeCommand<TItem>(w,
                                                                     new Pair<UiState>(UiState.Selected, UiState.None)));
            }
            Scene.Selected.Clear();
            if (Scene.Focused != null) {
                Scene.Selected.Add(Scene.Focused);
            }
        }

        public override void OnMouseDown(MouseActionEventArgs e) {
            if (Scene == null)
                return;
            base.OnMouseDown(e);
            if (e.Button == MouseActionButtons.Left) {
                TItem last = Scene.Focused;
                Current = HitTest(e.Location);

                Resolved = (Current != null) && (!Current.Equals(Scene.Focused));

                if (Current != null && Current.Equals(last)
                    && e.Modifiers == ModifierKeys.Control) {
                    Scene.Focused = default(TItem);
                    Scene.Selected.Remove(last);
                    Current = default(TItem);
                } else {
                    Scene.Focused = Current;
                }

                if (last != null && !last.Equals(Current)) {
                    if (e.Modifiers == ModifierKeys.None) {
                        ClearSelection();
                    }
                    Scene.Requests.Add(
                        new StateChangeCommand<TItem>(last,
                            new Pair<UiState>(UiState.Selected, UiState.None))
                        );
                }

                if (Scene.Focused != null) {
                    PointI sp = Camera.ToSource(e.Location);
                    if (e.Modifiers == ModifierKeys.None
                        &&
                        ! Scene.ItemShape(Scene.Focused).IsBorderHit(sp, HitSize)) {

                        ClearSelection();
                        Scene.Requests.Add(
                            new StateChangeCommand<TItem>(Scene.Focused,
                                                          new Pair<UiState>(UiState.None, UiState.Selected))
                            );
                    }
                }

                if (Resolved && Current != null) {
                    if (!Scene.Selected.Contains(Current)) {
                        if (e.Modifiers == ModifierKeys.None)
                            ClearSelection();
                        Scene.Selected.Add(Current);
                    }

                    Scene.Requests.Add(
                        new StateChangeCommand<TItem>(Current,
                                                      new Pair<UiState>(UiState.None, UiState.Selected))
                        );
                }
                if (Current == null && e.Modifiers == ModifierKeys.None) {
                    ClearSelection(); 
                }
            }
        }


        public override void OnMouseMove(MouseActionEventArgs e) {
            if (Scene == null)
                return;
            
            Exclusive = false;
            TItem before = Scene.Hovered;

            Current = HitTest(e.Location);

            if (Current != null && Scene.Selected.Contains(Current)
                && e.Button == MouseActionButtons.None) {
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