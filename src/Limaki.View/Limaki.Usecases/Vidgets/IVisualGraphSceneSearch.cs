/*
 * Limada 
 * 
 * This code is free software; you can redistribute it and/or modify it
 * under the terms of the GNU General Public License version 2 only, as
 * published by the Free Software Foundation.
 * 
 * Author: Lytico
 * Copyright (C) 2006-2017 Lytico
 *
 * http://www.limada.org
 */


using Limaki.View.Visuals;

namespace Limaki.View.Vidgets {

    public interface IVisualGraphSceneSearch {
        
        bool IsSearchable (IGraphScene<IVisual, IVisualEdge> scene);
        void LoadSearch (IGraphScene<IVisual, IVisualEdge> scene, IGraphSceneLayout<IVisual, IVisualEdge> layout, object name);

    }
    
}