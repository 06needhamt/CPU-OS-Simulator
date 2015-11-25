using CPU_OS_Simulator.CPU;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

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
#if DEBUG
            owner.SayHello();
#endif
        }

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
                string fulldescription = Extentions.DescriptionAttr<EnumOpcodes>(value);
                string opcode = value.ToString();
                string[] split = fulldescription.Split(new char[] { ':' }); // split the category from the description
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
            txtDescriptionDataTransfer.Text = instructionDescriptions.ElementAt<string>((int)selected);
            int operands = Extentions.NumberOfOperandsAttr<EnumOpcodes>(selected);
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
            txtDescriptionLogical.Text = instructionDescriptions.ElementAt<string>((int)selected);
            int operands = Extentions.NumberOfOperandsAttr<EnumOpcodes>(selected);
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
            txtDescriptionArithmetic.Text = instructionDescriptions.ElementAt<string>((int)selected);
            int operands = Extentions.NumberOfOperandsAttr<EnumOpcodes>(selected);
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
            txtDescriptionControlTransfer.Text = instructionDescriptions.ElementAt<string>((int)selected);
            int operands = Extentions.NumberOfOperandsAttr<EnumOpcodes>(selected);
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
            txtDescriptionComparison.Text = instructionDescriptions.ElementAt<string>((int)selected);
            int operands = Extentions.NumberOfOperandsAttr<EnumOpcodes>(selected);
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
            txtDescriptionIO.Text = instructionDescriptions.ElementAt<string>((int)selected);
            int operands = Extentions.NumberOfOperandsAttr<EnumOpcodes>(selected);
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
            txtDescriptionMiscellaneous.Text = instructionDescriptions.ElementAt<string>((int)selected);
            int operands = Extentions.NumberOfOperandsAttr<EnumOpcodes>(selected);
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
            Console.WriteLine("Instruction Window closing");
            if (instructionMode.Equals(EnumInstructionMode.SHOW))
            {
                return;
            }
            else
            {
                CreateInstruction();
            }
        }

        #region Instruction Creation Functions

        private int CreateDataTransferInstruction()
        {
            TabItem SelectedTab = (TabItem)InstructionTabs.SelectedItem;
            EnumOpcodes opcode;
            Operand op1;
            Operand op2;
            int index = owner.lst_InstructionsList.SelectedIndex;

            SimulatorProgram prog = (SimulatorProgram)owner.lst_ProgramList.SelectedItem;
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
            owner.AddInstruction(i, index);
            return 0;
        }

        private int CreateLogicalInstruction()
        {
            TabItem SelectedTab = (TabItem)InstructionTabs.SelectedItem;
            EnumOpcodes opcode;
            Operand op1;
            Operand op2;
            int index = owner.lst_InstructionsList.SelectedIndex;

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
            owner.AddInstruction(i, index);
            return 0;
        }

        private int CreateArithmeticInstruction()
        {
            TabItem SelectedTab = (TabItem)InstructionTabs.SelectedItem;
            EnumOpcodes opcode;
            Operand op1;
            Operand op2;
            int index = owner.lst_InstructionsList.SelectedIndex;

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
            owner.AddInstruction(i, index);
            return 0;
        }

        private int CreateControlTransferInstruction()
        {
            TabItem SelectedTab = (TabItem)InstructionTabs.SelectedItem;
            EnumOpcodes opcode;
            Operand op1;
            Operand op2;
            int index = owner.lst_InstructionsList.SelectedIndex;

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
            owner.AddInstruction(i, index);
            return 0;
        }

        private int CreateComparisonInstruction()
        {
            TabItem SelectedTab = (TabItem)InstructionTabs.SelectedItem;
            EnumOpcodes opcode;
            Operand op1;
            Operand op2;
            int index = owner.lst_InstructionsList.SelectedIndex;

            opcode = (EnumOpcodes)Enum.Parse(typeof(EnumOpcodes), lst_OpcodeListComparison.SelectedItem.ToString());
            if (rdb_SourceValueComparison.IsChecked.Value)
            {
                op1 = new Operand(Convert.ToInt32(txtSourceValueComparison.Text), EnumOperandType.VALUE);
            }
            else if (rdb_SourceRegisterComparison.IsChecked.Value)
            {
                string selectedRegister = (string)cmb_SourceRegisterComparison.SelectedItem;
                Register reg = Register.FindRegister(selectedRegister);
                op1 = new Operand(reg, reg.Type);
            }
            else
            {
                op1 = null;
            }

            if (rdb_DestinationValueComparison.IsChecked.Value)
            {
                op2 = new Operand(Convert.ToInt32(txtDestinationValueComparison.Text), EnumOperandType.VALUE);
            }
            else if (rdb_DestinationRegisterComparison.IsChecked.Value)
            {
                string selectedRegister = (string)cmb_DestinationRegisterComparison.SelectedItem;
                Register reg = Register.FindRegister(selectedRegister);
                op2 = new Operand(reg, reg.Type);
            }
            else
            {
                op2 = null;
            }
            Instruction i = owner.CreateInstruction(opcode, op1, op2, 4);
            owner.AddInstruction(i, index);
            return 0;
        }

        private int CreateIOInstruction()
        {
            TabItem SelectedTab = (TabItem)InstructionTabs.SelectedItem;
            EnumOpcodes opcode;
            Operand op1;
            Operand op2;
            int index = owner.lst_InstructionsList.SelectedIndex;

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
            owner.AddInstruction(i, index);
            return 0;
        }

        private int CreateMiscellaneousInstruction()
        {
            TabItem SelectedTab = (TabItem)InstructionTabs.SelectedItem;
            EnumOpcodes opcode;
            Operand op1;
            Operand op2;
            int index = owner.lst_InstructionsList.SelectedIndex;

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

        #endregion UI control
    }
}