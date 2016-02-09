using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using CPU_OS_Simulator.CPU;

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

        private List<int> NumberOfOperands = new List<int>();

        /// <summary>
        /// The window that owns this window
        /// </summary>
        private MainWindow owner;

        /// <summary>
        /// How We should add the instruction to the program
        /// </summary>
        private EnumInstructionMode instructionMode;

        /// <summary>
        /// Array of function pointers to point to the instruction creation functions
        /// used when creating instructions
        /// </summary>
        private Func<int>[] InstructionCreationFunctions = new Func<int>[7];

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
        public InstructionsWindow(MainWindow owner) : this()
        {
            //InitializeComponent();
            this.owner = owner;
        }
        /// <summary>
        /// Constructor for instruction window that takes the window instance that is creating this window
        /// PLEASE NOTE: This constructor should always be used so data can be passed back to the main window
        /// and the correct instruction mode if necessary 
        /// </summary>
        /// <param name="owner">The window that is creating this window </param>
        /// <param name="instructionMode"> the selected instruction mode</param>
        public InstructionsWindow(MainWindow owner, EnumInstructionMode instructionMode) : this(owner)
        {
            this.instructionMode = instructionMode;
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
                string fulldescription = value.DescriptionAttr();
                string opcode = value.ToString();
                string[] split = fulldescription.Split(':'); // split the category from the description
                string category = split[0];
                string description = split[1];
                instructionDescriptions.Add(description); // add the description to the list

                switch (category)
                {
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
                            lst_OpcodeListArithmetic.Items.Add(opcode); //populate arithmetic instructions
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
            Close();
        }

        /// <summary>
        /// This event handler is called when the window first loads
        /// </summary>
        /// <param name="sender"> the object that fired this event</param>
        /// <param name="e"> the eventargs associated with this event</param>
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            InstructionCreationFunctions[0] = () => CreateDataTransferInstruction();
            InstructionCreationFunctions[1] = () => CreateLogicalInstruction();
            InstructionCreationFunctions[2] = () => CreateArithmeticInstruction();
            InstructionCreationFunctions[3] = () => CreateControlTransferInstruction();
            InstructionCreationFunctions[4] = () => CreateComparisonInstruction();
            InstructionCreationFunctions[5] = () => CreateIOInstruction();
            InstructionCreationFunctions[6] = () => CreateMiscellaneousInstruction();
            InstructionTabs.SelectedItem = DataTransferTab;
            PopulateInstructions();
        }

        #region Populate Instruction Descriptions

        private void lst_OpcodeListDataTransfer_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            EnumOpcodes selected = (EnumOpcodes)Enum.Parse(typeof(EnumOpcodes), lst_OpcodeListDataTransfer.SelectedItem.ToString());
            txtDescriptionDataTransfer.Text = instructionDescriptions.ElementAt((int)selected);
            int operands = selected.NumberOfOperandsAttr();
            if (operands == 0)
            {
                rdb_SourceValueDataTransfer.IsEnabled = false;
                txtSourceValueDataTransfer.IsEnabled = false;
                rdb_SourceRegisterDataTransfer.IsEnabled = false;
                cmb_SourceRegisterDataTransfer.IsEnabled = false;
                rdb_DestinationValueDataTransfer.IsEnabled = false;
                txtDestinationValueDataTransfer.IsEnabled = false;
                rdb_DestinationRegisterDataTransfer.IsEnabled = false;
                cmb_DestinationRegisterDataTransfer.IsEnabled = false;
            }
            else if (operands == 1)
            {
                rdb_DestinationValueDataTransfer.IsEnabled = false;
                txtDestinationValueDataTransfer.IsEnabled = false;
                rdb_DestinationRegisterDataTransfer.IsEnabled = false;
                cmb_DestinationRegisterDataTransfer.IsEnabled = false;
            }
        }

        private void lst_OpcodeListLogical_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            EnumOpcodes selected = (EnumOpcodes)Enum.Parse(typeof(EnumOpcodes), lst_OpcodeListLogical.SelectedItem.ToString());
            txtDescriptionLogical.Text = instructionDescriptions.ElementAt((int)selected);
            int operands = selected.NumberOfOperandsAttr();
            if (operands == 0)
            {
                rdb_SourceValueLogical.IsEnabled = false;
                txtSourceValueLogical.IsEnabled = false;
                rdb_SourceRegisterLogical.IsEnabled = false;
                cmb_SourceRegisterLogical.IsEnabled = false;
                rdb_DestinationValueLogical.IsEnabled = false;
                txtDestinationValueLogical.IsEnabled = false;
                rdb_DestinationRegisterLogical.IsEnabled = false;
                cmb_DestinationRegisterLogical.IsEnabled = false;
            }
            else if (operands == 1)
            {
                rdb_DestinationValueLogical.IsEnabled = false;
                txtDestinationValueLogical.IsEnabled = false;
                rdb_DestinationRegisterLogical.IsEnabled = false;
                cmb_DestinationRegisterLogical.IsEnabled = false;
            }
        }

        private void lst_OpcodeListArithmetic_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            EnumOpcodes selected = (EnumOpcodes)Enum.Parse(typeof(EnumOpcodes), lst_OpcodeListArithmetic.SelectedItem.ToString());
            txtDescriptionArithmetic.Text = instructionDescriptions.ElementAt((int)selected);
            int operands = selected.NumberOfOperandsAttr();
            if (operands == 0)
            {
                rdb_SourceValueArithmetic.IsEnabled = false;
                txtSourceValueArithmetic.IsEnabled = false;
                rdb_SourceRegisterArithmetic.IsEnabled = false;
                cmb_SourceRegisterArithmetic.IsEnabled = false;
                rdb_DestinationValueArithmetic.IsEnabled = false;
                txtDestinationValueArithmetic.IsEnabled = false;
                rdb_DestinationRegisterArithmetic.IsEnabled = false;
                cmb_DestinationRegisterArithmetic.IsEnabled = false;
            }
            else if (operands == 1)
            {
                rdb_DestinationValueArithmetic.IsEnabled = false;
                txtDestinationValueArithmetic.IsEnabled = false;
                rdb_DestinationRegisterArithmetic.IsEnabled = false;
                cmb_DestinationRegisterArithmetic.IsEnabled = false;
            }
        }

        private void lst_OpcodeListControlTransfer_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            EnumOpcodes selected = (EnumOpcodes)Enum.Parse(typeof(EnumOpcodes), lst_OpcodeListControlTransfer.SelectedItem.ToString());
            txtDescriptionControlTransfer.Text = instructionDescriptions.ElementAt((int)selected);
            int operands = selected.NumberOfOperandsAttr();
            if (operands == 0)
            {
                rdb_SourceValueControlTransfer.IsEnabled = false;
                txtSourceValueControlTransfer.IsEnabled = false;
                rdb_SourceRegisterControlTransfer.IsEnabled = false;
                cmb_SourceRegisterControlTransfer.IsEnabled = false;
                rdb_DestinationValueControlTransfer.IsEnabled = false;
                txtDestinationValueControlTransfer.IsEnabled = false;
                rdb_DestinationRegisterControlTransfer.IsEnabled = false;
                cmb_DestinationRegisterControlTransfer.IsEnabled = false;
            }
            else if (operands == 1)
            {
                rdb_DestinationValueControlTransfer.IsEnabled = false;
                txtDestinationValueControlTransfer.IsEnabled = false;
                rdb_DestinationRegisterControlTransfer.IsEnabled = false;
                cmb_DestinationRegisterControlTransfer.IsEnabled = false;
            }
        }

        private void lst_OpcodeListComparison_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            EnumOpcodes selected = (EnumOpcodes)Enum.Parse(typeof(EnumOpcodes), lst_OpcodeListComparison.SelectedItem.ToString());
            txtDescriptionComparison.Text = instructionDescriptions.ElementAt((int)selected);
            int operands = selected.NumberOfOperandsAttr();
            if (operands == 0)
            {
                rdb_SourceValueComparison.IsEnabled = false;
                txtSourceValueComparison.IsEnabled = false;
                rdb_SourceRegisterComparison.IsEnabled = false;
                cmb_SourceRegisterComparison.IsEnabled = false;
                rdb_DestinationValueComparison.IsEnabled = false;
                txtDestinationValueComparison.IsEnabled = false;
                rdb_DestinationRegisterComparison.IsEnabled = false;
                cmb_DestinationRegisterComparison.IsEnabled = false;
            }
            else if (operands == 1)
            {
                rdb_DestinationValueComparison.IsEnabled = false;
                txtDestinationValueComparison.IsEnabled = false;
                rdb_DestinationRegisterComparison.IsEnabled = false;
                cmb_DestinationRegisterComparison.IsEnabled = false;
            }
        }

        private void lst_OpcodeListIO_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            EnumOpcodes selected = (EnumOpcodes)Enum.Parse(typeof(EnumOpcodes), lst_OpcodeListIO.SelectedItem.ToString());
            txtDescriptionIO.Text = instructionDescriptions.ElementAt((int)selected);
            int operands = selected.NumberOfOperandsAttr();
            if (operands == 0)
            {
                rdb_SourceValueIO.IsEnabled = false;
                txtSourceValueIO.IsEnabled = false;
                rdb_SourceRegisterIO.IsEnabled = false;
                cmb_SourceRegisterIO.IsEnabled = false;
                rdb_DestinationValueIO.IsEnabled = false;
                txtDestinationValueIO.IsEnabled = false;
                rdb_DestinationRegisterIO.IsEnabled = false;
                cmb_DestinationRegisterIO.IsEnabled = false;
            }
            else if (operands == 1)
            {
                rdb_DestinationValueIO.IsEnabled = false;
                txtDestinationValueIO.IsEnabled = false;
                rdb_DestinationRegisterIO.IsEnabled = false;
                cmb_DestinationRegisterIO.IsEnabled = false;
            }
        }

        private void lst_OpcodeListMiscellaneous_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            EnumOpcodes selected = (EnumOpcodes)Enum.Parse(typeof(EnumOpcodes), lst_OpcodeListMiscellaneous.SelectedItem.ToString());
            txtDescriptionMiscellaneous.Text = instructionDescriptions.ElementAt((int)selected);
            int operands = selected.NumberOfOperandsAttr();
            if (operands == 0)
            {
                rdb_SourceValueMiscellaneous.IsEnabled = false;
                txtSourceValueMiscellaneous.IsEnabled = false;
                rdb_SourceRegisterMiscellaneous.IsEnabled = false;
                cmb_SourceRegisterMiscellaneous.IsEnabled = false;
                rdb_DestinationValueMiscellaneous.IsEnabled = false;
                txtDestinationValueMiscellaneous.IsEnabled = false;
                rdb_DestinationRegisterMiscellaneous.IsEnabled = false;
                cmb_DestinationRegisterMiscellaneous.IsEnabled = false;
            }
            else if (operands == 1)
            {
                rdb_DestinationValueMiscellaneous.IsEnabled = false;
                txtDestinationValueMiscellaneous.IsEnabled = false;
                rdb_DestinationRegisterMiscellaneous.IsEnabled = false;
                cmb_DestinationRegisterMiscellaneous.IsEnabled = false;
            }
        }

        #endregion Populate Instruction Descriptions

        /// <summary>
        /// This event handler is called when the window is closing
        /// </summary>
        /// <param name="sender"> the object that fired this event</param>
        /// <param name="e"> the eventargs associated with this event</param>
        private void InstructionsWindow1_Closing(object sender, CancelEventArgs e)
        {
            System.Console.WriteLine("Instruction Window closing");
        }

        #region Instruction Creation Functions

        private int CreateDataTransferInstruction()
        {
            TabItem SelectedTab = (TabItem)InstructionTabs.SelectedItem;
            EnumOpcodes opcode;
            Operand op1;
            Operand op2;
            EnumAddressType op1mem = EnumAddressType.UNKNOWN;
            EnumAddressType op2mem = EnumAddressType.UNKNOWN;
            int index = owner.lst_InstructionsList.SelectedIndex;

            SimulatorProgram prog = (SimulatorProgram)owner.lst_ProgramList.SelectedItem;
            opcode = (EnumOpcodes)Enum.Parse(typeof(EnumOpcodes), lst_OpcodeListDataTransfer.SelectedItem.ToString());

            if (rdb_SourceValueDataTransfer.IsChecked != null && rdb_SourceValueDataTransfer.IsChecked.Value)
            {
                if (rdb_DataTransfer_Source_Direct_Mem.IsChecked != null && rdb_DataTransfer_Source_Direct_Mem.IsChecked.Value)
                {
                    op1mem = EnumAddressType.DIRECT;
                }
                else if (rdb_DataTransfer_Source_InDirect_Mem.IsChecked != null && rdb_DataTransfer_Source_InDirect_Mem.IsChecked.Value)
                {
                    op1mem = EnumAddressType.INDIRECT;
                }

                if (op1mem == EnumAddressType.DIRECT || op1mem == EnumAddressType.INDIRECT)
                {
                    op1 = new Operand(Convert.ToInt32(txtSourceValueDataTransfer.Text), EnumOperandType.ADDRESS);
                }
                else
                {
                    op1 = new Operand(Convert.ToInt32(txtSourceValueDataTransfer.Text), EnumOperandType.VALUE);
                }
            }
            else if (rdb_SourceRegisterDataTransfer.IsChecked != null && rdb_SourceRegisterDataTransfer.IsChecked.Value)
            {
                string selectedRegister = (string)cmb_SourceRegisterDataTransfer.SelectedItem;
                Register reg = Register.FindRegister(selectedRegister);
                if (rdb_DataTransfer_Source_Reg_Direct.IsChecked != null && rdb_DataTransfer_Source_Reg_Direct.IsChecked.Value)
                {
                    op1mem = EnumAddressType.DIRECT;
                }
                else if (rdb_DataTransfer_Source_Reg_Indirect.IsChecked != null && rdb_DataTransfer_Source_Reg_Indirect.IsChecked.Value)
                {
                    op1mem = EnumAddressType.INDIRECT;
                }
                if (op1mem == EnumAddressType.DIRECT || op1mem == EnumAddressType.INDIRECT)
                {
                    op1 = new Operand(reg, EnumOperandType.ADDRESS);
                }
                else
                {
                    op1 = new Operand(reg, EnumOperandType.VALUE);
                }
            }
            else
            {
                op1 = null;
            }
            if (rdb_DestinationValueDataTransfer.IsChecked != null && rdb_DestinationValueDataTransfer.IsChecked.Value)
            {
                if (rdb_DataTransfer_Destination_Direct_Mem.IsChecked != null && rdb_DataTransfer_Destination_Direct_Mem.IsChecked.Value)
                {
                    op2mem = EnumAddressType.DIRECT;
                }
                else if (rdb_DataTransfer_Destination_InDirect_Mem.IsChecked != null && rdb_DataTransfer_Destination_InDirect_Mem.IsChecked.Value)
                {
                    op2mem = EnumAddressType.INDIRECT;
                }
                if (op2mem == EnumAddressType.DIRECT || op2mem == EnumAddressType.INDIRECT)
                {
                    op2 = new Operand(Convert.ToInt32(txtDestinationValueDataTransfer.Text), EnumOperandType.ADDRESS);
                }
                else
                {
                    op2 = new Operand(Convert.ToInt32(txtDestinationValueDataTransfer.Text), EnumOperandType.VALUE);
                }
                
            }
            else if (rdb_DestinationRegisterDataTransfer.IsChecked != null && rdb_DestinationRegisterDataTransfer.IsChecked.Value)
            {
                string selectedRegister = (string)cmb_DestinationRegisterDataTransfer.SelectedItem;
                Register reg = Register.FindRegister(selectedRegister);
                if (rdb_DataTransfer_Destination_Reg_Direct.IsChecked != null && rdb_DataTransfer_Destination_Reg_Direct.IsChecked.Value)
                {
                    op2mem = EnumAddressType.DIRECT;
                }
                else if (rdb_DataTransfer_Destination_Reg_Indirect.IsChecked != null && rdb_DataTransfer_Destination_Reg_Indirect.IsChecked.Value)
                {
                    op2mem = EnumAddressType.INDIRECT;
                }
                if (op2mem == EnumAddressType.DIRECT || op2mem == EnumAddressType.INDIRECT)
                {
                    op2 = new Operand(reg, EnumOperandType.ADDRESS);
                }
                else
                {
                    op2 = new Operand(reg, EnumOperandType.VALUE);
                }
            }
            else
            {
                op2 = null;
            }
            Instruction i = owner.CreateInstruction(opcode, op1, op1mem, op2, op2mem, 4);
            owner.AddInstruction(i, index);
            return 0;
        }

        private int CreateLogicalInstruction()
        {
            TabItem SelectedTab = (TabItem)InstructionTabs.SelectedItem;
            EnumOpcodes opcode;
            Operand op1;
            Operand op2;
            EnumAddressType op1mem = EnumAddressType.UNKNOWN;
            EnumAddressType op2mem = EnumAddressType.UNKNOWN;
            int index = owner.lst_InstructionsList.SelectedIndex;

            SimulatorProgram prog = (SimulatorProgram)owner.lst_ProgramList.SelectedItem;
            opcode = (EnumOpcodes)Enum.Parse(typeof(EnumOpcodes), lst_OpcodeListLogical.SelectedItem.ToString());

            if (rdb_SourceValueLogical.IsChecked != null && rdb_SourceValueLogical.IsChecked.Value)
            {
                if (rdb_Logical_Source_Direct_Mem.IsChecked != null && rdb_Logical_Source_Direct_Mem.IsChecked.Value)
                {
                    op1mem = EnumAddressType.DIRECT;
                }
                else if (rdb_Logical_Source_InDirect_Mem.IsChecked != null && rdb_Logical_Source_InDirect_Mem.IsChecked.Value)
                {
                    op1mem = EnumAddressType.INDIRECT;
                }

                if (op1mem == EnumAddressType.DIRECT || op1mem == EnumAddressType.INDIRECT)
                {
                    op1 = new Operand(Convert.ToInt32(txtSourceValueLogical.Text), EnumOperandType.ADDRESS);
                }
                else
                {
                    op1 = new Operand(Convert.ToInt32(txtSourceValueLogical.Text), EnumOperandType.VALUE);
                }
            }
            else if (rdb_SourceRegisterLogical.IsChecked != null && rdb_SourceRegisterLogical.IsChecked.Value)
            {
                string selectedRegister = (string)cmb_SourceRegisterLogical.SelectedItem;
                Register reg = Register.FindRegister(selectedRegister);
                if (rdb_Logical_Source_Reg_Direct.IsChecked != null && rdb_Logical_Source_Reg_Direct.IsChecked.Value)
                {
                    op1mem = EnumAddressType.DIRECT;
                }
                else if (rdb_Logical_Source_Reg_Indirect.IsChecked != null && rdb_Logical_Source_Reg_Indirect.IsChecked.Value)
                {
                    op1mem = EnumAddressType.INDIRECT;
                }
                if (op1mem == EnumAddressType.DIRECT || op1mem == EnumAddressType.INDIRECT)
                {
                    op1 = new Operand(reg, EnumOperandType.ADDRESS);
                }
                else
                {
                    op1 = new Operand(reg, EnumOperandType.VALUE);
                }
            }
            else
            {
                op1 = null;
            }
            if (rdb_DestinationValueLogical.IsChecked != null && rdb_DestinationValueLogical.IsChecked.Value)
            {
                if (rdb_Logical_Destination_Direct_Mem.IsChecked != null && rdb_Logical_Destination_Direct_Mem.IsChecked.Value)
                {
                    op2mem = EnumAddressType.DIRECT;
                }
                else if (rdb_Logical_Destination_InDirect_Mem.IsChecked != null && rdb_Logical_Destination_InDirect_Mem.IsChecked.Value)
                {
                    op2mem = EnumAddressType.INDIRECT;
                }
                if (op2mem == EnumAddressType.DIRECT || op2mem == EnumAddressType.INDIRECT)
                {
                    op2 = new Operand(Convert.ToInt32(txtDestinationValueLogical.Text), EnumOperandType.ADDRESS);
                }
                else
                {
                    op2 = new Operand(Convert.ToInt32(txtDestinationValueLogical.Text), EnumOperandType.VALUE);
                }

            }
            else if (rdb_DestinationRegisterLogical.IsChecked != null && rdb_DestinationRegisterLogical.IsChecked.Value)
            {
                string selectedRegister = (string)cmb_DestinationRegisterLogical.SelectedItem;
                Register reg = Register.FindRegister(selectedRegister);
                if (rdb_Logical_Destination_Reg_Direct.IsChecked != null && rdb_Logical_Destination_Reg_Direct.IsChecked.Value)
                {
                    op2mem = EnumAddressType.DIRECT;
                }
                else if (rdb_Logical_Destination_Reg_Indirect.IsChecked != null && rdb_Logical_Destination_Reg_Indirect.IsChecked.Value)
                {
                    op2mem = EnumAddressType.INDIRECT;
                }
                if (op2mem == EnumAddressType.DIRECT || op2mem == EnumAddressType.INDIRECT)
                {
                    op2 = new Operand(reg, EnumOperandType.ADDRESS);
                }
                else
                {
                    op2 = new Operand(reg, EnumOperandType.VALUE);
                }
            }
            else
            {
                op2 = null;
            }
            Instruction i = owner.CreateInstruction(opcode, op1, op1mem, op2, op2mem, 4);
            owner.AddInstruction(i, index);
            return 0;
        }

        private int CreateArithmeticInstruction()
        {
            TabItem SelectedTab = (TabItem)InstructionTabs.SelectedItem;
            EnumOpcodes opcode;
            Operand op1;
            Operand op2;
            EnumAddressType op1mem = EnumAddressType.UNKNOWN;
            EnumAddressType op2mem = EnumAddressType.UNKNOWN;
            int index = owner.lst_InstructionsList.SelectedIndex;

            SimulatorProgram prog = (SimulatorProgram)owner.lst_ProgramList.SelectedItem;
            opcode = (EnumOpcodes)Enum.Parse(typeof(EnumOpcodes), lst_OpcodeListArithmetic.SelectedItem.ToString());

            if (rdb_SourceValueArithmetic.IsChecked != null && rdb_SourceValueArithmetic.IsChecked.Value)
            {
                if (rdb_Arithmetic_Source_Direct_Mem.IsChecked != null && rdb_Arithmetic_Source_Direct_Mem.IsChecked.Value)
                {
                    op1mem = EnumAddressType.DIRECT;
                }
                else if (rdb_Arithmetic_Source_InDirect_Mem.IsChecked != null && rdb_Arithmetic_Source_InDirect_Mem.IsChecked.Value)
                {
                    op1mem = EnumAddressType.INDIRECT;
                }

                if (op1mem == EnumAddressType.DIRECT || op1mem == EnumAddressType.INDIRECT)
                {
                    op1 = new Operand(Convert.ToInt32(txtSourceValueArithmetic.Text), EnumOperandType.ADDRESS);
                }
                else
                {
                    op1 = new Operand(Convert.ToInt32(txtSourceValueArithmetic.Text), EnumOperandType.VALUE);
                }
            }
            else if (rdb_SourceRegisterArithmetic.IsChecked != null && rdb_SourceRegisterArithmetic.IsChecked.Value)
            {
                string selectedRegister = (string)cmb_SourceRegisterArithmetic.SelectedItem;
                Register reg = Register.FindRegister(selectedRegister);
                if (rdb_Arithmetic_Source_Reg_Direct.IsChecked != null && rdb_Arithmetic_Source_Reg_Direct.IsChecked.Value)
                {
                    op1mem = EnumAddressType.DIRECT;
                }
                else if (rdb_Arithmetic_Source_Reg_Indirect.IsChecked != null && rdb_Arithmetic_Source_Reg_Indirect.IsChecked.Value)
                {
                    op1mem = EnumAddressType.INDIRECT;
                }
                if (op1mem == EnumAddressType.DIRECT || op1mem == EnumAddressType.INDIRECT)
                {
                    op1 = new Operand(reg, EnumOperandType.ADDRESS);
                }
                else
                {
                    op1 = new Operand(reg, EnumOperandType.VALUE);
                }
            }
            else
            {
                op1 = null;
            }
            if (rdb_DestinationValueArithmetic.IsChecked != null && rdb_DestinationValueArithmetic.IsChecked.Value)
            {
                if (rdb_Arithmetic_Destination_Direct_Mem.IsChecked != null && rdb_Arithmetic_Destination_Direct_Mem.IsChecked.Value)
                {
                    op2mem = EnumAddressType.DIRECT;
                }
                else if (rdb_Arithmetic_Destination_InDirect_Mem.IsChecked != null && rdb_Arithmetic_Destination_InDirect_Mem.IsChecked.Value)
                {
                    op2mem = EnumAddressType.INDIRECT;
                }
                if (op2mem == EnumAddressType.DIRECT || op2mem == EnumAddressType.INDIRECT)
                {
                    op2 = new Operand(Convert.ToInt32(txtDestinationValueArithmetic.Text), EnumOperandType.ADDRESS);
                }
                else
                {
                    op2 = new Operand(Convert.ToInt32(txtDestinationValueArithmetic.Text), EnumOperandType.VALUE);
                }

            }
            else if (rdb_DestinationRegisterArithmetic.IsChecked != null && rdb_DestinationRegisterArithmetic.IsChecked.Value)
            {
                string selectedRegister = (string)cmb_DestinationRegisterArithmetic.SelectedItem;
                Register reg = Register.FindRegister(selectedRegister);
                if (rdb_Arithmetic_Destination_Reg_Direct.IsChecked != null && rdb_Arithmetic_Destination_Reg_Direct.IsChecked.Value)
                {
                    op2mem = EnumAddressType.DIRECT;
                }
                else if (rdb_Arithmetic_Destination_Reg_Indirect.IsChecked != null && rdb_Arithmetic_Destination_Reg_Indirect.IsChecked.Value)
                {
                    op2mem = EnumAddressType.INDIRECT;
                }
                if (op2mem == EnumAddressType.DIRECT || op2mem == EnumAddressType.INDIRECT)
                {
                    op2 = new Operand(reg, EnumOperandType.ADDRESS);
                }
                else
                {
                    op2 = new Operand(reg, EnumOperandType.VALUE);
                }
            }
            else
            {
                op2 = null;
            }
            Instruction i = owner.CreateInstruction(opcode, op1, op1mem, op2, op2mem, 4);
            owner.AddInstruction(i, index);
            return 0;
        }

        private int CreateControlTransferInstruction()
        {
            TabItem SelectedTab = (TabItem)InstructionTabs.SelectedItem;
            EnumOpcodes opcode;
            Operand op1;
            Operand op2;
            EnumAddressType op1mem = EnumAddressType.UNKNOWN;
            EnumAddressType op2mem = EnumAddressType.UNKNOWN;
            int index = owner.lst_InstructionsList.SelectedIndex;

            SimulatorProgram prog = (SimulatorProgram)owner.lst_ProgramList.SelectedItem;
            opcode = (EnumOpcodes)Enum.Parse(typeof(EnumOpcodes), lst_OpcodeListControlTransfer.SelectedItem.ToString());

            if (rdb_SourceValueControlTransfer.IsChecked != null && rdb_SourceValueControlTransfer.IsChecked.Value)
            {
                if (rdb_ControlTransfer_Source_Direct_Mem.IsChecked != null && rdb_ControlTransfer_Source_Direct_Mem.IsChecked.Value)
                {
                    op1mem = EnumAddressType.DIRECT;
                }
                else if (rdb_ControlTransfer_Source_InDirect_Mem.IsChecked != null && rdb_ControlTransfer_Source_InDirect_Mem.IsChecked.Value)
                {
                    op1mem = EnumAddressType.INDIRECT;
                }

                if (op1mem == EnumAddressType.DIRECT || op1mem == EnumAddressType.INDIRECT)
                {
                    op1 = new Operand(Convert.ToInt32(txtSourceValueControlTransfer.Text), EnumOperandType.ADDRESS);
                }
                else
                {
                    op1 = new Operand(Convert.ToInt32(txtSourceValueControlTransfer.Text), EnumOperandType.VALUE);
                }
            }
            else if (rdb_SourceRegisterControlTransfer.IsChecked != null && rdb_SourceRegisterControlTransfer.IsChecked.Value)
            {
                string selectedRegister = (string)cmb_SourceRegisterControlTransfer.SelectedItem;
                Register reg = Register.FindRegister(selectedRegister);
                if (rdb_ControlTransfer_Source_Reg_Direct.IsChecked != null && rdb_ControlTransfer_Source_Reg_Direct.IsChecked.Value)
                {
                    op1mem = EnumAddressType.DIRECT;
                }
                else if (rdb_ControlTransfer_Source_Reg_Indirect.IsChecked != null && rdb_ControlTransfer_Source_Reg_Indirect.IsChecked.Value)
                {
                    op1mem = EnumAddressType.INDIRECT;
                }
                if (op1mem == EnumAddressType.DIRECT || op1mem == EnumAddressType.INDIRECT)
                {
                    op1 = new Operand(reg, EnumOperandType.ADDRESS);
                }
                else
                {
                    op1 = new Operand(reg, EnumOperandType.VALUE);
                }
            }
            else
            {
                op1 = null;
            }
            if (rdb_DestinationValueControlTransfer.IsChecked != null && rdb_DestinationValueControlTransfer.IsChecked.Value)
            {
                if (rdb_ControlTransfer_Destination_Direct_Mem.IsChecked != null && rdb_ControlTransfer_Destination_Direct_Mem.IsChecked.Value)
                {
                    op2mem = EnumAddressType.DIRECT;
                }
                else if (rdb_ControlTransfer_Destination_InDirect_Mem.IsChecked != null && rdb_ControlTransfer_Destination_InDirect_Mem.IsChecked.Value)
                {
                    op2mem = EnumAddressType.INDIRECT;
                }
                if (op2mem == EnumAddressType.DIRECT || op2mem == EnumAddressType.INDIRECT)
                {
                    op2 = new Operand(Convert.ToInt32(txtDestinationValueControlTransfer.Text), EnumOperandType.ADDRESS);
                }
                else
                {
                    op2 = new Operand(Convert.ToInt32(txtDestinationValueControlTransfer.Text), EnumOperandType.VALUE);
                }

            }
            else if (rdb_DestinationRegisterControlTransfer.IsChecked != null && rdb_DestinationRegisterControlTransfer.IsChecked.Value)
            {
                string selectedRegister = (string)cmb_DestinationRegisterControlTransfer.SelectedItem;
                Register reg = Register.FindRegister(selectedRegister);
                if (rdb_ControlTransfer_Destination_Reg_Direct.IsChecked != null && rdb_ControlTransfer_Destination_Reg_Direct.IsChecked.Value)
                {
                    op2mem = EnumAddressType.DIRECT;
                }
                else if (rdb_ControlTransfer_Destination_Reg_Indirect.IsChecked != null && rdb_ControlTransfer_Destination_Reg_Indirect.IsChecked.Value)
                {
                    op2mem = EnumAddressType.INDIRECT;
                }
                if (op2mem == EnumAddressType.DIRECT || op2mem == EnumAddressType.INDIRECT)
                {
                    op2 = new Operand(reg, EnumOperandType.ADDRESS);
                }
                else
                {
                    op2 = new Operand(reg, EnumOperandType.VALUE);
                }
            }
            else
            {
                op2 = null;
            }
            Instruction i = owner.CreateInstruction(opcode, op1, op1mem, op2, op2mem, 4);
            owner.AddInstruction(i, index);
            return 0;
        }

        private int CreateComparisonInstruction()
        {
            TabItem SelectedTab = (TabItem)InstructionTabs.SelectedItem;
            EnumOpcodes opcode;
            Operand op1;
            Operand op2;
            EnumAddressType op1mem = EnumAddressType.UNKNOWN;
            EnumAddressType op2mem = EnumAddressType.UNKNOWN;
            int index = owner.lst_InstructionsList.SelectedIndex;

            SimulatorProgram prog = (SimulatorProgram)owner.lst_ProgramList.SelectedItem;
            opcode = (EnumOpcodes)Enum.Parse(typeof(EnumOpcodes), lst_OpcodeListComparison.SelectedItem.ToString());

            if (rdb_SourceValueComparison.IsChecked != null && rdb_SourceValueComparison.IsChecked.Value)
            {
                if (rdb_Comparison_Source_Direct_Mem.IsChecked != null && rdb_Comparison_Source_Direct_Mem.IsChecked.Value)
                {
                    op1mem = EnumAddressType.DIRECT;
                }
                else if (rdb_Comparison_Source_InDirect_Mem.IsChecked != null && rdb_Comparison_Source_InDirect_Mem.IsChecked.Value)
                {
                    op1mem = EnumAddressType.INDIRECT;
                }

                if (op1mem == EnumAddressType.DIRECT || op1mem == EnumAddressType.INDIRECT)
                {
                    op1 = new Operand(Convert.ToInt32(txtSourceValueComparison.Text), EnumOperandType.ADDRESS);
                }
                else
                {
                    op1 = new Operand(Convert.ToInt32(txtSourceValueComparison.Text), EnumOperandType.VALUE);
                }
            }
            else if (rdb_SourceRegisterComparison.IsChecked != null && rdb_SourceRegisterComparison.IsChecked.Value)
            {
                string selectedRegister = (string)cmb_SourceRegisterComparison.SelectedItem;
                Register reg = Register.FindRegister(selectedRegister);
                if (rdb_Comparison_Source_Reg_Direct.IsChecked != null && rdb_Comparison_Source_Reg_Direct.IsChecked.Value)
                {
                    op1mem = EnumAddressType.DIRECT;
                }
                else if (rdb_Comparison_Source_Reg_Indirect.IsChecked != null && rdb_Comparison_Source_Reg_Indirect.IsChecked.Value)
                {
                    op1mem = EnumAddressType.INDIRECT;
                }
                if (op1mem == EnumAddressType.DIRECT || op1mem == EnumAddressType.INDIRECT)
                {
                    op1 = new Operand(reg, EnumOperandType.ADDRESS);
                }
                else
                {
                    op1 = new Operand(reg, EnumOperandType.VALUE);
                }
            }
            else
            {
                op1 = null;
            }
            if (rdb_DestinationValueComparison.IsChecked != null && rdb_DestinationValueComparison.IsChecked.Value)
            {
                if (rdb_Comparison_Destination_Direct_Mem.IsChecked != null && rdb_Comparison_Destination_Direct_Mem.IsChecked.Value)
                {
                    op2mem = EnumAddressType.DIRECT;
                }
                else if (rdb_Comparison_Destination_InDirect_Mem.IsChecked != null && rdb_Comparison_Destination_InDirect_Mem.IsChecked.Value)
                {
                    op2mem = EnumAddressType.INDIRECT;
                }
                if (op2mem == EnumAddressType.DIRECT || op2mem == EnumAddressType.INDIRECT)
                {
                    op2 = new Operand(Convert.ToInt32(txtDestinationValueComparison.Text), EnumOperandType.ADDRESS);
                }
                else
                {
                    op2 = new Operand(Convert.ToInt32(txtDestinationValueComparison.Text), EnumOperandType.VALUE);
                }

            }
            else if (rdb_DestinationRegisterComparison.IsChecked != null && rdb_DestinationRegisterComparison.IsChecked.Value)
            {
                string selectedRegister = (string)cmb_DestinationRegisterComparison.SelectedItem;
                Register reg = Register.FindRegister(selectedRegister);
                if (rdb_Comparison_Destination_Reg_Direct.IsChecked != null && rdb_Comparison_Destination_Reg_Direct.IsChecked.Value)
                {
                    op2mem = EnumAddressType.DIRECT;
                }
                else if (rdb_Comparison_Destination_Reg_Indirect.IsChecked != null && rdb_Comparison_Destination_Reg_Indirect.IsChecked.Value)
                {
                    op2mem = EnumAddressType.INDIRECT;
                }
                if (op2mem == EnumAddressType.DIRECT || op2mem == EnumAddressType.INDIRECT)
                {
                    op2 = new Operand(reg, EnumOperandType.ADDRESS);
                }
                else
                {
                    op2 = new Operand(reg, EnumOperandType.VALUE);
                }
            }
            else
            {
                op2 = null;
            }
            Instruction i = owner.CreateInstruction(opcode, op1, op1mem, op2, op2mem, 4);
            owner.AddInstruction(i, index);
            return 0;
        }

        private int CreateIOInstruction()
        {
            TabItem SelectedTab = (TabItem)InstructionTabs.SelectedItem;
            EnumOpcodes opcode;
            Operand op1;
            Operand op2;
            EnumAddressType op1mem = EnumAddressType.UNKNOWN;
            EnumAddressType op2mem = EnumAddressType.UNKNOWN;
            int index = owner.lst_InstructionsList.SelectedIndex;

            SimulatorProgram prog = (SimulatorProgram)owner.lst_ProgramList.SelectedItem;
            opcode = (EnumOpcodes)Enum.Parse(typeof(EnumOpcodes), lst_OpcodeListIO.SelectedItem.ToString());

            if (rdb_SourceValueIO.IsChecked != null && rdb_SourceValueIO.IsChecked.Value)
            {
                if (rdb_IO_Source_Direct_Mem.IsChecked != null && rdb_IO_Source_Direct_Mem.IsChecked.Value)
                {
                    op1mem = EnumAddressType.DIRECT;
                }
                else if (rdb_IO_Source_InDirect_Mem.IsChecked != null && rdb_IO_Source_InDirect_Mem.IsChecked.Value)
                {
                    op1mem = EnumAddressType.INDIRECT;
                }

                if (op1mem == EnumAddressType.DIRECT || op1mem == EnumAddressType.INDIRECT)
                {
                    op1 = new Operand(Convert.ToInt32(txtSourceValueIO.Text), EnumOperandType.ADDRESS);
                }
                else
                {
                    op1 = new Operand(Convert.ToInt32(txtSourceValueIO.Text), EnumOperandType.VALUE);
                }
            }
            else if (rdb_SourceRegisterIO.IsChecked != null && rdb_SourceRegisterIO.IsChecked.Value)
            {
                string selectedRegister = (string)cmb_SourceRegisterIO.SelectedItem;
                Register reg = Register.FindRegister(selectedRegister);
                if (rdb_IO_Source_Reg_Direct.IsChecked != null && rdb_IO_Source_Reg_Direct.IsChecked.Value)
                {
                    op1mem = EnumAddressType.DIRECT;
                }
                else if (rdb_IO_Source_Reg_Indirect.IsChecked != null && rdb_IO_Source_Reg_Indirect.IsChecked.Value)
                {
                    op1mem = EnumAddressType.INDIRECT;
                }
                if (op1mem == EnumAddressType.DIRECT || op1mem == EnumAddressType.INDIRECT)
                {
                    op1 = new Operand(reg, EnumOperandType.ADDRESS);
                }
                else
                {
                    op1 = new Operand(reg, EnumOperandType.VALUE);
                }
            }
            else
            {
                op1 = null;
            }
            if (rdb_DestinationValueIO.IsChecked != null && rdb_DestinationValueIO.IsChecked.Value)
            {
                if (rdb_IO_Destination_Direct_Mem.IsChecked != null && rdb_IO_Destination_Direct_Mem.IsChecked.Value)
                {
                    op2mem = EnumAddressType.DIRECT;
                }
                else if (rdb_IO_Destination_InDirect_Mem.IsChecked != null && rdb_IO_Destination_InDirect_Mem.IsChecked.Value)
                {
                    op2mem = EnumAddressType.INDIRECT;
                }
                if (op2mem == EnumAddressType.DIRECT || op2mem == EnumAddressType.INDIRECT)
                {
                    op2 = new Operand(Convert.ToInt32(txtDestinationValueIO.Text), EnumOperandType.ADDRESS);
                }
                else
                {
                    op2 = new Operand(Convert.ToInt32(txtDestinationValueIO.Text), EnumOperandType.VALUE);
                }

            }
            else if (rdb_DestinationRegisterIO.IsChecked != null && rdb_DestinationRegisterIO.IsChecked.Value)
            {
                string selectedRegister = (string)cmb_DestinationRegisterIO.SelectedItem;
                Register reg = Register.FindRegister(selectedRegister);
                if (rdb_IO_Destination_Reg_Direct.IsChecked != null && rdb_IO_Destination_Reg_Direct.IsChecked.Value)
                {
                    op2mem = EnumAddressType.DIRECT;
                }
                else if (rdb_IO_Destination_Reg_Indirect.IsChecked != null && rdb_IO_Destination_Reg_Indirect.IsChecked.Value)
                {
                    op2mem = EnumAddressType.INDIRECT;
                }
                if (op2mem == EnumAddressType.DIRECT || op2mem == EnumAddressType.INDIRECT)
                {
                    op2 = new Operand(reg, EnumOperandType.ADDRESS);
                }
                else
                {
                    op2 = new Operand(reg, EnumOperandType.VALUE);
                }
            }
            else
            {
                op2 = null;
            }
            Instruction i = owner.CreateInstruction(opcode, op1, op1mem, op2, op2mem, 4);
            owner.AddInstruction(i, index);
            return 0;
        }

        private int CreateMiscellaneousInstruction()
        {
            TabItem SelectedTab = (TabItem)InstructionTabs.SelectedItem;
            EnumOpcodes opcode;
            Operand op1;
            Operand op2;
            EnumAddressType op1mem = EnumAddressType.UNKNOWN;
            EnumAddressType op2mem = EnumAddressType.UNKNOWN;
            int index = owner.lst_InstructionsList.SelectedIndex;

            SimulatorProgram prog = (SimulatorProgram)owner.lst_ProgramList.SelectedItem;
            opcode = (EnumOpcodes)Enum.Parse(typeof(EnumOpcodes), lst_OpcodeListMiscellaneous.SelectedItem.ToString());

            if (rdb_SourceValueMiscellaneous.IsChecked != null && rdb_SourceValueMiscellaneous.IsChecked.Value)
            {
                if (rdb_Miscellaneous_Source_Direct_Mem.IsChecked != null && rdb_Miscellaneous_Source_Direct_Mem.IsChecked.Value)
                {
                    op1mem = EnumAddressType.DIRECT;
                }
                else if (rdb_Miscellaneous_Source_InDirect_Mem.IsChecked != null && rdb_Miscellaneous_Source_InDirect_Mem.IsChecked.Value)
                {
                    op1mem = EnumAddressType.INDIRECT;
                }

                if (op1mem == EnumAddressType.DIRECT || op1mem == EnumAddressType.INDIRECT)
                {
                    op1 = new Operand(Convert.ToInt32(txtSourceValueMiscellaneous.Text), EnumOperandType.ADDRESS);
                }
                else
                {
                    op1 = new Operand(Convert.ToInt32(txtSourceValueMiscellaneous.Text), EnumOperandType.VALUE);
                }
            }
            else if (rdb_SourceRegisterMiscellaneous.IsChecked != null && rdb_SourceRegisterMiscellaneous.IsChecked.Value)
            {
                string selectedRegister = (string)cmb_SourceRegisterMiscellaneous.SelectedItem;
                Register reg = Register.FindRegister(selectedRegister);
                if (rdb_Miscellaneous_Source_Reg_Direct.IsChecked != null && rdb_Miscellaneous_Source_Reg_Direct.IsChecked.Value)
                {
                    op1mem = EnumAddressType.DIRECT;
                }
                else if (rdb_Miscellaneous_Source_Reg_Indirect.IsChecked != null && rdb_Miscellaneous_Source_Reg_Indirect.IsChecked.Value)
                {
                    op1mem = EnumAddressType.INDIRECT;
                }
                if (op1mem == EnumAddressType.DIRECT || op1mem == EnumAddressType.INDIRECT)
                {
                    op1 = new Operand(reg, EnumOperandType.ADDRESS);
                }
                else
                {
                    op1 = new Operand(reg, EnumOperandType.VALUE);
                }
            }
            else
            {
                op1 = null;
            }
            if (rdb_DestinationValueMiscellaneous.IsChecked != null && rdb_DestinationValueMiscellaneous.IsChecked.Value)
            {
                if (rdb_Miscellaneous_Destination_Direct_Mem.IsChecked != null && rdb_Miscellaneous_Destination_Direct_Mem.IsChecked.Value)
                {
                    op2mem = EnumAddressType.DIRECT;
                }
                else if (rdb_Miscellaneous_Destination_InDirect_Mem.IsChecked != null && rdb_Miscellaneous_Destination_InDirect_Mem.IsChecked.Value)
                {
                    op2mem = EnumAddressType.INDIRECT;
                }
                if (op2mem == EnumAddressType.DIRECT || op2mem == EnumAddressType.INDIRECT)
                {
                    op2 = new Operand(Convert.ToInt32(txtDestinationValueMiscellaneous.Text), EnumOperandType.ADDRESS);
                }
                else
                {
                    op2 = new Operand(Convert.ToInt32(txtDestinationValueMiscellaneous.Text), EnumOperandType.VALUE);
                }

            }
            else if (rdb_DestinationRegisterMiscellaneous.IsChecked != null && rdb_DestinationRegisterMiscellaneous.IsChecked.Value)
            {
                string selectedRegister = (string)cmb_DestinationRegisterMiscellaneous.SelectedItem;
                Register reg = Register.FindRegister(selectedRegister);
                if (rdb_Miscellaneous_Destination_Reg_Direct.IsChecked != null && rdb_Miscellaneous_Destination_Reg_Direct.IsChecked.Value)
                {
                    op2mem = EnumAddressType.DIRECT;
                }
                else if (rdb_Miscellaneous_Destination_Reg_Indirect.IsChecked != null && rdb_Miscellaneous_Destination_Reg_Indirect.IsChecked.Value)
                {
                    op2mem = EnumAddressType.INDIRECT;
                }
                if (op2mem == EnumAddressType.DIRECT || op2mem == EnumAddressType.INDIRECT)
                {
                    op2 = new Operand(reg, EnumOperandType.ADDRESS);
                }
                else
                {
                    op2 = new Operand(reg, EnumOperandType.VALUE);
                }
            }
            else
            {
                op2 = null;
            }
            Instruction i = owner.CreateInstruction(opcode, op1, op1mem, op2, op2mem, 4);
            owner.AddInstruction(i, index);
            return 0;
        }

        #endregion Instruction Creation Functions

        /// <summary>
        /// Creates an instruction based on selected options
        /// </summary>
        private void CreateInstruction()
        {
            int SelectedTab = InstructionTabs.SelectedIndex;
            InstructionCreationFunctions[SelectedTab]();
        }

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
            rdb_DestinationRegisterLogical.IsChecked = true;
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

        #endregion UI control

        private void btn_NewInstruction_Click(object sender, RoutedEventArgs e)
        {
            if (!instructionMode.Equals(EnumInstructionMode.SHOW))
            {
                CreateInstruction();
            }
        }
    }
}