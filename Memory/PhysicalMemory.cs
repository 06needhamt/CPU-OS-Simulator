using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using System.Windows;

namespace CPU_OS_Simulator.Memory
{   
    /// <summary>
    /// This class represents physical memory (RAM)
    /// </summary>
    public class PhysicalMemory : ISwappable
    {
        private int capacity;
        private List<MemoryPage> pages;
        private PageTable pageTable;
        private SwapSpace swapSpace;

        /// <summary>
        /// Constructor for physical memory
        /// </summary>
        /// <param name="capacity"> capacity of physical memory in pages</param>
        public PhysicalMemory(int capacity)
        {
            this.capacity = capacity;
            pages = new List<MemoryPage>(capacity);
            pageTable = new PageTable(1);
            swapSpace = new SwapSpace();
        }
        /// <summary>
        /// Returns whether memory is currently full
        /// </summary>
        /// <returns>whether memory is currently full</returns>
        public bool isFull()
        {
            return pages.Count >= capacity;
        }
        /// <summary>
        /// This function adds a page into memory
        /// </summary>
        /// <param name="page"> the page to add to memory </param>
        /// <param name="index"> the index to add the page to memory</param>
        /// <returns> a reference to the page that has been added to memory</returns>
        public MemoryPage AddPage(MemoryPage page, int index)
        {
            if (!isFull()) // if memory is not full
            {
                pages.Add(page);
                pageTable.Entries.Add(new PageTableEntry(pageTable.Entries.Count, page.StartOffset,
                    page.StartOffsetPhysical, false, page)); // add it to memory
                
            }
            else // if memory is full
            {
                Random R = new Random(); // generate a random number between 0 and the number of pages 
                int number = R.Next(0, pageTable.Entries.Count - 1);
                while (pageTable.Entries[number].SwappedOut) // if this page is already swapped out generate another number
                {
                    number = R.Next(0, pageTable.Entries.Count - 1 );
                }
                MemoryPage swappedMemoryPage = pageTable.Entries[number].Page; 
                //pageTable.Entries[number].SwappedOut = true;
                swappedMemoryPage.SwapOut(swappedMemoryPage.StartOffsetPhysical,number); // swap out this memory page
                pages.Add(page); // insert the new memory page
                /*pageTable.Entries.Add(new PageTableEntry(pageTable.Entries.Count + 1, page.StartOffset,
                    page.StartOffsetPhysical, false, page)); */
            }
            return page;
        }
        /// <summary>
        /// Property for the capacity of memory
        /// </summary>
        public int Capacity
        {
            get { return capacity; }
            set { capacity = value; }
        }
        /// <summary>
        /// Property for the list of pages currently in memory
        /// </summary>
        public List<MemoryPage> Pages
        {
            get { return pages; }
            set { pages = value; }
        }
        /// <summary>
        /// Property for the page table
        /// </summary>
        public PageTable Table
        {
            get { return pageTable; }
            set { pageTable = value; }
        }
        /// <summary>
        /// Property for the swap space where swapped out memory pages are held
        /// </summary>
        public SwapSpace Space
        {
            get { return swapSpace; }
            set { swapSpace = value; }
        }
        /// <summary>
        /// This function swaps out this memory page
        /// </summary>
        /// <param name="LocationToSwap"> the physical address to swap from</param>
        /// <param name="FrameNumber"> this page's frame number</param>
        public void SwapOut(int LocationToSwap, int FrameNumber)
        {
            MemoryPage temp;
            dynamic wind = GetMainWindowInstance();
            PhysicalMemory physicalMemory = wind.Memory;
            SwapSpace swap = wind.SwapSpace;
            temp = RequestMemoryPage(FrameNumber);
            if (!physicalMemory.Table.Entries[FrameNumber].SwappedOut)
            {
                physicalMemory.Table.Entries[FrameNumber].SwappedOut = true;
                physicalMemory.Table.Entries[FrameNumber].Faults++;
                physicalMemory.Pages.RemoveAt(GetIndexMemory(FrameNumber));
                swap.SwappedMemoryPages.Add(temp);
             }
            else
            {
                MessageBox.Show("Cannot swap out page that is already swapped out", "ERROR", MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }

        }
        /// <summary>
        /// This function swaps in this memory page
        /// </summary>
        /// <param name="LocationToSwap"> the physical address to swap this page in to</param>
        /// <param name="FrameNumber"> this pages frame number</param>
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
                physicalMemory.Table.Entries[FrameNumber].Faults++;
                physicalMemory.AddPage(temp,FrameNumber);
                swap.SwappedMemoryPages.RemoveAt(GetIndexSwap(FrameNumber));
            }
            else
            {
                MessageBox.Show("Cannot swap in page that is already in memory", "ERROR", MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }
        /// <summary>
        /// This function gets the main window instance from the window bridge
        /// </summary>
        /// <returns> the active instance of main window </returns>
        private dynamic GetMainWindowInstance()
        {
            Assembly windowBridge = Assembly.LoadFrom("CPU_OS_Simulator.WindowBridge.dll"); // Load the window bridge module
            Console.WriteLine(windowBridge.GetExportedTypes()[0]);
            Type WindowType = windowBridge.GetType(windowBridge.GetExportedTypes()[1].ToString()); // get the name of the type that contains the window instances
            dynamic window = WindowType.GetField("MainWindowInstance").GetValue(null); // get the value of the static MainWindowInstance field
            return window;
        }
        /// <summary>
        /// Requests a memory page to be swapped into memory
        /// </summary>
        /// <param name="frameNumber"> the frame number of the requested memory page</param>
        /// <returns> the requested memory page</returns>
        public MemoryPage RequestMemoryPage(int frameNumber)
        {
            try
            {
                PageTableEntry requiredPage = pageTable.Entries.FirstOrDefault(x => x.FrameNumber == frameNumber);
                if (requiredPage.SwappedOut)
                {
                    requiredPage.Page.SwapIn(requiredPage.Page.StartOffsetPhysical, frameNumber);
                }
                return pages[frameNumber];
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
                MessageBox.Show("The requested page is not available");
                return null;
            }
            
           
        }

        public int GetIndexMemory(int frameNumber)
        {
            dynamic wind = GetMainWindowInstance();
            PhysicalMemory memory = wind.Memory;
            int index = memory.Pages.IndexOf(memory.Pages.Where(x => x.FrameNumber == frameNumber).FirstOrDefault());
            return index;
        }

        public int GetIndexSwap(int frameNumber)
        {
            dynamic wind = GetMainWindowInstance();
            SwapSpace swap = wind.SwapSpace;
            int index =
                swap.SwappedMemoryPages.IndexOf(
                    swap.SwappedMemoryPages.Where(x => x.FrameNumber == frameNumber).FirstOrDefault());
            return index;

        }

    }
}