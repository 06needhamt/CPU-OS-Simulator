using System;

namespace CPU_OS_Simulator.Compiler.Frontend
{
    /// <summary>
    /// This enum defines all of the operators in the simulator programming language
    /// </summary>
    [Flags]
    public enum EnumOperatorType
    {
#pragma warning disable 1591
        UNKNOWN = 0,
        PLUS = 1 << 0,
        MINUS = 1 << 1,
        ASSIGNMENT = 1 << 2,
        MULTIPLY = 1 << 3,
        DIVIDE = 1 << 4,
        MODULO = 1 << 5,
        EQUALITY = 1 << 6,
        OR = 1 << 7,
        NOT = 1 << 8,
        AND = 1 << 9,
        XOR = 1 << 10,
        NOT_EQUAL = 1 << 11

    }
}