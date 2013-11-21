using Gecko.Net;

namespace Gecko
{
	/// <summary>
	/// Represents a response to a Gecko web request.
	/// TODO -- Think about removing GeckoResponse and use classes Request/Channel/HttpChannel
	/// Channel is base class for HttpChannel
	/// Request is base class fo Channel
	/// 
	/// </summary>
	public class GeckoResponse
	{
		private Request _request;
		private Channel _channel;
		private HttpChannel _httpChannel;

		internal GeckoResponse(nsIRequest request)
		{
			// we use only one wrapper
			_request = Request.WrapRequest( request );
			if ( _request is Channel )
			{
				_channel = ( Channel ) _request;
				if ( _channel is HttpChannel )
				{
					_httpChannel = ( HttpChannel ) _channel;
				}
			}
		}

		
		/// <summary>
		/// Gets the MIME type of the channel's content if available.
		/// </summary>
		public string ContentType
		{
			get
			{
				return _channel != null ? _channel.ContentType : null;
			}
		}

		/// <summary>
		/// Gets the character set of the channel's content if available and if applicable.
		/// </summary>
		public string ContentCharset
		{
			get { return _channel != null ? _channel.ContentCharset : null; }
		}

		/// <summary>
		/// Gets the length of the data associated with the channel if available. A value of -1 indicates that the content length is unknown.
		/// </summary>
		public int ContentLength
		{
			get { return _channel != null ? _channel.ContentLength : -1; }
		}
		
		/// <summary>
		/// Gets the HTTP request method.
		/// </summary>
		public string HttpRequestMethod
		{
			get
			{
				return ( _httpChannel != null ) ? _httpChannel.RequestMethod : null;
			}
		}
		
		/// <summary>
		/// Returns true if the HTTP response code indicates success. This value will be true even when processing a 404 response because a 404 response
		/// may include a message body that (in some cases) should be shown to the user.
		/// </summary>
		public bool HttpRequestSucceeded
		{
			get
			{
				return (_httpChannel != null) ? _httpChannel.RequestSucceeded : false;
			}
		}
		
		/// <summary>
		/// Gets the HTTP response code (a value of 200 indicates success).
		/// </summary>
		public int HttpResponseStatus
		{
			get
			{
				return (_httpChannel != null) ? (int)_httpChannel.ResponseStatus : 0;
			}
		}
		
		/// <summary>
		/// Gets the HTTP response status text.
		/// </summary>
		public string HttpResponseStatusText
		{
			get
			{
				return ( _httpChannel != null ) ? _httpChannel.ResponseStatusText : null;
			}
		}
	}
}
