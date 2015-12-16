using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using CPU_OS_Simulator.Console;
using Color = System.Windows.Media.Color;
using FontStyle = System.Drawing.FontStyle;

namespace CPU_OS_Simulator
{
    /// <summary>
    /// Interaction logic for ConsoleWindow.xaml
    /// </summary>
    public partial class ConsoleWindow : Window
    {
        private MainWindow parent;
        private Color textColor = Colors.Black;
        private string fontName = String.Empty;
        private int fontSize = 12;
        private int fontStyles;

        /// <summary>
        /// The current active instance of the console window
        /// </summary>
        public static ConsoleWindow currentInstance;

        /// <summary>
        /// Default Constructor for console window
        /// </summary>
        public ConsoleWindow()
        {
            InitializeComponent();
            currentInstance = this;
            SetConsoleWindowInstance();
        }

        /// <summary>
        /// Constructor for console window that takes the window instance that is creating this window
        /// PLEASE NOTE: This constructor should always be used so data can be passed back to the main window
        /// </summary>
        /// <param name="parent">The window that is creating this window </param>
        public ConsoleWindow(MainWindow parent)
        {
            this.parent = parent;
            InitializeComponent();
            currentInstance = this;
            SetConsoleWindowInstance();
        }

        public Color TextColor
        {
            get { return textColor; }
            set { textColor = value; }
        }

        public string FontName
        {
            get { return fontName; }
            set { fontName = value; }
        }

        public int FontSizes
        {
            get { return fontSize; }
            set { fontSize = value; }
        }

        public int FontStyles
        {
            get { return fontStyles; }
            set { fontStyles = value; }
        }

        /// <summary>
        /// This function parses a line of input into a console command
        /// </summary>
        /// <param name="text"> the line of input to parse</param>
        private void ParseInput(string text)
        {
            string[] input = text.Split('\n');
            string inputLine = input[input.Length - 1];
            if (inputLine.StartsWith("//"))
            {
                //txt_Console.Clear();
                inputLine = inputLine.Substring(2);
                List<string> splitInputStrings = inputLine.Split(' ').ToList();
                string commandString = splitInputStrings[0];
                splitInputStrings.RemoveAt(0);
                ConsoleCommand command = new ConsoleCommand(commandString, splitInputStrings.ToArray());
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
            else
            {
                txt_Console.Text += "\n";
                txt_Console.CaretIndex = txt_Console.Text.Length;
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
            ColourPickerWindow window = new ColourPickerWindow(this);
            window.Show();
        }

        private void ConsoleWindow1_Closing(object sender, CancelEventArgs e)
        {
            currentInstance = null;
            SetConsoleWindowInstance();
        }

        /// <summary>
        /// This method sets the console window instance in the window bridge o it can be accessed by other modules
        /// </summary>
        private void SetConsoleWindowInstance()
        {
            Assembly windowBridge = Assembly.LoadFrom("CPU_OS_Simulator.WindowBridge.dll"); // Load the window bridge module
            System.Console.WriteLine(windowBridge.GetExportedTypes()[0]);
            Type WindowType = windowBridge.GetType(windowBridge.GetExportedTypes()[0].ToString()); // get the name of the type that contains the window instances
            WindowType.GetField("ConsoleWindowInstance").SetValue(null, currentInstance);
        }

        private void btn_Print_Click(object sender, RoutedEventArgs e)
        {
            string textToPrint = txt_Console.Text;
            if (fontName == String.Empty)
            {
                fontName = "Consolas";
                fontStyles = 0;
                fontSize = 12;
            }
            textColor = ((SolidColorBrush)txt_Console.Foreground).Color;
            PrintableDocument printableDocument = new PrintableDocument();
            Font f = new Font(fontName, fontSize, (FontStyle)fontStyles);
            printableDocument.PrintPage += delegate (object o, PrintPageEventArgs args)
            {
                args.Graphics.DrawString(textToPrint, f, new SolidBrush(System.Drawing.Color.Black), 0, 0);
            };
#if DEBUG
            printableDocument.PrinterSettings.PrintToFile = true;
            printableDocument.PrinterSettings.PrintFileName = "Test File \n";
            txt_Console.Text += "\n Printing To File: " + printableDocument.PrinterSettings.PrintFileName + "\n";
            txt_Console.CaretIndex = txt_Console.Text.Length;
            printableDocument.Print();
#else
            printableDocument.PrinterSettings.PrintToFile = false;
            printableDocument.Print();
#endif
        }

        private void btn_Clear_Click(object sender, RoutedEventArgs e)
        {
            txt_Console.Clear();
            txt_Console.CaretIndex = 0;
        }

        private void btn_Close_Click(object sender, RoutedEventArgs e)
        {
            currentInstance = null;
            SetConsoleWindowInstance();
            Close();
        }

        private void btn_SetFonts_Click(object sender, RoutedEventArgs e)
        {
            FontPickerWindow window = new FontPickerWindow(this);
            window.Show();
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