using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBookCrawler.Parsing
{
    public class Parser
    {
        public string Text;
        public int CurrentPosition;
        public int LineNumber;
        public int PositionInLine;
        public char CurrentSymbol;
        public bool EoF = false;

        public bool FoundError = false;
        public string ErrorMessage;

        public List<HTMLElement> HTMLElements;

        private void Reset(string Text)
        {
            this.HTMLElements = new List<HTMLElement>();
            CurrentPosition = 0;
            LineNumber = 0;
            PositionInLine = 0;
            this.Text = Text;
            this.EoF = CurrentPosition >= Text.Length;
            if (!EoF)
                CurrentSymbol = Text[CurrentPosition];
        }

        private void Next()
        {
            CurrentPosition++;
            PositionInLine++;
            this.EoF = CurrentPosition >= Text.Length;
            if (CurrentSymbol == '\n')
            {
                LineNumber++;
                PositionInLine = 0;
            }
            if (!EoF)
                CurrentSymbol = Text[CurrentPosition];
            else
                CurrentSymbol = (char)0;
        }
        private bool CanContinue() => !EoF && !FoundError;

        public void WriteError(string message)
        {
            this.FoundError = true;
            this.ErrorMessage = message;
        }

        public void Parse(string Text)
        {
            Reset(Text);

            SkipWhiteSpaces();
            while (CanContinue())
            {
                ParseHTMLElement();
                if (FoundError)
                    return;
                SkipWhiteSpaces();
            }
        }
        private void ParseHTMLElement()
        {
            if (CurrentSymbol != '<')
            {
                WriteError("Expected <");
                return;
            }
            var element = new HTMLElement
            {
                SelfClosed = false,
                Attributes = new List<(string name, string value)>(),
                Position = CurrentPosition,
                LineNumber = LineNumber,
                PositionInLine = PositionInLine
            };
            Next();
            element.Tag = RetrieveTagName();
            if (FoundError)
                return;

            while (CanContinue())
            {
                SkipWhiteSpaces();

                if (CurrentSymbol == '>')
                {
                    Next();
                    break;
                }
                if (CurrentSymbol == '/')
                {
                    if (CurrentPosition < Text.Length - 1 && Text[CurrentPosition + 1] == '>')
                    {
                        Next();
                        Next();
                        element.SelfClosed = true;
                        break;
                    }
                    else if(element.Tag =="")
                    {
                        WriteError("Overlapping HTML Tags detected");
                        return;
                    }
                    else
                    {
                        WriteError("Expected />");
                        return;
                    }
                }
                var att = RetrieveAttribute();
                if (FoundError)
                    return;
                element.Attributes.Add(att);
            }
            if (Text[CurrentPosition - 1] != '>')
            {
                WriteError("Current Tag not closed");
                return;
            }

            if (element.SelfClosed)
                element.Raw = "";
            else
                element.Raw = CloseTag(element.Tag);
            HTMLElements.Add(element);
        }
        private string CloseTag(string tag)
        {
            bool isClosed = false;
            bool checkIfClosingTag()
            {
                if (CurrentSymbol != '<')
                    return false;
                int pointer = CurrentPosition + 1;
                if (pointer >= Text.Length)
                {
                    WriteError("File ended before Current Tag was closed");
                    return false;
                }
                if (Text[pointer] != '/')
                    return false;
                pointer++;
                if (pointer + tag.Length > Text.Length)
                {
                    WriteError("File ended before Current Tag was closed");
                    return false;
                }
                if (Text.Substring(pointer, tag.Length) != tag)
                    return false;
                pointer += tag.Length;
                while (pointer < Text.Length)
                {
                    if (Text[pointer] == '>')
                    {
                        pointer++;
                        while (CurrentPosition < pointer)
                            Next();
                        isClosed = true;
                        return true;
                    }
                    if (IsWhiteSpace(Text[pointer]))
                        pointer++;
                    else
                        return false;
                }
                WriteError("File ended before Current Tag was closed");
                return false;
            }

            int start = CurrentPosition;
            int length = 0;
            while (CanContinue())
            {
                if (checkIfClosingTag())
                    break;
                if (FoundError)
                    return "";
                length++;
                Next();
            }
            if (!isClosed)
            {
                WriteError("File ended before Current Tag was closed");
                return "";
            }

            return Text.Substring(start, length);
        }
        private (string name, string value) RetrieveAttribute()
        {
            string name = RetrieveAttributeName();
            if (FoundError)
                return ("", "");
            SkipWhiteSpaces();

            if (CurrentSymbol != '=')
            {
                WriteError("Expected =");
                return (name, "");
            }
            Next();
            string value = RetrieveValue();
            return (name, value);
        }
        private string RetrieveValue()
        {
            SkipWhiteSpaces();
            if (CurrentSymbol != '"')
            {
                WriteError("Expected \"");
                return "";
            }
            Next();
            int start = CurrentPosition;
            int length = 0;

            while (CanContinue())
            {
                if (CurrentSymbol == '\"')
                {
                    Next();
                    break;
                }
                length++;
                Next();
            }
            if (Text[CurrentPosition - 1] != '"')
            {
                WriteError("Current Quotation not closed");
                return "";
            }
            return Text.Substring(start, length);
        }
        private string RetrieveAttributeName()
        {
            SkipWhiteSpaces();
            int start = CurrentPosition;
            int length = 0;
            while (!EoF)
            {
                if (IsLiteral() || IsDigit())
                {
                    Next();
                    length++;
                }
                else if (IsWhiteSpace())
                {
                    Next();
                    break;
                }
                else if (CurrentSymbol == '=')
                    break;
                else
                {
                    WriteError("Expected Literal, digit, = or Whitespace");
                    return "";
                }
            }
            return Text.Substring(start, length);
        }
        private string RetrieveTagName()
        {
            SkipWhiteSpaces();
            int start = CurrentPosition;
            int length = 0;
            while (!EoF)
            {
                if (IsLiteral() || IsDigit())
                {
                    Next();
                    length++;
                }
                else if (IsWhiteSpace())
                {
                    Next();
                    break;
                }
                else if (CurrentSymbol == '>' || CurrentSymbol == '/')
                    break;
                else
                {
                    WriteError("Expected Literal, digit, >, / or Whitespace");
                    return "";
                }
            }
            return Text.Substring(start, length);
        }
        private void SkipWhiteSpaces()
        {
            while (CanContinue())
            {
                if (IsWhiteSpace())
                    Next();
                else
                    break;
            }
        }

        private bool IsWhiteSpace() => IsWhiteSpace(CurrentSymbol);
        private bool IsWhiteSpace(char c) => (c == ' ' || c == '\n' || c == '\r' || c == '\t');
        private bool IsLiteral() => IsLiteral(CurrentSymbol);
        private bool IsLiteral(char c) => IsSmallLiteral(c) || IsLargeLiteral(c);
        private bool IsSmallLiteral() => IsSmallLiteral(CurrentSymbol);
        private bool IsSmallLiteral(char c) => 'a' <= c && c <= 'z';
        private bool IsLargeLiteral() => IsLargeLiteral(CurrentSymbol);
        private bool IsLargeLiteral(char c) => 'A' <= c && c <= 'Z';
        private bool IsDigit() => IsDigit(CurrentSymbol);
        private bool IsDigit(char c) => '0' <= c && c <= '9';

        public string GetState()
        {
            StringBuilder sb = new StringBuilder();
            if (FoundError)
                sb.AppendLine("Error Found At:");
            sb.AppendLine("Position " + CurrentPosition + ", Line " + LineNumber + ", Pos in Line " + PositionInLine);
            sb.AppendLine("TextLength " + Text.Length);

            if (FoundError)
            {
                sb.AppendLine();
                sb.AppendLine(ErrorMessage);
            }
            sb.AppendLine();
            foreach (var item in HTMLElements)
                sb.AppendLine(item.ToString());

            return sb.ToString();
        }
    }
}
