using System;

namespace CPU_OS_Simulator.Console
{
    /// <summary>
    /// This class represents an attribute placed on console commands to indicate how many parameters they take
    /// </summary>
    public class NumberOfParametersAttribute : Attribute, IAttribute<int>
    {
        private int value;
        /// <summary>
        /// Constructor for NumberOfParametersAttribute
        /// </summary>
        /// <param name="value"> the value of the attribute</param>
        public NumberOfParametersAttribute(int value)
        {
            this.value = value;
        }
        /// <summary>
        /// Property to store the value of the attribute
        /// </summary>
        public int Value
        {
            get { return value; }

            set { this.value = value; }
        }
    }
}