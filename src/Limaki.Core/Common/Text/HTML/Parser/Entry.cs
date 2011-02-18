namespace Limaki.Common.Text.HTML.Parser{
	public class Entry{
		public string Name;
		public int Starts;
		public int Ends;
		public Entry(string name, int starts, int ends){
			Name = name;
			Starts = starts;
			Ends = ends;
		}
	}
}