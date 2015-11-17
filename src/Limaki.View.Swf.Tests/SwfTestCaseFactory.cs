using System.Windows.Forms;
using Limada.Usecases;
using Limada.UseCases;
using Limaki.Tests.View.Winform;

namespace Limaki.Tests.UseCases {

    public class SwfTestCaseFactory :TestCaseFactoryBase {

        public override void Compose (ConceptUsecase useCase) {
            base.Compose (useCase);
            var deviceComposer = BackendComposer as SwfConceptUseCaseComposer;
            CreateTestCases (useCase, deviceComposer);
        }

        public void CreateTestCases (ConceptUsecase useCase, SwfConceptUseCaseComposer composer) {

            composer.MenuStrip.Items.AddRange (
                new ToolStripMenuItem[] {

                    new ToolStripMenuItem ("Test", null, new ToolStripMenuItem[] {

                        new ToolStripMenuItem ("Open Testcase...", null, (s, e) => {
                            this.ExampleOpen (useCase);
                                                                        }),
                        new ToolStripMenuItem ("Selector", null, (s, e) => {
                            var test = DisplayTest();
                            test.RunSelectorTest();
                            test.TearDown();
                        }),
                        new ToolStripMenuItem ("BenchmarkOne", null, (s, e) => {
                            var test = DisplayTest();
                            test.MoveAlongSceneBoundsTest();
                            test.TearDown();
                        }),
                        new ToolStripMenuItem ("QuadTree", null, (s, e) => {
                            this.ShowQuadTree (useCase.GetCurrentDisplay().Data);
                        }),
                        new ToolStripMenuItem ("WCF", null, (s, e) => {
                            this.WCFServiceTest (useCase);
                        }),
                        new ToolStripMenuItem ("SchemaFilter off", null, (s, e) => {
                            this.NoSchemaThingGraph (useCase);
                        }),
                        new ToolStripMenuItem("Database refresh Compression", null, (s, e) => {
                            useCase.RefreshCompression();
                        }), 
                        new ToolStripMenuItem ("Timeline", null, (s, e) => {
                             useCase.TimelineSheet ();
                        }),
                        new ToolStripMenuItem ("current problem", null, (s, e) => {
                            this.CurrentProblem (useCase);
                        }),
                    })
               });

        }


        public void ExampleOpen (ConceptUsecase useCase) {

            var dialog = new OpenExampleData ();
            if (dialog.ShowDialog () == System.Windows.Forms.DialogResult.OK) {
                var testData = dialog.SceneExamples.Selected;
                testData.Data.Count = (int)dialog.numericUpDown1.Value;
                var scene = dialog.SceneExamples.GetScene (testData.Data);
                useCase.SplitView.ChangeData (scene);
            }
            dialog.Dispose ();

        }


    }
}