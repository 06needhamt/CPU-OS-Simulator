using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CPU_OS_Simulator.Compiler.Frontend
{
    public class Token
    {
        private string value;
        private EnumTokenType type;

        public Token(string value, EnumTokenType type)
        {
            this.value = value;
            this.type = type;
        }

        public string Value
        {
            get { return value; }
            set { this.value = value; }
        }

        public EnumTokenType Type
        {
            get { return type; }
            set { type = value; }
        }
    }
}
