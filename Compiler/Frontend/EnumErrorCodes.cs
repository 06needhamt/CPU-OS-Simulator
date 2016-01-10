namespace CPU_OS_Simulator.Compiler.Frontend
{
    /// <summary>
    /// This enum defines the errors that can be thrown by the simulator language compiler
    /// </summary>
    public enum EnumErrorCodes
    {
        #pragma warning disable 1591
        UNKNOWN = 0,
        SYNTAX_ERROR = 1,
        EXPECTED_AN_IDENTIFIER = 2,
        EXPECTED_A_KEYWORD = 3,
        EXPECTED_AN_OPERATOR = 4,
        EXPECTED_A_TYPENAME = 5
    }
}