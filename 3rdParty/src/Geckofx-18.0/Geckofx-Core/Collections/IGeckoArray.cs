using System.Collections.Generic;

namespace Gecko.Collections
{
	/// <summary>
	/// External interface to enumerable array(access via index and enumerator) with fixed length
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public interface IGeckoArray<T>
		: IEnumerable<T>
	{
		T this[int index] { get; }
		int Length { get; }
	}

	/// <summary>
	/// MAY BE USED 
	///  External interface to enumerable list(access via index and enumerator + adding items)
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public interface IGeckoList<T>
		: IEnumerable<T>
	{
		T this[int index] { get; }
		int Count { get; }
		int Capacity { get; }
		void Add( T item );
	}
}