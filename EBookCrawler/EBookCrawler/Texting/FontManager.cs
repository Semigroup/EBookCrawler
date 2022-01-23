using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBookCrawler.Texting
{
    public class FontManager
    {
        public struct Mode
        {
            public bool IsBold;
            public bool IsItalic;
            public bool IsEmphasis;
            public bool IsUnderlined;
            public bool IsCrossedOut;
            public bool IsUpper;
            public bool IsLower;
            public bool IsSmallCaps;
            public bool IsMonoSpace;
            public bool IsFootnote;
            public bool IsSuper;
            public bool IsSub;
            public bool IsWide;

            public bool ChangeColor;
            public byte ColorRed;
            public byte ColorGreen;
            public byte ColorBlue;

            public int Size;

            public string ToolTip;

            public void SetColor(string hex)
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
                this.ColorRed = getByte(hex[0], hex[1]);
                this.ColorGreen = getByte(hex[2], hex[3]);
                this.ColorBlue = getByte(hex[4], hex[5]);
            }

            public void ApplyDelta(Mode delta)
            {
                this.IsBold |= delta.IsBold;
                this.IsItalic |= delta.IsItalic;
                this.IsEmphasis |= delta.IsEmphasis;
                this.IsUnderlined |= delta.IsUnderlined;
                this.IsCrossedOut |= delta.IsCrossedOut;
                this.IsUpper |= delta.IsUpper;
                this.IsLower |= delta.IsLower;
                this.IsSmallCaps |= delta.IsSmallCaps;
                this.IsMonoSpace |= delta.IsMonoSpace;
                this.IsFootnote |= delta.IsFootnote;
                this.IsSub |= delta.IsSub;
                this.IsSuper |= delta.IsSuper;
                this.IsWide |= delta.IsWide;

                if (delta.ChangeColor)
                {
                    this.ChangeColor = true;
                    this.ColorRed = delta.ColorRed;
                    this.ColorGreen = delta.ColorGreen;
                    this.ColorBlue = delta.ColorBlue;
                }

                this.Size += delta.Size;
            }
        }

        public Mode CurrentMode { get; set; }
        public List<(string tag, Mode delta)> Deltas { get; set; } = new List<(string tag, Mode delta)>();

        public void AddBold()
        {
            Add("b", new Mode() { IsBold = true });
        }
        public void AddItalic()
        {
            Add("i", new Mode() { IsItalic = true });
        }
        public void AddTeleType()
        {
            Add("tt", new Mode() { IsMonoSpace = true });
        }
        public void AddEmphasis()
        {
            Add("em", new Mode() { IsEmphasis = true });
        }
        public void AddSuper()
        {
            Add("sup", new Mode() { IsSuper = true });
        }
        public void AddSub()
        {
            Add("sub", new Mode() { IsSub = true });
        }
        public void AddBig()
        {
            Add("big", new Mode() { Size = 1 });
        }
        public void AddSmall()
        {
            Add("small", new Mode() { Size = -1 });
        }
        public void Add(string tag, Mode delta)
        {
            Deltas.Add((tag, delta));
            CurrentMode.ApplyDelta(delta);
        }
        public Mode Remove(string tag)
        {
            int index = Deltas.FindLastIndex(x => x.tag == tag);
            Mode m = Deltas[index].delta;
            Deltas.RemoveAt(index);
            CurrentMode = new Mode();
            foreach (var item in Deltas)
                CurrentMode.ApplyDelta(item.delta);
            return m;
        }
    }
}
