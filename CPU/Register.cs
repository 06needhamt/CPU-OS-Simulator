using System;

namespace CPU_OS_Simulator.CPU
{
    public class Register
    {
        private string name;
        private int value;
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

        public Register()
        {
        }

        protected Register(string name)
        {
            this.name = name;
            value = 0;
            type = EnumOperandType.VALUE;
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

        /// <summary>
        /// Sets the value in a register
        /// </summary>
        /// <param name="value"> the value to store in the register</param>
        /// <param name="type">the type of data memory or value</param>
        public void setRegisterValue(int value, EnumOperandType type)
        {
            this.value = value;
            this.type = type;
        }

        public static Register FindRegister(string selectedItem)
        {
            switch (selectedItem)
            {
                case "R00":
                    {
                        return R00;
                    }
                case "R01":
                    {
                        return R01;
                    }
                case "R02":
                    {
                        return R02;
                    }
                case "R03":
                    {
                        return R03;
                    }
                case "R04":
                    {
                        return R04;
                    }
                case "R05":
                    {
                        return R05;
                    }
                case "R06":
                    {
                        return R06;
                    }
                case "R07":
                    {
                        return R07;
                    }
                case "R08":
                    {
                        return R08;
                    }
                case "R09":
                    {
                        return R09;
                    }
                case "R10":
                    {
                        return R10;
                    }
                case "R11":
                    {
                        return R11;
                    }
                case "R12":
                    {
                        return R12;
                    }
                case "R13":
                    {
                        return R13;
                    }
                case "R14":
                    {
                        return R14;
                    }
                case "R15":
                    {
                        return R15;
                    }
                case "R16":
                    {
                        return R16;
                    }
                case "R17":
                    {
                        return R17;
                    }
                case "R18":
                    {
                        return R18;
                    }
                case "R19":
                    {
                        return R19;
                    }
                case "R20":
                    {
                        return R20;
                    }

                default:
                    {
                        return null;
                    }
            }
        }
    }
}