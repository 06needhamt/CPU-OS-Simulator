namespace CPU_OS_Simulator.Operating_System.Threading
{
    public enum EnumThreadState
    {
        #pragma warning disable 1591
        UNKNOWN = -1,
        RUNNING = 0,
        WAITING = 1,
        READY = 2,
        IN_SEMAPHORE = 3,
        WAITING_SEMAPHORE = 4,
        DEADLOCKED = 5,
        JOINED = 6
    }
}