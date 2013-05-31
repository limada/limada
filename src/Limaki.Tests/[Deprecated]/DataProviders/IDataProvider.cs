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


using Limaki.Common;
using System;
using Limaki.Model.Content.IO;
namespace Limaki.Data {

    public interface IDataProvider<T>:IProgress {

        string Extension { get; }
        string Description { get; }

        bool Saveable { get; }
        bool Readable { get; }

        T Data { get; set; }
        void Open ( IoInfo fileName );
        void Open();

        void Save();
        void SaveAs(T source, IoInfo fileName);
        void Merge (T source, T sink);
        void Close();
        void SaveCurrent();
        IDataProvider<T> Clone();
        Action<string,int,int> Progress { get; set; }
    }
	
	
}