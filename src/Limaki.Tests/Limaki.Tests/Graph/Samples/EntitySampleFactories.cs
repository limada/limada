using Limaki.Model;

namespace Limaki.Tests.Graph.Model {

    public class EntityGraphFactory : SampleGraphFactory<IGraphEntity, IGraphEdge> { }

    public class EntityProgrammingLanguageFactory : ProgrammingLanguageFactory<IGraphEntity, IGraphEdge> { }

    public class EntityBinaryGraphFactory : BinaryGraphFactory<IGraphEntity, IGraphEdge> { }

    public class EntityBinaryTreeFactory : BinaryTreeFactory<IGraphEntity, IGraphEdge> { }

}