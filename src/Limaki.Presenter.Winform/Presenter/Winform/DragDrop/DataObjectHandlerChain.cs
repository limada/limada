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

using System.Collections.Generic;
using System.Windows.Forms;
using Limaki.Widgets;
using Limaki.Graphs;


namespace Limaki.Presenter.Winform.DragDrop {
    public class DataObjectHandlerChain {
        public IList<IWidgetDataObjectHandler> DataObjectHandlers =
            new List<IWidgetDataObjectHandler>();

        public virtual bool IsValidData(IDataObject data) {
            return GetDataObjectHandler(data,true) != null;
        }

        public virtual IWidgetDataObjectHandler GetDataObjectHandler(IDataObject data, bool inProcess) {
            IWidgetDataObjectHandler result = null;
            foreach (IWidgetDataObjectHandler objectHandler in DataObjectHandlers) {
                bool isAInprocHandler =
                    objectHandler is DataInprocObjectHandler<IGraph<IWidget, IEdgeWidget>, IWidget>;
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
            foreach (IWidgetDataObjectHandler objectHandler in DataObjectHandlers) {
                if (data.GetDataPresent(objectHandler.HandledType)) {
                    result = data.GetData(objectHandler.HandledType);
                    break;
                }
            }
            return result;
        }

        public virtual IWidget GetWidget(IDataObject data, IGraph<IWidget,IEdgeWidget> graph, bool inProcess) {
            IWidget result = null;
            IWidgetDataObjectHandler objectHandler = GetDataObjectHandler (data,inProcess);
            if (objectHandler !=null) {
                result = objectHandler.GetData (data, graph);
            }
            return result;
        }

        public virtual void SetWidget(IDataObject data, IGraph<IWidget, IEdgeWidget> graph, IWidget value) {
            foreach (IWidgetDataObjectHandler objectHandler in DataObjectHandlers) {
                objectHandler.SetData(data, graph,value);
            }
        }

        public void InitDataObjectHanders() {
            DataObjectHandlers.Add(new WidgetInprocDataObjectHandler());
            DataObjectHandlers.Add(new SerializedWidgetDataObjectHandler());
            DataObjectHandlers.Add(new WidgetImageDataObjectHandler());
            DataObjectHandlers.Add(new WidgetTextDataObjectHandler());
            DataObjectHandlers.Add(new StringDataObjectHandler());
            
        }
    }
}