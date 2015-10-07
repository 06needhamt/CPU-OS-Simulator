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
using CPU_OS_Simulator.CPU;

namespace CPU_OS_Simulator
{
    /// <summary>
    /// Interaction logic for InstructionsWindow.xaml
    /// </summary>
    public partial class InstructionsWindow : Window
    {
        public InstructionsWindow()
        {
            InitializeComponent();
        }

        private void btn_Close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            InstructionTabs.SelectedItem = DataTransferTab;
        }

        private void InstructionTabs_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            TabItem Selected = (TabItem) InstructionTabs.SelectedItem;
            if(Selected.Header.Equals("Data Transfer"))
            {
                lst_OpcodeListDataTransfer.Items.Clear();
                int MemberCount = Enum.GetNames(typeof(EnumOpcodes)).Length;
                for (int i = -0; i < MemberCount; i++)
                {
                    string opcode = ((EnumOpcodes)i).ToString();
                    lst_OpcodeListDataTransfer.Items.Add(opcode);
                }
            } 
            
        }
    }
}
