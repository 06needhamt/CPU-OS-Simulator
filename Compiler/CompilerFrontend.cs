using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using CPU_OS_Simulator.CPU;

namespace CPU_OS_Simulator.Compiler
{
    public class CompilerFrontend
    {
        private EnumCompilerMode mode = EnumCompilerMode.UNKNOWN;
        private SourceFile file = null;
        private List<Instruction> instructions = null;
        private string name = String.Empty;

        public CompilerFrontend(SourceFile file)
        {
            this.file = file;
            this.mode = EnumCompilerMode.SOURCE_CODE;
        }

        public CompilerFrontend(List<Instruction> instructions, string name )
        {
            this.instructions = instructions;
            this.mode = EnumCompilerMode.INSTRUCTIONS;
            this.name = name;
        }

        public List<List<InstructionSegment>> CompileFromInstructions()
        {
            List<List<InstructionSegment>> segments = new List<List<InstructionSegment>>();
            foreach (Instruction ins in instructions)
            {
                List<InstructionSegment> segList = new List<InstructionSegment>();
                segList.Add(new InstructionSegment((byte)ins.Opcode,(int) EnumSegmentSizes.OPCODE));
                segList.Add(new InstructionSegment(ins.Operand1.Value,(int) EnumSegmentSizes.OPERAND));
                segList.Add(new InstructionSegment(ins.Operand2.Value, (int) EnumSegmentSizes.OPERAND));
                segments.Add(segList);
            }
            return segments;
        }

        public List<byte> CompileToBytes(List<List<InstructionSegment>> segmentLists)
        {
            List<byte> bytes = new List<byte>();
            foreach (List<InstructionSegment> segments in segmentLists)
            {
                foreach (InstructionSegment segment in segments)
                {
                    bytes.AddRange(segment.toBytes());
                }
            }
            return bytes;
        } 
    }
}