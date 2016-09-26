/*
 * Limaki 
 * 
 * This code is free software; you can redistribute it and/or modify it
 * under the terms of the GNU General Public License version 2 only, as
 * published by the Free Software Foundation.
 * 
 * Author: Lytico
 * Copyright (C) 2006-2010 Lytico
 *
 * http://www.limada.org
 * 
 */

using Limaki.Common;
using Limaki.View;

namespace Limaki.Usecases {
	
    public class UsecaseFactory<T> where T:new() {

        public virtual T Create() {
            var result = new T();
            return result;
        }

        public IComposer<T> Composer { get; set; }
        public IComposer<T> BackendComposer { get; set; }

        public virtual void Compose(T useCase) {
            Composer.Factor(useCase);
            BackendComposer.Factor(useCase);

            BackendComposer.Compose(useCase);
            Composer.Compose(useCase);

        }
    }
}