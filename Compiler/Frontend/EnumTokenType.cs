using System;

namespace CPU_OS_Simulator.Compiler.Frontend
{
    [Flags]
    #pragma warning disable 1591
    public enum EnumTokenType : int
    {
        KEYWORD_TOKEN = 1 << 0,
        OPERATOR_TOKEN = 1 << 1,
        LEFT_BRACE_TOKEN = 1 << 2,
        RIGHT_BRACE_TOKEN = 1 << 3,
        LEFT_SQUARE_PAREN_TOKEN = 1 << 4,
        RIGHT_SQUARE_PAREN_TOKEN = 1 << 5,
        LEFT_PAREN_TOKEN = 1 << 6,
        RIGHT_PAREN_TOKEN = 1 << 7,
        WHITESPACE_TOKEN = 1 << 8,
        END_OF_FILE_TOKEN = 1 << 9,
        IDENTIFIER_TOKEN = 1 << 10,
        NUMERIC_LITERAL = 1 << 11,
        STRING_LITERAL = 1 << 12,
        COMMENT_TOKEN = 1 << 13,
        COMMA_TOKEN = 1 << 14,
        DOT_TOKEN = 1 << 15,
        UNKNOWN_TOKEN = 1 << 32
    }
}