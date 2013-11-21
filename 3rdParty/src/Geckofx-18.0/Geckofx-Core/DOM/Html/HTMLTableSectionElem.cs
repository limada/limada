

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using Gecko.Collections;
using Gecko.Interop;

namespace Gecko.DOM
{	
	public class GeckoTableSectionElement : GeckoHtmlElement
	{
		nsIDOMHTMLTableSectionElement DOMHTMLElement;
		internal GeckoTableSectionElement(nsIDOMHTMLTableSectionElement element) : base(element)
		{
			this.DOMHTMLElement = element;
		}
		public GeckoTableSectionElement(object element) : base(element as nsIDOMHTMLElement)
		{
			this.DOMHTMLElement = element as nsIDOMHTMLTableSectionElement;
		}
		public string Align {
			get { return nsString.Get(DOMHTMLElement.GetAlignAttribute); }
			set { DOMHTMLElement.SetAlignAttribute(new nsAString(value)); }
		}

		public string Ch {
			get { return nsString.Get(DOMHTMLElement.GetChAttribute); }
			set { DOMHTMLElement.SetChAttribute(new nsAString(value)); }
		}

		public string ChOff {
			get { return nsString.Get(DOMHTMLElement.GetChOffAttribute); }
			set { DOMHTMLElement.SetChOffAttribute(new nsAString(value)); }
		}

		public string VAlign {
			get { return nsString.Get(DOMHTMLElement.GetVAlignAttribute); }
			set { DOMHTMLElement.SetVAlignAttribute(new nsAString(value)); }
		}

		public IGeckoArray<GeckoElement> Rows
		{
			get
			{
				return DOMHTMLElement.GetRowsAttribute()
									 .Wrap( x => new DomHtmlCollection<GeckoElement, nsIDOMElement>( x, CreateDomElementWrapper ) );
			}
		}

		public GeckoHtmlElement insertRow(int index)
		{
			return new GeckoHtmlElement(DOMHTMLElement.InsertRow(index));
		}

		public void deleteRow(int index)
		{
			DOMHTMLElement.DeleteRow(index);
		}

	}
}
