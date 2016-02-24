using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Threading;
using Microsoft.Win32;
using CPU_OS_Simulator.Compiler;
using CPU_OS_Simulator.Compiler.Backend;
using CPU_OS_Simulator.CPU;
using CPU_OS_Simulator.Memory;
using Newtonsoft.Json; // See Third Party Libs/Credits.txt for licensing information for JSON.Net


namespace CPU_OS_Simulator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    ///
    public partial class MainWindow : Window, IDisposable
    {
        #region Global Variables

        private List<SimulatorProgram> programList;
        private EnumInstructionMode instructionMode;

        /// <summary>
        /// String variable to hold the name of the currently loaded program
        /// </summary>
        public string currentProgram = string.Empty;
        private ExecutionUnit activeUnit;
        private Stopwatch s;

        /// <summary>
        /// Variable to hold the current instance of this window
        /// </summary>
        public static MainWindow currentInstance;
        private Dispatcher dispatcher = Dispatcher.CurrentDispatcher;
        private BackgroundWorker executionWorker;
        private bool saved;
        private PageTableEntry currentPage;
        private PhysicalMemory memory;
        private SwapSpace swapSpace;

        #endregion Global Variables

        #region Properties
        /// <summary>
        /// Property to hold the list of loaded programs
        /// </summary>
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
        /// <summary>
        /// Property to hold the selected instruction mode
        /// </summary>
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
        /// <summary>
        /// Property yo hold the active execution unit
        /// </summary>
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
        /// <summary>
        /// Property yo hold the active physical memory
        /// </summary>
        public PhysicalMemory Memory
        {
            get { return memory; }
            set { memory = value; }
        }
        /// <summary>
        /// Property to hold the active swap space
        /// </summary>
        public SwapSpace SwapSpace
        {
            get { return swapSpace; }
            set { swapSpace = value; }
        }
        /// <summary>
        /// Property to hold the active background worker
        /// </summary>
        public BackgroundWorker ExecutionWorker
        {
            get { return executionWorker; }
            set { executionWorker = value; }
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
                string path = (new Uri(Assembly.GetExecutingAssembly().CodeBase)).AbsolutePath;
                SetAssociation(".sas", "Simulator Program File", path, "CPU-OS Simulator Program File");
                SetAssociation(".soss","Simulator OS State", path, "Simulator OS State");
            }
            programList = new List<SimulatorProgram>();
            PopulateRegisters();
            System.Console.WriteLine("Hello World");
            currentInstance = this;
            memory = new PhysicalMemory(10);
            swapSpace = new SwapSpace();
        }

        #endregion Constructors

        #region Methods

        private static bool IsAdministrator()
        {
            var identity = WindowsIdentity.GetCurrent();
            var principal = new WindowsPrincipal(identity);
            return principal.IsInRole(WindowsBuiltInRole.Administrator);
        }

        [DllImport("shell32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern void SHChangeNotify(uint wEventId, uint uFlags, IntPtr dwItem1, IntPtr dwItem2);

        /// <summary>
        /// This Function sets file associations for simulator file types
        /// </summary>
        /// <param name="Extension"> the extension of the file type</param>
        /// <param name="KeyName"> the name of the registry key to hold the association </param>
        /// <param name="OpenWith"> the program the file should open with</param>
        /// <param name="FileDescription"> the description of the file type</param>
        private static void SetAssociation(string Extension, string KeyName, string OpenWith, string FileDescription)
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

            CurrentUser = Registry.CurrentUser.OpenSubKey("Software\\Microsoft\\Windows\\CurrentVersion\\Explorer\\FileExts\\" + Extension, true);
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
            txt_IR.Text = SpecialRegister.FindSpecialRegister("IR").ValueString;
            txt_MAR.Text = SpecialRegister.FindSpecialRegister("MAR").Value.ToString();
            txt_MDR.Text = SpecialRegister.FindSpecialRegister("MDR").ValueString;
            txt_PC.Text = SpecialRegister.FindSpecialRegister("PC").Value.ToString();
            txt_SP.Text = SpecialRegister.FindSpecialRegister("SP").Value.ToString();
            txt_SR.Text = SpecialRegister.FindSpecialRegister("SR").Value.ToString();
        }

        private void MainWindow2_Loaded(object sender, RoutedEventArgs e)
        {
            Title += " " + GetProgramVersion();
            SpecialRegister.FindSpecialRegister("BR")
                .SetRegisterValue(Convert.ToInt32(txt_BR.Text), EnumOperandType.VALUE);
            SpecialRegister.FindSpecialRegister("IR").SetRegisterValue(txt_IR.Text, EnumOperandType.VALUE);
            SpecialRegister.FindSpecialRegister("MAR")
                .SetRegisterValue(Convert.ToInt32(txt_MAR.Text), EnumOperandType.ADDRESS);
            SpecialRegister.FindSpecialRegister("MDR").SetRegisterValue(txt_MDR.Text, EnumOperandType.VALUE);
            SpecialRegister.FindSpecialRegister("PC")
                .SetRegisterValue(Convert.ToInt32(txt_PC.Text), EnumOperandType.ADDRESS);
            SpecialRegister.FindSpecialRegister("SP")
                .SetRegisterValue(Convert.ToInt32(txt_SP.Text), EnumOperandType.ADDRESS);
            SpecialRegister.FindSpecialRegister("SR")
                .SetRegisterValue(Convert.ToInt32(txt_SR.Text), EnumOperandType.VALUE);
#if !DEBUG
        }

#else
            Title += " DEBUG BUILD ";
            //DebugFunction();
        }
        private void DebugFunction()
        { 
            //DebugCompilingProgram();
            MemoryPage m = new MemoryPage(0, 0,"Debug",-1);
            //m.Data[0] = new MemorySegment(0);
            m.Data[0].Byte0 = (byte)'A';
            m.Data[0].Byte1 = (byte)'B';
            DebugTestSwapping(m);
            DebugTestSwappingFromMemoryOverflow();
            MemoryWindow wind = new MemoryWindow(this, m);
            wind.Show();
        }

        private void DebugCompilingProgram()
        {
            SimulatorProgram program = CreateNewProgram("Debug", 0, 1);
            program.Instructions.Add(new Instruction((int) EnumOpcodes.MOV,new Operand(Register.R01, EnumOperandType.VALUE),EnumAddressType.UNKNOWN, new Operand(13,EnumOperandType.VALUE),EnumAddressType.UNKNOWN, 4));
            if (program == null) return;
            lst_ProgramList.Items.Add(program);
            programList.Add(program);
            currentProgram = program.Name;
            CompilerMain compiler = new CompilerMain(program.Instructions,program.Name);
            List<List<InstructionSegment>> segmentList = compiler.CompileFromInstructions();
            List<byte> bytes = compiler.CompileToBytes(segmentList);
            CompiledProgram compiledProgram = new CompiledProgram(bytes,program.Name,bytes.Count);
            MemoryPage m = new MemoryPage(0, 0,program.Name,-1);
            m.ZeroMemory();
            memory.AddPage(m,0);
            compiledProgram.LoadinMemory(0);
            MemoryWindow wind = new MemoryWindow(this, memory.Pages[0]);
            wind.Show();
        }

        private void DebugTestSwappingFromMemoryOverflow()
        {
            for (int i = 0; i < memory.Capacity + 1; i++)
            {
                MemoryPage m = new MemoryPage(i, i * MemoryPage.PAGE_SIZE,"Debug",-1);
                memory.AddPage(m, memory.Pages.Count);
                System.Console.WriteLine("Pages in Memory = " + memory.Pages.Count);
                System.Console.WriteLine("Pages Swapped Out = " + swapSpace.SwappedMemoryPages.Count);
            }
        }

        private void DebugTestSwapping(MemoryPage memoryPage)
        {
            memory.AddPage(memoryPage, memory.Pages.Count);
            PageTableEntry f = memory.Table.Entries.FirstOrDefault(p => p.Page.Equals(memoryPage));
            memoryPage.SwapOut(f.PhysicalAddress, f.FrameNumber);
            System.Console.WriteLine("Pages in Memory = " + memory.Pages.Count);
            System.Console.WriteLine("Pages Swapped Out = " + swapSpace.SwappedMemoryPages.Count);
            memoryPage.SwapIn(f.PhysicalAddress, f.FrameNumber);
            System.Console.WriteLine("Pages in Memory = " + memory.Pages.Count);
            System.Console.WriteLine("Pages Swapped Out = " + swapSpace.SwappedMemoryPages.Count);
        }
