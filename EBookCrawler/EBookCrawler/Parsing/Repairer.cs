﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBookCrawler.Parsing
{
    public class Repairer
    {
        public string Text { get; set; }
        public Token[] Input { get; set; }
        public List<Token> Output { get; set; }
        private Stack<Token> OpenTokens { get; set; }
        private Stack<Token> Overlapping { get; set; }
        private Stack<Token> Temp { get; set; }
        private Stack<Token> NewOpenings { get; set; }
        private int Position;
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
                IsOpening = !CurrentToken.IsEnd;
                IsClosing = !CurrentToken.IsBeginning;
            }
        }

        private void Reset(IEnumerable<Token> input)
        {
            this.FoundError = false;
            this.Input = input.ToArray();
            this.Output = new List<Token>();
            this.OpenTokens = new Stack<Token>();
            this.Overlapping = new Stack<Token>();
            this.Temp = new Stack<Token>();
            this.NewOpenings = new Stack<Token>();
            this.Position = 0;

            CurrentToken = Input[Position];
            if (Input.Length > 0)
            {
                IsOpening = !CurrentToken.IsEnd;
                IsClosing = !CurrentToken.IsBeginning;
            }
        }

        public void Repair(string Text, IEnumerable<Token> input)
        {
            this.Text = Text;
            this.Reset(input);

            while (this.Position < Input.Length)
            {
                if (IsOpening)
                {
                    OpenTokens.Push(CurrentToken);
                    Output.Add(CurrentToken);
                }
                else if (IsClosing)
                {
                    if (FindOpening(CurrentToken))
                    {
                        Close();
                        Output.Add(CurrentToken);
                        OutputNewOpenings();
                    }
                    else
                    {
                        WriteWarning("Repairer: Unexpected closing tag " + CurrentToken);
                        ShuffleBack();
                    }
                }
                else
                    Output.Add(CurrentToken);
                Next();
            }
            Token lastToken = Output.Last();
            while (this.OpenTokens.Count > 0)
            {
                var open = OpenTokens.Pop();
                WriteWarning("EoF reached, not closed tag: " + open);
                open.EndPosition = Text.Length;
                Output.Add(open.GetArtificialClosing(lastToken));
            }
        }
        private void WriteWarning(string msg)
        {
            Logger.LogWarning(msg);
        }
        private void WriteError(string msg)
        {
            this.FoundError = true;
            Logger.LogError(msg);
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
                var newOpen = open.GetArtificialOpening(CurrentToken);
                OpenTokens.Push(newOpen);
                NewOpenings.Push(newOpen);

                Temp.Push(open.GetArtificialClosing(CurrentToken));
                WriteWarning("Repairer: Overlapping " + CurrentToken + " and " + open);
            }
            while (Temp.Count > 0)
                Output.Add(Temp.Pop());
        }
        private bool FindOpening(Token closing)
        {
            while (OpenTokens.Count > 0)
            {
                var opening = OpenTokens.Pop();
                if (opening.Tag == closing.Tag)
                {
                    opening.EndPosition = closing.Position;
                    return true;
                }

                Overlapping.Push(opening);
            }
            return false;
        }
        private void ShuffleBack()
        {
            while (Overlapping.Count > 0)
                OpenTokens.Push(Overlapping.Pop());
        }

        public IEnumerable<Token> RemoveTag(string tag, IEnumerable<Token> tokens)
        {
            foreach (var token in tokens)
                if (token.Tag != tag)
                    yield return token;
        }
    }
}
