using System;
using Limaki.Drawing;
using Limaki.Drawing.GDI.UI;
using Limaki.Graphs;
using Limaki.Model;
using Limaki.Tests.Graph.Model;
using Limaki.Tests.Widget;
using Limaki.Drawing.UI;
using Limaki.Widgets;
using Limaki.Widgets.Layout;
using Limaki.Widgets.Paint;

namespace Limaki.Tests.Graph.Wrappers {
    public class Mock<TFactory>
        where TFactory : GenericGraphFactory<IGraphItem, IGraphEdge>, new() {

        protected Scene _scene;
        public virtual Scene Scene {
            get {
                if (_scene == null) {
                    IGraph<IWidget, IEdgeWidget> g = this.Factory.Scene.Graph;
                    g = new GraphView<IWidget,IEdgeWidget>(
                        ((GenericBiGraphFactory<IWidget, IGraphItem, IEdgeWidget, IGraphEdge>)this.Factory).GraphPair, 
                        new WidgetGraph());
                    _scene = new Scene ();
                    _scene.Graph = g;
                }
                return _scene;
            }
            set { _scene = value; }
        }

        private ILayout<Scene, IWidget> _layout = null;
        public ILayout<Scene, IWidget> Layout {
            get {
                if (_layout == null) {
                    _layout = new GraphLayout<Scene, IWidget>(SceneHandler, StyleSheet.CreateDefaultStyleSheet ());
                }
                return _layout;
            }
            set { _layout = value; }
        }

        protected SceneFacade _sceneFacade;
        public virtual SceneFacade SceneFacade {
            get {
                if (_sceneFacade == null) {
                    _sceneFacade = new SceneFacade(SceneHandler, this.Layout);
                }
                return _sceneFacade;
            }
            set { _sceneFacade = value; }
        }

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

        private ControlDummy _dummy = null;
        public ControlDummy Control {
            get {
                if(_dummy == null) {
                    _dummy = new ControlDummy(SceneHandler,
                        delegate() { SceneControler.Execute(); }
                        );
                }
                return _dummy;
            }
            set { _dummy = value; }
        }

        protected GDISceneControler<Scene,IWidget> _SceneControler;
        public virtual GDISceneControler<Scene, IWidget> SceneControler {
            get {
                if (_SceneControler == null) {
                    _SceneControler = new GDISceneControler<Scene, IWidget>(
                        SceneHandler,Control,Control,new Camera(new Matrice()),this.Layout);
                    _SceneControler.Invoke();
                    Control.CommandsExecute();
                    Scene.Commands.Clear();
                }
                return _SceneControler;
            }
            set { _SceneControler = value; }
        }

        public virtual void Reset() {
            _scene = null;
            _factory = null;
            _sceneFacade = null;
        }

        public virtual Scene SceneHandler() { return _scene; }

        public class ControlDummy:IGDIControl,IScrollTarget,IDataDisplay<Scene> {

            public ControlDummy(Func<Scene> DataHandler, Action commandsExecute) {
                this.dataHandler = DataHandler;
                this._commandsExecute = commandsExecute;
            }
            #region IControl Member

            public void Invalidate() {}

            public void Invalidate(RectangleI rect) {}

            public void Invalidate(System.Drawing.Region region) {}

            public void Invalidate(System.Drawing.Drawing2D.GraphicsPath path) {}

            public RectangleI ClientRectangle {
                get { throw new System.Exception("The method or operation is not implemented."); }
            }

            public SizeI Size {
                get { throw new System.Exception("The method or operation is not implemented."); }
            }
            
            public void Update() {}
            public void UpdateScrollSize() {}
            public void UpdateCamera() { }

            Action _commandsExecute = null;
            public void CommandsExecute() { 
                if (_commandsExecute != null) {
                    _commandsExecute ();
                }
            }

            #endregion

            #region IScrollTarget Member

            private PointI _scrollPosition = PointI.Empty;
            public PointI ScrollPosition {
                get { return _scrollPosition; }
                set { _scrollPosition = value; }
            }

            public SizeI _scrollMinSize = SizeI.Empty;
            public SizeI ScrollMinSize {
                get { return _scrollMinSize; }
                set { _scrollMinSize = value; }
            }

            public virtual PointI Offset { get; set; }
            #endregion

           
            #region IDisposable Member

            public void Dispose() {}

            #endregion

            #region IData<Scene> Member
            Func<Scene> dataHandler = null;
            public Scene Data {
                get {return dataHandler ();}
                set {throw new System.Exception("The method or operation is not implemented.");}
            }

            #endregion

            #region IDataDisplay<Scene> Member


            Action _commandsInvoke = null;
            public void CommandsInvoke() {
                if (_commandsInvoke != null) {
                    _commandsInvoke();
                }
            }


            #endregion
        }
        
        }
}