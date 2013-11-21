using System.Text;
using Gecko.Interop;

namespace Gecko.IO
{
	public sealed class ScriptableInputStream
	{
		private ComPtr<nsIScriptableInputStream> _scriptableInputStream;

		public ScriptableInputStream()
		{
			_scriptableInputStream = Xpcom.CreateInstance2<nsIScriptableInputStream>(Contracts.ScriptableInputStream);
		}

		public ScriptableInputStream(InputStream stream)
			:this()
		{
			Init( stream );
		}

		public int Available
		{
			get { return (int) _scriptableInputStream.Instance.Available(); }
		}

		public void Close()
		{
			_scriptableInputStream.Instance.Close();
		}

		public void Init(InputStream stream)
		{
			_scriptableInputStream.Instance.Init(stream._inputStream.Instance);
		}

		public string Read(int count)
		{
			return _scriptableInputStream.Instance.Read((uint)count);
		}

		public string ReadBytes(int count)
		{
			return nsString.Get(_scriptableInputStream.Instance.ReadBytes, (uint)count);
		}
		
		

	}
}