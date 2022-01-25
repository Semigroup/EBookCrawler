using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBookCrawler.Texting
{
    public struct Color
    {
        public byte Red;
        public byte Green;
        public byte Blue;

        public Color(byte Red, byte Green, byte Blue)
        {
            this.Red = Red;
            this.Green = Green;
            this.Blue = Blue;
        }
        public Color(string hex)
        {
            int getValue(char hexDigit)
            {
                if ('a' <= hexDigit && hexDigit <= 'f')
                    return 10 + hexDigit - 'a';
                if ('0' <= hexDigit && hexDigit <= '9')
                    return hexDigit - '0';
                throw new NotImplementedException();
            }
            byte getByte(char high, char low)
                => (byte)(16 * getValue(high) + getValue(low));

            hex = hex.ToLower();
            if (hex.Substring(0, 2) == "0x")
                hex = hex.Substring(2);
            else if (hex.Substring(0, 1) == "#")
                hex = hex.Substring(1);
            this.Red = getByte(hex[0], hex[1]);
            this.Green = getByte(hex[2], hex[3]);
            this.Blue = getByte(hex[4], hex[5]);
        }

        public bool IsBlack() => Red == 0 && Green == 0 && Blue == 0;
    }
}
