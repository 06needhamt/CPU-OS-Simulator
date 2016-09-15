using System;
using System.Collections.Generic;
using System.Linq;

namespace CPU_OS_Simulator.Compiler.Old.Backend
{
    /// <summary>
    /// this class represents a segment of an instruction 
    /// i.e the opcode or an operand
    /// </summary>
    public class InstructionSegment
    {
        private int value;
        private int size;
        private string name = String.Empty;
        private EnumSegmentType type = EnumSegmentType.UNKNOWN;

        /// <summary>
        /// Constructor for an instruction segment that is a literal value
        /// </summary>
        /// <param name="value"> the value</param>
        /// <param name="size"> the size of the segment</param>
        /// <param name="type"> the type of the segment</param>
        public InstructionSegment(int value, int size,EnumSegmentType type)
        {
            this.value = value;
            this.size = size;
            this.type = type;
        }
        /// <summary>
        /// Constructor for an instruction segment that is a register
        /// </summary>
        /// <param name="value"> the value</param>
        /// <param name="size"> the size of the segment</param>
        /// <param name="type"> the type of the segment</param>
        /// <param name="name"> the name of the segment</param>
        public InstructionSegment(int value, int size, EnumSegmentType type, string name) : this(value,size,type)
        {
            this.name = name;
        }

        /// <summary>
        /// Property to store the value of this segment
        /// </summary>
        public int Value
        {
            get { return value; }
            set { this.value = value; }
        }

        /// <summary>
        /// Property to store the size of the segment
        /// </summary>
        public int Size
        {
            get { return size; }
            set { size = value; }
        }

        /// <summary>
        /// This function converts an instruction segment to bytes 
        /// </summary>
        /// <returns> the bytes that represent the instruction segment</returns>
        /// <exception cref="Exception">Thrown if the segment matches no known pattern</exception>
        public List<byte> toBytes()
        {
            List<byte> bytes = new List<byte>();
            if (type == EnumSegmentType.REGISTER && size == 5) // if the segment is a register
            {
                bytes.Add(0xFF); // write 255 to the memory to indicate this operand is a register
                value = int.Parse(name.Substring(1, 2));
                List<byte> temp = BitConverter.GetBytes(value).ToList();
                bytes.AddRange(temp);
            }
            else if (type == EnumSegmentType.VALUE && size == 5) // if the segment is a value
            {
                bytes.Add(0xEE); // write 238 to the memory to indicate this operand is a literal value
                List<byte> temp = BitConverter.GetBytes(value).ToList();
                bytes.AddRange(temp);
            }
            else if (type == EnumSegmentType.VALUE && size == 1) // if the segment is a opcode
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