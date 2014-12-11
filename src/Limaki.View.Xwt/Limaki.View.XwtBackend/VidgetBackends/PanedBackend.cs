/*
 * Limaki 
 * 
 * This code is free software; you can redistribute it and/or modify it
 * under the terms of the GNU General Public License version 2 only, as
 * published by the Free Software Foundation.
 * 
 * Author: Lytico
 * Copyright (C) 2014 Lytico
 *
 * http://www.limada.org
 * 
 */

using System.Linq;
using Xwt;

namespace Limaki.View.XwtBackend {

    public abstract class PanedBackend : VidgetBackend<HPaned>  {

        protected Paned SplitContainer { get { return this.Widget; } }

        protected virtual Widget SetScrollingPanelContent (Widget widget, Panel panel) {
            if (widget is IScrollContainingWidget) {
                panel.Content = widget;
            } else {
                var panelScroll = panel.Content as ScrollView;
                if (panelScroll != null) {
                    panelScroll.Content = widget;
                } else {
                    panel.Content = widget.WithScrollView ();
                }
            }
            return panel.Content;
        }

        protected Panel PanelOf (IVidget vidget) {
            var widget = vidget.Backend.ToXwt ();

            if (SplitContainer.Panel1.Content.ScrollPeeledChildren ().Contains (widget)) {
                return SplitContainer.Panel1;
            } else if (SplitContainer.Panel2.Content.ScrollPeeledChildren ().Contains (widget)) {
                return SplitContainer.Panel2;
            }
            return null;
        }

        protected Panel AdjacentPanelOf (IVidget vidget) {
            var widget = vidget.Backend.ToXwt ();

            if (SplitContainer.Panel1.Content.ScrollPeeledChildren ().Contains (widget)) {
                return SplitContainer.Panel2;
            } else if (SplitContainer.Panel2.Content.ScrollPeeledChildren ().Contains (widget)) {
                return SplitContainer.Panel1;
            }
            return null;
        }
    }
}