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

        #region Methods

        ///// <summary>
        ///// Overloaded equality operator for the operator class 
        ///// </summary>
        ///// <param name="lhs"> the left hand side of the operation</param>
        ///// <param name="rhs"> the right hand side of the operation</param>
        ///// <returns> true if the operands are equal false if not</returns>
        //public static bool operator ==(Operand lhs, Operand rhs)
        //{
        //    if (lhs.Equals(null) || rhs.Equals(null))
        //        return false;
        //    if(lhs.isRegister && rhs.isRegister)
        //        if (lhs.register.Name.Equals(rhs.register.Name))
        //            if(lhs.Type.Equals(rhs.Type))
        //                return true;
        //    if (lhs.value.Equals(rhs.value))
        //        if(lhs.type.Equals(rhs.Type))
        //            return true;
        //    return false;
        //}
        ///// <summary>
        ///// Overloaded not equality operator for the operator class 
        ///// </summary>
        ///// <param name="lhs"> the left hand side of the operation</param>
        ///// <param name="rhs"> the right hand side of the operation</param>
        ///// <returns> true if the operands are not equal false if they are </returns>
        //public static bool operator !=(Operand lhs, Operand rhs)
        //{
        //    if (lhs.Equals(null) || rhs.Equals(null))
        //        return false;
        //    if (lhs.isRegister && rhs.isRegister)
        //        if (lhs.register.Name.Equals(rhs.register.Name))
        //            if(lhs.Type.Equals(rhs.Type))
        //                return false;
        //    if (lhs.value.Equals(rhs.value))
        //        if(lhs.Type.Equals(rhs.Type))
        //            return false;
        //    return true;
        //}

        /// <summary>
        /// Determines whether the specified object is equal to the current object.
        /// </summary>
        /// <returns>
        /// true if the specified object  is equal to the current object; otherwise, false.
        /// </returns>
        /// <param name="obj">The object to compare with the current object. </param>
        public override bool Equals(object obj)
        {
            Operand op = obj as Operand;
            if ((object) op == null)
                return false;
            if(op.isRegister && this.isRegister)
                if (op.register.Name.Equals(this.register.Name))
                    if(op.Type.Equals(this.Type))
                        return true;
            if (op.value.Equals(this.value))
                if(op.Type.Equals(this.Type))
                    return true;
            return false;
        }
        #endregion Methods
    }
}
