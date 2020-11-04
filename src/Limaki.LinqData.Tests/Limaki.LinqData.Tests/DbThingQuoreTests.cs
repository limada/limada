/*
 * Limada
 * 
 * This code is free software; you can redistribute it and/or modify it
 * under the terms of the GNU Lesser Public License version 2 only, as
 * published by the Free Software Foundation.
 * 
 * Author: Lytico
 * Copyright (C) 2014 Lytico
 *
 * http://www.limada.org
 * 
 */

using System.Linq;
using Limaki.Common.Linqish;
using NUnit.Framework;
using Limaki.Data;
using Limada.Data;
using Limada.Model;
using Limaki.Common;
using Id = System.Int64;
using System.Linq.Expressions;
using System;
using Limada.LinqData;
using Limaki.Repository;

namespace Limaki.LinqData.Tests {

    [TestFixture]
    public class DbThingQuoreTests : Limaki.Tests.DomainTest {

        public bool IsInMem => DbQuoreTestData.Iori.Provider == "InMemoryProvider";
        public bool IsMsSql => DbQuoreTestData.Iori.Provider == "MsSqlServer";
        public bool IsEf6 => IsMsSql && DbQuoreTestData.Orm == Orms.Ef6;
        public bool IsNh => IsMsSql && DbQuoreTestData.Orm == Orms.NH;
        
        public bool IsLinq2Db => IsMsSql && DbQuoreTestData.Orm == Orms.Linq2Db;

        public override void Setup () {
            base.Setup ();
            DbQuoreTestData.TearUp ();
        }

        private IDbGateway _gateway;

        public IDbGateway Gateway {
            get {
                if (_gateway == null) {
                    var iori = DbQuoreTestData.Iori;
                    if (IsLinq2Db) {
                        _gateway = new LinqToDBGateway (iori);
                    } else {
                        _gateway = new DbGateway ();
                        _gateway.Open (iori);
                    }

                   
                }

                return _gateway;
            }
        }

        public IThingQuore CreateThingQuore () {
            ThingQuoreFactory quoreFactory = null;
            if (IsInMem)
                quoreFactory = new InMemoryThingQuoreFactory ();
            // else if (IsEf6) {
            //     quoreFactory = new Ef6ThingQuore.Factory ();
            // } else if (IsNh) {
            //     quoreFactory = new NhThingQuore.Factory ();
            // }
            else if (IsLinq2Db) {
                quoreFactory = new Linq2DbThingQuoreFactory ();
            }

            return quoreFactory.CreateThingQuore (() => quoreFactory.CreateQuore (Gateway));
        }

        [Test]
        public void TestGateway () {
            var gateway = this.Gateway;
            ReportDetail (gateway.Provider.DataBaseExists (gateway.Iori).ToString ());
            if (!gateway.Provider.DataBaseExists (gateway.Iori)) {
                gateway.Provider.CreateDatabase (gateway.Iori);
            }

            if (gateway is DbGateway dbGw) {
                using (var con = dbGw.Connection) {
                    ReportDetail ("Connected to: {0}", con.ConnectionString);
                }
            }

            gateway.Close ();
        }

        [Test]
        public void TestThingQuoreExtensions () {
            QuoreExtensions.QueryableTypes<ThingQuore> ()
                .ForEach (t => {
                        ReportDetail ("{0}", t);
                    }
                );
        }

        [Test]
        public void TestIdLinkThingQuore () {
            using (var quore = CreateThingQuore ()) {
                var leaf = quore.IdLinks.Join (quore.StringThings,
                        e => e.Leaf,
                        e => e.Id,
                        (l, e) => e)
                    .FirstOrDefault ();
            }
        }

        public class AThing : IThing {

            public const int Thing = 1;
            public const int StringThing = 2;
            public const int Link = 3;
            public const int StreamThing = 5;
            public const int NumberThing = 6;

            public static Func<int, Expression<Func<IThing, AThing>>> Selector = type =>
                thing =>
                    new AThing {
                        Id = thing.Id,
                        CreationDate = thing.CreationDate,
                        ChangeDate = thing.ChangeDate,
                        Type = type,
                    };

            public static Expression<Func<IThing, AThing>> From<T> () {
                if (typeof(IThing) == typeof(T))
                    return Selector (Thing);
                if (typeof(ILink) == typeof(T))
                    return Selector (Link);
                if (typeof(IThing<string>) == typeof(T) || typeof(IStringThing) == typeof(T))
                    return Selector (StringThing);
                if (typeof(IStreamThing) == typeof(T))
                    return Selector (StreamThing);
                if (typeof(INumberThing) == typeof(T))
                    return Selector (NumberThing);
                return null;
            }

            public int Type { get; set; }

            public Id Id { get; set; }
            public DateTime CreationDate { get; set; }
            public DateTime ChangeDate { get; set; }

            public object Data { get; set; }

            public void SetId (Id id) { throw new NotImplementedException (); }

