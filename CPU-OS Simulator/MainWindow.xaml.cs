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

        private void btn_Show_Click(object sender, RoutedEventArgs e)
        {
            InstructionMode = "Show";
            InstructionsWindow iw = new InstructionsWindow(this);
            iw.Show();
        }

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
        public void SayHello()
        {
            Console.WriteLine("Hello From Main Window");
        }

        public Instruction CreateInstruction(EnumOpcodes opcode, Operand op1, Operand op2, Int32 Size)
        {
            return new Instruction((int) opcode, op1, op2,Size);
        }
        public Instruction CreateInstruction(EnumOpcodes opcode, Operand op1, Int32 Size)
        {
            return new Instruction((int)opcode, op1,Size);
        }

        public Instruction CreateInstruction(EnumOpcodes opcode, Int32 Size)
        {
            return new Instruction((int)opcode, Size);
        }

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
                SimulatorProgram prog = programList.Where(x => x.Name.Equals(currentProgram)).FirstOrDefault();
                prog.Instructions.Add(ins);
                //Console.WriteLine(node.Value.Instructions.Count);
                UpdateIntructions();
            }
        }

        private void lst_InstructionsList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            
        }

        private void lst_ProgramList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateIntructions();
        }

        private void UpdateIntructions()
        {
            if (currentProgram.Equals(string.Empty))
            {
                programList = lst_ProgramList.Items.OfType<SimulatorProgram>().ToList();
                Console.WriteLine(programList.Count);
                currentProgram = programList.First().Name;
            }
            lst_InstructionsList.Items.Clear();
            if ((lst_ProgramList.SelectedItem) == null)
            {
                currentProgram = ((SimulatorProgram)lst_ProgramList.Items.GetItemAt(0)).Name;
            }
            else
            {
                currentProgram = ((SimulatorProgram)lst_ProgramList.SelectedItem).Name;
            }
            SimulatorProgram prog = programList.Where(x => x.Name.Equals(currentProgram)).FirstOrDefault();
            //lst_InstructionsList.ItemsSource.
            lst_InstructionsList.ItemsSource = prog.Instructions;
            Console.WriteLine(lst_InstructionsList.Items.Count);
        }

        private void btn_Load_Click(object sender, RoutedEventArgs e)
        {
            bool loaded = LoadProgram();
            if (!loaded)
            {
                throw new Exception("An error occured while loading the program");
            }
            Console.WriteLine("Program Loaded Successfully");
        }
        private void btn_Save_Click(object sender, RoutedEventArgs e)
        {
            bool saved = SaveProgram();
            if (!saved)
            {
                throw new Exception("An error occured while saving the program");
            }
            Console.WriteLine("Program Saved Successfully");
        }

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
                    SerializeObject<SimulatorProgram>(progs[i], dlg.FileName);
                }
            }
            return true;
        }

        private bool LoadProgram()
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.FileName = "Program";
            ofd.DefaultExt = "*.sas";
            ofd.Filter = "Simulator Programs (.sas)|*.sas"; // Filter files by extension
            Nullable<bool> result = ofd.ShowDialog();
            if (result.Value == true)
            {
                DeSerializeObject<SimulatorProgram>(ofd.FileName);
            }
            return true;
        }

        /// <summary>
        /// Serializes an object.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="serializableObject"></param>
        /// <param name="fileName"></param>
        public void SerializeObject<T>(T serializableObject, string filePath)
        {
            if (serializableObject == null) { return; }
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
            StreamWriter writer = new StreamWriter(filePath, true);
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string json = serializer.Serialize(serializableObject);
            writer.WriteLine(json);
            writer.Flush();
            writer.Close();
            writer.Dispose();
        }


        /// <summary>
        /// Deserializes an xml file into an object list
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public T DeSerializeObject<T>(string fileName)
        {
            if (string.IsNullOrEmpty(fileName)) { return default(T); }
            JavaScriptSerializer deserializer = new JavaScriptSerializer();
            StreamReader reader = new StreamReader(fileName);
            string json;

            while ((json = reader.ReadLine()) != null)
            {
                SimulatorProgram prog = deserializer.Deserialize<SimulatorProgram>(json);
                lst_ProgramList.Items.Add(prog);
            }
            
            return default(T);
        }

    
    }
}
