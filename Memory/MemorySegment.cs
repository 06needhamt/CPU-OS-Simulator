using System;
using System.Text;

namespace CPU_OS_Simulator.Memory
{
    /// <summary>
    /// This class represents a 8 byte segment of a memory page
    /// </summary>
    public class MemorySegment
    {
        #region Global Variables

        private int logicalAddress;
        private int physicalAddress;
        private byte byte0;
        private byte byte1;
        private byte byte2;
        private byte byte3;
        private byte byte4;
        private byte byte5;
        private byte byte6;
        private byte byte7;
        private IntPtr[] bytePointers;
        private string dataString;

        #endregion Global Variables

        #region Properties
        /// <summary>
        /// The logical address of the first byte in the segment
        /// </summary>
        public int LogicalAddress
        {
            get
            {
                return logicalAddress;
            }

            set
            {
                logicalAddress = value;
            }
        }
        /// <summary>
        /// The physical address of the first byte in the segment
        /// </summary>
        public int PhysicalAddress
        {
            get
            {
                return physicalAddress;
            }

            set
            {
                physicalAddress = value;
            }
        }
        /// <summary>
        /// The first byte in the segment
        /// </summary>
        public byte Byte0
        {
            get
            {
                return byte0;
            }

            set
            {
                byte0 = value;
                dataString = BuildDataString();
            }
        }
        /// <summary>
        /// The second byte in the segment
        /// </summary>
        public byte Byte1
        {
            get
            {
                return byte1;
            }

            set
            {
                byte1 = value;
                dataString = BuildDataString();
            }
        }
        /// <summary>
        /// The third byte in the segment
        /// </summary>
        public byte Byte2
        {
            get
            {
                return byte2;
            }

            set
            {
                byte2 = value;
                dataString = BuildDataString();
            }
        }
        /// <summary>
        /// The forth byte in the segment
        /// </summary>
        public byte Byte3
        {
            get
            {
                return byte3;
            }

            set
            {
                byte3 = value;
                dataString = BuildDataString();
            }
        }
        /// <summary>
        /// The fifth byte in the segment
        /// </summary>
        public byte Byte4
        {
            get
            {
                return byte4;
            }

            set
            {
                byte4 = value;
                dataString = BuildDataString();
            }
        }
        /// <summary>
        /// The sixth byte in the segment
        /// </summary>
        public byte Byte5
        {
            get
            {
                return byte5;
            }

            set
            {
                byte5 = value;
                dataString = BuildDataString();
            }
        }
        /// <summary>
        /// The seventh byte in the segment
        /// </summary>
        public byte Byte6
        {
            get
            {
                return byte6;
            }

            set
            {
                byte6 = value;
                dataString = BuildDataString();
            }
        }
        /// <summary>
        /// The eighth byte in the segment
        /// </summary>
        public byte Byte7
        {
            get
            {
                return byte7;
            }

            set
            {
                byte7 = value;
                dataString = BuildDataString();
            }
        }
        /// <summary>
        /// The string that represents the data in this segment
        /// </summary>
        public string DataString
        {
            get
            {
                return dataString;
            }

            set
            {
                dataString = value;
            }
        }

        #endregion Properties

        #region Constructors
        /// <summary>
        /// Default constructor for a memory segment
        /// </summary>
        public MemorySegment()
        {
            logicalAddress = 0;
            physicalAddress = 0;
            byte0 = 0;
            byte1 = 0;
            byte2 = 0;
            byte3 = 0;
            byte4 = 0;
            byte5 = 0;
            byte6 = 0;
            byte7 = 0;
            dataString = BuildDataString();
        }
        /// <summary>
        /// Constructor for a memory segment
        /// </summary>
        /// <param name="physicalAddress"> the physical address of a memory segment</param>
        public MemorySegment(int physicalAddress) : this()
        {
            this.physicalAddress = physicalAddress;
        }

        /// <summary>
        /// Gets a specified byte from this memory segment
        /// </summary>
        /// <param name="number"> the byte number to get</param>
        /// <returns>the value stored within the requested byte</returns>
        /// <exception cref="InvalidOperationException"> thrown if invalid byte number is passed</exception>
        public byte GetByte(int number)
        {
            switch (number)
            {
                case 0:
                    {
                        return byte0;
                    }
                case 1:
                    {
                        return byte1;
                    }
                case 2:
                    {
                        return byte2;
                    }
                case 3:
                    {
                        return byte3;
                    }
                case 4:
                    {
                        return byte4;
                    }
                case 5:
                    {
                        return byte5;
                    }
                case 6:
                    {
                        return byte6;
                    }
                case 7:
                    {
                        return byte7;
                    }
                default:
                    {
                        throw new InvalidOperationException("Invalid byte number");
                    }
            }
        }

        #endregion Constructors

        #region Methods
        /// <summary>
        /// Returns the data string for this memory segment
        /// </summary>
        /// <returns>the data string for this memory segment</returns>
        public override string ToString()
        {
            return BuildDataString();
        }
        /// <summary>
        /// Builds the data string for this memory segment
        /// </summary>
        /// <returns>the data string for this memory segment</returns>
        public string BuildDataString()
        {
            StringBuilder sb = new StringBuilder(8);
            sb.Append((char)byte0);
            sb.Append((char)byte1);
            sb.Append((char)byte2);
            sb.Append((char)byte4);
            sb.Append((char)byte5);
            sb.Append((char)byte6);
            sb.Append((char)byte7);
            return sb.ToString();
        }

        /// <summary>
        /// Sets the requested byte in this memory segment
        /// </summary>
        /// <param name="number"> the byte number to set</param>
        /// <param name="value"> the value to set the byte to </param>
        /// <exception cref="InvalidOperationException"> Thrown if an invalid byte number is passed</exception>
        public void SetByte(int number, byte value)
        {
            switch (number)
            {
                case 0:
                    {
                        Byte0 = value;
                        break;
                    }
                case 1:
                    {
                        Byte1 = value;
                        break;
                    }
                case 2:
                    {
                        Byte2 = value;
                        break;
                    }
                case 3:
                    {
                        Byte3 = value;
                        break;
                    }
                case 4:
                    {
                        Byte4 = value;
                        break;
                    }
                case 5:
                    {
                        Byte5 = value;
                        break;
                    }
                case 6:
                    {
                        Byte6 = value;
                        break;
                    }
                case 7:
                    {
                        Byte7 = value;
                        break;
                    }
                default:
                    {
                        throw new InvalidOperationException("Invalid byte number");
                    }
            }

            #endregion Methods
        }
    }
}