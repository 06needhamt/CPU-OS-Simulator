namespace CPU_OS_Simulator.Compiler.Frontend.Tokens
{
    public enum EnumTokenType
    {
        UNKNOWNN = -1,
        TYPE = 0,
        KEYWORD = 1,
        OPERATOR = 2,
        NUMBER = 3,
        IDENTIFIER = 4,
        OPENING_BRACE = 5,
        CLOSING_BRACE = 6,
        OPENING_CURLY_BRACE = 7,
        CLOSING_CURLY_BRACE = 8,
        COMMENT = 9,
        NEW_LINE = 10,
        COMMA = 11,
        DOT = 12,
        END_OF_FILE = 13,
        TAB = 14,
        STRING = 15,
        NUMERIC_LITERAL = 16,
        STRING_LITERAL = 17,
    }
}