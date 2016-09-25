using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CPU_OS_Simulator.Compiler.Old.Frontend.Symbols;

namespace CPU_OS_Simulator.Compiler.Frontend.Symbols
{
    public class VariableSymbol : Symbol
    {
        private EnumTypes variableType = EnumTypes.UNKNOWN;

        public VariableSymbol()
        {
            
        }

    }
}
