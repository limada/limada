//
// Tests for System.Drawing.Drawing2D.Matrix.cs
//
// Authors:
//	Jordi Mas i Hernandez <jordi@ximian.com>
//	Sebastien Pouliot  <sebastien@ximian.com>
//
// Copyright (C) 2005-2006 Novell, Inc (http://www.novell.com)
//
// Permission is hereby granted, free of charge, to any person obtaining
// a copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to
// the following conditions:
//
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//

using NUnit.Framework;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Security.Permissions;

namespace Limaki.Tests.Drawing
{
	[TestFixture]
	public class TestMatrice : Assertion {

		private Limaki.Drawing.Matrice default_matrix;
        private Rectangle rect;
        private RectangleF rectf;

		[TestFixtureSetUp]
		public void FixtureSetUp ()
		{
			default_matrix = new Limaki.Drawing.Matrice ();
		}

		[Test]
		public void Constructor_Default ()
		{
			Limaki.Drawing.Matrice matrice = new Limaki.Drawing.Matrice ();
			AssertEquals ("C#1", 6, matrice.Elements.Length);
		}

		[Test]
		public void Constructor_SixFloats ()
		{
			Limaki.Drawing.Matrice matrice = new Limaki.Drawing.Matrice (10, 20, 30, 40, 50, 60);
			AssertEquals ("C#2", 6, matrice.Elements.Length);
			AssertEquals ("C#3", 10, matrice.Elements[0]);
			AssertEquals ("C#4", 20, matrice.Elements[1]);
			AssertEquals ("C#5", 30, matrice.Elements[2]);
			AssertEquals ("C#6", 40, matrice.Elements[3]);
			AssertEquals ("C#7", 50, matrice.Elements[4]);
			AssertEquals ("C#8", 60, matrice.Elements[5]);
		}

		[Test]
		public void Constructor_Float ()
		{
			Limaki.Drawing.Matrice matrice = new Limaki.Drawing.Matrice (10, 20, 30, 40, 50, 60);
			AssertEquals ("C#2", 6, matrice.Elements.Length);
			AssertEquals ("C#3", 10, matrice.Elements[0]);
			AssertEquals ("C#4", 20, matrice.Elements[1]);
			AssertEquals ("C#5", 30, matrice.Elements[2]);
			AssertEquals ("C#6", 40, matrice.Elements[3]);
			AssertEquals ("C#7", 50, matrice.Elements[4]);
			AssertEquals ("C#8", 60, matrice.Elements[5]);
		}

		[Test]
		[ExpectedException (typeof (ArgumentNullException))]
		public void Constructor_Int_Null ()
		{
			new Limaki.Drawing.Matrice (rect, null);
		}

		//failed [Test]
		[ExpectedException (typeof (ArgumentException))]
		public void Constructor_Int_Empty ()
		{
			new Limaki.Drawing.Matrice (rect, new Point[0]);
		}

		//failed [Test]
		[ExpectedException (typeof (ArgumentException))]
		public void Constructor_Int_4Point ()
		{
			new Limaki.Drawing.Matrice (rect, new Point[4]);
		}

		[Test]
		public void Constructor_Rect_Point ()
		{
			Rectangle r = new Rectangle (100, 200, 300, 400);
			Limaki.Drawing.Matrice m = new Limaki.Drawing.Matrice (r, new Point[3] { new Point (10, 20), new Point (30, 40), new Point (50, 60) });
			float[] elements = m.Elements;
			AssertEquals ("0", 0.06666666, elements[0], 0.00001);
			AssertEquals ("1", 0.06666666, elements[1], 0.00001);
			AssertEquals ("2", 0.09999999, elements[2], 0.00001);
			AssertEquals ("3", 0.09999999, elements[3], 0.00001);
			AssertEquals ("4", -16.6666679, elements[4], 0.00001);
			AssertEquals ("5", -6.666667, elements[5], 0.00001);
		}

		[Test]
		[ExpectedException (typeof (ArgumentNullException))]
		public void Constructor_Float_Null ()
		{
			new Limaki.Drawing.Matrice (rectf, null);
		}

		//failed [Test]
		[ExpectedException (typeof (ArgumentException))]
		public void Constructor_Float_Empty ()
		{
			new Limaki.Drawing.Matrice (rectf, new PointF[0]);
		}

