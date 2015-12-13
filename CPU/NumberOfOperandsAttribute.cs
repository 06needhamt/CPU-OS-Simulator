using System;

namespace CPU_OS_Simulator.CPU
{
    /// <summary>
    /// This class represents the NumberOfOperands attribute
    /// </summary>
    public sealed class NumberOfOperandsAttribute : Attribute, IAttribute<int>
    {
        /// <summary>
        /// The value of the attribute
        /// </summary>
        private readonly int value;

        /// <summary>
        /// Constructor for the NumberOFOperands attribute
        /// </summary>
        /// <param name="value"> the value to store in the attribute </param>
        public NumberOfOperandsAttribute(int value)
        {
            this.value = value;
        }
        /// <summary>
        ///  Property for the value of the attribute
        /// </summary>
        public int Value
        {
            get
            {
                return value;
            }
        }
    }
}