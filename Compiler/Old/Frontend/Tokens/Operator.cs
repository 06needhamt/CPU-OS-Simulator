#pragma warning disable 1591
using System;

namespace CPU_OS_Simulator.Compiler.Old.Frontend.Tokens
{
    public class Operator : Token
    {
        private int operandCount;

        public Operator(string value)
        {
            this.value = value;
        }
        public override Enum DetectType()
        {
            switch (value)
            {
                case "+":
                    return EnumOperatorType.PLUS;
                case "-":
                    return EnumOperatorType.MINUS;
                case "*":
                    return EnumOperatorType.MULTIPLY;
                case "/":
                    return EnumOperatorType.DIVIDE;
                case "%":
                    return EnumOperatorType.MODULO;
                case "==":
                    return EnumOperatorType.EQUALITY;
                case "!=":
                    return EnumOperatorType.NOT_EQUAL;
                case "&":
                    return EnumOperatorType.AND;
                case "|":
                    return EnumOperatorType.OR;
                case "~":
                    return EnumOperatorType.NOT;
                case "^":
                    return EnumOperatorType.XOR;
                case "=":
                    return EnumOperatorType.ASSIGNMENT;
                default:
                    return EnumOperatorType.UNKNOWN;
            }
        }

        public EnumOperatorType GetOperatorType()
        {
            return (EnumOperatorType) type;
        }
    }
}
