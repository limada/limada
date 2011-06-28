using System.Collections.Generic;

namespace Limaki.Common.Text.HTML.Parser{
	public class Style{
		public int Starts;
		public int Ends;
		public IList<Tag> Tags {get; private set;}
		public Style(){
			Tags = new List<Tag>();
		}
		public void Add(Tag tag){
			Tags.Add(tag);
		}
	}
}