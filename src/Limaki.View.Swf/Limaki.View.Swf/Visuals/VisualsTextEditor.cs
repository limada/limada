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

using System.Windows.Forms;

using System;
using System.ComponentModel;
using Limaki.Actions;
using Limaki.Common;
using Limaki.Drawing;
using Limaki.Drawing.Gdi;
using Limaki.View.Layout;
using Limaki.View.UI;
using Limaki.View.UI.GraphScene;
using Limaki.View.Visuals.UI;
using Limaki.Visuals;
using Xwt;
using Xwt.Backends;
using Xwt.Gdi.Backend;
using SWF = System.Windows.Forms;

namespace Limaki.View.Swf.Visuals {
     public class VisualsTextEditor:VisualsTextEditorBase {

         public VisualsTextEditor (
            Func<IGraphScene<IVisual, IVisualEdge>> sceneHandler, 
            ContainerControl device,
            IDisplay display,
            ICamera camera,
            IGraphSceneLayout<IVisual, IVisualEdge> layout)
             : base(sceneHandler,display,camera,layout) {

            this.device = device;

        }

         ContainerControl device = null;

         #region Editor-Handling

         private TextBox editor = new TextBox();

         protected override void ActivateMarkers () {
             var scene = Scene;
             if (Current is IVisualEdge && scene.Markers != null) {
                 editor.AutoCompleteCustomSource =
                     new AutoCompleteStringCollection();
                 editor.AutoCompleteCustomSource.AddRange(scene.Markers.MarkersAsStrings());
                 editor.AutoCompleteMode = AutoCompleteMode.Suggest;
                 editor.AutoCompleteSource = AutoCompleteSource.CustomSource;
                 editor.Multiline = false;
             } else {
                 editor.Multiline = true;
             }
         }

         protected override void AttachEditor () {
             editor.KeyDown += control_KeyCancelEdit;
             editor.KeyDown += control_KeyStartOrConfirmEdit;

             StyleEditor();

             device.Controls.Add(editor);

             editor.Text = DataToText(Current);

             editor.Visible = true;
             ActivateMarkers();
             editor.Focus();
             device.ActiveControl = editor;
             display.ActiveVidget = editor;
         }


         protected override void DetachEditor (bool writeData) {
             if (Current == null)
                 return;
             if (writeData) {
                 TextToData(Current, editor.Text);
             }
             if (focusAfterEdit) {
                 var scene = Scene;
                 scene.Selected.Clear();
                 if (scene.Focused != null)
                     scene.Requests.Add(new StateChangeCommand<IVisual>(scene.Focused, new Pair<UiState>(UiState.Selected, UiState.None)));
                 scene.Focused = Current;
             }

             Current = null;

             editor.Visible = false;

             editor.KeyDown -= control_KeyCancelEdit;
             editor.KeyDown -= control_KeyStartOrConfirmEdit;

             editor.Text = String.Empty;
             if (hoverAfteredit && !focusAfterEdit) { // this does not work!
                 Cursor.Position =
                     device.PointToScreen(editor.Location +
                                          new System.Drawing.Size(editor.Size.Width / 2, editor.Size.Height / 2));
                 //Scene scene = Scene;
                 //if (scene.Hovered != null)
                 //scene.Commands.Add(new StateChangeCommand(scene.Hovered, new Pair<UiState>(UiState.Hovered, UiState.None)));
                 //scene.Hovered = Current;

             }

             editor.AutoCompleteMode = AutoCompleteMode.None;
             editor.AutoCompleteCustomSource = null;
             editor.AutoCompleteSource = AutoCompleteSource.None;

             device.ActiveControl = null;
             device.Controls.Remove(editor);
             device.Focus();

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

         void editor_DragDrop (object sender, SWF.DragEventArgs e) {
             string text = e.Data.GetData(typeof(string)) as string;
             if (text != null) {
                 editor.Text = text;
             }
         }

         void editor_DragOver (object sender, SWF.DragEventArgs e) {
             if (e.Data.GetDataPresent(typeof(string))) {
                 e.Effect = DragDropEffects.Copy;
             } else {
                 e.Effect = DragDropEffects.None;
             }
         }
         #endregion


         protected void control_KeyCancelEdit (object sender, SWF.KeyEventArgs e) {
             if (e.KeyData == Keys.Escape) {
                 Exclusive = false;
                 DetachEditor(false);
                 e.Handled = true;
             }
         }

         protected void control_KeyStartOrConfirmEdit (object sender, SWF.KeyEventArgs e) {
             if (e.KeyData == Keys.F2) {
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

         public override void OnKeyPressed (KeyActionEventArgs e) {
             if (e.Key == Key.Escape) {
                 control_KeyCancelEdit(this, new SWF.KeyEventArgs(Keys.Escape));
             }
             if (e.Key == Key.F2)
                 control_KeyStartOrConfirmEdit(this, new SWF.KeyEventArgs(Keys.F2));

             focusAfterEdit = false;
             hoverAfteredit = false;
             bool insert = false;
             if (e.Key == Key.Return) {
                 insert = true;
             }
             if (e.Key == Key.Insert) {
                 insert = true;
                 focusAfterEdit = true;
             }

             if (insert) {
                 DetachEditor(true);
                 Exclusive = Resolved = true;
                 Current = Registry.Pool.TryGetCreate<IVisualFactory>().CreateItem<string>("XXXXXXXX");
                 var scene = Scene;
                 IVisual root = scene.Focused;

                 if (root == null) {
                     var pt = device.PointToClient(Cursor.Position).ToXwt();
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

     }
}
