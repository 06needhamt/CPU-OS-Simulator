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
            int numberOfPages = size / MemoryPage.PAGE_SIZE;
            dynamic wind = GetMainWindowInstance();
            int index = 0;
            PhysicalMemory memory = wind.Memory;
            for (int i = 0; i < numberOfPages; i++)
            {
                memory.RequestMemoryPage(frameNumber + i);
                memory.Pages[frameNumber + i].ZeroMemory();
                for (int j = 0; j < memory.Pages[frameNumber + i].Data.Length; j++)
                {
                    for (int k = 0; k < 7; k++)
                    {
                        memory.Pages[frameNumber + i].Data[j].SetByte(k,bytes[index]);
                        index++;
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