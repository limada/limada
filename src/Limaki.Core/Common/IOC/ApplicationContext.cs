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
 * http://limada.sourceforge.net
 * 
 */


namespace Limaki.Common.IOC {
    public class ApplicationContext : IApplicationContext {
        IPool _pool = new Pool();
        public virtual IPool Pool {
            get {
                _pool.PoolFactory = this.Factory;
                return _pool;
            }
        }

        private IFactory _factory = new Factory();
        public IFactory Factory {
            get { return _factory; }
        }
    }
}