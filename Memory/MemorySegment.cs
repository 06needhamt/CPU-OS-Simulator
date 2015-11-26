using System;
using System.Text;

namespace CPU_OS_Simulator.Memory
{
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
                dataString = BuildDataString();
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
                dataString = BuildDataString();
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
                dataString = BuildDataString();
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
                dataString = BuildDataString();
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
                dataString = BuildDataString();
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
                dataString = BuildDataString();
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
                dataString = BuildDataString();
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
                dataString = BuildDataString();
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

        public override string ToString()
        {
            return BuildDataString();
        }

        public string BuildDataString()
        {
            StringBuilder sb = new StringBuilder(8);
            sb.Append((char)byte0);
            sb.Append((char)byte1);
            sb.Append((char)byte2);
            sb.Append((char)byte3);
            sb.Append((char)byte4);
            sb.Append((char)byte5);
            sb.Append((char)byte6);
            sb.Append((char)byte7);
            return sb.ToString();
        }

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