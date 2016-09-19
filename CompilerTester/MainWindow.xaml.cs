using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Windows;
using CPU_OS_Simulator.Compiler.Frontend;


namespace CPU_OS_Simulator.CompilerTester
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //private SymbolTable symbolTable;

        /// <summary>
        /// Default Constructor for Compiler Tester Main Window
        /// </summary>
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
            // TODO Replace With New Compiler
            #region old
            ////CompilerMain compiler = new CompilerMain(new SourceFile(ref txt_Input));
            //SourceFile source = new SourceFile(ref txt_Input);
            //Lexer lexical = new Lexer(source.FileContents);
            //lexical.WritingToCompilerTester = true;
            //lexical.Output = txt_Output;
            //txt_Output.Text = String.Empty;
            //if (!lexical.Start())
            //{
            //    MessageBox.Show("Lexer Error Occurred: " + lexical.Error + " Compilation Terminated");
            //    return;
            //}
            //MessageBox.Show("Lexing Completed Successfully");
            //symbolTable = new SymbolTable(lexical.Output);
            //List<Tuple<string, EnumTypes, string>> vars = lexical.Variables;
            //List<Tuple<string, EnumTypes, string>> subs = lexical.Subroutines;
            //List<Tuple<string, EnumTypes, string>> funs = lexical.Functions;

            //foreach (Tuple<string, EnumTypes, string> var in vars)
            //{
            //    Symbol s = new Symbol(var.Item1,var.Item2,var.Item3,Scope.GLOBAL_SCOPE,false,false);
            //    symbolTable.AddSymbol(new LinkedListNode<Symbol>(s));
            //}
            //foreach (Tuple<string, EnumTypes, string> sub in subs)
            //{
            //    Subroutine subroutine = new Subroutine(sub.Item1,sub.Item2,sub.Item3,Scope.GLOBAL_SCOPE);
            //    symbolTable.AddSymbol((new LinkedListNode<Symbol>(subroutine)));
            //}
            //foreach (Tuple<string, EnumTypes, string> fun in funs)
            //{
            //    Function func = new Function(fun.Item1,fun.Item2,fun.Item3,Scope.GLOBAL_SCOPE);
            //    symbolTable.AddSymbol(new LinkedListNode<Symbol>(func));
            //}
            //symbolTable.PrintSymbols();
            #endregion old
            Lexer l = new Lexer(txt_Input.Text);
            if (!l.TokeniseSourceCode())
            {
                MessageBox.Show("Compiler Error Occurred");
            }
            #if DEBUG
                Debugger.Break();
            #endif
            MessageBox.Show("Compilation Completed Successfully");
        }


        private void CompilerTesterWindow_Loaded(object sender, RoutedEventArgs e)
        {
            #if DEBUG
                Title += " " + GetProgramVersion() + " DEBUG BUILD ";
            #else
                Title += " " + GetProgramVersion();
            #endif
        }

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

    }
}
