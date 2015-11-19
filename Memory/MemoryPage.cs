using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CPU_OS_Simulator.Memory
{
    class MemoryPage
    {
        private int pageIndex;
        private readonly int startOffset;
        private readonly int pageSize;
        private readonly int endOffset;
        private MemorySegment[] data;
        
        #region Constructors

        public MemoryPage(int pageIndex, int startOffset, int pageSize)
        {
            this.pageIndex = pageIndex;
            this.startOffset = startOffset;
            this.pageSize = pageSize;
            this.endOffset = startOffset + pageSize;
            data = new MemorySegment[pageSize / 8];
            PopulateData();
        }

        private void PopulateData()
        {
            for(int i = 0; i < data.Length; i++)
            {
                data[i] = new MemorySegment();
            }
        }

        #endregion Constructors

        #region Methods



        #endregion Methods
    }
}
