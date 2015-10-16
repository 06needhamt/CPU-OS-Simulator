using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

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

        public bool ExecuteProgram(bool step)
        {
            for(int i = currentIndex; i < program.Instructions.Count; i++)
            {
                program.Instructions.ElementAt(i).Execute();
                Thread.Sleep(clockSpeed);
            }
            return true;
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