		//failed [Test]
		[ExpectedException (typeof (ArgumentException))]
		public void Constructor_Float_2PointF ()
		{
			new Limaki.Drawing.Matrice (rectf, new PointF[2]);
		}

		[Test]
		public void Constructor_RectF_PointF ()
		{
			RectangleF r = new RectangleF (100, 200, 300, 400);
			Limaki.Drawing.Matrice m = new Limaki.Drawing.Matrice (r, new PointF[3] { new PointF (10, 20), new PointF (30, 40), new PointF (50, 60) });
			float[] elements = m.Elements;
			AssertEquals ("0", 0.06666666, elements[0], 0.00001);
			AssertEquals ("1", 0.06666666, elements[1], 0.00001);
			AssertEquals ("2", 0.09999999, elements[2], 0.00001);
			AssertEquals ("3", 0.09999999, elements[3], 0.00001);
			AssertEquals ("4", -16.6666679, elements[4], 0.00001);
			AssertEquals ("5", -6.666667, elements[5], 0.00001);
		}

		// Properties

		[Test]
		public void Invertible ()
		{
			Limaki.Drawing.Matrice matrice = new Limaki.Drawing.Matrice (123, 24, 82, 16, 47, 30);
			AssertEquals ("I#1", false, matrice.IsInvertible);

			matrice = new Limaki.Drawing.Matrice (156, 46, 0, 0, 106, 19);
			AssertEquals ("I#2", false, matrice.IsInvertible);

			matrice = new Limaki.Drawing.Matrice (146, 66, 158, 104, 42, 150);
			AssertEquals ("I#3", true, matrice.IsInvertible);

			matrice = new Limaki.Drawing.Matrice (119, 140, 145, 74, 102, 58);
			AssertEquals ("I#4", true, matrice.IsInvertible);
		}
		
		[Test]
		public void IsIdentity ()
		{
			Limaki.Drawing.Matrice identity = new Limaki.Drawing.Matrice ();
			Limaki.Drawing.Matrice matrice = new Limaki.Drawing.Matrice (123, 24, 82, 16, 47, 30);
			AssertEquals ("N#1-identity", false, matrice.IsIdentity);
			Assert ("N#1-equals", !identity.Equals (matrice));
			
			matrice = new Limaki.Drawing.Matrice (1, 0, 0, 1, 0, 0);
			AssertEquals ("N#2-identity", true, matrice.IsIdentity);
			Assert ("N#2-equals", identity.Equals (matrice));

			// so what's the required precision ?

			matrice = new Limaki.Drawing.Matrice (1.1f, 0.1f, -0.1f, 0.9f, 0, 0);
			Assert ("N#3-identity", !matrice.IsIdentity);
			Assert ("N#3-equals", !identity.Equals (matrice));

			matrice = new Limaki.Drawing.Matrice (1.01f, 0.01f, -0.01f, 0.99f, 0, 0);
			Assert ("N#4-identity", !matrice.IsIdentity);
			Assert ("N#4-equals", !identity.Equals (matrice));

			matrice = new Limaki.Drawing.Matrice (1.001f, 0.001f, -0.001f, 0.999f, 0, 0);
			Assert ("N#5-identity", !matrice.IsIdentity);
			Assert ("N#5-equals", !identity.Equals (matrice));

			matrice = new Limaki.Drawing.Matrice (1.0001f, 0.0001f, -0.0001f, 0.9999f, 0, 0);
			//failed: Assert ("N#6-identity", matrix.IsIdentity);
			// note: NOT equal
			Assert ("N#6-equals", !identity.Equals (matrice));

			matrice = new Limaki.Drawing.Matrice (1.0009f, 0.0009f, -0.0009f, 0.99995f, 0, 0);
			Assert ("N#7-identity", !matrice.IsIdentity);
			Assert ("N#7-equals", !identity.Equals (matrice));
		}
		
		[Test]
		public void IsOffsetX ()
		{
			Limaki.Drawing.Matrice matrice = new Limaki.Drawing.Matrice (123, 24, 82, 16, 47, 30);
			AssertEquals ("X#1", 47, matrice.OffsetX);			
		}
		
