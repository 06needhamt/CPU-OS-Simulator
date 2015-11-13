using System;
using System.Reflection;

namespace CPU_OS_Simulator.CPU
{
    [Serializable]
    public class SpecialRegister : Register
    {
        public static SpecialRegister PC = new SpecialRegister("PC");
        public static SpecialRegister SR = new SpecialRegister("SR");
        public static SpecialRegister SP = new SpecialRegister("SP");
        public static SpecialRegister BR = new SpecialRegister("BR");
        public static SpecialRegister IR = new SpecialRegister("IR");
        public static SpecialRegister MDR = new SpecialRegister("MDR");
        public static SpecialRegister MAR = new SpecialRegister("MAR");

        public SpecialRegister() : base()
        {
        }

        protected SpecialRegister(string name) : base(name)
        {
            if (name.Equals("SP"))
            {
                this.Value = 8096;
            }
        }

        public static void UpdateSpecialRegisters()
        {
            dynamic mainWindow = GetMainWindowInstance();
            // TODO update special register values in UI.
        }

        /// <summary>
        /// This function gets the main window instance from the window bridge
        /// </summary>
        /// <returns> the active instance of main window </returns>
        private static dynamic GetMainWindowInstance()
        {
            Assembly windowBridge = Assembly.LoadFrom("CPU_OS_Simulator.WindowBridge.dll"); // Load the window bridge module
            Console.WriteLine(windowBridge.GetExportedTypes()[0]);
            Type WindowType = windowBridge.GetType(windowBridge.GetExportedTypes()[0].ToString()); // get the name of the type that contains the window inatances
            dynamic window = WindowType.GetField("MainWindowInstance").GetValue(null); // get the value of the static MainWindowInstance field
            return window;
        }
    }
}