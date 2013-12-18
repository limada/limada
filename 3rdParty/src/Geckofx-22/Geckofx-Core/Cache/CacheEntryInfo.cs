using System;
using System.Runtime.InteropServices;
using System.Threading;
using Gecko.Interop;

namespace Gecko.Cache
{
	public class CacheEntryInfo
	{
		private InstanceWrapper< nsICacheEntryInfo> _cacheEntryInfo;
		internal CacheEntryInfo(nsICacheEntryInfo cacheEntryInfo)
		{
			_cacheEntryInfo = new InstanceWrapper<nsICacheEntryInfo>( cacheEntryInfo );
		}


		public string ClientID
		{
			get { return _cacheEntryInfo.Instance.GetClientIDAttribute(); }
		}

		public uint DataSize
		{
			get { return _cacheEntryInfo.Instance.GetDataSizeAttribute(); }
		}

		public string DeviceID
		{
			get { return _cacheEntryInfo.Instance.GetDeviceIDAttribute(); }
		}

		public uint ExpirationTimeNative
		{
			get { return _cacheEntryInfo.Instance.GetExpirationTimeAttribute(); }
		}

		public DateTime ExpirationTime
		{
			get { return Xpcom.Time.FromSecondsSinceEpoch(ExpirationTimeNative); }
		}


		public int FetchCount
		{
			get { return _cacheEntryInfo.Instance.GetFetchCountAttribute(); }
		}

		public string Key
		{
			get { return nsString.Get( _cacheEntryInfo.Instance.GetKeyAttribute ); }
		}

		public uint LastFetched
		{
			get { return _cacheEntryInfo.Instance.GetLastFetchedAttribute(); }
		}

		public uint LastModified
		{
			get { return _cacheEntryInfo.Instance.GetLastModifiedAttribute(); }
		}

		public bool IsStreamBased
		{
			get { return _cacheEntryInfo.Instance.IsStreamBased(); }
		}


	}
}