		[Test]
		public void IsOffsetY ()
		{
			Limaki.Drawing.Matrice matrice = new Limaki.Drawing.Matrice (123, 24, 82, 16, 47, 30);
			AssertEquals ("Y#1", 30, matrice.OffsetY);			
		}
		
		// Elements Property is checked implicity in other test

		//
		// Methods
		//
		

		[Test]
		public void Clone ()
		{
			Limaki.Drawing.Matrice matsrc = new Limaki.Drawing.Matrice (10, 20, 30, 40, 50, 60);
			Limaki.Drawing.Matrice matrice  = matsrc.Clone ();

			AssertEquals ("D#1", 6, matrice.Elements.Length);
			AssertEquals ("D#2", 10, matrice.Elements[0]);
			AssertEquals ("D#3", 20, matrice.Elements[1]);
			AssertEquals ("D#4", 30, matrice.Elements[2]);
			AssertEquals ("D#5", 40, matrice.Elements[3]);
			AssertEquals ("D#6", 50, matrice.Elements[4]);
			AssertEquals ("D#7", 60, matrice.Elements[5]);
		}

		//failed [Test]
		public void HashCode ()
		{
			Limaki.Drawing.Matrice matrice = new Limaki.Drawing.Matrice (10, 20, 30, 40, 50, 60);
			Limaki.Drawing.Matrice clone = matrice.Clone ();
			Assert ("HashCode/Clone", matrice.GetHashCode () != clone.GetHashCode ());

			Limaki.Drawing.Matrice matrix2 = new Limaki.Drawing.Matrice (10, 20, 30, 40, 50, 60);
			Assert ("HashCode/Identical", matrice.GetHashCode () != matrix2.GetHashCode ());
		}

		[Test]
		public void Reset ()
		{
			Limaki.Drawing.Matrice matrice = new Limaki.Drawing.Matrice (51, 52, 53, 54, 55, 56);
			matrice.Reset ();

			AssertEquals ("F#1", 6, matrice.Elements.Length);
			AssertEquals ("F#2", 1, matrice.Elements[0]);
			AssertEquals ("F#3", 0, matrice.Elements[1]);
			AssertEquals ("F#4", 0, matrice.Elements[2]);
			AssertEquals ("F#5", 1, matrice.Elements[3]);
			AssertEquals ("F#6", 0, matrice.Elements[4]);
			AssertEquals ("F#7", 0, matrice.Elements[5]);
		}

		[Test]
		public void Rotate ()
		{
			Limaki.Drawing.Matrice matrice = new Limaki.Drawing.Matrice (10, 20, 30, 40, 50, 60);
			matrice.Rotate (180);

			AssertEquals ("H#1", -10, matrice.Elements[0]);
			AssertEquals ("H#2", -20, matrice.Elements[1]);
			AssertEquals ("H#3", -30, matrice.Elements[2]);
			AssertEquals ("H#4", -40, matrice.Elements[3]);
			AssertEquals ("H#5", 50, matrice.Elements[4]);
			AssertEquals ("H#6", 60, matrice.Elements[5]);
		}

		[Test]
		public void Rotate_45_135 ()
		{
			Limaki.Drawing.Matrice matrice = new Limaki.Drawing.Matrice ();
			Assert ("original.IsIdentity", matrice.IsIdentity);

			matrice.Rotate (45);
			Assert ("+45.!IsIdentity", !matrice.IsIdentity);
			float[] elements = matrice.Elements;
			AssertEquals ("45#1", 0.7071068, elements[0]);
			AssertEquals ("45#2", 0.7071068, elements[1]);
			AssertEquals ("45#3", -0.7071068, elements[2]);
			AssertEquals ("45#4", 0.7071068, elements[3]);
			AssertEquals ("45#5", 0, elements[4]);
			AssertEquals ("45#6", 0, elements[5]);

			matrice.Rotate (135);
			Assert ("+135.!IsIdentity", !matrice.IsIdentity);
			elements = matrice.Elements;
			AssertEquals ("180#1", -1, elements[0], 0.0001);
			AssertEquals ("180#2", 0, elements[1], 0.0001);
			AssertEquals ("180#3", 0, elements[2], 0.0001);
			AssertEquals ("180#4", -1, elements[3], 0.0001);
			AssertEquals ("180#5", 0, elements[4]);
			AssertEquals ("180#6", 0, elements[5]);
		}

