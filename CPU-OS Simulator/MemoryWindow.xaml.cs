using CPU_OS_Simulator.Memory;
using System;
using System.Windows;

namespace CPU_OS_Simulator
{
    /// <summary>
    /// Interaction logic for MemoryWindow.xaml
    /// </summary>
    public partial class MemoryWindow : Window
    {
        private MainWindow window;
        private MemoryPage currentPage;
        public static MemoryWindow currentInstance;

        public MemoryWindow()
        {
            InitializeComponent();
        }

        public MemoryWindow(MainWindow window, MemoryPage currentPage) : this()
        {
            this.window = window;
            this.currentPage = currentPage;
            currentInstance = this;
            PopulateDataView();
        }

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
            if (rdb_Integer.IsChecked.Value)
            {
                int value = Convert.ToInt32(txt_IntegerValue.Text);
                int address = Convert.ToInt32(txt_AddressLocation.Text);
                int offset = address / currentPage.PageSize;

                if (offset == 0)
                {
                    offset = address % currentPage.PageSize;
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
                int offset = address / currentPage.PageSize;
                char[] chars = value.ToCharArray();
                if (offset == 0)
                {
                    offset = address % currentPage.PageSize;
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
                int offset = address / currentPage.PageSize;
                if (offset == 0)
                {
                    offset = address % currentPage.PageSize;
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
    }
}