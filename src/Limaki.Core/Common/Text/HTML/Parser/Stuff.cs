using System.Text;

namespace Limaki.Common.Text.HTML.Parser {
	
	public class Stuff{
        public Stuff(string content) {
            Text = new StringBuilder(content);
            Position = 0;
            Origin = 0;
            TagPosition = 0;
            State = State.None;
        }

        public StringBuilder Text { get; set; }
		
        /// <summary>
        /// Cursor-Position
        /// </summary>
        public int Position { get; set; }

        public int Origin { get; set; }

        public int TagPosition { get; set; }

        public State State { get; set; }


	    public string Element {
	        get {
	            var len = Position - Origin;
	            var result = new char[len];
	            Text.CopyTo(Origin, result, 0, len);
	            return new string(result);
	            return Text.ToString().Substring(Origin, len);
	        }
	    }

	    public string Tag {
	        get {
	            var len = Position - TagPosition;
	            var result = new char[len];
	            Text.CopyTo(TagPosition, result, 0, len);
	            return new string(result);
	            return Text.ToString().Substring(TagPosition, Position - TagPosition);
	        }
	    }
	}
}