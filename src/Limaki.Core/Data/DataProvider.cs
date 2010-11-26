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


namespace Limaki.Data {
    /// <summary>
    /// encapsulates the opening of a datasource
    /// </summary>
    public abstract class DataProvider<T> : IDataProvider<T> {

        public abstract string Extension { get; }
        public abstract string Description { get; }
        public abstract bool Saveable { get; }
        public abstract T Data { get; set; }
        public abstract void Open(DataBaseInfo FileName);
        public abstract void Open();
        public abstract void Save();
        public abstract void SaveAs(DataBaseInfo FileName);
        public abstract void Close();
        public abstract void SaveCurrent();
        public abstract IDataProvider<T> Clone();
    }
}