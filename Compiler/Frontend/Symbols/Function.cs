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
        public Function(string name, EnumTypes type, string value) : base(name, type, value)
        {
            issub = false;
            isfun = true;
        }
    }
}
