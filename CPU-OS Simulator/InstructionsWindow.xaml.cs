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
using System.Reflection;
using System.ComponentModel;

namespace CPU_OS_Simulator
{
    /// <summary>
    /// Interaction logic for InstructionsWindow.xaml
    /// </summary>
    public partial class InstructionsWindow : Window
    {
        private List<string> instructionDescriptions = new List<string>();
        private MainWindow owner;

        public InstructionsWindow()
        {
            InitializeComponent();
        }
        public InstructionsWindow(MainWindow owner)
        {
            InitializeComponent();
            this.owner = owner;
            owner.SayHello();
        }
        private void PopulateInstructions()
        {
            int count = Enum.GetNames(typeof(EnumOpcodes)).Length;
            for (int i = 0; i < count; i++)
            {
                EnumOpcodes value = (EnumOpcodes)i;
                string fulldescription = Extentions.DescriptionAttr<EnumOpcodes>(value);
                string opcode = value.ToString();
                string[] split = fulldescription.Split(new char[] { ':' });
                string category = split[0];
                string description = split[1];
                instructionDescriptions.Add(description);
                switch (category)
                {
                    case "Data Transfer":
                        {
                            lst_OpcodeListDataTransfer.Items.Add(opcode);
                            break;
                        }
                    case "Logical":
                        {
                            lst_OpcodeListLogical.Items.Add(opcode);
                            break;
                        }
                    case "Arithmetic":
                        {
                            lst_OpcodeListArithmetic.Items.Add(opcode);
                            break;
                        }
                    default:
                        {
                            throw new Exception("Unknown instruction category");
                        }
                }
            }
        }

        private void btn_Close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            InstructionTabs.SelectedItem = DataTransferTab;
            PopulateInstructions();

        }

        private void lst_OpcodeListDataTransfer_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            EnumOpcodes selected = (EnumOpcodes) Enum.Parse(typeof(EnumOpcodes), lst_OpcodeListDataTransfer.SelectedItem.ToString());
            txtDescriptionDataTransfer.Text = instructionDescriptions.ElementAt<string>((int)selected);
        }

        private void lst_OpcodeListLogical_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            EnumOpcodes selected = (EnumOpcodes)Enum.Parse(typeof(EnumOpcodes), lst_OpcodeListLogical.SelectedItem.ToString());
            txtDescriptionLogical.Text = instructionDescriptions.ElementAt<string>((int)selected);
        }

        private void lst_OpcodeListArithmetic_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            EnumOpcodes selected = (EnumOpcodes)Enum.Parse(typeof(EnumOpcodes), lst_OpcodeListArithmetic.SelectedItem.ToString());
            txtDescriptionArithmetic.Text = instructionDescriptions.ElementAt<string>((int)selected);
        }

        private void InstructionsWindow1_Closing(object sender, CancelEventArgs e)
        {
            Console.WriteLine("Instruction Window closing");
            TabItem SelectedTab = (TabItem) InstructionTabs.SelectedItem;
            string opcode;
            switch (SelectedTab.Header.ToString())
            {    
                case "Data Transfer":
                    {
                        opcode = lst_OpcodeListDataTransfer.SelectedItem.ToString();
                        break;
                    }
            }
        }

        // TODO FIX Commented out due to strange null pointer when checking unchecking radio buttons

        //private void rdb_SourceValueDataTransfer_Checked(object sender, RoutedEventArgs e)
        //{
        //    rdb_SourceRegisterDataTransfer.IsChecked = false;
        //    cmb_SourceRegisterDataTransfer.IsEnabled = false;
        //    txtSourceValueDataTransfer.IsEnabled = true;
        //}

        //private void rdb_SourceRegisterDataTransfer_Checked(object sender, RoutedEventArgs e)
        //{
        //    rdb_SourceValueDataTransfer.IsChecked = false;
        //    //cmb_SourceRegisterDataTransfer.IsEnabled = true;
        //    txtSourceValueDataTransfer.IsEnabled = false;

        //}

        //private void rdb_DestinationValueDataTransfer_Checked(object sender, RoutedEventArgs e)
        //{
        //    rdb_DestinationRegisterDataTransfer.IsChecked = false;
        //    cmb_DestinationRegisterDataTransfer.IsEnabled = false;
        //    txtDestinationValueDataTransfer.IsEnabled = true;
        //}

        //private void rdb_DestinationRegisterDataTransfer_Checked(object sender, RoutedEventArgs e)
        //{
        //    rdb_DestinationValueDataTransfer.IsChecked = false;
        //    cmb_DestinationRegisterDataTransfer.IsEnabled = true;
        //    txtDestinationValueDataTransfer.IsEnabled = false;

        //}

    }
}
