using System.ComponentModel;

namespace CPU_OS_Simulator.CPU
{
    /// <summary>
    /// This enum holds the opcodes for each instruction type
    /// </summary>
    public enum EnumOpcodes
    {
        #pragma warning disable 1591
        
        #region Data Transfer

        [Description("Data Transfer: MOV Moves data to register")]
        [NumberOfOperands(2)]
        MOV = 0,

        [Description("Data Transfer: MVS Moves string to memory")]
        [NumberOfOperands(2)]
        MVS = 1,

        [Description("Data Transfer: CVS Not currently used")]
        [NumberOfOperands(0)]
        CVS = 2,

        [Description("Data Transfer: CVI Not currently used")]
        [NumberOfOperands(0)]
        CVI = 3,

        [Description("Data Transfer: LDB Loads byte from memory into register")]
        [NumberOfOperands(2)]
        LDB = 4,

        [Description("Data Transfer: LDW Loads word from memory into register")]
        [NumberOfOperands(2)]
        LDW = 5,

        [Description("Data Transfer: LDDW Loads double word from memory into register")]
        [NumberOfOperands(2)]
        LDDW = 53,

        [Description("Data Transfer: LNS  Loads word from memory into register - No SR update")]
        [NumberOfOperands(2)]
        LNS = 6,

        [Description("Data Transfer: LDBI Loads byte from memory into register, increments indirect source address")]
        [NumberOfOperands(2)]
        LDBI = 7,

        [Description("Data Transfer: LDWI  Loads word from memory into register, increments indirect source address")]
        [NumberOfOperands(2)]
        LDWI = 8,

        [Description("Data Transfer: LDDWI Loads double word from memory into register increments indirect source address")]
        [NumberOfOperands(2)]
        LDDWI = 54,

        [Description("Data Transfer: TAS Test and set")]
        [NumberOfOperands(1)]
        TAS = 9,

        [Description("Data Transfer: STB Stores byte to memory")]
        [NumberOfOperands(2)]
        STB = 10,

        [Description("Data Transfer: STW Stores word to memory")]
        [NumberOfOperands(2)]
        STW = 11,

        [Description("Data Transfer: STDW Stores double word to memory")]
        [NumberOfOperands(2)]
        STDW = 55,

        [Description("Data Transfer: STBI Stores byte to memory, increments indirect destination address")]
        [NumberOfOperands(2)]
        STBI = 12,

        [Description("Data Transfer: STWI Stores word to memory, increments indirect destination address")]
        [NumberOfOperands(2)]
        STWI = 13,

        [Description("Data Transfer: STDWI Stores double word to memory, increments indirect destination address")]
        [NumberOfOperands(2)]
        STDWI = 56,

        [Description("Data Transfer: PUSH Pushes value or value in register onto top of stack")]
        [NumberOfOperands(1)]
        PUSH = 14,

        [Description("Data Transfer: POP Pops value from top of stack into register")]
        [NumberOfOperands(1)]
        POP = 15,

        [Description("Data Transfer: SWP Swaps two register values")]
        [NumberOfOperands(2)]
        SWP = 16,

        #endregion Data Transfer

        #region Logical

        [Description("Logical: AND Logical AND of registers")]
        [NumberOfOperands(2)]
        AND = 17,

        [Description("Logical: OR Logical OR of registers")]
        [NumberOfOperands(2)]
        OR = 18,

        [Description("Logical: NOT Logical NOT of registers")]
        [NumberOfOperands(2)]
        NOT = 19,

        [Description("Logical: SHL Shifts register value to left")]
        [NumberOfOperands(2)]
        SHL = 20,

        [Description("Logical: SHR Shifts register value to right")]
        [NumberOfOperands(2)]
        SHR = 21,

        #endregion Logical

        #region Arithmetic

        [Description("Arithmetic: ADD Adds values in registers")]
        [NumberOfOperands(2)]
        ADD = 22,

        [Description("Arithmetic: SUB Subtracts values in registers")]
        [NumberOfOperands(2)]
        SUB = 23,

