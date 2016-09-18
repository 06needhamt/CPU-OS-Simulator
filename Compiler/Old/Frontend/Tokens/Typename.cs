#pragma warning disable 1591
using System;

namespace CPU_OS_Simulator.Compiler.Old.Frontend.Tokens
{
    public class Typename : Token
    {
       public Typename(string value)
        {
            this.value = value;
        }
        public override Enum DetectType()
        {
            switch (value)
            {
                case "integer":
                    return EnumTypes.INTEGER;
                case "boolean":
                    return EnumTypes.BOOLEAN;
                case "byte":
                    return EnumTypes.BYTE;
                case "object":
                    return EnumTypes.OBJECT;
                case "string":
                    return EnumTypes.STRING;
                case "array":
                    return EnumTypes.ARRAY;
                default:
                    return EnumTypes.UNKNOWN;

            }
        }

        public EnumTypes GetTypename()
        {
            return (EnumTypes) type;
        }
    }
}