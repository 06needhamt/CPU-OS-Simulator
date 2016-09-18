#pragma warning disable 1591
using CPU_OS_Simulator.Compiler.Old.Frontend.Tokens;

namespace CPU_OS_Simulator.Compiler.Old.Frontend.Symbols
{
    public class Function : Subroutine
    {
        public Function(string name, EnumTypes type, string value,Scope scope) : base(name, type, value,scope)
        {
            issub = false;
            isfun = true;
        }
    }
}
