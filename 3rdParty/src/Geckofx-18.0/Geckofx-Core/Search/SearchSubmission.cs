using System;
using Gecko.Interop;

namespace Gecko.Search
{
	public sealed class SearchSubmission
	{
		private ComPtr<nsISearchSubmission> _searchSubmission;

		internal SearchSubmission(nsISearchSubmission searchSubmission)
		{
			_searchSubmission =new ComPtr<nsISearchSubmission>( searchSubmission );
			//???
			_searchSubmission.Instance.GetPostDataAttribute();
		}

		public Uri Uri
		{
			get { return _searchSubmission.Instance.GetUriAttribute().ToUri(); }
		}

		public IO.InputStream PostData
		{
			get
			{
				return _searchSubmission.Instance.GetPostDataAttribute()
										.Wrap( IO.InputStream.Create );
			}
		}
	}
}