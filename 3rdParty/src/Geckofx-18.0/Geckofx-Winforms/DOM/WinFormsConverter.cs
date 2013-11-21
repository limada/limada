using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace Gecko.DOM
{
	public static class WinFormsConverter
	{
		public static Rectangle WrapDomClientRect(nsIDOMClientRect domRect)
		{
			var r = new Rectangle(
				(int)domRect.GetLeftAttribute(),
				(int)domRect.GetTopAttribute(),
				(int)domRect.GetWidthAttribute(),
				(int)domRect.GetHeightAttribute());
			return r;
		}

		public static Rectangle[] WrapDomClientRectList( nsIDOMClientRectList list )
		{
			uint count = list.GetLengthAttribute();
			Rectangle[] rects = new Rectangle[count];
			for (uint i = 0; i < count; i++)
			{
				nsIDOMClientRect domRect = list.Item(i);
				rects[i] = WrapDomClientRect(domRect);
				// TODO - check code for memory leaks
				//	Marshal.ReleaseComObject( domRect );
			}
			return rects;
		}
	}
}
