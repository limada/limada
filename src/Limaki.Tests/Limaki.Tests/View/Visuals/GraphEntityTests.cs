
using Limaki.Model;
using Limaki.Tests.Graph.Model;

namespace Limaki.Tests.View.Visuals {

    public class ProgrammingGraphSceneTest : ProgrammingGraphSceneTest<IGraphEntity, IGraphEdge> { }

    public class SceneTestWrap<TFactory> : SceneTestWrap<IGraphEntity, IGraphEdge, TFactory>
       where TFactory : ISampleGraphFactory<IGraphEntity, IGraphEdge>, new () {

        public SceneTestWrap (SceneFacadeTest<IGraphEntity, IGraphEdge, TFactory> test) : base (test) { }
    }

    public class SampleSceneFactory<T> : SampleSceneFactory<IGraphEntity, IGraphEdge, T> where T : ISampleGraphFactory<IGraphEntity, IGraphEdge>, new () { }

}