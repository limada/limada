using Gecko.Interop;

namespace Gecko.IO
{
	public sealed class InputStreamTee
		:InputStream
	{
		private nsIInputStreamTee _inputStreamTee;

		internal InputStreamTee(nsIInputStreamTee inputStreamTee)
			:base(inputStreamTee)
		{
			_inputStreamTee = inputStreamTee;
		}


		public InputStream Source
		{
			get { return Create( _inputStreamTee.GetSourceAttribute() ); }
			set { _inputStreamTee.SetSourceAttribute( value._inputStream.Instance ); }
		}

		public OutputStream Sink
		{
			get
			{
				return _inputStreamTee.GetSinkAttribute()
									  .Wrap( x => new OutputStream( x ) );
			}
			set { _inputStreamTee.SetSinkAttribute( value._outputStream.Instance ); }
		}

		public nsIEventTarget EventTarget
		{
			get { return _inputStreamTee.GetEventTargetAttribute(); }
			set { _inputStreamTee.SetEventTargetAttribute( value ); }
		}
	}
}