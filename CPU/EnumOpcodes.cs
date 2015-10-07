using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CPU_OS_Simulator.CPU
{
    public enum EnumOpcodes
    {
        MOV = 0,
        MVS = 1,
        CVS = 2,
        CVI = 3,
        LDB = 4,
        LDW = 5,
        LNS = 6,
        LDBI = 7,
        LDWI = 8,
        TAS = 9,
        STB = 10,
        STW = 11,
        STBI = 12,
        STWI = 13,
        PUSH = 14,
        POP = 15,
        SWP = 16,
        AND = 17,
        OR = 18,
        NOT = 19,
        SHL = 20,
        SHR = 21,
    }
}
