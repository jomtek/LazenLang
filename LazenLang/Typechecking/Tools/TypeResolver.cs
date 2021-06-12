using LazenLang.Lexing;
using LazenLang.Parsing.Ast;
using LazenLang.Parsing.Ast.Expressions;
using LazenLang.Parsing.Ast.Expressions.Literals;
using LazenLang.Parsing.Ast.Expressions.OOP;
using LazenLang.Parsing.Ast.Types;
using LazenLang.Parsing.Ast.Types.AtomTypes;
using LazenLang.Typechecking.Checkers;
using Microsoft.VisualBasic.CompilerServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;

namespace LazenLang.Typechecking.Tools
{
    class TypeResolver
    {
        private static TypeDesc ResolveType(NativeArray arr, Environment env, CodePosition position)
        {
            ExprNode[] elements = ((NativeArray)arr).Elements;

            if (elements.Length == 0)
            {
                return Utils.FreshTypeVariable();
            }
            else
            {
                TypeDesc listType = ResolveType(elements[0], env, position);

                foreach (ExprNode elem in elements)
                {
                    TypeDesc elemType = ResolveType(elem, env, position);
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

        /*
        private static TypeDesc ResolveType(Instanciation instanciation, Environment env, CodePosition position)
        {
            env.LookupId(instanciation.ClassName, position);

            if (instanciation.GenericArgs.Length == 0)
            {
                return new NameType(instanciation.ClassName);
            }
            else
            {
                int typevarCount = ((Class)env.LookupId(instanciation.ClassName, position)).GenericParameters.Length;
                var generics = new List<TypeDesc>();

                foreach (TypeNode node in instanciation.GenericArgs)
                    generics.Add(node.Type);

                if (generics.Count() != typevarCount)
                {
                    throw new TypecheckerError(
                        new EnvironmentError($"No class `{instanciation.ClassName.Value}` holds {typevarCount + 1} generic parameters"),
                        position
                    );
                }

                return new TypeApp(new NameType(instanciation.ClassName), generics.ToArray());
            }
        }
        */


        private static TypeDesc ResolveType(IfInstr ifinstr, Environment env, CodePosition position)
        {
            TypeDesc conditionType = ResolveType(ifinstr.Condition, env, ifinstr.Condition.Position);
            ExprNode mainBranchLast;
            ExprNode elseBranchLast;
            TypeDesc mainBranchType;
            TypeDesc elseBranchType;

            /*
            if (!(conditionType is BoolType))
            {
                throw new TypecheckerError(
                    new MismatchedTypes(new BoolType(), conditionType),
                    ifinstr.Condition.Position
                );
            }
            */

            RegularChecker mainChecker = new RegularChecker(ifinstr.MainBranch, env);
            RegularChecker elseChecker = new RegularChecker(ifinstr.ElseBranch, env);
            mainChecker.Typecheck();
            elseChecker.Typecheck();

            try
            {
                mainBranchLast = Utils.GetBlockLast(ifinstr.MainBranch);
            } catch (ArgumentException)
            {
                mainBranchLast = new ExprNode(new NullExpr(), ifinstr.Condition.Position);
            }

            try
            {
                elseBranchLast = Utils.GetBlockLast(ifinstr.ElseBranch);
            } catch (ArgumentException)
            {
                elseBranchLast = new ExprNode(new NullExpr(), ifinstr.Condition.Position);
            }

            mainBranchType = ResolveType(mainBranchLast, env, position);
            elseBranchType = ResolveType(elseBranchLast, env, position);

            if (mainBranchType.GetType() != elseBranchType.GetType())
            {
                throw new TypecheckerError(
                    new MismatchedTypes(mainBranchType, elseBranchType),
                    position
                );
            }

            return mainBranchType;
        }

        private static TypeDesc ResolveType(FuncCall call, Environment env, CodePosition position)
        {
            TypeDesc supposedFuncType = env.LookupId(call.Name, position);
            FuncType targetFuncType;

            if (!(supposedFuncType is FuncType))
            {
                throw new TypecheckerError(
                    new EnvironmentError($"Name `{call.Name}` is not a function"),
                    position
                );
            }

            targetFuncType = (FuncType)supposedFuncType;

            if (call.Arguments.Count() != targetFuncType.Domain.Count())
            {
                throw new TypecheckerError(
                    new EnvironmentError($"Function `{call.Name}` takes {targetFuncType.Domain.Count()} arguments"),
                    position
                );
            }

            for (int i = 0; i < call.Arguments.Count(); i++)
            {
                TypeDesc callArgType = ResolveType(call.Arguments[i], env, position);
                TypeDesc targetFuncParamType = targetFuncType.Domain[i];

                if (callArgType.GetType() != targetFuncParamType.GetType())
                {
                    throw new TypecheckerError(
                        new MismatchedTypes(targetFuncParamType, callArgType),
                        call.Arguments[i].Position
                    );
                }
            }

            return targetFuncType;
        }

        public static TypeDesc ResolveType(ExprNode expr, Environment env, CodePosition position)
        {
            Expr value = expr.Value;

            /*
            if (value is BooleanLit)    return new BoolType();
            if (value is CharLit)       return new CharType();
            if (value is DoubleLit)     return new DoubleType();
            if (value is IntegerLit)    return new IntType();
            if (value is StringLit)     return new StringType();
            */

            if (value is NullExpr)      return new NullType();
            if (value is Identifier)    return env.LookupId((Identifier)value, position);

            if (value is NativeArray)   return ResolveType((NativeArray)value, env, position);
            // TODO
            //if (value is Instanciation) return ResolveType((Instanciation)value, env, position);

            if (value is IfInstr)       return ResolveType((IfInstr)value, env, position);
            if (value is FuncCall)      return ResolveType((FuncCall)value, env, position);

            //return new VoidType();
            throw new NotImplementedException();
        }
    }
}
