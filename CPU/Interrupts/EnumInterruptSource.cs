using System;

namespace CPU_OS_Simulator.CPU.Interrupts
{
    [Flags]
    public enum EnumInterruptSource : long
    {
        #pragma warning disable 1591
        UNKNOWN = -1,
        SWI_INSTRUCTION = 1 << 0,
        INPUT = 1 << 1,
        INTERNAL_CALL = 1 << 2,
        MANUAL_CALL = 1 << 3
    }
}