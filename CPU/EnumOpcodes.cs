using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CPU_OS_Simulator.CPU
{
    public enum EnumOpcodes
    {
        #region Data Transfer
        [Description("Data Transfer: MOV Moves data to register")]
        MOV = 0,
        [Description("Data Transfer: MVS Moves string to memory")]
        MVS = 1,
        [Description("Data Transfer: CVS Not currently used")]
        CVS = 2,
        [Description("Data Transfer: CVI Not currently used")]
        CVI = 3,
        [Description("Data Transfer: LDB Loads byte from memory into register")]
        LDB = 4,
        [Description("Data Transfer: LDW Loads word from memory into register")]
        LDW = 5,
        [Description("Data Transfer: LNS  Loads word from memory into register - No SR update")]
        LNS = 6,
        [Description("Data Transfer: LDBI Loads byte from memory into register, increments indirect source address")]
        LDBI = 7,
        [Description("Data Transfer: LDWI  Loads word from memory into register, increments indirect source address")]
        LDWI = 8,
        [Description("Data Transfer: TAS Test and set")]
        TAS = 9,
        [Description("Data Transfer: STB Stores byte to memory")]
        STB = 10,
        [Description("Data Transfer: STW Stores word to memory")]
        STW = 11,
        [Description("Data Transfer: STBI Stores byte to memory, increments indirect destination address")]
        STBI = 12,
        [Description("Data Transfer: STWI Stores word to memory, increments indirect destination address")]
        STWI = 13,
        [Description("Data Transfer: PUSH Pushes value or value in register onto top of stack")]
        PUSH = 14,
        [Description("Data Transfer: POP Pops value from top of stack into register")]
        POP = 15,
        [Description("Data Transfer: SWP Swaps two register values")]
        SWP = 16,
        #endregion
        #region Logical
        [Description("Logical: AND Logical AND of registers")]
        AND = 17,
        [Description("Logical: OR Logical OR of registers")]
        OR = 18,
        [Description("Logical: NOT Logical NOT of registers")]
        NOT = 19,
        [Description("Logical: SHL Shifts register value to left")]
        SHL = 20,
        [Description("Logical: SHR Shifts register value to right")]
        SHR = 21,
        #endregion
        #region Arithmetic
        [Description("Arithmetic: ADD Adds values in registers")]
        ADD = 22,
        [Description("Arithmetic: SUB Subtracts values in registers")]
        SUB = 23,
        [Description("Arithmetic: SUBU Subtracts Unsigned values in registers")]
        SUBU = 24,
        [Description("Arithmetic: MUL Multiplies values in registers")]
        MUL = 25,
        [Description("Arithmetic: DIV Divides values in registers")]
        DIV = 26,
        [Description("Arithmetic: INC Increments values in registers")]
        INC = 27,
        [Description("Arithmetic: DEC Decrements values in registers")]
        DEC = 28
        #endregion
    }
}
