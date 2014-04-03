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
 */

using System;
using System.Windows.Forms;
using Limaki.Common;
using Limaki.Drawing;
using Limaki.GdiBackend;
using Limaki.View;
using Limaki.View.Visuals;
using Limaki.View.Viz;
using Limaki.View.Viz.Visuals;
using Xwt.Backends;
using Xwt.GdiBackend;
using System.Diagnostics;
using Point = Xwt.Point;
using Size = Xwt.Size;

namespace Limaki.SwfBackend.Viz {

     public class VisualsTextEditAction:VisualsTextEditActionBase {

         public VisualsTextEditAction ( Func<IGraphScene<IVisual, IVisualEdge>> sceneHandler,
            IDisplay display, ICamera camera,
            IGraphSceneLayout<IVisual, IVisualEdge> layout): base(sceneHandler, display, camera, layout) {

             this.displayBackend = display.Backend as ContainerControl;

         }

         ContainerControl displayBackend = null;

         #region Editor-Handling

         private TextBox editor = new TextBox();

         protected override void ActivateMarkers () {
             var scene = Scene;
             if (Current is IVisualEdge && scene.Markers != null) {
                 editor.AutoCompleteCustomSource = new AutoCompleteStringCollection();
                 editor.AutoCompleteCustomSource.AddRange(scene.Markers.MarkersAsStrings());
                 editor.AutoCompleteMode = AutoCompleteMode.Suggest;
                 editor.AutoCompleteSource = AutoCompleteSource.CustomSource;
                 editor.Multiline = false;
             } else {
                 editor.Multiline = true;
             }
         }

         protected override void AttachEditor () {

             editor.KeyDown += KeyEditBehaviour;

             StyleEditor();

             displayBackend.Controls.Add(editor);

             editor.Text = DataToText(Current);
             if (string.IsNullOrEmpty (editor.Text)) {
                 var size = Registry.Pooled<IDrawingUtils> ().GetTextDimension ("XXXX", Layout.GetStyle (Current));
                 size = camera.FromSource (size);
                 editor.ClientSize = new Size (size.Width + 2, size.Height + 5).ToGdi ();
             }

             editor.Visible = true;
             ActivateMarkers();
             editor.Focus();
             displayBackend.ActiveControl = editor;
             display.ActiveVidget = editor;
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

             editor.KeyDown -= KeyEditBehaviour;

             editor.Text = String.Empty;
             if (hoverAfteredit && !focusAfterEdit) { // this does not work!
                 Cursor.Position =
                     displayBackend.PointToScreen(editor.Location +
                                          new System.Drawing.Size(editor.Size.Width / 2, editor.Size.Height / 2));
                 //Scene scene = Scene;
                 //if (scene.Hovered != null)
                 //scene.Commands.Add(new StateChangeCommand(scene.Hovered, new Pair<UiState>(UiState.Hovered, UiState.None)));
                 //scene.Hovered = Current;

             }

             editor.AutoCompleteMode = AutoCompleteMode.None;
             editor.AutoCompleteCustomSource = null;
             editor.AutoCompleteSource = AutoCompleteSource.None;

             displayBackend.ActiveControl = null;
             display.ActiveVidget = null;
             displayBackend.Controls.Remove(editor);
             displayBackend.Focus();

             display.ActiveVidget = null;
         }

         private GdiFontCache gdiFontCache = new GdiFontCache();
         void StyleEditor () {
             var style = Layout.StyleSheet.ItemStyle.DefaultStyle;
             var newFont = new FontMemento(style.Font.GetBackend() as System.Drawing.Font);
             newFont.SizeInPoints = (float) camera.Matrix.TransformFontSize(newFont.SizeInPoints);
             editor.Font = gdiFontCache.GetFont(newFont);

             editor.BorderStyle = BorderStyle.FixedSingle;
             editor.Multiline = true;
             editor.ScrollBars = ScrollBars.None;
             editor.WordWrap = true;

             editor.BackColor = System.Drawing.Color.FromArgb((int) style.FillColor.ToRgb());
             var location = camera.FromSource(Current.Location);
             var size = camera.FromSource(Current.Size);
             if (Current is IVisualEdge) {
                 location = camera.FromSource(Current.Shape[Anchor.Center]);
                 var text = Current.Data == null ? "" : Current.Data.ToString();
                 size = (Size)
                        GdiUtils.GetTextDimension(editor.Font, text, new System.Drawing.SizeF())
                     ;
                 size.Height = Math.Max(size.Height + 2, (int) newFont.SizeInPoints + 2);
                 size.Width = Math.Max(size.Width + 2, (int) newFont.SizeInPoints * 4);
                 location.X = location.X - size.Width / 2;
                 location.Y = location.Y - size.Height / 2;
             }
             editor.Location = location.ToGdi();
             editor.Size = size.ToGdi();
             editor.AllowDrop = true;
             editor.DragOver += editor_DragOver;
             editor.DragDrop += editor_DragDrop;
             // does not work:
             //editor.Scale (new SizeF (camera.Matrice.Elements[0], transformer.Matrice.Elements[3]));
         }

         void editor_DragDrop (object sender, System.Windows.Forms.DragEventArgs e) {
             string text = e.Data.GetData(typeof(string)) as string;
             if (text != null) {
                 editor.Text = text;
             }
         }

         void editor_DragOver (object sender, System.Windows.Forms.DragEventArgs e) {
             if (e.Data.GetDataPresent(typeof(string))) {
                 e.Effect = DragDropEffects.Copy;
             } else {
                 e.Effect = DragDropEffects.None;
             }
         }
         #endregion

         protected override Point CursorPosition () {
             return displayBackend.PointToClient (Cursor.Position).ToXwt();
         }


         protected void KeyEditBehaviour (object sender, System.Windows.Forms.KeyEventArgs e) {
             Debug.WriteLine (string.Format ("{0} {1} {2}", e.KeyCode, e.KeyData, e.KeyValue));
             var ev = Converter.Convert (e, new System.Drawing.Point());
             base.KeyEditBehaviour (sender, ev);
         }

       

     }
}
