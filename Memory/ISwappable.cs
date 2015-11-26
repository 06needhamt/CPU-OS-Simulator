using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CPU_OS_Simulator.Memory
{
    interface ISwappable
    {
        void SwapOut(int LocationToSwap,int FrameNumber);
        void SwapIn(int LocationToSwap, int FrameNumber);
    }
}
