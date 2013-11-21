using System;
using Gecko.Interop;

namespace Gecko.Net
{
	public class Channel
		:Request
	{
		private ComPtr<nsIChannel> _channel; 

		protected Channel(nsIChannel channel)
			:base(channel)
		{
			_channel = new ComPtr<nsIChannel>(channel);
		}

		public static Channel WrapChannel(nsIChannel channel)
		{
			if ( channel is nsIHttpChannel )
			{
				return new HttpChannel( ( nsIHttpChannel ) channel );
			}
			if ( channel is nsIJARChannel )
			{
				
			}
			if ( channel is nsIViewSourceChannel )
			{
				
			}
			if ( channel is nsIWyciwygChannel )
			{
				
			}
			return new Channel( channel );
		}

		public Uri OriginalUri
		{
			//TODO - check for memory leaks
			get
			{
				return _channel.Instance.GetOriginalURIAttribute().ToUri();
			}
			//TODO - check for memory leaks
			set { _channel.Instance.SetOriginalURIAttribute(IOService.CreateNsIUri(value.ToString())); }
		}

		public Uri Uri
		{
			get { return Xpcom.ToUri(_channel.Instance.GetURIAttribute()); }
		}

		public nsISupports Owner
		{
			get { return _channel.Instance.GetOwnerAttribute(); }
			set { _channel.Instance.SetOwnerAttribute( value ); }
		}

		public nsIInterfaceRequestor NotificationCallbacks
		{
			get { return _channel.Instance.GetNotificationCallbacksAttribute(); }
			set { _channel.Instance.SetNotificationCallbacksAttribute(value); }
		}

		public nsISupports SecurityInfo
		{
			get { return _channel.Instance.GetSecurityInfoAttribute(); }
		}

		public string ContentType
		{
			get { return nsString.Get(_channel.Instance.GetContentTypeAttribute); }
			set { nsString.Set(_channel.Instance.SetContentTypeAttribute, value); }
		}

		public string ContentCharset
		{
			get { return nsString.Get(_channel.Instance.GetContentCharsetAttribute); }
			set { nsString.Set(_channel.Instance.SetContentCharsetAttribute, value); }
		}

		public int ContentLength
		{
			get { return _channel.Instance.GetContentLengthAttribute(); }
			set { _channel.Instance.SetContentLengthAttribute(value); }
		}

		public IO.InputStream Open()
		{
			return IO.InputStream.Create(_channel.Instance.Open());
		}

		//void AsyncOpen([MarshalAs(UnmanagedType.Interface)] nsIStreamListener aListener, [MarshalAs(UnmanagedType.Interface)] nsISupports aContext);

		public uint ContentDisposition
		{
			get { return _channel.Instance.GetContentDispositionAttribute(); }
		}

		public string ContentDispositionFilename
		{
			get { return nsString.Get(_channel.Instance.GetContentDispositionFilenameAttribute); }
		}

		public string ContentDispositionHeader
		{
			get { return nsString.Get(_channel.Instance.GetContentDispositionHeaderAttribute); }
		}
	}
}