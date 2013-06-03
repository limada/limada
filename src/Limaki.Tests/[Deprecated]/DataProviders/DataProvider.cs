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
 * http://www.limada.org
 * 
 */


using System;
using Limaki.Model.Content;
using Limaki.Model.Content.IO;
namespace Limaki.Data {
    /// <summary>
    /// encapsulates the opening of a datasource
    /// </summary>
    public abstract class DataProvider<T> : IDataProvider<T> {

        public abstract T Data { get; set; }

        public abstract string Extension { get; }
        public abstract string Description { get; }
        public abstract bool Saveable { get; }
        public abstract bool Readable { get; }
       
        public abstract void Open(Iori fileName);
        public abstract void Open();
        public abstract void Save();
        public abstract void SaveAs(T source, Iori fileName);
        public abstract void Merge (T source, T sink);
        public abstract void Close();
        public abstract void SaveCurrent();
        public abstract IDataProvider<T> Clone();
       
        public Action<string, int, int> Progress { get; set; }


    }
}