using System.Collections.Generic;
using System.Drawing;

namespace Gecko.DOM
{
	public static class GeckoRangeExtension
	{
		/// <summary>
		/// Get Rectange which surrounds entire selection.
		/// </summary>
		/// <param name="selection"></param>
		/// <returns></returns>
		public static Rectangle GetBoundingClientRect( this GeckoRange range )
		{
			nsIDOMClientRect domRect = range.DomRange.GetBoundingClientRect();
			if ( domRect == null ) return Rectangle.Empty;
			return WinFormsConverter.WrapDomClientRect( domRect );
		}

		/// <summary>
		/// Get the Individual rectangles that make up a selection.
		/// </summary>
		public static Rectangle[] GetClientRects( this GeckoRange range )
		{
			nsIDOMClientRectList rects = range.DomRange.GetClientRects();
			return WinFormsConverter.WrapDomClientRectList( rects );
		}
	}
}