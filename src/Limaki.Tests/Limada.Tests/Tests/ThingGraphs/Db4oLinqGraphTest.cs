using Limada.Data;
using Db4objects.Db4o.Linq;
using Limada.Model;
using NUnit.Framework;
using System.Linq;

namespace Limada.Tests.Data.db4o {
    public class Db4oLinqGraphTest : Limada.Tests.ThingGraphs.ThingGraphTest {
        public override void Setup() {
            this.ThingGraphProvider = new Db4oThingGraphIo();
            base.Setup();
        }
        [Test]
        public void LinqTest() {
            
            
            var graph = this.Graph as Limada.Data.db4o.ThingGraph;
            Assert.NotNull(graph);
            var count = (from IThing item in graph.Session
                         select item).Count();

            ReportDetail(count.ToString());
            if (count == 0)
                base.StandardGraphTest();

            var l = from IThing item in graph.Session
                    select item;
            
            foreach (var item in l)
                ReportDetail(item.ToString());


            var links = from ILink item in graph.Session
                        select item;
            ReportDetail("** links");
            foreach (var item in links)
                ReportDetail(item.ToString());

            ReportDetail("** querable");
            var ll = graph.Session.AsQueryable<IThing>().Select(e=>e.Id);
            foreach (var item in ll)
                ReportDetail(item.ToString());

            ReportDetail("** graph.where");
            var lll = graph.Where(e => e.Data != null && e is IThing<string>);
            foreach (var item in lll)
                ReportDetail(item.ToString());
            var llll = graph.Where<IThing<string>>(e => e.Data != null);
            ReportDetail("** graph.graph.Where<IThing<string>>");
            foreach (var item in llll)
                ReportDetail(item.ToString());
        }
    }
}