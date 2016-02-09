using System;
using System.ComponentModel;
using System.Reflection;
using System.Windows;
using CPU_OS_Simulator.Memory;

namespace CPU_OS_Simulator
{
    /// <summary>
    /// Interaction logic for MemoryWindow.xaml
    /// </summary>
    public partial class MemoryWindow : Window
    {
        private MainWindow mainParentWindow;
        private PhysicalMemoryWindow physicalMemoryParentWindow;
        private MemoryPage currentPage;
        /// <summary>
        /// Variable to hold the current instance of the memory window
        /// </summary>
        public static MemoryWindow currentInstance;
        /// <summary>
        /// Default constructor for memory window
        /// </summary>
        public MemoryWindow()
        {
            InitializeComponent();
            currentInstance = this;
            SetMemoryWindowInstance();
        }
        /// <summary>
        /// Constructor for memory window that takes the window instance that is creating this window
        /// PLEASE NOTE: This constructor should always be used so data can be passed back to the parent window
        /// </summary>
        /// <param name="parentWindow">The window that is creating this window </param>
        /// <param name="currentPage"> The memory page to be displayed in the window </param>
        public MemoryWindow(MainWindow parentWindow, MemoryPage currentPage) : this()
        {
            this.mainParentWindow = parentWindow;
            this.currentPage = currentPage;
            currentInstance = this;
            SetMemoryWindowInstance();
            PopulateDataView();
        }

        public MemoryWindow(PhysicalMemoryWindow parentWindow, MemoryPage currentPage)
            : this()
        {
            this.mainParentWindow = null;
            this.physicalMemoryParentWindow = parentWindow;
            this.currentPage = currentPage;
            currentInstance = this;
            SetMemoryWindowInstance();
            PopulateDataView();
        }
        /// <summary>
        /// Property to hold the current instance of the memory window
        /// </summary>
        public MainWindow MainParentWindow
        {
            get { return mainParentWindow; }
            set { mainParentWindow = value; }
        }

        public PhysicalMemoryWindow PhysicalMemoryParentWindow
        {
            get { return physicalMemoryParentWindow; }
            set { physicalMemoryParentWindow = value; }
        }

        /// <summary>
        /// This function populates the data view with the contents of the current memoryPage
        /// </summary>
        private void PopulateDataView()
        {
            if (currentPage != null)
            {
                foreach (MemorySegment seg in currentPage.Data)
                {
                    if (seg != null)
                    {
                        seg.DataString = seg.BuildDataString();
                        lst_data.Items.Add(seg);
                    }
                }
            }
        }

        private void btn_Update_Click(object sender, RoutedEventArgs e)
        {
            //TODO update to work with new memory manager
            if (rdb_Integer.IsChecked.Value)
            {
                int value = Convert.ToInt32(txt_IntegerValue.Text);
                int address = Convert.ToInt32(txt_AddressLocation.Text);
                int offset = address / MemoryPage.PAGE_SIZE;

                if (offset == 0)
                {
                    offset = address % MemoryPage.PAGE_SIZE;
                }
                if (address > currentPage.EndOffset)
                {
                    // TODO Swap in required page
                    MessageBox.Show("Required Page is not in memory");
                }
                while (value > 255)
                {
                    currentPage.Data[offset / 8].SetByte(offset % 8, 255);
                    value -= 255;
                    offset++;
                }
                if (value != 0)
                {
                    currentPage.Data[offset / 8].SetByte(offset % 8, (byte)value);
                }
            }
            else if (rdb_String.IsChecked.Value)
            {
                string value = txt_StringValue.Text;
                int address = Convert.ToInt32(txt_AddressLocation.Text);
                int offset = address / MemoryPage.PAGE_SIZE;
                char[] chars = value.ToCharArray();
                if (offset == 0)
                {
                    offset = address % MemoryPage.PAGE_SIZE;
                }
                if (address > currentPage.EndOffset)
                {
                    // TODO Swap in required page
                    MessageBox.Show("Required Page is not in memory");
                }
                for (int i = 0; i < chars.Length; i++)
                {
                    currentPage.Data[offset / 8].SetByte(offset % 8, (byte)chars[i]);
                    offset++;
                }
            }
            else if (rdb_Boolean.IsChecked.Value)
            {
                bool value = (bool)cmb_BooleanValue.SelectedItem;
                int address = Convert.ToInt32(txt_AddressLocation.Text);
                int offset = address / MemoryPage.PAGE_SIZE;
                if (offset == 0)
                {
                    offset = address % MemoryPage.PAGE_SIZE;
                }
                if (address > currentPage.EndOffset)
                {
                    // TODO Swap in required page
                    MessageBox.Show("Required Page is not in memory");
                }
                if (value)
                {
                    currentPage.Data[offset / 8].SetByte(offset % 8, 0x01);
                }
                else
                {
                    currentPage.Data[offset / 8].SetByte(offset % 8, 0x00);
                }
            }
            else
            {
                MessageBox.Show("Please Select a data type");
            }
            UpdateData();
        }

        private void UpdateData()
        {
            lst_data.ItemsSource = null;
            lst_data.Items.Clear();
            PopulateDataView();
        }

        private void btn_Reset_All_Click(object sender, RoutedEventArgs e)
        {
            currentPage.ZeroMemory();
            UpdateData();
        }

        private void btn_ShowPageTable_Click(object sender, RoutedEventArgs e)
        {
            PageTableWindow window = new PageTableWindow(this);
            window.Show();
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            currentInstance = null;
            SetMemoryWindowInstance();
        }
        /// <summary>
        /// This method sets the current instance of memory window in the window bridge 
        /// so it can be accessed by other modules 
        /// </summary>
        private void SetMemoryWindowInstance()
        {
            Assembly windowBridge = Assembly.LoadFrom("CPU_OS_Simulator.WindowBridge.dll"); // Load the window bridge module
            //System.Console.WriteLine(windowBridge.GetExportedTypes()[1]);
            Type WindowType = windowBridge.GetType(windowBridge.GetExportedTypes()[1].ToString()); // get the name of the type that contains the window instances
            WindowType.GetField("MemoryWindowInstance").SetValue(null, currentInstance);
        }

    }
}