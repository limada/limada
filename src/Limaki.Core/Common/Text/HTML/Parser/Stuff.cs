using System.Text;

namespace Limaki.Common.Text.HTML.Parser {
	
	public class Stuff{
	
		public StringBuilder Text;
		
        /// <summary>
        /// Cursor-Position
        /// </summary>
		public int ActAt;
		
		public int StartAt;
		
		public int StartTag;
		
        public Status Status;
		
		internal Stuff(string content){
			Text = new StringBuilder(content);
			ActAt = 0;
			StartAt = 0;
			StartTag = 0;
			Status = Status.None;
		}
        public string Element() {
            return Text.ToString().Substring(StartAt, ActAt - StartAt);
        }
        public string Tag() {
            return Text.ToString().Substring(StartTag, ActAt - StartTag);
        }
	}
}