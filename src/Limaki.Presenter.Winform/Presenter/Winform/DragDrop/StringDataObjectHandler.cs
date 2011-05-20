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

namespace Limaki.Presenter.Winform.DragDrop {
    public class StringDataObjectHandler : VisualsDataObjectHandler<string> {
        public override string[] DataFormats {
            get { return new string[]
                    {System.Windows.Forms.DataFormats.Text}
                ; }
        }

        public override void SetData(IDataObject data, IGraph<IVisual,IVisualEdge> container,IVisual value) {
            //if ( data is DataObject ) {
            //    DataObject dataObject = (DataObject) data;
            //    dataObject.SetText(value.Data.ToString());
            //} else 
            {
                data.SetData(System.Windows.Forms.DataFormats.Text, value.Data.ToString());
                data.SetData(System.Windows.Forms.DataFormats.UnicodeText, value.Data.ToString());
            }
        }
    }
}