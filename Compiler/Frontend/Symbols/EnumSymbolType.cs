using System;

namespace CPU_OS_Simulator.Compiler.Symbols
{
    #pragma warning disable 1591
    [Flags]
    public enum EnumSymbolType
    {
        UNKNOWN = 0,
        PROGRAM = 1 << 0,
        VARIABLE = 1 << 1,
        PARAMETER = 1 << 2,
        FUNCTION = 1 << 3,
        SUBROUTINE = 1 << 4
    }
}