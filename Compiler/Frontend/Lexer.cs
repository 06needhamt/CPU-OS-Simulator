using System;
using System.Text;
using System.Windows;

namespace CPU_OS_Simulator.Compiler.Frontend
{
    public class Lexer
    {
        private readonly string sourceString;

        public Lexer(string sourceString)
        {
            this.sourceString = sourceString;
        }

        public bool TokeniseSourceCode()
        {
            char[] sourceChars = sourceString.ToCharArray();
            int idx = 0;
            StringBuilder currentToken = new StringBuilder();
            while (idx < sourceChars.Length - 1)
            {
                while (sourceChars[idx] != ' ' && sourceChars[idx] != '\n')
                {
                    currentToken.Append(sourceChars[idx]);
                    idx++;
                }
                if (!String.IsNullOrWhiteSpace(currentToken.ToString()))
                {
                    if (currentToken.ToString().StartsWith("\"") && !currentToken.ToString().EndsWith("\""))
                    {
                        while (!currentToken.ToString().EndsWith("\""))
                        {
                            if (idx > sourceChars.Length - 1)
                            {
                                MessageBox.Show("Error: Unterminated String Literal " + currentToken.ToString());
                                return false;
                            }
                            currentToken.Append(sourceChars[idx]);
                            idx++;
                        }
                    }
                }
            }
            return true;
        }
    }
}