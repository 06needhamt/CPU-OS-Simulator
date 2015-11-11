namespace CPU_OS_Simulator.CPU
{
    public class StatusFlags
    {
        public static StatusFlags OV = new StatusFlags(false, 4);
        public static StatusFlags Z = new StatusFlags(false, 1);
        public static StatusFlags N = new StatusFlags(false, 2);

        private bool isSet;
        private int value;

        protected StatusFlags(bool set, int value)
        {
            this.isSet = set;
            this.value = value;
        }

        public void ToggleFlag(StatusFlags flag)
        {
            flag.isSet = !flag.isSet;
        }

        public bool IsSet
        {
            get
            {
                return isSet;
            }

            set
            {
                isSet = value;
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
    }
}