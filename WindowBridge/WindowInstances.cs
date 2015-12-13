namespace CPU_OS_Simulator.WindowBridge
{
    public class WindowInstances
    {
        public static MainWindow MainWindowInstance = MainWindow.currentInstance;
        public static MemoryWindow MemoryWindowInstance = MemoryWindow.currentInstance;
        public static ConsoleWindow ConsoleWindowInstance = ConsoleWindow.currentInstance;
    }
}