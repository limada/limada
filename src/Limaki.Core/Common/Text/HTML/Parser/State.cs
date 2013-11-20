namespace Limaki.Common.Text.HTML.Parser{
	public enum State{
		None,
		Text,
		Prename,
		Name,
		Endtag,
		Commenttag,
		Solotag,
		Attribute,
		Value,
		Cite
	} ;
}