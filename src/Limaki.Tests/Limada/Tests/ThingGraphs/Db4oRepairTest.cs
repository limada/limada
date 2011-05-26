using Limaki.Tests;
using NUnit.Framework;
using Limaki.Data;
using Limada.Data.db4o;


namespace Limada.Tests.Data.db4o {
    public class Db4oRepairTest : DomainTest {
        public class Db4oRepairTester : DomainTest {
            [Test]
            public void ReadCorruptedFileTest() {

                var fileName = TestLocations.LimadaCloud008;
                var newFile = TestLocations.LimadaCloud008Repaired;
                var repairer = new Db4oRepairer();
                repairer.WriteDetail = (e)=>this.ReportDetail(e);
                repairer.ReadAndSaveAs(DataBaseInfo.FromFileName(fileName), DataBaseInfo.FromFileName(newFile), false);
            }
        }
    }
}
