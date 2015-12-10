using System;
using CPU_OS_Simulator.CPU;
using CPU_OS_Simulator.Memory;
using System.Collections.Generic;
using System.Reflection;
using System.Security.Cryptography;

namespace CPU_OS_Simulator.CPU
{
    public class CompiledProgram
    {
        private List<byte> bytes = null;
        private string name = String.Empty;
        private int size = 0;
        public CompiledProgram(List<Byte> bytes, string name, int size)
        {
            this.bytes = bytes;
            this.name = name;
            this.size = size;
        }

        public bool LoadinMemory(int frameNumber)
        {
            double DnumberOfPages = (double) size/MemoryPage.PAGE_SIZE;
            int numberOfPages = (int) Math.Ceiling(DnumberOfPages);
            dynamic wind = GetMainWindowInstance();
            PhysicalMemory memory = wind.Memory;
            int bytecount = 0;
            for (int i = 0; i <= numberOfPages - 1; i++)
            {
                for (int pageoffset = 0; pageoffset < MemoryPage.PAGE_SIZE; pageoffset += 8)
                {
                    int row = pageoffset/8;
                    for (int j = 0; j < 7; j++)
                    {
                        if (bytecount < bytes.Count)
                        {
                            memory.Pages[frameNumber].Data[row].SetByte(j,bytes[bytecount]);
                            bytecount++;
                        }
                    }
                    
                }
            }
            return true;
        }

        private dynamic GetMainWindowInstance()
    {
        Assembly windowBridge = Assembly.LoadFrom("CPU_OS_Simulator.WindowBridge.dll"); // Load the window bridge module
        Console.WriteLine(windowBridge.GetExportedTypes()[0]);
        Type WindowType = windowBridge.GetType(windowBridge.GetExportedTypes()[0].ToString()); // get the name of the type that contains the window instances
        dynamic window = WindowType.GetField("MainWindowInstance").GetValue(null); // get the value of the static MainWindowInstance field
        return window;
    }
}
}