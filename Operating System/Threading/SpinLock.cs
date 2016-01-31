using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace CPU_OS_Simulator.Operating_System.Threading
{
    [Serializable]
    public class SpinLock : IDisposable
    {
        public volatile bool locked;
        private SimulatorProcess process;
        private SimulatorThread thread;
        public volatile int timeout;

        public SpinLock()
        {
            
        }

        public SpinLock(SimulatorProcess process, int timeout = int.MaxValue)
        {
            this.locked = false;
            this.process = process;
            this.thread = null;
            this.timeout = timeout;
        }

        public SpinLock(SimulatorThread thread, int timeout = int.MaxValue)
        {
            this.locked = false;
            this.process = null;
            this.thread = thread;
            this.timeout = timeout;
        }

        public SimulatorProcess Process
        {
            get { return process; }
            set { process = value; }
        }

        public SimulatorThread Thread
        {
            get { return thread; }
            set { thread = value; }
        }

        public bool Unlock()
        {
            if (locked)
                locked = false;
            else
                MessageBox.Show("Can not unlock an already unlocked mutex");
            return locked;
        }

        public bool Lock()
        {
            if (!locked)
                locked = true;
            else
                MessageBox.Show("Can not lock an already locked mutex");
            return locked;
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            if (this.locked)
                this.Unlock();

        }
    }
}
