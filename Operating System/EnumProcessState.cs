namespace CPU_OS_Simulator.Operating_System
{
    /// <summary>
    /// This enum represents the possible states that a process can be in
    /// </summary>
    public enum EnumProcessState
    {
        UNKNOWN = -1,
        RUNNING = 0,
        READY = 1,
        WAITING = 2
    }
}