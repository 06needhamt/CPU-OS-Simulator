using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CPU_OS_Simulator.Memory
{
    class MemorySegment
    {
        #region Global Variables
        private Int32 logicalAddress;
        private Int32 physicalAddress;
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

        public byte Byte0
        {
            get
            {
                return byte0;
            }

            set
            {
                byte0 = value;
            }
        }

        public byte Byte1
        {
            get
            {
                return byte1;
            }

            set
            {
                byte1 = value;
            }
        }

        public byte Byte2
        {
            get
            {
                return byte2;
            }

            set
            {
                byte2 = value;
            }
        }

        public byte Byte3
        {
            get
            {
                return byte3;
            }

            set
            {
                byte3 = value;
            }
        }

        public byte Byte4
        {
            get
            {
                return byte4;
            }

            set
            {
                byte4 = value;
            }
        }

        public byte Byte5
        {
            get
            {
                return byte5;
            }

            set
            {
                byte5 = value;
            }
        }

        public byte Byte6
        {
            get
            {
                return byte6;
            }

            set
            {
                byte6 = value;
            }
        }

        public byte Byte7
        {
            get
            {
                return byte7;
            }

            set
            {
                byte7 = value;
            }
        }

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
        public MemorySegment(int physicalAddress) : this()
        {
            this.physicalAddress = physicalAddress;
        }
        public byte GetByte(int number)
        {
            switch (number)
            {
                case 0:
                    {
                        return Byte0;
                    }
                case 1:
                    {
                        return Byte1;
                    }
                case 2:
                    {
                        return Byte2;
                    }
                case 3:
                    {
                        return Byte3;
                    }
                case 4:
                    {
                        return byte4;
                    }
                case 5:
                    {
                        return Byte5;
                    }
                case 6:
                    {
                        return Byte6;
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
        public override string ToString()
        {
            return BuildDataString();
        }

        public string BuildDataString()
        {
            StringBuilder sb = new StringBuilder(8);
            sb.Append(byte0);
            sb.Append(byte1);
            sb.Append(byte2);
            sb.Append(byte3);
            sb.Append(byte4);
            sb.Append(byte5);
            sb.Append(byte6);
            sb.Append(Byte7);
            return sb.ToString();
        }
        #endregion Methods
    }
}
