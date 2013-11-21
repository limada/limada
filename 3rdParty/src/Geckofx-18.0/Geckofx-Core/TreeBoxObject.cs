using Gecko.Interop;

namespace Gecko
{
	public class TreeBoxObject
	{
		internal ComPtr<nsITreeBoxObject> _treeBoxObject;

		internal TreeBoxObject(nsITreeBoxObject treeBoxObject)
		{
			_treeBoxObject = new ComPtr<nsITreeBoxObject>( treeBoxObject );
		}
	}
}