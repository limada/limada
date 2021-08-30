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

using System.Collections;

namespace Limaki.Common.Text.RTF.Parser {

    public class Charcode {
        
        #region Cached Values

        private static Charcode _ansiGeneric;

        #endregion

        #region Local Variables

        private readonly StandardCharCode[] _codes;
        private readonly Hashtable _reverse;
        private readonly int _size;

        #endregion	// Local Variables

        #region Public Constructors

        public Charcode () : this (256) { }

        private Charcode (int size) {
            _size = size;
            _codes = new StandardCharCode[size];
            _reverse = new Hashtable (size);

            // No need to reinitialize array to its default value
            //for (int i = 0; i < size; i++) {
            //	codes[i] = StandardCharCode.nothing;
            //}
        }

        #endregion	// Public Constructors

        #region Public Instance Properties

        public int this [StandardCharCode c] {
            get {
                var obj = _reverse[c];
                if (obj != null) return (int)obj;
                for (var i = 0; i < _size; i++)
                    if (_codes[i] == c)
                        return i;

                return -1;
            }
        }

        public StandardCharCode this [int c] {
            get {
                if (c < 0 || c >= _size) return StandardCharCode.nothing;

                return _codes[c];
            }

#if NET_2_0
			private 
#endif
            set {
                if (c < 0 || c >= _size) return;

                _codes[c] = value;
                _reverse[value] = c;
            }
        }

        #endregion	// Public Instance Properties

        #region Public Instance Methods

        #endregion	// Public Instance Methods

        #region Public Static Methods

