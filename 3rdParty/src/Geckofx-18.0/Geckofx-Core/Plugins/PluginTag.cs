using Gecko.Interop;

namespace Gecko.Plugins
{
	public sealed class PluginTag
	{
		internal ComPtr<nsIPluginTag> _pluginTag;

		internal PluginTag(nsIPluginTag pluginTag)
		{
			_pluginTag = new ComPtr<nsIPluginTag>( pluginTag );
		}

		public bool Blocklisted
		{
			get { return _pluginTag.Instance.GetBlocklistedAttribute(); }
			set{_pluginTag.Instance.SetBlocklistedAttribute( value );}
		}

		public string Description
		{
			get { return nsString.Get(_pluginTag.Instance.GetDescriptionAttribute); }
		}

		public bool Disabled
		{
			get { return _pluginTag.Instance.GetDisabledAttribute(); }
			set { _pluginTag.Instance.SetDisabledAttribute(value); }
		}

		public string FileName
		{
			get { return nsString.Get(_pluginTag.Instance.GetFilenameAttribute); }
		}

		public string Fullpath
		{
			get { return nsString.Get(_pluginTag.Instance.GetFullpathAttribute); }
		}

		public string Name
		{
			get { return nsString.Get(_pluginTag.Instance.GetNameAttribute); }
		}

		public string Version
		{
			get { return nsString.Get(_pluginTag.Instance.GetVersionAttribute); }
		}
	}
}