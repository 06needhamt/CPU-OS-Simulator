using System;
using System.Windows;
using System.IO;
using System.Text;
using Microsoft.Win32;
using Newtonsoft.Json;// See Third Party Libs/Credits.txt for licensing information for JSON.Net
using Newtonsoft.Json.Linq;

namespace CPU_OS_Simulator.Save_File_Editor
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string filePath;
        private JObject obj;
        private StreamReader file;
        private JsonTextReader reader;
        private JObject obj2;
        private StreamWriter writer;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void OpenItem_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Multiselect = false;
            ofd.DefaultExt = "*.sas";
            ofd.Filter = "Simulator Programs (.sas)|*.sas | Simulator OS State (.soss)|*.soss"; // Filter files by extension
            bool? result = ofd.ShowDialog(this);
            if (result.Value)
            {
                filePath = ofd.FileName;
                SaveFileParser parser = new SaveFileParser(filePath);
                parser.ParseFile(filePath); // parse the file
                parser.Dispose();
            }
        }

        private void btn_Open_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
