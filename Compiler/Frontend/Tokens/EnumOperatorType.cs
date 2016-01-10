namespace CPU_OS_Simulator.Compiler.Frontend.Tokens
{
    /// <summary>
    /// This enum defines all of the operators in the simulator programming language
    /// </summary>
    public enum EnumOperatorType
    {
        #pragma warning disable 1591
        UNKNOWN = -1,
        PLUS = 1,
        MINUS =2,
        ASSIGNMENT =3,
        MULTIPLY = 4,
        DIVIDE = 5,
        MODULO = 6,
        EQUALITY = 7,
        OR = 8,
        NOT = 9,
        AND = 10,
        XOR = 11,
        NOT_EQUAL = 12

    }
}