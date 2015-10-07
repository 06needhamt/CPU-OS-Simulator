using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CPU_OS_Simulator.CPU
{
    [Serializable]
    public class Instruction
    {
        private Int32 opcode;
        private string category;
        private Delegate exec;
        private Operand operand1;
        private Operand operand2;

        public Instruction(Int32 opcode)
        {
            this.opcode = opcode;
            this.operand1 = null;
            this.operand2 = null;
        }
        public int Opcode
        {
            get
            {
                return opcode;
            }

            set
            {
                opcode = value;
            }
        }

        public string Category
        {
            get
            {
                return category;
            }

            set
            {
                category = value;
            }
        }

        public Delegate Exec
        {
            get
            {
                return exec;
            }

            set
            {
                exec = value;
            }
        }

        public Operand Operand1
        {
            get
            {
                return operand1;
            }

            set
            {
                operand1 = value;
            }
        }

        public Operand Operand2
        {
            get
            {
                return operand2;
            }

            set
            {
                operand2 = value;
            }
        }
    }
}
