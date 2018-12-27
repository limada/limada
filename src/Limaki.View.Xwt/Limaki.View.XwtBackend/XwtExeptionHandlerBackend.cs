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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Limaki.Common;
using Xwt;

namespace Limaki.View.XwtBackend {

    public class XwtExeptionHandlerBackend : XwtMessager, IExceptionHandler {

        protected bool ShowRetryCancel (Exception e) {
            var msg = new ConfirmationMessage(e?.Message??"", XwtMessager.Retry);
            msg.Buttons.Clear();
            msg.Buttons.Add(XwtMessager.Abort);
            msg.ConfirmButton = XwtMessager.Retry;
            msg.Icon = StockIcons.Error;
            return MessageDialog.Confirm(msg);
        }

        public void Catch (Exception e) {
            if(ShowRetryCancel(e))
                throw e;
        }

        public void Catch (Exception e, MessageType messageType) {
            if (messageType == MessageType.RetryCancel) {
                if (ShowRetryCancel(e))
                    throw e;
            } else {
                MessageDialog.ShowMessage(StockIcons.Error, e.Message, null);
            }
        }


    }
}
