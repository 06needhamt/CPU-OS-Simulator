namespace CPU_OS_Simulator.Operating_System
{
    /// <summary>
    /// This process represents the scheduling policies that can be used by the process scheduler 
    /// </summary>
    public enum EnumSchedulingPolicies
    {
        UNKNOWN = -1,
        FIRST_COME_FIRST_SERVED = 0,
        SHORTEST_JOB_FIRST = 1,
        ROUND_ROBIN = 2,
        LOTTERY_SCHEDULING = 3,
        FAIR_SHARE_SCEDULING = 4
    }
}