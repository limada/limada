/*
 * Limaki 
 * Version 0.064
 * 
 * This code is free software; you can redistribute it and/or modify it
 * under the terms of the license below.
 *
 * Changes:
 * Adopted to work with RectangleF and PointF
 * Generic Items introduced
 * 
 * Author of changes: Lytico
 *
 * http://limada.sourceforge.net
 * 
 */

/* NetTopologySuite is a collection of .NET classes written in C# that
implement the fundamental operations required to validate a given
geo-spatial data set to a known topological specification.

This collection of classes is a porting (with some additions and modifications) of 
JTS Topology Suite (see next license for more informations).

Copyright (C) 2005 Diego Guidi

This library is free software; you can redistribute it and/or
modify it under the terms of the GNU Lesser General Public
License as published by the Free Software Foundation; either
version 2.1 of the License, or (at your option) any later version.

This library is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
Lesser General Public License for more details.

You should have received a copy of the GNU Lesser General Public
License along with this library; if not, write to the Free Software
Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA

For more information, contact:

    Diego Guidi
    via Po 15
	61031 Cuccurano di Fano (PU)
    diegoguidi@libero.it
    http://blogs.ugidotnet.org/gissharpblog

*/


using System;

namespace Limaki.Drawing.Indexing.QuadTrees {
    /// <summary>
    /// 
    /// </summary>
    public class HexConverter {
        /// <summary>
        /// Only static methods!
        /// </summary>
        private HexConverter() { }

        /// <summary>
        /// Convert the given numeric value (passed as string) of the base specified by <c>baseIn</c>
        /// to the value specified by <c>baseOut</c>.
        /// </summary>
        /// <param name="valueIn">Numeric value to be converted, as string.</param>
        /// <param name="baseIn">Base of input value.</param>
        /// <param name="baseOut">Base to use for conversion.</param>
        /// <returns>Converted value, as string.</returns>
        public static string ConvertAny2Any(string valueIn, int baseIn, int baseOut) {
            string result = "Error";

            valueIn = valueIn.ToUpper();
            const string codice = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ";

            // test per limite errato sulle basi in input e/o in output
            if ((baseIn < 2) || (baseIn > 36) ||
                 (baseOut < 2) || (baseOut > 36))
                return result;

            if (valueIn.Trim().Length == 0)
                return result;

            // se baseIn e baseOut sono uguali la conversione è già fatta!
            if (baseIn == baseOut)
                return valueIn;

            // determinazione del valore totale
            double valore = 0;
            try {
                // se il campo è in base 10 non c'è bisogno di calcolare il valore
                if (baseIn == 10)
                    valore = double.Parse(valueIn);
                else {
                    char[] c = valueIn.ToCharArray();

                    // mi serve per l'elevazione a potenza e la trasformazione
                    // in valore base 10 della cifra
                    int posizione = c.Length;

                    // ciclo sui caratteri di valueIn
                    // calcolo del valore decimale

                    for (int k = 0; k < c.Length; k++) {
                        // valore posizionale del carattere
                        int valPos = codice.IndexOf(c[k]);

                        // verifica per caratteri errati
                        if ((valPos < 0) || (valPos > baseIn - 1))
                            return result;

                        posizione--;
                        valore += valPos * Math.Pow((double)baseIn, (double)posizione);
                    }
                }

                // generazione del risultato final
                // se il risultato da generare è in base 10 non c'è
                // bisogno di calcoli
                if (baseOut == 10)
                    result = valore.ToString();

                else {
                    result = String.Empty;
                    while (valore > 0) {
                        int resto = (int)(valore % baseOut);
                        valore = (valore - resto) / baseOut;
                        result = codice.Substring(resto, 1) + result;
                    }
                }

            } catch (Exception ex) {
                result = ex.Message;
            }
            return result;
        }
    }
}
