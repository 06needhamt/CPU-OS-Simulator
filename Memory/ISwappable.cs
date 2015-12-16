namespace CPU_OS_Simulator.Memory
{
    interface ISwappable
    {
        void SwapOut(int LocationToSwap,int FrameNumber);
        void SwapIn(int LocationToSwap, int FrameNumber);
    }
}
