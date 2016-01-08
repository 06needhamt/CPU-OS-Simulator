using System.Collections.Generic;

namespace CPU_OS_Simulator.Memory
{
    /// <summary>
    /// This class represents swap space where swapped outmemory pages are moved to
    /// </summary>
    public class SwapSpace
    {
        private List<MemoryPage> swappedMemoryPages;
        
        /// <summary>
        /// Constructor for Swap Space
        /// </summary>
        public SwapSpace()
        {
            swappedMemoryPages = new List<MemoryPage>();
        }

        /// <summary>
        /// Property for the list of swapped out memory pages
        /// </summary>
        public List<MemoryPage> SwappedMemoryPages
        {
            get { return swappedMemoryPages; }
            set { swappedMemoryPages = value; }
        }

        //public void SwapOut(int LocationToSwap, int FrameNumber)
        //{
        //    MemoryPage temp;
        //    dynamic wind = GetMainWindowInstance();
        //    PhysicalMemory physicalMemory = wind.Memory;
        //    temp = physicalMemory.Pages[FrameNumber];
        //    if (!physicalMemory.Table.Entries[FrameNumber].SwappedOut)
        //    {
        //        physicalMemory.Table.Entries[FrameNumber].SwappedOut = true;
        //        physicalMemory.Pages.RemoveAt(FrameNumber);
        //        swappedMemoryPages.Add(temp);
        //     }
        //    else
        //    {
        //        MessageBox.Show("Cannot swap in page that is already swapped out", "ERROR", MessageBoxButton.OK,
        //            MessageBoxImage.Error);
        //    }

        //}

        //public void SwapIn(int LocationToSwap, int FrameNumber)
        //{
        //    MemoryPage temp;
        //    dynamic wind = GetMainWindowInstance();
        //    PhysicalMemory physicalMemory = wind.Memory;
        //    temp = swappedMemoryPages[FrameNumber];
        //    if (physicalMemory.Table.Entries[FrameNumber].SwappedOut)
        //    {
        //        physicalMemory.Table.Entries[FrameNumber].SwappedOut = false;
        //        physicalMemory.AddPage(temp,FrameNumber);
        //        swappedMemoryPages.RemoveAt(FrameNumber);
        //    }
        //    else
        //    {
        //        MessageBox.Show("Cannot swap in page that is already in memory", "ERROR", MessageBoxButton.OK,
        //            MessageBoxImage.Error);
        //    }
        //}

        //private dynamic GetMainWindowInstance()
        //{
        //    Assembly windowBridge = Assembly.LoadFrom("CPU_OS_Simulator.WindowBridge.dll"); // Load the window bridge module
        //    Console.WriteLine(windowBridge.GetExportedTypes()[0]);
        //    Type WindowType = windowBridge.GetType(windowBridge.GetExportedTypes()[1].ToString()); // get the name of the type that contains the window instances
        //    dynamic window = WindowType.GetField("MainWindowInstance").GetValue(null); // get the value of the static MainWindowInstance field
        //    return window;
        //}
    }
}