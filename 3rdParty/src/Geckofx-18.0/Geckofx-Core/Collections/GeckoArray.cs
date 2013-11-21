﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gecko.Collections
{
	/// <summary>
	/// Internal wrapper realization
	/// </summary>
	/// <typeparam name="TWrapper"></typeparam>
	/// <typeparam name="TGeckoObject"></typeparam>
	internal sealed class GeckoArray<TWrapper,TGeckoObject>
		: IGeckoArray<TWrapper>
	{
		private nsIArray _array;
		private Func<TGeckoObject, TWrapper> _translator;
		internal GeckoArray(nsIArray array, Func<TGeckoObject, TWrapper> translator)
		{
			_array = array;
			_translator = translator;
		}
		public int Length { get { return (int)_array.GetLengthAttribute(); } }

		public TWrapper this[ int index ]
		{
			get
			{
				var obj = _array.GetElementAs<TGeckoObject>(index);
				var ret = _translator( obj );
				return ret;
			}
		}

		public IEnumerator<TWrapper> GetEnumerator()
		{
			return new GeckoEnumerator<TWrapper, TGeckoObject>( _array.Enumerate(), _translator );
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
	}
}