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
        /// <summary>
        /// List to hold the descriptions of each instruction
        /// </summary>
        private List<string> instructionDescriptions = new List<string>();
        /// <summary>
        /// The window that owns this window
        /// </summary>
        private MainWindow owner;
        /// <summary>
        /// Default Constructor for Instruction Window
        /// </summary>
        public InstructionsWindow()
        {
            InitializeComponent();
        }
        /// <summary>
        /// Constructor for instruction window that takes the window instance that is creating this window
        /// PLEASE NOTE: This constructor should always be used so data can be passed back to the main window
        /// </summary>
        /// <param name="owner">The window that is creating this window </param>
        public InstructionsWindow(MainWindow owner)
        {
            InitializeComponent();
            this.owner = owner;
            #if DEBUG
            owner.SayHello();
            #endif
        }
        /// <summary>
        /// this function populates each of the instruction list boxes with the correct instructions
        /// </summary>
        private void PopulateInstructions()
        {
            int count = Enum.GetNames(typeof(EnumOpcodes)).Length;
            for (int i = 0; i < count; i++)
            {
                EnumOpcodes value = (EnumOpcodes)i;
                string fulldescription = Extentions.DescriptionAttr<EnumOpcodes>(value);
                string opcode = value.ToString();
                string[] split = fulldescription.Split(new char[] { ':' }); // split the catagory from the description
                string category = split[0];
                string description = split[1];
                instructionDescriptions.Add(description); // add the description to the list
                switch (category)
                {
                    //TODO Add other instruction Types
                    case "Data Transfer":
                        {
                            lst_OpcodeListDataTransfer.Items.Add(opcode); // populate data transfer instructions
                            break;
                        }
                    case "Logical":
                        {
                            lst_OpcodeListLogical.Items.Add(opcode); // populate logical instructions
                            break;
                        }
                    case "Arithmetic":
                        {
                            lst_OpcodeListArithmetic.Items.Add(opcode); //populate aritmetic instructions
                            break;
                        }
                    case "Control Transfer":
                        {
                            lst_OpcodeListControlTransfer.Items.Add(opcode); // populate control transfer instructions
                            break;
                        }
                    case "Comparison":
                        {
                            lst_OpcodeListComparison.Items.Add(opcode); // populate comparison instructions
                            break;
                        }
                    case "I/O":
                        {
                            lst_OpcodeListIO.Items.Add(opcode); // populate I/O instructions
                            break;
                        }
                    case "Miscellaneous":
                        {
                            lst_OpcodeListMiscellaneous.Items.Add(opcode); // populate Miscellaneous instructions
                            break;
                        }
                    default:
                        {
                            throw new Exception("Unknown instruction category"); // we should never get here
                        }
                }
            }
        }
        /// <summary>
        /// This event handler is called when the close button is clicked
        /// </summary>
        /// <param name="sender"> the object that fired this event</param>
        /// <param name="e"> the eventargs associated with this event</param>
        private void btn_Close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        /// <summary>
        /// This event handler is called when the window first loads
        /// </summary>
        /// <param name="sender"> the object that fired this event</param>
        /// <param name="e"> the eventargs associated with this event</param>
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            InstructionTabs.SelectedItem = DataTransferTab;
            PopulateInstructions();

        }
        #region Populate Instruction Descriptions
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
        #endregion
        /// <summary>
        /// This event handler is called when the window is closing
        /// </summary>
        /// <param name="sender"> the object that fired this event</param>
        /// <param name="e"> the eventargs associated with this event</param>
        private void InstructionsWindow1_Closing(object sender, CancelEventArgs e)
        {
            Console.WriteLine("Instruction Window closing");
            CreateInstruction();
        }
        /// <summary>
        /// Creates an instruction based on selected options
        /// </summary>
        private void CreateInstruction()
        {
            TabItem SelectedTab = (TabItem)InstructionTabs.SelectedItem;
            EnumOpcodes opcode;
            Operand op1;
            Operand op2;
            switch (SelectedTab.Header.ToString())
            {
                case "Data Transfer":
                    {
                        opcode = (EnumOpcodes)Enum.Parse(typeof(EnumOpcodes), lst_OpcodeListDataTransfer.SelectedItem.ToString());
                        if (rdb_SourceValueDataTransfer.IsChecked.Value)
                        {
                            op1 = new Operand(Convert.ToInt32(txtSourceValueDataTransfer.Text), EnumOperandType.VALUE);
                        }
                        else if (rdb_SourceRegisterDataTransfer.IsChecked.Value)
                        {
                            string selectedRegister = (string)cmb_SourceRegisterDataTransfer.SelectedItem;
                            Register reg = Register.FindRegister(selectedRegister);
                            op1 = new Operand(reg, reg.Type);
                        }
                        else
                        {
                            op1 = null;
                        }
                        if (rdb_DestinationValueDataTransfer.IsChecked.Value)
                        {
                            op2 = new Operand(Convert.ToInt32(txtDestinationValueDataTransfer.Text), EnumOperandType.VALUE);
                        }
                        else if (rdb_DestinationRegisterDataTransfer.IsChecked.Value)
                        {
                            string selectedRegister = (string)cmb_DestinationRegisterDataTransfer.SelectedItem;
                            Register reg = Register.FindRegister(selectedRegister);
                            op2 = new Operand(reg, reg.Type);
                        }
                        else
                        {
                            op2 = null;
                        }
                        Instruction i = owner.CreateInstruction(opcode, op1, op2, 4);
                        owner.AddInstruction(i);
                        break;
                    }
                case "Logical":
                    {
                        opcode = (EnumOpcodes)Enum.Parse(typeof(EnumOpcodes), lst_OpcodeListLogical.SelectedItem.ToString());
                        if (rdb_SourceValueLogical.IsChecked.Value)
                        {
                            op1 = new Operand(Convert.ToInt32(txtSourceValueLogical.Text), EnumOperandType.VALUE);
                        }
                        else if (rdb_SourceRegisterLogical.IsChecked.Value)
                        {
                            string selectedRegister = (string)cmb_SourceRegisterLogical.SelectedItem;
                            Register reg = Register.FindRegister(selectedRegister);
                            op1 = new Operand(reg, reg.Type);
                        }
                        else
                        {
                            op1 = null;
                        }
                        if (rdb_DestinationValueLogical.IsChecked.Value)
                        {
                            op2 = new Operand(Convert.ToInt32(txtDestinationValueLogical.Text), EnumOperandType.VALUE);
                        }
                        else if (rdb_DestinationRegisterLogical.IsChecked.Value)
                        {
                            string selectedRegister = (string)cmb_DestinationRegisterLogical.SelectedItem;
                            Register reg = Register.FindRegister(selectedRegister);
                            op2 = new Operand(reg, reg.Type);
                        }
                        else
                        {
                            op2 = null;
                        }
                        Instruction i = owner.CreateInstruction(opcode, op1, op2, 4);
                        owner.AddInstruction(i);
                        break;
                    }
                case "Arithmetic":
                    {
                        opcode = (EnumOpcodes)Enum.Parse(typeof(EnumOpcodes), lst_OpcodeListArithmetic.SelectedItem.ToString());
                        if (rdb_SourceValueArithmetic.IsChecked.Value)
                        {
                            op1 = new Operand(Convert.ToInt32(txtSourceValueArithmetic.Text), EnumOperandType.VALUE);
                        }
                        else if (rdb_SourceRegisterArithmetic.IsChecked.Value)
                        {
                            string selectedRegister = (string)cmb_SourceRegisterArithmetic.SelectedItem;
                            Register reg = Register.FindRegister(selectedRegister);
                            op1 = new Operand(reg, reg.Type);
                        }
                        else
                        {
                            op1 = null;
                        }
                        if (rdb_DestinationValueArithmetic.IsChecked.Value)
                        {
                            op2 = new Operand(Convert.ToInt32(txtDestinationValueArithmetic.Text), EnumOperandType.VALUE);
                        }
                        else if (rdb_DestinationRegisterArithmetic.IsChecked.Value)
                        {
                            string selectedRegister = (string)cmb_DestinationRegisterArithmetic.SelectedItem;
                            Register reg = Register.FindRegister(selectedRegister);
                            op2 = new Operand(reg, reg.Type);
                        }
                        else
                        {
                            op2 = null;
                        }
                        Instruction i = owner.CreateInstruction(opcode, op1, op2, 4);
                        owner.AddInstruction(i);
                        break;
                    }
                case "Control Transfer":
                    {
                        opcode = (EnumOpcodes)Enum.Parse(typeof(EnumOpcodes), lst_OpcodeListControlTransfer.SelectedItem.ToString());
                        if (rdb_SourceValueControlTransfer.IsChecked.Value)
                        {
                            op1 = new Operand(Convert.ToInt32(txtSourceValueControlTransfer.Text), EnumOperandType.VALUE);
                        }
                        else if (rdb_SourceRegisterControlTransfer.IsChecked.Value)
                        {
                            string selectedRegister = (string)cmb_SourceRegisterControlTransfer.SelectedItem;
                            Register reg = Register.FindRegister(selectedRegister);
                            op1 = new Operand(reg, reg.Type);
                        }
                        else
                        {
                            op1 = null;
                        }

                        if (rdb_DestinationValueControlTransfer.IsChecked.Value)
                        {
                            op2 = new Operand(Convert.ToInt32(txtDestinationValueControlTransfer.Text), EnumOperandType.VALUE);
                        }
                        else if (rdb_DestinationRegisterControlTransfer.IsChecked.Value)
                        {
                            string selectedRegister = (string)cmb_DestinationRegisterControlTransfer.SelectedItem;
                            Register reg = Register.FindRegister(selectedRegister);
                            op2 = new Operand(reg, reg.Type);
                        }
                        else
                        {
                            op2 = null;
                        }
                        Instruction i = owner.CreateInstruction(opcode, op1, op2, 4);
                        owner.AddInstruction(i);
                        break;
                    }

                case "I/O":
                    {
                        opcode = (EnumOpcodes)Enum.Parse(typeof(EnumOpcodes), lst_OpcodeListIO.SelectedItem.ToString());
                        if (rdb_SourceValueIO.IsChecked.Value)
                        {
                            op1 = new Operand(Convert.ToInt32(txtSourceValueIO.Text), EnumOperandType.VALUE);
                        }
                        else if (rdb_SourceRegisterIO.IsChecked.Value)
                        {
                            string selectedRegister = (string)cmb_SourceRegisterIO.SelectedItem;
                            Register reg = Register.FindRegister(selectedRegister);
                            op1 = new Operand(reg, reg.Type);
                        }
                        else
                        {
                            op1 = null;
                        }
                        if (rdb_DestinationValueIO.IsChecked.Value)
                        {
                            op2 = new Operand(Convert.ToInt32(txtDestinationValueIO.Text), EnumOperandType.VALUE);
                        }
                        else if (rdb_DestinationRegisterIO.IsChecked.Value)
                        {
                            string selectedRegister = (string)cmb_DestinationRegisterIO.SelectedItem;
                            Register reg = Register.FindRegister(selectedRegister);
                            op2 = new Operand(reg, reg.Type);
                        }
                        else
                        {
                            op2 = null;
                        }
                        Instruction i = owner.CreateInstruction(opcode, op1, op2, 4);
                        owner.AddInstruction(i);
                        break;
                    }
                case "Miscellaneous":
                    {
                        opcode = (EnumOpcodes)Enum.Parse(typeof(EnumOpcodes), lst_OpcodeListMiscellaneous.SelectedItem.ToString());
                        if (rdb_SourceValueMiscellaneous.IsChecked.Value)
                        {
                            op1 = new Operand(Convert.ToInt32(txtSourceValueMiscellaneous.Text), EnumOperandType.VALUE);
                        }
                        else if (rdb_SourceRegisterMiscellaneous.IsChecked.Value)
                        {
                            string selectedRegister = (string)cmb_SourceRegisterMiscellaneous.SelectedItem;
                            Register reg = Register.FindRegister(selectedRegister);
                            op1 = new Operand(reg, reg.Type);
                        }
                        else
                        {
                            op1 = null;
                        }
                        if (rdb_DestinationValueMiscellaneous.IsChecked.Value)
                        {
                            op2 = new Operand(Convert.ToInt32(txtDestinationValueMiscellaneous.Text), EnumOperandType.VALUE);
                        }
                        else if (rdb_DestinationRegisterMiscellaneous.IsChecked.Value)
                        {
                            string selectedRegister = (string)cmb_DestinationRegisterMiscellaneous.SelectedItem;
                            Register reg = Register.FindRegister(selectedRegister);
                            op2 = new Operand(reg, reg.Type);
                        }
                        else
                        {
                            op2 = null;
                        }
                        Instruction i = owner.CreateInstruction(opcode, op1, op2, 4);
                        owner.AddInstruction(i);
                        break;
                    }
                default:
                    {
                        throw new Exception("Unknown tab selected");
                    }
            }
        }

        /// <summary>
        /// Finds the register object for the selected register
        /// </summary>
        /// <param name="selectedItem"> the selected register</param>
        /// <returns>The register object of the selected register</returns> 
       

        #region UI control
        private void rdb_SourceValueDataTransfer_Checked(object sender, RoutedEventArgs e)
        {
            rdb_SourceRegisterDataTransfer.IsChecked = false;
            cmb_SourceRegisterDataTransfer.IsEnabled = false;
            txtSourceValueDataTransfer.IsEnabled = true;
        }

        private void rdb_SourceRegisterDataTransfer_Checked(object sender, RoutedEventArgs e)
        {
            rdb_SourceValueDataTransfer.IsChecked = false;
            cmb_SourceRegisterDataTransfer.IsEnabled = true;
            txtSourceValueDataTransfer.IsEnabled = false;

        }

        private void rdb_DestinationValueDataTransfer_Checked(object sender, RoutedEventArgs e)
        {
            rdb_DestinationRegisterDataTransfer.IsChecked = false;
            cmb_DestinationRegisterDataTransfer.IsEnabled = false;
            txtDestinationValueDataTransfer.IsEnabled = true;
        }

        private void rdb_DestinationRegisterDataTransfer_Checked(object sender, RoutedEventArgs e)
        {
            rdb_DestinationValueDataTransfer.IsChecked = false;
            cmb_DestinationRegisterDataTransfer.IsEnabled = true;
            txtDestinationValueDataTransfer.IsEnabled = false;

        }

        private void rdb_SourceValueLogical_Checked(object sender, RoutedEventArgs e)
        {
            rdb_SourceRegisterLogical.IsChecked = false;
            cmb_SourceRegisterLogical.IsEnabled = false;
            txtSourceValueLogical.IsEnabled = true;
        }

        private void rdb_SourceRegisterLogical_Checked(object sender, RoutedEventArgs e)
        {
            rdb_SourceValueLogical.IsChecked = false;
            cmb_SourceRegisterLogical.IsEnabled = true;
            txtSourceValueLogical.IsEnabled = false;

        }

        private void rdb_DestinationValueLogical_Checked(object sender, RoutedEventArgs e)
        {
            rdb_DestinationRegisterLogical.IsChecked = false;
            cmb_DestinationRegisterLogical.IsEnabled = false;
            txtDestinationValueLogical.IsEnabled = true;
        }

        private void rdb_DestinationRegisterLogical_Checked(object sender, RoutedEventArgs e)
        {
            rdb_DestinationValueLogical.IsChecked = false;
            cmb_DestinationRegisterLogical.IsEnabled = true;
            txtDestinationValueLogical.IsEnabled = false;

        }

        private void rdb_SourceValueArithmetic_Checked(object sender, RoutedEventArgs e)
        {
            rdb_SourceRegisterArithmetic.IsChecked = false;
            cmb_SourceRegisterArithmetic.IsEnabled = false;
            txtSourceValueArithmetic.IsEnabled = true;
        }

        private void rdb_SourceRegisterArithmetic_Checked(object sender, RoutedEventArgs e)
        {
            rdb_SourceValueArithmetic.IsChecked = false;
            cmb_SourceRegisterArithmetic.IsEnabled = true;
            txtSourceValueArithmetic.IsEnabled = false;

        }

        private void rdb_DestinationValueArithmetic_Checked(object sender, RoutedEventArgs e)
        {
            rdb_DestinationRegisterArithmetic.IsChecked = false;
            cmb_DestinationRegisterArithmetic.IsEnabled = false;
            txtDestinationValueArithmetic.IsEnabled = true;
        }

        private void rdb_DestinationRegisterArithmetic_Checked(object sender, RoutedEventArgs e)
        {
            rdb_DestinationValueArithmetic.IsChecked = false;
            cmb_DestinationRegisterArithmetic.IsEnabled = true;
            txtDestinationValueArithmetic.IsEnabled = false;

        }

        private void rdb_SourceValueControlTransfer_Checked(object sender, RoutedEventArgs e)
        {
            rdb_SourceRegisterControlTransfer.IsChecked = false;
            cmb_SourceRegisterControlTransfer.IsEnabled = false;
            txtSourceValueControlTransfer.IsEnabled = true;
        }

        private void rdb_SourceRegisterControlTransfer_Checked(object sender, RoutedEventArgs e)
        {
            rdb_SourceValueControlTransfer.IsChecked = false;
            cmb_SourceRegisterControlTransfer.IsEnabled = true;
            txtSourceValueControlTransfer.IsEnabled = false;

        }

        private void rdb_DestinationValueControlTransfer_Checked(object sender, RoutedEventArgs e)
        {
            rdb_DestinationRegisterControlTransfer.IsChecked = false;
            cmb_DestinationRegisterControlTransfer.IsEnabled = false;
            txtDestinationValueControlTransfer.IsEnabled = true;
        }

        private void rdb_DestinationRegisterControlTransfer_Checked(object sender, RoutedEventArgs e)
        {
            rdb_DestinationValueControlTransfer.IsChecked = false;
            cmb_DestinationRegisterControlTransfer.IsEnabled = true;
            txtDestinationValueControlTransfer.IsEnabled = false;

        }

        private void rdb_SourceValueComparison_Checked(object sender, RoutedEventArgs e)
        {
            rdb_SourceRegisterComparison.IsChecked = false;
            cmb_SourceRegisterComparison.IsEnabled = false;
            txtSourceValueComparison.IsEnabled = true;
        }

        private void rdb_SourceRegisterComparison_Checked(object sender, RoutedEventArgs e)
        {
            rdb_SourceValueComparison.IsChecked = false;
            cmb_SourceRegisterComparison.IsEnabled = true;
            txtSourceValueComparison.IsEnabled = false;

        }

        private void rdb_DestinationValueComparison_Checked(object sender, RoutedEventArgs e)
        {
            rdb_DestinationRegisterComparison.IsChecked = false;
            cmb_DestinationRegisterComparison.IsEnabled = false;
            txtDestinationValueComparison.IsEnabled = true;
        }

        private void rdb_DestinationRegisterComparison_Checked(object sender, RoutedEventArgs e)
        {
            rdb_DestinationValueComparison.IsChecked = false;
            cmb_DestinationRegisterComparison.IsEnabled = true;
            txtDestinationValueComparison.IsEnabled = false;

        }

        private void rdb_SourceValueIO_Checked(object sender, RoutedEventArgs e)
        {
            rdb_SourceRegisterIO.IsChecked = false;
            cmb_SourceRegisterIO.IsEnabled = false;
            txtSourceValueIO.IsEnabled = true;
        }

        private void rdb_SourceRegisterIO_Checked(object sender, RoutedEventArgs e)
        {
            rdb_SourceValueIO.IsChecked = false;
            cmb_SourceRegisterIO.IsEnabled = true;
            txtSourceValueIO.IsEnabled = false;

        }

        private void rdb_DestinationValueIO_Checked(object sender, RoutedEventArgs e)
        {
            rdb_DestinationRegisterIO.IsChecked = false;
            cmb_DestinationRegisterIO.IsEnabled = false;
            txtDestinationValueIO.IsEnabled = true;
        }

        private void rdb_DestinationRegisterIO_Checked(object sender, RoutedEventArgs e)
        {
            rdb_DestinationValueIO.IsChecked = false;
            cmb_DestinationRegisterIO.IsEnabled = true;
            txtDestinationValueIO.IsEnabled = false;

        }

        private void rdb_SourceValueMiscellaneous_Checked(object sender, RoutedEventArgs e)
        {
            rdb_SourceRegisterMiscellaneous.IsChecked = false;
            cmb_SourceRegisterMiscellaneous.IsEnabled = false;
            txtSourceValueMiscellaneous.IsEnabled = true;
        }

        private void rdb_SourceRegisterMiscellaneous_Checked(object sender, RoutedEventArgs e)
        {
            rdb_SourceValueMiscellaneous.IsChecked = false;
            cmb_SourceRegisterMiscellaneous.IsEnabled = true;
            txtSourceValueMiscellaneous.IsEnabled = false;

        }

        private void rdb_DestinationValueMiscellaneous_Checked(object sender, RoutedEventArgs e)
        {
            rdb_DestinationRegisterMiscellaneous.IsChecked = false;
            cmb_DestinationRegisterMiscellaneous.IsEnabled = false;
            txtDestinationValueMiscellaneous.IsEnabled = true;
        }

        private void rdb_DestinationRegisterMiscellaneous_Checked(object sender, RoutedEventArgs e)
        {
            rdb_DestinationValueMiscellaneous.IsChecked = false;
            cmb_DestinationRegisterMiscellaneous.IsEnabled = true;
            txtDestinationValueMiscellaneous.IsEnabled = false;

        }

        private void txtSourceValueDataTransfer_GotFocus(object sender, RoutedEventArgs e)
        {
            rdb_SourceValueDataTransfer.IsChecked = true;
        }

        private void cmb_SourceRegisterDataTransfer_GotFocus(object sender, RoutedEventArgs e)
        {
            rdb_SourceRegisterDataTransfer.IsChecked = true;
        }

        private void txtDestinationValueDataTransfer_GotFocus(object sender, RoutedEventArgs e)
        {
            rdb_DestinationValueDataTransfer.IsChecked = true;
        }

        private void cmb_DestinationRegisterDataTransfer_GotFocus(object sender, RoutedEventArgs e)
        {
            rdb_DestinationRegisterDataTransfer.IsChecked = true;
        }

        private void txtSourceValueLogical_GotFocus(object sender, RoutedEventArgs e)
        {
            rdb_SourceValueLogical.IsChecked = true;
        }

        private void cmb_SourceRegisterLogical_GotFocus(object sender, RoutedEventArgs e)
        {
            rdb_SourceRegisterLogical.IsChecked = true;
        }

        private void txtDestinationValueLogical_GotFocus(object sender, RoutedEventArgs e)
        {
            rdb_DestinationValueLogical.IsChecked = true;
        }

        private void cmb_DestinationRegisterLogical_GotFocus(object sender, RoutedEventArgs e)
        {
            rdb_DestinationValueLogical.IsChecked = true;
        }

        private void txtSourceValueArithmetic_GotFocus(object sender, RoutedEventArgs e)
        {
            rdb_SourceValueArithmetic.IsChecked = true;
        }

        private void cmb_SourceRegisterArithmetic_GotFocus(object sender, RoutedEventArgs e)
        {
            rdb_SourceRegisterArithmetic.IsChecked = true;
        }

        private void txtDestinationValueArithmetic_GotFocus(object sender, RoutedEventArgs e)
        {
            rdb_DestinationValueArithmetic.IsChecked = true;
        }

        private void cmb_DestinationRegisterArithmetic_GotFocus(object sender, RoutedEventArgs e)
        {
            rdb_DestinationRegisterArithmetic.IsChecked = true;
        }

        private void txtSourceValueControlTransfer_GotFocus(object sender, RoutedEventArgs e)
        {
            rdb_SourceValueControlTransfer.IsChecked = true;
        }

        private void cmb_SourceRegisterControlTransfer_GotFocus(object sender, RoutedEventArgs e)
        {
            rdb_SourceRegisterControlTransfer.IsChecked = true;
        }

        private void txtDestinationValueControlTransfer_GotFocus(object sender, RoutedEventArgs e)
        {
            rdb_DestinationValueControlTransfer.IsChecked = true;
        }

        private void cmb_DestinationRegisterControlTransfer_GotFocus(object sender, RoutedEventArgs e)
        {
            rdb_DestinationRegisterControlTransfer.IsChecked = true;
        }

        private void txtSourceValueComparison_GotFocus(object sender, RoutedEventArgs e)
        {
            rdb_SourceValueComparison.IsChecked = true;
        }

        private void cmb_SourceRegisterComparison_GotFocus(object sender, RoutedEventArgs e)
        {
            rdb_SourceRegisterComparison.IsChecked = true;
        }

        private void txtDestinationValueComparison_GotFocus(object sender, RoutedEventArgs e)
        {
            rdb_DestinationValueControlTransfer.IsChecked = true;
        }

        private void cmb_DestinationRegisterComparison_GotFocus(object sender, RoutedEventArgs e)
        {
            rdb_DestinationRegisterComparison.IsChecked = true;
        }

        private void txtSourceValueIO_GotFocus(object sender, RoutedEventArgs e)
        {
            rdb_SourceValueIO.IsChecked = true;
        }

        private void cmb_SourceRegisterIO_GotFocus(object sender, RoutedEventArgs e)
        {
            rdb_SourceRegisterIO.IsChecked = true;
        }

        private void txtDestinationValueIO_GotFocus(object sender, RoutedEventArgs e)
        {
            rdb_DestinationValueIO.IsChecked = true;
        }

        private void cmb_DestinationRegisterIO_GotFocus(object sender, RoutedEventArgs e)
        {
            rdb_DestinationRegisterIO.IsChecked = true;
        }

        private void txtSourceValueMiscellaneous_GotFocus(object sender, RoutedEventArgs e)
        {
            rdb_SourceValueMiscellaneous.IsChecked = true;
        }

        private void cmb_SourceRegisterMiscellaneous_GotFocus(object sender, RoutedEventArgs e)
        {
            rdb_SourceRegisterMiscellaneous.IsChecked = true;
        }

        private void txtDestinationValueMiscellaneous_GotFocus(object sender, RoutedEventArgs e)
        {
            rdb_DestinationValueMiscellaneous.IsChecked = true;
        }

        private void cmb_DestinationRegisterMiscellaneous_GotFocus(object sender, RoutedEventArgs e)
        {
            rdb_DestinationRegisterMiscellaneous.IsChecked = true;
        }
        #endregion
    }
}
