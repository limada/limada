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
// Copyright (c) 2007 Novell, Inc. (http://www.novell.com)
//
// Authors:
// Chris Toshok (toshok@ximian.com)
//

using System;
using System.Globalization;
using NUnit.Framework;
using Xwt;

namespace Xwt.Test{
    [TestFixture]
    public class RectTest {
        [Test]
        public void Ctor_Accessor() {
            Rectangle r;

            r = new Rectangle(10, 15, 20, 30);
            Assert.AreEqual(10, r.X);
            Assert.AreEqual(15, r.Y);
            Assert.AreEqual(20, r.Width);
            Assert.AreEqual(30, r.Height);
            Assert.AreEqual(10, r.Left);
            Assert.AreEqual(15, r.Top);
            Assert.AreEqual(30, r.Right);
            Assert.AreEqual(45, r.Bottom);
            Assert.AreEqual(new Point(10, 15), r.Location);
            Assert.AreEqual(new Size(20, 30), r.Size);

            r = new Rectangle(new Point(10, 15), new Size(20, 30));
            Assert.AreEqual(10, r.X);
            Assert.AreEqual(15, r.Y);
            Assert.AreEqual(20, r.Width);
            Assert.AreEqual(30, r.Height);
            Assert.AreEqual(10, r.Left);
            Assert.AreEqual(15, r.Top);
            Assert.AreEqual(30, r.Right);
            Assert.AreEqual(45, r.Bottom);
            Assert.AreEqual(new Point(10, 15), r.Location);
            Assert.AreEqual(new Size(20, 30), r.Size);
        }

        [Test]
        public void Zero() {
            Rectangle r = Rectangle.Zero;
            Assert.AreEqual(0, r.X);
            Assert.AreEqual(0, r.Y);
            Assert.AreEqual(0, r.Width);
            Assert.AreEqual(0, r.Height);
        }

        [Test]
        //[ExpectedException(typeof(ArgumentException))]
        public void Ctor_NegativeWidth() {
            new Rectangle(10, 10, -10, 10);
        }

        [Test]
        //[ExpectedException(typeof(ArgumentException))]
        public void Ctor_NegativeHeight() {
            new Rectangle(10, 10, 10, -10);
        }
        [Test]
        //[ExpectedException(typeof(InvalidOperationException))]
        public void ModifyEmpty_x() {
            Rectangle r = Rectangle.Zero;
            r.X = 5;
        }

        [Test]
        //[ExpectedException(typeof(InvalidOperationException))]
        public void ModifyEmpty_y() {
            Rectangle r = Rectangle.Zero;
            r.Y = 5;
        }

        [Test]
        //[ExpectedException(typeof(InvalidOperationException))]
        public void ModifyEmpty_width() {
            Rectangle r = Rectangle.Zero;
            r.Width = 5;
        }

        [Test]
        //[ExpectedException(typeof(InvalidOperationException))]
        public void ModifyEmpty_height() {
            Rectangle r = Rectangle.Zero;
            r.Height = 5;
        }

        [Test]
        //[ExpectedException(typeof(InvalidOperationException))]
        public void ModifyEmpty_negative_width() {
            Rectangle r = Rectangle.Zero;
            r.Width = -5;
        }

        [Test]
        //[ExpectedException(typeof(InvalidOperationException))]
        public void ModifyEmpty_negative_height() {
            Rectangle r = Rectangle.Zero;
            r.Height = -5;
        }


        [Test]
        //[ExpectedException(typeof(ArgumentException))]
        public void Modify_negative_width() {
            Rectangle r = new Rectangle(0, 0, 10, 10);
            r.Width = -5;
        }

        [Test]
        //[ExpectedException(typeof(ArgumentException))]
        public void Modify_negative_height() {
            Rectangle r = new Rectangle(0, 0, 10, 10);
            r.Height = -5;
        }

        [Test]
        public void Empty_Size() {
            Assert.AreEqual(Size.Zero, Rectangle.Zero.Size);
        }