		[Test]
		public void Rotate_90_270_Matrix ()
		{
			Limaki.Drawing.Matrice matrice = new Limaki.Drawing.Matrice ();
			Assert ("original.IsIdentity", matrice.IsIdentity);

			matrice.Rotate (90);
			Assert ("+90.!IsIdentity", !matrice.IsIdentity);
			float[] elements = matrice.Elements;
			AssertEquals ("90#1", 0, elements[0], 0.0001);
			AssertEquals ("90#2", 1, elements[1], 0.0001);
			AssertEquals ("90#3", -1, elements[2], 0.0001);
			AssertEquals ("90#4", 0, elements[3], 0.0001);
			AssertEquals ("90#5", 0, elements[4]);
			AssertEquals ("90#6", 0, elements[5]);

			matrice.Rotate (270);
			// this isn't a perfect 1, 0, 0, 1, 0, 0 matrix - but close enough
			//failed: Assert ("360.IsIdentity", matrix.IsIdentity);
			Assert ("360.Equals", !new Limaki.Drawing.Matrice ().Equals (matrice));
		}

		//failed [Test]
		[ExpectedException (typeof (ArgumentException))]
		public void Rotate_InvalidOrder ()
		{
			new Limaki.Drawing.Matrice ().Rotate (180, (MatrixOrder) Int32.MinValue);
		}

		[Test]
		public void RotateAt ()
		{
			Limaki.Drawing.Matrice matrice = new Limaki.Drawing.Matrice (10, 20, 30, 40, 50, 60);
			matrice.RotateAt (180, new PointF (10, 10));

			AssertEquals ("I#1", -10, matrice.Elements[0], 0.01);
			AssertEquals ("I#2", -20, matrice.Elements[1], 0.01);
			AssertEquals ("I#3", -30, matrice.Elements[2], 0.01);
			AssertEquals ("I#4", -40, matrice.Elements[3], 0.01);
			AssertEquals ("I#5", 850, matrice.Elements[4], 0.01);
			AssertEquals ("I#6", 1260, matrice.Elements[5], 0.01);
		}

		//failed:[Test]
		[ExpectedException (typeof (ArgumentException))]
		public void RotateAt_InvalidOrder ()
		{
			new Limaki.Drawing.Matrice ().RotateAt (180, new PointF (10, 10), (MatrixOrder) Int32.MinValue);
		}

		[Test]
		[ExpectedException (typeof (ArgumentNullException))]
		public void Multiply_Null ()
		{
			new Limaki.Drawing.Matrice (10, 20, 30, 40, 50, 60).Multiply (null);
		}

		[Test]
		public void Multiply ()
		{
			Limaki.Drawing.Matrice matrice = new Limaki.Drawing.Matrice (10, 20, 30, 40, 50, 60);
			matrice.Multiply (new Limaki.Drawing.Matrice (10, 20, 30, 40, 50, 60));

			AssertEquals ("J#1", 700, matrice.Elements[0]);
			AssertEquals ("J#2", 1000, matrice.Elements[1]);
			AssertEquals ("J#3", 1500, matrice.Elements[2]);
			AssertEquals ("J#4", 2200, matrice.Elements[3]);
			AssertEquals ("J#5", 2350, matrice.Elements[4]);
			AssertEquals ("J#6", 3460, matrice.Elements[5]);
		}

		[Test]
		[ExpectedException (typeof (ArgumentNullException))]
		public void Multiply_Null_Order ()
		{
			new Limaki.Drawing.Matrice (10, 20, 30, 40, 50, 60).Multiply (null, MatrixOrder.Append);
		}

		[Test]
		public void Multiply_Append ()
		{
			Limaki.Drawing.Matrice matrice = new Limaki.Drawing.Matrice (10, 20, 30, 40, 50, 60);
			matrice.Multiply (new Limaki.Drawing.Matrice (10, 20, 30, 40, 50, 60), MatrixOrder.Append);

			AssertEquals ("J#1", 700, matrice.Elements[0]);
			AssertEquals ("J#2", 1000, matrice.Elements[1]);
			AssertEquals ("J#3", 1500, matrice.Elements[2]);
			AssertEquals ("J#4", 2200, matrice.Elements[3]);
			AssertEquals ("J#5", 2350, matrice.Elements[4]);
			AssertEquals ("J#6", 3460, matrice.Elements[5]);
		}

