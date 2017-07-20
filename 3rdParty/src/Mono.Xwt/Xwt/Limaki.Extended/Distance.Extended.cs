using System;
using System.ComponentModel;
using System.Globalization;

namespace Xwt {
	
	public partial struct Distance 
	{
		public static Distance operator * (Distance d1, Distance d2) => new Distance (d1.dx * d2.dx, d1.dy * d2.dy);

		public static Distance operator * (Distance d1, double value) => new Distance (d1.dx * value, d1.dy * value);

		public static Distance operator / (Distance d1, double value) => new Distance (d1.dx / value, d1.dy / value);

		public static Distance operator + (Distance d1, double value) => new Distance (d1.dx + value, d1.dy + value);

		public static Distance operator - (Distance d1, double value) => new Distance (d1.dx - value, d1.dy - value);

	}
}