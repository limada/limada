namespace Limaki.Common.Text.HTML.Parser{
	public enum Status{
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