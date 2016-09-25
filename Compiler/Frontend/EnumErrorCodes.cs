using System;

namespace CPU_OS_Simulator.Compiler.Frontend
{
    #pragma warning disable 1591
    [Flags]
    public enum EnumErrorCodes
    {
        NO_ERROR = 0,
        UNEXPECTED_TOKEN = 1 << 0,
        EXPECTED_TOKEN = 1 << 1,
        UNTERMINATED_STRING = 1 << 2,
        UNKNOWN_IDENTIFIER = 1 << 3,
        INCORRECT_TYPE = 1 << 4,
        INVALID_OPERATION = 1 << 5,
        UNKNOWN_ERROR = 1 << 32

    }
}