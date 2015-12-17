﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CPU_OS_Simulator.Compiler.Frontend.Tokens
{
    public class NumericLiteral : Literal
    {
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
            return EnumTypes.INTEGER;
        }
    }
}