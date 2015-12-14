using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CPU_OS_Simulator.Compiler.Frontend
{
    public abstract class Token
    {
        protected EnumTokenType type;
        protected string value;

        public EnumTokenType Type
        {
            get { return type; }
            set { type = value; }
        }

        public string Value
        {
            get { return value; }
            set { this.value = value; }
        }

        public abstract Enum DetectType();
    }
}
