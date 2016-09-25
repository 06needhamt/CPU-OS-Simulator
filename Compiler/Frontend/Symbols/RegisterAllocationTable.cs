using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CPU_OS_Simulator.Compiler.Frontend.Symbols
{
    public static class RegisterAllocationTable
    {
        public static int registerSetSize = 20;
        public static readonly List<bool> allocated = (List<bool>) Enumerable.Repeat<bool>(false, registerSetSize);
    }
}
