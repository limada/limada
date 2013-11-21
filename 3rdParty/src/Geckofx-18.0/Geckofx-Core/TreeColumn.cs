using Gecko.Interop;

namespace Gecko
{
	public class TreeColumn
	{
		internal ComPtr<nsITreeColumn> _treeColumn;

		internal TreeColumn(nsITreeColumn treeColumn)
		{
			_treeColumn = new ComPtr<nsITreeColumn>( treeColumn );
		}
	}
}