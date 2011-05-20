/*
 * Limaki 
 * Version 0.081
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

using System.Windows.Forms;
using Limaki.Graphs;
using Limaki.Visuals;
using Limaki.Common;

namespace Limaki.Presenter.Winform.DragDrop {
    public abstract class VisualsDataObjectHandler<OUT> : DataObjectHandler<IGraph<IVisual, IVisualEdge>, IVisual, OUT>, IVisualsDataObjectHandler {
        public override IVisual GetData(IDataObject data, IGraph<IVisual,IVisualEdge> graph) {
            object odata = data.GetData(HandledType);
            IVisual result = null;
            if (odata is OUT) {
                result = Registry.Pool.TryGetCreate<IVisualFactory>().CreateItem< OUT > ((OUT)odata);
            }
            return result;
        }
    }
}