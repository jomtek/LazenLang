using LazenLang.Lexing;
using LazenLang.Parsing.Ast.Expressions.Literals;
using System;
using System.Collections.Generic;
using System.Text;

namespace LazenLang.Typechecking
{
    class Environment
    {
        private Dictionary<Identifier, TypeDesc> table;
        
        public Environment(Dictionary<Identifier, TypeDesc> table)
        {
            this.table = table;
        }

        public bool Has(Identifier id)
        {
            return table.ContainsKey(id);
        }

        public TypeDesc LookupId(Identifier id, CodePosition position)
        {
            if (!Has(id)) throw new TypecheckerError(new EnvironmentError($"Unknown name `{id}`"), position);
            return table[id];
        }

        public void SetIdType(Identifier id, CodePosition position, TypeDesc type)
        {
            LookupId(id, position);
            table[id] = type; 
        }

        public void AddEntry(Identifier id, TypeDesc type, CodePosition position)
        {
            if (Has(id)) throw new TypecheckerError(new EnvironmentError($"Name `{id}` is already defined"), position);
            table.Add(id, type);
        }
    }
}
