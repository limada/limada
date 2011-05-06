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
using System.Collections.Generic;
using System.Text;

namespace Db4oTool.MSBuild.Tests
{
    public class IntItem
    {
        public int _intValue;
        public IntItem _next;
        public bool _isRoot;

        public static IntItem NewIntItem(int depth)
        {
            if (depth == 0)
            {
                return null;
            }

            IntItem root = new IntItem();
            root._intValue = depth;
            root._next = NewIntItem(depth - 1);
            return root;
        }

        public int GetIntValue()
        {
            return _intValue;
        }

        public IntItem Next()
        {
            return _next;
        }
    }
}
