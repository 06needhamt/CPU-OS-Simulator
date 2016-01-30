using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CPU_OS_Simulator.Operating_System.Threading
{
    [Serializable]
    public class SimulatorThread : IComparable<Int32>
    {
        private SimulatorProcess ownerProcess;
        private List<SimulatorThread> childThreads;
        private ThreadControlBlock controlBlock;
        private ThreadExecutionUnit executionUnit;
        private EnumThreadState currentState = EnumThreadState.UNKNOWN;
        private EnumThreadState previousState = EnumThreadState.UNKNOWN;
        private int threadPriority;
        private bool ownsSemaphore;
        private bool waitingForSemaphore;
        private bool resourceStarved;
        private List<SimulatorResource> allocatedResources;
        private List<SimulatorResource> requestedResources;
        private bool terminated;
        private List<LotteryTicket> lotteryTickets;

        /// <summary>
        /// Default Constructor for a thread used when deserialising threads
        /// NOTE: DO NOT USE IN CODE
        /// </summary>
        public SimulatorThread()
        {
            
        }

        public SimulatorThread(ThreadFlags flags)
        {
            ownerProcess = flags.ownerProcess;
            childThreads = flags.childThreads;
            controlBlock = flags.controlBlock;
            executionUnit = flags.executionUnit;
            currentState = flags.currentState;
            previousState = flags.previousState;
            threadPriority = flags.threadPriority;
            ownsSemaphore = flags.ownsSemaphore;
            waitingForSemaphore = flags.waitingForSemaphore;
            resourceStarved = flags.resourceStarved;
            allocatedResources = flags.allocatedResources;
            requestedResources = flags.requestedResources;
            terminated = flags.terminated;
            lotteryTickets = flags.lotteryTickets;
        }

        /// <summary>
        /// Compares the current object with another object of the same type.
        /// </summary>
        /// <returns>
        /// A value that indicates the relative order of the objects being compared. The return value has the following meanings: Value Meaning Less than zero This object is less than the <paramref name="other"/> parameter.Zero This object is equal to <paramref name="other"/>. Greater than zero This object is greater than <paramref name="other"/>. 
        /// </returns>
        /// <param name="other">An object to compare with this object.</param>
        public int CompareTo(int other)
        {
            if (threadPriority > other)
            {
                return -1;
            }
            else if (threadPriority == other)
            {
                return 0;
            }
            else if (threadPriority < other)
            {
                return 1;
            }
            return 0;
        }

        public bool isResourceStarved()
        {
            if (resourceStarved)
            {
                previousState = currentState;
                currentState = EnumThreadState.WAITING;
                return true;
            }
            return false;
        }

        public void Terminate()
        {
            terminated = true;
        }

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

        public ThreadControlBlock ControlBlock
        {
            get { return controlBlock; }
            set { controlBlock = value; }
        }

        public ThreadExecutionUnit ExecutionUnit
        {
            get { return executionUnit; }
            set { executionUnit = value; }
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

        public List<SimulatorResource> AllocatedResources
        {
            get { return allocatedResources; }
            set { allocatedResources = value; }
        }

        public List<SimulatorResource> RequestedResources
        {
            get { return requestedResources; }
            set { requestedResources = value; }
        }

        public bool Terminated
        {
            get { return terminated; }
            set { terminated = value; }
        }

        public List<LotteryTicket> LotteryTickets
        {
            get { return lotteryTickets; }
            set { lotteryTickets = value; }
        }

    }
}
