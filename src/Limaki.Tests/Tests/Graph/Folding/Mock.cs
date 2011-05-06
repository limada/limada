using System;
using Limaki.Drawing;
using Limaki.Graphs;
using Limaki.Model;
using Limaki.Tests.Graph.Model;
using Limaki.Tests.Widget;
using Limaki.Widgets;
using Limaki.Presenter.Widgets.Layout;
using Limaki.Presenter.UI;
using Limaki.Presenter;
using Limaki.Presenter.Widgets;
using Limaki.Common;
using Limaki.Presenter.Display;
using Limaki.Presenter.Widgets.UI;

namespace Limaki.Tests.Graph.Wrappers {
    public class Mock<TFactory>
        where TFactory : GenericGraphFactory<IGraphItem, IGraphEdge>, new() {

        protected ISceneFactory _factory;
        public virtual ISceneFactory Factory {
            get {
                if (_factory == null) {
                    _factory = new SceneFactory<TFactory>();
                }
                return _factory;
            }
            set { _factory = value; }
        }

        protected Scene _scene;
        public virtual Scene Scene {
            get {
                if (_scene == null) {
                    IGraph<IWidget, IEdgeWidget> g = this.Factory.Scene.Graph;
                    g = new GraphView<IWidget, IEdgeWidget>(
                        ((GenericBiGraphFactory<IWidget, IGraphItem, IEdgeWidget, IGraphEdge>)this.Factory).GraphPair,
                        new WidgetGraph());
                    _scene = new Scene();
                    _scene.Graph = g;
                }
                return _scene;
            }
            set { _scene = value; 
                  if (_display != null) {
                      _display.Data = value;
                  }
            }
        }


        WidgetDisplay _display = null;
        public WidgetDisplay Display {
            get {
                if (_display == null) {
                    var factory = new WidgetDisplayFactory ();
                    var inst = new MockDeviceComposer<IGraphScene<IWidget, IEdgeWidget>> ();
                    inst.DataLayer = new MockGraphSceneLayer<IWidget, IEdgeWidget> ();
                    factory.DeviceComposer = inst;
                    var display = new WidgetDisplay();
                    factory.Compose(display);
                    display.Data = this.Scene;
                    //display.DataHandler = () => this.Scene;
                    _display = display;
                }
                return _display;
            }
            set { _display = value; }
        }

        protected GraphSceneFacade<IWidget, IEdgeWidget> _sceneFacade;
        public virtual GraphSceneFacade<IWidget, IEdgeWidget> SceneFacade {
            get {
                if (_sceneFacade == null) {
                    _sceneFacade = new GraphSceneFacade<IWidget, IEdgeWidget>(() => this.Scene, Display.Layout);
                }
                return _sceneFacade;

            }
            //set { _sceneFacade = value; }
        }

        public virtual void Reset() {
            _scene = null;
            _factory = null;
            _sceneFacade = null;
            _display = null;
        }

    }

    public class MockWidgetDisplay:WidgetDisplay {
        public Get<Scene> DataHandler { get; set; }
        public override Scene Data {
            get {
                base.Data = DataHandler ();
                return base.Data;
            }
            set {  }
        }
    }


}