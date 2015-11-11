namespace CPU_OS_Simulator.CPU
{
    public class StackItem
    {
        private string annotation;
        private int value;
        private int position;
        private int address;

        public StackItem(int value)
        {
            this.value = value;
        }

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