        [Description("Arithmetic: SUBU Subtracts Unsigned values in registers")]
        [NumberOfOperands(2)]
        SUBU = 24,

        [Description("Arithmetic: MUL Multiplies values in registers")]
        [NumberOfOperands(2)]
        MUL = 25,

        [Description("Arithmetic: DIV Divides values in registers")]
        [NumberOfOperands(2)]
        DIV = 26,

        [Description("Arithmetic: INC Increments values in registers")]
        [NumberOfOperands(1)]
        INC = 27,

        [Description("Arithmetic: DEC Decrements values in registers")]
        [NumberOfOperands(2)]
        DEC = 28,

        #endregion Arithmetic

        #region Control Transfer

        [Description("Control Transfer: JMP Jumps unconditionally")]
        [NumberOfOperands(1)]
        JMP = 29,

        [Description("Control Transfer: JEQ Jumps if equal")]
        [NumberOfOperands(1)]
        JEQ = 30,

        [Description("Control Transfer: JNE Jumps if not equal")]
        [NumberOfOperands(1)]
        JNE = 31,

        [Description("Control Transfer: JGT Jumps if greater than")]
        [NumberOfOperands(1)]
        JGT = 32,

        [Description("Control Transfer: JGE Jumps if greater than or equal")]
        [NumberOfOperands(1)]
        JGE = 33,

        [Description("Control Transfer: JLT Jumps if less than")]
        [NumberOfOperands(1)]
        JLT = 34,

        [Description("Control Transfer: JLE Jumps if less than or equal")]
        [NumberOfOperands(1)]
        JLE = 35,

        [Description("Control Transfer: JNZ Jumps if not zero")]
        [NumberOfOperands(1)]
        JNZ = 36,

        [Description("Control Transfer: JZR Jumps if Z status flag is set")]
        [NumberOfOperands(1)]
        JZR = 37,

        [Description("Control Transfer: CALL Calls subroutine")]
        [NumberOfOperands(1)]
        CALL = 38,

        [Description("Control Transfer: LOOP Loops if register value is greater than 0")]
        [NumberOfOperands(1)]
        LOOP = 39,

        [Description("Control Transfer: JSEL Not currently used")]
        [NumberOfOperands(0)]
        JSEL = 40,

        [Description("Control Transfer: TABE Not currently used")]
        [NumberOfOperands(0)]
        TABE = 41,

        [Description("Control Transfer: TABI Not currently used")]
        [NumberOfOperands(0)]
        TABI = 42,

        [Description("Control Transfer: MSF Mark stack frame")]
        [NumberOfOperands(0)]
        MSF = 43,

        [Description("Control Transfer: RET Returns from subroutine")]
        [NumberOfOperands(0)]
        RET = 44,

        [Description("Control Transfer: IRET Returns from interrupt routine")]
        [NumberOfOperands(0)]
        IRET = 45,

        [Description("Control Transfer: SWI Software interrupt - used in OS system calls")]
        [NumberOfOperands(1)]
        SWI = 46,

        [Description("Control Transfer: HLT Halts the simulator")]
        [NumberOfOperands(0)]
        HLT = 47,

        #endregion Control Transfer

        #region Comparison

        [Description("Comparison: CMP Compares two numeric values")]
        [NumberOfOperands(2)]
        CMP = 48,

        [Description("Comparison: CPS Compares two string values")]
        [NumberOfOperands(2)]
        CPS = 49,

        #endregion Comparison

        #region I/O

        [Description("I/O: IN Gets input into register or memory")]
        [NumberOfOperands(2)]
        IN = 50,

        [Description("I/O: OUT Puts output from register or memory")]
        [NumberOfOperands(2)]
        OUT = 51,

        #endregion I/O

        #region Miscellaneous

        [Description("Miscellaneous: NOP No operation (or null operation)")]
        [NumberOfOperands(0)]
        NOP = 52

        #endregion Miscellaneous
    }
}