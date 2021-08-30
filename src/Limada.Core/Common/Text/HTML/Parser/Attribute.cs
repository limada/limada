namespace Limaki.Common.Text.HTML.Parser{
	public class Attribute{
		public Entry Name;
		public Entry Value;
		public Attribute(string name, int starts, int ends){
			SetName(name, starts, ends);
		}
		public Attribute(string nameName, int nameStarts, int nameEnds,
			string valueName, int valueStarts, int valueEnds){
			SetName(nameName, nameStarts, nameEnds);
			SetValue(valueName, valueStarts, valueEnds);
		}
		public void SetName(string name, int starts, int ends){
			Name = new Entry(name, starts, ends);
		}
		public void SetValue(string name, int starts, int ends){
			Value = new Entry(name, starts, ends);
		}
	}
}