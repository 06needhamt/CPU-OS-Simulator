using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CPU_OS_Simulator.CPU.Interrupts
{
    [Serializable]
    public class InterruptVectorTable
    {
        private List<InterruptVectorTableEntry> entries;
    }
}
