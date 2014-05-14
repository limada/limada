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
using Limaki.View;
using Limaki.View.DragDrop;
using Xwt;
using Xwt.Backends;
using Xwt.SwfBackend;

namespace Limaki.View.SwfBackend.DragDrop {

    /// <summary>
    /// implements Windows.Forms DragDrop over Xwt.DragDrop
    /// it handles dragdrop 
    /// needs a Windows.Forms.Control 
    /// usage: extend a Backend with DragDrop features
    /// <remarks>see: Xwt.Backends.IVidgetBackend / Xwt.WPFBackend.VidgetBackend</remarks>
    /// </summary>
    public class DragDropBackendHandler : DragDropBackendHandlerBase {

        public DragDropBackendHandler (IVidgetBackend backend) {
            this.Backend = backend as System.Windows.Forms.Control;
        }

        protected System.Windows.Forms.Control Backend { get; set; }

        #region Drag (Backend is source, is dragged)

        

        protected override DragStartData GetDragStartData () {
            object imageBackend = GetDragImageBackend();
            return new DragStartData(DragDataSource(), Xwt.DragDropAction.All, imageBackend, Backend.Left, Backend.Top);
        }


        /// <summary>
        /// Starts a drag operation originated in this widget
        /// </summary>
        /// <param name='data'>
        /// Drag operation arguments
        /// </param>
        public override void DragStart (DragStartData data) {
            if (data.Data == null)
                throw new ArgumentNullException("data");

            if (data.ImageBackend != null) {
                ShowDragImage(data);
            }

            var dataObj = data.Data.ToSwf();

            //Backend.BeginInvoke((Action)(() => {
                var effect = Backend.DoDragDrop(dataObj, data.DragAction.ToSwf());

                OnDragFinished(new DragFinishedEventArgs(effect == System.Windows.Forms.DragDropEffects.Move));

                HideDragImage();
            //}));
        }

        protected virtual object GetDragImageBackend () { return null; }
        protected override void ShowDragImage (DragStartData data) { }
        protected override void HideDragImage () { }


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
        public override void SetDragTarget (DragDropAction dragAction, params TransferDataType[] types) {
            DragDropInfo.TargetTypes = types == null ? new TransferDataType[0] : types;
            Backend.AllowDrop = true;
        }

        

        #endregion
        
        

    }
}