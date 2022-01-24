using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBookCrawler.Parsing
{
    public class Repairer
    {
        public Token[] Input { get; set; }
        public List<Token> Output { get; set; }
        private Stack<HTMLToken> OpenTokens { get; set; }
        private Stack<HTMLToken> Overlapping { get; set; }
        private Stack<HTMLToken> Temp { get; set; }
        private Stack<HTMLToken> NewOpenings { get; set; }
        private int Position;
        private HTMLToken HTMLToken;
        private Token CurrentToken;
        private bool IsOpening;
        private bool IsClosing;
        public bool FoundError { get; set; }

        private void Next()
        {
            Position++;
            if (Position < Input.Length)
            {
                CurrentToken = Input[Position];
                HTMLToken = CurrentToken as HTMLToken;
                if (HTMLToken == null)
                    IsOpening = IsClosing = false;
                else
                {
                    IsOpening = !HTMLToken.IsEnd;
                    IsClosing = !HTMLToken.IsBeginning;
                }
            }
        }

        public void Repair(IEnumerable<Token> input)
        {
            this.FoundError = false;
            this.Input = input.ToArray();
            this.Output = new List<Token>();
            this.OpenTokens = new Stack<HTMLToken>();
            this.Overlapping = new Stack<HTMLToken>();
            this.Temp = new Stack<HTMLToken>();
            this.NewOpenings = new Stack<HTMLToken>();
            this.Position = 0;

            while (this.Position < Input.Length)
            {
                if (IsOpening)
                {
                    OpenTokens.Push(HTMLToken);
                    Output.Add(HTMLToken);
                }
                else if (IsClosing)
                {
                    if (FindOpening(HTMLToken, out HTMLToken opening))
                    {
                        Close();
                        Output.Add(HTMLToken);
                        OutputNewOpenings();
                    }
                    else
                    {
                        WriteError("Repairer: Unexpected closing tag " + HTMLToken);
                        ShuffleBack();
                    }
                }
                else
                    Output.Add(CurrentToken);
                Next();
            }
        }
        private void WriteError(string msg)
        {
            this.FoundError = true;
            Logger.LogLine(msg);
        }
        private void OutputNewOpenings()
        {
            while (NewOpenings.Count > 0)
                Output.Add(NewOpenings.Pop());
        }
        private void Close()
        {
            while (Overlapping.Count > 0)
            {
                var open = Overlapping.Pop();
                var newOpen = open.GetArtificialOpening(HTMLToken);
                OpenTokens.Push(newOpen);
                NewOpenings.Push(newOpen);

                Temp.Push(open.GetArtificialClosing(HTMLToken));
                WriteError("Repairer: Overlapping " + HTMLToken + " and " + open);
            }
            while (Temp.Count > 0)
                Output.Add(Temp.Pop());
        }
        private bool FindOpening(HTMLToken closing, out HTMLToken opening)
        {
            while (OpenTokens.Count > 0)
            {
                opening = OpenTokens.Pop();
                if (opening.Tag == closing.Tag)
                    return true;

                Overlapping.Push(opening);
            }
            opening = null;
            return false;
        }
        private void ShuffleBack()
        {
            while (Overlapping.Count > 0)
                OpenTokens.Push(Overlapping.Pop());
        }
    }
}
