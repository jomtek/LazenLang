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

        public TypeDesc LookupId(Identifier id)
        {
            if (!Has(id)) throw new KeyNotFoundException();
            return table[id];
        }

        public void SetIdType(Identifier id, TypeDesc type)
        {
            LookupId(id);
            table[id] = type; 
        }

        public void AddEntry(Identifier id, CodePosition position, TypeDesc type)
        {
            if (Has(id))
            {
                throw new TypecheckerError(
                    new IdentifierShadowing(id),
                    position
                );
            }

            table.Add(id, type);
        }
    }
}
