using System;

namespace CPU_OS_Simulator.CPU
{
    /// <summary>
    /// This class represents an operand which can be passed to an instruction
    /// </summary>
    [Serializable]
    public class Operand
    {
        #region Global Variables

        private Int32 value;
        private EnumOperandType type;
        private bool isRegister;
        private Register register;

        #endregion Global Variables

        #region Constructors

        /// <summary>
        /// Default constructor for an operand used when deserialising an operand
        /// NOTE do not use in code
        /// </summary>
        public Operand()
        {
        }

        /// <summary>
        /// Constructor for an operand which is an intermediate value
        /// </summary>
        /// <param name="value"> the value of the operand </param>
        /// <param name="type"> the type of the operand i.e memory address or intermediate value</param>
        public Operand(Int32 value, EnumOperandType type)
        {
            this.value = value;
            this.type = type;
            IsRegister = false;
            this.register = null;
        }

        /// <summary>
        /// Constructor for an operand which is a register
        /// </summary>
        /// <param name="reg">the register to be passed as an operand</param>
        /// <param name="type"> the type of the operand i.e memory address or intermediate value</param>
        public Operand(Register reg, EnumOperandType type)
        {
            this.isRegister = true;
            this.register = reg;
            this.value = reg.Value;
            this.type = type;
        }

        #endregion Constructors

        #region Properties

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

        public bool IsRegister
        {
            get
            {
                return isRegister;
            }

            set
            {
                isRegister = value;
            }
        }

        public Register Register
        {
            get
            {
                return register;
            }

            set
            {
                register = value;
            }
        }

        #endregion Properties
    }
}