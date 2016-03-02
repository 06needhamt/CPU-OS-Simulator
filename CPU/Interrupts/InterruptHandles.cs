using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CPU_OS_Simulator.CPU.Interrupts
{
    public static class InterruptHandles
    {
        public static VectoredInterrupt VINT1;
        public static VectoredInterrupt VINT2;
        public static VectoredInterrupt VINT3;
        public static VectoredInterrupt VINT4;
        public static VectoredInterrupt VINT5;
        public static VectoredInterrupt VINT6;

        public static PolledInterrupt PINT1;
        public static PolledInterrupt PINT2;
        public static PolledInterrupt PINT3;
        public static PolledInterrupt PINT4;
        public static PolledInterrupt PINT5;
        public static PolledInterrupt PINT6;

        public static PolledInterrupt GetPolledInterrupt(int number)
        {
            switch (number)
            {
                case 1:
                    return PINT1;
                case 2:
                    return PINT2;
                case 3:
                    return PINT3;
                case 4:
                    return PINT4;
                case 5:
                    return PINT5;
                case 6:
                    return PINT6;
                default:
                    throw new InvalidOperationException("Invalid Polled Interrupt Number: " + number);
            }
        }

        public static VectoredInterrupt GetVectoredInterrupt(int number)
        {
            switch (number)
            {
                case 1:
                    return VINT1;
                case 2:
                    return VINT2;
                case 3:
                    return VINT3;
                case 4:
                    return VINT4;
                case 5:
                    return VINT5;
                case 6:
                    return VINT6;
                default:
                    throw new InvalidOperationException("Invalid Vectored Interrupt Number: " + number);
            }
        }


        public static void SetVectoredInterrupt(int number,int logicalAddress = int.MinValue, Func<int> handlerFunc = null )
        {
            if(logicalAddress < 0 && handlerFunc == null)
                throw new InvalidOperationException("Cannot construct a Vectored Interrupt Without a logical Address or handler function");

            switch (number)
            {
                case 1:
                    if(handlerFunc != null)
                        VINT1 = new VectoredInterrupt(new InterruptHandler(handlerFunc));
                    else
                        VINT1 = new VectoredInterrupt(logicalAddress);
                    break;
                case 2:
                    if (handlerFunc != null)
                        VINT2 = new VectoredInterrupt(new InterruptHandler(handlerFunc));
                    else
                        VINT2 = new VectoredInterrupt(logicalAddress);
                    break;
                case 3:
                    if (handlerFunc != null)
                        VINT3 = new VectoredInterrupt(new InterruptHandler(handlerFunc));
                    else
                        VINT3 = new VectoredInterrupt(logicalAddress);
                    break;
                case 4:
                    if (handlerFunc != null)
                        VINT4 = new VectoredInterrupt(new InterruptHandler(handlerFunc));
                    else
                        VINT4 = new VectoredInterrupt(logicalAddress); 
                    break;
                case 5:
                    if (handlerFunc != null)
                        VINT5 = new VectoredInterrupt(new InterruptHandler(handlerFunc));
                    else
                        VINT5 = new VectoredInterrupt(logicalAddress);
                    break;
                case 6:
                    if (handlerFunc != null)
                        VINT6 = new VectoredInterrupt(new InterruptHandler(handlerFunc));
                    else
                        VINT6 = new VectoredInterrupt(logicalAddress);
                    break;
                default:
                    throw new InvalidOperationException("Invalid Vectored Interrupt Number: " + number);
            }
        }

        public static void SetPolledInterrupt(int number, int logicalAddress = int.MinValue, Func<int> handlerFunc = null)
        {
            if (logicalAddress < 0 && handlerFunc == null)
                throw new InvalidOperationException("Cannot construct a Polled Interrupt Without a logical Address or handler function");

            switch (number)
            {
                case 1:
                    if (handlerFunc != null)
                        PINT1 = new PolledInterrupt(new InterruptHandler(handlerFunc));
                    else
                        PINT1 = new PolledInterrupt(logicalAddress);
                    break;
                case 2:
                    if (handlerFunc != null)
                        PINT2 = new PolledInterrupt(new InterruptHandler(handlerFunc));
                    else
                        PINT2 = new PolledInterrupt(logicalAddress);
                    break;
                case 3:
                    if (handlerFunc != null)
                        PINT3 = new PolledInterrupt(new InterruptHandler(handlerFunc));
                    else
                        PINT3 = new PolledInterrupt(logicalAddress);
                    break;
                case 4:
                    if (handlerFunc != null)
                        PINT4 = new PolledInterrupt(new InterruptHandler(handlerFunc));
                    else
                        PINT4 = new PolledInterrupt(logicalAddress);
                    break;
                case 5:
                    if (handlerFunc != null)
                        PINT5 = new PolledInterrupt(new InterruptHandler(handlerFunc));
                    else
                        PINT5 = new PolledInterrupt(logicalAddress);
                    break;
                case 6:
                    if (handlerFunc != null)
                        PINT6 = new PolledInterrupt(new InterruptHandler(handlerFunc));
                    else
                        PINT6 = new PolledInterrupt(logicalAddress);
                    break;
                default:
                    throw new InvalidOperationException("Invalid Polled Interrupt Number: " + number);
            }
        }
    }
}
