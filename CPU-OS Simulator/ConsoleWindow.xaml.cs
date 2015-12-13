using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
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
using CPU_OS_Simulator.Console;

namespace CPU_OS_Simulator
{
    /// <summary>
    /// Interaction logic for ConsoleWindow.xaml
    /// </summary>
    public partial class ConsoleWindow : Window
    {
        private MainWindow parent = null;
        public static ConsoleWindow currentInstance;

        public ConsoleWindow()
        {
            InitializeComponent();
            currentInstance = this;
            SetConsoleWindowInstance();
        }

        public ConsoleWindow(MainWindow window)
        {
            parent = window;
            InitializeComponent();
            currentInstance = this;
            SetConsoleWindowInstance();
        }

        private void ParseInput(String text)
        {
            string[] input = text.Split(new char[] {'\n'});
            string inputLine = input[input.Length - 1];
            if (inputLine.StartsWith("//"))
            {
                //txt_Console.Clear();
                inputLine = inputLine.Substring(2);
                List<string> splitInputStrings = inputLine.Split(new char[] {' '}).ToList();
                string commandString = splitInputStrings[0];
                splitInputStrings.RemoveAt(0);
                ConsoleCommand command = new ConsoleCommand(commandString,splitInputStrings.ToArray());
                string result = command.Execute();
                if (result.Equals("\"Clear\""))
                {
                    txt_Console.Clear();
                    txt_Console.CaretIndex = txt_Console.Text.Length;
                }
                else
                {
                    txt_Console.Text += "\n" + result;
                    txt_Console.CaretIndex = txt_Console.Text.Length;
                }
            }

        }

        private void txt_Console_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                string text = txt_Console.Text;
                ParseInput(text);
                e.Handled = true;
            }
            else
            {
                e.Handled = false;
            }
        }

        private void btn_SetColour_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ConsoleWindow1_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            currentInstance = null;
            SetConsoleWindowInstance();
        }

        private void SetConsoleWindowInstance()
        {
            Assembly windowBridge = Assembly.LoadFrom("CPU_OS_Simulator.WindowBridge.dll"); // Load the window bridge module
            System.Console.WriteLine(windowBridge.GetExportedTypes()[0]);
            Type WindowType = windowBridge.GetType(windowBridge.GetExportedTypes()[0].ToString()); // get the name of the type that contains the window instances
            WindowType.GetField("ConsoleWindowInstance").SetValue(null,currentInstance);
        }



        //private void txt_Console_KeyDown(object sender, KeyEventArgs e)
        //{
        //    if (e.Key == Key.Enter)
        //    {
        //        ParseInput();
        //        e.Handled = true;
        //    }
        //    else
        //    {
        //        //KeyConverter kc = new KeyConverter();
        //        //string keyString = kc.ConvertToString(e.Key);
        //        //txt_Console.Text += keyString;
        //        e.Handled = false;
        //    }
        //}

    }
}
