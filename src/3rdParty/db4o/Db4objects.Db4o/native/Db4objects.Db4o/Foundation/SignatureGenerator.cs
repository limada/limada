/* This file is part of the db4o object database http://www.db4o.com

Copyright (C) 2004 - 2010  Versant Corporation http://www.versant.com

db4o is free software; you can redistribute it and/or modify it under
the terms of version 3 of the GNU General Public License as published
by the Free Software Foundation.

db4o is distributed in the hope that it will be useful, but WITHOUT ANY
WARRANTY; without even the implied warranty of MERCHANTABILITY or
FITNESS FOR A PARTICULAR PURPOSE.  See the GNU General Public License
for more details.

You should have received a copy of the GNU General Public License along
with this program.  If not, see http://www.gnu.org/licenses/. */
using System;

namespace Db4objects.Db4o.Foundation
{
    public sealed class SignatureGenerator
    {
        private static Random _random = new Random();
	
	    private static int _counter;
	
	    public static string GenerateSignature() {
            string signature = ToHexString(Environment.TickCount);
            signature += Pad(ToHexString(_random.Next()));
            signature += Guid.NewGuid();
            signature += ToHexString(_counter++);
            return signature;
	    }

        private static string ToHexString(int i)
        {
            return i.ToString("X");
        }

        private static string ToHexString(long l)
        {
            return l.ToString("X");
        }

        private static string Pad(String str)
        {
            return (str + "XXXXXXXX").Substring(0, 8);
        }

    }
}
