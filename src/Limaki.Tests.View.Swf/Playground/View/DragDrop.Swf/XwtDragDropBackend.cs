/*
 * Limaki 
 * 
 * This code is free software; you can redistribute it and/or modify it
 * under the terms of the GNU General Public License version 2 only, as
 * published by the Free Software Foundation.
 * 
 * Author: Lytico
 * Copyright (C) 2012-2013 Lytico
 *
 * http://www.limada.org
 * 
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xwt;
using SWF = System.Windows.Forms;
using SD = System.Drawing;
using Xwt.Backends;

namespace Limaki.View.Ui.DragDrop {
    /// <summary>
    /// this class is a placeholder for Xwt.Swf.WidgetBackend (see also: Xwt.WPFBackend.WidgetBackend)
    /// </summary>
    public abstract class XwtDragDropBackend {

        public abstract DragStartData GetDragDataOnStart ();

        WidgetEvent enabledEvents;

        class DragDropData {
            // Source
            public bool AutodetectDrag;
            public SD.Rectangle DragRect;
            // Target
            public TransferDataType[] TargetTypes = new TransferDataType[0];
        }

        DragDropData _dragDropInfo;

        const WidgetEvent dragDropEvents = WidgetEvent.DragDropCheck | WidgetEvent.DragDrop | WidgetEvent.DragOver | WidgetEvent.DragOverCheck;

        DragDropData DragDropInfo {
            get {
                if (_dragDropInfo == null)
                    _dragDropInfo = new DragDropData();

                return _dragDropInfo;
            }
        }

        SWF.Control Widget { get; set; }
        public void SetDragTarget (TransferDataType[] types, DragDropAction dragAction) {
            DragDropInfo.TargetTypes = types == null ? new TransferDataType[0] : types;
            Widget.AllowDrop = true;
        }

        public void SetDragSource (TransferDataType[] types, DragDropAction dragAction) {
            if (DragDropInfo.AutodetectDrag)
                return; // Drag auto detect has been already activated.

            DragDropInfo.AutodetectDrag = true;
            Widget.MouseUp += WidgetMouseUpForDragHandler;
            Widget.MouseMove += WidgetMouseMoveForDragHandler;
        }
        private void SetupDragRect (SWF.MouseEventArgs e) {
            var width = SWF.SystemInformation.DragSize.Width;
            var height = SWF.SystemInformation.DragSize.Height;
            var loc = e.GetPosition(Widget);
            DragDropInfo.DragRect = new SD.Rectangle(loc.X - width / 2, loc.Y - height / 2, width, height);
        }

        void WidgetMouseUpForDragHandler (object o, EventArgs e) {
            DragDropInfo.DragRect = SD.Rectangle.Empty;
        }

        void WidgetMouseMoveForDragHandler (object o, SWF.MouseEventArgs e) {
            if (enabledEvents.HasFlag(WidgetEvent.DragStarted))
                return;
            if (e.Button != SWF.MouseButtons.Left)
                return;

            if (DragDropInfo.DragRect.IsEmpty)
                SetupDragRect(e);

            if (DragDropInfo.DragRect.Contains(e.GetPosition(Widget)))
                return;

            DragStartData dragData = GetDragDataOnStart();

            if (dragData != null)
                DragStart(dragData);

            DragDropInfo.DragRect = SD.Rectangle.Empty;
        }

        public void DragStart (DragStartData data) {
            if (data.Data == null)
                throw new ArgumentNullException("data");

            SWF.IDataObject dataObj = data.Data.ToSwf();

            if (data.ImageBackend != null) {
                //AdornedWindow = GetParentWindow();
                //AdornedWindow.AllowDrop = true;

                //var e = (UIElement) AdornedWindow.Content;

                //Adorner = new ImageAdorner(e, data.ImageBackend);

                //AdornedLayer = AdornerLayer.GetAdornerLayer(e);
                //AdornedLayer.Add(Adorner);

                //AdornedWindow.DragOver += AdornedWindowOnDragOver;
            }

            Widget.BeginInvoke((Action)(() => {
                var effect = Widget.DoDragDrop(dataObj, data.DragAction.ToSwf());

                OnDragFinished(this, new DragFinishedEventArgs(effect == SWF.DragDropEffects.Move));

                //if (Adorner != null) {
                //    AdornedLayer.Remove(Adorner);
                //    AdornedLayer = null;
                //    Adorner = null;

                //    AdornedWindow.AllowDrop = false;
                //    AdornedWindow.DragOver -= AdornedWindowOnDragOver;
                //    AdornedWindow = null;
                //}
            }));
        }

        protected virtual void OnDragFinished (object sender, DragFinishedEventArgs e) {

        }


    }

    public static class DragDropExtensions {
        public static SD.Point GetPosition (this SWF.MouseEventArgs e, SWF.Control widget) {
            return widget.PointToClient(e.Location);

        }

    }

}