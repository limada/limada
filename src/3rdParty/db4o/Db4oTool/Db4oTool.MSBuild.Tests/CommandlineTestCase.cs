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
using Db4oUnit;
using Db4oUnit.Extensions;

namespace Db4oTool.MSBuild.Tests
{
    public class CommandlineTestCase : AbstractDb4oTestCase
    {
        public void testByName()
        {
            Type intItemType = typeof(IntItem);
            Assert.IsTrue(IsInstrumented(intItemType));

            Type nonTAItemType = typeof(NonTAItem);
            Assert.IsFalse(IsInstrumented(nonTAItemType));
        }

        public void testByName2()
        {
            Type taIntItemType = Type.GetType("Db4oTool.MSBuild.Tests.Project.TAIntItem, Db4oTool.MSBuild.Tests.Project", true);

            Assert.IsTrue(IsInstrumented(taIntItemType));

            Type stringItemType = Type.GetType("Db4oTool.MSBuild.Tests.Project.StringItem, Db4oTool.MSBuild.Tests.Project", true);
            Assert.IsFalse(IsInstrumented(stringItemType));
        }

        private bool IsInstrumented(Type type)
        {
            Type t = type.GetInterface("Db4objects.Db4o.TA.IActivatable");
            return (t != null);
        }
    }
}
