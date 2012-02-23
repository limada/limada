using System;
using System.IO;
using Limaki.Drawing;
using Limaki.Graphs;
using Limaki.Model.Streams;

namespace Limaki.Visuals {
    /// <summary>
    /// replaces ISheetManager
    /// replaces all methods where to load and save scenes
    /// </summary>
    public interface ISceneManager {
        
    }

    public interface ISheetManager {
        /// <summary>
        /// null if not registered
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        SceneInfo GetSheetInfo ( Int64 id );
        SceneInfo RegisterSheet(Int64 id, string name);
        void RegisterSheet(SceneInfo info);
        Action<SceneInfo> SheetRegistered { get; set; }
        void VisitRegisteredSheets(Action<SceneInfo> visitor);

        SceneInfo CreateSheet(IGraphScene<IVisual, IVisualEdge> scene);

        bool Load(IGraphScene<IVisual, IVisualEdge> scene, IGraphLayout<IVisual, IVisualEdge> layout, Int64 id);

        bool IsSaveable(IGraphScene<IVisual, IVisualEdge> scene);
        SceneInfo LoadFromContent(Content<Stream> source, IGraphScene<IVisual, IVisualEdge> target, IGraphLayout<IVisual, IVisualEdge> layout);
        void SaveInGraph(IGraphScene<IVisual, IVisualEdge> scene, IGraphLayout<IVisual, IVisualEdge> layout, SceneInfo info);
        bool SaveStreamInGraph(Stream source, IGraph<IVisual, IVisualEdge> target, SceneInfo info);

        bool SaveInStore(IGraphScene<IVisual, IVisualEdge> scene, IGraphLayout<IVisual, IVisualEdge> layout, Int64 id);
        bool LoadFromStore(IGraphScene<IVisual, IVisualEdge> scene, IGraphLayout<IVisual, IVisualEdge> layout, Int64 id);
        Stream GetFromStore(Int64 id);
        bool StoreContains(Int64 id);
        
        void Clear();

        
    }

   
}