        [Test]
        public void IsEmpty() {
            Assert.IsTrue(Rectangle.Zero.IsEmpty);
            Assert.IsFalse((new Rectangle(5, 5, 5, 5)).IsEmpty);
        }

        [Test]
        public void Location() {
            Rectangle r = new Rectangle(0, 0, 0, 0);

            r.Location = new Point(10, 15);
            Assert.AreEqual(10, r.X);
            Assert.AreEqual(15, r.Y);
        }

        [Test]
        public void RectSize() {
            Rectangle r = new Rectangle(0, 0, 5, 5);

            r.Size = new Size(10, 15);
            Assert.AreEqual(10, r.Width);
            Assert.AreEqual(15, r.Height);
        }

        [Test]
        public void Equals() {
            Rectangle r1 = new Rectangle(1, 2, 3, 4);
            Rectangle r2 = r1;

            Assert.IsTrue(r1.Equals(r1));

            r2.X = 0;
            Assert.IsFalse(r1.Equals(r2));
            r2.X = r1.X;

            r2.Y = 0;
            Assert.IsFalse(r1.Equals(r2));
            r2.Y = r1.Y;

            r2.Width = 0;
            Assert.IsFalse(r1.Equals(r2));
            r2.Width = r1.Width;

            r2.Height = 0;
            Assert.IsFalse(r1.Equals(r2));
            r2.Height = r1.Height;

            Assert.IsFalse(r1.Equals(new object()));

            r1 = Rectangle.Zero;
            r2 = Rectangle.Zero;

            Assert.AreEqual(true, r1.Equals(r2));
            Assert.AreEqual(true, r2.Equals(r1));
        }

        [Test]
        public void ContainsRect() {
            Rectangle r = new Rectangle(0, 0, 50, 50);

            // fully contained
            Assert.IsTrue(r.Contains(new Rectangle(10, 10, 10, 10)));

            // crosses top side
            Assert.IsFalse(r.Contains(new Rectangle(5, -5, 10, 10)));

            // crosses right side
            Assert.IsFalse(r.Contains(new Rectangle(5, 5, 50, 10)));

            // crosses bottom side
            Assert.IsFalse(r.Contains(new Rectangle(5, 5, 10, 50)));

            // crosses left side
            Assert.IsFalse(r.Contains(new Rectangle(-5, 5, 10, 10)));

            // completely outside (top)
            Assert.IsFalse(r.Contains(new Rectangle(5, -5, 1, 1)));

            // completely outside (right)
            Assert.IsFalse(r.Contains(new Rectangle(75, 5, 1, 1)));

            // completely outside (bottom)
            Assert.IsFalse(r.Contains(new Rectangle(5, 75, 1, 1)));

            // completely outside (left)
            Assert.IsFalse(r.Contains(new Rectangle(-25, 5, 1, 1)));
        }

        [Test]
        public void ContainsDoubles() {
            Rectangle r = new Rectangle(0, 0, 50, 50);

            Assert.IsTrue(r.Contains(10, 10));
            Assert.IsFalse(r.Contains(-5, -5));
        }

        [Test]
        public void ContainsPoint() {
            Rectangle r = new Rectangle(0, 0, 50, 50);

            Assert.IsTrue(r.Contains(new Point(10, 10)));
            Assert.IsFalse(r.Contains(new Point(-5, -5)));
        }

        [Test]
        public void Inflate() {
            Rectangle r;
            //r = Rectangle.Inflate(new Rectangle(0, 0, 20, 20), 10, 15);
            //Assert.AreEqual(new Rectangle(-10, -15, 40, 50), r);

            //r = Rectangle.Inflate(new Rectangle(0, 0, 20, 20), new Size(10, 15));
            //Assert.AreEqual(new Rectangle(-10, -15, 40, 50), r);

            r = new Rectangle(0, 0, 20, 20);
            r = r.Inflate(10, 15);
            Assert.AreEqual(new Rectangle(-10, -15, 40, 50), r);

            r = new Rectangle(0, 0, 20, 20);
            r = r.Inflate(new Size(10, 15));
            Assert.AreEqual(new Rectangle(-10, -15, 40, 50), r);
        }

