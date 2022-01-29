using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace EBookCrawler.Texting
{
    public class LatexWriter : StreamWriter
    {
        public int Depth => Styles.Count;
        public bool StartWithCapital { get; set; }
        public Stack<Style> Styles { get; set; } = new Stack<Style>();
        public Style CurrentStyle { get; set; }

        public LatexWriter(string path) : base(path)
        {
        }

        public void WritePreamble()
        {
            WriteLine(@"\usepackage{lettrine}");
            WriteLine(@"\usepackage[document]{ragged2e}");
            WriteLine(@"\usepackage{xcolor}");
            WriteLine(@"\usepackage{yfonts}");
            WriteLine(@"\usepackage{ulem}");
            WriteLine(@"\usepackage{changepage}");
        }
        public override void WriteLine()
        {
            base.WriteLine("%");
        }
        public override void WriteLine(string line)
        {
            base.Write(line);
            this.WriteLine();
        }
        public void WriteLineBreak(int n)
        {
            for (int i = 0; i < n; i++)
                WriteLineBreak();
        }
        public void WriteLineBreak()
        {
            base.WriteLine();
        }
        public void WriteAlignment(int alignment)
        {
            switch (alignment)
            {
                case 0:
                    WriteLine(@"\justifying");
                    break;
                case 1:
                    WriteLine(@"\Centering");
                    break;
                case 2:
                    WriteLine(@"\RaggedRight");
                    break;
                default:
                    throw new NotImplementedException();
            }
        }
        public void WriteSize(int size)
        {
            switch (size)
            {
                case -4:
                    WriteLine(@"\tiny");
                    break;
                case -3:
                    WriteLine(@"\scriptsize");
                    break;
                case -2:
                    WriteLine(@"\footnotesize");
                    break;
                case -1:
                    WriteLine(@"\small");
                    break;
                case 0:
                    WriteLine(@"\normalsize");
                    break;
                case 1:
                    WriteLine(@"\large");
                    break;
                case 2:
                    WriteLine(@"\Large");
                    break;
                case 3:
                    WriteLine(@"\LARGE");
                    break;
                case 4:
                    WriteLine(@"\huge");
                    break;
                case 5:
                    WriteLine(@"\Huge");
                    break;
                default:
                    if (size > 0)
                        WriteLine(@"\Huge");
                    else
                        WriteLine(@"\tiny");
                    break;
            }
        }
        public void WriteColor(Color color)
        {
            Write(@"\color[rgb]{");
            Write(color.Red / 255.0);
            Write(", ");
            Write(color.Green / 255.0);
            Write(", ");
            Write(color.Blue / 255.0);
            WriteLine("}");
        }
        public void PushStyle(Style style)
        {
            CurrentStyle |= style;
            Styles.Push(style);
        }
        public void PopStyle()
        {
            Styles.Pop();
            CurrentStyle = new Style();
            foreach (var item in Styles)
                CurrentStyle |= item;
        }
        public void WriteStyle()
        {
            if (CurrentStyle.IsBold)
                WriteLine(@"\bfseries");
            if (CurrentStyle.IsEmphasis)
                WriteLine(@"\em");
                //WriteLine(@"\slshape");
            if (CurrentStyle.IsFraktur)
                WriteLine(@"\frakfamily");
            if (CurrentStyle.IsItalic)
                WriteLine(@"\itshape");
            if (CurrentStyle.IsMonoSpace)
                WriteLine(@"\ttfamily");
            if (CurrentStyle.IsSmallCaps)
                WriteLine(@"\scshape");
        }
        public void WriteText(string text)
        {
            void writeWord(string word)
            {
                if (CurrentStyle.IsCrossedOut)
                    Write(@"\sout{");
                if (CurrentStyle.IsUnderlined)
                    Write(@"\uline{");
                if (CurrentStyle.IsOverlined)
                    Write(@"$\overline{\text{");

                if (CurrentStyle.IsWide)
                    foreach (var c in word)
                    {
                        Write(c);
                        Write(@"\,");
                    }
                else
                    Write(word);

                if (CurrentStyle.IsOverlined)
                    Write(@"}}$");
                if (CurrentStyle.IsCrossedOut)
                    Write(@"}");
                if (CurrentStyle.IsUnderlined)
                    Write(@"}");
            }

            if (CurrentStyle.IsUpper)
                text = text.ToUpper();
            else if (CurrentStyle.IsLower)
                text = text.ToLower();

            var words = ProcessWords(text);
            var iter = words.GetEnumerator();
            if (StartWithCapital)
            {
                while (iter.MoveNext())
                {
                    if (!iter.Current.isWhitespace)
                    {
                        StartWithCapital = false;
                        Write(@"\lettrine{");
                        writeWord(iter.Current.word[0].ToString());
                        Write(@"}{");
                        writeWord(iter.Current.word.Substring(1));
                        Write(@"}");
                        break;
                    }
                    Write(iter.Current.word);
                }
            }
            while (iter.MoveNext())
                if (iter.Current.isWhitespace)
                    Write(iter.Current.word);
                else
                    writeWord(iter.Current.word);
        }
        private static IEnumerable<(string word, bool isWhitespace)> ProcessWords(string text)
        {
            StringBuilder sb = new StringBuilder();
            foreach (var c in text)
                if (IsWhitespace(c))
                {
                    if (sb.Length > 0)
                    {
                        yield return (sb.ToString(), false);
                        sb.Clear();
                    }
                    yield return (ReplaceCharacter(c), true);

                }
                else
                    sb.Append(ReplaceCharacter(c));
            if (sb.Length > 0)
            {
                yield return (sb.ToString(), false);
                sb.Clear();
            }
        }
        public static bool IsWhitespace(char c)
        {
            return c == ' ' || c == '\n' || c == '\t' || c == '\r' || c == '\u00a0';
        }
        public static string ReplaceCharacter(char c)
        {
            switch (c)
            {
                case '&':
                    return @"\&{}";
                case '%':
                    return @"\%{}";
                case '$':
                    return @"\${}";
                case '#':
                    return @"\#{}";
                case '_':
                    return @"\_{}";
                case '{':
                    return @"\{{}";
                case '}':
                    return @"\}{}";
                case '~':
                    return @"\}textasciitilde{}";
                case '^':
                    return @"\}textasciicircum{}";
                case '\\':
                    return @"\textbackslash{}";
                case '\u00a0': //Non breaking Space
                    return "~";
                default:
                    return c.ToString();
            }
        }
    }
}
