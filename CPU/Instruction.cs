using System;

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
        private Int32 address;
        private Int32 size;
        private Int32 result;
        private string instructionString;

        #endregion Global Variables

        #region Constructors

        public Instruction()
        {
        }

        public Instruction(Int32 opcode, Int32 size)
        {
            this.opcode = opcode;
            this.operand1 = null;
            this.operand2 = null;
            this.size = size;
            instructionString = this.ToString();
            BindDelegate();
        }

        public Instruction(Int32 opcode, Operand op1, Int32 size)
        {
            this.opcode = opcode;
            this.operand1 = op1;
            this.operand2 = null;
            this.size = size;
            instructionString = this.ToString();
            BindDelegate();
        }

        public Instruction(Int32 opcode, Operand op1, Operand op2, Int32 size)
        {
            this.opcode = opcode;
            this.operand1 = op1;
            this.operand2 = op2;
            this.size = size;
            instructionString = this.ToString();
            BindDelegate();
        }

        #endregion Constructors

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

        public int Size
        {
            get
            {
                return size;
            }

            set
            {
                size = value;
            }
        }

        public int Address
        {
            get
            {
                return address;
            }

            set
            {
                address = value;
            }
        }

        public string InstructionString
        {
            get
            {
                return instructionString;
            }

            set
            {
                instructionString = value;
            }
        }

        #endregion Properties

        #region Methods

        public void BindDelegate()
        {
            switch (opcode)
            {
                case 0:
                    {
                        this.execute = () => Move(operand1, operand2);
                        break;
                    }
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

        public override string ToString()
        {
            string parsedOpcode = Enum.GetName(typeof(EnumOpcodes), opcode);
            string op1 = "";
            string op2 = "";
            if (operand1 != null)
            {
                if (operand1.IsRegister)
                {
                    op1 = Operand1.Register.Name;
                }
                else
                {
                    op1 = Operand1.Value.ToString();
                }
            }
            if (operand2 != null)
            {
                if (Operand2.IsRegister)
                {
                    op2 = Operand2.Register.Name;
                }
                else
                {
                    op2 = Operand2.Value.ToString();
                }
            }
            return parsedOpcode.ToUpper() + " " + op1.ToUpper() + "," + op2.ToUpper();
        }

        #endregion Methods

        #region Instruction Execution Functions

        private void Move(Operand lhs, Operand rhs)
        {
            result = rhs.Value;
            if (lhs.IsRegister)
            {
                lhs.Register.Value = result;
            }
        }
        private void Add(Operand lhs, Operand rhs)
        {
            //TODO Allow for memory operands
            result = lhs.Value + rhs.Value;
            if (lhs.IsRegister)
            {
                lhs.Register.Value = result;
            }
        }

        #endregion Instruction Execution Functions
    }
}