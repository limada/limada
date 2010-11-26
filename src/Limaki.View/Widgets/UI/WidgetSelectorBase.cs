
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
using Limaki.Drawing.UI;

namespace Limaki.Widgets.UI {
    /// <summary>
    /// Selects a widget on mouse down or mouse move
    /// Sets scene.focused and scene.selected or scene.hovered
    /// </summary>
    public class WidgetSelectorBase : MouseActionBase {
        public WidgetSelectorBase()
            : base() {
            this.Priority = ActionPriorities.SelectionPriority - 10;
        }
        ///<directed>True</directed>
        protected ICamera camera = null;

        public WidgetSelectorBase(Func<Scene> sceneHandler, ICamera camera)
            : this() {
            this.camera = camera;
            this.SceneHandler = sceneHandler;
        }

        ///<directed>True</directed>
        protected Func<Scene> SceneHandler;
        public Scene Scene {
            get { return SceneHandler(); }
        }

        protected int _hitSize = 5;
        /// <summary>
        /// has to be the same as in WidgetResizer
        /// </summary>
        public int HitSize {
            get { return _hitSize; }
            set { _hitSize = value; }
        }

        private IWidget _current = null;
        public IWidget Current {
            get { return _current; }
            set { _current = value; }
        }

        IWidget HitTest(PointI p) {
            IWidget result = null;
            PointI sp = camera.ToSource(p);

            result = Scene.Hit(sp, HitSize);

            return result;
        }

        bool _exclusive = false;
        public override bool Exclusive {
            get { return _exclusive; }
            protected set { _exclusive = value; }
        }

        void ClearSelection() {
            foreach (IWidget w in Scene.Selected.Elements) {
                if (w != Current)
                    Scene.Commands.Add(new StateChangeCommand(w,
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
                IWidget last = Scene.Focused;
                Current = HitTest(e.Location);

                Resolved = (Current != null) && (Scene.Focused != Current);

                if (Current != null && Current == last
                    && e.Modifiers == ModifierKeys.Control) {
                    Scene.Focused = null;
                    Scene.Selected.Remove(last);
                    Current = null;
                } else {
                    Scene.Focused = Current;
                }

                if (Current != last && last != null) {
                    if (e.Modifiers == ModifierKeys.None) {
                        ClearSelection();
                    }
                    Scene.Commands.Add(
                        new StateChangeCommand(last,
                                                new Pair<UiState>(UiState.Selected, UiState.None))
                        );
                }

                if (Scene.Focused != null) {
                    PointI sp = camera.ToSource(e.Location);
                    if (e.Modifiers == ModifierKeys.None
                         &&
                         !Scene.Focused.Shape.IsBorderHit(sp, HitSize)) {

                        ClearSelection();
                        Scene.Commands.Add(
                            new StateChangeCommand(Scene.Focused,
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

                    Scene.Commands.Add(
                        new StateChangeCommand(Current,
                                               new Pair<UiState>(UiState.None, UiState.Selected))
                        );
                }
            }
        }


        public override void OnMouseMove(MouseActionEventArgs e) {
            if (Scene == null)
                return;
            Exclusive = false;
            IWidget before = Scene.Hovered;
            IWidget hit = HitTest(e.Location);
            //if (hit != Scene.Focused || Scene.Focused == null) {
                Current = hit;
            //}
            if (Current != null && Scene.Selected.Contains(Current) 
                && e.Button == MouseActionButtons.None) {
                if (Scene.Focused != null && Scene.Focused != Current) {
                    Scene.Commands.Add(
                        new StateChangeCommand(Scene.Focused,
                                                new Pair<UiState>(UiState.None, UiState.Focus))
                        );
                }
                Scene.Focused = Current;
                Scene.Hovered = null;
            } else {
                Scene.Hovered = Current; 
            }
            if (before != Current) {
                if (before != null)
                    Scene.Commands.Add(
                        new StateChangeCommand(before,
                                               new Pair<UiState>(UiState.Hovered, UiState.None)));
                if (Current != null)
                    Scene.Commands.Add(
                        new StateChangeCommand(Current,
                                               new Pair<UiState>(UiState.None, UiState.Hovered))
                        );
                //control.CommandsExecute ();
            }
            Resolved = false;
        }


    }
}