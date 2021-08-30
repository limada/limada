using System;
using System.Diagnostics;
using System.IO;
using Limada.Usecases;
using Limaki.Common;
using Limaki.View.Viz.Mapping;
using Limaki.View.Visuals;
using Limaki.View.GraphScene;
using Limaki.View.Vidgets;
using Xwt;
using Limaki.Usecases;
using System.Xml.Linq;
using Limaki.View.Common;

namespace Limaki.Tests.View {

	public class UsecaseSerializerTest {

		public void WriteUsecase (ConceptUsecase usecase) {
			try {
				var hasPerst = false;
				var perst = new UsecasePersistor ();
				var fileName = "laststate.limstate";
				if (File.Exists ( fileName )) {
					using (var reader = new StreamReader ( fileName )) {
						perst.XmlUsecase = XElement.Parse ( reader.ReadToEnd () );
					}
					hasPerst = true;
				}

				if (perst.XmlUsecase == null) {
					
					perst.Save ( usecase );

					Trace.WriteLine ( perst.XmlUsecase.ToString () );

					if (Registry.Pooled<IMessageBoxShow> ().Show ( "save state", "save", MessageBoxButtons.YesNo ) == DialogResult.Yes) {
						using (var writer = new StreamWriter ( fileName )) {
							writer.Write ( perst.XmlUsecase );
						}
					}
				}

				if (hasPerst) {

                    // usecase.SplitView = new Limada.View.Vidgets.SplitView0 ();

					perst.Resume ( usecase );
				}

			} catch (Exception ex) {
				Registry.Pooled<IExceptionHandler> ().Catch ( ex );
			}

		}

        public void WriteUsecase0 (ConceptUsecase usecase) {

			try {
                var interactor = Registry.Pooled<IGraphSceneMapDisplayOrganizer<IVisual, IVisualEdge>> ();
                //TODO:move this to another test
                if (true) {
					
                    var interactor2 = Registry.Pooled<IGraphSceneMapOrganizer<IVisual, IVisualEdge>> ();

					if (interactor2 != interactor)
						throw new Exception ();
				}

			    XElement dmx = null;

			    var fileName = "laststate.limstate";
			    if (File.Exists (fileName)) {
                    using (var reader = new StreamReader (fileName)) {
                        dmx = XElement.Parse (reader.ReadToEnd ());
                    }
                }

                // write current state
                var dmx1 = new UsecaseXmlSerializer ()
                       .Write (usecase);

                Trace.WriteLine (dmx1.ToString ());

			    if (Registry.Pooled<IMessageBoxShow> ().Show ("save state", "save", MessageBoxButtons.YesNo) == DialogResult.Yes) {
			        using (var writer = new StreamWriter (fileName)) {
			            writer.Write (dmx1);
			        }
			    }
			    var usecase1 = usecase;

                // test with new usecase
			    var testNewUsecase = false && dmx!=null;
			    if (testNewUsecase) {
			        var backendComposer = Registry.Create<IBackendConceptUseCaseComposer> ();
			        backendComposer.MainWindow = new Vindow {Size = new Size (800, 600)};

			        var factory = new UsecaseFactory<ConceptUsecase> ();
			        factory.Composer = new ConceptUsecaseComposer ();
			        factory.BackendComposer = backendComposer;

			        usecase1 = factory.Create ();
                    factory.Compose (usecase1);
                }

			    if (dmx != null) {

                    interactor.Clear ();

			        new UsecaseXmlSerializer ()
			            .Read (dmx, usecase1);

			        if (testNewUsecase) {
			            usecase1.MainWindow.Show ();
			        }
			    }

			} catch (Exception ex) {
				Registry.Pooled<IExceptionHandler> ().Catch ( ex );
			}

        }
    }
}