#pragma warning disable 1591
using System;

namespace CPU_OS_Simulator.Compiler.Old.Frontend.Tokens
{
    public class Identifier : Token
    {
        public override Enum DetectType()
        {
            return EnumTokenType.IDENTIFIER;
        }

        public virtual bool ResolveIdentifier()
        {
            //TODO implement me after symbol table
            return false;
        }
    }
}