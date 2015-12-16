namespace CPU_OS_Simulator.CPU
{
    /// <summary>
    /// This class represents a status flag used within the CPU to determine whether a condition has been met.
    /// </summary>
    public class StatusFlags
    {
        /// <summary>
        /// This variable represents the Overflow flag which is set when an integer overflow occurs
        /// </summary>
        public static StatusFlags OV = new StatusFlags(false, 4);
        /// <summary>
        /// This variable represents the zero flag which is set when a value is equal to zero (0)
        /// </summary>
        public static StatusFlags Z = new StatusFlags(false, 1);
        /// <summary>
        /// This variable represents the negative flag which is set when a value is less than zero (0)
        /// </summary>
        public static StatusFlags N = new StatusFlags(false, 2);

        private bool isSet;
        private int value;

        /// <summary>
        /// Constructor for status flags
        /// </summary>
        /// <param name="set">weather the flag should be set</param>
        /// <param name="value"> the integral value of this flag</param>
        protected StatusFlags(bool set, int value)
        {
            isSet = set;
            this.value = value;
        }
        /// <summary>
        /// This function toggles a flag
        /// i.e. if it is set it will be reset
        /// if it is reset it will be set.
        /// </summary>
        public void ToggleFlag()
        {
            isSet = !isSet;
        }
        /// <summary>
        /// Property to store whether the flag is set
        /// </summary>
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
        /// <summary>
        /// property to store the integral value of the flag
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
    }
}