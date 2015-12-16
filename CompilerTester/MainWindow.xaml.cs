using System;
using System.Windows;
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
            Close();
        }

        private void btn_Compile_Click(object sender, RoutedEventArgs e)
        {
            //CompilerMain compiler = new CompilerMain(new SourceFile(ref txt_Input));
            SourceFile source = new SourceFile(ref txt_Input);
            Lexer lexical = new Lexer(source.FileContents);
            lexical.WritingToCompilerTester = true;
            lexical.Output = txt_Output;
            txt_Output.Text = String.Empty;
            if (!lexical.Start())
            {
                MessageBox.Show("Lexer Error Occurred: " + lexical.Error + " Compilation Terminated");
            }
            else
            {
                MessageBox.Show("Lexing Completed Successfully");
            }

        }
    }
}
