using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace CPU_OS_Simulator.WindowBridge
{
    /// <summary>
    /// This class represents a way to access the user interface windows within code modules
    /// </summary>
    public class WindowAccessor
    {
        /// <summary>
        /// A dispatcher object which can dispatch tasks to the main program thread
        /// </summary>
        public Dispatcher dispatcher = Dispatcher.CurrentDispatcher;

        /// <summary>
        /// Constructor for Window Accessor
        /// </summary>
        public WindowAccessor()
        {
            Console.WriteLine("Window Accessor Created");
        }

        /// <summary>
        /// This function updates the main window interface by dispatching a task to the main thread
        /// </summary>
        /// <returns>A task to indicate to the calling thread that the function has finished executing</returns>
        public async Task<int> UpdateMainWindowInterface()
        {
            Console.WriteLine("Updating Main Window Interface From Window Accessor");
            await CallFromMainThread(WindowInstances.MainWindowInstance.UpdateInterface);
            return 0;
        }

        /// <summary>
        /// Bridge function used to call functions on the main thread from within the background thread
        /// </summary>
        /// <param name="FunctionPointer"> The function to call </param>
        /// <returns>A task to indicate to the calling thread that the function has finished executing</returns>
        private async Task<int> CallFromMainThread(Func<Task<int>> FunctionPointer)
        {
            Console.WriteLine("Updating Main Window Interface Using Main thread From Window Accessor");
            var invoke = dispatcher.Invoke(FunctionPointer);
            if (invoke != null) await invoke;
            return 0;
        }
    }
}
