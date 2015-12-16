using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using CPU_OS_Simulator.Compiler.Frontend.Tokens;

namespace CPU_OS_Simulator.Compiler.Frontend
{
    /// <summary>
    /// This class represents the lexer for the simulator programming language
    /// the lexer converts the source code into tokens for parsing to an Abstract Syntax Tree
    /// </summary>
    public class Lexer
    {
        #region Global Variables
        private readonly string sourceString;

        private LinkedList<string> tokenStrings;
        private LinkedListNode<string> currentTokenString;
        private LinkedListNode<string> nextTokenString;
        private LinkedListNode<string> previousTokenString;

        private LinkedList<Token> tokens;
        private LinkedListNode<Token> currentToken;
        private LinkedListNode<Token> nextToken;
        private LinkedListNode<Token> previousToken;

        private bool writingToCompilerTester = false;
        private string error;
        private TextBox output = null;
        #endregion GlobalVariables

        #region Constructors
        public Lexer(string sourceString)
        {
            this.sourceString = sourceString;
            this.tokens = new LinkedList<Token>();
        }

        public bool WritingToCompilerTester
        {
            get { return writingToCompilerTester; }
            set { writingToCompilerTester = value; }
        }

        public TextBox Output
        {
            get { return output; }
            set { output = value; }
        }

        public string Error
        {
            get { return error; }
            set { error = value; }
        }

        #endregion Constructors

        #region Methods

        public bool Start()
        {
            tokens = GenerateTokens();
            IdentifyUnknownTokens();
            PrintTokens();
            //CheckForErrors();
            return CheckForErrors();
            //return true;

        }

        private bool CheckForErrors()
        {
            currentToken = tokens.First;
            nextToken = currentToken.Next;
            previousToken = currentToken.Previous;
            if ((EnumKeywordType) tokens.First.Value.Type != EnumKeywordType.PROGRAM)
            {
                ThrowError(EnumErrorCodes.EXPECTED_A_KEYWORD, "program");
                return false;
            }
            while (currentToken.Next != null)
            {
                if ((EnumKeywordType) currentToken.Value.Type == EnumKeywordType.PROGRAM &&
                    ((EnumTokenType) nextToken.Value.Type != EnumTokenType.IDENTIFIER))
                {
                    ThrowError(EnumErrorCodes.EXPECTED_AN_IDENTIFIER, "Program Name");
                    return false;
                }
                if ((EnumKeywordType) currentToken.Value.Type == EnumKeywordType.SUB &&
                    (EnumKeywordType) previousToken.Value.Type != EnumKeywordType.END)
                {
                    if ((EnumKeywordType) currentToken.Value.Type == EnumKeywordType.SUB &&
                    ((EnumTokenType) nextToken.Value.Type != EnumTokenType.IDENTIFIER))
                    {
                        ThrowError(EnumErrorCodes.EXPECTED_AN_IDENTIFIER, "Subroutine name");
                        return false;
                    }

                }
                if ((EnumKeywordType)currentToken.Value.Type == EnumKeywordType.FUN &&
                    (EnumKeywordType)previousToken.Value.Type != EnumKeywordType.END)
                {
                    if ((EnumKeywordType)currentToken.Value.Type == EnumKeywordType.FUN &&
                    ((EnumTokenType)nextToken.Value.Type != EnumTokenType.IDENTIFIER))
                    {
                        ThrowError(EnumErrorCodes.EXPECTED_AN_IDENTIFIER, "Function name");
                        return false;
                    }

                }
                currentToken = nextToken;
                nextToken = currentToken.Next;
                previousToken = currentToken.Previous;
            }
            if ((EnumKeywordType)currentToken.Value.Type != EnumKeywordType.END)
            {
                ThrowError(EnumErrorCodes.EXPECTED_A_KEYWORD, "end");
                return false;
            }

            return true;
        }

        private void ThrowError(EnumErrorCodes enumErrorCodes, string expectedToken)
        {
            error = "Error: " + enumErrorCodes.ToString() + " " + expectedToken;
            return;
        }

        private LinkedList<Token> GenerateTokens()
        {
            string[] temp = sourceString.Split(new char[] { '\n', '\r', '\t', ' ' });
            temp = temp.Where(x => !String.IsNullOrEmpty(x)).ToArray();
            tokenStrings = new LinkedList<string>(temp);
            foreach (string tokenString in tokenStrings)
            {
                Token token = new GenericToken(tokenString);
                if (token.isOperator())
                {
                    Operator op = new Operator(tokenString);
                    op.Type = op.DetectType();
                    tokens.AddLast(new LinkedListNode<Token>(op));
                }
                else if (token.isKeyword())
                {
                    Keyword keyword = new Keyword(tokenString);
                    keyword.Type = keyword.DetectType();
                    tokens.AddLast(new LinkedListNode<Token>(keyword));
                }
                else if (token.isType())
                {
                    Typename typename = new Typename(tokenString);
                    typename.Type = typename.DetectType();
                    tokens.AddLast(new LinkedListNode<Token>(typename));
                }
                else
                {
                    token.Type = token.DetectType();
                    tokens.AddLast(new LinkedListNode<Token>(token));
                }

                
            }
            return tokens;
        }

        public void IdentifyUnknownTokens()
        {
            currentToken = tokens.First;
            while (currentToken.Next != null)
            {
                previousToken = currentToken.Previous;
                nextToken = currentToken.Next;
                if ((EnumTokenType) currentToken.Value.Type == EnumTokenType.UNKNOWNN)
                {
                    if (previousToken != null)
                    {
                        if ((EnumKeywordType) previousToken?.Value.Type == EnumKeywordType.VAR
                            || (EnumKeywordType) previousToken?.Value.Type == EnumKeywordType.PROGRAM
                            || (EnumKeywordType) previousToken?.Value.Type == EnumKeywordType.FUN
                            || (EnumKeywordType) previousToken?.Value.Type == EnumKeywordType.SUB
                            || (EnumKeywordType) previousToken?.Value.Type == EnumKeywordType.GOTO
                            || (EnumKeywordType) previousToken?.Value.Type == EnumKeywordType.CALL)
                        {
                            currentToken.Value.Type = EnumTokenType.IDENTIFIER;
                        }
                    }

                }
                currentToken = nextToken;
            }
        }

        public void PrintTokens()
        {
            currentToken = tokens.First;
            nextToken = currentToken.Next;
            previousToken = currentToken.Previous;

            if (writingToCompilerTester)
            {
                if (output != null)
                {
                    while (currentToken?.Next != null)
                    {
                        output.Text += currentToken.Value.ToString() + "\r";
                        previousToken = currentToken;
                        currentToken = nextToken;
                        nextToken = currentToken.Next;
                    }
                }

            }
            else
            {
                CreateTokensFile("Tokens.tokens");
                StreamWriter writer = new StreamWriter("Tokens.tokens", false);
                while (currentToken != null)
                {
                    writer.Write(currentToken.Value.ToString());
                    previousToken = currentToken;
                    currentToken = nextToken;
                    nextToken = currentToken.Next;
                }
                writer.Flush();
                writer.Close();
                writer.Dispose();
            }
        }

        private void CreateTokensFile(string filename)
        {
            if (File.Exists(filename))
            {
                File.Delete(filename);
            }
            File.Create(filename);
        }

        #endregion Methods

    }
}
