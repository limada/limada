
using Limaki.Model;
using Limaki.Tests.Graph.Model;

namespace Limaki.Tests.View.Visuals {

    public class EntitySampleSceneFactory<T> : SampleSceneFactory<IGraphEntity, IGraphEdge, T> where T : ISampleGraphFactory<IGraphEntity, IGraphEdge>, new () { }

    public class ProgrammingEntitySceneTest : ProgrammingSceneTest<IGraphEntity, IGraphEdge> { }

    public class BinaryEntitySceneTest : BinarySceneTest<IGraphEntity, IGraphEdge> { }

    public class GCJohnBostonEntitySceneTest : GCJohnBostonSceneTest<IGraphEntity, IGraphEdge> { }

}