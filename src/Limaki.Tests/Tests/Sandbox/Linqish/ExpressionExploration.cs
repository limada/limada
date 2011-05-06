/*
 * Limada
 * 
 * This code is free software; you can redistribute it and/or modify it
 * under the terms of the GNU Lesser Public License version 2 only, as
 * published by the Free Software Foundation.
 * 
 * Author: Lytico
 * Copyright (C) 2011 Lytico
 *
 * http://limada.sourceforge.net
 * 
 */

using System;
using System.Collections.Generic;
using System.Linq;
using Limada.Common.Linqish;
using NUnit.Framework;

namespace Limada.Tests.Linqish {
    [TestFixture]
    public class ExpressionExplorationTest  {
        [Test]
        public void TestMemberExpressonBuilder() {
            var now = DateTime.Now;
            var list = (new MyClass[] {
                new MyClass {MyString = "4",MyDateTime=now.AddDays(4)},
                new MyClass {MyString = "3",MyDateTime=now.AddDays(3)},
                new MyClass {MyString = "8",MyDateTime=now.AddDays(8)},
                new MyClass {MyString = "1",MyDateTime=now.AddDays(1)},
            }).AsQueryable();

            var builder = new MemberByNameExpressionBuilder();
            foreach (var item in builder.OrderByKey(list, e => e.MyString))
                System.Console.WriteLine(item.MyString);

            System.Console.WriteLine("MemberByName.Ascending");

            Assert.IsTrue(builder.IsValidMemberName<MyClass>("MyString"));

            Assert.IsFalse(builder.IsValidMemberName<MyClass>("SomeThingStupid"));
            Assert.Throws(typeof (ArgumentException), () => builder.OrderByMemberName(list, "SomeThingStupid", false));

            foreach (var item in builder.OrderByMemberName(list, "MyString",false))
                System.Console.WriteLine(item.MyString);

            System.Console.WriteLine("MemberByName.Descending");
            foreach (var item in builder.OrderByMemberName(list, "MyString", true))
                System.Console.WriteLine(item.MyString);

            var result1 = list.OrderBy(e => e.MyString).ToArray();
            var result2 = builder.OrderByMemberName(list, "MyString", false).ToArray();
            Prove(result1, result2, e => e.MyString);

            result1 = list.OrderByDescending(e => e.MyString).ToArray();
            result2 = builder.OrderByMemberName(list, "MyString", true).ToArray();
            Prove(result1, result2, e => e.MyString);

            var result3 = list.OrderBy(e => e.MyDateTime).ToArray();
            var result4 = builder.OrderByMemberName(list, "MyDateTime", false).ToArray();
            Prove(result3, result4, e => e.MyDateTime);

            result3 = list.OrderByDescending(e => e.MyDateTime).ToArray();
            Assert.IsTrue(builder.IsValidMemberName<MyClass>("MyDateTime"));
            result4 = builder.OrderByMemberName(list, "MyDateTime", true).ToArray();
            Prove(result3, result4, e => e.MyDateTime);

            var list1 = list.Select(e => new {NewNameOfMember = e.MyDateTime});
            var result5 = list1.OrderBy(e => e.NewNameOfMember).ToArray();
            var result6 = builder.OrderByMemberName(list1, "NewNameOfMember", false).ToArray();
            Prove(result5, result6, e => e.NewNameOfMember);
        }
      
        public void Prove<T,K>(IList<T> result1, IList<T> result2, Func<T,K> member) {
            for (int i = 0; i < result1.Count; i++)
                Assert.AreEqual(member(result1[i]), member(result2[i]));
        }

        [Test]
        public void TestSelectMany() {
            var now = DateTime.Now;
            var list1 = (new MyClass[] {
                new MyClass {MyString = "4",MyDateTime=now.AddDays(4)},
                new MyClass {MyString = "3",MyDateTime=now.AddDays(3)},
                new MyClass {MyString = "8",MyDateTime=now.AddDays(8)},
                new MyClass {MyString = "1",MyDateTime=now.AddDays(1)},
            }).AsQueryable();
            now.AddYears(1);
            var list2 = (new MyClass[] {
                new MyClass {MyString = "4",MyDateTime=now.AddDays(4)},
                new MyClass {MyString = "3",MyDateTime=now.AddDays(3)},
                new MyClass {MyString = "8",MyDateTime=now.AddDays(8)},
                new MyClass {MyString = "1",MyDateTime=now.AddDays(1)},
            }).AsQueryable();

            var result = from c1 in list1
                         from c2 in list2
                         where c1.MyString == c2.MyString
                         select new {c1.MyString, c1.MyDateTime, MyDate2 = c2.MyDateTime};


            var ex = result.Expression;
            System.Console.WriteLine(ex.ToString());
            foreach (var item in result)
                System.Console.WriteLine(item.MyString);

            var result1 = list1.SelectMany(
                c1 => list2,
                (c1, c2) => new { c1, c2 })
                .Where(s => s.c1.MyString == s.c2.MyString)
                .Select(s => new { s.c1.MyString, s.c1.MyDateTime, MyDate2 = s.c2.MyDateTime });

        }
    }

    public class MyClass {
        public string MyString { get; set; }
        public DateTime MyDateTime { get; set; }
    }
}