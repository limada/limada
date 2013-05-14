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

using System.Collections.Generic;
using Limaki.Graphs;
using Limaki.Visuals;
using Xwt;

namespace Limaki.View.Ui.DragDrop {
    public class TranferDataHandlerChain {
        public IList<IVisualsTransferDataHandler> TransferDataHandlers =
            new List<IVisualsTransferDataHandler>();

        public virtual bool IsValidData (TransferDataSource data) {
            return GetDataObjectHandler(data, true) != null;
        }

        public virtual IVisualsTransferDataHandler GetDataObjectHandler (TransferDataSource data, bool inProcess) {
            IVisualsTransferDataHandler result = null;
            foreach (var dataHandler in TransferDataHandlers) {
                bool isAInprocHandler =
                    dataHandler is TransferDataInprocHandler<IGraph<IVisual, IVisualEdge>, IVisual>;
                bool doTry = false;
                if ((inProcess && isAInprocHandler) || (!inProcess && !isAInprocHandler))
                    doTry = data.GetDataPresent(dataHandler.HandledType);
                if (!doTry)
                    foreach (string format in dataHandler.Formats)
                        if (data.GetDataPresent(format)) {
                            doTry = true;
                            break;
                        }
                if (doTry) {
                    result = dataHandler;
                    break;
                }

            }
            return result;
        }

        public virtual object GetData (TransferDataSource data) {
            object result = false;
            foreach (var dataHandler in TransferDataHandlers) {
                if (data.GetDataPresent(dataHandler.HandledType)) {
                    result = data.GetData(dataHandler.HandledType);
                    break;
                }
            }
            return result;
        }

        public virtual IVisual GetVisual (TransferDataSource data, IGraph<IVisual, IVisualEdge> graph, bool inProcess) {
            IVisual result = null;
            var dataHandler = GetDataObjectHandler(data, inProcess);
            if (dataHandler != null) {
                result = dataHandler.GetData(data, graph);
            }
            return result;
        }

        public virtual void SetVisual (TransferDataSource data, IGraph<IVisual, IVisualEdge> graph, IVisual value) {
            foreach (var dataHandler in TransferDataHandlers) {
                dataHandler.SetData(data, graph, value);
            }
        }

        public void InitDataObjectHanders () {
            TransferDataHandlers.Add(new VisualsTransferInprocDataHandler());
            TransferDataHandlers.Add(new SerializedVisualsTransferDataHandler());
            TransferDataHandlers.Add(new VisualsImageTransferDataHandler());
            TransferDataHandlers.Add(new VisualsTextTransferDataHandler());
            TransferDataHandlers.Add(new TextTransferDataHandler());

        }
    }
}