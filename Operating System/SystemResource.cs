using System;

namespace CPU_OS_Simulator.Operating_System
{
    [Serializable]
    public class SystemResource
    {
        private string resourceName = String.Empty;
        private SimulatorProcess resourceOwner;

        public string ResourceName
        {
            get { return resourceName; }
        }

        public SimulatorProcess ResourceOwner
        {
            get { return resourceOwner; }
        }

        //TODO implement me 
    }
}