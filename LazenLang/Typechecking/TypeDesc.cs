using LazenLang.Parsing.Ast.Expressions.Literals;
using System;
using System.Collections.Generic;
using System.Text;

namespace LazenLang.Typechecking
{
    public abstract class TypeDesc
    { }

    public abstract class AtomType : TypeDesc
    { }

    public class NameType : TypeDesc
    {
        public Identifier Name;
        public NameType(Identifier name)
        {
            Name = name;
        }
    }

    public class TypeApp : TypeDesc
    {
        public NameType BaseType { get; }
        public TypeDesc[] Generics { get; }
        public TypeApp(NameType baseType, TypeDesc[] generics)
        {
            BaseType = baseType;
            Generics = generics;
        }
    }

    public class ArrayType : TypeDesc
    {
        public TypeDesc ElementsType { get; }
        public ArrayType(TypeDesc elementsType)
        {
            ElementsType = elementsType;
        }
    }

    public class FuncType : TypeDesc
    {
        public TypeDesc[] Domain { get; }
        public TypeDesc Codomain { get; }
        public FuncType(TypeDesc[] domain, TypeDesc codomain)
        {
            Domain = domain;
            Codomain = codomain;
        }
    }

    public class TypeVariable : TypeDesc
    {
        public int Num { get; }
        public TypeVariable(int num)
        {
            Num = num;
        }
    }

    public class NullType : TypeDesc
    {}

    public class Class : TypeDesc
    {
        public Identifier[] GenericParameters { get; }
        public Class(Identifier[] genericParameters)
        {
            GenericParameters = genericParameters;
        }
    }

    public class BoolType : AtomType {}
    public class CharType : AtomType {}
    public class DoubleType : AtomType {}
    public class IntType : AtomType {}
    public class StringType : AtomType {}
    public class VoidType : AtomType {}
}