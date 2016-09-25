using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Documents;

namespace CPU_OS_Simulator.Compiler.Frontend
{
    public class TokenRegistry
    {
        public readonly List<Token> predefinedTokens = new Token[]
        {
                new Token("program",EnumTokenType.KEYWORD_TOKEN),
                new Token("end",EnumTokenType.KEYWORD_TOKEN),
                new Token("while",EnumTokenType.KEYWORD_TOKEN),
                new Token("wend",EnumTokenType.KEYWORD_TOKEN),
                new Token("for",EnumTokenType.KEYWORD_TOKEN),
                new Token("next",EnumTokenType.KEYWORD_TOKEN),
                new Token("if",EnumTokenType.KEYWORD_TOKEN),
                new Token("else",EnumTokenType.KEYWORD_TOKEN),
                new Token("then",EnumTokenType.KEYWORD_TOKEN),
                new Token("select",EnumTokenType.KEYWORD_TOKEN),
                new Token("default",EnumTokenType.KEYWORD_TOKEN),
                new Token("thread",EnumTokenType.KEYWORD_TOKEN),
                new Token("synchronise",EnumTokenType.KEYWORD_TOKEN),
                new Token("enter",EnumTokenType.KEYWORD_TOKEN),
                new Token("leave",EnumTokenType.KEYWORD_TOKEN),
                new Token("continue",EnumTokenType.KEYWORD_TOKEN),
                new Token("break",EnumTokenType.KEYWORD_TOKEN),
                new Token("true",EnumTokenType.KEYWORD_TOKEN),
                new Token("false",EnumTokenType.KEYWORD_TOKEN),
                new Token("resource",EnumTokenType.KEYWORD_TOKEN),
                new Token("sub",EnumTokenType.KEYWORD_TOKEN),
                new Token("fun",EnumTokenType.KEYWORD_TOKEN),
                new Token("do",EnumTokenType.KEYWORD_TOKEN),
                new Token("loop",EnumTokenType.KEYWORD_TOKEN),
                new Token("read",EnumTokenType.KEYWORD_TOKEN),
                new Token("write",EnumTokenType.KEYWORD_TOKEN),
                new Token("var",EnumTokenType.KEYWORD_TOKEN),
                new Token("ret",EnumTokenType.KEYWORD_TOKEN),
                new Token("call",EnumTokenType.KEYWORD_TOKEN),
                new Token("goto",EnumTokenType.KEYWORD_TOKEN),
                new Token("as",EnumTokenType.KEYWORD_TOKEN),
                new Token("integer",EnumTokenType.TYPE_TOKEN),
                new Token("boolean",EnumTokenType.TYPE_TOKEN),
                new Token("byte",EnumTokenType.TYPE_TOKEN),
                new Token("object",EnumTokenType.TYPE_TOKEN),
                new Token("string",EnumTokenType.TYPE_TOKEN),
                new Token("array",EnumTokenType.KEYWORD_TOKEN),
                new Token("+", EnumTokenType.OPERATOR_TOKEN),
                new Token("-", EnumTokenType.OPERATOR_TOKEN),
                new Token("*", EnumTokenType.OPERATOR_TOKEN),
                new Token("/", EnumTokenType.OPERATOR_TOKEN),
                new Token("%", EnumTokenType.OPERATOR_TOKEN),
                new Token("==", EnumTokenType.OPERATOR_TOKEN),
                new Token("!=", EnumTokenType.OPERATOR_TOKEN),
                new Token("&", EnumTokenType.OPERATOR_TOKEN),
                new Token("|", EnumTokenType.OPERATOR_TOKEN),
                new Token("~", EnumTokenType.OPERATOR_TOKEN),
                new Token("^", EnumTokenType.OPERATOR_TOKEN),
                new Token("=", EnumTokenType.OPERATOR_TOKEN),
                new Token("(",EnumTokenType.LEFT_PAREN_TOKEN),
                new Token(")", EnumTokenType.RIGHT_PAREN_TOKEN),
                new Token("[", EnumTokenType.LEFT_SQUARE_PAREN_TOKEN),
                new Token("]", EnumTokenType.RIGHT_SQUARE_PAREN_TOKEN),
                new Token("{", EnumTokenType.LEFT_BRACE_TOKEN),
                new Token("}", EnumTokenType.RIGHT_BRACE_TOKEN),
                new Token("\n", EnumTokenType.WHITESPACE_TOKEN),
                new Token("\r", EnumTokenType.WHITESPACE_TOKEN),
                new Token(" ", EnumTokenType.WHITESPACE_TOKEN), 
                new Token(",", EnumTokenType.COMMA_TOKEN),
                new Token(".", EnumTokenType.DOT_TOKEN),
                new Token("\t", EnumTokenType.WHITESPACE_TOKEN),
                new Token("", EnumTokenType.END_OF_FILE_TOKEN),
                new Token("\0", EnumTokenType.END_OF_FILE_TOKEN), 
        }.ToList();

        private List<Token> registeredTokens = new List<Token>();

        public TokenRegistry()
        {

        }

        public bool RegisterToken(string value)
        {
            Token temp = predefinedTokens.FirstOrDefault(x => x.Value.Equals(value));

            if (temp != null)
            {
                registeredTokens.Add(temp);
            }
            else
            {
                double outD = 0.0;
                if(double.TryParse(value,out outD))
                    temp = new Token(value,EnumTokenType.INTEGER_LITERAL);
                else if(value.StartsWith("\"") && value.EndsWith("\""))
                    temp = new Token(value,EnumTokenType.STRING_LITERAL);
                else if (!String.IsNullOrEmpty(value))
                    temp = new Token(value, EnumTokenType.IDENTIFIER_TOKEN);
                else
                {
                    MessageBox.Show("Found Unknown Token ", value);
                    return false;
                }
                registeredTokens.Add(temp);
            }
          
            return true;
        }

        public List<Token> PredefinedTokens
        {
            get { return predefinedTokens; }
        }

        public List<Token> RegisteredTokens
        {
            get { return registeredTokens; }
            set { registeredTokens = value; }
        }
    }
}