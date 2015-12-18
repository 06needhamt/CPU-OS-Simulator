using System;
using System.Collections.Generic;
using System.Windows;
using CPU_OS_Simulator.Compiler;
using CPU_OS_Simulator.Compiler.Frontend;
using CPU_OS_Simulator.Compiler.Frontend.Symbols;
using CPU_OS_Simulator.Compiler.Frontend.Tokens;

namespace CompilerTester
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private SymbolTable symbolTable;
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
                return;
            }
            MessageBox.Show("Lexing Completed Successfully");
            symbolTable = new SymbolTable(lexical.Output);
            List<Tuple<string, EnumTypes, string>> vars = lexical.Variables;
            foreach (Tuple<string, EnumTypes, string> var in vars)
            {
                Symbol s = new Symbol(var.Item1,var.Item2,var.Item3,false,false);
                symbolTable.AddSymbol(new LinkedListNode<Symbol>(s));
            }
            symbolTable.PrintSymbols();

        }
    }
}
