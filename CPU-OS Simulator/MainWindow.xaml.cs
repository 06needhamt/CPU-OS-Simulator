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

namespace CPU_OS_Simulator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 
    public partial class MainWindow : Window
    {
        LinkedList<SimulatorProgram> programList;
        string InstructionMode;

        public MainWindow()
        {
            InitializeComponent();
            programList = new LinkedList<SimulatorProgram>();
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
                programList.AddLast(prog);
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
            programList.AddLast(program);
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

        public Instruction CreateInstruction(EnumOpcodes opcode, Operand op1, Operand op2)
        {
            return new Instruction((int) opcode, op1, op2);
        }
        public Instruction CreateInstruction(EnumOpcodes opcode, Operand op1)
        {
            return new Instruction((int)opcode, op1);
        }

        public Instruction CreateInstruction(EnumOpcodes opcode)
        {
            return new Instruction((int)opcode);
        }
    }
}
