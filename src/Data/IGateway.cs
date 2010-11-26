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

using System;
namespace Limaki.Data {
    public interface IGateway {
        void Close();
        DataBaseInfo DataBaseInfo { get; set; }
        string FileExtension { get; }
        bool IsOpen();
        void Open( DataBaseInfo dataBaseInfo );
    }
}
