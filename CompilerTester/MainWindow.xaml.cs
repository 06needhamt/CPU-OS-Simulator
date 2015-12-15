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
using CPU_OS_Simulator.Compiler;
using CPU_OS_Simulator.Compiler.Frontend;

namespace CompilerTester
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void btn_Close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btn_Compile_Click(object sender, RoutedEventArgs e)
        {
            //CompilerMain compiler = new CompilerMain(new SourceFile(ref txt_Input));
            SourceFile source = new SourceFile(ref txt_Input);
            Lexer lexical = new Lexer(source.FileContents);
            lexical.WritingToCompilerTester = true;
            lexical.Output = txt_Output;
            txt_Output.Text = String.Empty;
            lexical.Start();

        }
    }
}
