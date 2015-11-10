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
        private bool stop;
        private bool done;
        

        public ExecutionUnit(SimulatorProgram program, int clockSpeed)
        {
            this.program = program;
            this.clockSpeed = clockSpeed;
            this.currentIndex = 0;
            stop = false;
        }

        public ExecutionUnit(SimulatorProgram program, int clockSpeed, int currentIndex) : this(program, clockSpeed)
        {
            if(currentIndex < 0)
            {
                currentIndex = 0;
            }
            this.currentIndex = currentIndex;
            stop = false;
        }

        public void ExecuteInstruction(bool step)
        {
            Console.WriteLine("Executing instruction");
            program.Instructions.ElementAt(currentIndex).Execute();
            currentIndex++;
            if(currentIndex == program.Instructions.Count)
            {
                Done = true;
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

        public bool Stop
        {
            get
            {
                return stop;
            }

            set
            {
                stop = value;
            }
        }

        public bool Done
        {
            get
            {
                return done;
            }

            set
            {
                done = value;
            }
        }
    }
}