using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Limaki.Tests {
    public class TestLocations {
        public static string TestDataDir {
            get { return OS.Unix ? "../../TestData/" : @"..\..\testdata\"; }
        }
        public static string GraphtestDir {
            get { return OS.Unix ? TestDataDir : TestDataDir + @"Limaki\"; }
        }

        public static string GraphtestFile = GraphtestDir + "graphtest";

        public static string BlobSource = TestDataDir + @"BlobSource";

        public static string ErrorsDir = GraphtestDir + Path.DirectorySeparatorChar + "errors" + Path.DirectorySeparatorChar;
        public static string LimadaCloud008 = ErrorsDir + "limadacloud008.limo";
        public static string LimadaCloud008Repaired = ErrorsDir + "limadacloud008.repaired.limo";

        public static string AdressExample_pib = TestDataDir + @"ADRESSEXAMPLE.pib";

        public static string Sample4_pib = TestDataDir+@"sample_4.pib";
        public static string Sample4_Persons = "personen";
        public static long Sample4_Node1 = unchecked((long)0x0D0A3C7B7F6A4D22);

    }
}
