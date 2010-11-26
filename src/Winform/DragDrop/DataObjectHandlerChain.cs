/*
 * Limaki 
 * Version 0.07
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
using Limaki.Winform.Widgets;

namespace Limaki.Winform.DragDrop {
    public class DataObjectHandlerChain {
        public IList<IWidgetDataObjectHandler> DataObjectHandlers =
            new List<IWidgetDataObjectHandler>();

        public virtual bool IsValidData(IDataObject data) {
            bool result = false;
            foreach (IWidgetDataObjectHandler objectHandler in DataObjectHandlers) {
                if (data.GetDataPresent(objectHandler.HandledType)) {
                    result = true;
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
        public virtual IWidget GetWidget(IDataObject data) {
            IWidget result = null;
            foreach (IWidgetDataObjectHandler objectHandler in DataObjectHandlers) {
                if (data.GetDataPresent(objectHandler.HandledType)) {
                    result = objectHandler.GetData(data);
                    break;
                }
            }
            return result;
        }
        public virtual void SetData(IDataObject data, IWidget value) {
            foreach (IWidgetDataObjectHandler objectHandler in DataObjectHandlers) {
                objectHandler.SetData(data, value);
            }
        }
        public void InitDataObjectHanders() {
            DataObjectHandlers.Add(new WidgetWidgetDataObjectHandler());
            DataObjectHandlers.Add(new StringDataObjectHandler());
        }
    }
}