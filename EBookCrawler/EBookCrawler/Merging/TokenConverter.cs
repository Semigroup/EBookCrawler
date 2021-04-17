using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBookCrawler.Merging
{
   public class TokenConverter
    {
        public static int ToNumber(string token)
        {
            token = token.Replace(".", "").Replace(",", "");

            if (int.TryParse(token, out int result))
                return result;

            switch (token)
            {
                case "erste":
                case "erster":
                case "erstes":
                case "eins":
                case "i":
                    return 1;

                case "zweite":
                case "zweiter":
                case "zweites":
                case "zwei":
                case "ii":
                    return 2;

                case "dritter":
                case "drittes":
                case "dritte":
                case "drei":
                case "iii":
                    return 3;

                case "vierter":
                case "viertes":
                case "vierte":
                case "vier":
                case "iv":
                    return 4;

                case "fünfter":
                case "fünftes":
                case "fünfte":
                case "fünf":
                case "v":
                    return 5;

                case "sechster":
                case "sechstes":
                case "sechste":
                case "sechs":
                case "vi":
                    return 6;

                case "siebter":
                case "siebtes":
                case "siebte":
                case "sieben":
                case "siebentes":
                case "vii":
                    return 7;

                case "achter":
                case "achtes":
                case "achte":
                case "acht":
                case "viii":
                    return 8;

                case "neunter":
                case "neuntes":
                case "neunte":
                case "neun":
                case "ix":
                    return 9;

                case "zehnter":
                case "zehntes":
                case "zehnte":
                case "zehn":
                case "x":
                    return 10;

                case "elfter":
                case "elftes":
                case "elf":
                case "xi":
                    return 11;

                case "zwölfte":
                case "xii":
                    return 12;
                case "dreizehnte":
                case "xiii":
                    return 13;
                case "vierzehnte":
                case "xiv":
                    return 14;
                case "fünfzehnte":
                case "xv":
                    return 15;
                case "sechszehnte":
                case "sechzehnte":
                case "xvi":
                    return 16;
                case "siebenzehnte":
                case "xvii":
                    return 17;
                case "achtzehnte":
                case "xviii":
                    return 18;
                case "neunzehnte":
                case "xix":
                    return 19;
                case "xx":
                    return 20;
                case "xxi":
                    return 21;
                case "xxii":
                    return 22;
                case "xxiii":
                    return 23;
                case "xxiv":
                    return 24;

                default:
                    return -1;
            }
        }
    }
}