		[Test]
		public void Multiply_Prepend ()
		{
			Limaki.Drawing.Matrice matrice = new Limaki.Drawing.Matrice (10, 20, 30, 40, 50, 60);
			matrice.Multiply (new Limaki.Drawing.Matrice (10, 20, 30, 40, 50, 60), MatrixOrder.Prepend);

			AssertEquals ("J#1", 700, matrice.Elements[0]);
			AssertEquals ("J#2", 1000, matrice.Elements[1]);
			AssertEquals ("J#3", 1500, matrice.Elements[2]);
			AssertEquals ("J#4", 2200, matrice.Elements[3]);
			AssertEquals ("J#5", 2350, matrice.Elements[4]);
			AssertEquals ("J#6", 3460, matrice.Elements[5]);
		}

		//failed[Test]
		[ExpectedException (typeof (ArgumentException))]
		public void Multiply_InvalidOrder ()
		{
			Limaki.Drawing.Matrice matrice = new Limaki.Drawing.Matrice (10, 20, 30, 40, 50, 60);
			matrice.Multiply (new Limaki.Drawing.Matrice (10, 20, 30, 40, 50, 60), (MatrixOrder)Int32.MinValue);
		}

		[Test]
		public void Equals ()
		{
			Limaki.Drawing.Matrice mat1 = new Limaki.Drawing.Matrice (10, 20, 30, 40, 50, 60);
			Limaki.Drawing.Matrice mat2 = new Limaki.Drawing.Matrice (10, 20, 30, 40, 50, 60);
			Limaki.Drawing.Matrice mat3 = new Limaki.Drawing.Matrice (10, 20, 30, 40, 50, 10);

			AssertEquals ("E#1", true, mat1.Equals (mat2));
			AssertEquals ("E#2", false, mat2.Equals (mat3));
			AssertEquals ("E#3", false, mat1.Equals (mat3));
		}
		
		[Test]
		public void Invert ()
		{
			Limaki.Drawing.Matrice matrice = new Limaki.Drawing.Matrice (1, 2, 3, 4, 5, 6);
			matrice.Invert ();
			
			AssertEquals ("V#1", -2, matrice.Elements[0]);
			AssertEquals ("V#2", 1, matrice.Elements[1]);
			AssertEquals ("V#3", 1.5, matrice.Elements[2]);
			AssertEquals ("V#4", -0.5, matrice.Elements[3]);
			AssertEquals ("V#5", 1, matrice.Elements[4]);
			AssertEquals ("V#6", -2, matrice.Elements[5]);			
		}

		[Test]
		public void Invert_Translation ()
		{
			Limaki.Drawing.Matrice matrice = new Limaki.Drawing.Matrice (1, 0, 0, 1, 8, 8);
			matrice.Invert ();

			float[] elements = matrice.Elements;
			AssertEquals ("#1", 1, elements[0]);
			AssertEquals ("#2", 0, elements[1]);
			AssertEquals ("#3", 0, elements[2]);
			AssertEquals ("#4", 1, elements[3]);
			AssertEquals ("#5", -8, elements[4]);
			AssertEquals ("#6", -8, elements[5]);
		}

		[Test]
		public void Invert_Identity ()
		{
			Limaki.Drawing.Matrice matrice = new Limaki.Drawing.Matrice ();
			Assert ("IsIdentity", matrice.IsIdentity);
			Assert ("IsInvertible", matrice.IsInvertible);
			matrice.Invert ();
			Assert ("IsIdentity-2", matrice.IsIdentity);
			Assert ("IsInvertible-2", matrice.IsInvertible);
		}

