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
        #region Global Variables
        private Int32 opcode;
        private string category;
        private Action execute;
        private Operand operand1;
        private Operand operand2;
        private Int32 result;
        #endregion

        #region Constructors
        public Instruction(Int32 opcode)
        {
            this.opcode = opcode;
            this.operand1 = null;
            this.operand2 = null;
        }
        #endregion

        #region Properties
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

        public Action Execute
        {
            get
            {
                return execute;
            }

            set
            {
                execute = value;
            }
        }

        public int Result
        {
            get
            {
                return result;
            }

            set
            {
                result = value;
            }
        }
        #endregion

        #region Methods
        public void BindDelegate()
        {
            switch (opcode)
            {
                case 22:
                    {
                        this.execute = () => Add(operand1, operand2); // save the function in memory to call later
                        break;
                    }
                default:
                    {
                        break;
                    }
            }
        }
        #endregion

        #region Instruction Execution Functions
        private void Add(Operand lhs, Operand rhs)
        {
            result = lhs.Value + rhs.Value;
        }
        #endregion
    }
}
