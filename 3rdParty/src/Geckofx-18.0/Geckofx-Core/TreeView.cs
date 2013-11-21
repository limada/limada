using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using Gecko.Interop;

namespace Gecko
{
	public class TreeView
	{
		private ComPtr<nsITreeView> _treeView;

		internal TreeView( nsITreeView treeView )
		{
			_treeView = new ComPtr<nsITreeView>( treeView );
		}

		/// <summary>
		/// The total number of rows in the tree (including the offscreen rows).
		/// </summary>
		public int RowCount
		{
			get { return _treeView.Instance.GetRowCountAttribute(); }
		}

		/// <summary>
		/// The selection for this view.
		/// </summary>
		public TreeSelection Selection
		{
			get
			{
				return _treeView.Instance.GetSelectionAttribute()
								.Wrap( x => new TreeSelection( x ) );
			}
			set { _treeView.Instance.SetSelectionAttribute(value._treeSelection.Instance); }
		}

		/// <summary>
		/// An atomized list of properties for a given row.  Each property, x, that
		/// the view gives back will cause the pseudoclass :moz-tree-row-x
		/// to be matched on the pseudoelement ::moz-tree-row.
		/// </summary>
		//[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		//void GetRowProperties(int index, [MarshalAs(UnmanagedType.Interface)] nsISupportsArray properties);

		/// <summary>
		/// An atomized list of properties for a given cell.  Each property, x, that
		/// the view gives back will cause the pseudoclass :moz-tree-cell-x
		/// to be matched on the ::moz-tree-cell pseudoelement.
		/// </summary>
		//[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		//void GetCellProperties(int row, [MarshalAs(UnmanagedType.Interface)] nsITreeColumn col, [MarshalAs(UnmanagedType.Interface)] nsISupportsArray properties);

		/// <summary>
		/// Called to get properties to paint a column background.  For shading the sort
		/// column, etc.
		/// </summary>
		//MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		//void GetColumnProperties([MarshalAs(UnmanagedType.Interface)] nsITreeColumn col, [MarshalAs(UnmanagedType.Interface)] nsISupportsArray properties);

		/// <summary>
		/// Methods that can be used to test whether or not a twisty should be drawn,
		/// and if so, whether an open or closed twisty should be used.
		/// </summary>
		public bool IsContainer(int index)
		{
			return _treeView.Instance.IsContainer(index);
		}

		/// <summary>Member IsContainerOpen </summary>
		/// <param name='index'> </param>
		/// <returns>A System.Boolean</returns>
		public bool IsContainerOpen(int index)
		{
			return _treeView.Instance.IsContainerOpen(index);
		}

		/// <summary>Member IsContainerEmpty </summary>
		/// <param name='index'> </param>
		/// <returns>A System.Boolean</returns>
		public bool IsContainerEmpty(int index)
		{
			return _treeView.Instance.IsContainerEmpty(index);
		}

		/// <summary>
		/// isSeparator is used to determine if the row at index is a separator.
		/// A value of true will result in the tree drawing a horizontal separator.
		/// The tree uses the ::moz-tree-separator pseudoclass to draw the separator.
		/// </summary>
		public bool IsSeparator(int index)
		{
			return _treeView.Instance.IsSeparator(index);
		}

		/// <summary>
		/// Specifies if there is currently a sort on any column. Used mostly by dragdrop
		/// to affect drop feedback.
		/// </summary>
		public bool IsSorted
		{
			get { return _treeView.Instance.IsSorted(); }
		}

		/// <summary>
		/// Methods used by the drag feedback code to determine if a drag is allowable at
		/// the current location. To get the behavior where drops are only allowed on
		/// items, such as the mailNews folder pane, always return false when
		/// the orientation is not DROP_ON.
		/// </summary>
		//bool CanDrop(int index, int orientation, [MarshalAs(UnmanagedType.Interface)] nsIDOMDataTransfer dataTransfer);

		/// <summary>
		/// Called when the user drops something on this view. The |orientation| param
		/// specifies before/on/after the given |row|.
		/// </summary>
		//void Drop(int row, int orientation, [MarshalAs(UnmanagedType.Interface)] nsIDOMDataTransfer dataTransfer);

		/// <summary>
		/// Methods used by the tree to draw thread lines in the tree.
		/// getParentIndex is used to obtain the index of a parent row.
		/// If there is no parent row, getParentIndex returns -1.
		/// </summary>
		public int GetParentIndex(int rowIndex)
		{
			return _treeView.Instance.GetParentIndex(rowIndex);
		}

		/// <summary>
		/// hasNextSibling is used to determine if the row at rowIndex has a nextSibling
		/// that occurs *after* the index specified by afterIndex.  Code that is forced
		/// to march down the view looking at levels can optimize the march by starting
		/// at afterIndex+1.
		/// </summary>
		public bool HasNextSibling(int rowIndex, int afterIndex)
		{
			return _treeView.Instance.HasNextSibling(rowIndex, afterIndex);
		}

