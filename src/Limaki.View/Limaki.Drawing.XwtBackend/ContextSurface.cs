/*
 * Limaki 
 
 * 
 * This code is free software; you can redistribute it and/or modify it
 * under the terms of the GNU General Public License version 2 only, as
 * published by the Free Software Foundation.
 * 
 * Author: Lytico
 * Copyright (C) 2011-2013 Lytico
 *
 * http://www.limada.org
 * 
 */

using Limaki.Drawing;
using Xwt.Drawing;

namespace Limaki.Drawing.XwtBackend {

    /// <summary>
    /// a Surface providing a Xwt.Drawing.Context
	/// if no Matrix is set, then the Context.CTM will be used
    /// </summary>
    public class ContextSurface : ISurface {

		Context _context=null;
        public virtual Context Context { 
			get {return _context; }
			set{ _context=value; 
				_matrix = null;
				if (_context != null)
					_matrix = Context.GetCTM ();
			}
		}

        protected Matrix _matrix = null;
		/// <summary>
		/// the matrix of Context 
		/// at the time of setting this.Context
		/// </summary>
		/// <value>The matrix.</value>
		public virtual Matrix Matrix {
			get { return _matrix ?? (
							Context != null ?
							Context.GetCTM () :
							new Matrix ()); 
			}
			protected set { _matrix = value; }
		}
    }
}