            public void SetCreationDate (DateTime date) { throw new NotImplementedException (); }

            public void SetChangeDate (DateTime date) { throw new NotImplementedException (); }

            public void MakeEqual (IThing thing) { throw new NotImplementedException (); }

            public State State => throw new NotImplementedException ();
        }

        [Test]
        public void TestAllThingsWithAThing () {
            Expression<Func<AThing, bool>> where = t => true;
            using (var quore = CreateThingQuore ()) {
                var all = quore.Things.Select (AThing.From<IThing> ()).Where (where)
                        .Union (quore.StringThings.Select (AThing.From<IStringThing> ()).Where (where))
                        .Union (quore.Links.Select (AThing.From<ILink> ()).Where (where))
                        .Union (quore.StreamThings.Select (AThing.From<IStreamThing> ()).Where (where))
                        .Union (quore.NumberThings.Select (AThing.From<INumberThing> ()).Where (where))
                    ;
                var node = all.Where (a => a.Type == AThing.Thing)
                        .Join (quore.Things, a => a.Id, c => c.Id, (a, c) => c)
                    ;
                var text = all.Where (a => a.Type == AThing.StringThing)
                        .Join (quore.StringThings, a => a.Id, c => c.Id, (a, c) => c)
                    ;
                var link = all.Where (a => a.Type == AThing.Link)
                        .Join (quore.Links, a => a.Id, c => c.Id, (a, c) => c)
                    ;
                var stream = all.Where (a => a.Type == AThing.StreamThing)
                        .Join (quore.StreamThings, a => a.Id, c => c.Id, (a, c) => c)
                    ;
                var number = all.Where (a => a.Type == AThing.NumberThing)
                        .Join (quore.NumberThings, a => a.Id, c => c.Id, (a, c) => c)
                    ;
                // cheap trick:
                var things = node.Yield ()
                    .Union (text).Yield ()
                    .Union (link).Yield ()
                    .Union (stream).Yield ()
                    .Union (number).Yield ()
                    .ToArray ();
            }
        }

        [Test]
        public void TestAllThingsWithQThing () {
            Expression<Func<IThing, bool>> where = t => true;
            using (var quore = CreateThingQuore ()) {
                var all = quore.Things.ToQThing<IThing> (where)
                        .Union (quore.StringThings.ToQThing<IThing<string>> (where))
                        .Union (quore.Links.ToQThing<ILink> (where))
                        .Union (quore.StreamThings.ToQThing<IStreamThing> (where))
                        .Union (quore.NumberThings.ToQThing<INumberThing> (where))
                    ;
                var node = all.JoinQThing (quore.Things)
                    ;
                var text = all.QThingsOf<IThing<string>> ()
                        .Join (quore.StreamThings, a => a.Id, c => c.Id, (a, c) => c)
                    ;
                var link = all.QThingsOf<ILink> ()
                        .Join (quore.Links, a => a.Id, c => c.Id, (a, c) => c)
                    ;
                var stream = all.QThingsOf<IStreamThing> ()
                        .Join (quore.StreamThings, a => a.Id, c => c.Id, (a, c) => c)
                    ;
                var number = all.QThingsOf<INumberThing> ()
                        .Join (quore.NumberThings, a => a.Id, c => c.Id, (a, c) => c)
                    ;
                // cheap trick:
                var things = node.Yield ()
                    .Union (text).Yield ()
                    .Union (link).Yield ()
                    .Union (stream).Yield ()
                    .Union (number).Yield ()
                    .ToArray ();
            }
        }

        [Test]
        public void TestThingQuore () {
            var thing = new Thing ();

            var t2 = new Thing<string> ("b");
            var m = new Thing<string> ("marker");
            var link = new Link (thing, t2, m);
            using (var quore = CreateThingQuore ()) {
                quore.Upsert (new Thing[] {thing, t2, m, link});
                var id = thing.Id;
                var r = quore.Things.FirstOrDefault (e => e.Id == id);
                Assert.IsNotNull (r);
                var idlink = quore.IdLinks.FirstOrDefault (e => e.Leaf == t2.Id);
                Assert.IsNotNull (idlink);
                var leaf = quore.IdLinks.Join (quore.StringThings,
                        e => e.Leaf,
                        e => e.Id,
                        (l, e) => e)
                    .FirstOrDefault ();
                Assert.IsNotNull (leaf);
                var things = quore.Things;
                Assert.IsNotNull (things);
            }
        }

        [Test]
        public void TestMarkers () {
            using (var quore = CreateThingQuore ()) {
                var markerIds = quore.IdLinks.Select (e => e.Marker);
                var t = quore.Things.Where (e => markerIds.Contains (e.Id));
                ReportDetail (Mono.Linq.Expressions.CSharp.ToCSharpCode (t.Expression));
                ReportDetail (t.ToArray ().Count ().ToString ());
            }
        }

    }

}