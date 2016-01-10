namespace CPU_OS_Simulator.Compiler.Frontend.Tokens
{
    /// <summary>
    /// This enum defines all of the keywords in the simulator programming language
    /// </summary>
    public enum EnumKeywordType
    {
        #pragma warning disable 1591
        UNKNOWN = -1,
        PROGRAM =0,
        END = 2,
        WHILE = 3,
        WEND= 4,
        FOR = 5,
        NEXT = 6,
        IF = 7,
        END_IF = 8,
        SELECT = 9,
        CASE = 10,
        DEFAULT =11,
        THREAD =12,
        RET = 13,
        CALL = 14,
        GOTO = 15,
        ENTER = 16,
        LEAVE = 17,
        SYNCHRONISE = 18,
        CONTINUE = 19,
        BREAK = 20,
        TRUE = 21,
        FALSE = 22,
        RESOURCE = 23,
        THEN = 24,
        ELSE = 25,
        ELSE_IF = 26,
        SUB = 27,
        FUN = 28,
        DO = 29,
        LOOP = 30,
        READ = 31,
        WRITE = 32,
        VAR = 33,
        END_SELECT = 34,
        END_SUB = 35,
        AS = 36
    }
}