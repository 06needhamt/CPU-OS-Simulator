namespace CPU_OS_Simulator.WindowBridge
{
    /// <summary>
    /// This class represents all of the active window instances
    /// </summary>
    public class WindowInstances
    {
        /// <summary>
        /// This variable represents the active main window instance
        /// </summary>
        public static MainWindow MainWindowInstance = MainWindow.currentInstance;
        /// <summary>
        /// This variable represents the active memory window instance
        /// </summary>
        public static MemoryWindow MemoryWindowInstance = MemoryWindow.currentInstance;
        /// <summary>
        /// This variable represents the active console window instance
        /// </summary>
        public static ConsoleWindow ConsoleWindowInstance = ConsoleWindow.currentInstance;
        /// <summary>
        /// This variable represents the active operating system main window instance
        /// </summary>
        public static OperatingSystemMainWindow OperatingSystemMainWindowInstance =
            OperatingSystemMainWindow.currentInstance;
        /// <summary>
        /// This variable represents the active process control block window instance
        /// </summary>
        public static ProcessControlBlockWindow ProcessControlBlockWindowInstance = 
            ProcessControlBlockWindow.currentInstance;
        /// <summary>
        /// This variable represents the active process list window instance
        /// </summary>
        public static ProcessListWindow ProcessListWindowInstance = ProcessListWindow.currentInstance;
        /// <summary>
        /// This variable represents the active Log Window
        /// </summary>
        public static LogWindow LogWindowInstance = LogWindow.currentInstance;
        /// <summary>
        /// This variable represents the active Utilisation Window
        /// </summary>
        public static UtilisationWindow UtilisationWindowInstance = UtilisationWindow.currentInstance;
        /// <summary>
        /// This variable represents the active  PhysicalMemoryWindow instance
        /// </summary>
        public static PhysicalMemoryWindow PhysicalMemoryWindowInstance = PhysicalMemoryWindow.currentInstance;
    }
}