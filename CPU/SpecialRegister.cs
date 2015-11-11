using System;

namespace CPU_OS_Simulator.CPU
{
    [Serializable]
    public class SpecialRegister : Register
    {
        public static SpecialRegister PC = new SpecialRegister("PC");
        public static SpecialRegister SR = new SpecialRegister("SR");
        public static SpecialRegister SP = new SpecialRegister("SP");
        public static SpecialRegister BR = new SpecialRegister("BR");
        public static SpecialRegister IR = new SpecialRegister("IR");
        public static SpecialRegister MDR = new SpecialRegister("MDR");
        public static SpecialRegister MAR = new SpecialRegister("MAR");

        public SpecialRegister() : base()
        {
        }

        protected SpecialRegister(string name) : base(name)
        {
            if (name.Equals("SP"))
            {
                this.Value = 8096;
            }
        }
    }
}