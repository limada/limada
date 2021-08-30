/*
 * Limaki 
 * 
 * This code is free software; you can redistribute it and/or modify it
 * under the terms of the GNU General Public License version 2 only, as
 * published by the Free Software Foundation.
 * 
 * Author: Lytico
 * Copyright (C) 2010-2012 Lytico
 *
 * http://www.limada.org
 * 
 */

using System;

namespace Limaki.Common {

    public enum MessageType {
        OK,
        RetryCancel
    }

    public interface IExceptionHandler {
        void Catch ( Exception e );
        void Catch(Exception e, MessageType messageType);
    }

    public class ThrowingExceptionHandler:IExceptionHandler {
        public virtual void Catch(Exception e) {
            throw e;
        }
        public virtual void Catch(Exception e, MessageType messageType) {
            throw e;
        }
    }
}