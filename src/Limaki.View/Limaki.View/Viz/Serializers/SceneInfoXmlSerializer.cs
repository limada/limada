using System;
using System.Collections.Generic;
using System.Linq;
using Limaki.Common;
using Limaki.Common.Linqish;
using Limaki.Drawing;
using Limaki.View;
using Limaki.View.ContentViewers;
using Limaki.View.Visuals;
using Limaki.View.Viz;
using Limaki.View.Viz.Mesh;
using Xwt;
using System.Xml.Linq;
using Limada.Model;
using Limaki.Graphs;
using Limada.View.VisualThings;
using Limaki.Data;
using Limada.IO;
using Limaki.View.GraphScene;
using System.IO;
using Limaki.Contents;
using Limaki.Drawing.Styles;
using System.Diagnostics;

namespace Limada.UseCases {

	public class SceneInfoXmlSerializer : XmlSerializerBase {
		public static class NodeNames {
			public const string SceneInfo = "sceneinfo";
			public const string Id = "id";
			public const string Name = "name";
			public const string State = "state";
		}

		public virtual XElement Write (SceneInfo info) {
			var result = new XElement (NodeNames.SceneInfo);
			result.Add (Write (NodeNames.Id, info.Id.ToString ("X")));
			result.Add (Write (NodeNames.Name, info.Name));
			result.Add (Write (NodeNames.State, info.State.Memento ()));

			return result;
		}

		public virtual SceneInfo ReadSceneInfo (XElement parent) {
			var node = ReadElement (parent, NodeNames.SceneInfo);

			var result = new SceneInfo ();
			if (node == null)
				return result;
			
			result.Id = Read<long> (node, NodeNames.Id);
			result.Name = Read<string> (node, NodeNames.Name);
			var state = Read<StateMemento> (node, NodeNames.State);
			result.State.Memento (state);

			return result;
		}
	}
}