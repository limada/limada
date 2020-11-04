using System;
using System.Collections.Generic;
using Limaki.Data;
using Limaki.Tests;
using NUnit;

namespace Limaki.LinqData.Tests {
    
    class MainClass {

        public static void RunTest () {

            Action<DomainTest> setup = t => {
                t.WriteDetail += (s, m) => System.Console.WriteLine (m);
                t.WriteSummary += (s, m) => System.Console.WriteLine (m);
                t.Setup ();
            };

            Action linqDbThingContextTests = () => {
                var test = new DbThingQuoreTests ();
                setup (test);

                var createDb = false;
                //Ef6ThingModelBuilder.CheckIfModelChanged = createDb;
                if (createDb) {
                    test.TestGateway ();
                }

                test.TestThingQuore ();
                test.TearDown ();
            };

            //linqDbThingContextTests ();

            Action thingGraphTest = () => {
                var test = new ThingGraphTest ();
                setup (test);
                test.CheckInvalidLinks ();
                //test.RepeatCount = 10;
                //test.StorePerformanceTest ();
               
            };

            thingGraphTest ();

            Action schemaGraphPerformanceTest = () => {
                var test = new SchemaGraphPerformanceTest ();
                setup (test);
                test.ReadDescriptionTest ();
                test.TearDown ();
            };

        }

        public static void Main (string[] args) {
            if (true)
                RunTest ();
            else
                RunNUnitTests (args);
            System.Console.WriteLine ("done");
            //System.Console.Read ();
        }

        protected static void RunNUnitTests (string[] args) {
            var list = new List<string> (args);
            list.Add ("-domain=None");
            list.Add ("-noshadow");
            list.Add ("-nothread");
            if (!list.Contains (typeof (MainClass).Assembly.Location))
                list.Add (typeof (MainClass).Assembly.Location);
            //NUnit.ConsoleRunner.Runner.Main (list.ToArray ());
            
        }
    }
}