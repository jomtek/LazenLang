using LazenLang.Lexing;
using Parsing.Ast;
using System.Collections.Generic;
using Typechecking.Errors;

namespace Typechecking
{
    public class LocalContext
    {
        public Dictionary<string, TypeDesc> Context { get; private set; }

        // For function contexts
        public TypeDesc CurrentCodomain;

        public LocalContext(bool init = true, TypeDesc currentCodomain = null)
        {
            CurrentCodomain = currentCodomain;

            if (init)
            {
                Context = new Dictionary<string, TypeDesc>()
                {
                    // TODO: Prelude
                };
            }
        }

        public bool Exists(string name)
        {
            return Context.ContainsKey(name);
        }

        public void AssertExists(string name, CodePosition pos)
        {
            if (!Exists(name))
                throw new TypeCheckerError(new UnknownName(name), pos);
        }

        public void Add(string name, TypeDesc desc, CodePosition pos, bool overrideCtx = false)
        {
            if (Exists(name))
            {
                if (overrideCtx)
                    Context[name] = desc;
                else
                    throw new TypeCheckerError(new NameShadowing(name), pos);
            }
            else
            {
                Context.Add(name, desc);
            }
        }

        // You should assert that the name exists before using this method
        public TypeDesc GetTypeUnsafe(string name)
        {
            return Context[name];
        }

        public static LocalContext CopyFrom(LocalContext context)
        {
            // No need for a deep copy
            return new LocalContext(false, context.CurrentCodomain)
            {
                Context = new Dictionary<string, TypeDesc>(context.Context)
            };
        }
    }
}
