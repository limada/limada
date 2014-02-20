using Limaki.Model;

namespace Limaki.Tests.Graph.Model {

    public class EntityGraphFactory : SampleGraphFactory<IGraphEntity, IGraphEdge> { }

    public class ProgrammingLanguageFactory : ProgrammingLanguageFactory<IGraphEntity, IGraphEdge> { }

    public class BinaryGraphFactory : BinaryGraphFactory<IGraphEntity, IGraphEdge> { }

    public class BinaryTreeFactory : BinaryTreeFactory<IGraphEntity, IGraphEdge> { }

}