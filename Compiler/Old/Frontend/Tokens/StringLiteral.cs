#pragma warning disable 1591
using System;

namespace CPU_OS_Simulator.Compiler.Old.Frontend.Tokens
{
    public class StringLiteral : Literal
    {
        public StringLiteral(string value)
        {
            this.value = value;
        }

        /// <summary>
        /// This function detects the type of literal
        /// </summary>
        /// <returns> the type of literal</returns>
        public override Enum DetectType()
        {
            return EnumTokenType.STRING_LITERAL;
        }

        /// <summary>
        /// This function identifies the type of data stored in the literal
        /// </summary>
        /// <returns> the type of value in the literal</returns>
        public override EnumTypes identfyValueType()
        {
            return EnumTypes.STRING;
        }
    }
}