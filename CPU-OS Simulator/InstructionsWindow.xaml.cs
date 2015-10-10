﻿using System;
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
            owner.SayHello();
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
        /// <summary>
        /// This event handler is called when the window is closing
        /// </summary>
        /// <param name="sender"> the object that fired this event</param>
        /// <param name="e"> the eventargs associated with this event</param>
        private void InstructionsWindow1_Closing(object sender, CancelEventArgs e)
        {
            Console.WriteLine("Instruction Window closing");
            TabItem SelectedTab = (TabItem) InstructionTabs.SelectedItem;
            EnumOpcodes opcode;
            Operand op1;
            Operand op2;
            switch (SelectedTab.Header.ToString())
            {    
                case "Data Transfer":
                    {
                        opcode = (EnumOpcodes) Enum.Parse(typeof(EnumOpcodes),lst_OpcodeListDataTransfer.SelectedItem.ToString());
                        if (rdb_SourceValueDataTransfer.IsChecked.Value)
                        {
                            op1 = new Operand(Convert.ToInt32(txtSourceValueDataTransfer.Text),EnumOperandType.VALUE);
                        }
                        else if (rdb_SourceRegisterDataTransfer.IsChecked.Value)
                        {
                            string selectedRegister = (string)cmb_SourceRegisterDataTransfer.SelectedItem;
                            Register reg = FindRegister(selectedRegister);
                            op1 = new Operand(reg.Value,reg.Type);
                        }

                        if (rdb_DestinationValueDataTransfer.IsChecked.Value)
                        {
                            op2 = new Operand(Convert.ToInt32(txtDestinationValueDataTransfer.Text), EnumOperandType.VALUE);
                        }
                        else if (rdb_DestinationRegisterDataTransfer.IsChecked.Value)
                        {
                            string selectedRegister = (string)cmb_DestinationRegisterDataTransfer.SelectedItem;
                            Register reg = FindRegister(selectedRegister);
                            op2 = new Operand(reg.Value, reg.Type);
                        }
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
                            Register reg = FindRegister(selectedRegister);
                            op1 = new Operand(reg.Value, reg.Type);
                        }

                        if (rdb_DestinationValueLogical.IsChecked.Value)
                        {
                            op2 = new Operand(Convert.ToInt32(txtDestinationValueLogical.Text), EnumOperandType.VALUE);
                        }
                        else if (rdb_DestinationRegisterLogical.IsChecked.Value)
                        {
                            string selectedRegister = (string)cmb_DestinationRegisterLogical.SelectedItem;
                            Register reg = FindRegister(selectedRegister);
                            op2 = new Operand(reg.Value, reg.Type);
                        }
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
                            Register reg = FindRegister(selectedRegister);
                            op1 = new Operand(reg.Value, reg.Type);
                        }

                        if (rdb_DestinationValueArithmetic.IsChecked.Value)
                        {
                            op2 = new Operand(Convert.ToInt32(txtDestinationValueArithmetic.Text), EnumOperandType.VALUE);
                        }
                        else if (rdb_DestinationRegisterArithmetic.IsChecked.Value)
                        {
                            string selectedRegister = (string)cmb_DestinationRegisterArithmetic.SelectedItem;
                            Register reg = FindRegister(selectedRegister);
                            op2 = new Operand(reg.Value, reg.Type);
                        }
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
                            Register reg = FindRegister(selectedRegister);
                            op1 = new Operand(reg.Value, reg.Type);
                        }

                        if (rdb_DestinationValueControlTransfer.IsChecked.Value)
                        {
                            op2 = new Operand(Convert.ToInt32(txtDestinationValueControlTransfer.Text), EnumOperandType.VALUE);
                        }
                        else if (rdb_DestinationRegisterControlTransfer.IsChecked.Value)
                        {
                            string selectedRegister = (string)cmb_DestinationRegisterControlTransfer.SelectedItem;
                            Register reg = FindRegister(selectedRegister);
                            op2 = new Operand(reg.Value, reg.Type);
                        }
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
                            Register reg = FindRegister(selectedRegister);
                            op1 = new Operand(reg.Value, reg.Type);
                        }

                        if (rdb_DestinationValueIO.IsChecked.Value)
                        {
                            op2 = new Operand(Convert.ToInt32(txtDestinationValueIO.Text), EnumOperandType.VALUE);
                        }
                        else if (rdb_DestinationRegisterIO.IsChecked.Value)
                        {
                            string selectedRegister = (string)cmb_DestinationRegisterIO.SelectedItem;
                            Register reg = FindRegister(selectedRegister);
                            op2 = new Operand(reg.Value, reg.Type);
                        }
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
                            Register reg = FindRegister(selectedRegister);
                            op1 = new Operand(reg.Value, reg.Type);
                        }

                        if (rdb_DestinationValueMiscellaneous.IsChecked.Value)
                        {
                            op2 = new Operand(Convert.ToInt32(txtDestinationValueMiscellaneous.Text), EnumOperandType.VALUE);
                        }
                        else if (rdb_DestinationRegisterMiscellaneous.IsChecked.Value)
                        {
                            string selectedRegister = (string)cmb_DestinationRegisterMiscellaneous.SelectedItem;
                            Register reg = FindRegister(selectedRegister);
                            op2 = new Operand(reg.Value, reg.Type);
                        }
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
        private Register FindRegister(string selectedItem)
        {
            switch (selectedItem)
            {
                //TODO add other registers
                case "R00":
                    {
                        return Register.R00;
                        break;
                    }
                case "R01":
                    {
                        return Register.R01;
                    }
                case "R02":
                    {
                        return Register.R02;
                    }
                case "R03":
                    {
                        return Register.R03;
                    }
                case "R04":
                    {
                        return Register.R04;
                    }
                case "R05":
                    {
                        return Register.R05;
                    }
                case "R06":
                    {
                        return Register.R06;
                    }
                case "R07":
                    {
                        return Register.R07;
                    }
                case "R08":
                    {
                        return Register.R08;
                    }
                case "R09":
                    {
                        return Register.R09;
                    }
                case "R10":
                    {
                        return Register.R10;
                    }
                case "R11":
                    {
                        return Register.R11;
                    }
                case "R12":
                    {
                        return Register.R12;
                    }
                case "R13":
                    {
                        return Register.R13;
                    }
                case "R14":
                    {
                        return Register.R14;
                    }
                case "R15":
                    {
                        return Register.R15;
                    }
                case "R16":
                    {
                        return Register.R16;
                    }
                case "R17":
                    {
                        return Register.R17;
                    }
                case "R18":
                    {
                        return Register.R18;
                    }
                case "R19":
                    {
                        return Register.R19;
                    }
                case "R20":
                    {
                        return Register.R20;
                    }

                default:
                    {
                        return null;
                    }
            }
        }

        // TODO FIX Commented out due to strange null pointer when checking unchecking radio buttons

        private void rdb_SourceValueDataTransfer_Checked(object sender, RoutedEventArgs e)
        {
            //rdb_SourceRegisterDataTransfer.IsChecked = false;
            //cmb_SourceRegisterDataTransfer.IsEnabled = false;
            //txtSourceValueDataTransfer.IsEnabled = true;
        }

        private void rdb_SourceRegisterDataTransfer_Checked(object sender, RoutedEventArgs e)
        {
            //rdb_SourceValueDataTransfer.IsChecked = false;
            //cmb_SourceRegisterDataTransfer.IsEnabled = true;
            //txtSourceValueDataTransfer.IsEnabled = false;

        }

        private void rdb_DestinationValueDataTransfer_Checked(object sender, RoutedEventArgs e)
        {
            //rdb_DestinationRegisterDataTransfer.IsChecked = false;
            //cmb_DestinationRegisterDataTransfer.IsEnabled = false;
            //txtDestinationValueDataTransfer.IsEnabled = true;
        }

        private void rdb_DestinationRegisterDataTransfer_Checked(object sender, RoutedEventArgs e)
        {
            //rdb_DestinationValueDataTransfer.IsChecked = false;
            //cmb_DestinationRegisterDataTransfer.IsEnabled = true;
            //txtDestinationValueDataTransfer.IsEnabled = false;

        }

    }
}
