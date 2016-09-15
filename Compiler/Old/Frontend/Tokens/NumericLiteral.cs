using System;

namespace CPU_OS_Simulator.Compiler.Old.Frontend.Tokens
{
    public class NumericLiteral : Literal
    {
        int number;
        public NumericLiteral(string value)
        {
            this.value = value;
        }
        /// <summary>
        /// This function detects the type of literal
        /// </summary>
        /// <returns> the type of literal</returns>
        public override Enum DetectType()
        {
            return EnumTokenType.NUMERIC_LITERAL;
        }

        /// <summary>
        /// This function identifies the type of data stored in the literal
        /// </summary>
        /// <returns> the type of value in the literal</returns>
        public override EnumTypes identfyValueType()
        {
            if(value.StartsWith("0x"))
            {
                number = Convert.ToInt32(value, 16);
            }
            else if(value.StartsWith("0b"))
            {
                number = Convert.ToInt32(value,2);
            }
            else if (value.StartsWith("0") && value.Length > 1)
            {
                number = Convert.ToInt32(value,8);
            }
            else
            {
                number = Convert.ToInt32(value,10);
            }
            return EnumTypes.INTEGER;
        }

        public int Number { get { return number; } }
    }
}
