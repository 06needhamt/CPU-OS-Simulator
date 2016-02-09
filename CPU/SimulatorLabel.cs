using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CPU_OS_Simulator.CPU
{
    [Serializable]
    public class SimulatorLabel
    {
        private SimulatorProgram program;

        public SimulatorProgram Program
        {
            get { return program; }
            set { program = value; }
        }
        private int logicalAddress;

        public int LogicalAddress
        {
            get { return logicalAddress; }
            set { logicalAddress = value; }
        }
        private int physicalAddress;

        public int PhysicalAddress
        {
            get { return physicalAddress; }
            set { physicalAddress = value; }
        }
        private string name;

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public SimulatorLabel()
        {

        }

        public SimulatorLabel(SimulatorProgram program, int logicalAddress, int physicalAddress, string name)
        {
            this.program = program;
            this.logicalAddress = logicalAddress;
            this.physicalAddress = physicalAddress;
            this.name = name;
        }

    }
}
