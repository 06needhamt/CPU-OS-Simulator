using System;
using System.Collections.Generic;
using System.Linq;
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
using CPU_OS_Simulator.Memory;

namespace CPU_OS_Simulator
{
    /// <summary>
    /// Interaction logic for PageTableWindow.xaml
    /// </summary>
    public partial class PageTableWindow : Window
    {
        private MemoryWindow parent;

        private PageTableWindow()
        {
            InitializeComponent();
        }

        public PageTableWindow(MemoryWindow parent)
        {
            this.parent = parent;
            InitializeComponent();
        }

        private void PageTableWindow1_Loaded(object sender, RoutedEventArgs e)
        {
            lst_Pages.ItemsSource = null;
            lst_Pages.Items.Clear();
            lst_Pages.ItemsSource = parent.MainWindowInstance.Memory.Table.Entries;

        }

        private void chk_Stay_On_Top_Checked(object sender, RoutedEventArgs e)
        {
            this.Topmost = true;
        }

        private void chk_Stay_On_Top_Unchecked(object sender, RoutedEventArgs e)
        {
            this.Topmost = false;
        }

        private void btn_Close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btn_Swap_Out_Click(object sender, RoutedEventArgs e)
        {
            if (((Button) sender).Content.ToString().Equals("SWAP OUT"))
            {
                int index = lst_Pages.SelectedIndex;
                parent.MainWindowInstance.Memory.Table.Entries[index].Page.SwapOut(0, index);
            }
            else if (((Button) sender).Content.ToString().Equals("SWAP IN"))
            {
                int index = lst_Pages.SelectedIndex;
                parent.MainWindowInstance.Memory.Table.Entries[index].Page.SwapIn(0, index);
            }
            else
            {
                throw new Exception("Unknown memory page operation requested " + ((Button) sender).Content.ToString());
            }
            UpdateEntries();
        }

        private void UpdateEntries()
        {
            lst_Pages.ItemsSource = null;
            lst_Pages.Items.Clear();
            lst_Pages.ItemsSource = parent.MainWindowInstance.Memory.Table.Entries;
        }

        private void lst_Pages_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            PageTableEntry selectedEntry = (PageTableEntry) lst_Pages.SelectedItem;
            if (selectedEntry != null)
            { 
                if (selectedEntry.SwappedOut)
                {
                    btn_Swap_Out.Content = "SWAP IN";
                }
                else
                {
                    btn_Swap_Out.Content = "SWAP OUT";
                }
            }
        }
    }
}