        [Test]
        public void IntersectsWith() {
            Rectangle r = new Rectangle(0, 0, 50, 50);

            // fully contained
            Assert.IsTrue(r.IntersectsWith(new Rectangle(10, 10, 10, 10)));

            // crosses top side
            Assert.IsTrue(r.IntersectsWith(new Rectangle(5, -5, 10, 10)));

            // crosses right side
            Assert.IsTrue(r.IntersectsWith(new Rectangle(5, 5, 50, 10)));

            // crosses bottom side
            Assert.IsTrue(r.IntersectsWith(new Rectangle(5, 5, 10, 50)));

            // crosses left side
            Assert.IsTrue(r.IntersectsWith(new Rectangle(-5, 5, 10, 10)));

            // completely outside (top)
            Assert.IsFalse(r.IntersectsWith(new Rectangle(5, -5, 1, 1)));

            // completely outside (right)
            Assert.IsFalse(r.IntersectsWith(new Rectangle(75, 5, 1, 1)));

            // completely outside (bottom)
            Assert.IsFalse(r.IntersectsWith(new Rectangle(5, 75, 1, 1)));

            // completely outside (left)
            Assert.IsFalse(r.IntersectsWith(new Rectangle(-25, 5, 1, 1)));
        }

        [Test]
        public void Intersect() {
            Rectangle r;

            // fully contained
            r = new Rectangle(0, 0, 50, 50);
            r = r.Intersect(new Rectangle(10, 10, 10, 10));
            Assert.AreEqual(new Rectangle(10, 10, 10, 10), r);

            // crosses top side
            r = new Rectangle(0, 0, 50, 50);
            r = r.Intersect(new Rectangle(5, -5, 10, 10));
            Assert.AreEqual(new Rectangle(5, 0, 10, 5), r);

            // crosses right side
            r = new Rectangle(0, 0, 50, 50);
            r = r.Intersect(new Rectangle(5, 5, 50, 10));
            Assert.AreEqual(new Rectangle(5, 5, 45, 10), r);

            // crosses bottom side
            r = new Rectangle(0, 0, 50, 50);
            r = r.Intersect(new Rectangle(5, 5, 10, 50));

            // crosses left side
            r = new Rectangle(0, 0, 50, 50);
            r = r.Intersect(new Rectangle(-5, 5, 10, 10));

            // completely outside (top)
            r = new Rectangle(0, 0, 50, 50);
            r = r.Intersect(new Rectangle(5, -5, 1, 1));
            Assert.AreEqual(Rectangle.Zero, r);

            // completely outside (right)
            r = new Rectangle(0, 0, 50, 50);
            r = r.Intersect(new Rectangle(75, 5, 1, 1));
            Assert.AreEqual(Rectangle.Zero, r);

            // completely outside (left)
            r = new Rectangle(0, 0, 50, 50);
            r = r.Intersect(new Rectangle(-25, 5, 1, 1));
            Assert.AreEqual(Rectangle.Zero, r);

            // completely outside (bottom)
            r = new Rectangle(0, 0, 50, 50);
            r = r.Intersect(new Rectangle(5, 75, 1, 1));
            Assert.AreEqual(Rectangle.Zero, r);
        }


