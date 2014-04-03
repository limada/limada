/*
 * Limaki 
 * 
 * This code is free software; you can redistribute it and/or modify it
 * under the terms of the GNU General Public License version 2 only, as
 * published by the Free Software Foundation.
 * 
 * Author: Lytico
 * Copyright (C) 2008-2013 Lytico
 *
 * http://www.limada.org
 * 
 */

using System;
using System.Windows.Forms;
using Limaki.Common;

namespace Limaki.View.SwfBackend {
    
    public class SwfExeptionHandlerBackend:IExceptionHandler {

        public virtual void Catch(Exception e, MessageType messageType) {
            MessageBoxButtons buttons = MessageBoxButtons.RetryCancel;
            if (messageType == MessageType.OK)
                buttons = MessageBoxButtons.OK;
            if (MessageBox.Show(e.Message, "Error", buttons) == DialogResult.Cancel)
                throw e;
        }
        public virtual void Catch(Exception e) {
            if (MessageBox.Show(e.Message, "Error", MessageBoxButtons.RetryCancel) == DialogResult.Cancel)
                throw e;
        }

    }
}