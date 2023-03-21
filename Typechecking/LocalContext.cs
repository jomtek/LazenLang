using LazenLang.Lexing;
using LazenLang.Parsing.Ast.Types;
using Parsing.Ast;
using System;
using System.Collections.Generic;
using Typechecking.Errors;

namespace Typechecking
{
    public class LocalContext
    {
        private Dictionary<string, TypeDesc> _context;

        public LocalContext()
        {
            _context = new Dictionary<string, TypeDesc>()
            {
                // Prelude
            };
        }

        public bool Exists(string name)
        {
            return _context.ContainsKey(name);
        }

        public void AssertExists(string name, CodePosition pos)
        {
            if (!Exists(name))
                throw new TypeCheckerError(new UnknownName(name), pos);
        }

        public void Add(string name, TypeDesc desc, CodePosition pos)
        {
            if (Exists(name))
            {
                throw new TypeCheckerError(new NameShadowing(name), pos);
            }
            else
            {
                _context.Add(name, desc);
            }
        }

        // You should assert that the name exists before using this method
        public TypeDesc GetTypeUnsafe(string name)
        {
            return _context[name];
        }
    }
}
