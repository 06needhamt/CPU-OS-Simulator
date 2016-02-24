using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using Newtonsoft.Json; // See Third Party Libs/Credits.txt for licensing information for JSON.Net

namespace CPU_OS_Simulator.Interrupts
{
    [Serializable]
    public class InterruptHandler
    {
        [JsonIgnore]
        [ScriptIgnore]
        private Func<int> handlerFunction;

        public InterruptHandler(Func<int> handlerFunction)
        {
            this.handlerFunction = handlerFunction;
        }

        public Func<int> HandlerFunction
        {
            get { return handlerFunction; }
        }
    }
}
