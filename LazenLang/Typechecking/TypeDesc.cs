using LazenLang.Parsing.Ast.Expressions.Literals;
using LazenLang.Parsing.Display;
using System;
using System.Collections.Generic;
using System.Text;

namespace LazenLang.Typechecking
{
    public abstract class TypeDesc : IPrettyPrintable
    {
        public abstract string Pretty(int level);
    }

    public abstract class AtomType : TypeDesc
    { }

    public class NameType : TypeDesc, IPrettyPrintable
    {
        public Identifier Name;
        public NameType(Identifier name)
        {
            Name = name;
        }

        public override string Pretty(int level)
        {
            return $"NameType: {Name.Pretty(level)}";
        }
    }

    public class TypeApp : TypeDesc, IPrettyPrintable
    {
        public NameType BaseType { get; }
        public TypeDesc[] Generics { get; }
        public TypeApp(NameType baseType, TypeDesc[] generics)
        {
            BaseType = baseType;
            Generics = generics;
        }

        public override string Pretty(int level)
        {
            var sb = new StringBuilder("TypeApp");
            sb.AppendLine();
            sb.AppendLine(Parsing.Display.Utils.Indent(level + 1) + $"Base: {BaseType.Pretty(level + 1)}");
            sb.AppendLine(Parsing.Display.Utils.Indent(level + 1) + $"Generics: {Parsing.Display.Utils.PrettyArray(Generics, level + 1)}");

            return sb.ToString();
        }
    }

    public class ArrayType : TypeDesc, IPrettyPrintable
    {
        public TypeDesc ElementsType { get; }
        public ArrayType(TypeDesc elementsType)
        {
            ElementsType = elementsType;
        }

        public override string Pretty(int level)
        {
            return $"ArrayType: {ElementsType.Pretty(level)}";
        }
    }

    public class FuncType : TypeDesc, IPrettyPrintable
    {
        public TypeDesc[] Domain { get; }
        public TypeDesc Codomain { get; }
        public FuncType(TypeDesc[] domain, TypeDesc codomain)
        {
            Domain = domain;
            Codomain = codomain;
        }

        public override string Pretty(int level)
        {
            var sb = new StringBuilder("FuncType");
            sb.AppendLine();
            sb.AppendLine(Parsing.Display.Utils.Indent(level + 1) + $"Domain: {Parsing.Display.Utils.PrettyArray(Domain, level + 1)}");
            sb.AppendLine(Parsing.Display.Utils.Indent(level + 1) + $"Codomain: {Codomain.Pretty(level + 1)}");

            return sb.ToString();
        }
    }

    public class TypeVariable : TypeDesc, IPrettyPrintable
    {
        public int Num { get; }
        public TypeVariable(int num)
        {
            Num = num;
        }

        public override string Pretty(int level)
        {
            return $"TypeVariable #{Num}";
        }
    }

    public class NullType : TypeDesc, IPrettyPrintable
    {
        public override string Pretty(int level)
        {
            return "NullType";
        }
    }

    /*public class Class : TypeDesc
    {
        public Identifier[] GenericParameters { get; }
        public Class(Identifier[] genericParameters)
        {
            GenericParameters = genericParameters;
        }
    }
    */

    /*
    public class BoolType : AtomType, IPrettyPrintable {}
    public class CharType : AtomType, IPrettyPrintable {}
    public class DoubleType : AtomType, IPrettyPrintable {}
    public class IntType : AtomType, IPrettyPrintable {}
    public class StringType : AtomType, IPrettyPrintable {}
    public class VoidType : AtomType, IPrettyPrintable {}*/
}