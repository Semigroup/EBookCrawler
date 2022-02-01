﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace EBookCrawler.Texting
{
    public class LatexWriter : StreamWriter
    {
        public bool StartWithCapital { get; set; }
        public Stack<Style> Styles { get; set; } = new Stack<Style>();
        public Style CurrentStyle { get; set; }
        public string BuildRoot { get; set; }
        public string BuildDirectory { get; set; }
        private bool LineIsEmpty = true;
        public int TabularDepth { get; set; } = 0;

        public LatexWriter(string BuildRoot, string path) : base(path)
        {
            this.BuildRoot = BuildRoot;
            this.BuildDirectory = Path.GetDirectoryName(path) + "\\";
        }

        public void WritePreamble()
        {
            WriteLine(@"\documentclass[12pt,a4paper,oneside]{book}");

            WriteLine(@"\usepackage[ngerman]{babel}");
            WriteLine(@"\usepackage[T1]{fontenc}");
            WriteLine(@"\usepackage[utf8]{inputenc}");

            WriteLine(@"\usepackage{lettrine}");
            WriteLine(@"\usepackage[document]{ragged2e}");
            WriteLine(@"\usepackage{xcolor}");
            WriteLine(@"\usepackage{yfonts}");
            WriteLine(@"\usepackage{ulem}");
            WriteLine(@"\usepackage{changepage}");
            WriteLine(@"\usepackage{amsmath}");
            WriteLine(@"\usepackage{amssymb}");
            WriteLine(@"\usepackage{amsthm}");
            WriteLine(@"\usepackage{amsfonts}");
            WriteLine(@"\usepackage{mathtools}");
            WriteLine(@"\usepackage{wasysym}");
            WriteLine(@"\usepackage{tabularx}");
            WriteLine(@"\usepackage{lmodern}");
        }
        public override void Write(string value)
        {
            base.Write(value);
            LineIsEmpty &= value.Length == 0;
        }
        public override void WriteLine()
        {
            if (!LineIsEmpty)
            {
                base.WriteLine("%");
                LineIsEmpty = true;
            }
        }
        public override void WriteLine(string line)
        {
            this.Write(line);
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
            LineIsEmpty = true;
        }
        public void WriteAlignment(TextElement.Alignment alignment)
        {
            switch (alignment)
            {
                case TextElement.Alignment.Unspecified:
                    break;
                case TextElement.Alignment.Left:
                    Write(@"\RaggedLeft");
                    break;
                case TextElement.Alignment.Center:
                    Write(@"\Centering");
                    break;
                case TextElement.Alignment.Right:
                    Write(@"\RaggedRight");
                    break;
                default:
                    throw new NotImplementedException();
            }
        }
        public void WriteBeginAlignment(TextElement.Alignment alignment)
        {
            switch (alignment)
            {
                case TextElement.Alignment.Unspecified:
                    break;
                case TextElement.Alignment.Left:
                    Write(@"\begin{flushleft}");
                    break;
                case TextElement.Alignment.Center:
                    Write(@"\begin{center}");
                    break;
                case TextElement.Alignment.Right:
                    Write(@"\begin{flushright}");
                    break;
                default:
                    throw new NotImplementedException();
            }
        }
        public void WriteEndAlignment(TextElement.Alignment alignment)
        {
            switch (alignment)
            {
                case TextElement.Alignment.Unspecified:
                    break;
                case TextElement.Alignment.Left:
                    Write(@"\end{flushleft}");
                    break;
                case TextElement.Alignment.Center:
                    Write(@"\end{center}");
                    break;
                case TextElement.Alignment.Right:
                    Write(@"\end{flushright}");
                    break;
                default:
                    throw new NotImplementedException();
            }
        }
        public void WriteSize(int? size)
        {
            if (!size.HasValue)
                return;
            switch (size)
            {
                case -4:
                    Write(@"\tiny");
                    break;
                case -3:
                    Write(@"\scriptsize");
                    break;
                case -2:
                    Write(@"\footnotesize");
                    break;
                case -1:
                    Write(@"\small");
                    break;
                case 0:
                    Write(@"\normalsize");
                    break;
                case 1:
                    Write(@"\large");
                    break;
                case 2:
                    Write(@"\Large");
                    break;
                case 3:
                    Write(@"\LARGE");
                    break;
                case 4:
                    Write(@"\huge");
                    break;
                case 5:
                    Write(@"\Huge");
                    break;
                default:
                    if (size > 0)
                        Write(@"\Huge");
                    else
                        Write(@"\tiny");
                    break;
            }
        }
        public void WriteColor(Color? color)
        {
            if (!color.HasValue)
                return;

            Write(@"\color[rgb]{");
            Write(color.Value.Red / 255.0);
            Write(", ");
            Write(color.Value.Green / 255.0);
            Write(", ");
            Write(color.Value.Blue / 255.0);
            Write("}");
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
                Write(@"\bfseries");
            if (CurrentStyle.IsEmphasis)
                Write(@"\em");
            //WriteLine(@"\slshape");
            if (CurrentStyle.IsFraktur)
                Write(@"\frakfamily");
            if (CurrentStyle.IsItalic)
                Write(@"\itshape");
            if (CurrentStyle.IsMonoSpace)
                Write(@"\ttfamily");
            if (CurrentStyle.IsSmallCaps)
                Write(@"\scshape");
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
            void writeLettrine(string word)
            {
                Write(@"\lettrine{");
                writeWord(word[0].ToString());
                Write(@"}{");
                writeWord(word.Substring(1));
                Write(@"}");
            }

            if (CurrentStyle.IsUpper)
                text = text.ToUpper();
            else if (CurrentStyle.IsLower)
                text = text.ToLower();

            if (IsWhitespace(text[0]))
                Write(" ");
            var words = ProcessWords(text);
            bool initial = true;
            foreach (var word in words)
                if (initial)
                {
                    initial = false;
                    if (StartWithCapital)
                    {
                        StartWithCapital = false;
                        writeLettrine(word);
                    }
                    else
                        writeWord(word);
                }
                else
                {
                    Write(" ");
                    writeWord(word);
                }
            if (IsWhitespace(text[text.Length - 1]))
                Write(" ");
        }
        private static IEnumerable<string> ProcessWords(string text)
        {
            StringBuilder sb = new StringBuilder();
            foreach (var c in text)
                if (IsWhitespace(c))
                {
                    if (sb.Length > 0)
                    {
                        yield return sb.ToString();
                        sb.Clear();
                    }
                }
                else
                    sb.Append(ReplaceCharacter(c));
            if (sb.Length > 0)
                yield return sb.ToString();
        }
        public static bool IsWhitespace(char c)
        {
            return c == ' ' || c == '\n' || c == '\t' || c == '\r';// || c == '\u00a0';
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
