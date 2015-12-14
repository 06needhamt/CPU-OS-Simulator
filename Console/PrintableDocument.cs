using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing.Printing;
using System.Reflection;

namespace CPU_OS_Simulator.Console
{
    public class PrintableDocument : PrintDocument
    {
        /// <summary>
        /// Constructor for the PrintableDocument class
        /// </summary>
        public PrintableDocument()
        {

        }

        /// <summary>
        /// Raises the <see cref="E:System.Drawing.Printing.PrintDocument.BeginPrint"/> event. It is called after the <see cref="M:System.Drawing.Printing.PrintDocument.Print"/> method is called and before the first page of the document prints.
        /// </summary>
        /// <param name="e">A <see cref="T:System.Drawing.Printing.PrintEventArgs"/> that contains the event data. </param>
        protected override void OnBeginPrint(PrintEventArgs e)
        {
            base.OnBeginPrint(e);
            dynamic window = GetConsoleWindowInstance();
            window.txt_Console.Text += "\n Printing Document... \n";
            window.txt_Console.CaretIndex = window.txt_Console.Text.Length;
        }

        /// <summary>
        /// Raises the <see cref="E:System.Drawing.Printing.PrintDocument.EndPrint"/> event. It is called when the last page of the document has printed.
        /// </summary>
        /// <param name="e">A <see cref="T:System.Drawing.Printing.PrintEventArgs"/> that contains the event data. </param>
        protected override void OnEndPrint(PrintEventArgs e)
        {
            base.OnEndPrint(e);
            dynamic window = GetConsoleWindowInstance();
            window.txt_Console.Text += "Finished Printing Document \n";
            window.txt_Console.CaretIndex = window.txt_Console.Text.Length;
        }
        /// <summary>
        /// This function gets the Console window instance from the window bridge
        /// </summary>
        /// <returns> the active instance of main window </returns>
        private dynamic GetConsoleWindowInstance()
        {
            Assembly windowBridge = Assembly.LoadFrom("CPU_OS_Simulator.WindowBridge.dll"); // Load the window bridge module
            System.Console.WriteLine(windowBridge.GetExportedTypes()[0]);
            Type WindowType = windowBridge.GetType(windowBridge.GetExportedTypes()[0].ToString()); // get the name of the type that contains the window instances
            dynamic window = WindowType.GetField("ConsoleWindowInstance").GetValue(null); // get the value of the static ConsoleWindowInstance field
            return window;
        }
    }
}
