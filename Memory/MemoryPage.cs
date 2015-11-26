using System.Security.Cryptography.X509Certificates;

namespace CPU_OS_Simulator.Memory
{
    public class MemoryPage : ISwappable
    {
        private int pageIndex;
        private readonly int startOffset;
        private readonly int pageSize;
        private readonly int endOffset;
        private MemorySegment[] data;

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

        public int PageSize
        {
            get
            {
                return pageSize;
            }
        }

        public int EndOffset
        {
            get
            {
                return endOffset;
            }
        }

        public MemorySegment[] Data
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

        #region Constructors

        public MemoryPage(int pageIndex, int startOffset, int pageSize)
        {
            this.pageIndex = pageIndex;
            this.startOffset = startOffset;
            this.pageSize = pageSize;
            endOffset = startOffset + pageSize;
            data = new MemorySegment[pageSize / 8];
            PopulateData();
        }

        private void PopulateData()
        {
            for (int i = 0; i < data.Length; i++)
            {
                data[i] = new MemorySegment(startOffset + (i*8));
                data[i].LogicalAddress = i*8;
            }
        }

        #endregion Constructors

        public void SwapOut(int LocationToSwap, int FrameNumber)
        {
            //TODO Implement ME!
        }

        public void SwapIn(int LocationToSwap, int FrameNumber)
        {
            //TODO Implement ME!
        }
    }
}