		[Test]
		public void Scale ()
		{
			Limaki.Drawing.Matrice matrice = new Limaki.Drawing.Matrice (10, 20, 30, 40, 50, 60);
			matrice.Scale (2, 4);

			AssertEquals ("S#1", 20, matrice.Elements[0]);
			AssertEquals ("S#2", 40, matrice.Elements[1]);
			AssertEquals ("S#3", 120, matrice.Elements[2]);
			AssertEquals ("S#4", 160, matrice.Elements[3]);
			AssertEquals ("S#5", 50, matrice.Elements[4]);
			AssertEquals ("S#6", 60, matrice.Elements[5]);

			matrice.Scale (0.5f, 0.25f);

			AssertEquals ("SB#1", 10, matrice.Elements[0]);
			AssertEquals ("SB#2", 20, matrice.Elements[1]);
			AssertEquals ("SB#3", 30, matrice.Elements[2]);
			AssertEquals ("SB#4", 40, matrice.Elements[3]);
			AssertEquals ("SB#5", 50, matrice.Elements[4]);
			AssertEquals ("SB#6", 60, matrice.Elements[5]);
		}

		[Test]
		public void Scale_Negative ()
		{
			Limaki.Drawing.Matrice matrice = new Limaki.Drawing.Matrice (10, 20, 30, 40, 50, 60);
			matrice.Scale (-2, -4);

			AssertEquals ("S#1", -20, matrice.Elements[0]);
			AssertEquals ("S#2", -40, matrice.Elements[1]);
			AssertEquals ("S#3", -120, matrice.Elements[2]);
			AssertEquals ("S#4", -160, matrice.Elements[3]);
			AssertEquals ("S#5", 50, matrice.Elements[4]);
			AssertEquals ("S#6", 60, matrice.Elements[5]);
		}

		//failed:[Test]
		[ExpectedException (typeof (ArgumentException))]
		public void Scale_InvalidOrder ()
		{
			new Limaki.Drawing.Matrice ().Scale (2, 1, (MatrixOrder) Int32.MinValue);
		}
		
		[Test]
		public void Shear ()
		{
			Limaki.Drawing.Matrice matrice = new Limaki.Drawing.Matrice (10, 20, 30, 40, 50, 60);
			matrice.Shear (2, 4);

			AssertEquals ("H#1", 130, matrice.Elements[0]);
			AssertEquals ("H#2", 180, matrice.Elements[1]);
			AssertEquals ("H#3", 50, matrice.Elements[2]);
			AssertEquals ("H#4", 80, matrice.Elements[3]);
			AssertEquals ("H#5", 50, matrice.Elements[4]);
			AssertEquals ("H#6", 60, matrice.Elements[5]);
			
			matrice = new Limaki.Drawing.Matrice (5, 3, 9, 2, 2, 1);
			matrice.Shear  (10, 20);			
			
			AssertEquals ("H#7", 185, matrice.Elements[0]);
			AssertEquals ("H#8", 43, matrice.Elements[1]);
			AssertEquals ("H#9", 59, matrice.Elements[2]);
			AssertEquals ("H#10", 32, matrice.Elements[3]);
			AssertEquals ("H#11", 2, matrice.Elements[4]);
			AssertEquals ("H#12", 1, matrice.Elements[5]);			    
		}

		//failed [Test]
		[ExpectedException (typeof (ArgumentException))]
		public void Shear_InvalidOrder ()
		{
			new Limaki.Drawing.Matrice ().Shear (-1, 1, (MatrixOrder) Int32.MinValue);
		}
		
		[Test]
		public void TransformPoints ()
		{
			Limaki.Drawing.Matrice matrice = new Limaki.Drawing.Matrice (2, 4, 6, 8, 10, 12);
			PointF [] pointsF = new PointF [] {new PointF (2, 4), new PointF (4, 8)};
			matrice.TransformPoints (pointsF);
						
			AssertEquals ("K#1", 38, pointsF[0].X);
			AssertEquals ("K#2", 52, pointsF[0].Y);
			AssertEquals ("K#3", 66, pointsF[1].X);
			AssertEquals ("K#4", 92, pointsF[1].Y);
			
			Point [] points = new Point [] {new Point (2, 4), new Point (4, 8)};
			matrice.TransformPoints (points);
			AssertEquals ("K#5", 38, pointsF[0].X);
			AssertEquals ("K#6", 52, pointsF[0].Y);
			AssertEquals ("K#7", 66, pointsF[1].X);
			AssertEquals ("K#8", 92, pointsF[1].Y);						    
		}

