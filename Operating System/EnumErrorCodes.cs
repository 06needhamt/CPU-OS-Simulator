namespace CPU_OS_Simulator.Operating_System
{
    /// <summary>
    /// This enum contains error codes used by the Operating system when an error occurs
    /// </summary>
    public enum EnumErrorCodes
    {
        #pragma warning disable 1591
        UNKNOWN = -1,
        NO_ERROR = 0,
        NO_PROCESSES =1,
        DEADLOCK = 2,
        OUT_OF_MEMORY = 3,

    }
}