using System;
using System.Linq;
using System.Text;
using LazenLang.Lexing;
using LazenLang.Parsing.Display;

namespace LazenLang.Parsing.Ast.Expressions
{
    public class NativeArray : Expr, IPrettyPrintable
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

        public override string Pretty(int level)
        {
            return "NativeArray: " + Display.Utils.PrettyArray(Elements, level);
        }
    }
}