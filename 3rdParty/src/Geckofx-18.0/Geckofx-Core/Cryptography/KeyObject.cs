using Gecko.Interop;

namespace Gecko.Cryptography
{
	public sealed class KeyObject
	{
		private ComPtr<nsIKeyObject> _keyObject;

		internal KeyObject(nsIKeyObject keyObject)
		{
			_keyObject = new ComPtr<nsIKeyObject>( keyObject );
		}
	}
}