namespace CPU_OS_Simulator.Console
{
    /// <summary>
    /// This enum represents all of the console commands that can be executed in the console
    /// </summary>
    public enum EnumConsoleCommands
    {
        [NumberOfParameters(0)]
        UNKNOWN = -1,
        [NumberOfParameters(0)]
        HELP = 0,
        [NumberOfParameters(0)]
        PROGRAM = 1,
        [NumberOfParameters(1)]
        SIZE = 2,
        [NumberOfParameters(0)]
        CLEAR = 3

    }
}