/*
 * Limaki 
 * 
 * This code is free software; you can redistribute it and/or modify it
 * under the terms of the GNU General Public License version 2 only, as
 * published by the Free Software Foundation.
 * 
 * Author: Lytico
 * Copyright (C) 2013 Lytico
 *
 * http://www.limada.org
 * 
 */

using System.Linq;
using Limaki.View.DragDrop;
using Xwt;

namespace Limaki.View.SwfBackend.DragDrop {

    public class SwfTransferDataInfo : ITransferDataInfo {

        public SwfTransferDataInfo (System.Windows.Forms.IDataObject dob) {
            this.Dob = dob;
        }

        public bool HasType (TransferDataType typeId) { return Dob.GetFormats().Any(t => t.ToLower() == typeId.Id.ToLower()); }

        public System.Windows.Forms.IDataObject Dob { get; set; }
    }
}