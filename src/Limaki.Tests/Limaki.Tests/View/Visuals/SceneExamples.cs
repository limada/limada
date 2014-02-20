﻿using System.Collections.Generic;
using Limada.Schemata;
using Limaki.Tests.Graph.Model;
using Limaki.Tests.Visuals;
using Limada.Tests.View;
using Limaki.Drawing;
using Limaki.Visuals;
using Limaki.Graphs;
using Limaki.Model;

namespace Limaki.Tests.View {
    
    public class SceneExamples {
        public interface ITypeChoose {
            ISceneFactory Data { get; }
        }

        public class TypeChoose<T> : ITypeChoose where T : class, ISceneFactory, new() {
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


            ISceneFactory ITypeChoose.Data {
                get { return this.Data; }
            }

        }

        public IList<ITypeChoose> _examples = null;
        public IList<ITypeChoose> Examples {
            get {
                if(_examples ==null) {
                    _examples = new List<ITypeChoose> ();
                    _examples.Add (new TypeChoose<SceneFactory<ProgrammingLanguageFactory<IGraphEntity, IGraphEdge>>> ());
                    _examples.Add (new TypeChoose<SceneFactory<BinaryTreeFactory<IGraphEntity, IGraphEdge>>> ());
                    _examples.Add (new TypeChoose<SceneFactory<BinaryGraphFactory<IGraphEntity, IGraphEdge>>> ());
                    _examples.Add (new TypeChoose<SceneFactory<GCJohnBostonGraphFactory<IGraphEntity, IGraphEdge>>> ());
                    _examples.Add (new TypeChoose<SceneFactory<WordGameGraphFactory<IGraphEntity, IGraphEdge>>> ());
                    _examples.Add (new TypeChoose<SceneFactory<LimakiShortHelpFactory<IGraphEntity, IGraphEdge>>> ());
                    _examples.Add(new TypeChoose<BenchmarkOneSceneFactory>());
                    _examples.Add(new TypeChoose<SchemaViewTestData<DigidocSchema>>());
                    Selected = _examples[0];
                }
                return _examples;
            }
        }

        public Viewers.DialogResult DialogResult { get; set; }
        public ITypeChoose Selected { get; set; }

        public IGraphScene<IVisual, IVisualEdge> GetScene (ISceneFactory factory) {
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