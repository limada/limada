using System;
using Gecko.Interop;

namespace Gecko
{
	/// <summary>
	/// Identifies a range of content in a document.
	/// </summary>
	public class GeckoRange
		: ICloneable
	{
		private ComPtr<nsIDOMRange> _range; 

		internal GeckoRange(nsIDOMRange range)
		{
			_range = new ComPtr<nsIDOMRange>( range );
		}
		
		/// <summary>
		/// Gets the unmanaged nsIDOMRange which this instance wraps.
		/// </summary>
		public nsIDOMRange DomRange
		{
			get { return _range.Instance; }
		}		
		
		public GeckoNode StartContainer
		{
			get
			{
				return _range.Instance.GetStartContainerAttribute()
							   .Wrap( GeckoNode.Create );
			}
		}

		public int StartOffset { get { return _range.Instance.GetStartOffsetAttribute(); } }

		public GeckoNode EndContainer
		{
			get
			{
				return _range.Instance.GetEndContainerAttribute()
							   .Wrap( GeckoNode.Create );
			}
		}

		public int EndOffset { get { return _range.Instance.GetEndOffsetAttribute(); } }

		public bool Collapsed { get { return _range.Instance.GetCollapsedAttribute(); } }
		
		public GeckoNode CommonAncestorContainer
		{
			get
			{
				return _range.Instance.GetCommonAncestorContainerAttribute()
							   .Wrap( GeckoNode.Create );

			}
		}

		public void SetStart(GeckoNode node, int offset)
		{
			_range.Instance.SetStart(node.DomObject, offset);
		}
		
		public void SetEnd(GeckoNode node, int offset)
		{
			_range.Instance.SetEnd(node.DomObject, offset);
		}
		
		public void SetStartBefore(GeckoNode node)
		{
			_range.Instance.SetStartBefore(node.DomObject);
		}
		
		public void SetStartAfter(GeckoNode node)
		{
			_range.Instance.SetStartAfter(node.DomObject);
		}
		
		public void SetEndBefore(GeckoNode node)
		{
			_range.Instance.SetEndBefore(node.DomObject);
		}
		
		public void SetEndAfter(GeckoNode node)
		{
			_range.Instance.SetEndAfter(node.DomObject);
		}
		
		public void Collapse(bool toStart)
		{
			_range.Instance.Collapse(toStart);
		}
		
		public void SelectNode(GeckoNode node)
		{
			_range.Instance.SelectNode((nsIDOMNode)node.DomObject);
		}
		
		public void SelectNodeContents(GeckoNode node)
		{
			_range.Instance.SelectNodeContents((nsIDOMNode)node.DomObject);
		}

		public short CompareBoundaryPoints(GeckoRangeComparsion how, GeckoRange sourceRange)
		{
			return _range.Instance.CompareBoundaryPoints((ushort)how, sourceRange.DomRange);
		}
		
		public void DeleteContents()
		{
			_range.Instance.DeleteContents();
		}
		
		public GeckoNode ExtractContents()
		{
			return _range.Instance.ExtractContents()
						 .Wrap( GeckoNode.Create );
		}
		
		public GeckoNode CloneContents()
		{
			return _range.Instance.CloneContents().Wrap(GeckoNode.Create);
		}
		
		public void InsertNode(GeckoNode newNode)
		{
			_range.Instance.InsertNode(newNode.DomObject);
		}
		
		public void SurroundContents(GeckoNode newParent)
		{
			_range.Instance.SurroundContents(newParent.DomObject);
		}
		
		public object Clone()
		{
			return CloneRange();
		}

		public GeckoRange CloneRange()
		{
			return new GeckoRange(_range.Instance.CloneRange());
		}
		
		public override string ToString()
		{
			return nsString.Get( _range.Instance.ToString );
		}
		
		public void Detach()
		{
			_range.Instance.Detach();
		}

		public bool IsPointInRange(GeckoNode node, int offset)
		{
			return DomRange.IsPointInRange(node.DomObject, offset);			
		}
	}

	public enum GeckoRangeComparsion
	: ushort
	{
		StartToStart = (int)nsIDOMRangeConsts.START_TO_START,
		StartToEnd = (int)nsIDOMRangeConsts.START_TO_END,
		EndToEnd = (int)nsIDOMRangeConsts.END_TO_END,
		EndToStart = (int)nsIDOMRangeConsts.END_TO_START
	}
}