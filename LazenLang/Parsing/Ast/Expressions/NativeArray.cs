using System;
using System.Linq;
using LazenLang.Lexing;

namespace LazenLang.Parsing.Ast.Expressions
{
    class NativeArray : Expr
    {
        public ExprNode[] Elements;

        public NativeArray(ExprNode[] elements)
        {
            Elements = elements;
        }

        public static NativeArray Consume(Parser parser)
        {
            parser.Eat(TokenInfo.TokenType.L_BRACKET);
            ExprNode[] elements = Utils.ParseSequence(parser, ExprNode.Consume);
            parser.Eat(TokenInfo.TokenType.R_BRACKET, false);

            return new NativeArray(elements);
        }

        public override string Pretty()
        {
            string prettyElements = "";

            for (int i = 0; i < Elements.Count(); i++)
            {
                ExprNode node = Elements[i];
                prettyElements += node.Pretty();
                if (i != Elements.Count() - 1) prettyElements += ", ";
            }

            return $"Array [{prettyElements}]";
        }
    }
}