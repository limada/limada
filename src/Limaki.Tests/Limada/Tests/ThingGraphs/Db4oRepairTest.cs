using Limaki.Tests;
using NUnit.Framework;
using Limaki.Data;
using Limada.Data.db4o;


namespace Limada.Tests.Data.db4o {
    public class Db4oRepairTest : DomainTest {
        public class Db4oRepairTester : DomainTest {
            [Test]
            public void ReadCorruptedFileTest() {

                var fileName = @"E:\testdata\txbProjekt\Limaki\errors\limadacloud008.limo";
                var newFile = @"E:\testdata\txbProjekt\Limaki\errors\limadacloud008.repaired.limo";
                var repairer = new Db4oRepairer();
                repairer.WriteDetail = (e)=>this.ReportDetail(e);
                repairer.ReadAndSaveAs(DataBaseInfo.FromFileName(fileName), DataBaseInfo.FromFileName(newFile), false);
            }
        }
    }
}
