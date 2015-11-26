using System;
using System.Reflection;

namespace CPU_OS_Simulator.CPU
{
    /// <summary>
    /// This class represents a special register i.e a non general purpose register
    /// </summary>
    [Serializable]
    public class SpecialRegister
    {
        /// <summary>
        /// The name of the Special Register
        /// </summary>
        private string name;

        /// <summary>
        /// The integer value of the special register
        /// </summary>
        private int value;

        /// <summary>
        /// The string value of the register used </para>
        /// for the instruction register (IR) </para>
        /// and the memory data register (MDR) </para>
        /// </summary>
        private string valueString;

        /// <summary>
        /// The type of value stored in the register </para>
        /// i.e intermediate value or memory address
        /// </summary>
        private EnumOperandType type;

        /// <summary>
        /// The Program Counter register
        /// </summary>
        public static SpecialRegister PC = new SpecialRegister("PC");

        /// <summary>
        /// The Stack Register
        /// </summary>
        public static SpecialRegister SR = new SpecialRegister("SR");

        /// <summary>
        /// The Stack Pointer Register
        /// </summary>
        public static SpecialRegister SP = new SpecialRegister("SP");

        /// <summary>
        ///
        /// </summary>
        public static SpecialRegister BR = new SpecialRegister("BR");

        /// <summary>
        /// The Instruction Register
        /// </summary>
        public static SpecialRegister IR = new SpecialRegister("IR");

        /// <summary>
        /// The Memory Data Register
        /// </summary>
        public static SpecialRegister MDR = new SpecialRegister("MDR");

        /// <summary>
        /// The Memory LogicalAddress Register
        /// </summary>
        public static SpecialRegister MAR = new SpecialRegister("MAR");

        /// <summary>
        /// Default constructor for Special Register
        /// used when deserialising a special register. </para>
        /// NOTE Do Not use in code
        /// </summary>
        public SpecialRegister()
        {
        }

        /// <summary>
        /// Primary constructor for a special register
        /// </summary>
        /// <param name="name"> The name of the register</param>
        protected SpecialRegister(string name)
        {
            this.name = name;
            value = 0;
            type = EnumOperandType.VALUE;

            if (name.Equals("SP"))
            {
                Value = 8096; // initialize the stack pointer to 8096
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static SpecialRegister FindSpecialRegister(string name)
        {
            switch (name)
            {
                case "PC":
                    {
                        return PC;
                    }
                case "SR":
                    {
                        return SR;
                    }
                case "SP":
                    {
                        return SP;
                    }
                case "BR":
                    {
                        return BR;
                    }
                case "IR":
                    {
                        return IR;
                    }
                case "MDR":
                    {
                        return MDR;
                    }
                case "MAR":
                    {
                        return MAR;
                    }
                default:
                    {
                        throw new Exception("Tried to fetch non-existent special register");
                    }
            }
        }

        /// <summary>
        /// Sets the value in a special register
        /// </summary>
        /// <param name="value"> the value to store in the register</param>
        /// <param name="type">the type of data memory or value</param>
        public void setRegisterValue(int value, EnumOperandType type)
        {
            this.value = value;
            this.type = type;
        }

        /// <summary>
        /// Sets the string value in a special register
        /// </summary>
        /// <param name="value"> the value to store in the register</param>
        /// <param name="type">the type of data memory or value</param>
        public void setRegisterValue(string value, EnumOperandType type)
        {
            valueString = value;
            this.type = type;
        }

        /// <summary>
        /// This function gets the main window instance from the window bridge
        /// </summary>
        /// <returns> the active instance of main window </returns>
        private static dynamic GetMainWindowInstance()
        {
            Assembly windowBridge = Assembly.LoadFrom("CPU_OS_Simulator.WindowBridge.dll"); // Load the window bridge module
            Console.WriteLine(windowBridge.GetExportedTypes()[0]);
            Type WindowType = windowBridge.GetType(windowBridge.GetExportedTypes()[0].ToString()); // get the name of the type that contains the window instances
            dynamic window = WindowType.GetField("MainWindowInstance").GetValue(null); // get the value of the static MainWindowInstance field
            return window;
        }

        public string Name
        {
            get
            {
                return name;
            }

            set
            {
                name = value;
            }
        }

        public int Value
        {
            get
            {
                return value;
            }

            set
            {
                this.value = value;
            }
        }

        public EnumOperandType Type
        {
            get
            {
                return type;
            }

            set
            {
                type = value;
            }
        }

        public string ValueString
        {
            get
            {
                return valueString;
            }

            set
            {
                valueString = value;
            }
        }
    }
}