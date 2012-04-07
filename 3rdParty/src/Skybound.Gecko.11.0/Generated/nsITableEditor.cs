// --------------------------------------------------------------------------------------------
// Version: MPL 1.1/GPL 2.0/LGPL 2.1
// 
// The contents of this file are subject to the Mozilla Public License Version
// 1.1 (the "License"); you may not use this file except in compliance with
// the License. You may obtain a copy of the License at
// http://www.mozilla.org/MPL/
// 
// Software distributed under the License is distributed on an "AS IS" basis,
// WITHOUT WARRANTY OF ANY KIND, either express or implied. See the License
// for the specific language governing rights and limitations under the
// License.
// 
// <remarks>
// Generated by IDLImporter from file nsITableEditor.idl
// 
// You should use these interfaces when you access the COM objects defined in the mentioned
// IDL/IDH file.
// </remarks>
// --------------------------------------------------------------------------------------------
namespace Gecko
{
	using System;
	using System.Runtime.InteropServices;
	using System.Runtime.InteropServices.ComTypes;
	using System.Runtime.CompilerServices;
	using System.Windows.Forms;
	
	
	/// <summary>nsITableEditor </summary>
	[ComImport()]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("4805e684-49b9-11d3-9ce4-ed60bd6cb5bc")]
	public interface nsITableEditor
	{
		
		/// <summary>
        ///Insert table methods
        /// Insert relative to the selected cell or the
        /// cell enclosing the selection anchor
        /// The selection is collapsed and is left in the new cell
        /// at the same row,col location as the original anchor cell
        ///
        /// @param aNumber    Number of items to insert
        /// @param aAfter     If TRUE, insert after the current cell,
        /// else insert before current cell
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void InsertTableCell(int aNumber, [MarshalAs(UnmanagedType.U1)] bool aAfter);
		
		/// <summary>Member InsertTableColumn </summary>
		/// <param name='aNumber'> </param>
		/// <param name='aAfter'> </param>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void InsertTableColumn(int aNumber, [MarshalAs(UnmanagedType.U1)] bool aAfter);
		
		/// <summary>Member InsertTableRow </summary>
		/// <param name='aNumber'> </param>
		/// <param name='aAfter'> </param>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void InsertTableRow(int aNumber, [MarshalAs(UnmanagedType.U1)] bool aAfter);
		
		/// <summary>
        ///Delete table methods
        /// Delete starting at the selected cell or the
        /// cell (or table) enclosing the selection anchor
        /// The selection is collapsed and is left in the
        /// cell at the same row,col location as
        /// the previous selection anchor, if possible,
        /// else in the closest neigboring cell
        ///
        /// @param aNumber    Number of items to insert/delete
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void DeleteTable();
		
		/// <summary>
        ///Delete just the cell contents
        /// This is what should happen when Delete key is used
        /// for selected cells, to minimize upsetting the table layout
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void DeleteTableCellContents();
		
		/// <summary>
        ///Delete cell elements as well as contents
        /// @param aNumber   Number of contiguous cells, rows, or columns
        ///
        /// When there are more than 1 selected cells, aNumber is ignored.
        /// For Delete Rows or Columns, the complete columns or rows are
        /// determined by the selected cells. E.g., to delete 2 complete rows,
        /// user simply selects a cell in each, and they don't
        /// have to be contiguous.
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void DeleteTableCell(int aNumber);
		
		/// <summary>Member DeleteTableColumn </summary>
		/// <param name='aNumber'> </param>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void DeleteTableColumn(int aNumber);
		
		/// <summary>Member DeleteTableRow </summary>
		/// <param name='aNumber'> </param>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void DeleteTableRow(int aNumber);
		
		/// <summary>
        ///Table Selection methods
        /// Selecting a row or column actually
        /// selects all cells (not TR in the case of rows)
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void SelectTableCell();
		
		/// <summary>
        ///Select a rectangular block of cells:
        /// all cells falling within the row/column index of aStartCell
        /// to through the row/column index of the aEndCell
        /// aStartCell can be any location relative to aEndCell,
        /// as long as they are in the same table
        /// @param aStartCell  starting cell in block
        /// @param aEndCell    ending cell in block
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void SelectBlockOfCells([MarshalAs(UnmanagedType.Interface)] nsIDOMElement aStartCell, [MarshalAs(UnmanagedType.Interface)] nsIDOMElement aEndCell);
		
		/// <summary>Member SelectTableRow </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void SelectTableRow();
		
		/// <summary>Member SelectTableColumn </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void SelectTableColumn();
		
		/// <summary>Member SelectTable </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void SelectTable();
		
		/// <summary>Member SelectAllTableCells </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void SelectAllTableCells();
		
		/// <summary>
        ///Create a new TD or TH element, the opposite type of the supplied aSourceCell
        /// 1. Copy all attributes from aSourceCell to the new cell
        /// 2. Move all contents of aSourceCell to the new cell
        /// 3. Replace aSourceCell in the table with the new cell
        ///
        /// @param aSourceCell   The cell to be replaced
        /// @return              The new cell that replaces aSourceCell
        /// </summary>
		[return: MarshalAs(UnmanagedType.Interface)]
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		nsIDOMElement SwitchTableCellHeaderType([MarshalAs(UnmanagedType.Interface)] nsIDOMElement aSourceCell);
		
		/// <summary>
        ///Merges contents of all selected cells
        /// for selected cells that are adjacent,
        /// this will result in a larger cell with appropriate
        /// rowspan and colspan, and original cells are deleted
        /// The resulting cell is in the location of the
        /// cell at the upper-left corner of the adjacent
        /// block of selected cells
        ///
        /// @param aMergeNonContiguousContents:
        /// If true:
        /// Non-contiguous cells are not deleted,
        /// but their contents are still moved
        /// to the upper-left cell
        /// If false: contiguous cells are ignored
        ///
        /// If there are no selected cells,
        /// and selection or caret is in a cell,
        /// that cell and the one to the right
        /// are merged
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void JoinTableCells([MarshalAs(UnmanagedType.U1)] bool aMergeNonContiguousContents);
		
		/// <summary>
        ///Split a cell that has rowspan and/or colspan > 0
        /// into cells such that all new cells have
        /// rowspan = 1 and colspan = 1
        /// All of the contents are not touched --
        /// they will appear to be in the upper-left cell
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void SplitTableCell();
		
		/// <summary>
        ///Scan through all rows and add cells as needed so
        /// all locations in the cellmap are occupied.
        /// Used after inserting single cells or pasting
        /// a collection of cells that extend past the
        /// previous size of the table
        /// If aTable is null, it uses table enclosing the selection anchor
        /// This doesn't doesn't change the selection,
        /// thus it can be used to fixup all tables
        /// in a page independent of the selection
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void NormalizeTable([MarshalAs(UnmanagedType.Interface)] nsIDOMElement aTable);
		
		/// <summary>
        ///Get the row an column index from the layout's cellmap
        /// If aCell is null, it will try to find enclosing table of selection anchor
        ///
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void GetCellIndexes([MarshalAs(UnmanagedType.Interface)] nsIDOMElement aCell, ref int aRowIndex, ref int aColIndex);
		
		/// <summary>
        ///Get the number of rows and columns in a table from the layout's cellmap
        /// If aTable is null, it will try to find enclosing table of selection ancho
        /// Note that all rows in table will not have this many because of
        /// ROWSPAN effects or if table is not "rectangular" (has short rows)
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void GetTableSize([MarshalAs(UnmanagedType.Interface)] nsIDOMElement aTable, ref int aRowCount, ref int aColCount);
		
		/// <summary>
        ///Get a cell element at cellmap grid coordinates
        /// A cell that spans across multiple cellmap locations will
        /// be returned multiple times, once for each location it occupies
        ///
        /// @param aTable                   A table in the document
        /// @param aRowIndex, aColIndex     The 0-based cellmap indexes
        ///
        /// (in C++ returns: NS_EDITOR_ELEMENT_NOT_FOUND if an element is not found
        /// passes NS_SUCCEEDED macro)
        ///
        /// You can scan for all cells in a row or column
        /// by iterating through the appropriate indexes
        /// until the returned aCell is null
        /// </summary>
		[return: MarshalAs(UnmanagedType.Interface)]
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		nsIDOMElement GetCellAt([MarshalAs(UnmanagedType.Interface)] nsIDOMElement aTable, int aRowIndex, int aColIndex);
		
		/// <summary>
        ///Get a cell at cellmap grid coordinates and associated data
        /// A cell that spans across multiple cellmap locations will
        /// be returned multiple times, once for each location it occupies
        /// Examine the returned aStartRowIndex and aStartColIndex to see
        /// if it is in the same layout column or layout row:
        /// A "layout row" is all cells sharing the same top edge
        /// A "layout column" is all cells sharing the same left edge
        /// This is important to determine what to do when inserting or deleting a column or row
        ///
        /// @param aTable                   A table in the document
        /// @param aRowIndex, aColIndex     The 0-based cellmap indexes
        /// returns values:
        /// @param aCell                    The cell at this cellmap location
        /// @param aStartRowIndex           The row index where cell starts
        /// @param aStartColIndex           The col index where cell starts
        /// @param aRowSpan                 May be 0 (to span down entire table) or number of cells spanned
        /// @param aColSpan                 May be 0 (to span across entire table) or number of cells spanned
        /// @param aActualRowSpan           The actual number of cellmap locations (rows) spanned by the cell
        /// @param aActualColSpan           The actual number of cellmap locations (columns) spanned by the cell
        /// @param aIsSelected
        /// @param
        ///
        /// (in C++ returns: NS_EDITOR_ELEMENT_NOT_FOUND if an element is not found
        /// passes NS_SUCCEEDED macro)
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void GetCellDataAt([MarshalAs(UnmanagedType.Interface)] nsIDOMElement aTable, int aRowIndex, int aColIndex, [MarshalAs(UnmanagedType.Interface)] ref nsIDOMElement aCell, ref int aStartRowIndex, ref int aStartColIndex, ref int aRowSpan, ref int aColSpan, ref int aActualRowSpan, ref int aActualColSpan, [MarshalAs(UnmanagedType.U1)] ref bool aIsSelected);
		
		/// <summary>
        ///Get the first row element in a table
        ///
        /// @return            The row at the requested index
        /// Returns null if there are no rows in table
        /// (in C++ returns: NS_EDITOR_ELEMENT_NOT_FOUND if an element is not found
        /// passes NS_SUCCEEDED macro)
        /// </summary>
		[return: MarshalAs(UnmanagedType.Interface)]
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		nsIDOMNode GetFirstRow([MarshalAs(UnmanagedType.Interface)] nsIDOMElement aTableElement);
		
		/// <summary>
        ///Get the next row element starting the search from aTableElement
        ///
        /// @param aTableElement Any TR or child-of-TR element in the document
        ///
        /// @return            The row to start search from
        /// and the row returned from the search
        /// Returns null if there isn't another row
        /// (in C++ returns: NS_EDITOR_ELEMENT_NOT_FOUND if an element is not found
        /// passes NS_SUCCEEDED macro)
        /// </summary>
		[return: MarshalAs(UnmanagedType.Interface)]
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		nsIDOMNode GetNextRow([MarshalAs(UnmanagedType.Interface)] nsIDOMNode aTableElement);
		
		/// <summary>
        ///Reset a selected cell or collapsed selection (the caret) after table editing
        ///
        /// @param aTable      A table in the document
        /// @param aRow        The row ...
        /// @param aCol        ... and column defining the cell
        /// where we will try to place the caret
        /// @param aSelected   If true, we select the whole cell instead of setting caret
        /// @param aDirection  If cell at (aCol, aRow) is not found,
        /// search for previous cell in the same
        /// column (aPreviousColumn) or row (ePreviousRow)
        /// or don't search for another cell (aNoSearch)
        /// If no cell is found, caret is place just before table;
        /// and if that fails, at beginning of document.
        /// Thus we generally don't worry about the return value
        /// and can use the nsSetSelectionAfterTableEdit stack-based
        /// object to insure we reset the caret in a table-editing method.
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void SetSelectionAfterTableEdit([MarshalAs(UnmanagedType.Interface)] nsIDOMElement aTable, int aRow, int aCol, int aDirection, [MarshalAs(UnmanagedType.U1)] bool aSelected);
		
		/// <summary>
        ///Examine the current selection and find
        /// a selected TABLE, TD or TH, or TR element.
        /// or return the parent TD or TH if selection is inside a table cell
        /// Returns null if no table element is found.
        ///
        /// @param aTagName         The tagname of returned element
        /// Note that "td" will be returned if name
        /// is actually "th"
        /// @param aCount           How many table elements were selected
        /// This tells us if we have multiple cells selected
        /// (0 if element is a parent cell of selection)
        /// @return                 The table element (table, row, or first selected cell)
        ///
        /// </summary>
		[return: MarshalAs(UnmanagedType.Interface)]
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		nsIDOMElement GetSelectedOrParentTableElement([MarshalAs(UnmanagedType.LPStruct)] nsAStringBase aTagName, ref int aCount);
		
		/// <summary>
        ///Generally used after GetSelectedOrParentTableElement
        /// to test if selected cells are complete rows or columns
        ///
        /// @param aElement           Any table or cell element or any element
        /// inside a table
        /// Used to get enclosing table.
        /// If null, selection's anchorNode is used
        ///
        /// @return
        /// 0                        aCellElement was not a cell
        /// (returned result = NS_ERROR_FAILURE)
        /// TABLESELECTION_CELL      There are 1 or more cells selected but
        /// complete rows or columns are not selected
        /// TABLESELECTION_ROW       All cells are in 1 or more rows
        /// and in each row, all cells selected
        /// Note: This is the value if all rows
        /// (thus all cells) are selected
        /// TABLESELECTION_COLUMN    All cells are in 1 or more columns
        /// and in each column, all cells are selected
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		uint GetSelectedCellsType([MarshalAs(UnmanagedType.Interface)] nsIDOMElement aElement);
		
		/// <summary>
        ///Get first selected element from first selection range.
        /// (If multiple cells were selected this is the first in the order they were selected)
        /// Assumes cell-selection model where each cell
        /// is in a separate range (selection parent node is table row)
        /// @param aCell     [OUT] Selected cell or null if ranges don't contain
        /// cell selections
        /// @param aRange    [OUT] Optional: if not null, return the selection range
        /// associated with the cell
        /// Returns the DOM cell element
        /// (in C++: returns NS_EDITOR_ELEMENT_NOT_FOUND if an element is not found
        /// passes NS_SUCCEEDED macro)
        /// </summary>
		[return: MarshalAs(UnmanagedType.Interface)]
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		nsIDOMElement GetFirstSelectedCell([MarshalAs(UnmanagedType.Interface)] ref nsIDOMRange aRange);
		
		/// <summary>
        ///Get first selected element in the table
        /// This is the upper-left-most selected cell in table,
        /// ignoring the order that the user selected them (order in the selection ranges)
        /// Assumes cell-selection model where each cell
        /// is in a separate range (selection parent node is table row)
        /// @param aCell       Selected cell or null if ranges don't contain
        /// cell selections
        /// @param aRowIndex   Optional: if not null, return row index of 1st cell
        /// @param aColIndex   Optional: if not null, return column index of 1st cell
        ///
        /// Returns the DOM cell element
        /// (in C++: returns NS_EDITOR_ELEMENT_NOT_FOUND if an element is not found
        /// passes NS_SUCCEEDED macro)
        /// </summary>
		[return: MarshalAs(UnmanagedType.Interface)]
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		nsIDOMElement GetFirstSelectedCellInTable(ref int aRowIndex, ref int aColIndex);
		
		/// <summary>
        ///Get next selected cell element from first selection range.
        /// Assumes cell-selection model where each cell
        /// is in a separate range (selection parent node is table row)
        /// Always call GetFirstSelectedCell() to initialize stored index of "next" cell
        /// @param aCell     Selected cell or null if no more selected cells
        /// or ranges don't contain cell selections
        /// @param aRange    Optional: if not null, return the selection range
        /// associated with the cell
        ///
        /// Returns the DOM cell element
        /// (in C++: returns NS_EDITOR_ELEMENT_NOT_FOUND if an element is not found
        /// passes NS_SUCCEEDED macro)
        /// </summary>
		[return: MarshalAs(UnmanagedType.Interface)]
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		nsIDOMElement GetNextSelectedCell([MarshalAs(UnmanagedType.Interface)] ref nsIDOMRange aRange);
	}
}
