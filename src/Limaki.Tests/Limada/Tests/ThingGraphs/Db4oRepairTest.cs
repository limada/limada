using Limaki.Tests;
using NUnit.Framework;
using Limaki.Data;
using Limada.Data.db4o;


namespace Limada.Tests.Data.db4o {
    public class Db4oRepairTest : DomainTest {
        public class Db4oRepairTester : DomainTest {
            [Test]
            public void ReadCorruptedFileTest() {
                // repairing from db4o to db4o is not working!!
                // cause same aliases are used
                var fileName = @"T:\PartsTest\verteilergesamt.limo";
                var newFile = @"T:\PartsTest\verteilergesamt.repaired.limo";
                var repairer = new Db4oRepairer();
                repairer.WriteDetail += (s,e)=>this.ReportDetail(e);
                repairer.ReadAndSaveAs(DataBaseInfo.FromFileName(fileName), DataBaseInfo.FromFileName(newFile), false);
            }
        }
    }
}
