/*
 * Limaki 
 * 
 * This code is free software; you can redistribute it and/or modify it
 * under the terms of the GNU General Public License version 2 only, as
 * published by the Free Software Foundation.
 * 
 * Author: Lytico
 * Copyright (C) 2014 Lytico
 *
 * http://www.limada.org
 * 
 */

using System;
using Limaki.Common;
using Limaki.Drawing;
using Limaki.View.UI;
using Limaki.View.Visuals.UI;
using Limaki.Visuals;
using Xwt;

namespace Limaki.View.XwtBackend.Visualizers {

    public class VisualsTextEditAction : VisualsTextEditActionBase {

        public VisualsTextEditAction (
            Func<IGraphScene<IVisual, IVisualEdge>> sceneHandler,
            IDisplay display, ICamera camera,
            IGraphSceneLayout<IVisual, IVisualEdge> layout)
            : base(sceneHandler, display, camera, layout) {

            this.displayBackend = display.Backend as DisplayBackend;
        }

        private DisplayBackend displayBackend = null;
        private TextEntry editor = new TextEntry();

        protected override void AttachEditor () {

            editor.KeyPressed += KeyCancelEdit;
            editor.KeyPressed += KeyStartOrConfirmEdit;
            editor.DragOver += EditorDragOver;
            editor.DragDrop += EditorDragDrop;

            var loc = StyleEditor(editor);

            displayBackend.AddChild(editor, loc);

            editor.Text = DataToText(Current);

            editor.Visible = true;
            ActivateMarkers();
            editor.SetFocus();
            display.ActiveVidget = editor;
        }

        protected Rectangle StyleEditor (TextEntry editor) {
            var style = Layout.StyleSheet.ItemStyle.DefaultStyle;
            editor.Font = style.Font;
            editor.BackgroundColor = style.FillColor;
            editor.MultiLine = true;
            editor.ShowFrame = true;

            var location = camera.FromSource(Current.Location);
            var size = camera.FromSource(Current.Size);
            if (Current is IVisualEdge) {
                location = camera.FromSource(Current.Shape[Anchor.Center]);
                var text = DataToText(Current);
                size = Registry.Pooled<IDrawingUtils>()
                    .GetTextDimension(text, style);
                size.Height = Math.Max(size.Height + 2, style.Font.Size + 2);
                size.Width = Math.Max(size.Width + 2, style.Font.Size * 4);
                location.X = location.X - size.Width / 2;
                location.Y = location.Y - size.Height / 2;
            }

            return new Rectangle(location, size);
        }

        protected override void DetachEditor (bool writeData) {
            if (Current == null)
                return;
            if (writeData) {
                TextToData(Current, editor.Text);
            }
            AfterEdit();

            Current = null;

            editor.Visible = false;

            editor.KeyPressed -= KeyCancelEdit;
            editor.KeyPressed -= KeyStartOrConfirmEdit;
            editor.DragOver -= EditorDragOver;
            editor.DragDrop -= EditorDragDrop;

            editor.Text = String.Empty;

            displayBackend.RemoveChild(editor);
            displayBackend.SetFocus();

            display.ActiveVidget = null;

        }
        
        public override void OnKeyPressed (KeyActionEventArgs e) {
            if (e.Key == Key.Escape) {
                KeyCancelEdit(this, e);
            }
            if (e.Key == Key.F2)
                KeyStartOrConfirmEdit(this, e);

            focusAfterEdit = false;
            hoverAfteredit = false;
            bool insert = false;
            if (e.Key == Key.Return && e.Modifiers == ModifierKeys.Control) {
                insert = true;
            }
            if (e.Key == Key.Insert ) {
                insert = true;
                focusAfterEdit = true;
            }

            if (insert) {
                DetachEditor(true);
                Exclusive = Resolved = true;
                Current = Registry.Pooled<IVisualFactory>().CreateItem<string>("XXXXXXXX");
                var scene = Scene;
                var root = scene.Focused;

                if (root == null) {
                    var pt = displayBackend.MouseLocation();
                    pt = camera.ToSource(pt) - Layout.Distance;
                    SceneExtensions.AddItem(scene, Current, Layout, pt);
                } else {
                    SceneExtensions.PlaceVisual(scene, root, Current, Layout);
                }

                display.Perform();
                TextToData(Current, string.Empty);

                AttachEditor();
            }
        }


        protected virtual void KeyStartOrConfirmEdit (object sender, KeyEventArgs e) {
            if (e.Key == Key.F2) {
                if (Exclusive) {
                    Exclusive = false;
                    DetachEditor(true);
                } else {
                    if (Visual != null) {//&& HitTest(Visual, lastMousePos)
                        Exclusive = Resolved = true;
                        Current = Visual;
                        AttachEditor();
                    }
                }
                e.Handled = true;
            }
        }

        protected virtual void KeyCancelEdit (object sender, KeyEventArgs e) {
            if (e.Key == Key.Escape) {
                Exclusive = false;
                DetachEditor(false);
                e.Handled = true;
            }
        }

        [TODO]
        private void EditorDragDrop (object sender, DragEventArgs e) {

        }

        [TODO]
        private void EditorDragOver (object sender, DragOverEventArgs e) {

        }

        [TODO]
        protected override void ActivateMarkers () {

        }
    }
}
