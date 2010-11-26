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


using System;
namespace Limaki.Data {
    public interface IDataProvider<T> {
        string Extension { get; }
        string Description { get; }
        T Data { get; set; }
        void Open ( DataBaseInfo FileName );
        void Open();
        bool Saveable { get; }
        void Save();
        void SaveAs(T source, DataBaseInfo FileName);
        void Close();
        void SaveCurrent();
        IDataProvider<T> Clone();
    }
	
	
}