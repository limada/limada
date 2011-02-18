using System.Collections.Generic;

namespace Limaki.Common.Text.HTML.Parser{
	public class Style{
		public int Starts;
		public int Ends;
		public List<Tag> tags = null;
		public Style(){
			tags = new List<Tag>();
		}
		public void AddTag(Tag tag){
			tags.Add(tag);
		}
	}
}