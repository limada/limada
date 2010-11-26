/*
 * Limaki 
 * Version 0.063
 * 
 * This code is free software; you can redistribute it and/or modify it
 * under the terms of the GNU General Public License version 2 only, as
 * published by the Free Software Foundation.
 * 
 * Author: Lytico
 * Copyright (C) 2006-2008 Lytico
 *
 * http://limada.sourceforge.net
 * 
 */

using Limaki.Drawing;

namespace Limaki.Widgets.Layout {
    public class RouterBase : IRouter {
        public virtual void routeLink(ILinkWidget link) {
            if (link.Root is ILinkWidget) {
                link.RootAnchor = Anchor.Center;
            }
            if (link.Leaf is ILinkWidget) {
                link.LeafAnchor = Anchor.Center;
            }
        }
    }
}