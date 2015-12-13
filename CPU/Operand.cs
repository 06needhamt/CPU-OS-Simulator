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

        private int value;
        private EnumOperandType type;
        private bool isRegister;
        [NonSerialized]
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
        public Operand(int value, EnumOperandType type)
        {
            this.value = value;
            this.type = type;
            IsRegister = false;
            register = null;
        }

        /// <summary>
        /// Constructor for an operand which is a register
        /// </summary>
        /// <param name="reg">the register to be passed as an operand</param>
        /// <param name="type"> the type of the operand i.e memory address or intermediate value</param>
        public Operand(Register reg, EnumOperandType type)
        {
            isRegister = true;
            register = reg;
            value = reg.Value;
            this.type = type;
        }

        #endregion Constructors

        #region Properties
        /// <summary>
        /// Property for the value of the operand
        /// </summary>
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
        /// <summary>
        /// Property For the type of the operand
        /// </summary>
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
        /// <summary>
        /// Property for whether the operand is a register or not
        /// </summary>
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
        /// <summary>
        /// Property for the register that this operand represents
        /// or null if the operand is not a register
        /// </summary>
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