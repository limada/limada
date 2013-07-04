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


using System;
using Xwt;
using Xwt.Drawing;

namespace Limaki.View.Ui.DragDrop0 {
    public static class TransferDataSourceExensions {
        public static object GetData(this TransferDataSource data, string format) {
            throw new NotImplementedException();
        }

        public static byte[] GetData (this TransferDataSource data, string format, int index) {
            throw new NotImplementedException();
        }

        public static object GetData (this TransferDataSource data, Type format) {
            throw new NotImplementedException();
        }

        public static void SetData (this TransferDataSource data, object value) {
            throw new NotImplementedException();
        }

        public static void SetData (this TransferDataSource data, string format, object value) {
            throw new NotImplementedException();
        }

        public static void SetData (this TransferDataSource data, Type format, object value) {
            throw new NotImplementedException();
        }

        public static bool ContainsText (this TransferDataSource data, TextTransferDataFormat format) {
            throw new NotImplementedException();
        }

        public static bool ContainsImage (this TransferDataSource data) {
            throw new NotImplementedException();
        }
        public static Image GetImage (this TransferDataSource data) {
            throw new NotImplementedException();
        }
        public static string GetText (this TransferDataSource data, TextTransferDataFormat format) {
            throw new NotImplementedException();
        }

        public static bool GetDataPresent (this TransferDataSource data, string format) {
            throw new NotImplementedException();
        }

        public static bool GetDataPresent (this TransferDataSource data, Type format) {
            throw new NotImplementedException();
        }
    }
}