using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CPU_OS_Simulator.Memory
{
    public class PageTable
    {
        private int tableNumber;
        private List<PageTableEntry> entries;

        public int TableNumber
        {
            get { return tableNumber; }
            set { tableNumber = value; }
        }

        public List<PageTableEntry> Entries
        {
            get { return entries; }
            set { entries = value; }
        }
    }
}
