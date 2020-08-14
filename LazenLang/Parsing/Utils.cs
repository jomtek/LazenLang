using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using LazenLang.Lexing;
using LazenLang.Parsing.Ast;
using LazenLang.Parsing.Ast.Expressions.Literals;
using LazenLang.Parsing.Ast.Statements;
using LazenLang.Parsing.Ast.Statements.Functions;

namespace LazenLang.Parsing
{
    class Utils
    {
        public static T[] ParseSequence<T>(Parser parser, Func<Parser, T> consumer, TokenInfo.TokenType delimiter = TokenInfo.TokenType.COMMA)
        {
            var sequence = new List<T>();
            bool lastlyEaten = false;
            Token lastTokenEaten = null;

            while (true)
            {
                try
                {
                    sequence.Add(parser.TryConsumer(consumer));
                    lastlyEaten = false;
                    lastTokenEaten = parser.Eat(delimiter);
                    lastlyEaten = true;
                } catch (ParserError ex)
                {
                    if (!ex.IsExceptionFictive()) throw ex;
                    break;
                }
            }

            if (lastlyEaten)
            {
                throw new ParserError(
                    new UnexpectedTokenException(delimiter),
                    lastTokenEaten.Pos
                );
            }

            return sequence.ToArray();
        }

        public static Block InstrToBlock(InstrNode instr)
        {
            if (instr.Value is Block)
                return (Block)instr.Value;
            else
                return new Block(new InstrNode[] { instr });
        }

        public static string PrettyArgs(ExprNode[] args)
        {
            string result = "";
            for (int i = 0; i < args.Count(); i++)
            {
                ExprNode node = args[i];
                result += node.Pretty();
                if (i != args.Count() - 1) result += ", ";
            }
            return $"{{{result}}}";
        }

        public static void CheckBannedIds(Identifier id, CodePosition cursor)
        {
            var bannedIds = new string[]
            {
                "this"
            };

            if (bannedIds.Contains(id.Value))
            {
                throw new ParserError(
                    new BannedIdentifier(id.Value),
                    cursor
                );
            }
        }
        /*public static string PrettyArray<T, T1>(T[] list, Func<T, string> prettyPrinter)
        {
            string result = "";
            for (int i = 0; i < list.Count(); i++)
            {
                T elem = list[i];
                result += prettyPrinter(elem);
                if (i != list.Count()) result += ", ";
            }
            return "{" + result + "}";
        }*/
    }
}
