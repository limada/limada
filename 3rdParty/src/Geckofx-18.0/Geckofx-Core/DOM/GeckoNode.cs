using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Diagnostics;
using Gecko.Collections;
using Gecko.DOM;
using Gecko.Interop;

namespace Gecko
{
	/// <summary>
	/// Provides a base class for DOM nodes.
	/// </summary>
	public class GeckoNode
		:IDisposable,IEquatable<GeckoNode>
	{
		static GeckoWrapperCache<nsIDOMNode, GeckoNode> m_nodeCache = new GeckoWrapperCache<nsIDOMNode, GeckoNode>(DOMSelector.CreateDomNodeWrapper);

		#region fields
		private ComPtr<nsIDOMNode> _domNode;
		#endregion

		#region ctor & creation methods
		internal GeckoNode(nsIDOMNode domNode)
		{
			//ComDebug.WriteDebugInfo( domObject );
			_domNode = new ComPtr<nsIDOMNode>(domNode);
		}

		~GeckoNode()
		{
			Xpcom.DisposeObject(ref _domNode);
		}

		public void Dispose()
		{
			Xpcom.DisposeObject( ref _domNode );
			GC.SuppressFinalize( this );
		}

		internal static GeckoNode Create(nsIDOMNode domObject)
		{
			return m_nodeCache.Get(domObject);
		}
		#endregion


		#region Properties for Native XPCOM objects access
		/// <summary>
		/// Gets the underlying XPCOM object.
		/// </summary>
		public nsIDOMNode DomObject
		{
			get { return _domNode.Instance; }
		}
		#endregion

		#region Equality
		public override bool Equals(object obj)
		{
			if (ReferenceEquals(this, obj)) return true;
			if ( ReferenceEquals( null, obj ) ) return false;
			if ( obj is GeckoNode )
			{
				// compare native objects, NOT wrappers
				return _domNode.Instance.GetHashCode() == ((GeckoNode)obj)._domNode.Instance.GetHashCode();
			}
			return false;
		}

		public bool Equals(GeckoNode obj)
		{
			if ( ReferenceEquals( this, obj ) ) return true;
			if (ReferenceEquals(null, obj)) return false;
			return _domNode.Instance.GetHashCode() == obj._domNode.Instance.GetHashCode();
		}

		public override int GetHashCode()
		{
			return _domNode.Instance.GetHashCode();
		}
		#endregion

		/// <summary>
		/// Gets the text contents of the node.
		/// </summary>
		public string TextContent
		{
			get { return nsString.Get(_domNode.Instance.GetTextContentAttribute); }
			set { nsString.Set(_domNode.Instance.SetTextContentAttribute, value); }
		}

		/// <summary>
		/// Gets or sets the value of the node.
		/// </summary>
		public string NodeValue
		{
			get { return nsString.Get(_domNode.Instance.GetNodeValueAttribute); }
			set { nsString.Set(_domNode.Instance.SetNodeValueAttribute, value); }
		}

		public string NodeName
		{
			get { return nsString.Get(_domNode.Instance.GetNodeNameAttribute); }
		}

		/// <summary>
		/// Gets a collection containing all child nodes of this node.
		/// </summary>
		public IGeckoArray<GeckoNode> ChildNodes
		{
			get
			{
				return _domNode.Instance.GetChildNodesAttribute()
									.Wrap( x => new DomNodeList<GeckoNode, nsIDOMNode>( x, Create ) );
			}
		}

		public GeckoNode FirstChild { get { return _domNode.Instance.GetFirstChildAttribute().Wrap(Create); } }
		public GeckoNode LastChild { get { return _domNode.Instance.GetLastChildAttribute().Wrap(Create); } }
		public GeckoNode NextSibling { get { return _domNode.Instance.GetNextSiblingAttribute().Wrap(Create); } }
		public GeckoNode PreviousSibling { get { return _domNode.Instance.GetPreviousSiblingAttribute().Wrap(Create); } }
		public bool HasChildNodes { get { return _domNode.Instance.HasChildNodes(); } }
		public bool HasAttributes { get { return _domNode.Instance.HasAttributes(); } }

		public GeckoDomDocument OwnerDocument
		{
			get
			{
				var nativeDoc = _domNode.Instance.GetOwnerDocumentAttribute();
				return nativeDoc.Wrap( GeckoDomDocument.CreateDomDocumentWraper );
			}
		}

		public GeckoNode AppendChild(GeckoNode node)
		{
			if (node == null)
				throw new ArgumentNullException("node");

			_domNode.Instance.AppendChild(node._domNode.Instance);
			return node;
		}

		public GeckoNode CloneNode(bool deep)
		{
			return GeckoNode.Create(_domNode.Instance.CloneNode(deep, 1));
		}

		public GeckoNode InsertBefore(GeckoNode newChild, GeckoNode before)
		{
			if (newChild == null)
				throw new ArgumentNullException("newChild");
			if (before == null)
				throw new ArgumentNullException("before");

			_domNode.Instance.InsertBefore(newChild._domNode.Instance, before._domNode.Instance);
			return newChild;
		}

		public GeckoNode RemoveChild(GeckoNode node)
		{
			if (node == null)
				throw new ArgumentNullException("node");

			_domNode.Instance.RemoveChild(node._domNode.Instance);
			return node;
		}

		public GeckoNode ReplaceChild(GeckoNode newChild, GeckoNode oldChild)
		{
			if (newChild == null)
				throw new ArgumentNullException("newChild");
			if (oldChild == null)
				throw new ArgumentNullException("oldChild");

			_domNode.Instance.ReplaceChild(newChild._domNode.Instance, oldChild._domNode.Instance);
			return newChild;
		}

		public string NamespaceURI
		{
			get { return nsString.Get(_domNode.Instance.GetNamespaceURIAttribute); }
		}

		public string Prefix
		{
			get { return nsString.Get(_domNode.Instance.GetPrefixAttribute); }
		}

		public string LocalName
		{
			get { return nsString.Get(_domNode.Instance.GetLocalNameAttribute); }
		}

		public GeckoNamedNodeMap Attributes
		{
			get { return _domNode.Instance.GetAttributesAttribute().Wrap(GeckoNamedNodeMap.Create); }
		}

		private NodeType m_cachedType;

		public NodeType NodeType
		{
			get {
				if (m_cachedType != 0)
					return m_cachedType;

				return m_cachedType = (NodeType)_domNode.Instance.GetNodeTypeAttribute(); 
			}
		}

		public GeckoNode ParentNode
		{
			get { return _domNode.Instance.GetParentNodeAttribute().Wrap(Create); }
		}

		public GeckoElement ParentElement
		{
			get
			{
				nsIDOMNode node = _domNode.Instance.GetParentNodeAttribute();
				while ( node != null && !( node is nsIDOMElement ) )
				{
					node = node.GetParentNodeAttribute();
				}
				// after cycle node is nsIDOMElement or null

				return ((nsIDOMElement)node).Wrap(GeckoElement.CreateDomElementWrapper);
			}
		}


		private nsIDOMXPathResult EvaluateXPath( string xpath )
		{
			var evaluator = Xpcom.CreateInstance2<nsIDOMXPathEvaluator>( Contracts.XPathEvaluator );
			nsIDOMNode node = DomObject;
			nsIDOMXPathNSResolver resolver = evaluator.Instance.CreateNSResolver( node );
			nsIDOMXPathResult result =
				( nsIDOMXPathResult ) evaluator.Instance.Evaluate( new nsAString( xpath ), node, resolver, 0, null );
			return result;
		}

		/// <summary>
		/// Get GeckoNodes from give xpath expression.
		/// </summary>
		/// <param name="xpath"></param>
		/// <returns></returns>
		public IEnumerable<GeckoNode> GetNodes(string xpath)
		{
			var result = EvaluateXPath( xpath );

			return new GeckoNodeEnumerable(result);
		}

		/// <summary>
		/// Get GeckoNodes from give xpath expression.
		/// </summary>
		/// <param name="xpath"></param>
		/// <returns></returns>
		public IEnumerable<GeckoHtmlElement> GetElements(string xpath)
		{
			var result = EvaluateXPath( xpath );

			return new GeckoElementEnumerable(result);
		}

		public GeckoNode GetSingleElement( string xpath )
		{
			var result = EvaluateXPath( xpath );

			var type = result.GetResultTypeAttribute();

			if ( type == nsIDOMXPathResultConsts.UNORDERED_NODE_ITERATOR_TYPE )
			{
				return result.IterateNext().Wrap( Create );
			}

			return result.GetSingleNodeValueAttribute().Wrap( Create );
		}


		public DomEventTarget GetEventTarget()
		{
			var eventTarget = Xpcom.QueryInterface<nsIDOMEventTarget>( _domNode.Instance );
			return eventTarget.Wrap( DomEventTarget.Create );
		}
	}
}
