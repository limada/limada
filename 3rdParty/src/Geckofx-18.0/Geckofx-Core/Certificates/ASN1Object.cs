using Gecko.Interop;

namespace Gecko.Certificates
{
	public sealed class ASN1Object
	{
		internal ComPtr<nsIASN1Object> _object;

		internal ASN1Object( nsIASN1Object o )
		{
			_object = new ComPtr<nsIASN1Object>( o );
		}


		public string DisplayName
		{
			get { return nsString.Get( _object.Instance.GetDisplayNameAttribute ); }
			set { nsString.Set( _object.Instance.SetDisplayNameAttribute, value ); }
		}

		public string DisplayValue
		{
			get { return nsString.Get( _object.Instance.GetDisplayValueAttribute ); }
			set { nsString.Set( _object.Instance.SetDisplayValueAttribute, value ); }
		}

		public uint Tag
		{
			get { return _object.Instance.GetTagAttribute(); }
			set { _object.Instance.SetTagAttribute( value ); }
		}

		public uint Type
		{
			get { return _object.Instance.GetTypeAttribute(); }
			set { _object.Instance.SetTypeAttribute( value ); }
		}
	}
}