/*
 * Limada 
 * 
 * This code is free software; you can redistribute it and/or modify it
 * under the terms of the GNU General Public License version 2 only, as
 * published by the Free Software Foundation.
 * 
 * Author: Lytico
 * Copyright (C) 2013-2014 Lytico
 *
 * http://www.limada.org
 */

using System.Collections.Generic;
using System.IO;
using Limada.Model;
using Limaki.Common;
using Limaki.Contents;
using Limaki.Contents.IO;
using Limaki.Data;
using Limaki.Graphs;

namespace Limada.Data {

    public class ThingGraphContent : Content<IThingGraph> { }

    /// <summary>
    /// pool of ThingGraph-Readers and Writers
    /// </summary>
    public class ThingGraphIoPool : ContentIoPool<Iori, ThingGraphContent> { }

    /// <summary>
    /// manager to read and write ThingGraphs
    /// </summary>
    public class ThingGraphIoManager : IoManager<Iori, ThingGraphContent, ThingGraphIoPool> { }

    /// <summary>
    /// pool of exporters/importers that uses the GraphCursor.Cursor
    /// </summary>
    public class ThingGraphCursorPool : ContentIoPool<Stream, GraphCursor<IThing, ILink>> { }

    /// <summary>
    /// manager to  export/import the thing of GraphCursor.Cursor
    /// </summary>
    public class ThingGraphCursorIoManager : IoUriManager<Stream, GraphCursor<IThing, ILink>, ThingGraphCursorPool> { }

    /// <summary>
    /// pool of exporters/importers that uses a list of things
    /// </summary>
    public class ThingsStreamPool : ContentIoPool<Stream, IEnumerable<IThing>> { }

    /// <summary>
    /// manager to  export/import a list of things
    /// </summary>
    public class ThingsStreamIoManager : IoUriManager<Stream, IEnumerable<IThing>, ThingsStreamPool> { }

    public class ThingGraphRepairPool : ContentIoPool<IThingGraphRepair, Iori> { }
}