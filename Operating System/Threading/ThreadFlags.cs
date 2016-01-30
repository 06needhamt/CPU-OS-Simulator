using System.Collections.Generic;

namespace CPU_OS_Simulator.Operating_System.Threading
{
    public struct ThreadFlags
    {
        public SimulatorProcess ownerProcess;
        public List<SimulatorThread> childThreads;
        public ThreadControlBlock controlBlock;
        public ThreadExecutionUnit executionUnit;
        public EnumThreadState currentState;
        public EnumThreadState previousState;
        public int threadPriority;
        public bool ownsSemaphore;
        public bool waitingForSemaphore;
        public bool resourceStarved;
        public List<SimulatorResource> allocatedResources;
        public List<SimulatorResource> requestedResources;
        public bool terminated;
        public List<LotteryTicket> lotteryTickets;
    }
}