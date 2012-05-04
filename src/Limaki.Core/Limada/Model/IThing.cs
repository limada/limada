/*
 * Limada 
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
using Id = System.Int64;
using Limaki.Common;


namespace Limada.Model {
    public interface IThing {
        object Data { get; set; }
        Id Id { get; }
        DateTime ChangeDate { get; }
        DateTime CreationDate { get; }

        void SetId(Id id);
        void SetCreationDate(DateTime date);
        void SetChangeDate(DateTime date);
        
        void MakeEqual ( IThing thing );
        State State { get; }
    }

    public interface IThing<T>:IThing {
        new T Data { get; set; }
    }
}
