using Gecko.Interop;

namespace Gecko
{
	public class TreeSelection
	{
		internal ComPtr<nsITreeSelection> _treeSelection;

		internal TreeSelection(nsITreeSelection treeSelection)
		{
			_treeSelection = new ComPtr<nsITreeSelection>( treeSelection );
		}

		/// <summary>
		/// The tree widget for this selection.
		/// </summary>
		public TreeBoxObject Tree
		{
			get
			{
				return _treeSelection.Instance.GetTreeAttribute()
									 .Wrap( x => new TreeBoxObject( x ) );
			}
			set { _treeSelection.Instance.SetTreeAttribute(value._treeBoxObject.Instance); }
		}

		/// <summary>
		/// Boolean indicating single selection.
		/// </summary>
		public bool Single
		{
			get { return _treeSelection.Instance.GetSingleAttribute(); }
		}

		
		/// <summary>
		/// The number of rows currently selected in this tree.
		/// </summary>
		public int Count
		{
			get { return _treeSelection.Instance.GetCountAttribute(); }
		}

		/// <summary>
		/// Indicates whether or not the row at the specified index is
		/// part of the selection.
		/// </summary>
		public bool IsSelected(int index)
		{
			return _treeSelection.Instance.IsSelected(index);
		}

		/// <summary>
		/// Deselect all rows and select the row at the specified index.
		/// </summary>
		public void Select(int index)
		{
			_treeSelection.Instance.Select(index);
		}

		/// <summary>
		/// Perform a timed select.
		/// </summary>
		public void TimedSelect(int index, int delay)
		{
			_treeSelection.Instance.TimedSelect(index, delay);
		}

		/// <summary>
		/// Toggle the selection state of the row at the specified index.
		/// </summary>
		public void ToggleSelect(int index)
		{
			_treeSelection.Instance.ToggleSelect(index);
		}

		/// <summary>
		/// Select the range specified by the indices.  If augment is true,
		/// then we add the range to the selection without clearing out anything
		/// else.  If augment is false, everything is cleared except for the specified range.
		/// </summary>
		public void RangedSelect(int startIndex, int endIndex,bool augment)
		{
			_treeSelection.Instance.RangedSelect(startIndex, endIndex, augment);
		}

		/// <summary>
		/// Clears the range.
		/// </summary>
		public void ClearRange(int startIndex, int endIndex)
		{
			_treeSelection.Instance.ClearRange(startIndex, endIndex);
		}

		/// <summary>
		/// Clears the selection.
		/// </summary>
		public void ClearSelection()
		{
			_treeSelection.Instance.ClearSelection();
		}

		/// <summary>
		/// Inverts the selection.
		/// </summary>
		public void InvertSelection()
		{
			_treeSelection.Instance.InvertSelection();
		}

		/// <summary>
		/// Selects all rows.
		/// </summary>
		public void SelectAll()
		{
			_treeSelection.Instance.SelectAll();
		}

		/// <summary>
		/// Iterate the selection using these methods.
		/// </summary>
		public int RangeCount
		{
			get { return _treeSelection.Instance.GetRangeCount(); }
		}

		/// <summary>Member GetRangeAt </summary>
		/// <param name='i'> </param>
		/// <param name='min'> </param>
		/// <param name='max'> </param>
		public void GetRangeAt(int i, ref int min, ref int max)
		{
			_treeSelection.Instance.GetRangeAt(i, ref min, ref max);
		}

		/// <summary>
		/// Can be used to invalidate the selection.
		/// </summary>
		public void InvalidateSelection()
		{
			_treeSelection.Instance.InvalidateSelection();
		}

		/// <summary>
		/// Called when the row count changes to adjust selection indices.
		/// </summary>
		public void AdjustSelection(int index, int count)
		{
			_treeSelection.Instance.AdjustSelection(index, count);
		}

		/// <summary>
		/// This attribute is a boolean indicating whether or not the
		/// "select" event should fire when the selection is changed using
		/// one of our methods.  A view can use this to temporarily suppress
		/// the selection while manipulating all of the indices, e.g., on
		/// a sort.
		/// Note: setting this attribute to false will fire a select event.
		/// </summary>
		public bool SelectEventsSuppressed
		{
			get { return _treeSelection.Instance.GetSelectEventsSuppressedAttribute(); }
			set { _treeSelection.Instance.SetSelectEventsSuppressedAttribute(value); }
		}

		/// <summary>
		/// The current item (the one that gets a focus rect in addition to being
		/// selected).
		/// </summary>
		public int CurrentIndex
		{
			get { return _treeSelection.Instance.GetCurrentIndexAttribute(); }
			set { _treeSelection.Instance.SetCurrentIndexAttribute(value); }
		}

		/// <summary>
		/// The current column.
		/// </summary>
		public TreeColumn CureentColumn
		{
			get
			{
				return _treeSelection.Instance.GetCurrentColumnAttribute()
									 .Wrap( x => new TreeColumn( x ) );
			}
			set { _treeSelection.Instance.SetCurrentColumnAttribute(value._treeColumn.Instance); }
		}

		/// <summary>
		/// The selection "pivot".  This is the first item the user selected as
		/// part of a ranged select.
		/// </summary>
		public int ShiftSelectPivot
		{
			get { return _treeSelection.Instance.GetShiftSelectPivotAttribute(); }
		}
	}
}