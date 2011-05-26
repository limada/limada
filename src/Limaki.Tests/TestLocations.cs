using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Limaki.Tests {
    public class TestLocations {
        public const string TestDataDir = @"..\..\..\testdata";

        public static string GraphtestFile {
            get {
                if (OS.Unix)
                    return "../../../testdata/graphtest";
                //Environment.SpecialFolder.MyDocuments+Path.DirectorySeparatorChar+
                else
                    return TestDataDir + @"\graphtest";
            }
        }
        
        public const string BlobSource = TestDataDir + @"\BlobSource";

        public const string LimadaCloud008 = TestDataDir+@"\errors\limadacloud008.limo";
        public const string LimadaCloud008Repaired = TestDataDir+@"\errors\limadacloud008.repaired.limo";

        public const string AdressExample_pib = TestDataDir + @"\ADRESSEXAMPLE.pib";

        public const string Sample4_pib = TestDataDir + @"\sample_4.pib";
        public const string Sample4_Persons = "personen";
        public const long Sample4_Node1 = unchecked((long)0x0D06589322AAA);
        
    }
}
