using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBookCrawler.Parsing
{
    public class Parser
    {
        public class Node
        {
            public Token Token { get; set; }
            public IEnumerable<Node> Children { get; set; }
            public bool IsLeaf => Children == null || Children.Count() == 0;
            public bool IsRoot => Token == null;
        }

        public Token[] Tokens { get; set; }
        public int CurrentPosition { get; set; }
        public Token CurrentToken => Tokens[CurrentPosition];
        public Node Root { get; set; }

        public void Parse(IEnumerable<Token> tokens)
        {
            this.CurrentPosition = 0;
            this.Tokens = tokens.ToArray();
            var children = new List<Node>();
            while (CurrentPosition < Tokens.Length)
                children.Add(ParseNode());
            this.Root = new Node() { Children = children };
        }
        private Node ParseNode()
        {
            if (CurrentToken.IsBeginning && CurrentToken.IsEnd)
            {
                var node = new Node() { Token = CurrentToken };
                CurrentPosition++;
                return node;
            }
            else if (CurrentToken.IsBeginning)
            {
                var node = new Node() { Token = CurrentToken };
                CurrentPosition++;
                node.Children = ParseChildren(node.Token.Tag);
                CurrentPosition++;
                return node;
            }
            else
                throw new NotImplementedException();
        }
        private bool ParseClosing(string closingTag)
        {
            return CurrentToken.Tag == closingTag && !CurrentToken.IsBeginning;
        }
        private IEnumerable<Node> ParseChildren(string closingTag)
        {
            while (CurrentPosition < Tokens.Length)
                if (ParseClosing(closingTag))
                    yield break;
                else
                    yield return ParseNode();
            throw new NotImplementedException();
        }
    }
}
