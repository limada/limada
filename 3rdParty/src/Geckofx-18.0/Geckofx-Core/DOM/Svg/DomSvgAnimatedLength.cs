using Gecko.Interop;

namespace Gecko.DOM.Svg
{
	public class DomSvgAnimatedLength
	{
		private ComPtr<nsIDOMSVGAnimatedLength> _domSvgAnimatedLength;


		private DomSvgAnimatedLength( nsIDOMSVGAnimatedLength domSvgAnimatedLength )
		{
			_domSvgAnimatedLength = new ComPtr<nsIDOMSVGAnimatedLength>(domSvgAnimatedLength);
		}

		public DomSvgLength AnimVal
		{
			get { return _domSvgAnimatedLength.Instance.GetAnimValAttribute().Wrap(DomSvgLength.Create); }
		}

		public DomSvgLength BaseVal
		{
			get { return _domSvgAnimatedLength.Instance.GetBaseValAttribute().Wrap(DomSvgLength.Create); }
		}
	}
}