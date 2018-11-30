
using System;
using Xwt.Drawing;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using Xwt.Backends;

namespace Xwt
{

	public partial class CellView
	{

		public virtual object GetValue ()
		{
			return null;
		}

		public virtual IDataField GetDataField ()
		{
			return null;
		}
	}

	public partial class CanvasCellView
	{

	}

	public partial class CheckBoxCellView
	{

		public override object GetValue ()
		{
            return Active;
		}

		public override IDataField GetDataField ()
		{
            return this.ActiveField;
		}
	}

	public partial class ImageCellView
	{
		public override object GetValue ()
		{
			return Image;
		}

		public override IDataField GetDataField ()
		{
			return this.ImageField;
		}
	}

	public partial class TextCellView
	{
		public override object GetValue ()
		{
			return Text;
		}

		public override IDataField GetDataField ()
		{
			return this.TextField;
		}
	}

	public partial class ListViewColumn
	{
		// see: Eto.Forms.GridColumn

		/// <summary>
		/// Gets or sets a value indicating whether this column will auto size to the content of the grid.
		/// </summary>
		/// <remarks>
		/// This usually will only auto size based on the visible content to be as performant as possible.
		/// </remarks>
		/// <value><c>true</c> to auto size the column; otherwise, <c>false</c>.</value>
		public bool AutoSize {
			get; set;
		}

		/// <summary>
		/// Gets or sets the initial width of the column.
		/// </summary>
		/// <value>The width of the column.</value>

		int width = -1;
		public int Width {
			get {
				return width;
			}
			set {
				width = value;
				if (Parent != null)
					Parent.UpdateColumn (this, Handle, ListViewColumnChange.Width);
			}
		}
	}

    public static class CellViewExtensions {
        
        public static CellView SetDataField (this CellView cellView, IDataField field) {
            
            if (field.Index == -1)
                throw new InvalidOperationException ("Field must be bound to a data source");
            if (cellView is CheckBoxCellView checkBox) {
                if (field.FieldType == typeof (bool))
                    checkBox.ActiveField = (IDataField<bool>)field;
                if (field.FieldType == typeof (CheckBoxState))
                    checkBox.StateField = (IDataField<CheckBoxState>)field;
                return cellView;
            }  
            if (cellView is ImageCellView imageCellView && field.FieldType == typeof (Image)) {
                imageCellView.ImageField = (IDataField<Image>)field;
                return cellView;
            }  
            if (cellView is ComboBoxCellView comboBoxCellView && field.FieldType == typeof (string)) {
                comboBoxCellView.SelectedTextField = (IDataField<string>)field;
                return cellView;
            }  
            if (cellView is TextCellView textCellView && field.FieldType == typeof (string)) {
                textCellView.TextField = field;
                return cellView;
            }  
            if (cellView is CanvasCellView) { 
                // canvascellview has no datafield
                return cellView;
            }

            throw new NotSupportedException ();

        }
    }
}
