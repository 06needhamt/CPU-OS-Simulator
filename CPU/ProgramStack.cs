using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CPU_OS_Simulator.CPU
{
    public class ProgramStack
    {
        private List<StackItem> stackItems = new List<StackItem>();
        private readonly int maxStackSize = 1024;
        private int stackSize;
         
        public ProgramStack()
        {
            stackSize = 0;
        }

        public void pushItem(StackItem item)
        {
            if(stackSize + 1 > maxStackSize)
            {
                MessageBox.Show("Stack Overflow the program will now terminate", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                // TODO Terminate Program
                return;
            }
            item.Position = stackItems.Count;
            stackItems.Add(item);
            stackSize++;
            SetAnnotations();
        }

        private void SetAnnotations()
        {
            for(int i = 0; i < stackSize; i++)
            {
                if(i == 0 && stackSize == 1)
                {
                    stackItems.ElementAt(i).Annotation = "TOS";
                }
                else if(i == 0)
                {
                    stackItems.ElementAt(i).Annotation = "BOS";
                }
                else if(i == (stackSize - 1))
                {
                    stackItems.ElementAt(i).Annotation = "TOS";
                }
                else
                {
                    stackItems.ElementAt(i).Annotation = "";
                }
            }
        }

        public int popItem()
        {
            if (stackSize - 1 < 0)
            {
                MessageBox.Show("Stack Underflow the program will now terminate", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                // TODO Terminate Program
                return int.MinValue;
            }
            int value = stackItems.Last().Value;
            stackItems.RemoveAt(stackItems.Count - 1);
            stackSize--;
            SetAnnotations();
            return value;
        }

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


    }
}
