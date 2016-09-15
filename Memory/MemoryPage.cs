using System;
using System.Linq;
using System.Reflection;
using System.Windows;

namespace CPU_OS_Simulator.Memory
{
    /// <summary>
    /// This class represents a page of data within memory
    /// </summary>
    public class MemoryPage : ISwappable
    {
        private int pageIndex;
        private int startOffsetPhysical;
        private readonly int startOffset;
        private int frameNumber;
        private int processID;
        /// <summary>
        /// The size of the memory pages to manage
        /// </summary>
        public const int PAGE_SIZE = 256;
        private readonly int endOffset;
        private MemorySegment[] data;
        private string programName;
         /// <summary>
         /// The index of the current page within its program
         /// </summary>
        public int PageIndex
        {
            get
            {
                return pageIndex;
            }

            set
            {
                pageIndex = value;
            }
        }
        /// <summary>
        /// The start offset of this page
        /// </summary>
        public int StartOffset
        {
            get
            {
                return startOffset;
            }
        }
        /// <summary>
        /// the end offset of this page
        /// </summary>
        public int EndOffset
        {
            get
            {
                return endOffset;
            }
        }
        /// <summary>
        /// array of memory segments which collectively make up this page
        /// </summary>
        public MemorySegment[] Data
        {
            get
            {
                return data;
            }

            set
            {
                data = value;
            }
        }
        /// <summary>
        /// The physical address of the first byte in the page
        /// </summary>
        public int StartOffsetPhysical
        {
            get
            {
                return startOffsetPhysical;
            }

            set
            {
                startOffsetPhysical = value;
            }
        }
        /// <summary>
        /// Property for the current frame number of this page
        /// </summary>
        public int FrameNumber
        {
            get { return frameNumber; }
            set { frameNumber = value; }
        }
        /// <summary>
        /// Property for the name of the program that this page belongs to
        /// </summary>
        public string ProgramName
        {
            get { return programName; }
            set { programName = value; }
        }

        public int ProcessId
        {
            get { return processID; }
            set { processID = value; }
        }

        #region Constructors
        /// <summary>
        /// Constructor for memory page
        /// </summary>
        /// <param name="pageIndex"> the index of the page within the program</param>
        /// <param name="startOffset">the start offset of this page</param>
        /// <param name="programName"> the name of the program this belongs to</param>
        public MemoryPage(int pageIndex, int startOffset, string programName, int processID)
        {
            this.pageIndex = pageIndex;
            this.startOffset = startOffset;
            this.programName = programName;
            this.processID = processID;
            //this.PAGE_SIZE = pageSize;
            endOffset = startOffset + PAGE_SIZE;
            data = new MemorySegment[PAGE_SIZE / 8];
            PopulateData();

        }
        /// <summary>
        /// Populates the data in this page with its initial value (0)
        /// </summary>
        private void PopulateData()
        {
            dynamic wind = GetMainWindowInstance();
            PhysicalMemory mem = wind.Memory;
            int pageCount = mem.Table.Entries.Count;
            for (int i = 0; i < data.Length; i++)
            {
                if (data[i] == null)
                {
                    data[i] = new MemorySegment((pageCount*MemoryPage.PAGE_SIZE) + (i*8));
                    data[i].LogicalAddress = i*8;
                }
            }
        }

        #endregion Constructors
        /// <summary>
        /// This function swaps out this memory page
        /// </summary>
        /// <param name="LocationToSwap"> the physical address to swap from</param>
        /// <param name="FrameNumber"> this page's frame number</param>
        public void SwapOut(int LocationToSwap, int FrameNumber)
        {
            MemoryPage temp;
            dynamic wind = GetMainWindowInstance();
            PhysicalMemory physicalMemory = wind.Memory; // get a reference to physical memory from the main window
            SwapSpace swap = wind.SwapSpace;
            temp = physicalMemory.Pages[GetIndexMemory(FrameNumber)];
            if (!physicalMemory.Table.Entries[FrameNumber].SwappedOut) // if this memory page is not already swapped out
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
            PhysicalMemory physicalMemory = wind.Memory; // get a reference to physical memory from the main window
            SwapSpace swap = wind.SwapSpace;
            temp = swap.SwappedMemoryPages.Where(x => x.StartOffset 
            == physicalMemory.Table.Entries[FrameNumber].Page.startOffset).FirstOrDefault();
            int index = swap.SwappedMemoryPages.IndexOf(temp);
            if (physicalMemory.Table.Entries[FrameNumber].SwappedOut)// if this memory page is not already swapped in
            {
                physicalMemory.Table.Entries[FrameNumber].SwappedOut = false;
                physicalMemory.Table.Entries[FrameNumber].Faults++;
                for (int i = 0; i < temp.data.Length; i++)
                {
                    temp.data[i].PhysicalAddress = (frameNumber*MemoryPage.PAGE_SIZE)*(i*8);
                }
                physicalMemory.Pages.Add(temp);
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
        /// This function zeros out (clears) a memory page
        /// </summary>
        public void ZeroMemory()
        {
            for (int i = 0; i < data.Length; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    data[i].SetByte(j,0x00);
                }
            }
        }

        private int GetIndexMemory(int frameNumber)
        {
            dynamic wind = GetMainWindowInstance();
            PhysicalMemory memory = wind.Memory;
            int index = memory.Pages.IndexOf(memory.Pages.Where(x => x.FrameNumber == frameNumber).FirstOrDefault());
            return index;
        }

        private int GetIndexSwap(int frameNumber)
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