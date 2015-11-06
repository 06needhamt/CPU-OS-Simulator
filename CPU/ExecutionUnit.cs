using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace CPU_OS_Simulator.CPU
{
    public class ExecutionUnit
    {
        private SimulatorProgram program;
        private int clockSpeed;
        private int currentIndex;

        public ExecutionUnit(SimulatorProgram program, int clockSpeed)
        {
            this.program = program;
            this.clockSpeed = clockSpeed;
            this.currentIndex = 0;
        }

        public ExecutionUnit(SimulatorProgram program, int clockSpeed, int currentIndex) : this(program, clockSpeed)
        {
            this.currentIndex = currentIndex;
        }

        public IEnumerable<int> ExecuteProgram(bool step)
        {
            Console.WriteLine("Executing");
            for (int i = currentIndex; i < program.Instructions.Count; i++)
            {
                program.Instructions.ElementAt(i).Execute();
                Thread.Sleep(clockSpeed);
                yield return i;
            }
        }

        public SimulatorProgram Program
        {
            get
            {
                return program;
            }

            set
            {
                program = value;
            }
        }

        public int ClockSpeed
        {
            get
            {
                return clockSpeed;
            }

            set
            {
                clockSpeed = value;
            }
        }
    }
}