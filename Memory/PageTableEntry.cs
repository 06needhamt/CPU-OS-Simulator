namespace CPU_OS_Simulator.Memory
{
    /// <summary>
    /// This class represents an entry within a page table
    /// </summary>
    public class PageTableEntry
    {
        private int frameNumber;
        private int logicalAddress;
        private int physicalAddress;
        private bool swappedOut;
        private int faults;
        private MemoryPage page;

        /// <summary>
        /// Constructor for a page table entry
        /// </summary>
        /// <param name="frameNumber"> the frame number of this entry</param>
        /// <param name="logicalAddress"> the logical address of this entry</param>
        /// <param name="physicalAddress"> the physical address of this entry</param>
        /// <param name="swappedOut"> whether this page is swapped out</param>
        /// <param name="page"> the memory page associated with this entry</param>
        public PageTableEntry(int frameNumber, int logicalAddress, int physicalAddress, bool swappedOut, MemoryPage page)
        {
            this.frameNumber = frameNumber;
            this.logicalAddress = logicalAddress;
            this.physicalAddress = physicalAddress;
            this.swappedOut = swappedOut;
            faults = 0;
            this.page = page;
            page.FrameNumber = frameNumber;
        }
        /// <summary>
        /// Property for the frame number
        /// </summary>
        public int FrameNumber
        {
            get { return frameNumber; }
            set { frameNumber = value; }
        }
        /// <summary>
        /// property for the logical address
        /// </summary>
        public int LogicalAddress
        {
            get { return logicalAddress; }
            set { logicalAddress = value; }
        }
        /// <summary>
        /// Property for the physical address
        /// </summary>
        public int PhysicalAddress
        {
            get { return physicalAddress; }
            set { physicalAddress = value; }
        }
        /// <summary>
        /// Property for whether this page is swapped out
        /// </summary>
        public bool SwappedOut
        {
            get { return swappedOut; }
            set { swappedOut = value; }
        }
        /// <summary>
        /// Property for the page associated with this entry
        /// </summary>
        public MemoryPage Page
        {
            get { return page; }
            set { page = value; }
        }
        /// <summary>
        /// Property for how many faults have occurred with this entry.
        /// i.e. how many times it has been swapped in/out
        /// </summary>
        public int Faults
        {
            get { return faults; }
            set { faults = value; }
        }
    }
}
