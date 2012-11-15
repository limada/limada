using NUnit.Framework;
using Limaki.Tests;
using System.Collections.Generic;
using Limaki.Drawing;
using Limaki.Drawing.Shapes;
using Limaki.Graphs;
using Limaki.Graphs.Extensions;
using System.Linq;
using System;
using Xwt;
using Limaki.Visuals;
using Limaki.Tests.View;
using Limaki.Common;
using Limaki.Drawing.Styles;
using Limaki.View.UI.GraphScene;
using Limaki.Common.Linqish;
using Limaki.View;
using Limaki.View.Modelling;
using Limaki.View.Layout;

namespace Limaki.Playground.View {
    public class SceneWorker<TItem, TEdge>
        where TEdge : TItem, IEdge<TItem> {
        public IGraphSceneLayout<TItem, TEdge> Layout { get; set; }
        public IGraphScene<TItem, TEdge> Scene { get; set; }
        public GraphSceneFacade<TItem, TEdge> Folder { get; set; }
        public IGraphSceneReceiver<TItem, TEdge> Receiver { get; set; }
    }

    public class AllignerPrototyper : DomainTest {
        SceneWorker<IVisual, IVisualEdge> CreateSceneWorker(IGraphScene<IVisual, IVisualEdge> scene) {
            var styleSheet = Registry.Pool.TryGetCreate<StyleSheets>().DefaultStyleSheet;
            Get<IGraphScene<IVisual, IVisualEdge>> fScene = () => scene;
            var layout = Registry.Factory.Create<IGraphSceneLayout<IVisual, IVisualEdge>>(fScene, styleSheet);
            layout.Orientation = Limaki.Drawing.Orientation.LeftRight;

            var folder = new GraphSceneFacade<IVisual, IVisualEdge>(fScene, layout);
            folder.ShowAllData();

            var painter = new VisualSceneContextPainter(scene, layout);

            var modelReceiver = new GraphItemReceiver<IVisual, IVisualEdge>();
            var receiver = new GraphSceneReceiver<IVisual, IVisualEdge>() as IGraphSceneReceiver<IVisual, IVisualEdge>;
            receiver.GraphScene = fScene;
            receiver.Layout = () => layout;
            receiver.Camera = () => painter.Viewport.Camera;
            receiver.Clipper = () => painter.Clipper;
            receiver.ModelReceiver = () => modelReceiver;

            return new SceneWorker<IVisual, IVisualEdge> {
                Scene = scene,
                Layout = layout,
                Receiver = receiver,
                Folder = folder,
            };   
        }

        IGraphScene<IVisual, IVisualEdge> SceneWithTestData (int example) {
            IGraphScene<IVisual, IVisualEdge> scene = null;
            var examples = new SceneExamples();
            var testData = examples.Examples[example];
            testData.Data.Count = 1;
            scene = examples.GetScene(testData.Data);

            return scene;
           
        }

        SceneWorker<IVisual, IVisualEdge> SceneWorkerWithTestData (int example) {
            var scene = SceneWithTestData(example);
            
            var worker = CreateSceneWorker(scene);
            worker.Receiver.Execute();
            worker.Receiver.Done();

            var scene2 = SceneWithTestData(example);
           
            var view = scene.Graph as GraphView<IVisual, IVisualEdge>;
            var graph = view.Two;
            var graph2 = (scene2.Graph as GraphView<IVisual, IVisualEdge>).Two;
            graph2.Where(e => e is Visual<string>).ForEach(e => { e.Data += "1"; });

            var root = graph.FindRoots(null).First();
            var root2 = graph2.FindRoots(null).First();
            graph2.Elements().ForEach(e => graph.Add(e));
            view.Add(root2);
            new Alligner<IVisual, IVisualEdge>(scene, worker.Layout, p => p.OneColumn(new IVisual[] { root, root2 }));

            scene.Focused = root2;

            worker.Receiver.Execute();
            worker.Receiver.Done();
            return worker;
        }

        [Test]
        public void Collisions () {
            var worker = SceneWorkerWithTestData(0);
            var scene = worker.Scene;
            ILocator<IVisual> loc = new SGraphSceneLocator<IVisual, IVisualEdge> { GraphScene = scene };
            Action<IEnumerable<IVisual>> reportElems = elms=>
                 elms
                .OrderBy(e => e.Location, new PointComparer { Delta = worker.Layout.Distance.Width, Order = PointOrder.LeftToRight })
                .ForEach(e => ReportDetail("{3}{0}\t{1}\t{2}", e.Data, loc.GetLocation(e), loc.GetSize(e), scene.Focused == e ? "*" : ""));

             
            Func<IEnumerable<IVisual>, Rectangle> reportExtent = elms => {
                var measure = new MeasureVisitBuilder<IVisual>(loc);
                Action<IVisual> visit = null;
                var fSizeToFit = measure.SizeToFit(ref visit, worker.Layout.Distance, Dimension.X);
                var fBounds = measure.Bounds(ref visit);
                var fMinSize = measure.MinSize(ref visit);

                elms.ForEach(e => visit(e));
                ReportDetail("Bounds {1}\tSizeToFit {0}\tMinSize  {2}", fSizeToFit(), fBounds(), fMinSize());
                return fBounds();
            };

            var ignore = new HashSet<IVisual>(scene.Elements.Where(e => !(e is IVisualEdge)));
            reportElems(ignore);
            reportExtent(ignore);
            IEnumerable<IVisual> elems = Walker.Create((scene.Graph as GraphView<IVisual, IVisualEdge>).Two)
                .ExpandWalk(scene.Focused, 0)
                .Select(l => l.Node)
                .ToArray();
            
            new Alligner<IVisual, IVisualEdge>(scene, worker.Layout, p => p.Justify(elems.Where(e=>e!=scene.Focused)));
            elems.ForEach(e => scene.Graph.Add(e));

            elems = elems.Where(e => !(e is IVisualEdge));
            var all = new Alligner<IVisual, IVisualEdge>(scene, worker.Layout);
            all.Columns(scene.Focused, elems, new AllignerOptions { AlignX = Alignment.Center, AlignY = Alignment.Center, Dimension = Dimension.X, Distance = worker.Layout.Distance, PointOrder = PointOrder.LeftToRight });
            loc = all.Locator;
            reportElems(elems);
            var bounds = reportExtent(elems);
            var free = CalculateNextFreeSpace(scene, bounds.Location, bounds.Size, Dimension.X, elems, worker.Layout.Distance.Width);
        }

        IList<Rectangle> CalculateNextFreeSpace (IGraphScene<IVisual, IVisualEdge> scene,
            Point start, Size sizeNeeded, Dimension dimension, IEnumerable<IVisual> ignore, double distance) {

            //var h = dimension == Dimension.X ? sizeNeeded.Height : scene.Shape.Size.Height;
            //var w = dimension == Dimension.X ? scene.Shape.Size.Width : sizeNeeded.Width;
            //var iRect = new Rectangle(start, new Size(w, h));

            var iRect = new Rectangle(start, sizeNeeded);

            var comparer = new PointComparer { Order = dimension == Dimension.X ? PointOrder.X : PointOrder.Y };
            var loc = new SGraphSceneLocator<IVisual,IVisualEdge> { GraphScene = scene };
            var elems = scene.ElementsIn(iRect)
                .Where(e => !(e is IVisualEdge))
                .Except(ignore)
                .OrderBy(e => loc.GetLocation(e), comparer);
            if(elems.Any()) {
                
            }
            return new Rectangle[0];
        }
    }
}