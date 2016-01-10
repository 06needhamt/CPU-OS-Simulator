namespace CPU_OS_Simulator.Operating_System
{
    /// <summary>
    /// This enum represents the possible states that the operating system can be in
    /// </summary>
    public enum EnumOSState
    {
        #pragma warning disable 1591
        UNKNOWN = -1,
        SUSPENDED = 0,
        RUNNING = 1,
        STOPPED = 2,
        DEADLOCKED = 3,
    }
}