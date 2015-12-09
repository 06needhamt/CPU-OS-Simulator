using System.CodeDom.Compiler;
using System.Collections.Generic;
using CPU_OS_Simulator.CPU;

namespace CPU_OS_Simulator.Compiler
{
    public class Compiler
    {
        private EnumCompilerMode mode = EnumCompilerMode.UNKNOWN;
        private SourceFile file = null;
        private List<Instruction> instructions = null;

        public Compiler(SourceFile file)
        {
            this.file = file;
            this.mode = EnumCompilerMode.SOURCE_CODE;
        }

        public Compiler(List<Instruction> instructions)
        {
            this.instructions = instructions;
            this.mode = EnumCompilerMode.INSTRUCTIONS;
        }

        public List<List<InstructionSegment>> CompileFromInstructions()
        {
            List<List<InstructionSegment>> segments = new List<List<InstructionSegment>>();
            foreach (Instruction ins in instructions)
            {
                List<InstructionSegment> segList = new List<InstructionSegment>();
                segList.Add(new InstructionSegment(ins.Opcode,(int) EnumSegmentSizes.OPCODE));
                segList.Add(new InstructionSegment(ins.Operand1,(int) EnumSegmentSizes.OPERAND));
                segList.Add(new InstructionSegment(ins.Operand2, (int) EnumSegmentSizes.OPERAND));
                segments.Add(segList);
            }
            return segments;
        }
    }
}