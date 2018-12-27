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
 * 
 */

using System;
using Limaki.Common;

namespace Limaki.UnitTest {
    
    public class TestExceptionHandler : IExceptionHandler {

        public void Catch (Exception e) {
            throw e;
        }

        public void Catch (Exception e, MessageType messageType) {
            throw e;
        }
    }
}
