/*
 * Limaki 
 * 
 * This code is free software; you can redistribute it and/or modify it
 * under the terms of the GNU General Public License version 2 only, as
 * published by the Free Software Foundation.
 * 
 * Author: Lytico
 * Copyright (C) 2006-2013 Lytico
 *
 * http://www.limada.org
 * 
 */

using Limaki.Common;
using Limaki.Graphs;
using Limaki.Visuals;
using Xwt;

namespace Limaki.View.Ui.DragDrop {

    public interface IVisualsTransferDataHandler : ITransferDataHandler<IGraph<IVisual, IVisualEdge>, IVisual> { }

    public abstract class VisualsTransferDataHandler<OUT> : TransferDataHandler<IGraph<IVisual, IVisualEdge>, IVisual, OUT>, IVisualsTransferDataHandler {
        public override IVisual GetData (TransferDataSource data, IGraph<IVisual, IVisualEdge> graph) {
            object odata = data.GetData(HandledType);
            IVisual result = null;
            if (odata is OUT) {
                result = Registry.Pool.TryGetCreate<IVisualFactory>().CreateItem<OUT>((OUT)odata);
            }
            return result;
        }
    }

    public class SerializedVisualsTransferDataHandler : SerializedTransferDataHandler<IGraph<IVisual, IVisualEdge>, IVisual>, IVisualsTransferDataHandler {
        public override string[] Formats {
            get { return new string[] { "Limaki_IVisual-Binary" }; }
        }
    }

    public class VisualsTransferInprocDataHandler : TransferDataInprocHandler<IGraph<IVisual, IVisualEdge>, IVisual>, IVisualsTransferDataHandler {
        public override string[] Formats {
            get { return new string[] { "Limaki/IVisual-Native" }; }
        }
    }
}