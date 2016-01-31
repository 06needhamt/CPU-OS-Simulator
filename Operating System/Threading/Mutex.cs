using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace CPU_OS_Simulator.Operating_System.Threading
{
    [Serializable]
    public class Mutex : IDisposable
    {
        private SpinLock spinLock;
        private bool autoRelease;

        public Mutex()
        {
            
        }

        public Mutex(SpinLock spinLock, bool autoRelease)
        {
            this.spinLock = spinLock;
            this.autoRelease = autoRelease;
        }

        public void Release()
        {
            if (spinLock != null && spinLock.locked)
            {
                spinLock.Unlock();
            }
        }
        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            if (autoRelease)
            {
                Release();
                spinLock.Dispose();
            }
            else
            {
                MessageBox.Show("Deadlock Possible because mutex will never be released");
            }
        }
    }
}
