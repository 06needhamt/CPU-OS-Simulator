using System;

namespace CPU_OS_Simulator.Console
{
    public class NumberOfParametersAttribute : Attribute, IAttribute<int>
    {
        private int value;

        public NumberOfParametersAttribute(int value)
        {
            this.value = value;
        }
        public int Value
        {
            get { return value; }

            set { this.value = value; }
        }
    }
}