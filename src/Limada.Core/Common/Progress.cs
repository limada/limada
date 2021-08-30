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

namespace Limaki.Common {

    public interface IProgress {
        /// <summary>
        /// string must be of format "message {0} {1}"
        /// if int0 == -1 && int0 == -1 then only string is displayed
        /// else string.Format(message, int0, int0)
        /// </summary>
        Action<string, int, int> Progress { get; set; }
    }

    public static class ProgressExtensions {
        public static void AttachProgress (this IProgress source, IProgress sink) {
            if (sink != null)
                sink.Progress = source.Progress;

        }
    }
}
