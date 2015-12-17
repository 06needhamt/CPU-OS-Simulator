﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CPU_OS_Simulator.Compiler.Frontend.Tokens;

namespace CPU_OS_Simulator.Compiler.Frontend.Symbols
{
    class Subroutine : Symbol
    {
        public Subroutine(string name, EnumTypes type, dynamic value)
        {
            this.name = name;
            this.type = type;
            this.value = value;
            this.issub = true;
            this.isfun = false;
        }
    }
}