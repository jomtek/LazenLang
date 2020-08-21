using LazenLang.Parsing.Ast;
using LazenLang.Parsing.Ast.Expressions;
using LazenLang.Parsing.Ast.Expressions.Literals;
using LazenLang.Parsing.Ast.Expressions.OOP;
using LazenLang.Parsing.Ast.Types;
using LazenLang.Parsing.Ast.Types.AtomTypes;
using Microsoft.VisualBasic.CompilerServices;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LazenLang.Typechecking.Tools
{
    class TypeResolver
    {
        private static TypeDesc ResolveType(NativeArray arr)
        {
            ExprNode[] elements = ((NativeArray)arr).Elements;

            if (elements.Length == 0)
            {
                return Utils.FreshTypeVariable();
            }
            else
            {
                TypeDesc listType = ResolveType(elements[0]);

                foreach (ExprNode elem in elements)
                {
                    TypeDesc elemType = ResolveType(elem);
                    if (elemType.GetType() != listType.GetType()) // Not sure, should use `is`
                    {
                        throw new TypecheckerError(
                            new MismatchedTypes(listType, elemType),
                            elem.Position
                        );
                    }
                }

                return listType;
            }
        }

        private static TypeDesc ResolveType(Instanciation instanciation)
        {
            if (instanciation.GenericArgs.Length == 0)
            {
                return new NameType(instanciation.ClassName);
            }
            else
            {
                var generics = new List<TypeDesc>();
                foreach (TypeNode node in instanciation.GenericArgs)
                    generics.Add(node.Type);

                return new TypeApp(new NameType(instanciation.ClassName), generics.ToArray());
            }
        }

        public static TypeDesc ResolveType(ExprNode expr)
        {
            Expr value = expr.Value;

            if (value is BooleanLit)    return new BoolType();
            if (value is CharLit)       return new CharType();
            if (value is DoubleLit)     return new DoubleType();
            if (value is IntegerLit)    return new IntType();
            if (value is StringLit)     return new StringType();

            if (value is NativeArray)   return ResolveType((NativeArray)value);
            if (value is Instanciation) return ResolveType((Instanciation)value);


            return new VoidType();
            throw new NotImplementedException();
        }
    }
}
