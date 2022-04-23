using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBookCrawler.Parsing
{
    public class Tokenizer
    {
        public string Text;
        public int CurrentPosition;
        public int LineNumber;
        public int PositionInLine;
        public char CurrentSymbol;
        public bool EoF = false;
        public bool FoundError = false;
        public string ErrorMessage;
        public bool IsJavaScript = false;

        public List<Token> Tokens;

        private void Reset(string Text)
        {
            this.IsJavaScript = false;
            this.FoundError = false;
            this.ErrorMessage = null;

            this.Tokens = new List<Token>();
            CurrentPosition = 0;
            LineNumber = 0;
            PositionInLine = 0;
            this.Text = Text;
            this.EoF = CurrentPosition >= Text.Length;
            if (!EoF)
                CurrentSymbol = Text[CurrentPosition];
            else
                CurrentSymbol = (char)0;
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
        public void WriteWarning(string message)
        {
            Logger.LogWarning("At Line " + (LineNumber + 1) + " Position " + (PositionInLine + 1) + ":");
            Logger.LogWarning("[Warning] " + message);
        }
        public void WriteError(string message)
        {
            Logger.LogError("At Line " + (LineNumber + 1) + " Position " + (PositionInLine + 1) + ":");
            Logger.LogError("[Error] " + message);
            this.FoundError = true;
            this.ErrorMessage = message;
        }
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
            foreach (var item in Tokens)
                sb.AppendLine(item.ToString());

            return sb.ToString();
        }

        public void Tokenize(string text)
        {
            this.Reset(text);
            while (CanContinue())
                ParseToken();
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
        private void ParseToken()
        {
            if (CurrentSymbol == '<')
                ParseHTMLToken();
            else if (IsJavaScript)
                ParseJavaScript();
            else
                ParseRaw();
        }
        private void ParseRaw()
        {
            var raw = new Token(this)
            {
                Tag = "raw",
                IsBeginning = true,
                IsEnd = true
            };
            this.Tokens.Add(raw);
            while (CanContinue())
                if (CurrentSymbol == '<')
                    break;
                else if (CurrentSymbol == '>')
                {
                    WriteWarning("Unexpected >");
                    //We tolerate those errors because they happen too often
                    //    return;
                    Next();
                    raw.Length++;
                }
                else
                {
                    Next();
                    raw.Length++;
                }
            raw.Text = Text.Substring(raw.Position, raw.Length);
        }
        private void ParseJavaScript()
        {
            if (CurrentSymbol == '<')
                throw new NotImplementedException();

            bool jsComment = false;

            var js = new Token(this)
            {
                Tag = "javascript",
                IsBeginning = true,
                IsEnd = true
            };

            void consumeCurrent()
            {
                Next();
                js.Length++;
            }

            this.Tokens.Add(js);
            while (CanContinue())
                if (jsComment)
                {
                    if (CurrentSymbol == '*')
                    {
                        consumeCurrent();
                        if (CanContinue() && CurrentSymbol == '/')
                        {
                            consumeCurrent();
                            jsComment = false;
                        }
                    }
                    else
                        consumeCurrent();
                }
                else if (CurrentSymbol == '/')
                {
                    consumeCurrent();
                    if (CanContinue() && CurrentSymbol == '*')
                    {
                        consumeCurrent();
                        jsComment = true;
                    }
                }
                else if (CurrentSymbol == '<')
                    break;
                else
                    consumeCurrent();

            js.Text = Text.Substring(js.Position, js.Length);
        }
        private void ParseHTMLToken()
        {
            if (CurrentSymbol != '<')
            {
                WriteError("Expected <");
                return;
            }
            Token html = new Token(this);
            this.Tokens.Add(html);
            Next();
            if (CurrentSymbol == '/')
            {
                html.IsEnd = true;
                Next();
            }
            else
                html.IsBeginning = true;

            html.Tag = RetrieveTagName();
            if (FoundError)
                return;

            while (CanContinue())
            {
                SkipWhiteSpaces();

                if (CurrentSymbol == '>')
                    break;
                if (CurrentSymbol == '/')
                {
                    Next();
                    if (CurrentSymbol == '>')
                    {
                        if (html.IsBeginning)
                        {
                            html.IsEnd = true;
                            break;
                        }
                        else
                        {
                            WriteError("Found tag of kind </.../>");
                            return;
                        }
                    }
                    else
                    {
                        WriteError("Expected />");
                        return;
                    }
                }
                var att = RetrieveAttribute();
                html.Attributes.Add(att);
                if (FoundError)
                    return;
            }
            if (CurrentSymbol != '>')
            {
                WriteError("Current Tag not closed");
                return;
            }
            Next();
            html.Length = CurrentPosition - html.Position;
            if (html.MyKind == Token.Kind.Script)
                IsJavaScript = !html.IsEnd;
        }
        private string RetrieveTagName()
        {
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
                    throw new NotImplementedException();
                }
            }
            return Text.Substring(start, length);
        }
        private Token.Attribute RetrieveAttribute()
        {
            string name = RetrieveAttributeName();
            if (FoundError)
                return new Token.Attribute();
            SkipWhiteSpaces();

            if (CurrentSymbol != '=')
            {
                WriteError("Expected =");
                return new Token.Attribute(name, null);
            }
            Next();
            string value = RetrieveValue();
            return new Token.Attribute(name, value);
        }
        private string RetrieveAttributeName()
        {
            SkipWhiteSpaces();
            int start = CurrentPosition;
            int length = 0;
            while (!EoF)
            {
                if (IsLiteral() || IsDigit() || IsLegalSymbol() || IsWhiteSpace())
                {
                    Next();
                    length++;
                }
                else if (CurrentSymbol == '=')
                    break;
                else
                {
                    WriteError("Expected Literal, digit, = or Whitespace");
                    return null;
                }
            }
            return Text.Substring(start, length).Trim();
        }
        private string RetrieveValue()
        {
            SkipWhiteSpaces();
            if (CurrentSymbol != '"' && CurrentSymbol != '\'')
            {
                WriteError("Expected \" or '");
                return null;
            }
            var endSymbol = CurrentSymbol;
            Next();
            int start = CurrentPosition;
            int length = 0;

            while (CanContinue())
            {
                if (CurrentSymbol == endSymbol)
                    break;
                length++;
                Next();
            }
            if (CurrentSymbol != endSymbol)
            {
                WriteError("Current Quotation not closed. Could not find " + endSymbol);
                return null;
            }
            Next();
            return Text.Substring(start, length);
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
        /// <summary>
        /// Legal for composing words in attribute names
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        private bool IsLegalSymbol(char c) => c == '-';
        private bool IsLegalSymbol() => IsLegalSymbol(CurrentSymbol);
    }
}
