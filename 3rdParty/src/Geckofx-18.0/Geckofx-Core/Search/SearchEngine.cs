using Gecko.Interop;

namespace Gecko.Search
{
	public sealed class SearchEngine
	{
		internal ComPtr<nsISearchEngine> _searchEngine;

		internal SearchEngine(nsISearchEngine searchEngine)
		{
			_searchEngine = new ComPtr<nsISearchEngine>( searchEngine );
		}

		public void AddParam(string name,string value,string responseType)
		{
			nsString.Set( _searchEngine.Instance.AddParam, name, value, responseType );
		}

		public string Name
		{
			get { return nsString.Get(_searchEngine.Instance.GetNameAttribute); }
		}

		public string SearchForm
		{
			get { return nsString.Get(_searchEngine.Instance.GetSearchFormAttribute); }
		}


		public string Alias
		{
			get { return nsString.Get(_searchEngine.Instance.GetAliasAttribute); }
			set { nsString.Set(_searchEngine.Instance.SetAliasAttribute, value); }
		}

		public string Description
		{
			get { return nsString.Get(_searchEngine.Instance.GetDescriptionAttribute); }
		}

		public bool Hidden
		{
			get { return _searchEngine.Instance.GetHiddenAttribute(); }
			set { _searchEngine.Instance.SetHiddenAttribute(value); }
		}

		public object GetSubmission(string data,string responseType)
		{
			return nsString.Pass<nsISearchSubmission>(_searchEngine.Instance.GetSubmission, data, responseType);
		}
	}
}