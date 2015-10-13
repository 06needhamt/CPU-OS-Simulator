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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Reflection;
using System.Diagnostics;
using CPU_OS_Simulator.CPU;
using System.Xml;
using System.IO;
using System.Web.Script.Serialization;
using Microsoft.Win32;

namespace CPU_OS_Simulator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 
    public partial class MainWindow : Window
    {
        List<SimulatorProgram> programList;
        string InstructionMode;
        string currentProgram = string.Empty;

        public MainWindow()
        {
            InitializeComponent();
            programList = new List<SimulatorProgram>();
        }

        private void MainWindow2_Loaded(object sender, RoutedEventArgs e)
        {
            this.Title += " " + GetProgramVersion();
            #if DEBUG
            this.Title += " DEBUG BUILD ";
            #endif
        }
        /// <summary>
        /// Geta the build number of the running program
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
                programList.Add(prog);
            }
        }
        /// <summary>
        /// Creates a new program based on entered data
        /// </summary>
        /// <returns>the created program</returns>
        private SimulatorProgram CreateNewProgram()
        {
            if(txtProgramName.Text == "")
            {
                MessageBox.Show("Please Enter a Program Name");
                return null;
            }
            else if(txtBaseAddress.Text == "")
            {
                MessageBox.Show("Please Enter a Base Address");
                return null;
            }
            string programName = txtProgramName.Text;
            Int32 baseAddress = Convert.ToInt32(txtBaseAddress.Text);
            Int32 pages = Convert.ToInt32(cmb_Pages.Text);
            SimulatorProgram program = new SimulatorProgram(programName, baseAddress, pages);
            lst_ProgramList.Items.Add(program);
            programList.Add(program);
            currentProgram = program.Name;
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
            InstructionMode = "Show";
            InstructionsWindow iw = new InstructionsWindow(this);
            iw.Show();
        }
        /// <summary>
        /// Called when the window is closing
        /// </summary>
        /// <param name="sender">the object that initiated the event</param>
        /// <param name="e"> the eventargs</param>
        private void MainWindow2_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Stopping the Simulator Continue?", "Stopping", MessageBoxButton.YesNo, MessageBoxImage.Information);
            if(result == MessageBoxResult.Yes)
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
            return new Instruction((int) opcode, op1, op2,Size);
        }
        /// <summary>
        /// Creates an instruction with 1 operand
        /// </summary>
        /// <param name="opcode"> the instruction opcode</param>
        /// <param name="op1"> the first operand</param>
        /// <param name="Size"> the size of the instruction</param>
        public Instruction CreateInstruction(EnumOpcodes opcode, Operand op1, Int32 Size)
        {
            return new Instruction((int)opcode, op1,Size);
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
        public void AddInstruction(Instruction ins)
        {
            if (ins != null)
            {
                //string currentProgramName = ((SimulatorProgram) lst_ProgramList.SelectedItem).Name;
                if(lst_ProgramList.Items.Count == 0)
                {
                    MessageBox.Show("Please Create a program before adding instructions");
                    return;
                }
                SimulatorProgram prog = programList.Where(x => x.Name.Equals(currentProgram)).FirstOrDefault(); // find the currently active program
                prog.Instructions.Add(ins); // add the instruction
                //Console.WriteLine(node.Value.Instructions.Count);
                UpdateIntructions(); // update the instruction list
            }
        }

        private void lst_InstructionsList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            
        }

        private void lst_ProgramList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateIntructions();
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
            lst_InstructionsList.ItemsSource = null; // WHY item source must be set to null when modifying the items within the list
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
            lst_InstructionsList.ItemsSource = prog.Instructions; // load the instructions into the instruction list
            Console.WriteLine(lst_InstructionsList.Items.Count);
        }

        private void btn_Load_Click(object sender, RoutedEventArgs e)
        {
            bool loaded = LoadProgram(); // load a program file 
            if (!loaded)
            {
                throw new Exception("An error occured while loading the program");
            }
            Console.WriteLine("Program Loaded Successfully");
        }
        private void btn_Save_Click(object sender, RoutedEventArgs e)
        {
            bool saved = SaveProgram(); //save a program file
            if (!saved)
            {
                throw new Exception("An error occured while saving the program");
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
                SimulatorProgram[] progs = programList.ToArray();
                for(int i = 0; i < progs.Length; i++)
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
        /// <param name="serializableObject"> the object to serialise</param>
        /// <param name="fileName">the file to save the objects to</param>
        public void SerializeObject<T>(T serializableObject, string filePath)
        {
            if (serializableObject == null) { return; }
            if (File.Exists(filePath))
            {
                File.Delete(filePath); // ensure we create a new file when we overwrite
            }
            StreamWriter writer = new StreamWriter(filePath, true); // initialise a file writer
            JavaScriptSerializer serializer = new JavaScriptSerializer(); // initialise a serializer
            string json = serializer.Serialize(serializableObject); // serialise the object
            writer.WriteLine(json); // write the object to the file
            writer.Flush();
            writer.Close();
            writer.Dispose(); // flush close and dispose of the writer
        }


        /// <summary>
        /// Deserializes an .sas file into a program list
        /// </summary>
        /// <typeparam name="T">The type to deserialise</typeparam>
        /// <param name="fileName"> the name of the file to load the objects from</param>
        public void DeSerializeObject<T>(string fileName)
        {
            if (string.IsNullOrEmpty(fileName)) { return; }
            JavaScriptSerializer deserializer = new JavaScriptSerializer(); // initialise the deserializer
            StreamReader reader = new StreamReader(fileName); // initialise file reader
            string json;

            while ((json = reader.ReadLine()) != null) // while there are lines to read
            {
                SimulatorProgram prog = deserializer.Deserialize<SimulatorProgram>(json); // deserialise the line into a object
                lst_ProgramList.Items.Add(prog); // add the object to the program list
            }

            return;
        }

    
    }
}
