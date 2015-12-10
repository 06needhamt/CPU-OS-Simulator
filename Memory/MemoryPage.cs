using System;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using System.Windows;

namespace CPU_OS_Simulator.Memory
{
    public class MemoryPage : ISwappable
    {
        private int pageIndex;
        private int startOffsetPhysical;
        private readonly int startOffset;
        public const int PAGE_SIZE = 256;
        private readonly int endOffset;
        private MemorySegment[] data;

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

        public int StartOffset
        {
            get
            {
                return startOffset;
            }
        }

        public int PageSize
        {
            get
            {
                return PAGE_SIZE;
            }
        }

        public int EndOffset
        {
            get
            {
                return endOffset;
            }
        }

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

        #region Constructors

        public MemoryPage(int pageIndex, int startOffset)
        {
            this.pageIndex = pageIndex;
            this.startOffset = startOffset;
            //this.PAGE_SIZE = pageSize;
            endOffset = startOffset + PAGE_SIZE;
            data = new MemorySegment[PAGE_SIZE / 8];
            PopulateData();

        }

        private void PopulateData()
        {
            for (int i = 0; i < data.Length; i++)
            {
                data[i] = new MemorySegment(startOffset + (i*8));
                data[i].LogicalAddress = i*8;
            }
        }

        #endregion Constructors

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
                MessageBox.Show("Cannot swap in page that is already swapped out", "ERROR", MessageBoxButton.OK,
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
                physicalMemory.AddPage(temp, FrameNumber);
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

        public void ZeroMemory()
        {
            for (int i = 0; i < data.Length; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    data[i].SetByte(j,0xAA);
                }
            }
        }

    }
}