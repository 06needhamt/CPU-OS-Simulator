using System;

namespace CPU_OS_Simulator.Memory
{
    public class MemoryPage_OLD
    {
        #region Global Variables

        private char[,] data;
        private int pageIndex;
        private readonly int startOffset;
        private readonly int endOffset;
        private readonly int pageSize;

        #endregion Global Variables

        #region Properties

        public char[,] Data
        {
            get
            {
                return data;
            }

            set
            {
                data = value;
            }
        }

        public int PageIndex
        {
            get
            {
                return pageIndex;
            }

            set
            {
                pageIndex = value;
            }
        }

        public int StartOffset
        {
            get
            {
                return startOffset;
            }
        }

        public int EndOffset
        {
            get
            {
                return endOffset;
            }
        }

        public int PageSize
        {
            get
            {
                return pageSize;
            }
        }

        #endregion Properties

        #region Constructors

        public MemoryPage_OLD(int pageIndex, int startOffset, int pageSize)
        {
            this.pageIndex = pageIndex;
            this.startOffset = startOffset;
            this.pageSize = pageSize;
            endOffset = startOffset + pageSize;
            data = new char[pageSize / 8, 8];
            PopulateArray<Char>(ref data, (char)0);
        }

        #endregion Constructors

        #region Methods

        public char getByte(int address)
        {
            int row = address / 8;
            int charnumber = address % 8;
            return data[row, charnumber];
        }

        public void setByte(int address, char value)
        {
            int row = address / 8;
            int charNumber = address % 8;
            data[row, charNumber] = value;
        }

        public void PopulateArray<T>(ref T[,] data, T Value)
        {
            for (int i = 0; i < data.GetLength(0); i++)
            {
                for (int j = 0; j < data.GetLength(1); j++)
                {
                    data[i, j] = default(T);
                }
            }
        }

        #endregion Methods
    }
}