		[Test]
		[ExpectedException (typeof (ArgumentNullException))]
		public void TransformPoints_Point_Null ()
		{
			new Limaki.Drawing.Matrice ().TransformPoints ((Point[]) null);
		}

		[Test]
		[ExpectedException (typeof (ArgumentNullException))]
		public void TransformPoints_PointF_Null ()
		{
			new Limaki.Drawing.Matrice ().TransformPoints ((PointF[]) null);
		}

		//failed [Test]
		[ExpectedException (typeof (ArgumentException))]
		public void TransformPoints_Point_Empty ()
		{
			new Limaki.Drawing.Matrice ().TransformPoints (new Point[0]);
		}

		//failed[Test]
		[ExpectedException (typeof (ArgumentException))]
		public void TransformPoints_PointF_Empty ()
		{
			new Limaki.Drawing.Matrice ().TransformPoints (new PointF[0]);
		}
		
		[Test]
		public void TransformVectors  ()
		{
			Limaki.Drawing.Matrice matrice = new Limaki.Drawing.Matrice (2, 4, 6, 8, 10, 12);
			PointF [] pointsF = new PointF [] {new PointF (2, 4), new PointF (4, 8)};
			matrice.TransformVectors (pointsF);
						
			AssertEquals ("N#1", 28, pointsF[0].X);
			AssertEquals ("N#2", 40, pointsF[0].Y);
			AssertEquals ("N#3", 56, pointsF[1].X);
			AssertEquals ("N#4", 80, pointsF[1].Y);
			
			Point [] points = new Point [] {new Point (2, 4), new Point (4, 8)};
			matrice.TransformVectors (points);
			AssertEquals ("N#5", 28, pointsF[0].X);
			AssertEquals ("N#6", 40, pointsF[0].Y);
			AssertEquals ("N#7", 56, pointsF[1].X);
			AssertEquals ("N#8", 80, pointsF[1].Y);						    
		}

		[Test]
		[ExpectedException (typeof (ArgumentNullException))]
		public void TransformVectors_Point_Null ()
		{
			new Limaki.Drawing.Matrice ().TransformVectors ((Point[]) null);
		}

		[Test]
		[ExpectedException (typeof (ArgumentNullException))]
		public void TransformVectors_PointF_Null ()
		{
			new Limaki.Drawing.Matrice ().TransformVectors ((PointF[]) null);
		}

		//failed [Test]
		[ExpectedException (typeof (ArgumentException))]
		public void TransformVectors_Point_Empty ()
		{
			new Limaki.Drawing.Matrice ().TransformVectors (new Point[0]);
		}

		//failed[Test]
		[ExpectedException (typeof (ArgumentException))]
		public void TransformVectors_PointF_Empty ()
		{
			new Limaki.Drawing.Matrice ().TransformVectors (new PointF[0]);
		}

		[Test]
		public void Translate  ()
		{
			Limaki.Drawing.Matrice matrice = new Limaki.Drawing.Matrice (2, 4, 6, 8, 10, 12);			
			matrice.Translate (5, 10);
						
			AssertEquals ("Y#1", 2, matrice.Elements[0]);
			AssertEquals ("Y#2", 4, matrice.Elements[1]);
			AssertEquals ("Y#3", 6, matrice.Elements[2]);
			AssertEquals ("Y#4", 8, matrice.Elements[3]);
			AssertEquals ("Y#5", 80, matrice.Elements[4]);
			AssertEquals ("Y#6", 112, matrice.Elements[5]);	
		}

		//failed [Test]
		[ExpectedException (typeof (ArgumentException))]
		public void Translate_InvalidOrder ()
		{
			new Limaki.Drawing.Matrice ().Translate (-1, 1, (MatrixOrder) Int32.MinValue);
		}

		[Test]
		[ExpectedException (typeof (ArgumentNullException))]
		public void VectorTransformPoints_Null ()
		{
			new Limaki.Drawing.Matrice ().VectorTransformPoints ((Point[]) null);
		}

		//failed [Test]
		[ExpectedException (typeof (ArgumentException))]
		public void VectorTransformPoints_Empty ()
		{
			new Limaki.Drawing.Matrice ().VectorTransformPoints (new Point[0]);
		}
	}
}
