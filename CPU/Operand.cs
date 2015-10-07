using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CPU_OS_Simulator.CPU
{
    [Serializable]
    public class Operand
    {
        private Int32 value;
        private EnumOperandType type;

        public Operand(Int32 value, EnumOperandType type)
        {
            this.value = value;
            this.type = type;
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

        internal EnumOperandType Type
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
    }
}
