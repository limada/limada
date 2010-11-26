using Limaki.Widgets;
using Limaki.Graphs;

namespace Limaki.Tests.Widget {
    public abstract class SceneTestData : ISceneTestData {
        public abstract string Name { get; }

        protected bool seperateLattice = false;
        protected bool addDensity = false;

        IGraph<IWidget, ILinkWidget> _graph = null;
        public virtual IGraph<IWidget, ILinkWidget> Graph {
            get {
                if (_graph == null) {
                    _graph = new Graph<IWidget, ILinkWidget> ();
                } 
                return _graph;
                
            }
            set { _graph = value; }
        }
        public virtual Scene Scene {
            get {
                Scene result = new Scene();
                result.Graph = this.Graph;

                IWidget lastNode1 = null;
                IWidget lastNode2 = null;
                IWidget lastNode3 = null;
                for (int i = 0; i < Count; i++) {
                    if (i > 0) {
                        lastNode1 = Node1;
                        lastNode2 = Node5;
                        lastNode3 = Node8;
                    }
                    Populate(result);
                    if (i > 0) {
                        ILinkWidget linkWidget = new LinkWidget<string>(string.Empty);
                        linkWidget.Root = lastNode1;
                        linkWidget.Leaf = Node1;
                        result.Add(linkWidget);
                        if (seperateLattice) {
                            linkWidget = new LinkWidget<string>(string.Empty);
                            linkWidget.Root = lastNode2;
                            linkWidget.Leaf = Node5;
                            result.Add(linkWidget);
                        }
                        if (addDensity) {
                            linkWidget = new LinkWidget<string>(string.Empty);
                            linkWidget.Root = Node2;
                            linkWidget.Leaf = lastNode3;
                            result.Add(linkWidget);
                        }
                    }
                }

                MakeLinkStrings (result);

                return result;
            }
        }

        private int _count = 1;
        public int Count {
            get { return _count; }
            set { _count = value; }
        }

        protected string GetLinkString(ILinkWidget link) {
            return "[" + link.Root.Data.ToString() + "->" + link.Leaf.Data.ToString() + "]";
        }

        public virtual void MakeLinkStrings(Scene scene) {
            foreach (IWidget awidget in scene.Graph) {
                if (!(awidget is ILinkWidget)) {
                    foreach (ILinkWidget link in scene.Graph.PreorderEdges(awidget)) {
                        link.Data = GetLinkString(link);
                    }

                }
            }
        }
        public abstract void Populate ( Scene scene );

        public IWidget Node1;
        public IWidget Node2;
        public IWidget Node3;
        public IWidget Node4;
        public IWidget Node5;
        public IWidget Node6;
        public IWidget Node7;
        public IWidget Node8;
        public IWidget Node9;
        public ILinkWidget Link1;
        public ILinkWidget Link2;
        public ILinkWidget Link3;
        public ILinkWidget Link4;
        public ILinkWidget Link5;
        public ILinkWidget Link6;
        public ILinkWidget Link7;
        public ILinkWidget Link8;
    }
}