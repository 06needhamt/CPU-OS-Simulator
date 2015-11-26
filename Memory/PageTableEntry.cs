using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CPU_OS_Simulator.Memory
{
    public class PageTableEntry
    {
        private int frameNumber;
        private int logicalAddress;
        private int physicalAddress;
        private bool swappedOut;
        private MemoryPage page;

        public int FrameNumber
        {
            get { return frameNumber; }
            set { frameNumber = value; }
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

        public bool SwappedOut
        {
            get { return swappedOut; }
            set { swappedOut = value; }
        }

        public MemoryPage Page
        {
            get { return page; }
            set { page = value; }
        }
    }
}
