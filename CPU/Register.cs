using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CPU_OS_Simulator.CPU
{
    public class Register
    {
        private string name;
        private Int32 value;
        private EnumOperandType type;

        public static Register R00 = new Register("R00");
        public static Register R01 = new Register("R01");
        public static Register R02 = new Register("R02");
        public static Register R03 = new Register("R03");
        public static Register R04 = new Register("R04");
        public static Register R05 = new Register("R05");
        public static Register R06 = new Register("R06");
        public static Register R07 = new Register("R07");
        public static Register R08 = new Register("R08");
        public static Register R09 = new Register("R09");
        public static Register R10 = new Register("R10");
        public static Register R11 = new Register("R11");
        public static Register R12 = new Register("R12");
        public static Register R13 = new Register("R13");
        public static Register R14 = new Register("R14");
        public static Register R15 = new Register("R15");
        public static Register R16 = new Register("R16");
        public static Register R17 = new Register("R17");
        public static Register R18 = new Register("R18");
        public static Register R19 = new Register("R19");
        public static Register R20 = new Register("R20");


        private Register(string name)
        {
            this.name = name;
            this.value = 0;
            this.type = EnumOperandType.VALUE;
        }

        public string Name
        {
            get
            {
                return name;
            }

            set
            {
                name = value;
            }
        }

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

        public void setRegisterValue(int value, EnumOperandType ismem)
        {
            Value = value;
            type = ismem;
        }
    }
}
