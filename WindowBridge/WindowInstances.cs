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
    }
}