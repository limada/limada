/*
 * Limaki 
 * Version 0.081
 * 
 * This code is free software; you can redistribute it and/or modify it
 * under the terms of the GNU General Public License version 2 only, as
 * published by the Free Software Foundation.
 * 
 * Author: Lytico
 * Copyright (C) 2006-2010 Lytico
 *
 * http://limada.sourceforge.net
 * 
 */

using Limaki.Presenter.Display;
using Limaki.UseCases.Viewers;

namespace Limaki.UseCases {
    public class UseCaseFactory<T> where T:new() {

        public virtual T Create() {
            var result = new T();
            return result;
        }

        public IComposer<T> Composer { get; set; }
        public IComposer<T> DeviceComposer { get; set; }

        public virtual void Instrument(T useCase) {
            Composer.Factor(useCase);
            DeviceComposer.Factor(useCase);

            DeviceComposer.Compose(useCase);
            Composer.Compose(useCase);

        }
    }
}