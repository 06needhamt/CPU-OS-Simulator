using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows;

namespace CPU_OS_Simulator.Memory
{
    public class PhysicalMemory : ISwappable
    {
        private int capacity;
        private List<MemoryPage> pages;
        private PageTable pageTable;
        private SwapSpace swapSpace;

        public PhysicalMemory(int capacity)
        {
            this.capacity = capacity;
            pages = new List<MemoryPage>(capacity);
            pageTable = new PageTable(1);
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
                pageTable.Entries.Add(new PageTableEntry(pageTable.Entries.Count, page.StartOffset,
                    page.StartOffsetPhysical, false, page));
                
            }
            else
            {
                Random R = new Random(int.MinValue);
                int number = R.Next(0, pageTable.Entries.Count);
                while (pageTable.Entries[number].SwappedOut)
                {
                    number = R.Next(0, pageTable.Entries.Count);
                }
                MemoryPage swappedMemoryPage = pageTable.Entries[number].Page;
                //pageTable.Entries[number].SwappedOut = true;
                swappedMemoryPage.SwapOut(swappedMemoryPage.StartOffsetPhysical,number);
                pages.Insert(index - 1,page);
                /*pageTable.Entries.Add(new PageTableEntry(pageTable.Entries.Count + 1, page.StartOffset,
                    page.StartOffsetPhysical, false, page)); */
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
          public void SwapOut(int LocationToSwap, int FrameNumber)
        {
            MemoryPage temp;
            dynamic wind = GetMainWindowInstance();
            PhysicalMemory physicalMemory = wind.Memory;
            SwapSpace swap = wind.SwapSpace;
            temp = physicalMemory.Pages[FrameNumber];
            if (!physicalMemory.Table.Entries[FrameNumber].SwappedOut)
            {
                physicalMemory.Table.Entries[FrameNumber].SwappedOut = true;
                physicalMemory.Pages.RemoveAt(FrameNumber);
                swap.SwappedMemoryPages.Add(temp);
             }
            else
            {
                MessageBox.Show("Cannot swap out page that is already swapped out", "ERROR", MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }

        }

        public void SwapIn(int LocationToSwap, int FrameNumber)
        {
            MemoryPage temp;
            dynamic wind = GetMainWindowInstance();
            PhysicalMemory physicalMemory = wind.Memory;
            SwapSpace swap = wind.SwapSpace;
            temp = swap.SwappedMemoryPages[FrameNumber];
            if (physicalMemory.Table.Entries[FrameNumber].SwappedOut)
            {
                physicalMemory.Table.Entries[FrameNumber].SwappedOut = false;
                physicalMemory.AddPage(temp,FrameNumber);
                swap.SwappedMemoryPages.RemoveAt(FrameNumber);
            }
            else
            {
                MessageBox.Show("Cannot swap in page that is already in memory", "ERROR", MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }
        private dynamic GetMainWindowInstance()
        {
            Assembly windowBridge = Assembly.LoadFrom("CPU_OS_Simulator.WindowBridge.dll"); // Load the window bridge module
            Console.WriteLine(windowBridge.GetExportedTypes()[0]);
            Type WindowType = windowBridge.GetType(windowBridge.GetExportedTypes()[0].ToString()); // get the name of the type that contains the window instances
            dynamic window = WindowType.GetField("MainWindowInstance").GetValue(null); // get the value of the static MainWindowInstance field
            return window;
        }

        public MemoryPage RequestMemoryPage(int frameNumber)
        {
            PageTableEntry requiredPage = pageTable.Entries.FirstOrDefault(x => x.FrameNumber == frameNumber);
            if (requiredPage.SwappedOut)
            {
                requiredPage.Page.SwapIn(requiredPage.Page.StartOffsetPhysical, frameNumber);
            }
            return pages[frameNumber];
        }

    }
}