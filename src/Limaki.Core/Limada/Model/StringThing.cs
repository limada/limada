/*
 * Limada 
 * 
 * This code is free software; you can redistribute it and/or modify it
 * under the terms of the GNU General Public License version 2 only, as
 * published by the Free Software Foundation.
 * 
 * Author: Lytico
 * Copyright (C) 2010 Lytico
 *
 * http://www.limada.org
 * 
 */

using System.Runtime.Serialization;
using Id = System.Int64;

namespace Limada.Model {

    public interface IStringThing : IThing<string> {}

    [DataContract]
    public class StringThing : Thing<string>, IStringThing {

        protected StringThing () : base () { }
        
        public StringThing (string data) : base (data) { }

        public StringThing (Id id, string data) : base (id, data) { }
    }
}