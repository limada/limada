/*
 * Limaki 
 * 
 * This code is free software; you can redistribute it and/or modify it
 * under the terms of the GNU General Public License version 2 only, as
 * published by the Free Software Foundation.
 * 
 * Author: Lytico
 * Copyright (C) 2013 Lytico
 *
 * http://www.limada.org
 * 
 */

using System;
using System.Linq;
using Limaki.Common;
using Limaki.View.UI;
using Xwt;
using Xwt.Backends;

using DragEventArgs = Limaki.View.DragDrop.DragEventArgs;
using DragOverEventArgs = Limaki.View.DragDrop.DragOverEventArgs;

namespace Limaki.View.DragDrop {

    public abstract class DragDropBackendHandlerBase : IDragDropBackendHandler {


        public Func<TransferDataSource> DragDataSource { get; set; }
        public Action<DragFinishedEventArgs> DragFinished { get; set; }
        public Action<DragEventArgs> Dropped { get; set; }

        public WidgetEvent EnabledEvents { get; set; }

        const WidgetEvent dragDropEvents = WidgetEvent.DragDropCheck | WidgetEvent.DragDrop | WidgetEvent.DragOver | WidgetEvent.DragOverCheck;
        DragDropInfo _dragDropInfo;
        protected virtual DragDropInfo DragDropInfo {
            get { return _dragDropInfo ?? (_dragDropInfo = new DragDropInfo()); }
        }

        #region Drag (Backend is source, is dragged)

        /// <summary>
        /// Sets up a widget so that XWT will start a drag operation when the user clicks and drags on the widget.
        /// </summary>
        /// <param name='types'>
        /// Types of data that can be dragged from this widget
        /// </param>
        /// <param name='dragAction'>
        /// Bitmask of possible actions for a drag from this widget
        /// </param>
        public virtual void SetDragSource (DragDropAction dragAction, params TransferDataType[] types) {
            if (DragDropInfo.AutodetectDrag)
                return; // Drag auto detect has been already activated.

            DragDropInfo.AutodetectDrag = true;
            DragDropInfo.SourceTypes = types;


        }

        public TransferDataType[] SourceTypes {
            get { return DragDropInfo.SourceTypes; }
        }

        static IUISystemInformation _systemInformation = null;
        protected static IUISystemInformation SystemInformation { get { return _systemInformation ?? (_systemInformation = Registry.Factory.Create<IUISystemInformation>()); } }

        protected virtual void SetupDragRect (MouseActionEventArgs e) {
            var size = SystemInformation.DragSize;
            DragDropInfo.DragRect = new Rectangle(e.Location.X - size.Width / 2, e.Location.Y - size.Height / 2, size.Width, size.Height);
        }

        protected abstract DragStartData GetDragStartData ();

        /// <summary>
        /// Starts a drag operation originated in this widget
        /// </summary>
        /// <param name='data'>
        /// Drag operation arguments
        /// </param>
        public abstract void DragStart (DragStartData data);

        protected virtual object GetDragImageBackend () { return null; }
        protected virtual void ShowDragImage (DragStartData data) { }
        protected virtual void HideDragImage () { }

        public virtual void OnDragFinished (DragFinishedEventArgs e) {
            if (DragFinished != null)
                DragFinished(e);
        }

        public virtual void DragLeave (EventArgs e) { }

        #endregion


        #region Drop

        /// <summary>
        /// Sets a widget as a potential drop destination
        /// </summary>
        /// <param name='types'>
        /// Types.
        /// </param>
        /// <param name='dragAction'>
        /// Drag action.
        /// </param>
        public abstract void SetDragTarget (DragDropAction dragAction, params TransferDataType[] types);

        public virtual TransferDataType[] TargetTypes {
            get { return DragDropInfo.TargetTypes; }
        }

        public virtual void DragOverCheck (DragOverCheckEventArgs args) {
            if (!args.DataTypes.Intersect(this.TargetTypes).Any()) {
                args.AllowedAction = DragDropAction.None;
            }
        }

        public virtual void DragOver (DragOverEventArgs args) {
            if (!args.Data.DataTypes.Intersect(this.TargetTypes).Any()) {
                args.AllowedAction = DragDropAction.None;
            }
        }

        public virtual void DropCheck (DragCheckEventArgs args) {
            if (!args.DataTypes.Intersect(this.TargetTypes).Any()) {
                args.Result = DragDropResult.Canceled;
            } else {
                args.Result = DragDropResult.Success;
            }
        }

        public virtual void OnDrop (DragEventArgs args) {
            if (Dropped != null)
                Dropped(args);
        }
        #endregion
    }
}