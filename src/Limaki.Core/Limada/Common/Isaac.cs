/*
 * Limada 
 
 * 
 * This code is free software; you can redistribute it and/or modify it
 * under the terms of the GNU General Public License version 2 only, as
 * published by the Free Software Foundation.
 * 
 * Author: Lytico
 * Copyright (C) 2006-2011 Lytico
 *
 * http://limada.sourceforge.net
 * 
 */

using System.Security.Cryptography;

namespace Limaki.Common {
    ///<summary>
    /// *****************************
    /// ISAAC Random Number Generator
    /// *****************************
    ///	ISAAC (Indirection, Shift, Accumulate, Add, and Count) is a cryptographically secure pseudo random number generator.
    ///	With an average cycle length of 2 to the 8295th power its output is uniformly distributed and unpredictable.
    ///	ISAAC has been developed by
    ///	***********
    ///	Bob Jenkins
    ///	***********
    ///	and placed into the public domain in 1996.
    /// </summary>
    ///  <stereotype>utility</stereotype>
    public class Isaac {

        private uint[] rsl;
        private uint[] mem;
        private int count;
        private uint aa;
        private uint bb;
        private uint cc;

        public Isaac( ) {
            rsl = new uint[256];
            mem = new uint[256];
            reSeed();
        }

        public Isaac( uint[] seed ):this() {
            reSeed(seed);
        }

