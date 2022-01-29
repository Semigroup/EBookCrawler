using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBookCrawler.Texting
{
    public struct Style
    {
        public bool IsBold;
        public bool IsItalic;
        public bool IsEmphasis;
        public bool IsSmallCaps;
        public bool IsMonoSpace;
        public bool IsFraktur;

        public bool IsUpper;
        public bool IsLower;

        public bool IsUnderlined;
        public bool IsOverlined;
        public bool IsCrossedOut;

        public bool IsWide;

        public static Style operator |(Style style1, Style style2)
               => new Style()
               {
                   IsBold = style1.IsBold || style2.IsBold,
                   IsItalic = style1.IsItalic || style2.IsItalic,
                   IsEmphasis = style1.IsEmphasis || style2.IsEmphasis,
                   IsUnderlined = style1.IsUnderlined || style2.IsUnderlined,
                   IsOverlined = style1.IsOverlined || style2.IsOverlined,
                   IsCrossedOut = style1.IsCrossedOut || style2.IsCrossedOut,
                   IsUpper = style1.IsUpper || style2.IsUpper,
                   IsLower = style1.IsLower || style2.IsLower,
                   IsSmallCaps = style1.IsSmallCaps || style2.IsSmallCaps,
                   IsMonoSpace = style1.IsMonoSpace || style2.IsMonoSpace,
                   IsWide = style1.IsWide || style2.IsWide,
                   IsFraktur = style1.IsFraktur || style2.IsFraktur
               };
    }
}
