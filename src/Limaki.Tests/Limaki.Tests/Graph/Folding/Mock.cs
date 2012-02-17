using System;
using Limaki.Drawing;
using Limaki.Graphs;
using Limaki.Model;
using Limaki.Tests.Graph.Model;
using Limaki.Tests.Visuals;
using Limaki.Visuals;
using Limaki.Presenter.Visuals.Layout;
using Limaki.Presenter.UI;
using Limaki.Presenter;
using Limaki.Presenter.Visuals;
using Limaki.Common;
using Limaki.Presenter.Display;
using Limaki.Presenter.Visuals.UI;

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
                    IGraph<IVisual, IVisualEdge> g = this.Factory.Scene.Graph;
                    g = new GraphView<IVisual, IVisualEdge>(
                        ((GenericBiGraphFactory<IVisual, IGraphItem, IVisualEdge, IGraphEdge>)this.Factory).GraphPair,
                        new VisualGraph());
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


        IGraphSceneDisplay<IVisual, IVisualEdge> _display = null;
        public IGraphSceneDisplay<IVisual, IVisualEdge> Display {
            get {
                if (_display == null) {
                    var factory = new VisualsDisplayFactory ();
                    var inst = new MockDeviceComposer<IGraphScene<IVisual, IVisualEdge>> ();
                    inst.DataLayer = new MockGraphSceneLayer<IVisual, IVisualEdge> ();
                    factory.DeviceComposer = inst;
                    var display = new GraphSceneDisplay<IVisual, IVisualEdge>();
                    factory.Compose(display);
                    display.Data = this.Scene;
                    //display.DataHandler = () => this.Scene;
                    _display = display;
                }
                return _display;
            }
            set { _display = value; }
        }

        protected GraphSceneFacade<IVisual, IVisualEdge> _sceneFacade;
        public virtual GraphSceneFacade<IVisual, IVisualEdge> SceneFacade {
            get {
                if (_sceneFacade == null) {
                    _sceneFacade = new GraphSceneFacade<IVisual, IVisualEdge>(() => this.Scene, Display.Layout);
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

    public class MockVisualsDisplay : GraphSceneDisplay<IVisual, IVisualEdge> {
        public Get<IGraphScene<IVisual, IVisualEdge>> DataHandler { get; set; }
        public override IGraphScene<IVisual, IVisualEdge> Data {
            get {
                base.Data = DataHandler ();
                return base.Data;
            }
            set {  }
        }
    }


}