        [Test]
        public void Equals_Operator() {
            Rectangle r1 = new Rectangle(1, 2, 30, 30);
            Rectangle r2 = new Rectangle(1, 2, 30, 30);

            Assert.AreEqual(true, r1 == r2);
            Assert.AreEqual(false, r1 != r2);

            r2 = new Rectangle(10, 20, 30, 30);

            Assert.AreEqual(false, r1 == r2);
            Assert.AreEqual(true, r1 != r2);

            r1 = Rectangle.Zero;
            r2 = Rectangle.Zero;

            Assert.AreEqual(true, r1 == r2);
            Assert.AreEqual(false, r1 != r2);
        }

    }

}
/*
 
  [Test]

        public void Ctor_Accessor1() {
            Rectangle r;

            r = new Rectangle(new Size(40, 50));
            Assert.AreEqual(0, r.X);
            Assert.AreEqual(0, r.Y);
            Assert.AreEqual(40, r.Width);
            Assert.AreEqual(50, r.Height);
            Assert.AreEqual(0, r.Left);
            Assert.AreEqual(0, r.Top);
            Assert.AreEqual(40, r.Right);
            Assert.AreEqual(50, r.Bottom);
            Assert.AreEqual(new Point(0, 0), r.Location);
            Assert.AreEqual(new Size(40, 50), r.Size);

            r = new Rectangle(new Point(4, 5), new Vector(20, 30));
            Assert.AreEqual(4, r.X);
            Assert.AreEqual(5, r.Y);
            Assert.AreEqual(20, r.Width);
            Assert.AreEqual(30, r.Height);
            Assert.AreEqual(4, r.Left);
            Assert.AreEqual(5, r.Top);
            Assert.AreEqual(24, r.Right);
            Assert.AreEqual(35, r.Bottom);
            Assert.AreEqual(new Point(4, 5), r.Location);
            Assert.AreEqual(new Size(20, 30), r.Size);

            r = new Rectangle(new Point(4, 5), new Point(20, 30));
            Assert.AreEqual(4, r.X);
            Assert.AreEqual(5, r.Y);
            Assert.AreEqual(16, r.Width);
            Assert.AreEqual(25, r.Height);
            Assert.AreEqual(4, r.Left);
            Assert.AreEqual(5, r.Top);
            Assert.AreEqual(20, r.Right);
            Assert.AreEqual(30, r.Bottom);
            Assert.AreEqual(new Point(4, 5), r.Location);
            Assert.AreEqual(new Size(16, 25), r.Size);

            r = new Rectangle(new Point(20, 30), new Point(4, 5));
            Assert.AreEqual(4, r.X);
            Assert.AreEqual(5, r.Y);
            Assert.AreEqual(16, r.Width);
            Assert.AreEqual(25, r.Height);
            Assert.AreEqual(4, r.Left);
            Assert.AreEqual(5, r.Top);
            Assert.AreEqual(20, r.Right);
            Assert.AreEqual(30, r.Bottom);
            Assert.AreEqual(new Point(4, 5), r.Location);
            Assert.AreEqual(new Size(16, 25), r.Size);
 }
 [Test]
        public void ToStringTest() {
            Rectangle r = new Rectangle(1.0, 2.5, 3, 4);

            string expectedStringOutput = "1,2.5,3,4";
            Assert.AreEqual(expectedStringOutput, r.ToString());
            Assert.AreEqual("Zero", Rectangle.Zero.ToString());

            // IFormattable.ToString
            IFormattable rFormattable = r;
            Assert.AreEqual(expectedStringOutput,
                            rFormattable.ToString(null, null),
                            "IFormattable.ToString with null format");
            Assert.AreEqual(expectedStringOutput,
                            rFormattable.ToString(string.Empty, null),
                            "IFormattable.ToString with empty format");
            Assert.AreEqual("1.00,2.50,3.00,4.00",
                            rFormattable.ToString("N2", null),
                            "IFormattable.ToString with N2 format");
            Assert.AreEqual("blah,blah,blah,blah",
                            rFormattable.ToString("blah", null),
                            "IFormattable.ToString with blah format");
            Assert.AreEqual(":,:,:,:",
                            rFormattable.ToString(":", null),
                            "IFormattable.ToString with : format");
           
            foreach (CultureInfo culture in CultureInfo.GetCultures(CultureTypes.AllCultures)) {
                if (culture.IsNeutralCulture)
                    continue;
                string separator = ",";
                if (culture.NumberFormat.NumberDecimalSeparator == separator)
                    separator = ";";
                expectedStringOutput =
                    1.ToString(culture) + separator +
                    2.5.ToString(culture) + separator +
                    3.ToString(culture) + separator +
                    4.ToString(culture);
                Assert.AreEqual(expectedStringOutput,
                                r.ToString(culture),
                                "ToString with Culture: " + culture.Name);
                Assert.AreEqual("Zero",
                                Rectangle.Zero.ToString(culture),
                                "ToString on Zero with Culture: " + culture.Name);

                // IFormattable.ToString
                Assert.AreEqual(expectedStringOutput,
                                rFormattable.ToString(null, culture),
                                "IFormattable.ToString with null format with Culture: " + culture.Name);
                Assert.AreEqual(expectedStringOutput,
                                rFormattable.ToString(string.Empty, culture),
                                "IFormattable.ToString with empty format with Culture: " + culture.Name);
                expectedStringOutput =
                    1.ToString("N2", culture) + separator +
                    2.5.ToString("N2", culture) + separator +
                    3.ToString("N2", culture) + separator +
                    4.ToString("N2", culture);
                Assert.AreEqual(expectedStringOutput,
                                rFormattable.ToString("N2", culture),
                                "IFormattable.ToString with N2 format with Culture: " + culture.Name);
            }
        }
        [Test]
        [Category("NotWorking")]
        public void ToString_FormatException() {
            // This test does not currently work because
            // String.Format does not throw all necessary exceptions
            IFormattable rFormattable = new Rectangle(1.0, 2.5, 3, 4);
            bool exceptionRaised = false;
            try {
                rFormattable.ToString("{", null);
            } catch (FormatException) {
                exceptionRaised = true;
            }
            Assert.IsTrue(exceptionRaised, "Expected FormatException with IFormattable.ToString (\"{\", null)");
        }

        [Test]
        [Category("NotWorking")]
        public void Parse() {
            Rectangle r = Rectangle.Parse("1 , 2, 3, 4");
            Assert.AreEqual(new Rectangle(1, 2, 3, 4), r);
        }

        [Test]
        [Category("NotWorking")]
        public void Parse2() {
            Rectangle r = Rectangle.Parse("1 2 3 4");
            Assert.AreEqual(new Rectangle(1, 2, 3, 4), r);
        }

        [Test]
        [Category("NotWorking")]
        public void Parse3() {
            Rectangle r = Rectangle.Parse(" 1 2 3 4 ");
            Assert.AreEqual(new Rectangle(1, 2, 3, 4), r);
        }

        [Test]
        [Category("NotWorking")]
        public void ParseWithBothSeparators() {
            Rectangle.Parse("1.0, 3 2.0, 5.0");
        }

        [Test]
        [Category("NotWorking")]
        [ExpectedException(typeof(ArgumentException))]
        public void ParseNegative() {
            Rectangle.Parse("1, 2, -3, -4");
        }

        [Test]
        [Category("NotWorking")]
        [ExpectedException(typeof(InvalidOperationException))] // "Premature string termination encountered."
        public void Parse3Doubles() {
            Rectangle.Parse("1.0, 3, -5");
        }

        [Test]
        [Category("NotWorking")]
        [ExpectedException(typeof(FormatException))]
        public void ParseInvalidString1() {
            Rectangle.Parse("1.0, 3, -x, 5.0");
        }

        [Test]
        [Category("NotWorking")]
        [ExpectedException(typeof(InvalidOperationException))]
        public void ParseInvalidString3() {
            Rectangle.Parse("1.0, 3, 2.0, 5.0, 2");
        }
 
        [Test]
        [Category("NotWorking")]
        [ExpectedException(typeof(FormatException))]
        public void ParseInvalidString4() {
            Rectangle.Parse("1.0-3, 2.0, 5.0, 2");
        }

        [Test]
        [Category("NotWorking")]
        [ExpectedException(typeof(InvalidOperationException))]
        public void ParseInvalidString5() {
            Rectangle.Parse("1.0, 2.0, 5.0, 2,");
        }

        [Test]
        [Category("NotWorking")]
        public void ParseInvalidString6() {
            Rectangle.Parse("\n1.0, 2.0, 5.0, 2");
        } 
[Test]
        [Category("NotWorking")]
        [ExpectedException(typeof(InvalidOperationException))]
        public void ParseInvalidString7() {
            Rectangle r = Rectangle.Parse("1,, 2, 3, 4");
            Assert.AreEqual(new Rectangle(1, 2, 3, 4), r);
        } 
*/
