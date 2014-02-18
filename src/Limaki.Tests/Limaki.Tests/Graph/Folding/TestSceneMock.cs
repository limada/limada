using Limaki.Drawing;
using Limaki.Graphs;
using Limaki.Model;
using Limaki.Tests.Graph.Model;
using Limaki.Tests.Visuals;
using Limaki.View.UI.GraphScene;
using Limaki.View.Visualizers;
using Limaki.View.Visuals.Visualizers;
using Limaki.Visuals;

namespace Limaki.Tests.Graph.Wrappers {

    public class TestSceneMock<TFactory>
        where TFactory : GenericGraphFactory<IGraphEntity, IGraphEdge>, new () {

        protected ISceneFactory _factory;
        public virtual ISceneFactory Factory {
            get {
                if (_factory == null) {
                    _factory = new SceneFactory<TFactory> ();
                }
                return _factory;
            }
            set { _factory = value; }
        }

        protected IGraphScene<IVisual, IVisualEdge> _scene;
        public virtual IGraphScene<IVisual, IVisualEdge> Scene {
            get {
                if (_scene == null) {
                    var g = this.Factory.Scene.Graph;
                    g = new SubGraph<IVisual, IVisualEdge> (
                        ((GenericBiGraphFactory<IVisual, IGraphEntity, IVisualEdge, IGraphEdge>)this.Factory).GraphPair,
                        new VisualGraph ());
                    _scene = new Scene ();
                    _scene.Graph = g;
                }
                return _scene;
            }
            set {
                _scene = value;
                if (_display != null) {
                    _display.Data = value;
                }
            }
        }


        IGraphSceneDisplay<IVisual, IVisualEdge> _display = null;
        public IGraphSceneDisplay<IVisual, IVisualEdge> Display {
            get { return _display ?? (_display = new VisualsDisplay { Data = this.Scene }); }
            set { _display = value; }
        }

        protected GraphSceneFacade<IVisual, IVisualEdge> _sceneFacade;
        public virtual GraphSceneFacade<IVisual, IVisualEdge> SceneFacade {
            get { return _sceneFacade ?? (_sceneFacade = new GraphSceneFacade<IVisual, IVisualEdge> (() => this.Scene, Display.Layout)); }
        }

        public virtual void Reset () {
            _scene = null;
            _factory = null;
            _sceneFacade = null;
            _display = null;
        }

        }
}