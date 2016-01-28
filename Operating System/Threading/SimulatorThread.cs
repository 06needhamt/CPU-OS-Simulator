using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CPU_OS_Simulator.Operating_System.Threading
{
    [Serializable]
    public class SimulatorThread
    {
        private SimulatorProcess ownerProcess;
        private List<SimulatorThread> childThreads;
        private ThreadControlBlock controlBlock;
        private EnumThreadState currentState = EnumThreadState.UNKNOWN;
        private EnumThreadState previousState = EnumThreadState.UNKNOWN;
        private int threadPriority;
        private bool ownsSemaphore;
        private bool waitingForSemaphore;

        public SimulatorProcess OwnerProcess
        {
            get { return ownerProcess; }
            set { ownerProcess = value; }
        }

        public List<SimulatorThread> ChildThreads
        {
            get { return childThreads; }
            set { childThreads = value; }
        }

        public int ThreadPriority
        {
            get { return threadPriority; }
            set { threadPriority = value; }
        }

        public bool OwnsSemaphore
        {
            get { return ownsSemaphore; }
            set { ownsSemaphore = value; }
        }

        public bool WaitingForSemaphore
        {
            get { return waitingForSemaphore; }
            set { waitingForSemaphore = value; }
        }

        public EnumThreadState CurrentState
        {
            get { return currentState; }
            set { currentState = value; }
        }

        public EnumThreadState PreviousState
        {
            get { return previousState; }
            set { previousState = value; }
        }

        public ThreadControlBlock ControlBlock
        {
            get { return controlBlock; }
            set { controlBlock = value; }
        }
    }
}
