using Limaki.Widgets;

namespace Limaki.Tests.Widget {
    public abstract class SceneTestWithTreeData : SceneTestData {
        protected int start = 1; 
        public override void Populate(Scene scene) {
            
            IWidget widget = new Widget<string> (( start++ ).ToString ());
            scene.Add (widget);
            Node1 = widget;

            widget = new Widget<string> (( start++ ).ToString ());
            scene.Add (widget);
            Node2 = widget;

            widget = new Widget<string> (( start++ ).ToString ());
            scene.Add (widget);
            Node3 = widget;

            widget = new Widget<string> (( start++ ).ToString ());
            scene.Add (widget);
            Node4 = widget;
            
            widget = new Widget<string> (( start++ ).ToString ());
            scene.Add (widget);
            Node5 = widget;

            widget = new Widget<string> (( start++ ).ToString ());
            scene.Add (widget);
            Node6 = widget;

            widget = new Widget<string> (( start++ ).ToString ());
            scene.Add (widget);
            Node7 = widget;

            widget = new Widget<string> (( start++ ).ToString ());
            scene.Add (widget);
            Node8 = widget;

            widget = new Widget<string> (( start++ ).ToString ());
            scene.Add (widget);
            Node9 = widget;

            
        }
    }
}