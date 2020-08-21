using System;
using System.Collections.Generic;
using System.Text;

namespace LazenLang.Typechecking.Tools
{
    class Utils
    {
        public static int typevarNum = 0;
        public static TypeVariable FreshTypeVariable()
        {
            typevarNum++;
            return new TypeVariable(typevarNum);
        }
    }
}
