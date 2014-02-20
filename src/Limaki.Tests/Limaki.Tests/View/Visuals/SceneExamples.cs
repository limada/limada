using Limada.Schemata;
using Limada.Tests.View;
using Limaki.Drawing;
using Limaki.Graphs;
using Limaki.Model;
using Limaki.Tests.Graph.Model;
using Limaki.Tests.View.Visuals;
using Limaki.Visuals;
using System.Collections.Generic;

namespace Limaki.Tests.View {
    
    public class SceneExamples {
        public interface ITypeChoose {
            ISampleGraphSceneFactory Data { get; }
        }

        public class TypeChoose<T> : ITypeChoose where T : class, ISampleGraphSceneFactory, new() {
            public TypeChoose() { }
            private T _data = default(T);

            public T Data {
                get {
                    if (_data == default(T)) {
                        _data = new T();
                    }
                    return _data;
                }
            }

            public override string ToString() {
                return this.Data.Name;
            }


            ISampleGraphSceneFactory ITypeChoose.Data {
                get { return this.Data; }
            }

        }

        public IList<ITypeChoose> _examples = null;
        public IList<ITypeChoose> Examples {
            get {
                if(_examples ==null) {
                    _examples = new List<ITypeChoose> ();
                    _examples.Add (new TypeChoose<SampleSceneFactory<ProgrammingLanguageFactory<IGraphEntity, IGraphEdge>>> ());
                    _examples.Add (new TypeChoose<SampleSceneFactory<BinaryTreeFactory<IGraphEntity, IGraphEdge>>> ());
                    _examples.Add (new TypeChoose<SampleSceneFactory<BinaryGraphFactory<IGraphEntity, IGraphEdge>>> ());
                    _examples.Add (new TypeChoose<SampleSceneFactory<GCJohnBostonGraphFactory<IGraphEntity, IGraphEdge>>> ());
                    _examples.Add (new TypeChoose<SampleSceneFactory<WordGameGraphFactory<IGraphEntity, IGraphEdge>>> ());
                    _examples.Add (new TypeChoose<SampleSceneFactory<LimakiShortHelpFactory<IGraphEntity, IGraphEdge>>> ());
                    _examples.Add(new TypeChoose<BenchmarkOneSceneFactory>());
                    _examples.Add(new TypeChoose<SchemaViewTestData<DigidocSchema>>());
                    Selected = _examples[0];
                }
                return _examples;
            }
        }

        public Viewers.DialogResult DialogResult { get; set; }
        public ITypeChoose Selected { get; set; }

        public IGraphScene<IVisual, IVisualEdge> GetScene (ISampleGraphSceneFactory factory) {
            var scene = factory.Scene;

            IGraph<IVisual, IVisualEdge> data = null;
            if (factory is SampleGraphPairFactory<IVisual, IGraphEntity, IVisualEdge, IGraphEdge>) {
                data = ((SampleGraphPairFactory<IVisual, IGraphEntity, IVisualEdge, IGraphEdge>) factory).GraphPair;
            } else {
                data = factory.Graph;
            }

            scene.Graph = new SubGraph<IVisual, IVisualEdge> (data, new VisualGraph ());
            return scene;
        }
    }
}