using System;
using CPU_OS_Simulator.Operating_System.Threading;

namespace CPU_OS_Simulator.Operating_System
{
    [Serializable]
    public class SimulatorResource
    {
        private string resourceName = String.Empty;
        private SimulatorProcess resourceOwner;
        private Semaphore semaphore;
        private bool hasSemaphore;

        public string ResourceName
        {
            get { return resourceName; }
        }

        public SimulatorProcess ResourceOwner
        {
            get
            {
                if (!hasSemaphore)
                    return resourceOwner;
                else
                    return semaphore.GetCurrentProcess();
            }
        }

        public Semaphore Semaphore
        {
            get { return semaphore; }
            set { semaphore = value; }
        }

        public bool HasSemaphore
        {
            get { return hasSemaphore; }
            set { hasSemaphore = value; }
        }
    }
}