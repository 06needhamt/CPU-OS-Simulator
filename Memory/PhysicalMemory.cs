using System;
using System.Collections.Generic;
using System.Linq;

namespace CPU_OS_Simulator.Memory
{
    public class PhysicalMemory
    {
        private int capacity;
        private List<MemoryPage> pages;
        private PageTable pageTable;
        private SwapSpace swapSpace;

        public PhysicalMemory(int capacity)
        {
            this.capacity = capacity;
            pages = new List<MemoryPage>(capacity);
            pageTable = new PageTable();
            swapSpace = new SwapSpace();
        }

        public bool isFull()
        {
            return pages.Count >= capacity;
        }

        public MemoryPage AddPage(MemoryPage page, int index)
        {
            if (!isFull())
            {
                pages.Insert(index,page);
                pageTable.Entries.Add(new PageTableEntry(pageTable.Entries.Count + 1, page.StartOffset,
                    page.StartOffsetPhysical, false, page));
                
            }
            else
            {
                Random R = new Random(int.MinValue);
                int number = R.Next(0, pageTable.Entries.Count);
                MemoryPage swappedMemoryPage = pageTable.Entries[(R.Next(0,number)].Page;
                pageTable.Entries.ElementAt(number).SwappedOut = true;
                swapSpace.SwapOut(number,number);
                pages.Insert(index,page);
                /*pageTable.Entries.Add(new PageTableEntry(pageTable.Entries.Count + 1, page.StartOffset,
                    page.StartOffsetPhysical, false, page)); */
                pageTable.Entries[index].SwappedOut = true;
            }
            return page;
        }
        public int Capacity
        {
            get { return capacity; }
            set { capacity = value; }
        }

        public List<MemoryPage> Pages
        {
            get { return pages; }
            set { pages = value; }
        }

        public PageTable Table
        {
            get { return pageTable; }
            set { pageTable = value; }
        }

        public SwapSpace Space
        {
            get { return swapSpace; }
            set { swapSpace = value; }
        }
    }
}