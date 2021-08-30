namespace Limaki.Common.Text.HTML.Parser{
	public class TagModifier{
		Tag Original {get;set;}
        Tag Modified { get; set; }
		public TagModifier(){
		}
		public TagModifier(Tag original){
			Original = original;
		}
		public TagModifier(Tag original, Tag modified){
			Original = original;
			SetModified(modified);
		}
		public void SetModified(Tag modified){
			Modified = modified;
		}
	}
}