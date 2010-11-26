using Limaki.Widgets;

namespace Limaki.Tests.Widget {
    public class SceneTestWithLatticeData : SceneTestWithTreeData {
        public override string Name {
            get { return "Lattice"; }
        }

        public override void Populate(Scene scene) {
            base.Populate(scene);
            ILinkWidget linkWidget = null;
            
            #region first lattice links
            linkWidget = new LinkWidget<string>(string.Empty);
            linkWidget.Root = Node1;
            linkWidget.Leaf = Node2;
            scene.Add(linkWidget);
            Link1 = linkWidget;

            linkWidget = new LinkWidget<string>(string.Empty);
            linkWidget.Root = Node4;
            linkWidget.Leaf = Node3;
            scene.Add(linkWidget);
            Link2 = linkWidget;

            linkWidget = new LinkWidget<string>(string.Empty);
            linkWidget.Root = Link1;
            linkWidget.Leaf = Link2;
            scene.Add(linkWidget);
            #endregion
            #region second lattice links

            linkWidget = new LinkWidget<string>(string.Empty);
            linkWidget.Root = Node5;
            linkWidget.Leaf = Node8;
            scene.Add(linkWidget);
            Link3 = linkWidget;

            linkWidget = new LinkWidget<string>(string.Empty);
            linkWidget.Root = Node5;
            linkWidget.Leaf = Node6;
            scene.Add(linkWidget);
            Link4 = linkWidget;

            linkWidget = new LinkWidget<string>(string.Empty);
            linkWidget.Root = Node5;
            linkWidget.Leaf = Node7;
            scene.Add(linkWidget);
            Link5 = linkWidget;


            linkWidget = new LinkWidget<string>(string.Empty);
            linkWidget.Root = Node8;
            linkWidget.Leaf = Node9;
            scene.Add(linkWidget);

            #endregion
            
            if (!seperateLattice) {
                linkWidget = new LinkWidget<string>(string.Empty);
                linkWidget.Root = Link2;
                linkWidget.Leaf = Link3;
                scene.Add(linkWidget);
            }

        }
    }
}