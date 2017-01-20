using System;
using System.IO;
using Limaki.Contents;
using Limaki.Graphs;

namespace Limaki.View.Visuals {

    [Obsolete("use ISceneManager")]
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

        /// <summary>
        /// clears scene and gives back a new, empty SceneInfo
        /// </summary>
        /// <returns>The sheet.</returns>
        /// <param name="scene">Scene.</param>
        SceneInfo CreateSheet(IGraphScene<IVisual, IVisualEdge> scene);

        bool Load(IGraphScene<IVisual, IVisualEdge> scene, IGraphSceneLayout<IVisual, IVisualEdge> layout, Int64 id);

        bool IsSaveable(IGraphScene<IVisual, IVisualEdge> scene);
        SceneInfo LoadFromContent(Content<Stream> source, IGraphScene<IVisual, IVisualEdge> target, IGraphSceneLayout<IVisual, IVisualEdge> layout);
        void SaveInGraph(IGraphScene<IVisual, IVisualEdge> scene, IGraphSceneLayout<IVisual, IVisualEdge> layout, SceneInfo info);
        bool SaveStreamInGraph(Stream source, IGraph<IVisual, IVisualEdge> target, SceneInfo info);

        bool SaveInStore(IGraphScene<IVisual, IVisualEdge> scene, IGraphSceneLayout<IVisual, IVisualEdge> layout, Int64 id);
        bool LoadFromStore(IGraphScene<IVisual, IVisualEdge> scene, IGraphSceneLayout<IVisual, IVisualEdge> layout, Int64 id);
        Stream GetFromStore(Int64 id);
        bool StoreContains(Int64 id);
        
        void Clear();

        
    }

   
}