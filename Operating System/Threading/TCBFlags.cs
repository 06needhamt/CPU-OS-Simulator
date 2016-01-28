namespace CPU_OS_Simulator.Operating_System.Threading
{
    public struct TCBFlags
    {
        #pragma warning disable 1591
        public int CPUID;
        public int OSID;
        public int processID;
        public string processName;
        public int lifetimeMills;
        public int currentAddress;
        public int startAddress;
        public int priority;
        public bool ownsSemaphore;
        public bool waitingForSemaphore;
        public EnumThreadState currentState; 
        public EnumThreadState previousState;
    }
}