        public static Charcode AnsiGeneric {
            get {
                if (_ansiGeneric != null)
                    return _ansiGeneric;

                _ansiGeneric = new Charcode (256);

                _ansiGeneric[0x06] = StandardCharCode.formula;
                _ansiGeneric[0x1e] = StandardCharCode.nobrkhyphen;
                _ansiGeneric[0x1f] = StandardCharCode.opthyphen;
                _ansiGeneric[' '] = StandardCharCode.space;
                _ansiGeneric['!'] = StandardCharCode.exclam;
                _ansiGeneric['"'] = StandardCharCode.quotedbl;
                _ansiGeneric['#'] = StandardCharCode.numbersign;
                _ansiGeneric['$'] = StandardCharCode.dollar;
                _ansiGeneric['%'] = StandardCharCode.percent;
                _ansiGeneric['&'] = StandardCharCode.ampersand;
                _ansiGeneric['\\'] = StandardCharCode.quoteright;
                _ansiGeneric['('] = StandardCharCode.parenleft;
                _ansiGeneric[')'] = StandardCharCode.parenright;
                _ansiGeneric['*'] = StandardCharCode.asterisk;
                _ansiGeneric['+'] = StandardCharCode.plus;
                _ansiGeneric[','] = StandardCharCode.comma;
                _ansiGeneric['-'] = StandardCharCode.hyphen;
                _ansiGeneric['.'] = StandardCharCode.period;
                _ansiGeneric['/'] = StandardCharCode.slash;
                _ansiGeneric['0'] = StandardCharCode.zero;
                _ansiGeneric['1'] = StandardCharCode.one;
                _ansiGeneric['2'] = StandardCharCode.two;
                _ansiGeneric['3'] = StandardCharCode.three;
                _ansiGeneric['4'] = StandardCharCode.four;
                _ansiGeneric['5'] = StandardCharCode.five;
                _ansiGeneric['6'] = StandardCharCode.six;
                _ansiGeneric['7'] = StandardCharCode.seven;
                _ansiGeneric['8'] = StandardCharCode.eight;
                _ansiGeneric['9'] = StandardCharCode.nine;
                _ansiGeneric[':'] = StandardCharCode.colon;
                _ansiGeneric[';'] = StandardCharCode.semicolon;
                _ansiGeneric['<'] = StandardCharCode.less;
                _ansiGeneric['='] = StandardCharCode.equal;
                _ansiGeneric['>'] = StandardCharCode.greater;
                _ansiGeneric['?'] = StandardCharCode.question;
                _ansiGeneric['@'] = StandardCharCode.at;
                _ansiGeneric['A'] = StandardCharCode.A;
                _ansiGeneric['B'] = StandardCharCode.B;
                _ansiGeneric['C'] = StandardCharCode.C;
                _ansiGeneric['D'] = StandardCharCode.D;
                _ansiGeneric['E'] = StandardCharCode.E;
                _ansiGeneric['F'] = StandardCharCode.F;
                _ansiGeneric['G'] = StandardCharCode.G;
                _ansiGeneric['H'] = StandardCharCode.H;
                _ansiGeneric['I'] = StandardCharCode.I;
                _ansiGeneric['J'] = StandardCharCode.J;
                _ansiGeneric['K'] = StandardCharCode.K;
                _ansiGeneric['L'] = StandardCharCode.L;
                _ansiGeneric['M'] = StandardCharCode.M;
                _ansiGeneric['N'] = StandardCharCode.N;
                _ansiGeneric['O'] = StandardCharCode.O;
                _ansiGeneric['P'] = StandardCharCode.P;
                _ansiGeneric['Q'] = StandardCharCode.Q;
                _ansiGeneric['R'] = StandardCharCode.R;
                _ansiGeneric['S'] = StandardCharCode.S;
                _ansiGeneric['T'] = StandardCharCode.T;
                _ansiGeneric['U'] = StandardCharCode.U;
                _ansiGeneric['V'] = StandardCharCode.V;
                _ansiGeneric['W'] = StandardCharCode.W;
                _ansiGeneric['X'] = StandardCharCode.X;
                _ansiGeneric['Y'] = StandardCharCode.Y;
                _ansiGeneric['Z'] = StandardCharCode.Z;
                _ansiGeneric['['] = StandardCharCode.bracketleft;
                _ansiGeneric['\\'] = StandardCharCode.backslash;
                _ansiGeneric[']'] = StandardCharCode.bracketright;
                _ansiGeneric['^'] = StandardCharCode.asciicircum;
                _ansiGeneric['_'] = StandardCharCode.underscore;
                _ansiGeneric['`'] = StandardCharCode.quoteleft;
                _ansiGeneric['a'] = StandardCharCode.a;
                _ansiGeneric['b'] = StandardCharCode.b;
                _ansiGeneric['c'] = StandardCharCode.c;
                _ansiGeneric['d'] = StandardCharCode.d;
                _ansiGeneric['e'] = StandardCharCode.e;
                _ansiGeneric['f'] = StandardCharCode.f;
                _ansiGeneric['g'] = StandardCharCode.g;
                _ansiGeneric['h'] = StandardCharCode.h;
                _ansiGeneric['i'] = StandardCharCode.i;
                _ansiGeneric['j'] = StandardCharCode.j;
                _ansiGeneric['k'] = StandardCharCode.k;
                _ansiGeneric['l'] = StandardCharCode.l;
                _ansiGeneric['m'] = StandardCharCode.m;
                _ansiGeneric['n'] = StandardCharCode.n;
                _ansiGeneric['o'] = StandardCharCode.o;
                _ansiGeneric['p'] = StandardCharCode.p;
                _ansiGeneric['q'] = StandardCharCode.q;
                _ansiGeneric['r'] = StandardCharCode.r;
                _ansiGeneric['s'] = StandardCharCode.s;
                _ansiGeneric['t'] = StandardCharCode.t;
                _ansiGeneric['u'] = StandardCharCode.u;
                _ansiGeneric['v'] = StandardCharCode.v;
                _ansiGeneric['w'] = StandardCharCode.w;
                _ansiGeneric['x'] = StandardCharCode.x;
                _ansiGeneric['y'] = StandardCharCode.y;
                _ansiGeneric['z'] = StandardCharCode.z;
                _ansiGeneric['{'] = StandardCharCode.braceleft;
                _ansiGeneric['|'] = StandardCharCode.bar;
                _ansiGeneric['}'] = StandardCharCode.braceright;
                _ansiGeneric['~'] = StandardCharCode.asciitilde;
                _ansiGeneric[0xa0] = StandardCharCode.nobrkspace;
                _ansiGeneric[0xa1] = StandardCharCode.exclamdown;
                _ansiGeneric[0xa2] = StandardCharCode.cent;
                _ansiGeneric[0xa3] = StandardCharCode.sterling;
                _ansiGeneric[0xa4] = StandardCharCode.currency;
                _ansiGeneric[0xa5] = StandardCharCode.yen;
                _ansiGeneric[0xa6] = StandardCharCode.brokenbar;
                _ansiGeneric[0xa7] = StandardCharCode.section;
                _ansiGeneric[0xa8] = StandardCharCode.dieresis;
                _ansiGeneric[0xa9] = StandardCharCode.copyright;
                _ansiGeneric[0xaa] = StandardCharCode.ordfeminine;
                _ansiGeneric[0xab] = StandardCharCode.guillemotleft;
                _ansiGeneric[0xac] = StandardCharCode.logicalnot;
                _ansiGeneric[0xad] = StandardCharCode.opthyphen;
                _ansiGeneric[0xae] = StandardCharCode.registered;
                _ansiGeneric[0xaf] = StandardCharCode.macron;
                _ansiGeneric[0xb0] = StandardCharCode.degree;
                _ansiGeneric[0xb1] = StandardCharCode.plusminus;
                _ansiGeneric[0xb2] = StandardCharCode.twosuperior;
                _ansiGeneric[0xb3] = StandardCharCode.threesuperior;
                _ansiGeneric[0xb4] = StandardCharCode.acute;
                _ansiGeneric[0xb5] = StandardCharCode.mu;
                _ansiGeneric[0xb6] = StandardCharCode.paragraph;
                _ansiGeneric[0xb7] = StandardCharCode.periodcentered;
                _ansiGeneric[0xb8] = StandardCharCode.cedilla;
                _ansiGeneric[0xb9] = StandardCharCode.onesuperior;
                _ansiGeneric[0xba] = StandardCharCode.ordmasculine;
                _ansiGeneric[0xbb] = StandardCharCode.guillemotright;
                _ansiGeneric[0xbc] = StandardCharCode.onequarter;
                _ansiGeneric[0xbd] = StandardCharCode.onehalf;
                _ansiGeneric[0xbe] = StandardCharCode.threequarters;
                _ansiGeneric[0xbf] = StandardCharCode.questiondown;
                _ansiGeneric[0xc0] = StandardCharCode.Agrave;
                _ansiGeneric[0xc1] = StandardCharCode.Aacute;
                _ansiGeneric[0xc2] = StandardCharCode.Acircumflex;
                _ansiGeneric[0xc3] = StandardCharCode.Atilde;
                _ansiGeneric[0xc4] = StandardCharCode.Adieresis;
                _ansiGeneric[0xc5] = StandardCharCode.Aring;
                _ansiGeneric[0xc6] = StandardCharCode.AE;
                _ansiGeneric[0xc7] = StandardCharCode.Ccedilla;
                _ansiGeneric[0xc8] = StandardCharCode.Egrave;
                _ansiGeneric[0xc9] = StandardCharCode.Eacute;
                _ansiGeneric[0xca] = StandardCharCode.Ecircumflex;
                _ansiGeneric[0xcb] = StandardCharCode.Edieresis;
                _ansiGeneric[0xcc] = StandardCharCode.Igrave;
                _ansiGeneric[0xcd] = StandardCharCode.Iacute;
                _ansiGeneric[0xce] = StandardCharCode.Icircumflex;
                _ansiGeneric[0xcf] = StandardCharCode.Idieresis;
                _ansiGeneric[0xd0] = StandardCharCode.Eth;
                _ansiGeneric[0xd1] = StandardCharCode.Ntilde;
                _ansiGeneric[0xd2] = StandardCharCode.Ograve;
                _ansiGeneric[0xd3] = StandardCharCode.Oacute;
                _ansiGeneric[0xd4] = StandardCharCode.Ocircumflex;
                _ansiGeneric[0xd5] = StandardCharCode.Otilde;
                _ansiGeneric[0xd6] = StandardCharCode.Odieresis;
                _ansiGeneric[0xd7] = StandardCharCode.multiply;
                _ansiGeneric[0xd8] = StandardCharCode.Oslash;
                _ansiGeneric[0xd9] = StandardCharCode.Ugrave;
                _ansiGeneric[0xda] = StandardCharCode.Uacute;
                _ansiGeneric[0xdb] = StandardCharCode.Ucircumflex;
                _ansiGeneric[0xdc] = StandardCharCode.Udieresis;
                _ansiGeneric[0xdd] = StandardCharCode.Yacute;
                _ansiGeneric[0xde] = StandardCharCode.Thorn;
                _ansiGeneric[0xdf] = StandardCharCode.germandbls;
                _ansiGeneric[0xe0] = StandardCharCode.agrave;
                _ansiGeneric[0xe1] = StandardCharCode.aacute;
                _ansiGeneric[0xe2] = StandardCharCode.acircumflex;
                _ansiGeneric[0xe3] = StandardCharCode.atilde;
                _ansiGeneric[0xe4] = StandardCharCode.adieresis;
                _ansiGeneric[0xe5] = StandardCharCode.aring;
                _ansiGeneric[0xe6] = StandardCharCode.ae;
                _ansiGeneric[0xe7] = StandardCharCode.ccedilla;
                _ansiGeneric[0xe8] = StandardCharCode.egrave;
                _ansiGeneric[0xe9] = StandardCharCode.eacute;
                _ansiGeneric[0xea] = StandardCharCode.ecircumflex;
                _ansiGeneric[0xeb] = StandardCharCode.edieresis;
                _ansiGeneric[0xec] = StandardCharCode.igrave;
                _ansiGeneric[0xed] = StandardCharCode.iacute;
                _ansiGeneric[0xee] = StandardCharCode.icircumflex;
                _ansiGeneric[0xef] = StandardCharCode.idieresis;
                _ansiGeneric[0xf0] = StandardCharCode.eth;
                _ansiGeneric[0xf1] = StandardCharCode.ntilde;
                _ansiGeneric[0xf2] = StandardCharCode.ograve;
                _ansiGeneric[0xf3] = StandardCharCode.oacute;
                _ansiGeneric[0xf4] = StandardCharCode.ocircumflex;
                _ansiGeneric[0xf5] = StandardCharCode.otilde;
                _ansiGeneric[0xf6] = StandardCharCode.odieresis;
                _ansiGeneric[0xf7] = StandardCharCode.divide;
                _ansiGeneric[0xf8] = StandardCharCode.oslash;
                _ansiGeneric[0xf9] = StandardCharCode.ugrave;
                _ansiGeneric[0xfa] = StandardCharCode.uacute;
                _ansiGeneric[0xfb] = StandardCharCode.ucircumflex;
                _ansiGeneric[0xfc] = StandardCharCode.udieresis;
                _ansiGeneric[0xfd] = StandardCharCode.yacute;
                _ansiGeneric[0xfe] = StandardCharCode.thorn;
                _ansiGeneric[0xff] = StandardCharCode.ydieresis;

                return _ansiGeneric;
            }
        }

