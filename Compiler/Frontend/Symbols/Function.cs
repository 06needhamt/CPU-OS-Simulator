using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CPU_OS_Simulator.Compiler.Frontend.Tokens;

namespace CPU_OS_Simulator.Compiler.Frontend.Symbols
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
