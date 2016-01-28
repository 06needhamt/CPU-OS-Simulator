using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CPU_OS_Simulator.Operating_System.Threading
{
    [Serializable]
    public class ThreadControlBlock
    {
        private int CPUID;
        private int OSID;
        private int processID;
        private string processName;
        private int lifetimeMills;
        private int currentAddress;
        private int startAddress;
        private int priority;
        private bool ownsSemaphore;
        private bool waitingForSemaphore;
        private EnumThreadState currentState = EnumThreadState.UNKNOWN;
        private EnumThreadState previousState = EnumThreadState.UNKNOWN;

    }
}
