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
// Copyright (c) 2005 Novell, Inc. (http://www.novell.com)
//
// Authors:
//	Peter Bartok	(pbartok@novell.com)
//
//

// COMPLETE

namespace Limaki.Common.Text.RTF.Parser {

	public enum StandardCharCode {
		nothing			= 0,
		space			= 1,
		exclam			= 2,
		quotedbl		= 3,
		numbersign		= 4,
		dollar			= 5,
		percent			= 6,
		ampersand		= 7,
		quoteright		= 8,
		parenleft		= 9,
		parenright		= 10,
		asterisk		= 11,
		plus			= 12,
		comma			= 13,
		hyphen			= 14,
		period			= 15,
		slash			= 16,
		zero			= 17,
		one			= 18,
		two			= 19,
		three			= 20,
		four			= 21,
		five			= 22,
		six			= 23,
		seven			= 24,
		eight			= 25,
		nine			= 26,
		colon			= 27,
		semicolon		= 28,
		less			= 29,
		equal			= 30,
		greater			= 31,
		question		= 32,
		at			= 33,
		A			= 34,
		B			= 35,
		C			= 36,
		D			= 37,
		E			= 38,
		F			= 39,
		G			= 40,
		H			= 41,
		I			= 42,
		J			= 43,
		K			= 44,
		L			= 45,
		M			= 46,
		N			= 47,
		O			= 48,
		P			= 49,
		Q			= 50,
		R			= 51,
		S			= 52,
		T			= 53,
		U			= 54,
		V			= 55,
		W			= 56,
		X			= 57,
		Y			= 58,
		Z			= 59,
		bracketleft		= 60,
		backslash		= 61,
		bracketright		= 62,
		asciicircum		= 63,
		underscore		= 64,
		quoteleft		= 65,
		a			= 66,
		b			= 67,
		c			= 68,
		d			= 69,
		e			= 70,
		f			= 71,
		g			= 72,
		h			= 73,
		i			= 74,
		j			= 75,
		k			= 76,
		l			= 77,
		m			= 78,
		n			= 79,
		o			= 80,
		p			= 81,
		q			= 82,
		r			= 83,
		s			= 84,
		t			= 85,
		u			= 86,
		v			= 87,
		w			= 88,
		x			= 89,
		y			= 90,
		z			= 91,
		braceleft		= 92,
		bar			= 93,
		braceright		= 94,
		asciitilde		= 95,
		exclamdown		= 96,
		cent			= 97,
		sterling		= 98,
		fraction		= 99,
		yen			= 100,
		florin			= 101,
		section			= 102,
		currency		= 103,
		quotedblleft		= 104,
		guillemotleft		= 105,
		guilsinglleft		= 106,
		guilsinglright		= 107,
		fi			= 108,
		fl			= 109,
		endash			= 110,
		dagger			= 111,
		daggerdbl		= 112,
		periodcentered		= 113,
		paragraph		= 114,
		bullet			= 115,
		quotesinglbase		= 116,
		quotedblbase		= 117,
		quotedblright		= 118,
		guillemotright		= 119,
		ellipsis		= 120,
		perthousand		= 121,
		questiondown		= 122,
		grave			= 123,
		acute			= 124,
		circumflex		= 125,
		tilde			= 126,
		macron			= 127,
		breve			= 128,
		dotaccent		= 129,
		dieresis		= 130,
		ring			= 131,
		cedilla			= 132,
		hungarumlaut		= 133,
		ogonek			= 134,
		caron			= 135,
		emdash			= 136,
		AE			= 137,
		ordfeminine		= 138,
		Lslash			= 139,
		Oslash			= 140,
		OE			= 141,
		ordmasculine		= 142,
		ae			= 143,
		dotlessi		= 144,
		lslash			= 145,
		oslash			= 146,
		oe			= 147,
		germandbls		= 148,
		Aacute			= 149,
		Acircumflex		= 150,
		Adieresis		= 151,
		Agrave			= 152,
		Aring			= 153,
		Atilde			= 154,
		Ccedilla		= 155,
		Eacute			= 156,
		Ecircumflex		= 157,
		Edieresis		= 158,
		Egrave			= 159,
		Eth			= 160,
		Iacute			= 161,
		Icircumflex		= 162,
		Idieresis		= 163,
		Igrave			= 164,
		Ntilde			= 165,
		Oacute			= 166,
		Ocircumflex		= 167,
		Odieresis		= 168,
		Ograve			= 169,
		Otilde			= 170,
		Scaron			= 171,
		Thorn			= 172,
		Uacute			= 173,
		Ucircumflex		= 174,
		Udieresis		= 175,
		Ugrave			= 176,
		Yacute			= 177,
		Ydieresis		= 178,
		aacute			= 179,
		acircumflex		= 180,
		adieresis		= 181,
		agrave			= 182,
		aring			= 183,
		atilde			= 184,
		brokenbar		= 185,
		ccedilla		= 186,
		copyright		= 187,
		degree			= 188,
		divide			= 189,
		eacute			= 190,
		ecircumflex		= 191,
		edieresis		= 192,
		egrave			= 193,
		eth			= 194,
		iacute			= 195,
		icircumflex		= 196,
		idieresis		= 197,
		igrave			= 198,
		logicalnot		= 199,
		minus			= 200,
		multiply		= 201,
		ntilde			= 202,
		oacute			= 203,
		ocircumflex		= 204,
		odieresis		= 205,
		ograve			= 206,
		onehalf			= 207,
		onequarter		= 208,
		onesuperior		= 209,
		otilde			= 210,
		plusminus		= 211,
		registered		= 212,
		thorn			= 213,
		threequarters		= 214,
		threesuperior		= 215,
		trademark		= 216,
		twosuperior		= 217,
		uacute			= 218,
		ucircumflex		= 219,
		udieresis		= 220,
		ugrave			= 221,
		yacute			= 222,
		ydieresis		= 223,
		Alpha			= 224,
		Beta			= 225,
		Chi			= 226,
		Delta			= 227,
		Epsilon			= 228,
		Phi			= 229,
		Gamma			= 230,
		Eta			= 231,
		Iota			= 232,
		Kappa			= 233,
		Lambda			= 234,
		Mu			= 235,
		Nu			= 236,
		Omicron			= 237,
		Pi			= 238,
		Theta			= 239,
		Rho			= 240,
		Sigma			= 241,
		Tau			= 242,
		Upsilon			= 243,
		varUpsilon		= 244,
		Omega			= 245,
		Xi			= 246,
		Psi			= 247,
		Zeta			= 248,
		alpha			= 249,
		beta			= 250,
		chi			= 251,
		delta			= 252,
		epsilon			= 253,
		phi			= 254,
		varphi			= 255,
		gamma			= 256,
		eta			= 257,
		iota			= 258,
		kappa			= 259,
		lambda			= 260,
		mu			= 261,
		nu			= 262,
		omicron			= 263,
		pi			= 264,
		varpi			= 265,
		theta			= 266,
		vartheta		= 267,
		rho			= 268,
		sigma			= 269,
		varsigma		= 270,
		tau			= 271,
		upsilon			= 272,
		omega			= 273,
		xi			= 274,
		psi			= 275,
		zeta			= 276,
		nobrkspace		= 277,
		nobrkhyphen		= 278,
		lessequal		= 279,
		greaterequal		= 280,
		infinity		= 281,
		integral		= 282,
		notequal		= 283,
		radical			= 284,
		radicalex		= 285,
		approxequal		= 286,
		apple			= 287,
		partialdiff		= 288,
		opthyphen		= 289,
		formula			= 290,
		lozenge			= 291,
		universal		= 292,
		existential		= 293,
		suchthat		= 294,
		congruent		= 295,
		therefore		= 296,
		perpendicular		= 297,
		minute			= 298,
		club			= 299,
		diamond			= 300,
		heart			= 301,
		spade			= 302,
		arrowboth		= 303,
		arrowleft		= 304,
		arrowup			= 305,
		arrowright		= 306,
		arrowdown		= 307,
		second			= 308,
		proportional		= 309,
		equivalence		= 310,
		arrowvertex		= 311,
		arrowhorizex		= 312,
		carriagereturn		= 313,
		aleph			= 314,
		Ifraktur		= 315,
		Rfraktur		= 316,
		weierstrass		= 317,
		circlemultiply		= 318,
		circleplus		= 319,
		emptyset		= 320,
		intersection		= 321,
		union			= 322,
		propersuperset		= 323,
		reflexsuperset		= 324,
		notsubset		= 325,
		propersubset		= 326,
		reflexsubset		= 327,
		element			= 328,
		notelement		= 329,
		angle			= 330,
		gradient		= 331,
		product			= 332,
		logicaland		= 333,
		logicalor		= 334,
		arrowdblboth		= 335,
		arrowdblleft		= 336,
		arrowdblup		= 337,
		arrowdblright		= 338,
		arrowdbldown		= 339,
		angleleft		= 340,
		registersans		= 341,
		copyrightsans		= 342,
		trademarksans		= 343,
		angleright		= 344,
		mathplus		= 345,
		mathminus		= 346,
		mathasterisk		= 347,
		mathnumbersign		= 348,
		dotmath			= 349,
		mathequal		= 350,
		mathtilde		= 351,

		MaxChar			= 352
	}
}
