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


using Limaki.Actions;

namespace Limaki.View {
    /// <summary>
    /// executes a request-queue
    /// on the receiver's subject
    /// </summary>
    public interface IReceiver : IAction {
        /// <summary>
        /// set initial values
        /// </summary>
        void Reset ();

        /// <summary>
        /// performs the requests
        /// </summary>
        void Perform ();
        
        /// <summary>
        /// actions to do on the end of the performance
        /// </summary>
        void Finish ();
    }
}