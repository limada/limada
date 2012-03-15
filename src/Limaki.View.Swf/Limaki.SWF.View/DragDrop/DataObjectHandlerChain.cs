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

using System.Collections.Generic;
using System.Windows.Forms;
using Limaki.Graphs;
using Limaki.Visuals;


namespace Limaki.View.Winform.DragDrop {
    public class DataObjectHandlerChain {
        public IList<IVisualsDataObjectHandler> DataObjectHandlers =
            new List<IVisualsDataObjectHandler>();

        public virtual bool IsValidData(IDataObject data) {
            return GetDataObjectHandler(data,true) != null;
        }

        public virtual IVisualsDataObjectHandler GetDataObjectHandler(IDataObject data, bool inProcess) {
            IVisualsDataObjectHandler result = null;
            foreach (IVisualsDataObjectHandler objectHandler in DataObjectHandlers) {
                bool isAInprocHandler =
                    objectHandler is DataInprocObjectHandler<IGraph<IVisual, IVisualEdge>, IVisual>;
                bool doTry = false;
                if (( inProcess && isAInprocHandler ) || ( !inProcess && !isAInprocHandler ))
                    doTry = data.GetDataPresent (objectHandler.HandledType);
                if (!doTry)
                    foreach (string format in objectHandler.DataFormats)
                        if (data.GetDataPresent (format)) {
                            doTry = true;
                            break;
                        }
                if (doTry) {
                    result = objectHandler;
                    break;
                }

            }
            return result;
        }

        public virtual object GetData(IDataObject data) {
            object result = false;
            foreach (IVisualsDataObjectHandler objectHandler in DataObjectHandlers) {
                if (data.GetDataPresent(objectHandler.HandledType)) {
                    result = data.GetData(objectHandler.HandledType);
                    break;
                }
            }
            return result;
        }

        public virtual IVisual GetVisual(IDataObject data, IGraph<IVisual,IVisualEdge> graph, bool inProcess) {
            IVisual result = null;
            IVisualsDataObjectHandler objectHandler = GetDataObjectHandler (data,inProcess);
            if (objectHandler !=null) {
                result = objectHandler.GetData (data, graph);
            }
            return result;
        }

        public virtual void SetVisual(IDataObject data, IGraph<IVisual, IVisualEdge> graph, IVisual value) {
            foreach (IVisualsDataObjectHandler objectHandler in DataObjectHandlers) {
                objectHandler.SetData(data, graph,value);
            }
        }

        public void InitDataObjectHanders() {
            DataObjectHandlers.Add(new VisualsInprocDataObjectHandler());
            DataObjectHandlers.Add(new SerializedVisualsDataObjectHandler());
            DataObjectHandlers.Add(new VisualsImageDataObjectHandler());
            DataObjectHandlers.Add(new VisualsTextDataObjectHandler());
            DataObjectHandlers.Add(new StringDataObjectHandler());
            
        }
    }
}