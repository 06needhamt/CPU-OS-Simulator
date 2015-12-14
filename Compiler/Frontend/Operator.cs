using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CPU_OS_Simulator.Compiler.Frontend
{
    public class Operator : Token
    {
        private EnumOperatorType opType = EnumOperatorType.UNKNOWN;
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
                case "=":
                    return EnumOperatorType.ASSIGNMENT;
                default:
                    return EnumOperatorType.UNKNOWN;
            }
        }
    }
}
