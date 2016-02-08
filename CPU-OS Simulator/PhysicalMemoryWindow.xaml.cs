using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using CPU_OS_Simulator.Controls;
using CPU_OS_Simulator.Memory;

namespace CPU_OS_Simulator
{
    /// <summary>
    /// Interaction logic for PhysicalMemoryWindow.xaml
    /// </summary>
    public partial class PhysicalMemoryWindow : Window
    {
        public static PhysicalMemoryWindow currentInstance;
        private MainWindow mainParent;
        private OperatingSystemMainWindow osParent;
        private UtilisationWindow utilisationParent;

        public PhysicalMemoryWindow()
        {
            osParent = null;
            mainParent = null;
            this.utilisationParent = null;
            InitializeComponent();
            currentInstance = this;
            SetPhysicalMemoryWindowInstance();
        }

        public PhysicalMemoryWindow(MainWindow mainParent)
        {
            this.mainParent = mainParent;
            this.osParent = null;
            this.utilisationParent = null;
            InitializeComponent();
            currentInstance = this;
            SetPhysicalMemoryWindowInstance();
        }

        public PhysicalMemoryWindow(OperatingSystemMainWindow osParent)
        {
            this.osParent = osParent;
            this.mainParent = null;
            this.utilisationParent = null;
            InitializeComponent();
            currentInstance = this;
            SetPhysicalMemoryWindowInstance();
        }

        public PhysicalMemoryWindow(UtilisationWindow utilisationParent)
        {
            this.osParent = null;
            this.mainParent = null;
            this.utilisationParent = utilisationParent;
            InitializeComponent();
            currentInstance = this;
            SetPhysicalMemoryWindowInstance();
        }

        private void PhysicalMemoryWindow1_Loaded(object sender, RoutedEventArgs e)
        {
            if (osParent != null)
            {
                if (osParent.WindowParent != null)
                {
                    foreach (MemoryPage page in osParent.WindowParent.Memory.Pages)
                    {
                        MemoryButton b = new MemoryButton(page);
                        b.Content = "Size: " + MemoryPage.PAGE_SIZE + " Frame Number: " + page.FrameNumber + "Start Offset: " + page.StartOffset;
                        b.Width = lst_Pages.Width - 5;
                        b.Height = lst_Pages.Height / 10;
                        b.Click += delegate
                        {
                            MemoryWindow wind = new MemoryWindow(currentInstance, page);
                            wind.Show();
                        };
                        lst_Pages.Items.Add((MemoryButton)b);

                    }

                }
                else
                {
                    MessageBox.Show("ERROR With window manager OSWin Please report this error to your tutor \n" + "The program will now terminate");
                    Environment.Exit(1);
                }

            }
            else if (utilisationParent != null)
            {
                if (utilisationParent.OsParent.WindowParent != null)
                {
                    foreach (MemoryPage page in utilisationParent.OsParent.WindowParent.Memory.Pages)
                    {
                        MemoryButton b = new MemoryButton(page);
                        b.Content = "Size: " + MemoryPage.PAGE_SIZE + " Frame Number: " + page.FrameNumber + "Start Offset: " + page.StartOffset;
                        b.Width = lst_Pages.Width - 5;
                        b.Height = lst_Pages.Height / 10;
                        b.Click += delegate
                        {
                            MemoryWindow wind = new MemoryWindow(currentInstance, page);
                            wind.Show();
                        };
                        lst_Pages.Items.Add((MemoryButton)b);

                    }
                }
                else
                {
                    MessageBox.Show("ERROR With window manager UtilWin Please report this error to your tutor \n" + "The program will now terminate");
                    Environment.Exit(1);
                }
            }
            else if (mainParent != null)
            {
                foreach (MemoryPage page in mainParent.Memory.Pages)
                {
                    MemoryButton b = new MemoryButton(page);
                    b.Content = "Size: " + MemoryPage.PAGE_SIZE + " Frame Number: " + page.FrameNumber + "Start Offset: " + page.StartOffset;
                    b.Width = lst_Pages.Width - 5;
                    b.Height = lst_Pages.Height / 10;
                    b.Click += delegate
                    {
                        MemoryWindow wind = new MemoryWindow(currentInstance, page);
                        wind.Show();
                    };
                    lst_Pages.Items.Add((MemoryButton)b);

                }

            }
            else
            {
                MessageBox.Show("ERROR With window manager MainWin Please report this error to your tutor \n" + "The program will now terminate");
                Environment.Exit(1);
            }
        }

        /// <summary>
        /// This method sets the current instance of OS window in the window bridge 
        /// so it can be accessed by other modules 
        /// </summary>
        private void SetPhysicalMemoryWindowInstance()
        {
            Assembly windowBridge = Assembly.LoadFrom("CPU_OS_Simulator.WindowBridge.dll"); // Load the window bridge module
            //System.Console.WriteLine(windowBridge.GetExportedTypes()[1]);
            Type WindowType = windowBridge.GetType(windowBridge.GetExportedTypes()[1].ToString()); // get the name of the type that contains the window instances
            WindowType.GetField("PhysicalMemoryWindowInstance").SetValue(null, currentInstance);
        }
    }
}