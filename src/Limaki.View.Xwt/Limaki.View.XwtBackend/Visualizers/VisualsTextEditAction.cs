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
using Limaki.Visuals.GraphScene;
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
            var style = Layout.GetStyle(Current);
            editor.Font = style.Font;
            editor.BackgroundColor = style.FillColor;
            editor.MultiLine = true;
            editor.ShowFrame = true;
            
            var location = camera.FromSource(Current.Location);
            var size = camera.FromSource(Current.Size);
            if (Current is IVisualEdge) {
                location = camera.FromSource(Current.Shape[Anchor.Center]);
                var text = DataToText(Current);
                if (string.IsNullOrEmpty (text))
                    text = "XXXX";
                size = Registry.Pooled<IDrawingUtils>()
                    .GetTextDimension(text, style);
                size.Height = Math.Max(size.Height + 2, style.Font.Size + 2);
                size.Width = Math.Max(size.Width + 2, style.Font.Size * 4);
                size = camera.FromSource (size);
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

        protected override Point CursorPosition () {
            return displayBackend.MouseLocation();
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
