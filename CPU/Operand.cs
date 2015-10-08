﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CPU_OS_Simulator.CPU
{
    [Serializable]
    public class Operand
    {
        #region Global Variables
        private Int32 value;
        private EnumOperandType type;
        #endregion

        #region Constructors
        public Operand(Int32 value, EnumOperandType type)
        {
            this.value = value;
            this.type = type;
        }
        public Operand(Register reg, EnumOperandType type)
        {
            this.value = reg.Value;
            this.type = type;
        }
        #endregion

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
        #endregion
    }
}
