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
using Db4objects.Db4o;
using Db4objects.Db4o.Config;
using Db4objects.Db4o.TA;
using Db4objects.Db4o.Query;
using Db4objects.Db4o.Reflect;
using Db4oUnit;
using Db4oUnit.Extensions;

namespace Db4oTool.MSBuild.Tests
{
    class IntItemTestCase : AbstractDb4oTestCase
    {
        protected override void Configure(IConfiguration config)
        {
            config.Add(new TransparentActivationSupport());
        }

        private const int DEPTH = 10;

        protected override void Store()
        {
            IntItem item = IntItem.NewIntItem(DEPTH);
            item._isRoot = true;
            Store(item);
        }

        public void Test()
        {
            IntItem item = RetrieveRoot();
            Assert.IsNotNull(item);

            for (int i = 0; i < DEPTH - 1; i++)
            {
                Assert.AreEqual(0, GetField(item, "_intValue"));
                Assert.IsNull(GetField(item, "_next"));
                Assert.AreEqual(DEPTH - i, item.GetIntValue());
                Assert.IsNotNull(item.Next());
                Assert.AreEqual(DEPTH - i, GetField(item, "_intValue"));
                Assert.IsNotNull(GetField(item, "_next"));
                item = item.Next();
            }

            Assert.AreEqual(0, GetField(item, "_intValue"));
            Assert.IsNull(GetField(item, "_next"));
            Assert.AreEqual(1, item.GetIntValue());
            Assert.IsNull(GetField(item, "_next"));
        }

        private IntItem RetrieveRoot()
        {
            IQuery query = Db().Query();
            query.Constrain(typeof(IntItem));
            query.Descend("_isRoot").Constrain(true);
            IObjectSet result = query.Execute();
            return result.HasNext() ? (IntItem)result.Next() : null;
        }

        private object GetField(object obj, string fieldName)
        {
            IReflectClass clazz = Reflector().ForObject(obj);
            IReflectField field = clazz.GetDeclaredField(fieldName);
            return field.Get(obj);
        }
    }
}
