using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using Newtonsoft.Json; // See Third Party Libs/Credits.txt for licensing information for JSON.Net

namespace CPU_OS_Simulator.CPU.Interrupts
{
    [Serializable]
    public class InterruptHandler
    {
        [JsonIgnore]
        [ScriptIgnore]
        private Func<int> handlerFunction;
        private int logicalAddress;
        private int physicalAddress;

        public InterruptHandler()
        {
            
        }

        public InterruptHandler(Func<int> handlerFunction)
        {
            this.handlerFunction = handlerFunction;
        }

        public InterruptHandler(int logicalAddress)
        {
            this.logicalAddress = logicalAddress;
        }

        [JsonIgnore]
        [ScriptIgnore]
        public Func<int> HandlerFunction
        {
            get { return handlerFunction; }
        }

        public int LogicalAddress
        {
            get { return logicalAddress; }
            set { logicalAddress = value; }
        }

        public int PhysicalAddress
        {
            get { return physicalAddress; }
            set { physicalAddress = value; }
        }

    }
}
