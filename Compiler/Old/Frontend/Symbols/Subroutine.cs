#pragma warning disable 1591
using CPU_OS_Simulator.Compiler.Old.Frontend.Tokens;

namespace CPU_OS_Simulator.Compiler.Old.Frontend.Symbols
{
    public class Subroutine : Symbol
    {
        public Subroutine(string name, EnumTypes type, string value,Scope scope)
        {
            this.name = name;
            this.type = type;
            this.value = value;
            this.issub = true;
            this.isfun = false;
            SymbolScope = scope;
        }
    }
}
