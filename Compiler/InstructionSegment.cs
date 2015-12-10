using System;
using System.Collections.Generic;
using System.Linq;

namespace CPU_OS_Simulator.Compiler
{
    public class InstructionSegment
    {
        private int value;
        private int size;
        private string name = String.Empty;
        private EnumSegmentType type = EnumSegmentType.UNKNOWN;

        public InstructionSegment(int value, int size,EnumSegmentType type)
        {
            this.value = value;
            this.size = size;
            this.type = type;
        }

        public InstructionSegment(int value, int size, EnumSegmentType type, string name) : this(value,size,type)
        {
            this.name = name;
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
            if (type == EnumSegmentType.REGISTER && size == 5)
            {
                bytes.Add(0xFF);
                value = int.Parse(name.Substring(1, 2));
                bytes.AddRange(BitConverter.GetBytes((int)value).ToList());
            }
            else if (type == EnumSegmentType.VALUE && size == 5)
            {
                 bytes.Add(0xEE);
                bytes.AddRange(BitConverter.GetBytes((int)value).ToList());
            }
            else if (type == EnumSegmentType.VALUE && size == 1)
            {
                bytes.Add((byte)value);
            }
            else
            {
                throw new Exception();
            }
            return bytes;
        }
    }
}