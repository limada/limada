using System;
using System.Collections.Generic;
using System.Linq;
using Limaki.Common;
using Limaki.Common.Linqish;
using Limaki.View.Visuals;
using Limaki.View.Viz.Mapping;
using System.Xml.Linq;
using Limada.Model;
using Limada.View.VisualThings;


namespace Limada.Usecases {

	public class GraphSceneDisplayOrganizerXmlSerializer : XmlSerializerBase {
		
		public static class NodeNames {
			public const string Mesh = "mesh";

			public const string Files = "files";
			public const string File = "file";
			public const string Name = "name";

			public const string Displays = "displays";
		}
		
		public virtual XElement Write (IGraphSceneMapDisplayOrganizer<IVisual, IVisualEdge> organizer) {

			var result = new XElement (NodeNames.Mesh);

            var interactor = organizer.MapInteractor<IThing,ILink>();

			var thingGraphs = new XElement (NodeNames.Files,
				interactor.MappedGraphs
					.Select (g => ThingMapExtensions.GetIori (g))
					.Where (i => i != null)
					.Select (i => new XElement (NodeNames.File, Write (NodeNames.Name, i.ToString ()))));
			result.Add (thingGraphs);

			var displaySerializer = new GraphSceneDisplayXmlSerializer ();
			var displays = new XElement (NodeNames.Displays);

			foreach (var disp in organizer.Displays.Where(d=>d.Data!=null)) {
				var iori = ThingMapExtensions.GetIori (interactor.MappedGraphOf (disp.Data.Graph));
				if (disp.DataId == 0) {
					disp.DataId = Isaac.Long;
				}
				displays.Add (displaySerializer.Write (new GraphSceneDisplayMemento (disp) { Iori = iori }));
			}
			result.Add (displays);

			return result;
		}

	    public IList<GraphSceneDisplayMemento> Displays { get; set; }
	    public IList<string> FileNames { get; set; }

	    public virtual void Read (XElement parent, IGraphSceneMapDisplayOrganizer<IVisual, IVisualEdge> organizer) {
			
			var node = ReadElement (parent, NodeNames.Mesh);
			if (node == null)
				return;

	        FileNames = ReadElements (ReadElement (node, NodeNames.Files), NodeNames.File)
	            .Select (n => ReadString (n, NodeNames.Name))
	            .ToList ();

            Displays = new List<GraphSceneDisplayMemento> ();

            var displaySerializer = new GraphSceneDisplayXmlSerializer { Organizer = organizer };
			foreach (var dnode in ReadElements (ReadElement (node, NodeNames.Displays), GraphSceneDisplayXmlSerializer.NodeNames.Display)){
				var displayMemento = displaySerializer.ReadDisplay (dnode);
			    Displays.Add (displayMemento);
			}

		}
	}
	
}