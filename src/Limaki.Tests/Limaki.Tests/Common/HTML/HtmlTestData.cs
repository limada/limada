﻿namespace Limaki.Tests.Common.HTML {
    public class HtmlTestData {
        public string Commented {
            get {
                return @"
<meta name=""collection"" content=""api"">
/* Generated by javadoc (build 1.4.2-rc) on Fri Jun 13 00:14:19 PDT 2003 */
<TITLE>
";
            }
        }




        public string HTML19 {
            get {
                return @"
<html>
<meta name=""collection"" content=""api"">
    <style>
        p.Flieatext {
          FirstLineIndent:8.6;
		  Alignment:justify;
          SpaceBelow:0.0;
          SpaceAbove:0.0;
          RightIndent:0.0;
          LeftIndent:0.0;
        }
        p.InhaltaTa {
          FirstLineIndent:0.0;
          Alignment:left;
          SpaceBelow:3.0;
          SpaceAbove:4.0;
          RightIndent:0.0;
          LeftIndent:0.0;
        }
        p.Titelaa {
          FirstLineIndent:0.0;
          Alignment:left;
          SpaceBelow:6.0;
          SpaceAbove:4.0;
          RightIndent:0.0;
          LeftIndent:0.0;
        }
        p.Titelaa {
          FirstLineIndent:0.0;
          Alignment:left;
          SpaceBelow:5.0;
          SpaceAbove:3.0;
          RightIndent:0.0;
          LeftIndent:0.0;
        }
        p.Quelle {
          FirstLineIndent:0.0;
          Alignment:left;
          SpaceBelow:0.0;
          SpaceAbove:0.0;
          RightIndent:0.0;
          LeftIndent:0.0;
        }
        p.Titelaa {
          FirstLineIndent:0.0;
          Alignment:left;
          SpaceBelow:5.0;
          SpaceAbove:3.0;
          RightIndent:0.0;
          LeftIndent:0.0;
        }
        p.Titelaa {
          FirstLineIndent:0.0;
          Alignment:left;
          SpaceBelow:9.5;
          SpaceAbove:7.0;
          RightIndent:0.0;
          LeftIndent:0.0;
        }
        p.aNoaparagraphastylea {
          FirstLineIndent:0.0;
          Alignment:left;
          SpaceBelow:0.0;
          SpaceAbove:0.0;
          RightIndent:0.0;
          LeftIndent:0.0;
        }
        p.Fuanotenzeichen {
          superscript:super;
          italic:;
          bold:normal;
          underline:;
        }
        p.Flieatextakursiv {
          FirstLineIndent:8.6;
          Alignment:justify;
          SpaceBelow:0.0;
          SpaceAbove:0.0;
          RightIndent:0.0;
          LeftIndent:0.0;
        }
    </style>
</html>
";
            }
        }


        public string HTML4 {
            get {
                return @"<html>
	<body>
		<p class='noclass' align='center'>
			<b>
				Good Morning
				<br/>
				<!-- this is a comment-->
				<i>hey, < how > does'it
					<a href=bla>
						Here the HREF-Text
					</a>
			</b>
			<b>
			</b>
		</p>
	</body>
</html>";
            }
        }



        public string HTML2 {
            get {
                return @"
<!DOCTYPE html PUBLIC \""-//W3C//DTD XHTML 1.0 Strict//EN\"" 

\""http://www.w3.org/TR/xhtml1/DTD/xhtml1-strict.dtd\""><html>  <head>    <style>      <!--     

   p.Flieatextakursiv {          RightIndent:0.0;          LeftIndent:0.0;          

FirstLineIndent:8.6;          Alignment:justify;          SpaceBelow:0.0;          

SpaceAbove:0.0;        }        p.InhaltaTa {          RightIndent:0.0;          

LeftIndent:0.0;          FirstLineIndent:0.0;          Alignment:left;          SpaceBelow:5.0;   

       SpaceAbove:3.0;        }        p.Titelaa {          RightIndent:0.0;          

LeftIndent:0.0;          FirstLineIndent:0.0;          Alignment:left;          SpaceBelow:6.0;   

       SpaceAbove:4.0;        }        p.Titelaa {          RightIndent:0.0;          

LeftIndent:0.0;          FirstLineIndent:0.0;          Alignment:left;          SpaceBelow:5.0;   

       SpaceAbove:3.0;        }        p.InhaltaTa {          RightIndent:0.0;          

LeftIndent:0.0;          FirstLineIndent:0.0;          Alignment:left;          SpaceBelow:3.0;   

       SpaceAbove:4.0;        }        p.Titelaa {          RightIndent:0.0;          

LeftIndent:0.0;          FirstLineIndent:0.0;          Alignment:left;          SpaceBelow:5.0;   

       SpaceAbove:3.0;        }        p.Titelaa {          RightIndent:0.0;          

LeftIndent:0.0;          FirstLineIndent:0.0;          Alignment:left;          SpaceBelow:9.5;   

       SpaceAbove:7.0;        }        p.Quelle {          RightIndent:0.0;          

LeftIndent:0.0;          FirstLineIndent:0.0;          Alignment:left;          SpaceBelow:0.0;   

       SpaceAbove:0.0;        }        p.Fuanotenzeichen {          bold:normal;          

superscript:super;          underline:;          italic:;        }        p.aNoaparagraphastylea 

{          RightIndent:0.0;          LeftIndent:0.0;          FirstLineIndent:0.0;          

Alignment:left;          SpaceBelow:0.0;          SpaceAbove:0.0;        }        p.Flieatext 

{          RightIndent:0.0;          LeftIndent:0.0;          FirstLineIndent:8.6;          

Alignment:justify;          SpaceBelow:0.0;          SpaceAbove:0.0;        }      -->    

</style>  </head>  <body>    <p class=Quelle align=nowhere>        

<i>http://download.oracle.com/javase/1.4.2/docs/api/java/lang/String.html/i<i>    </i></i>/p>    <p 

class=InhaltaTa>        <b>Class String</b>    </p>    <p class=Flieatextakursiv>        

<i>public final class String</i>        <i>1</i>        <i>The String class represents character 

strings. All string literals in Java programs, such as \""abc\"", are implemented as instances of this class. 

</i>    </p>    <p class=Titelaa>      <span style=\""color: #000000; font-size: 11pt; 

font-family: Times New Roman\"">        <b>Constructor Summary</b>        <br/>      </span>    

</p>    <p class=Flieatext>      <span style=\""color: #000000; font-size: 9pt; font-family: 

ClassGarmnd BT\"">Constructs a new String by decoding the specified array of bytes using the platform's 

default charset. The length of the new String is a function of the charset, and hence may not be equal to the 

length of the byte array.The behavior of this constructor when the given bytes are not valid in the 

default charset is unspecified. The CharsetDecoder class should be used when more control over the decoding 

process is required.       </span>    </p>    <p class=Flieatext>      <span style=\""color: 

#000000; font-size: 9pt; font-family: ClassGarmnd BT\"">The Java language provides special support for the 

string concatenation operator ( + ), and for conversion of other objects to strings. String concatenation is 

implemented through the StringBuffer class and its append method. String conversions are implemented through the 

method toString, defined by Object and inherited by all classes in Java. For additional information on string 

concatenation and conversion, see Gosling, Joy, and Steele, The Java Language Specification.      

</span>    </p>  </body></html>
";
            }
        }

        public string HTML1 {
            get {
                return @"
<!DOCTYPE html PUBLIC ""-//W3C//DTD XHTML 1.0 Strict//EN"" ""http://www.w3.org/TR/xhtml1/DTD/xhtml1-strict.dtd"">
<html>
  <head>
    <style>
      <!--
        p.Flieatextakursiv {
          RightIndent:0.0;
          LeftIndent:0.0;
          FirstLineIndent:8.6;
          Alignment:justify;
          SpaceBelow:0.0;
          SpaceAbove:0.0;
        }
        p.InhaltaTa {
          RightIndent:0.0;
          LeftIndent:0.0;
          FirstLineIndent:0.0;
          Alignment:left;
          SpaceBelow:5.0;
          SpaceAbove:3.0;
        }
        p.Titelaa {
          RightIndent:0.0;
          LeftIndent:0.0;
          FirstLineIndent:0.0;
          Alignment:left;
          SpaceBelow:6.0;
          SpaceAbove:4.0;
        }
        p.Titelaa {
          RightIndent:0.0;
          LeftIndent:0.0;
          FirstLineIndent:0.0;
          Alignment:left;
          SpaceBelow:5.0;
          SpaceAbove:3.0;
        }
        p.InhaltaTa {
          RightIndent:0.0;
          LeftIndent:0.0;
          FirstLineIndent:0.0;
          Alignment:left;
          SpaceBelow:3.0;
          SpaceAbove:4.0;
        }
        p.Titelaa {
          RightIndent:0.0;
          LeftIndent:0.0;
          FirstLineIndent:0.0;
          Alignment:left;
          SpaceBelow:5.0;
          SpaceAbove:3.0;
        }
        p.Titelaa {
          RightIndent:0.0;
          LeftIndent:0.0;
          FirstLineIndent:0.0;
          Alignment:left;
          SpaceBelow:9.5;
          SpaceAbove:7.0;
        }
        p.Quelle {
          RightIndent:0.0;
          LeftIndent:0.0;
          FirstLineIndent:0.0;
          Alignment:left;
          SpaceBelow:0.0;
          SpaceAbove:0.0;
        }
        p.Fuanotenzeichen {
          bold:normal;
          superscript:super;
          underline:;
          italic:;
        }
        p.aNoaparagraphastylea {
          RightIndent:0.0;
          LeftIndent:0.0;
          FirstLineIndent:0.0;
          Alignment:left;
          SpaceBelow:0.0;
          SpaceAbove:0.0;
        }
        p.Flieatext {
          RightIndent:0.0;
          LeftIndent:0.0;
          FirstLineIndent:8.6;
          Alignment:justify;
          SpaceBelow:0.0;
          SpaceAbove:0.0;
        }
      -->
    </style>
  </head>
  <body>

    <p class=Quelle>
        <i>http://download.oracle.com/javase/1.4.2/docs/api/java/lang/String.html/i<i>
    </p>
    <p class=InhaltaTa>
        <b>Class String</b>
    </p>
    <p class=Flieatextakursiv>
        <i>public final class String</i>
        <i>1</i>

        <i>The String class represents character strings. All string literals in Java programs, such as ""abc"", are implemented as instances of this class. </i>

    </p>
    <p class=Titelaa>
      <span style=""color: #000000; font-size: 11pt; font-family: Times New Roman"">
        <b>Constructor Summary</b>
        <br/>
      </span>
    </p>
    <p class=Flieatext>

      <span style=""color: #000000; font-size: 9pt; font-family: ClassGarmnd BT"">
Constructs a new String by decoding the specified array of bytes using the platform's default charset. The length of the new String is a function of the charset, and hence may not be equal to the length of the byte array.

The behavior of this constructor when the given bytes are not valid in the default charset is unspecified. The CharsetDecoder class should be used when more control over the decoding process is required. 
      </span>

    </p>
    <p class=Flieatext>
      <span style=""color: #000000; font-size: 9pt; font-family: ClassGarmnd BT"">
The Java language provides special support for the string concatenation operator ( + ), and for conversion of other objects to strings. String concatenation is implemented through the StringBuffer class and its append method. String conversions are implemented through the method toString, defined by Object and inherited by all classes in Java. For additional information on string concatenation and conversion, see Gosling, Joy, and Steele, The Java Language Specification.
      </span>

    </p>


  </body>
</html>

";
            }
        }

        public string MissingEndTags {
            get { return @"
<HTML><BODY>
<!--StartFragment-->
<p><i>this is a paragraph
</BODY></HTML>
"; }
        }
    
            public string MissingEndTags1 {
            get { return @"
<HTML><BODY>
<!--StartFragment-->
<p><i>this is a paragraph</i>
<img src=""http://www.somelink.org/someimage.gif"" border=""0"" height=""41"" width=""49"">
</BODY></HTML>
"; }
        }
    }
}