#endif
        /// <summary>
        /// Gets the build number of the running program
        /// </summary>
        /// <returns> The build number of the running program</returns>
        private static string GetProgramVersion()
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
            if (prog == null) return;
            lst_ProgramList.Items.Add(prog);
            programList.Add(prog);
            currentProgram = prog.Name;
            lst_ProgramList.SelectedItem = lst_ProgramList.Items.GetItemAt(lst_ProgramList.Items.Count - 1);
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
            if (txtBaseAddress.Text == "")
            {
                MessageBox.Show("Please Enter a Base Address");
                return null;
            }
            string programName = txtProgramName.Text;
            int baseAddress = Convert.ToInt32(txtBaseAddress.Text);
            int pages = Convert.ToInt32(cmb_Pages.Text);
            SimulatorProgram program = new SimulatorProgram(programName, baseAddress, pages);
            AllocateMemory(program);
            System.Console.WriteLine("Program " + program.Name + " Created");
            return program;
        }

        /// <summary>
        /// Creates a new program based on entered data
        /// </summary>
        /// <returns>the created program</returns>
        private SimulatorProgram CreateNewProgram(string programName, int baseAddress, int pages)
        {
            SimulatorProgram program = new SimulatorProgram(programName, baseAddress, pages);
            AllocateMemory(program);
            System.Console.WriteLine("Program " + program.Name + " Created");
            return program;
        }

        private void AllocateMemory(SimulatorProgram prog)
        {
            for (int i = 0; i < prog.Pages; i++)
            {
                MemoryPage memoryPage = new MemoryPage(i, (i * MemoryPage.PAGE_SIZE),prog.Name,-1);
                memory.AddPage(memoryPage, memory.Pages.Count);
            }
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
            try
            {
                int index = lst_InstructionsList.SelectedIndex;
                lst_InstructionsList.ItemsSource = null;
                SimulatorProgram prog = programList.Where(x => x.Name.Equals(currentProgram)).FirstOrDefault();
                prog.Instructions.RemoveAt(index);
                UpdateIntructions();
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex.StackTrace);
                MessageBox.Show("Could not delete instruction or there was no instruction selected to delete");
            }
           
        }

        /// <summary>
        /// Called when the window is closing
        /// </summary>
        /// <param name="sender">the object that initiated the event</param>
        /// <param name="e"> the eventargs</param>
        private void MainWindow2_Closing(object sender, CancelEventArgs e)
        {
            if (programList.Count > 0 && !saved)
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
            }
        }

        /// <summary>
        /// Creates an instruction with 2 operands
        /// </summary>
        /// <param name="opcode"> the instruction opcode</param>
        /// <param name="op1"> the first operand</param>
        /// <param name="op1mem"> whether the first operand is a memory address</param>
        /// <param name="op2"> the second operand</param>
        /// <param name="op2mem"> whether the second operand is a memory address</param>
        /// <param name="Size"> the size of the instruction</param>
        /// <returns></returns>
        public Instruction CreateInstruction(EnumOpcodes opcode, Operand op1, EnumAddressType op1mem, Operand op2, EnumAddressType op2mem, int Size)
        {
            return new Instruction((int)opcode, op1, op1mem, op2, op2mem, Size);
        }

        /// <summary>
        /// Creates an instruction with 1 operand
        /// </summary>
        /// <param name="opcode"> the instruction opcode</param>
        /// <param name="op1"> the first operand</param>
        /// <param name="op1mem"> whether the first operand is a memory address</param>
        /// <param name="Size"> the size of the instruction</param>
        public Instruction CreateInstruction(EnumOpcodes opcode, Operand op1, EnumAddressType op1mem, int Size)
        {
            return new Instruction((int)opcode, op1, op1mem, Size);
        }

        /// <summary>
        /// Creates an instruction with no operands
        /// </summary>
        /// <param name="opcode"> the instruction opcode</param>
        /// <param name="Size"> the size of the instruction</param>
        public Instruction CreateInstruction(EnumOpcodes opcode, int Size)
        {
            return new Instruction((int)opcode, Size);
        }

        public void AddLabel(string name, int logicalAddress, int physicalAddress)
        {
            SimulatorProgram prog = ProgramList.Where(x => x.Name.Equals(currentProgram)).FirstOrDefault();
            if (prog != null)
            {
                SimulatorLabel label = new SimulatorLabel(prog, logicalAddress, physicalAddress, name);
                prog.Labels.Add(label);
                //lst_InstructionsList.Items.Add(label);
            }
            UpdateIntructions();
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
                        prog.AddInstruction(ref ins, 1);
                    }
                    else
                    {
                        prog.AddInstruction(ref ins, index + 1); // add the instruction
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
            if (activeUnit != null)
            {
                activeUnit.CurrentIndex = lst_InstructionsList.SelectedIndex;
                activeUnit.Stop = false;
                activeUnit.Done = false;
            }

        }

        private void lst_ProgramList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lst_ProgramList.Items.Count > 0)
            {
                UpdateIntructions();
                UpdateStack();
            }
            else
            {
                lst_InstructionsList.ItemsSource = null;
                lst_Stack.ItemsSource = null;
                lst_InstructionsList.Items.Clear();
                lst_Stack.Items.Clear();
                UpdateIntructions();
                //lst_InstructionsList.ItemsSource = programList;
                //lst_InstructionsList.ItemsSource = new List<ItemsControl>(1);
                //UpdateInterface(sender, null);
            }
        }

        /// <summary>
        /// This function updates the stack display in the main window whenever a value is pushed or popped from the stack
        /// </summary>
        private void UpdateStack()
        {
            if (currentProgram.Equals(string.Empty)) // if a program has been loaded from a file
            {
                programList = lst_ProgramList.Items.OfType<SimulatorProgram>().ToList(); // populate the program list with the loaded programs
                System.Console.WriteLine(programList.Count);
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
            System.Console.WriteLine("Stack Updated");
        }

        /// <summary>
        /// This function updates the list of instructions
        /// </summary>
        private void UpdateIntructions()
        {
            if (currentProgram.Equals(string.Empty)) // if a program has been loaded from a file
            {
                programList = lst_ProgramList.Items.OfType<SimulatorProgram>().ToList(); // populate the program list with the loaded programs
                System.Console.WriteLine(programList.Count);
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
           
            if (prog.Instructions != null)
            {
                lst_InstructionsList.ItemsSource = prog.Instructions; // load the instructions into the instruction list
            }
            else
            {
                lst_InstructionsList.ItemsSource = null;
                return;
            }

            CompositeCollection c = new CompositeCollection();
            foreach (Instruction ins in prog.Instructions)
            {
                SimulatorLabel label = prog.Labels.Where(x => x.LogicalAddress == ins.LogicalAddress).FirstOrDefault();
                if (label != null)
                    c.Add(label);
                c.Add(ins);
            }
            lst_InstructionsList.ItemsSource = c;
            prog.UpdateAddresses();
            saved = false;
            System.Console.WriteLine("Instructions Updated");
        }

        private void btn_Load_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                bool isloaded = LoadProgram(); // load a program file
                if (!isloaded)
                {
                    throw new Exception("An error occurred while loading the program");
                }
                lst_ProgramList.SelectedItem = lst_ProgramList.Items[0];
                System.Console.WriteLine("Program Loaded Successfully");
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex.StackTrace);
            }
        }

        private void btn_Save_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                bool issaved = SaveProgram(); //save a program file
                if (!issaved)
                {
                    throw new Exception("An error occurred while saving the program");
                }
                System.Console.WriteLine("Program Saved Successfully");
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex.StackTrace);
            }
            
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
            bool? result = dlg.ShowDialog();
            if (result.Value)
            {
                if (File.Exists(dlg.FileName))
                {
                    File.Delete(dlg.FileName); // ensure we create a new file when we overwrite
                }
                SimulatorProgram[] progs = programList.ToArray();
                foreach (SimulatorProgram t in progs)
                {
                    //SerializeObjectNoLib(t,dlg.FileName);
                    SerializeObjectLib(t, dlg.FileName); // save all programs in the program list
                }
            }
            saved = true;
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
            bool? result = ofd.ShowDialog();
            if (result.Value)
            {
                //DeSerializeObjectNoLib<SimulatorProgram>(ofd.FileName); 
                DeSerialiseObjectLib(ofd.FileName); // load all programs from the file
            }
            return true;
        }

        /// <summary>
        /// Serializes a program List.
        /// </summary>
        /// <typeparam name="T">The type of program</typeparam>
        /// <param name="serializableObject"> the object to serialize</param>
        /// <param name="filePath">the file to save the objects to</param>
        private void SerializeObjectNoLib<T>(T serializableObject, string filePath)
        {
            if (serializableObject == null || string.IsNullOrEmpty(filePath)) { return; }

            StreamWriter writer = new StreamWriter(filePath, false); // initialize a file writer
            JavaScriptSerializer serializer = new JavaScriptSerializer(); // initialize a serialize
            string json = serializer.Serialize(serializableObject); // serialize the object
            writer.WriteLine(json); // write the object to the file
            writer.Flush();
            writer.Close();
            writer.Dispose(); // flush close and dispose of the writer
        }
        /// <summary>
        /// Serializes a program List using JSON.NET (See Third Party Libs/Credits.txt for more information
        /// </summary>
        /// <param name="serializableObject"> the object to serialize</param>
        /// <param name="filePath">the file to save the objects to</param>
        private void SerializeObjectLib(SimulatorProgram serializableObject, string filePath)
        {
            if (serializableObject == null || filePath.Equals(String.Empty)){ return; }
            JsonSerializerSettings settings = new JsonSerializerSettings
            {
                PreserveReferencesHandling = PreserveReferencesHandling.Objects,
                ReferenceLoopHandling = ReferenceLoopHandling.Serialize,
                Formatting = Formatting.None
            };
            StreamWriter writer = new StreamWriter(filePath,false);
            //JsonSerializer serializer = JsonSerializer.Create(settings);
            string json = JsonConvert.SerializeObject(serializableObject, settings);
            json = Regex.Replace(json, "(\"(?:[^\"\\\\]|\\\\.)*\")|\\s+", "$1"); // remove all whitespace from the string
            writer.WriteLine(json);
            writer.Flush();
            writer.Close();
            writer.Dispose();
        }

        /// <summary>
        /// De-serializes an .sas file into a program list
        /// </summary>
        /// <typeparam name="T">The type to deserialise</typeparam>
        /// <param name="fileName"> the name of the file to load the objects from</param>
        private void DeSerializeObjectNoLib<T>(string fileName)
        {
            if (string.IsNullOrEmpty(fileName)) { return; }
            JavaScriptSerializer deserializer = new JavaScriptSerializer(); // initialize the deserializer
            StreamReader reader = new StreamReader(fileName); // initialize file reader
            string json;
            //programList.Clear();
            //lst_ProgramList.Items.Clear();
            while ((json = reader.ReadLine()) != null) // while there are lines to read
            {
                SimulatorProgram prog = deserializer.Deserialize<SimulatorProgram>(json); // deserialise the line into a object
                BindInstructionDelegates(ref prog);
                if (prog.Stack == null)
                {
                    prog.Stack = new ProgramStack();
                }
                if (prog.Labels == null)
                {
                    prog.Labels = new List<SimulatorLabel>();
                }
                for (int i = 0; i < prog.Pages; i++)
                {
                    MemoryPage memoryPage = new MemoryPage(i, (i*MemoryPage.PAGE_SIZE),prog.Name,-1);
                    memory.AddPage(memoryPage, memory.Pages.Count);
                }
                programList.Add(prog);
                lst_ProgramList.Items.Add(prog); // add the object to the program list
            }
        }
        /// <summary>
        /// De-serializes an .sas file into a program list using JSON.NET (See Third Party Libs/Credits.txt for more information
        /// </summary>
        /// <param name="fileName"> the name of the file to load the objects from</param>
        private void DeSerialiseObjectLib(string fileName)
        {
            if (string.IsNullOrEmpty(fileName)) { return; }
            string json = String.Empty;
            StreamReader reader = new StreamReader(fileName);
            while ((json = reader.ReadLine()) != null)
            {
                //JToken root = JObject.Parse(json);
                //JToken program = root["$id"];
                JsonSerializerSettings settings = new JsonSerializerSettings
                {
                    PreserveReferencesHandling = PreserveReferencesHandling.Objects,
                    ReferenceLoopHandling = ReferenceLoopHandling.Serialize,
                    Formatting = Formatting.None
                };
                SimulatorProgram prog = JsonConvert.DeserializeObject<SimulatorProgram>(json,settings);
                BindInstructionDelegates(ref prog);
                if (prog.Stack == null)
                {
                    prog.Stack = new ProgramStack();
                }
                if (prog.Labels == null)
                {
                    prog.Labels = new List<SimulatorLabel>();
                }
                for (int i = 0; i < prog.Pages; i++)
                {
                    MemoryPage page = new MemoryPage(i,i * MemoryPage.PAGE_SIZE,prog.Name,-1);
                    memory.AddPage(page, memory.Pages.Count);
                }
                programList.Add(prog);
                lst_ProgramList.Items.Add(prog);
                OperatingSystemMainWindow wind = GetOperatingSystemMainWindowInstance();
                if (wind != null)
                {
                    wind.UpdateInterface();
                }
            }
        }

        /// <summary>
        /// This function rebinds the delegate function to each instruction in a program after it is loaded from a file.
        /// </summary>
        /// <param name="prog"> the program to bind its instruction delegates </param>
        private void BindInstructionDelegates(ref SimulatorProgram prog)
        {
            foreach (Instruction i in prog.Instructions)
            {
                i.BindDelegate();
            }
        }

        private void sld_ClockSpeed_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            System.Console.WriteLine(sld_ClockSpeed.Value);
        }

        private void btn_Step_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                SimulatorProgram prog = programList.Where(x => x.Name.Equals(currentProgram)).FirstOrDefault();
                if (activeUnit == null || !activeUnit.Program.Equals(prog))
                {
                    activeUnit = new ExecutionUnit(prog, (int) sld_ClockSpeed.Value, lst_InstructionsList.SelectedIndex);
                }
                activeUnit.ExecuteInstruction();
                lst_InstructionsList.SelectedIndex = activeUnit.CurrentIndex;
                SpecialRegister.FindSpecialRegister("PC")
                    .SetRegisterValue(prog.Instructions.ElementAt(lst_InstructionsList.SelectedIndex).LogicalAddress,
                        EnumOperandType.VALUE);
                SpecialRegister.FindSpecialRegister("IR")
                    .SetRegisterValue(
                        prog.Instructions.ElementAt(lst_InstructionsList.SelectedIndex).InstructionString,
                        EnumOperandType.VALUE);
                SpecialRegister.FindSpecialRegister("MDR")
                    .SetRegisterValue(
                        prog.Instructions.ElementAt(lst_InstructionsList.SelectedIndex).InstructionString,
                        EnumOperandType.VALUE);
                SpecialRegister.FindSpecialRegister("MAR")
                    .SetRegisterValue(prog.Instructions.ElementAt(lst_InstructionsList.SelectedIndex).PhysicalAddress,
                        EnumOperandType.VALUE);
                UpdateRegisters();
                UpdateStack();
                UpdateSpecialRegisters();
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex.StackTrace);
                //MessageBox.Show("Please Load a Program before running the CPU");
            }
        }

        private void btn_Run_Click(object sender, RoutedEventArgs e)
        {
            SimulatorProgram prog = programList.Where(x => x.Name.Equals(currentProgram)).FirstOrDefault();
            if (activeUnit == null || !activeUnit.Program.Equals(prog))
            {
                activeUnit = new ExecutionUnit(prog, (int)sld_ClockSpeed.Value, lst_InstructionsList.SelectedIndex);
            }
            CreateBackgroundWorker();
            executionWorker.RunWorkerAsync(null);
            //var executionThread = new Thread(ExecuteProgram);
            //executionThread.Start();
        }

        /// <summary>
        /// Creates a background worker for the execution thread to run on
        /// </summary>
        private void CreateBackgroundWorker()
        {
            executionWorker = new BackgroundWorker();
            executionWorker.DoWork += CreateExecutionThread;
            executionWorker.WorkerSupportsCancellation = true;
            executionWorker.WorkerReportsProgress = true;
            executionWorker.ProgressChanged += UpdateInterface;
        }
        /// <summary>
        /// Asynchronous function called after every instruction is executed to update required values and user interface asynchronously
        /// </summary>
        /// <param name="sender"> the object that triggered this event</param>
        /// <param name="e">The parameters passed to this event ></param>
        public async void UpdateInterface(object sender, ProgressChangedEventArgs e)
        {
            SimulatorProgram prog = programList.Where(x => x.Name.Equals(currentProgram)).FirstOrDefault();
            lst_InstructionsList.SelectedIndex = activeUnit.CurrentIndex;
            SpecialRegister.FindSpecialRegister("PC").SetRegisterValue(prog.Instructions.ElementAt(lst_InstructionsList.SelectedIndex).LogicalAddress, EnumOperandType.VALUE);
            SpecialRegister.FindSpecialRegister("IR").SetRegisterValue(prog.Instructions.ElementAt(lst_InstructionsList.SelectedIndex).InstructionString, EnumOperandType.VALUE);
            SpecialRegister.FindSpecialRegister("MDR").SetRegisterValue(prog.Instructions.ElementAt(lst_InstructionsList.SelectedIndex).InstructionString, EnumOperandType.VALUE);
            SpecialRegister.FindSpecialRegister("MAR").SetRegisterValue(prog.Instructions.ElementAt(lst_InstructionsList.SelectedIndex).PhysicalAddress, EnumOperandType.VALUE);
            UpdateRegisters();
            UpdateStack();
            UpdateSpecialRegisters();
            UpdateStatusFlags();
        }

        private void CreateExecutionThread(object sender, DoWorkEventArgs e)
        {
            var executionThread = new Thread(ExecuteProgram);
            executionThread.Start();
        }

        /// <summary>
        /// Asynchronous function called after every instruction is executed to update required values and user interface asynchronously
        /// </summary>
        /// <returns> A task to indicate to the main thread that the function has finished executing</returns>
        public async Task<int> UpdateInterface()
        {
            SimulatorProgram prog = programList.Where(x => x.Name.Equals(currentProgram)).FirstOrDefault();
            if (activeUnit != null)
            {
                lst_InstructionsList.SelectedIndex = activeUnit.CurrentIndex;
            }
            //SpecialRegister.FindSpecialRegister("PC").SetRegisterValue(prog.Instructions.ElementAt(lst_InstructionsList.SelectedIndex).LogicalAddress, EnumOperandType.VALUE);
            //SpecialRegister.FindSpecialRegister("IR").SetRegisterValue(prog.Instructions.ElementAt(lst_InstructionsList.SelectedIndex).InstructionString, EnumOperandType.VALUE);
            //SpecialRegister.FindSpecialRegister("MDR").SetRegisterValue(prog.Instructions.ElementAt(lst_InstructionsList.SelectedIndex).InstructionString, EnumOperandType.VALUE);
            //SpecialRegister.FindSpecialRegister("MAR").SetRegisterValue(prog.Instructions.ElementAt(lst_InstructionsList.SelectedIndex).PhysicalAddress, EnumOperandType.VALUE);
            UpdateRegisters();
            UpdateStack();
            UpdateSpecialRegisters();
            UpdateStatusFlags();
            //executionWorker.ReportProgress(0,null);
            return 0;
        }

        private void UpdateStatusFlags()
        {
            chk_N.IsChecked = StatusFlags.N.IsSet;
            chk_OV.IsChecked = StatusFlags.OV.IsSet;
            chk_Z.IsChecked = StatusFlags.Z.IsSet;
        }

        /// <summary>
        /// Asynchronous function called to begin executing a program on the execution thread
        /// </summary>
        /// <param name="program"> The program object to execute</param>
        private async void ExecuteProgram(object program)
        {
            try
            {
                s = new Stopwatch();
                s.Start();
                while (!activeUnit.Done && !activeUnit.Stop && !executionWorker.CancellationPending)
                {
                    activeUnit.ExecuteInstruction();
                    try
                    {
                        await CallFromMainThread(UpdateInterface);
                    }
                    catch (TaskCanceledException ex)
                    {
                        System.Console.WriteLine(ex.StackTrace);
                        Environment.Exit(0);
                    }

                }
                s.Stop();
                MessageBox.Show("Program Completed in: " + await CalculateTime(s.ElapsedMilliseconds) + " Seconds", "", MessageBoxButton.OK, MessageBoxImage.Information);
                //Thread.CurrentThread.Join();
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex.StackTrace);
            }
          
        }
        /// <summary>
        /// Bridge function used to call functions on the main thread from within the background thread
        /// </summary>
        /// <param name="FunctionPointer"> The function to call </param>
        /// <returns>A task to indicate to the main thread that the function has finished executing</returns>
        private async Task<int> CallFromMainThread(Func<Task<int>> FunctionPointer)
        {
            var invoke = dispatcher.Invoke(FunctionPointer);
            if (invoke != null) await invoke;
            return 0;
        }

        /// <summary>
        /// This function calculates the time in seconds the last program took to execute
        /// </summary>
        /// <param name="mills"> the number of milliseconds the last program took to execute</param>
        /// <returns></returns>
        private async Task<string> CalculateTime(long mills)
        {
            long mils = 0;
            long secs = 0;
            secs = mills / 1000;
            mils = mills % 1000;
            return secs + "." + mils;
        }

        private void btn_Stop_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                executionWorker.CancelAsync();
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex.StackTrace);
                MessageBox.Show("No Program to Stop");
            }
            
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
            StatusFlags.N.IsSet = false;
            SpecialRegister.FindSpecialRegister("SR").Value -= StatusFlags.N.Value;
            UpdateSpecialRegisters();
        }
        /// <summary>
        /// This method is called when the window is getting disposed
        /// </summary>
        public void Dispose()
        {
            executionWorker.Dispose();
            executionWorker = null;
        }

        private void btn_ShowMemory_Click(object sender, RoutedEventArgs e)
        {
            SimulatorProgram program = programList.Where(x => x.Name.Equals(currentProgram)).FirstOrDefault();
            int index = programList.IndexOf(program);
            int framenumber = 0;
            for (int i = 0; i < index; i++)
            {
                framenumber += programList[i].Pages;
            }
            MemoryPage page = memory.RequestMemoryPage(framenumber);
            if (page != null)
            {
                MemoryWindow m = new MemoryWindow(this, page);
                m.Show();
            }
            else
            {
                MessageBox.Show("There was an error while loading a memory page");
            }

            //SimulatorProgram program = programList.Where(x => x.Name.Equals(currentProgram)).FirstOrDefault();
            //if (program == null) return;
            //MemoryWindow m = new MemoryWindow(this, program.Memory.FirstOrDefault());
            //m.Show();

            //MessageBox.Show("This Button is not implemented yet", "Not implemented", MessageBoxButton.OK,
            //    MessageBoxImage.Information);
        }

        private void btn_Console_Click(object sender, RoutedEventArgs e)
        {
            ConsoleWindow cw = new ConsoleWindow(this);
            cw.Show();
            cw.txt_Console.Focus();
        }

        private void btn_OS_Click(object sender, RoutedEventArgs e)
        {
            OperatingSystemMainWindow window = new OperatingSystemMainWindow(this);
            window.Show();
        }

        private void btn_ShowPCB_Click(object sender, RoutedEventArgs e)
        {
            ProcessControlBlockWindow window = new ProcessControlBlockWindow(this,null);
            window.Show();
        }

        private OperatingSystemMainWindow GetOperatingSystemMainWindowInstance()
        {
            Assembly windowBridge = Assembly.LoadFrom("CPU_OS_Simulator.WindowBridge.dll"); // Load the window bridge module
            //System.Console.WriteLine(windowBridge.GetExportedTypes()[1]);
            Type WindowType = windowBridge.GetType(windowBridge.GetExportedTypes()[1].ToString()); // get the name of the type that contains the window instances
            OperatingSystemMainWindow window = (OperatingSystemMainWindow) WindowType.GetField("OperatingSystemMainWindowInstance").GetValue(null); // get the value of the static OperatingSystemMainWindowInstance field
            return window;
        }

        private void btn_LoadCode_Click(object sender, RoutedEventArgs e)
        {
            CompileProgramFromInstructions();
        }

        /// <summary>
        /// This function compiles the currently selected program's instructions into their binary equivalent
        /// </summary>
        private void CompileProgramFromInstructions()
        {
            try
            {
                //TODO Not Fully Working because compiler is not complete
                SimulatorProgram p = programList.Where(x => x.Name.Equals(currentProgram)).FirstOrDefault();
                CompilerMain compiler = new CompilerMain(p.Instructions, p.Name);
                List<List<InstructionSegment>> compiledInstructions = compiler.CompileFromInstructions();
                if (compiledInstructions == null || compiledInstructions.Count == 0)
                {
                    MessageBox.Show("An error occurred during compilation");
                    return;
                }
                List<byte> programBytes = compiler.CompileToBytes(compiledInstructions);
                CompiledProgram compiledProgram = new CompiledProgram(programBytes,p.Name,programBytes.Count);
                MemoryPage temp = memory.Pages.Where(x => x.ProgramName.Equals(p.Name)).FirstOrDefault();
                compiledProgram.LoadinMemory(temp.FrameNumber);
                MemoryWindow wind = new MemoryWindow(this,memory.RequestMemoryPage(temp.FrameNumber));
                wind.Show();

            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex.StackTrace);
                MessageBox.Show("Please Select a program to load code from");
            }
        }

        private async void btn_RemoveAllPrograms_Click(object sender, RoutedEventArgs e)
        {
            await RemoveAllPrograms();
        }

        /// <summary>
        /// This function asynchronously removes all programs from the loaded program list and also deallocates their memory
        /// </summary>
        /// <returns>A task to notify the calling thread that the operation has completed </returns>
        private async Task<int> RemoveAllPrograms()
        {
            lst_ProgramList.Items.Clear();
            lst_ProgramList.ItemsSource = null;
            foreach (SimulatorProgram program in programList)
            {
                memory.Pages.RemoveAll(x => x.ProgramName.Equals(program.Name));
                programList.Remove(program);
            }
            lst_ProgramList.ItemsSource = programList;
            return await UpdateInterface();
        }

        private async void btn_RemovePrograms_Click(object sender, RoutedEventArgs e)
        {
            await RemoveProgram(currentProgram);
        }

        /// <summary>
        /// This function asynchronously removes a program from memory
        /// </summary>
        /// <param name="ProgramName">the name of the program to remove </param>
        /// <returns> a task to notify the calling thread that the task has completed</returns>
        private async Task<int> RemoveProgram(string ProgramName)
        {
            lst_ProgramList.Items.Clear();
            lst_ProgramList.ItemsSource = null;
            programList.RemoveAll(x => x.Name.Equals(ProgramName));
            lst_ProgramList.ItemsSource = programList;
            return await UpdateInterface();
        }

        private async void btn_DeleteProgramInstance_Click(object sender, RoutedEventArgs e)
        {
            await DeleteProgramInstance(currentProgram);
        }

        /// <summary>
        /// This function asynchronously removes a program instance from memory 
        /// </summary>
        /// <param name="ProgramName"> the name of the program instance to remove from memory </param>
        /// <returns> a task to notify the calling thread that the task has completed</returns>
        private async Task<int> DeleteProgramInstance(string ProgramName)
        {
            lst_ProgramList.ItemsSource = null;
            lst_ProgramList.Items.Clear();
            programList.RemoveAll(x => x.Name.Equals(ProgramName + "_inst"));
            lst_ProgramList.ItemsSource = programList;
            return await UpdateInterface();
        }

        private async void btn_CreateProgramInstance_Click(object sender, RoutedEventArgs e)
        {
            int result = await CreateProgramInstance(currentProgram);
            if (result == int.MinValue)
            {
                MessageBox.Show("An Error Occurred while creating an instance of program " + currentProgram);
            }
        }

        /// <summary>
        /// This method creates a program instance and loads it into memory
        /// </summary>
        /// <param name="ProgramName"> the name of the program to create</param>
        /// <returns> a task to notify the calling thread that the task has completed</returns>
        private async Task<int> CreateProgramInstance(string ProgramName)
        {
            lst_ProgramList.ItemsSource = null;
            lst_ProgramList.Items.Clear();
            SimulatorProgram prog = programList.Where(x => x.Name.Equals(ProgramName)).FirstOrDefault();
            if (prog == null)
                return int.MinValue;
            SimulatorProgram newProgram = CreateNewProgram(prog.Name + "_inst", prog.BaseAddress, prog.Pages);
            programList.Add(newProgram);
            lst_ProgramList.ItemsSource = programList;
            return await UpdateInterface();
        }
    }
}