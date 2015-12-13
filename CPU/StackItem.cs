namespace CPU_OS_Simulator.CPU
{
    /// <summary>
    /// This class represents an item that is stored on the stack
    /// </summary>
    public class StackItem
    {
        private string annotation;
        private int value;
        private int position;
        private int address;

        /// <summary>
        /// Constructor for stack item
        /// </summary>
        /// <param name="value"> the value to store on the stack</param>
        public StackItem(int value)
        {
            this.value = value;
        }
        /// <summary>
        /// Property to store the annotation associated with this stack item.
        /// i.e. "TOS" if the item is at the top of the stack.
        /// And "BOS" if the item is at the bottom of the stack.
        /// </summary>
        public string Annotation
        {
            get
            {
                return annotation;
            }

            set
            {
                annotation = value;
            }
        }
        /// <summary>
        /// Property to store the value within this stack item
        /// </summary>
        public int Value
        {
            get
            {
                return value;
            }

            set
            {
                this.value = value;
            }
        }
        /// <summary>
        /// Property to store the position of this item within the stack
        /// </summary>
        public int Position
        {
            get
            {
                return position;
            }

            set
            {
                position = value;
            }
        }
        /// <summary>
        /// Property to store the address of this stack item within memory
        /// </summary>
        public int Address
        {
            get
            {
                return address;
            }

            set
            {
                address = value;
            }
        }
    }
}