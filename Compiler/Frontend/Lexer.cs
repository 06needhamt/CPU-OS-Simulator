using System;
using System.Text;
using System.Windows;

namespace CPU_OS_Simulator.Compiler.Frontend
{
    public class Lexer
    {
        private readonly string sourceString;
        private TokenRegistry registry;

        public Lexer(string sourceString)
        {
            this.sourceString = sourceString;
            this.registry = new TokenRegistry();
        }

        public bool TokeniseSourceCode()
        {
            char[] sourceChars = sourceString.ToCharArray();
            int idx = 0;
            StringBuilder currentToken = new StringBuilder();
            while (idx < sourceChars.Length - 1)
            {
                if(!isControlCharacter(sourceChars[idx]))
                {
                    if (sourceChars[idx] == '\r')
                    {
                        idx++;
                        continue;
                    }
                    currentToken.Append(sourceChars[idx]);
                    idx++;
                }
                else
                {
                    if (!TerminateString(ref idx, ref currentToken, ref sourceChars))
                    {
                        MessageBox.Show("Error: Unterminated String Literal " + currentToken.ToString());
                        return false;
                    }
                    if(!currentToken.ToString().Equals(string.Empty))
                        RegisterTokenToRegistry(ref registry, currentToken.ToString());
                    RegisterTokenToRegistry(ref registry, sourceChars[idx].ToString());
                    currentToken.Clear();
                    idx++;
                }
            }
            return true;
        }

        private bool isControlCharacter(char ch)
        {
            return !(ch != ' ' && ch != '\n' &&
                   ch != '\t' && ch != '(' &&
                   ch != ')' && ch != '[' &&
                   ch != ']' && ch != '{' &&
                   ch != '}');
        }

        private bool TerminateString(ref int idx, ref StringBuilder currentToken, ref char[] sourceChars)
        {
            if (!String.IsNullOrWhiteSpace(currentToken.ToString()))
            {
                if (currentToken.ToString().StartsWith("\"") && !currentToken.ToString().EndsWith("\""))
                {
                    while (!currentToken.ToString().EndsWith("\""))
                    {
                        if (idx > sourceChars.Length - 1)
                            return false;
                        
                        currentToken.Append(sourceChars[idx]);
                        idx++;
                    }
                }
            }
            return true;
        }

        private void RegisterTokenToRegistry(ref TokenRegistry tokenRegistry, string token)
        {
            if(!String.IsNullOrEmpty(token))
                tokenRegistry.RegisterToken(token);
        }

        public string SourceString
        {
            get { return sourceString; }
        }

        public TokenRegistry Registry
        {
            get { return registry; }
            set { registry = value; }
        }
    }
}