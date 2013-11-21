using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Shapes;
using Gecko;

namespace Gecko.DOM
{
	public static class GeckoElementExtensionMethods
	{
		/// <summary>
		/// UI specific implementation extension method GetBoundingClientRect()
		/// </summary>
		/// <param name="element"></param>
		/// <returns></returns>
		public static System.Windows.Rect GetBoundingClientRect( this GeckoElement element )
		{
			nsIDOMClientRect domRect = element.DOMElement.GetBoundingClientRect();
			if ( domRect == null ) return System.Windows.Rect.Empty;
			var r = new System.Windows.Rect(
				domRect.GetLeftAttribute(),
				domRect.GetTopAttribute(),
				domRect.GetWidthAttribute(),
				domRect.GetHeightAttribute() );
			return r;
		}

		/// <summary>
		/// UI specific implementation extension method GetBoundingClientRect()
		/// </summary>
		/// <param name="element"></param>
		/// <returns></returns>
		public static System.Windows.Int32Rect GetBoundingClientRectInt32( this GeckoElement element )
		{
			nsIDOMClientRect domRect = element.DOMElement.GetBoundingClientRect();
			if ( domRect == null ) return System.Windows.Int32Rect.Empty;
			var r = new System.Windows.Int32Rect(
				( int ) domRect.GetLeftAttribute(),
				( int ) domRect.GetTopAttribute(),
				( int ) domRect.GetWidthAttribute(),
				( int ) domRect.GetHeightAttribute() );
			return r;

		}


		public static System.Windows.Rect[] GetClientRects( this GeckoElement element )
		{
			nsIDOMClientRectList domRects = element.DOMElement.GetClientRects();
			uint count = domRects.GetLengthAttribute();
			System.Windows.Rect[] rects = new System.Windows.Rect[count];
			for ( uint i = 0; i < count; i++ )
			{
				nsIDOMClientRect domRect = domRects.Item( i );
				rects[ i ] = new System.Windows.Rect(
					domRect.GetLeftAttribute(),
					domRect.GetTopAttribute(),
					domRect.GetWidthAttribute(),
					domRect.GetHeightAttribute() );
				// TODO - check code for memory leaks
				//	Marshal.ReleaseComObject( domRect );
			}
			//Marshal.ReleaseComObject( domRects );
			return rects;

		}

		public static System.Windows.Int32Rect[] GetClientRectsInt32( this GeckoElement element )
		{
			nsIDOMClientRectList domRects = element.DOMElement.GetClientRects();
			uint count = domRects.GetLengthAttribute();
			System.Windows.Int32Rect[] rects = new System.Windows.Int32Rect[count];
			for ( uint i = 0; i < count; i++ )
			{
				nsIDOMClientRect domRect = domRects.Item( i );
				rects[ i ] = new System.Windows.Int32Rect(
					( int ) Math.Round( domRect.GetLeftAttribute() ),
					( int ) Math.Round( domRect.GetTopAttribute() ),
					( int ) Math.Round( domRect.GetWidthAttribute() ),
					( int ) Math.Round( domRect.GetHeightAttribute() ) );
				// TODO - check code for memory leaks
				//	Marshal.ReleaseComObject( domRect );
			}
			//Marshal.ReleaseComObject( domRects );
			return rects;

		}
	}
}
