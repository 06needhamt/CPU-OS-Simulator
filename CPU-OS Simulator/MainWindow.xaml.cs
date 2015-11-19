using CPU_OS_Simulator.CPU;
using CPU_OS_Simulator.Memory;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.Web.Script.Serialization;
using System.Windows;
using System.Windows.Controls;

namespace CPU_OS_Simulator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    ///
    public partial class MainWindow : Window
    {
        #region Global Variables

        private List<SimulatorProgram> programList;
        private EnumInstructionMode instructionMode;
        public string currentProgram = string.Empty;
        private ExecutionUnit activeUnit;
        public static MainWindow currentInstance;

        #endregion Global Variables

        #region Properties

        public List<SimulatorProgram> ProgramList
        {
            get
            {
                return programList;
            }

            set
            {
                programList = value;
            }
        }

        public EnumInstructionMode InstructionMode
        {
            get
            {
                return instructionMode;
            }

            set
            {
                instructionMode = value;
            }
        }

        public ExecutionUnit ActiveUnit
        {
            get
            {
                return activeUnit;
            }

            set
            {
                activeUnit = value;
            }
        }

        public ExecutionUnit ActiveUnit1
        {
            get
            {
                return activeUnit;
            }

            set
            {
                activeUnit = value;
            }
        }

        #endregion Properties

        #region Constructors

        /// <summary>
        /// Constructor For the main window
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
            if (IsAdministrator())
            {
                string path = (new System.Uri(Assembly.GetExecutingAssembly().CodeBase)).AbsolutePath;
                SetAssociation(".sas", "Simulator Program File", path, "CPU-OS Simulator Program File");
            }
            programList = new List<SimulatorProgram>();
            PopulateRegisters();
            Console.WriteLine("Hello World");
            currentInstance = this;
        }

        #endregion Constructors

        #region Methods

        public static bool IsAdministrator()
        {
            var identity = WindowsIdentity.GetCurrent();
            var principal = new WindowsPrincipal(identity);
            return principal.IsInRole(WindowsBuiltInRole.Administrator);
        }

        [DllImport("shell32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern void SHChangeNotify(uint wEventId, uint uFlags, IntPtr dwItem1, IntPtr dwItem2);

        public static void SetAssociation(string Extension, string KeyName, string OpenWith, string FileDescription)
        {
            RegistryKey BaseKey;
            RegistryKey OpenMethod;
            RegistryKey Shell;
            RegistryKey CurrentUser;

            BaseKey = Registry.ClassesRoot.CreateSubKey(Extension);
            BaseKey.SetValue("", KeyName);

            OpenMethod = Registry.ClassesRoot.CreateSubKey(KeyName);
            OpenMethod.SetValue("", FileDescription);
            OpenMethod.CreateSubKey("DefaultIcon").SetValue("", "\"" + OpenWith + "\",0");
            Shell = OpenMethod.CreateSubKey("Shell");
            Shell.CreateSubKey("edit").CreateSubKey("command").SetValue("", "\"" + OpenWith + "\"" + " \"%1\"");
            Shell.CreateSubKey("open").CreateSubKey("command").SetValue("", "\"" + OpenWith + "\"" + " \"%1\"");
            BaseKey.Close();
            OpenMethod.Close();
            Shell.Close();

            CurrentUser = Registry.CurrentUser.OpenSubKey("Software\\Microsoft\\Windows\\CurrentVersion\\Explorer\\FileExts\\.sas", true);
            CurrentUser.DeleteSubKey("UserChoice", false);
            CurrentUser.Close();

            // Tell explorer the file association has been changed
            SHChangeNotify(0x08000000, 0x0000, IntPtr.Zero, IntPtr.Zero);
        }

        /// <summary>
        /// This function populates the register display in the main window
        /// </summary>
        private void PopulateRegisters()
        {
            for (int i = 0; i < 21; i++)
            {
                string registerString = "R";
                if (i < 10)
                {
                    registerString += "0" + i;
                }
                else
                {
                    registerString += i;
                }
                lst_Registers.Items.Add(Register.FindRegister(registerString));
            }
        }

        /// <summary>
        /// This function updates the register display in the main window whenever a register value is updated
        /// </summary>
        private void UpdateRegisters()
        {
            // HACK item source must be null before modifying the list
            lst_Registers.ItemsSource = null;
            lst_Registers.Items.Clear();
            PopulateRegisters();
        }
        /// <summary>
        /// This function updates the values in the UI for the special registers
        /// </summary>
        private void UpdateSpecialRegisters()
        {
            txt_BR.Text = SpecialRegister.FindSpecialRegister("BR").Value.ToString();
            txt_IR.Text = SpecialRegister.FindSpecialRegister("IR").ValueString.ToString();
            txt_MAR.Text = SpecialRegister.FindSpecialRegister("MAR").Value.ToString();
            txt_MDR.Text = SpecialRegister.FindSpecialRegister("MDR").Value.ToString();
            txt_PC.Text = SpecialRegister.FindSpecialRegister("PC").Value.ToString();
            txt_SP.Text = SpecialRegister.FindSpecialRegister("SP").Value.ToString();
            txt_SR.Text = SpecialRegister.FindSpecialRegister("SR").Value.ToString();
        }

        private void MainWindow2_Loaded(object sender, RoutedEventArgs e)
        {
            this.Title += " " + GetProgramVersion();
            SpecialRegister.FindSpecialRegister("BR").setRegisterValue(Convert.ToInt32(txt_BR.Text), EnumOperandType.VALUE);
            SpecialRegister.FindSpecialRegister("IR").setRegisterValue(txt_IR.Text, EnumOperandType.VALUE);
            SpecialRegister.FindSpecialRegister("MAR").setRegisterValue(Convert.ToInt32(txt_MAR.Text), EnumOperandType.VALUE);
            SpecialRegister.FindSpecialRegister("MDR").setRegisterValue(txt_MDR.Text, EnumOperandType.VALUE);
            SpecialRegister.FindSpecialRegister("PC").setRegisterValue(Convert.ToInt32(txt_PC.Text), EnumOperandType.VALUE);
            SpecialRegister.FindSpecialRegister("SP").setRegisterValue(Convert.ToInt32(txt_SP.Text), EnumOperandType.VALUE);
            SpecialRegister.FindSpecialRegister("SR").setRegisterValue(Convert.ToInt32(txt_SR.Text), EnumOperandType.VALUE);

#if DEBUG
            this.Title += " DEBUG BUILD ";
            MemoryPage_OLD m = new MemoryPage_OLD(0, 0, 255);
            for (int i = 0; i < m.Data.GetLength(0); i++)
            {
                for (int j = 0; j < m.Data.GetLength(1); j++)
                {
                    Console.WriteLine((int)m.Data[i, j]);
                }
            }
#endif
        }

        /// <summary>
        /// Gets the build number of the running program
        /// </summary>
        /// <returns> The build number of the running program</returns>
        private string GetProgramVersion()
        {
            Assembly ExecutingAssembly = Assembly.GetExecutingAssembly();
            FileVersionInfo VersionInfo = FileVersionInfo.GetVersionInfo(ExecutingAssembly.Location);
            return VersionInfo.FileVersion;
        }

        /// <summary>
        /// Creates and adds a program to the program list
        /// </summary>
        /// <param name="sender"> the clicked button </param>
        /// <param name="e">the event args</param>
        private void btn_ProgramAdd_Click(object sender, RoutedEventArgs e)
        {
            SimulatorProgram prog = CreateNewProgram();
            if (prog != null)
            {
                lst_ProgramList.Items.Add(prog);
                programList.Add(prog);
                currentProgram = prog.Name;
            }
        }

        /// <summary>
        /// Creates a new program based on entered data
        /// </summary>
        /// <returns>the created program</returns>
        private SimulatorProgram CreateNewProgram()
        {
            if (txtProgramName.Text == "")
            {
                MessageBox.Show("Please Enter a Program Name");
                return null;
            }
            else if (txtBaseAddress.Text == "")
            {
                MessageBox.Show("Please Enter a Base Address");
                return null;
            }
            string programName = txtProgramName.Text;
            Int32 baseAddress = Convert.ToInt32(txtBaseAddress.Text);
            Int32 pages = Convert.ToInt32(cmb_Pages.Text);
            SimulatorProgram program = new SimulatorProgram(programName, baseAddress, pages);
            //lst_ProgramList.Items.Add(program);
            //programList.Add(program);
            //currentProgram = program.Name;
            Console.WriteLine("Program " + program.Name + " Created");
            return program;
        }

        /// <summary>
        /// Called when the show button is clicked
        /// </summary>
        /// <param name="sender">the object that initiated the event</param>
        /// <param name="e"> the eventargs</param>
        private void btn_Show_Click(object sender, RoutedEventArgs e)
        {
            instructionMode = EnumInstructionMode.SHOW;
            InstructionsWindow iw = new InstructionsWindow(this, instructionMode);
            iw.Show();
        }

        /// <summary>
        /// Adds a new instruction to the program
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_AddNew_Click(object sender, RoutedEventArgs e)
        {
            instructionMode = EnumInstructionMode.ADD_NEW;
            InstructionsWindow iw = new InstructionsWindow(this, instructionMode);
            iw.Show();
        }

        /// <summary>
        /// Adds a new instruction to the program above the selected instruction
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_InsertAbove_Click(object sender, RoutedEventArgs e)
        {
            instructionMode = EnumInstructionMode.INSERT_ABOVE;
            InstructionsWindow iw = new InstructionsWindow(this, instructionMode);
            iw.Show();
        }

        /// <summary>
        /// Adds a new instruction to the program above below selected instruction
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_InsertBelow_Click(object sender, RoutedEventArgs e)
        {
            instructionMode = EnumInstructionMode.INSERT_BELOW;
            InstructionsWindow iw = new InstructionsWindow(this, instructionMode);
            iw.Show();
        }

        /// <summary>
        /// Deletes the selected instruction from the program
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Delete_Click(object sender, RoutedEventArgs e)
        {
            int index = lst_InstructionsList.SelectedIndex;
            lst_InstructionsList.ItemsSource = null;
            SimulatorProgram prog = programList.Where(x => x.Name.Equals(currentProgram)).FirstOrDefault();
            prog.Instructions.RemoveAt(index);
            UpdateIntructions();
        }

        /// <summary>
        /// Called when the window is closing
        /// </summary>
        /// <param name="sender">the object that initiated the event</param>
        /// <param name="e"> the eventargs</param>
        private void MainWindow2_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (programList.Count > 0)
            {
                MessageBoxResult saveresult = MessageBox.Show("There are unsaved programs do you want to save first?", "Save", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
                if (saveresult == MessageBoxResult.Yes)
                {
                    SaveProgram();
                }
                else if (saveresult == MessageBoxResult.Cancel)
                {
                    e.Cancel = true;
                    return;
                }
            }
            MessageBoxResult result = MessageBox.Show("Stopping the Simulator Continue?", "Stopping", MessageBoxButton.YesNo, MessageBoxImage.Information);
            if (result == MessageBoxResult.Yes)
            {
                e.Cancel = false;
            }
            else
            {
                e.Cancel = true;
                return;
            }
        }

#if DEBUG

        public void SayHello()
        {
            Console.WriteLine("Hello From Main Window");
        }

#endif

        /// <summary>
        /// Creates an instruction with 2 operands
        /// </summary>
        /// <param name="opcode"> the instruction opcode</param>
        /// <param name="op1"> the first operand</param>
        /// <param name="op2"> the second operand</param>
        /// <param name="Size"> the size of the instruction</param>
        /// <returns></returns>
        public Instruction CreateInstruction(EnumOpcodes opcode, Operand op1, Operand op2, Int32 Size)
        {
            return new Instruction((int)opcode, op1, op2, Size);
        }

        /// <summary>
        /// Creates an instruction with 1 operand
        /// </summary>
        /// <param name="opcode"> the instruction opcode</param>
        /// <param name="op1"> the first operand</param>
        /// <param name="Size"> the size of the instruction</param>
        public Instruction CreateInstruction(EnumOpcodes opcode, Operand op1, Int32 Size)
        {
            return new Instruction((int)opcode, op1, Size);
        }

        /// <summary>
        /// Creates an instruction with no operands
        /// </summary>
        /// <param name="opcode"> the instruction opcode</param>
        /// <param name="Size"> the size of the instruction</param>
        public Instruction CreateInstruction(EnumOpcodes opcode, Int32 Size)
        {
            return new Instruction((int)opcode, Size);
        }

        /// <summary>
        /// Adds an instruction to the currently loaded program
        /// </summary>
        /// <param name="ins"> the instruction to add</param>
        /// <param name="index"> the index in the program to add the instruction</param>
        public void AddInstruction(Instruction ins, int index)
        {
            if (ins != null)
            {
                if (lst_ProgramList.Items.Count == 0)
                {
                    MessageBox.Show("Please Create a program before adding instructions");
                    return;
                }

                if (instructionMode.Equals(EnumInstructionMode.ADD_NEW))
                {
                    SimulatorProgram prog = programList.Where(x => x.Name.Equals(currentProgram)).FirstOrDefault(); // find the currently active program
                    prog.AddInstruction(ref ins); // add the instruction
                }
                else if (instructionMode.Equals(EnumInstructionMode.INSERT_ABOVE))
                {
                    SimulatorProgram prog = programList.Where(x => x.Name.Equals(currentProgram)).FirstOrDefault(); // find the currently active program
                    if (index == -1)
                    {
                        prog.AddInstruction(ref ins);
                    }
                    else if (index == 0)
                    {
                        prog.AddInstruction(ref ins, 0);
                    }
                    else
                    {
                        prog.AddInstruction(ref ins, index); // add the instruction
                    }
                    //prog.AddInstruction(ref ins,index);
                }
                else if (instructionMode.Equals(EnumInstructionMode.INSERT_BELOW))
                {
                    SimulatorProgram prog = programList.Where(x => x.Name.Equals(currentProgram)).FirstOrDefault(); // find the currently active program
                    if (index == -1)
                    {
                        prog.AddInstruction(ref ins);
                    }
                    else if (index == 0)
                    {
                        prog.AddInstruction(ref ins, 0);
                    }
                    else
                    {
                        prog.AddInstruction(ref ins, index); // add the instruction
                    }
                    //prog.AddInstruction(ref ins,index);
                }
                else
                {
                    throw new ArgumentOutOfRangeException("Unknown instruction insertion mode");
                }
                UpdateIntructions();
            }
        }

        private void lst_InstructionsList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(activeUnit != null)
            {
                activeUnit.CurrentIndex = lst_InstructionsList.SelectedIndex;
                activeUnit.Stop = false;
                activeUnit.Done = false;
            }
            
        }

        private void lst_ProgramList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateIntructions();
            UpdateStack();
        }

        /// <summary>
        /// This function updates the stack display in the main window whenever a value is pushed or popped from the stack
        /// </summary>
        private void UpdateStack()
        {
            if (currentProgram.Equals(string.Empty)) // if a program has been loaded from a file
            {
                programList = lst_ProgramList.Items.OfType<SimulatorProgram>().ToList(); // populate the program list with the loaded programs
                Console.WriteLine(programList.Count);
                currentProgram = programList.First().Name; // load the first program in the list
            }
            if ((lst_ProgramList.SelectedItem) == null) // if no program is selected
            {
                lst_InstructionsList.SelectedIndex = 0;
                currentProgram = ((SimulatorProgram)lst_ProgramList.Items.GetItemAt(0)).Name; // select and load the first item
            }
            else
            {
                currentProgram = ((SimulatorProgram)lst_ProgramList.SelectedItem).Name; // load the selected item
            }
            lst_Stack.ItemsSource = null;
            lst_Stack.Items.Clear();
            SimulatorProgram prog = programList.Where(x => x.Name.Equals(currentProgram)).FirstOrDefault(); // find the currently active program
            if (prog.Stack != null)
            {
                lst_Stack.ItemsSource = prog.Stack.StackItems;
            }
            else
            {
                lst_Stack.ItemsSource = null;
            }
            Console.WriteLine("Stack Updated");
        }

        /// <summary>
        /// Updates the list of instructions
        /// </summary>
        private void UpdateIntructions()
        {
            if (currentProgram.Equals(string.Empty)) // if a program has been loaded from a file
            {
                programList = lst_ProgramList.Items.OfType<SimulatorProgram>().ToList(); // populate the program list with the loaded programs
                Console.WriteLine(programList.Count);
                currentProgram = programList.First().Name; // load the first program in the list
            }
            lst_InstructionsList.ItemsSource = null; // HACK item source must be set to null when modifying the items within the list
            lst_InstructionsList.Items.Clear(); // clear the item list
            if ((lst_ProgramList.SelectedItem) == null) // if no program is selected
            {
                lst_InstructionsList.SelectedIndex = 0;
                currentProgram = ((SimulatorProgram)lst_ProgramList.Items.GetItemAt(0)).Name; // select and load the first item
            }
            else
            {
                currentProgram = ((SimulatorProgram)lst_ProgramList.SelectedItem).Name; // load the selected item
            }
            SimulatorProgram prog = programList.Where(x => x.Name.Equals(currentProgram)).FirstOrDefault(); // find the selected program in the program list
            //lst_InstructionsList.ItemsSource.
            if (prog.Instructions != null)
            {
                lst_InstructionsList.ItemsSource = prog.Instructions; // load the instructions into the instruction list
            }
            else
            {
                lst_InstructionsList.ItemsSource = null;
            }
            Console.WriteLine("Instructions Updated");
        }

        private void btn_Load_Click(object sender, RoutedEventArgs e)
        {
            bool loaded = LoadProgram(); // load a program file
            if (!loaded)
            {
                throw new Exception("An error occurred while loading the program");
            }
            Console.WriteLine("Program Loaded Successfully");
        }

        private void btn_Save_Click(object sender, RoutedEventArgs e)
        {
            bool saved = SaveProgram(); //save a program file
            if (!saved)
            {
                throw new Exception("An error occurred while saving the program");
            }
            Console.WriteLine("Program Saved Successfully");
        }

        /// <summary>
        /// Saves the program list to a file
        /// </summary>
        /// <returns>True if succeeded false if not</returns>
        private bool SaveProgram()
        {
            SaveFileDialog dlg = new SaveFileDialog();
            dlg.FileName = "Program";
            dlg.DefaultExt = "*.sas";
            dlg.Filter = "Simulator Programs (.sas)|*.sas"; // Filter files by extension
            Nullable<bool> result = dlg.ShowDialog();
            if (result.Value == true)
            {
                if (File.Exists(dlg.FileName))
                {
                    File.Delete(dlg.FileName); // ensure we create a new file when we overwrite
                }
                SimulatorProgram[] progs = programList.ToArray();
                for (int i = 0; i < progs.Length; i++)
                {
                    SerializeObject<SimulatorProgram>(progs[i], dlg.FileName); // save all programs in the program list
                }
            }
            return true;
        }

        /// <summary>
        /// Loads a program list from a file
        /// </summary>
        /// <returns> true if succeeded false if not</returns>
        private bool LoadProgram()
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.FileName = "Program";
            ofd.DefaultExt = "*.sas";
            ofd.Filter = "Simulator Programs (.sas)|*.sas"; // Filter files by extension
            Nullable<bool> result = ofd.ShowDialog();
            if (result.Value == true)
            {
                DeSerializeObject<SimulatorProgram>(ofd.FileName); // load all programs from the file
            }
            return true;
        }

        /// <summary>
        /// Serializes a program List.
        /// </summary>
        /// <typeparam name="T">The type of program</typeparam>
        /// <param name="serializableObject"> the object to serialize</param>
        /// <param name="fileName">the file to save the objects to</param>
        public void SerializeObject<T>(T serializableObject, string filePath)
        {
            if (serializableObject == null) { return; }

            StreamWriter writer = new StreamWriter(filePath, true); // initialize a file writer
            JavaScriptSerializer serializer = new JavaScriptSerializer(); // initialize a serialize
            string json = serializer.Serialize(serializableObject); // serialize the object
            writer.WriteLine(json); // write the object to the file
            writer.Flush();
            writer.Close();
            writer.Dispose(); // flush close and dispose of the writer
        }

        /// <summary>
        /// De-serializes an .sas file into a program list
        /// </summary>
        /// <typeparam name="T">The type to deserialise</typeparam>
        /// <param name="fileName"> the name of the file to load the objects from</param>
        public void DeSerializeObject<T>(string fileName)
        {
            if (string.IsNullOrEmpty(fileName)) { return; }
            JavaScriptSerializer deserializer = new JavaScriptSerializer(); // initialize the deserializer
            StreamReader reader = new StreamReader(fileName); // initialize file reader
            string json;
            programList.Clear();
            lst_ProgramList.Items.Clear();
            while ((json = reader.ReadLine()) != null) // while there are lines to read
            {
                SimulatorProgram prog = deserializer.Deserialize<SimulatorProgram>(json); // deserialise the line into a object
                BindInstructionDelegates(ref prog);
                if (prog.Stack == null)
                {
                    prog.Stack = new ProgramStack();
                }
                programList.Add(prog);
                lst_ProgramList.Items.Add(prog); // add the object to the program list
            }

            return;
        }

        /// <summary>
        /// This function rebinds the delegate function to each instruction in a program after it is loaded from a file.
        /// </summary>
        /// <param name="prog"></param>
        private void BindInstructionDelegates(ref SimulatorProgram prog)
        {
            foreach (Instruction i in prog.Instructions)
            {
                i.BindDelegate();
            }
        }

        private void sld_ClockSpeed_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            Console.WriteLine(sld_ClockSpeed.Value);
        }

        private void btn_Step_Click(object sender, RoutedEventArgs e)
        {
            SimulatorProgram prog = programList.Where(x => x.Name.Equals(currentProgram)).FirstOrDefault();
            if (activeUnit == null || !activeUnit.Program.Equals(prog)) ;
            {
                activeUnit = new ExecutionUnit(prog, (int)sld_ClockSpeed.Value, lst_InstructionsList.SelectedIndex);
            }
            activeUnit.ExecuteInstruction();
            lst_InstructionsList.SelectedIndex = activeUnit.CurrentIndex;
            SpecialRegister.FindSpecialRegister("PC").setRegisterValue(prog.Instructions.ElementAt(lst_InstructionsList.SelectedIndex).Address, EnumOperandType.VALUE);
            SpecialRegister.FindSpecialRegister("IR").setRegisterValue(prog.Instructions.ElementAt(lst_InstructionsList.SelectedIndex).InstructionString, EnumOperandType.VALUE);
            UpdateRegisters();
            UpdateStack();
            UpdateSpecialRegisters();


        }

        private void btn_Run_Click(object sender, RoutedEventArgs e)
        {
            SimulatorProgram prog = programList.Where(x => x.Name.Equals(currentProgram)).FirstOrDefault();
            if (activeUnit == null || !activeUnit.Program.Equals(prog))
            {
                activeUnit = new ExecutionUnit(prog, (int)sld_ClockSpeed.Value, lst_InstructionsList.SelectedIndex);
            }
            Stopwatch s = new Stopwatch();
            s.Start();
            while (!activeUnit.Done)
            {
                activeUnit.ExecuteInstruction();
                lst_InstructionsList.SelectedIndex = activeUnit.CurrentIndex;
                SpecialRegister.FindSpecialRegister("PC").setRegisterValue(prog.Instructions.ElementAt(lst_InstructionsList.SelectedIndex).Address, EnumOperandType.VALUE);
                SpecialRegister.FindSpecialRegister("IR").setRegisterValue(prog.Instructions.ElementAt(lst_InstructionsList.SelectedIndex).InstructionString, EnumOperandType.VALUE);
                UpdateRegisters();
                UpdateStack();
                UpdateSpecialRegisters();

            }
            s.Stop();
            MessageBox.Show("Program Completed in: " + CalculateTime(s.ElapsedMilliseconds) + " Seconds", "", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        /// <summary>
        /// This function calculates the time in seconds the last program took to execute
        /// </summary>
        /// <param name="mills"> the number of milliseconds the last program took to execute</param>
        /// <returns></returns>
        private string CalculateTime(long mills)
        {
            long mils = 0;
            long secs = 0;
            secs = mills / 1000;
            mils = mills % 1000;
            return secs + "." + mills;
        }

        private void btn_Stop_Click(object sender, RoutedEventArgs e)
        {
            bool stopped = activeUnit != null ? activeUnit.Stop = true : activeUnit.Stop = false;
        }

        #endregion Methods

        private void btn_ResetProgram_Click(object sender, RoutedEventArgs e)
        {
            if (activeUnit != null)
            {
                lst_InstructionsList.SelectedIndex = 0;
                activeUnit.CurrentIndex = 0;
                activeUnit.Stop = false;
                activeUnit.Done = false;
            }
        }

        private void chk_OV_Checked(object sender, RoutedEventArgs e)
        {
            StatusFlags.OV.IsSet = true;
            SpecialRegister.FindSpecialRegister("SR").Value += StatusFlags.OV.Value;
            UpdateSpecialRegisters();
        }

        private void chk_OV_Unchecked(object sender, RoutedEventArgs e)
        {
            StatusFlags.OV.IsSet = false;
            SpecialRegister.FindSpecialRegister("SR").Value -= StatusFlags.OV.Value;
            UpdateSpecialRegisters();
        }

        private void chk_Z_Checked(object sender, RoutedEventArgs e)
        {
            StatusFlags.Z.IsSet = true;
            SpecialRegister.FindSpecialRegister("SR").Value += StatusFlags.Z.Value;
            UpdateSpecialRegisters();
        }

        private void chk_Z_Unchecked(object sender, RoutedEventArgs e)
        {
            StatusFlags.Z.IsSet = false;
            SpecialRegister.FindSpecialRegister("SR").Value -= StatusFlags.Z.Value;
            UpdateSpecialRegisters();
        }

        private void chk_N_Checked(object sender, RoutedEventArgs e)
        {
            StatusFlags.N.IsSet = true;
            SpecialRegister.FindSpecialRegister("SR").Value += StatusFlags.N.Value;
            UpdateSpecialRegisters();
        }

        private void chk_N_Unchecked(object sender, RoutedEventArgs e)
        {
            StatusFlags.N.IsSet = true;
            SpecialRegister.FindSpecialRegister("SR").Value -= StatusFlags.N.Value;
            UpdateSpecialRegisters();
        }
    }
}