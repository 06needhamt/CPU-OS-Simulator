using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace CPU_OS_Simulator.Operating_System.Threading
{
    /// <summary>
    /// This class implements a simple counting semaphore for managing resource allocation
    /// </summary>
    [Serializable]
    public class Semaphore
    {
        private SimulatorResource resource;
        private int slots;
        private List<SimulatorThread> insideThreads;
        private List<SimulatorProcess> insideProcesses; 
        private Queue<SimulatorThread> waitingThreads;
        private Queue<SimulatorProcess> waitingProcesses; 

        /// <summary>
        /// Constructor for semaphore
        /// </summary>
        /// <param name="resource"> the resource this semaphore will be managing</param>
        /// <param name="slots"> the number of allocation slots available</param>
        public Semaphore(ref SimulatorResource resource, int slots)
        {
            this.slots = slots;
            this.resource = resource;
            this.insideThreads = new List<SimulatorThread>(slots);
            this.waitingThreads = new Queue<SimulatorThread>();
            this.insideProcesses = new List<SimulatorProcess>(slots);
            this.waitingProcesses= new Queue<SimulatorProcess>();
            this.resource.Semaphore = this;
            this.resource.HasSemaphore = true;
        }
        /// <summary>
        /// This function releases a thread from the semaphore
        /// </summary>
        /// <param name="thread"> a reference to the thread to release</param>
        /// <returns>the number of free allocation slots available after releasing the thread</returns>
        public int Up(ref SimulatorThread thread)
        {
            if (insideThreads.Contains(thread))
            {
                insideThreads.Remove(thread);
                thread.OwnerProcess.AllocatedResources.Remove(resource);
                thread.WaitingForSemaphore = false;
                thread.OwnsSemaphore = false;
                thread.PreviousState = thread.CurrentState;
                thread.CurrentState = EnumThreadState.UNKNOWN;
                slots++;
                
            }
            else if (waitingThreads.Contains(thread))
            {
                List<SimulatorThread> temp = waitingThreads.ToList();
                temp.Remove(thread);
                waitingThreads = new Queue<SimulatorThread>(temp);
                thread.OwnerProcess.AllocatedResources.Remove(resource);
                thread.WaitingForSemaphore = false;
                thread.OwnsSemaphore = false;
                thread.PreviousState = thread.CurrentState;
                thread.CurrentState = EnumThreadState.UNKNOWN;

            }
            else
            {
                MessageBox.Show("Thread must be captured by the semaphore before being released");
                return int.MinValue;
            }
            CaptureNextThread();
            return slots;
        }

        /// <summary>
        /// This function releases a process from the semaphore
        /// </summary>
        /// <param name="process"> a reference to the process to release</param>
        /// <returns>the number of free allocation slots available after releasing the process</returns>
        public int Up(ref SimulatorProcess process)
        {
            if (insideProcesses.Contains(process))
            {
                insideProcesses.Remove(process);
                process.AllocatedResources.Remove(resource);
                process.WaitingForSemaphore = false;
                process.OwnsSemaphore = false;
                process.PreviousState = process.CurrentState;
                process.CurrentState = EnumProcessState.UNKNOWN;
            }
            else
            {
                MessageBox.Show("Process must be captured by the semaphore before being released");
                return int.MinValue;
            }
            CaptureNextProcess();
            return slots;
        }
        /// <summary>
        /// This function captures a thread inside the semaphore
        /// </summary>
        /// <param name="thread"> a reference to the thread to capture</param>
        /// <returns> the number of free allocation slots available after capturing the thread</returns>
        public int Down(ref SimulatorThread thread)
        {
            if (!insideThreads.Contains(thread) && !waitingThreads.Contains(thread))
            {
                if (slots > 0)
                {
                    thread.OwnerProcess.AllocatedResources.Add(resource);
                    insideThreads.Add(thread);
                    thread.OwnsSemaphore = true;
                    thread.WaitingForSemaphore = false;
                    thread.PreviousState = thread.CurrentState;
                    thread.CurrentState = EnumThreadState.IN_SEMAPHORE;
                    slots--;
                }
                else
                {
                    waitingThreads.Enqueue(thread);
                    thread.OwnerProcess.RequestedResources.Add(resource);
                    thread.OwnsSemaphore = false;
                    thread.WaitingForSemaphore = true;
                    thread.PreviousState = thread.CurrentState;
                    thread.CurrentState = EnumThreadState.WAITING_SEMAPHORE;
                }
            }
            else
            {
                MessageBox.Show("Can not capture a thread that has already been captured by the semaphore");
                return int.MinValue;
            }

            return slots;
        }

        public int Down(ref SimulatorProcess process)
        {
            if(!insideProcesses.Contains(process) && !waitingProcesses.Contains(process))
            {
                if (slots > 0)
                {
                    process.AllocatedResources.Add(resource);
                    insideProcesses.Add(process);
                    process.OwnsSemaphore = true;
                    process.WaitingForSemaphore = false;
                    process.PreviousState = process.CurrentState;
                    process.CurrentState = EnumProcessState.WAITING_SEMAPHORE;
                }
                else
                {
                    waitingProcesses.Enqueue(process);
                    process.RequestedResources.Add(resource);
                    process.OwnsSemaphore = false;
                    process.WaitingForSemaphore = true;
                    process.PreviousState = process.CurrentState;
                    process.CurrentState = EnumProcessState.WAITING_SEMAPHORE;
                }
            }
            else
            {
                MessageBox.Show("Can not capture a process that has already been captured by the semaphore");
                return int.MinValue;
            }
            return slots;
        }
        /// <summary>
        /// This function captures the next thread waiting for the semaphore after a thread has been released
        /// </summary>
        private void CaptureNextThread()
        {
            waitingThreads = new Queue<SimulatorThread>(waitingThreads.OrderBy(x => x.ThreadPriority));
            if (waitingThreads.Any())
            {
                if (waitingThreads.Peek() != null)
                {
                    insideThreads.Add(waitingThreads.Dequeue());
                    insideThreads.Last().OwnerProcess.RequestedResources.Remove(resource);
                    insideThreads.Last().OwnerProcess.AllocatedResources.Add(resource);
                    insideThreads.Last().PreviousState = insideThreads.Last().CurrentState;
                    insideThreads.Last().CurrentState = EnumThreadState.IN_SEMAPHORE;
                    slots--;
                }
            }
        }

        /// <summary>
        /// This function captures the next process waiting for the semaphore after a process has been released
        /// </summary>
        private void CaptureNextProcess()
        {
            waitingProcesses = new Queue<SimulatorProcess>(waitingProcesses.OrderBy(x => x.ProcessPriority));
            if (waitingProcesses.Any())
            {
                if (waitingProcesses.Peek() != null)
                {
                    insideProcesses.Add(waitingProcesses.Dequeue());
                    insideProcesses.Last().RequestedResources.Remove(resource);
                    insideProcesses.Last().AllocatedResources.Add(resource);
                    insideProcesses.Last().PreviousState = insideProcesses.Last().CurrentState;
                    insideProcesses.Last().CurrentState = EnumProcessState.IN_SEMAPHORE;
                    slots--;
                }
            }
        }
        /// <summary>
        /// This function returns the current thread using this semaphore
        /// </summary>
        /// <returns>the current thread using this semaphore</returns>
        public SimulatorThread GetCurrentThread()
        {
            if (insideThreads.FirstOrDefault() != null)
                return insideThreads.FirstOrDefault();
            else
                return null;
        }

        /// <summary>
        /// This function returns the current process using this semaphore
        /// </summary>
        /// <returns>the current process using this semaphore</returns>
        public SimulatorProcess GetCurrentProcess()
        {
            if (insideProcesses.FirstOrDefault() != null)
                return insideProcesses.FirstOrDefault();
            else
                return null;
        }

    }
}
