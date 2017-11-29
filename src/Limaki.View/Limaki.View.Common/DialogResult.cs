/*
 * Limaki 
 * 
 * This code is free software; you can redistribute it and/or modify it
 * under the terms of the GNU General Public License version 2 only, as
 * published by the Free Software Foundation.
 * 
 * Author: Lytico
 * Copyright (C) 2006-2014 Lytico
 *
 * http://www.limada.org
 * 
 */

using System;

namespace Limaki.View.Vidgets {
    
    [Flags]
    public enum MessageBoxButtons {
        None = 0,
        Ok = 1,
        Cancel = 1 << 1,
        Abort = 1 << 2,
        Retry = 1 << 3,
        Ignore = 1 << 4,
        Yes = 1 << 5,
        No = 1 << 6,
        OkCancel = Ok | Cancel,
        AbortRetryIgnore = Abort | Retry | Ignore,
        YesNoCancel = Yes | No | Cancel,
        YesNo = Yes | No,
        RetryCancel = Retry | Cancel,
    }

    public enum DialogResult {
        None = 0,
        Ok = 1,
        Cancel = 2,
        Abort = 3,
        Retry = 4,
        Ignore = 5,
        Yes = 6,
        No = 7
    }
}