        /// <summary>
        /// newSeed generates a 512 Byte-Seed out of c#-SecureRandom (SHA1PRNG)
        /// newSeed is essential for generating Unique Numbers.
        /// </summary>
        public void newSeed( ) {
//			SecureRandom random=null;
//			try {
//				random = SecureRandom.getInstance("SHA1PRNG");
//			} catch (NoSuchAlgorithmException e) {
//				// TODO Automatisch erstellter Catch-Block
//				e.printStackTrace();
//			}
//
//			for (int i = 0; i < rsl.Length; i++)
//				rsl[i] = random.nextInt();

            byte[] random = new byte[512];

            //RNGCryptoServiceProvider is an implementation of a random number generator.
            RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
            rng.GetBytes(random); // The array is now filled with cryptographically strong random bytes.
            for (int i= 0;i<random.Length;i+=2){
                uint rs = random[i];
                rs = rs << 8;
                rs += random[i+1];
                rsl[((int)((i+1)/2))]=rs;
                // TODO: Mix in Current Date Time to avoid bad Seed
            }
            this.reSeed(rsl);
        }
        /// <summary>
        /// reSeed fills the Seed with 0
        /// You should never use reSeed, its just for testing between Languages / Platform compatibility
        /// </summary>
        public void reSeed( ) {
            int i;
            for (i = 0; i < rsl.Length; i++)
                rsl[i] = 0;
            for (i = 0; i < mem.Length; i++)
                mem[i] = 0;
            init(false);
        }
        /// <summary>
        /// If you want to use your own seed
        /// </summary>
        /// <param name="seed"></param>
        public void reSeed( uint[] seed ) {
            int i;
            int m = seed.Length;
            for (i = 0; i < mem.Length; i++)
                mem[i] = 0;
            for (i = 0; i < rsl.Length; i++) {
                if (i < m)
                    rsl[i] = seed[i];
                else
                    rsl[i] = 0;
            }
            init(true);

        }
        private void init( bool flag ) {
            int i;
            uint a, b, c, d, e, f, g, h;
            aa = 0;
            bb = 0;
            cc = 0;
            a = 0x9e3779b9;
            b = c = d = e = f = g = h = a;
            for (i = 0; i <= 3; i++) {
                a = a ^ (b << 11);
                d = d + a;
                b = b + c; /* mix a,b,c,d,e,f,g and h */
                b = b ^ (c >> 2);
                e = e + b;
                c = c + d;
                c = c ^ (d << 8);
                f = f + c;
                d = d + e;
                d = d ^ (e >> 16);
                g = g + d;
                e = e + f;
                e = e ^ (f << 10);
                h = h + e;
                f = f + g;
                f = f ^ (g >> 4);
                a = a + f;
                g = g + h;
                g = g ^ (h << 8);
                b = b + g;
                h = h + a;
                h = h ^ (a >> 9);
                c = c + h;
                a = a + b;
            }

            i = 0;
            while (i < 256) /*fill in mem[] with messy stuff */ {
                if (flag) /* use all the information in the seed */ {
                    a = a + rsl[i];
                    b = b + rsl[i + 1];
                    c = c + rsl[i + 2];
                    d = d + rsl[i + 3];
                    e = e + rsl[i + 4];
                    f = f + rsl[i + 5];
                    g = g + rsl[i + 6];
                    h = h + rsl[i + 7];
                };
                a = a ^ (b << 11);
                d = d + a;
                b = b + c; /* mix a,b,c,d,e,f,g and h */
                b = b ^ (c >> 2);
                e = e + b;
                c = c + d;
                c = c ^ (d << 8);
                f = f + c;
                d = d + e;
                d = d ^ (e >> 16);
                g = g + d;
                e = e + f;
                e = e ^ (f << 10);
                h = h + e;
                f = f + g;
                f = f ^ (g >> 4);
                a = a + f;
                g = g + h;
                g = g ^ (h << 8);
                b = b + g;
                h = h + a;
                h = h ^ (a >> 9);
                c = c + h;
                a = a + b;

                mem[i] = a;
                mem[i + 1] = b;
                mem[i + 2] = c;
                mem[i + 3] = d;
                mem[i + 4] = e;
                mem[i + 5] = f;
                mem[i + 6] = g;
                mem[i + 7] = h;
                i = i + 8;
            };

            if (flag) { /* do a second pass to make all of the seed affect all of mem */
                i = 0;
                while (i < 256) {
                    a = a + mem[i];
                    b = b + mem[i + 1];
                    c = c + mem[i + 2];
                    d = d + mem[i + 3];
                    e = e + mem[i + 4];
                    f = f + mem[i + 5];
                    g = g + mem[i + 6];
                    h = h + mem[i + 7];
                    a = a ^ (b << 11);
                    d = d + a;
                    b = b + c; /* mix a,b,c,d,e,f,g and h */
                    b = b ^ (c >> 2);
                    e = e + b;
                    c = c + d;
                    c = c ^ (d << 8);
                    f = f + c;
                    d = d + e;
                    d = d ^ (e >> 16);
                    g = g + d;
                    e = e + f;
                    e = e ^ (f << 10);
                    h = h + e;
                    f = f + g;
                    f = f ^ (g >> 4);
                    a = a + f;
                    g = g + h;
                    g = g ^ (h << 8);
                    b = b + g;
                    h = h + a;
                    h = h ^ (a >> 9);
                    c = c + h;
                    a = a + b;

                    mem[i] = a;
                    mem[i + 1] = b;
                    mem[i + 2] = c;
                    mem[i + 3] = d;
                    mem[i + 4] = e;
                    mem[i + 5] = f;
                    mem[i + 6] = g;
                    mem[i + 7] = h;
                    i = i + 8;
                }
            }

            isaac(); /* fill in the first set of results */
            count = 0; /* prepare to use the first set of results */

        }
        public void isaac( ) {

            int i;
            uint x, y;

            cc = cc + 1;
            bb = bb + cc;
            for (i = 0; i <= 255; i++) {
                x = mem[i];
                int i3 = (i & 3);
                if (i3 == 0)
                    aa = aa ^ (aa << 13);
                if (i3 == 1)
                    aa = aa ^ (aa >> 6);
                if (i3 == 2)
                    aa = aa ^ (aa << 2);
                if (i3 == 3)
                    aa = aa ^ (aa >> 16);

                aa = aa + mem[(i + 128) & 255];
                y = mem[(x >> 2) & 255] + aa + bb;
                mem[i] = y;
                bb = mem[(y >> 10) & 255] + x;
                rsl[i] = bb;
            }
            count = 0;

        }
        /// <summary>
        /// returns an uint randowm value
        /// </summary>
        /// <returns></returns>
        public uint val( ) {
            uint result = rsl[count];
            count++;
            if (count > 255) {
                isaac();
                count = 0;
            }
            return result;
        }
        /// <summary>
        /// returns an long random value
        /// out of two uint values (val is called twice)
        /// </summary>
        /// <returns></returns>
        public long longval( ) {

            uint ival=this.val();
            long result = 0;
            result = result + ival;
            result = result << 32;
            ival=this.val();
            result = result + ival;
            return result;
        }
        public ulong ulongval( ) {

            uint ival=this.val();
            ulong result = 0;
            result = result + ival;
            result = result << 32;
            ival=this.val();
            result = result + ival;
            return result;
        }

        static Isaac _isaac = null;
        public static Isaac Asaac {
            get {
                if (_isaac == null) {
                    _isaac = new Isaac();
                    _isaac.newSeed();
                }
                return _isaac;
            }
        }
        public static long Long {
            get { return Asaac.longval (); }
        }
    }
}