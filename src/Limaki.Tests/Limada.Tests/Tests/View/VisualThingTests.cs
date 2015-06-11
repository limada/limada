using Limada.Model;
using Limada.View.VisualThings;
using Limaki.Tests.Graph.Model;
using Limaki.Tests.View.Visuals;
using Limaki.View.Visuals;

namespace Limada.Tests.View {

    // View-Model-Tests where View is IVisual and Model is IThing

    public class VisualThingSampleFactory<T> : SampleGraphPairFactory<IVisual, IThing, IVisualEdge, ILink>
    where T : ISampleGraphFactory<IThing, ILink>, new () {
        public VisualThingSampleFactory () :
            base (new T (),
                  new VisualThingTransformer ()) { }
    }

    public class ViusalThingSampleSceneFactory<T> : SampleSceneFactory<IThing, ILink, T>
    where T : ISampleGraphFactory<IThing, ILink>, new () { }

    public class VisualThingProgrammingSceneTest : ProgrammingSceneTest<IThing, ILink> { }

    public class VisualThingBinarySceneTest : BinarySceneTest<IThing, ILink> { }

    public class VisualThingGCJohnBostonSceneTest : GCJohnBostonSceneTest<IThing, ILink> { }

    public class VisualThingSceneMeshTest : SceneMeshTest<IThing, ILink> { }

}