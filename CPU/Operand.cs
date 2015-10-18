using System;

namespace CPU_OS_Simulator.CPU
{
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

        public Operand()
        {
        }

        public Operand(Int32 value, EnumOperandType type)
        {
            this.value = value;
            this.type = type;
            IsRegister = false;
            this.register = null;
        }

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