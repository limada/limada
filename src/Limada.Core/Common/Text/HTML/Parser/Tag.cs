using System.Collections.Generic;

namespace Limaki.Common.Text.HTML.Parser{
	
	public class Tag{
		public int Starts;
		public int Ends;
		public State State;
		public Entry Me = null;
		public List<Attribute> Attributes = null;
		public Tag(){
			Me = new Entry(null, 0, 0);
		}
		public Tag(string name, int starts, int ends){
			Me = new Entry(name, starts, ends);
		}
		public void AddAttribute(Attribute attribute){
			if(Attributes == null){
				Attributes = new List<Attribute>();
			}
			Attributes.Add(attribute);
		}
		public void SetStatus(State state){
		    State = state;
		}
	}
}