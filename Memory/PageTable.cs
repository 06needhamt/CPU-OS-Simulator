using System.Collections.Generic;

namespace CPU_OS_Simulator.Memory
{
    /// <summary>
    /// This class represents a page table
    /// where information about the loctions of memory pages are kept
    /// </summary>
    public class PageTable
    {
        private int tableNumber;
        private List<PageTableEntry> entries;
        /// <summary>
        /// Constructor for a page table with no entries
        /// </summary>
        /// <param name="number"> the number of the page table</param>
        public PageTable(int number)
        {
            tableNumber = number;
            entries = new List<PageTableEntry>();
        }
        /// <summary>
        /// Constructor for a page table with entries
        /// </summary>
        /// <param name="number"> the number of the page table</param>
        /// <param name="entries"> the entries to place in the table</param>
        public PageTable(int number, List<PageTableEntry> entries)
        {
            tableNumber = number;
            this.entries = entries;
        }
        /// <summary>
        /// Property for the table number
        /// </summary>
        public int TableNumber
        {
            get { return tableNumber; }
            set { tableNumber = value; }
        }
        /// <summary>
        /// Property for the table entries
        /// </summary>
        public List<PageTableEntry> Entries
        {
            get { return entries; }
            set { entries = value; }
        }
    }
}
