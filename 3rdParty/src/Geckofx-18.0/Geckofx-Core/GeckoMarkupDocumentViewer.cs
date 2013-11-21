using System;
using Gecko.Interop;

namespace Gecko
{

	public class GeckoMarkupDocumentViewer
	{
		private ComPtr<nsIMarkupDocumentViewer> _docViewer;

		public GeckoMarkupDocumentViewer(nsIMarkupDocumentViewer markupDocumentView)
		{
			_docViewer = new ComPtr<nsIMarkupDocumentViewer>(markupDocumentView);
		}

		/// <summary>
		///Scrolls to a given DOM content node.
		///	</summary>
		public void ScrollToNode(GeckoNode node)
		{
			_docViewer.Instance.ScrollToNode(node.DomObject);
		}

		/// <summary>
		///The amount by which to scale all text. Default is 1.0. 
		/// </summary>		
		public float GetTextZoomAttribute()
		{
			return _docViewer.Instance.GetTextZoomAttribute();
		}

		/// <summary>
		///The amount by which to scale all text. Default is 1.0. 
		/// </summary>		
		public void SetTextZoomAttribute(float aTextZoom)
		{
			_docViewer.Instance.SetTextZoomAttribute(aTextZoom);
		}

		/// <summary>
		///The amount by which to scale all lengths. Default is 1.0. 
		///</summary>		
		public float GetFullZoomAttribute()
		{
			return _docViewer.Instance.GetFullZoomAttribute();
		}

		/// <summary>
		///The amount by which to scale all lengths. Default is 1.0. 
		///</summary>		
		public void SetFullZoomAttribute(float aFullZoom)
		{
			_docViewer.Instance.SetFullZoomAttribute(aFullZoom);
		}

		/// <summary>
		///Disable entire author style level (including HTML presentation hints) 
		/// </summary>		
		public bool GetAuthorStyleDisabledAttribute()
		{
			return _docViewer.Instance.GetAuthorStyleDisabledAttribute();
		}

		/// <summary>
		///Disable entire author style level (including HTML presentation hints) 
		/// </summary>		
		public void SetAuthorStyleDisabledAttribute(bool aAuthorStyleDisabled)
		{
			_docViewer.Instance.SetAuthorStyleDisabledAttribute(aAuthorStyleDisabled);
		}

		public string GetDefaultCharacterSetAttribute()
		{
			return nsString.Get(_docViewer.Instance.GetDefaultCharacterSetAttribute);
		}


		public void SetDefaultCharacterSetAttribute(string aDefaultCharacterSet)
		{
			nsString.Set(_docViewer.Instance.SetDefaultCharacterSetAttribute, aDefaultCharacterSet);			
		}

		public string GetForceCharacterSetAttribute()
		{
			return nsString.Get(_docViewer.Instance.GetForceCharacterSetAttribute);
		}


		public void SetForceCharacterSetAttribute(string aForceCharacterSet)
		{
			nsString.Set(_docViewer.Instance.SetForceCharacterSetAttribute, aForceCharacterSet);
		}

		public string GetHintCharacterSetAttribute()
		{
			return nsString.Get(_docViewer.Instance.GetHintCharacterSetAttribute);
		}

		public void SetHintCharacterSetAttribute(string aHintCharacterSet)
		{
			nsString.Set(_docViewer.Instance.GetHintCharacterSetAttribute, aHintCharacterSet);
		}

		public int GetHintCharacterSetSourceAttribute()
		{
			return _docViewer.Instance.GetHintCharacterSetSourceAttribute();
		}

		public void SetHintCharacterSetSourceAttribute(int aHintCharacterSetSource)
		{
			_docViewer.Instance.SetHintCharacterSetSourceAttribute(aHintCharacterSetSource);
		}
		
		public string GetPrevDocCharacterSetAttribute()
		{
			return nsString.Get(_docViewer.Instance.GetPrevDocCharacterSetAttribute);
		}

		
		public void SetPrevDocCharacterSetAttribute(string aPrevDocCharacterSet)
		{
			nsString.Set(_docViewer.Instance.SetPrevDocCharacterSetAttribute, aPrevDocCharacterSet);
		}

		/// <summary>
		/// Tell the container to shrink-to-fit or grow-to-fit its contents
		///	</summary>		
		public void SizeToContent()
		{
			_docViewer.Instance.SizeToContent();
		}

		/// <summary>
		/// Use this attribute to access all the Bidi options in one operation
		/// </summary>	
		public uint GetBidiOptionsAttribute()
		{
			return _docViewer.Instance.GetBidiOptionsAttribute();
		}

		/// <summary>
		/// Use this attribute to access all the Bidi options in one operation
		/// </summary>
		public void SetBidiOptionsAttribute(uint aBidiOptions)
		{
			_docViewer.Instance.SetBidiOptionsAttribute(aBidiOptions);
		}

		/// <summary>
		///The minimum font size 
		///</summary>		
		public int GetMinFontSizeAttribute()
		{
			return _docViewer.Instance.GetMinFontSizeAttribute();
		}

		/// <summary>
		///The minimum font size 
		///</summary>		
		public void SetMinFontSizeAttribute(int aMinFontSize)
		{
			_docViewer.Instance.SetMinFontSizeAttribute(aMinFontSize);
		}
	}
}