using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CPU_OS_Simulator.CPU
{
    public sealed class NumberOfOperandsAttribute : Attribute, IAttribute<int>
    {
        private readonly int value;

        public NumberOfOperandsAttribute(int value)
        {
            this.value = value;
        }

        public int Value
        {
            get
            {
                return value;
            }
        }
    }
}
