#pragma warning disable 1591
using System;

namespace CPU_OS_Simulator.Compiler.Old.Frontend.Tokens
{
    public class GenericToken : Token
    {
        public GenericToken(string value)
        {
            this.value = value;
        }

        public override Enum DetectType()
        {
            double outpar = 0.0;
            if (Double.TryParse(value, out outpar))
                return EnumTokenType.NUMBER;
            if (value.StartsWith("%"))
                return EnumTokenType.COMMENT;
            if(value.StartsWith("\"") || value.EndsWith("\""))
            {
                return EnumTokenType.STRING;
            } 
            switch (value)
            {
                case "(":
                    return EnumTokenType.OPENING_BRACE;
                case ")":
                    return EnumTokenType.CLOSING_BRACE;
                case "{":
                    return EnumTokenType.OPENING_CURLY_BRACE;
                case "}":
                    return EnumTokenType.CLOSING_CURLY_BRACE;
                case "\n":
                    return EnumTokenType.NEW_LINE;
                case "\r":
                    return EnumTokenType.NEW_LINE;
                case ",": 
                    return EnumTokenType.COMMA;
                case ".":
                    return EnumTokenType.DOT;
                case "\t":
                    return EnumTokenType.TAB;
                case "":
                    return EnumTokenType.END_OF_FILE;
                default:
                    return EnumTokenType.UNKNOWNN;
            }
        }

        public EnumTokenType GetTokenType()
        {
            return (EnumTokenType) type;
        }
    }

}
    
      