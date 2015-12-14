using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using CPU_OS_Simulator.CPU;

namespace CPU_OS_Simulator.Compiler
{
    /// <summary>
    /// This class represents the front end of the compiler which is responsible
    /// for compiling programs
    /// </summary>
    public class CompilerMain
    {
        private EnumCompilerMode mode = EnumCompilerMode.UNKNOWN;
        private SourceFile file = null;
        private List<Instruction> instructions = null;
        private string name = String.Empty;

        /// <summary>
        /// Constructor for Compiler front end when compiling source files
        /// </summary>
        /// <param name="file"> the source file to compile</param>
        public CompilerMain(SourceFile file)
        {
            this.file = file;
            this.mode = EnumCompilerMode.SOURCE_CODE;
        }
        /// <summary>
        /// Constructor for compiler front end when compiling instructions
        /// </summary>
        /// <param name="instructions"> the instructions to compile</param>
        /// <param name="name"> the name of the program to compile</param>
        public CompilerMain(List<Instruction> instructions, string name )
        {
            this.instructions = instructions;
            this.mode = EnumCompilerMode.INSTRUCTIONS;
            this.name = name;
        }
        /// <summary>
        /// This function compiles a program from instructions 
        /// </summary>
        /// <returns> a list of lists of instruction segments that represent the original instructions</returns>
        public List<List<InstructionSegment>> CompileFromInstructions()
        {
            List<List<InstructionSegment>> segments = new List<List<InstructionSegment>>();
            foreach (Instruction ins in instructions)
            {
                List<InstructionSegment> segList = new List<InstructionSegment>();
                segList.Add(new InstructionSegment((byte)ins.Opcode,(int) EnumInstructionSegmentSizes.OPCODE,EnumSegmentType.VALUE));
                if (ins.Operand1 == null && ins.Operand1 == null) // if the instruction takes no operands
                {
                    continue;
                }
                else if (ins.Operand2 == null) // if the instruction takes one operand
                {
                    if (ins.Operand1.IsRegister) // if the first operand is a register
                    {
                        segList.Add(new InstructionSegment(ins.Operand1.Value, (int) EnumInstructionSegmentSizes.OPERAND,
                            EnumSegmentType.REGISTER, ins.Operand1.Register.Name));
                    }
                    else // if the first operand is a value
                    {
                        segList.Add(new InstructionSegment(ins.Operand1.Value, (int) EnumInstructionSegmentSizes.OPERAND,
                            EnumSegmentType.VALUE));
                    }
                }
                else
                {
                    if (ins.Operand1.IsRegister) // if the first operand is a register
                    {
                        segList.Add(new InstructionSegment(ins.Operand1.Value, (int) EnumInstructionSegmentSizes.OPERAND,
                            EnumSegmentType.REGISTER,ins.Operand1.Register.Name));
                    }
                    else // if the second operand is a value
                    {
                        segList.Add(new InstructionSegment(ins.Operand1.Value, (int) EnumInstructionSegmentSizes.OPERAND,
                            EnumSegmentType.VALUE));
                    }
                    if (ins.Operand2.IsRegister) // if the second operand is a register
                    {
                        segList.Add(new InstructionSegment(ins.Operand2.Value, (int) EnumInstructionSegmentSizes.OPERAND,
                            EnumSegmentType.REGISTER, ins.Operand1.Register.Name));
                    }
                    else // if the first operand is a value
                    {
                        segList.Add(new InstructionSegment(ins.Operand2.Value, (int) EnumInstructionSegmentSizes.OPERAND,
                            EnumSegmentType.VALUE));
                    }
                }
                segments.Add(segList);
            }
            return segments;
        }
        /// <summary>
        /// This function compiles a list of lists of instruction segments to their byte values
        /// </summary>
        /// <param name="segmentLists">a list of lists of instruction segments to be compiled</param>
        /// <returns> a list of bytes that represent the instruction segments</returns>
        public List<byte> CompileToBytes(List<List<InstructionSegment>> segmentLists)
        {
            List<byte> bytes = new List<byte>();
            foreach (List<InstructionSegment> segments in segmentLists)
            {
                foreach (InstructionSegment segment in segments)
                {
                    List<byte> temp = segment.toBytes();
                    bytes.AddRange(temp);
                }
            }
            return bytes;
        } 
    }
}