		/// <summary>
		/// The level is an integer value that represents
		/// the level of indentation.  It is multiplied by the width specified in the
		/// :moz-tree-indentation pseudoelement to compute the exact indendation.
		/// </summary>
		public int GetLevel(int index)
		{
			return _treeView.Instance.GetLevel(index);
		}

		/// <summary>
		/// The image path for a given cell. For defining an icon for a cell.
		/// If the empty string is returned, the :moz-tree-image pseudoelement
		/// will be used.
		/// </summary>
		public string GetImageSrc(int row,TreeColumn col)
		{
			return nsString.Get(_treeView.Instance.GetImageSrc, row, col._treeColumn.Instance);
		}

		/// <summary>
		/// The progress mode for a given cell. This method is only called for
		/// columns of type |progressmeter|.
		/// </summary>
		public int GetProgressMode(int row, TreeColumn col)
		{
			return _treeView.Instance.GetProgressMode(row, col._treeColumn.Instance);
		}

		/// <summary>
		/// The value for a given cell. This method is only called for columns
		/// of type other than |text|.
		/// </summary>
		public string GetCellValue(int row, TreeColumn col)
		{
			return nsString.Get(_treeView.Instance.GetCellValue, row, col._treeColumn.Instance);
		}

		/// <summary>
		/// The text for a given cell.  If a column consists only of an image, then
		/// the empty string is returned.
		/// </summary>
		public string GetCellText(int row, TreeColumn col)
		{
			return nsString.Get(_treeView.Instance.GetCellText, row, col._treeColumn.Instance);
		}

		/// <summary>
		/// Called during initialization to link the view to the front end box object.
		/// </summary>
		public void SetTree(TreeBoxObject tree)
		{
			_treeView.Instance.SetTree(tree._treeBoxObject.Instance);
		}

		/// <summary>
		/// Called on the view when an item is opened or closed.
		/// </summary>
		public void ToggleOpenState(int index)
		{
			_treeView.Instance.ToggleOpenState(index);
		}
 
		/// <summary>
		/// Called on the view when a header is clicked.
		/// </summary>
		public void CycleHeader(TreeColumn col)
		{
			_treeView.Instance.CycleHeader(col._treeColumn.Instance);
		}

		/// <summary>
		/// Should be called from a XUL onselect handler whenever the selection changes.
		/// </summary>
		public void SelectionChanged()
		{
			_treeView.Instance.SelectionChanged();
		}

		/// <summary>
		/// Called on the view when a cell in a non-selectable cycling column (e.g., unread/flag/etc.) is clicked.
		/// </summary>
		public void CycleCell(int row, TreeColumn col)
		{
			_treeView.Instance.CycleCell(row, col._treeColumn.Instance);
		}

		/// <summary>
		/// isEditable is called to ask the view if the cell contents are editable.
		/// A value of true will result in the tree popping up a text field when
		/// the user tries to inline edit the cell.
		/// </summary>
		public bool IsEditable(int row,  TreeColumn col)
		{
			return _treeView.Instance.IsEditable(row, col._treeColumn.Instance);
		}

		/// <summary>
		/// isSelectable is called to ask the view if the cell is selectable.
		/// This method is only called if the selection style is |cell| or |text|.
		/// XXXvarga shouldn't this be called isCellSelectable?
		/// </summary>
		public bool IsSelectable(int row,TreeColumn col)
		{
			return _treeView.Instance.IsSelectable(row, col._treeColumn.Instance);
		}

		/// <summary>
		/// setCellValue is called when the value of the cell has been set by the user.
		/// This method is only called for columns of type other than |text|.
		/// </summary>
		public void SetCellValue(int row, TreeColumn col,  string value)
		{
			nsString.Set(_treeView.Instance.SetCellValue, row, col._treeColumn.Instance, value);
		}

		/// <summary>
		/// setCellText is called when the contents of the cell have been edited by the user.
		/// </summary>
		public void SetCellText(int row, TreeColumn col,string value)
		{
			nsString.Set(_treeView.Instance.SetCellText, row, col._treeColumn.Instance, value);
		}

		/// <summary>
		/// A command API that can be used to invoke commands on the selection.  The tree
		/// will automatically invoke this method when certain keys are pressed.  For example,
		/// when the DEL key is pressed, performAction will be called with the "delete" string.
		/// </summary>
		public void PerformAction( string action)
		{
			_treeView.Instance.PerformAction(action);
		}

		/// <summary>
		/// A command API that can be used to invoke commands on a specific row.
		/// </summary>
		public void PerformActionOnRow(string action, int row)
		{
			_treeView.Instance.PerformActionOnRow(action, row);
		}

		/// <summary>
		/// A command API that can be used to invoke commands on a specific cell.
		/// </summary>
		public void PerformActionOnCell( string action, int row,  TreeColumn col)
		{
			_treeView.Instance.PerformActionOnCell(action, row, col._treeColumn.Instance);
		}
	}
}
