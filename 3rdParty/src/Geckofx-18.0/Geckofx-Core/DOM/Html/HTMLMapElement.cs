

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using Gecko.Collections;
using Gecko.Interop;

namespace Gecko.DOM
{	
	public class GeckoMapElement : GeckoHtmlElement
	{
		nsIDOMHTMLMapElement DOMHTMLElement;
		internal GeckoMapElement(nsIDOMHTMLMapElement element) : base(element)
		{
			this.DOMHTMLElement = element;
		}
		public GeckoMapElement(object element) : base(element as nsIDOMHTMLElement)
		{
			this.DOMHTMLElement = element as nsIDOMHTMLMapElement;
		}
		public IGeckoArray<GeckoElement> Areas
		{
			get
			{
				return DOMHTMLElement.GetAreasAttribute()
									 .Wrap(
										   x =>
										   new DomHtmlCollection<GeckoElement, nsIDOMElement>(
											   x,
											   CreateDomElementWrapper ) );
			}
		}

		public string Name {
			get { return nsString.Get(DOMHTMLElement.GetNameAttribute); }
			set { DOMHTMLElement.SetNameAttribute(new nsAString(value)); }
		}

	}
}

