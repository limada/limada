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


namespace Limaki.Data {

	public abstract class GatewayBase : IGateway {

        public virtual Iori Iori { get; protected set; }

        public virtual bool IsOpen { get; protected set; }

        public abstract void Open(Iori iori);

        public virtual bool IsClosed { get; protected set; }
		
        public abstract void Close();

	    public abstract void Dispose();

	}
}
