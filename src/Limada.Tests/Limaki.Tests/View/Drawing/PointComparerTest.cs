using System;
using System.Diagnostics;
using NUnit.Framework;

namespace Limaki.Tests.Drawing {
    public class PointComparerTest : DomainTest {
        [Test]
        public void TestRound () {
            Action<double, double> round = (val,delta)=> {
                var r = (int)(val / delta) * delta;
                Trace.WriteLine(string.Format("{0} d:{1} =  {2}",val,delta,r));
            };
            round (39d, 40d);
            round (40d, 40d);
            round(45d, 40d);
            round (26d, 40d);
            round (64d, 40d);
            
        }
    }
}