        public static Charcode AnsiSymbol {
            get {
                var code = new Charcode (256);

                code[0x06] = StandardCharCode.formula;
                code[0x1e] = StandardCharCode.nobrkhyphen;
                code[0x1f] = StandardCharCode.opthyphen;
                code[' '] = StandardCharCode.space;
                code['!'] = StandardCharCode.exclam;
                code['"'] = StandardCharCode.universal;
                code['#'] = StandardCharCode.mathnumbersign;
                code['$'] = StandardCharCode.existential;
                code['%'] = StandardCharCode.percent;
                code['&'] = StandardCharCode.ampersand;
                code['\\'] = StandardCharCode.suchthat;
                code['('] = StandardCharCode.parenleft;
                code[')'] = StandardCharCode.parenright;
                code['*'] = StandardCharCode.mathasterisk;
                code['+'] = StandardCharCode.mathplus;
                code[','] = StandardCharCode.comma;
                code['-'] = StandardCharCode.mathminus;
                code['.'] = StandardCharCode.period;
                code['/'] = StandardCharCode.slash;
                code['0'] = StandardCharCode.zero;
                code['1'] = StandardCharCode.one;
                code['2'] = StandardCharCode.two;
                code['3'] = StandardCharCode.three;
                code['4'] = StandardCharCode.four;
                code['5'] = StandardCharCode.five;
                code['6'] = StandardCharCode.six;
                code['7'] = StandardCharCode.seven;
                code['8'] = StandardCharCode.eight;
                code['9'] = StandardCharCode.nine;
                code[':'] = StandardCharCode.colon;
                code[';'] = StandardCharCode.semicolon;
                code['<'] = StandardCharCode.less;
                code['='] = StandardCharCode.mathequal;
                code['>'] = StandardCharCode.greater;
                code['?'] = StandardCharCode.question;
                code['@'] = StandardCharCode.congruent;
                code['A'] = StandardCharCode.Alpha;
                code['B'] = StandardCharCode.Beta;
                code['C'] = StandardCharCode.Chi;
                code['D'] = StandardCharCode.Delta;
                code['E'] = StandardCharCode.Epsilon;
                code['F'] = StandardCharCode.Phi;
                code['G'] = StandardCharCode.Gamma;
                code['H'] = StandardCharCode.Eta;
                code['I'] = StandardCharCode.Iota;
                code['K'] = StandardCharCode.Kappa;
                code['L'] = StandardCharCode.Lambda;
                code['M'] = StandardCharCode.Mu;
                code['N'] = StandardCharCode.Nu;
                code['O'] = StandardCharCode.Omicron;
                code['P'] = StandardCharCode.Pi;
                code['Q'] = StandardCharCode.Theta;
                code['R'] = StandardCharCode.Rho;
                code['S'] = StandardCharCode.Sigma;
                code['T'] = StandardCharCode.Tau;
                code['U'] = StandardCharCode.Upsilon;
                code['V'] = StandardCharCode.varsigma;
                code['W'] = StandardCharCode.Omega;
                code['X'] = StandardCharCode.Xi;
                code['Y'] = StandardCharCode.Psi;
                code['Z'] = StandardCharCode.Zeta;
                code['['] = StandardCharCode.bracketleft;
                code['\\'] = StandardCharCode.backslash;
                code[']'] = StandardCharCode.bracketright;
                code['^'] = StandardCharCode.asciicircum;
                code['_'] = StandardCharCode.underscore;
                code['`'] = StandardCharCode.quoteleft;
                code['a'] = StandardCharCode.alpha;
                code['b'] = StandardCharCode.beta;
                code['c'] = StandardCharCode.chi;
                code['d'] = StandardCharCode.delta;
                code['e'] = StandardCharCode.epsilon;
                code['f'] = StandardCharCode.phi;
                code['g'] = StandardCharCode.gamma;
                code['h'] = StandardCharCode.eta;
                code['i'] = StandardCharCode.iota;
                code['k'] = StandardCharCode.kappa;
                code['l'] = StandardCharCode.lambda;
                code['m'] = StandardCharCode.mu;
                code['n'] = StandardCharCode.nu;
                code['o'] = StandardCharCode.omicron;
                code['p'] = StandardCharCode.pi;
                code['q'] = StandardCharCode.theta;
                code['r'] = StandardCharCode.rho;
                code['s'] = StandardCharCode.sigma;
                code['t'] = StandardCharCode.tau;
                code['u'] = StandardCharCode.upsilon;
                code['w'] = StandardCharCode.omega;
                code['x'] = StandardCharCode.xi;
                code['y'] = StandardCharCode.psi;
                code['z'] = StandardCharCode.zeta;
                code['{'] = StandardCharCode.braceleft;
                code['|'] = StandardCharCode.bar;
                code['}'] = StandardCharCode.braceright;
                code['~'] = StandardCharCode.mathtilde;

                return code;
            }
        }

        #endregion	// Public Static Methods
    }

}