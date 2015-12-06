using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows;

namespace CPU_OS_Simulator.CPU
{
    /// <summary>
    /// This class represents a Last in first out (LIFO) stack data structure for use with simulator programs
    /// </summary>
    public class ProgramStack
    {
        #region Global Variables

        /// <summary>
        /// The items held within the stack
        /// </summary>
        private List<StackItem> stackItems = new List<StackItem>();

        /// <summary>
        /// The maximum size of the stack before it overflows
        /// </summary>
        private readonly int maxStackSize = 1024;

        /// <summary>
        /// the current size of the stack
        /// </summary>
        private int stackSize;

        #endregion Global Variables

        #region Constructors

        /// <summary>
        /// Constructor for program stack objects
        /// </summary>
        public ProgramStack()
        {
            stackSize = 0;
        }

        #endregion Constructors

        #region Methods

        /// <summary>
        /// this function pushes an item onto the stack
        /// </summary>
        /// <param name="item"> the item to be pushed onto the stack</param>
        public void pushItem(StackItem item)
        {
            if (stackSize + 1 > maxStackSize)
            {
                MessageBox.Show("Stack Overflow the program will now terminate", "", MessageBoxButton.OK, MessageBoxImage.Error);
                GetExecutionUnit().Stop = true;
                return;
            }
            item.Position = stackItems.Count;
            stackItems.Add(item);
            stackSize++;
            SetAnnotations();
        }

        /// <summary>
        /// This method sets an annotation to a stack item
        /// BOS if the item is at the bottom of the stack
        /// TOS if the item is at the top of the stack
        /// or an empty string if it in the middle of the stack
        /// </summary>
        private void SetAnnotations()
        {
            for (int i = 0; i < stackSize; i++)
            {
                if (i == 0 && stackSize == 1)
                {
                    stackItems.ElementAt(i).Annotation = "BOS";
                }
                else if (i == 0)
                {
                    stackItems.ElementAt(i).Annotation = "TOS";
                }
                else if (i == (stackSize - 1))
                {
                    stackItems.ElementAt(i).Annotation = "BOS";
                }
                else
                {
                    stackItems.ElementAt(i).Annotation = "";
                }
            }
        }

        /// <summary>
        /// This item pops a value off the top of the stack
        /// </summary>
        /// <returns> the value that was popped off the stack</returns>
        public int popItem()
        {
            if (stackSize - 1 < 0)
            {
                MessageBox.Show("Stack Underflow the program will now terminate", "", MessageBoxButton.OK, MessageBoxImage.Error);
                GetExecutionUnit().Stop = true;
                return int.MinValue;
            }
            int value = stackItems.Last().Value;
            stackItems.RemoveAt(stackItems.Count - 1);
            stackSize--;
            SetAnnotations();
            return value;
        }

        private dynamic GetMainWindowInstance()
        {
            Assembly windowBridge = Assembly.LoadFrom("CPU_OS_Simulator.WindowBridge.dll"); // Load the window bridge module
            Console.WriteLine(windowBridge.GetExportedTypes()[0]);
            Type WindowType = windowBridge.GetType(windowBridge.GetExportedTypes()[0].ToString()); // get the name of the type that contains the window instances
            dynamic window = WindowType.GetField("MainWindowInstance").GetValue(null); // get the value of the static MainWindowInstance field
            return window;
        }

        private ExecutionUnit GetExecutionUnit()
        {
            dynamic window = GetMainWindowInstance();
            return window.ActiveUnit;
        }

        #endregion Methods

        #region Properties

        public List<StackItem> StackItems
        {
            get
            {
                return stackItems;
            }

            set
            {
                stackItems = value;
                stackSize = stackItems.Count;
            }
        }

        public int MaxStackSize
        {
            get
            {
                return maxStackSize;
            }
        }

        public int StackSize
        {
            get
            {
                return stackSize;
            }

            set
            {
                stackSize = value;
            }
        }

        #endregion Properties
    }
}