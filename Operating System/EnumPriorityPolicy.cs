namespace CPU_OS_Simulator.Operating_System
{
    /// <summary>
    /// This enum represents the priority policies that can be used
    /// </summary>
    public enum EnumPriorityPolicy
    {
        #pragma warning disable 1591
        UNKNOWN = -1,
        NO_PRIORITY = 0,
        NON_PRE_EMPTIVE = 1,
        PRE_EMPTIVE = 2,
    }
}