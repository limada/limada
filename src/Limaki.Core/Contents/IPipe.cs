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


namespace Limaki.Contents {

    /// <summary>
    /// a morph, flux, transform, transition, flow, pipe, tube
    /// from source to sink
    /// </summary>
    /// <typeparam name="TSource"></typeparam>
    /// <typeparam name="TSink"></typeparam>
    public interface IPipe<TSource, TSink> {
        TSink Use(TSource source);
        TSink Use(TSource source, TSink sink);
    }

}