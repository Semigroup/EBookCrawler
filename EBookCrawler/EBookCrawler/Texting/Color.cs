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
        public Color(string value)
        {
            string hex = value.ToLower();
            switch (hex)
            {
                case "red":
                    Red = 255;
                    Green = 0;
                    Blue = 0;
                    break;
                case "green":
                    Red = 0;
                    Green = 255;
                    Blue = 0;
                    break;
                case "blue":
                    Red = 0;
                    Green = 0;
                    Blue = 255;
                    break;
                case "gray":
                    Red = Green = Blue = 160;
                    break;

                default:
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

                    if (hex.Substring(0, 2) == "0x")
                        hex = hex.Substring(2);
                    else if (hex.Substring(0, 1) == "#")
                        hex = hex.Substring(1);
                    this.Red = getByte(hex[0], hex[1]);
                    this.Green = getByte(hex[2], hex[3]);
                    this.Blue = getByte(hex[4], hex[5]);
                    break;
            }
        }

        public bool IsBlack() => Red == 0 && Green == 0 && Blue == 0;
    }
}
