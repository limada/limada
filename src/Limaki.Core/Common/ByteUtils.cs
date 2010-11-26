
using System;

namespace Limaki.Common
{


	public class ByteUtils
	{
		public static byte[] BytesOfArray(Array data){
			var len = Buffer.ByteLength(data);
			var result = new byte[len];
			for(int i = 0; i<len; i++){
				result[i]=Buffer.GetByte(data,i);
			}
			return result;
		}
		
	}
}
