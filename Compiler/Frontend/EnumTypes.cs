using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CPU_OS_Simulator.Compiler.Frontend
{
    
    /// <summary>
    /// This enum defines the built in types in the simulator programming language
    /// </summary>

    [Flags]
    public enum EnumTypes
    {
        #pragma warning disable 1591
        UNKNOWN = 0,
        INTEGER = 1 << 0,
        BOOLEAN = 1 << 1,
        BYTE =  1 << 2,
        OBJECT = 1 << 3,
        STRING = 1 << 4,
        ARRAY = 1 << 5,
        VOID = 1 << 6,
    }
}
