namespace CPU_OS_Simulator.Console
{
    public enum EnumConsoleCommands
    {
        [NumberOfParameters(0)]
        UNKNOWN = -1,
        [NumberOfParameters(0)]
        HELP = 0,
        [NumberOfParameters(0)]
        PROGRAM = 1,
        [NumberOfParameters(1)]
        SIZE = 2

    }
}