using System;
using System.Collections.Generic;
using System.Linq;

namespace CPU_OS_Simulator.Compiler
{
    public class InstructionSegment
    {
        private int value;
        private int size;

        public InstructionSegment(int value, int size)
        {
            this.value = value;
            this.size = size;
        }

        public int Value
        {
            get { return value; }
            set { this.value = value; }
        }

        public int Size
        {
            get { return size; }
            set { size = value; }
        }

        public List<byte> toBytes()
        {
            List<byte> bytes = new List<byte>();
            if (value < 2556)
            {
                this.size = sizeof(byte);
                bytes.Add((byte)value);
            }
            this.size = sizeof(int);
            bytes.AddRange(BitConverter.GetBytes((int)value).ToList());
            return bytes;
        }
    }
}