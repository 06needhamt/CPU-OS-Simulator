using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Navigation;
using CPU_OS_Simulator.Compiler.Frontend.Tokens;
using static CPU_OS_Simulator.Compiler.Frontend.Tokens.EnumKeywordType;
using static CPU_OS_Simulator.Compiler.Frontend.EnumErrorCodes;
using static CPU_OS_Simulator.Compiler.Frontend.Tokens.EnumTokenType;

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

        private bool writingToCompilerTester;
        private string error = String.Empty;
        private List<string> warningList; 
        private bool successful;
        private TextBox output;
        private List<Tuple<string, EnumTypes, string>> variables;

        #endregion GlobalVariables

        #region Constructors
        public Lexer(string sourceString)
        {
            this.sourceString = sourceString;
            tokens = new LinkedList<Token>();
        }
        #endregion Constructors
        #region Properties
        /// <summary>
        /// Property to hold whether we are writing the compiler tester app or not
        /// </summary>
        public bool WritingToCompilerTester
        {
            get { return writingToCompilerTester; }
            set { writingToCompilerTester = value; }
        }
        /// <summary>
        /// Property to hold the text box to display lexer output to
        /// </summary>
        public TextBox Output
        {
            get { return output; }
            set { output = value; }
        }
        /// <summary>
        /// Property for the string representation of the error that occurred while lexing
        /// </summary>
        public string Error
        {
            get { return error; }
            set { error = value; }
        }
        /// <summary>
        /// Property for the linked list of tokens produced by the lexer
        /// </summary>
        public LinkedList<Token> Tokens
        {
            get { return tokens; }
        }

        public List<Tuple<string, EnumTypes, string>> Variables
        {
            get { return variables; }
            set { variables = value; }
        }

        #endregion Properties


        #region Methods
        /// <summary>
        /// This function is called to start the lexer 
        /// </summary>
        /// <returns> whether the lexer completed successfully </returns>
        public bool Start()
        {
            tokens = GenerateTokens();
            IdentifyUnknownTokens();
            PrintTokens();
            warningList = CheckForWarnings();
            PrintWarnings();
            successful = CheckForErrors();
            if (!String.IsNullOrEmpty(error))
            {
                output.Text += error + "\r";
                return false;
            }
            DefineVariables();
            return true;

        }

        public void DefineVariables()
        {
            variables = new List<Tuple<string, EnumTypes, string>>();
            currentToken = tokens.First;
            nextToken = currentToken.Next;
            previousToken = currentToken.Previous;
            while (currentToken.Next != null)
            {
                if (currentToken.Value.Value.Equals("var"))
                {
                    string name = GetVariableName(currentToken);
                    EnumTypes type = GetVariableType(currentToken);
                    dynamic value = GetVariableValue(currentToken);

                    Tuple<string,EnumTypes,string> variable = new Tuple<string, EnumTypes, string>(name,type,value);
                    //output.Text += variable.Item1 + " ";
                    //output.Text += variable.Item2.ToString() + " ";
                    //output.Text += variable.Item3 + " ";
                    //output.Text += "\n";
                    variables.Add(variable);
                }
                currentToken = nextToken;
                nextToken = currentToken.Next;
                previousToken = currentToken.Previous;

            }
        }

        //HACK Possibly Some of the worst code i have ever written
        private string GetVariableValue(LinkedListNode<Token> token)
        {
            if (!token.Next.Next.Value.Isoperator)
            {
                return "uninitialised";
            }
            else
            {
                return token.Next.Next.Next.Value.Value;
            }
        }

        private EnumTypes GetVariableType(LinkedListNode<Token> token)
        {
            if (GetVariableValue(token).Equals("uninitialised"))
            {
                return (EnumTypes) token.Next.Next.Value.Type;
            }
            return (EnumTypes) token.Next.Next.Next.Next.Value.Type;
        }

        private string GetVariableName(LinkedListNode<Token> token)
        {
            return token.Next.Value.Value;
        }

        /// <summary>
        /// This function print out any warnings produced while lexing
        /// </summary>
        private void PrintWarnings()
        {
            foreach (string warning in warningList)
            {
                if (WritingToCompilerTester)
                {
                    if (output != null)
                    {
                        output.Text += warning + "\r";
                    }
                }
            }
        }
        /// <summary>
        /// This function checks the code for any warnings
        /// </summary>
        /// <returns> a list of warnings or an empty list if there were no warnings</returns>
        private List<string> CheckForWarnings()
        {
            List<string> warnings = new List<string>();
            currentToken = tokens.First;
            nextToken = currentToken.Next;
            previousToken = currentToken.Previous;

            while (currentToken.Next != null)
            {
                //TODO implement Warning Checker
                currentToken = nextToken;
                nextToken = currentToken?.Next;
                previousToken = currentToken?.Previous;
            }
            return warnings;
        }
        /// <summary>
        /// This function checks the code for errors
        /// </summary>
        /// <returns> false if errors occurred true if no errors occurred </returns>
        private bool CheckForErrors()
        {
            currentToken = tokens.First;
            nextToken = currentToken.Next;
            previousToken = currentToken.Previous;
            if ((EnumKeywordType) tokens.First.Value.Type != PROGRAM)
            {
                ThrowError(EXPECTED_A_KEYWORD, "program");
                return false;
            }
            while (currentToken.Next != null)
            {
                if (currentToken.Value.Value.Equals("program") &&
                    ((EnumTokenType) nextToken.Value.Type != IDENTIFIER))
                {
                    ThrowError(EXPECTED_AN_IDENTIFIER, "Program Name");
                    return false;
                }
                if (currentToken.Value.Value.Equals("sub") &&
                    (EnumKeywordType) previousToken.Value.Type != END)
                {
                    if (currentToken.Value.Value.Equals("sub") &&
                    ((EnumTokenType) nextToken.Value.Type != IDENTIFIER))
                    {
                        ThrowError(EXPECTED_AN_IDENTIFIER, "Subroutine name");
                        return false;
                    }

                }
                if (currentToken.Value.Value.Equals("fun") &&
                    (EnumKeywordType)previousToken.Value.Type != END)
                {
                    if (currentToken.Value.Value.Equals("fun") &&
                    ((EnumTokenType)nextToken.Value.Type != IDENTIFIER))
                    {
                        ThrowError(EXPECTED_AN_IDENTIFIER, "Function name");
                        return false;
                    }

                }
                currentToken = nextToken;
                nextToken = currentToken?.Next;
                previousToken = currentToken?.Previous;
            }
            if ((EnumKeywordType)currentToken.Value.Type != END)
            {
                ThrowError(EXPECTED_A_KEYWORD, "end");
                return false;
            }

            return true;
        }
        /// <summary>
        /// This function creates an error message to display
        /// in a message box to the user indicating what type 
        /// of error occurred
        /// </summary>
        /// <param name="enumErrorCodes"> the error code that identifies the error</param>
        /// <param name="expectedToken"> the expected token</param>
        private void ThrowError(EnumErrorCodes enumErrorCodes, string expectedToken)
        {
            error = "Error: " + enumErrorCodes + " " + expectedToken;
        }

        private LinkedList<Token> GenerateTokens()
        {
            string[] temp = sourceString.Split('\n', '\r', '\t', ' ');
            temp = temp.Where(x => !String.IsNullOrEmpty(x)).ToArray();
            tokenStrings = new LinkedList<string>(temp);
            currentTokenString = tokenStrings.First;
            nextTokenString = currentTokenString.Next;
            previousTokenString = currentTokenString.Previous;
            while (currentTokenString != null)
            {
                Token token = new GenericToken(currentTokenString.Value);
                if (token.isOperator())
                {
                    Operator op = new Operator(currentTokenString.Value);
                    op.Type = op.DetectType();
                    tokens.AddLast(new LinkedListNode<Token>(op));
                }
                else if (token.isKeyword())
                {
                    Keyword keyword = new Keyword(currentTokenString.Value);
                    keyword.Type = keyword.DetectType();
                    tokens.AddLast(new LinkedListNode<Token>(keyword));
                }
                else if (token.isType())
                {
                    Typename typename = new Typename(currentTokenString.Value);
                    typename.Type = typename.DetectType();
                    tokens.AddLast(new LinkedListNode<Token>(typename));
                }
                else
                {
                    token.Type = token.DetectType();
                    if ((EnumTokenType) token.Type == STRING)
                    {
                        StringLiteral literal = new StringLiteral(currentTokenString.Value);
                        literal.Type = STRING_LITERAL;
                        tokens.AddLast(new LinkedListNode<Token>(literal));
                    }
                    else if ((EnumTokenType) token.Type == NUMBER)
                    {
                        NumericLiteral literal = new NumericLiteral(currentTokenString.Value);
                        literal.Type = NUMERIC_LITERAL;
                        tokens.AddLast(new LinkedListNode<Token>(literal));
                    }
                    else
                    {
                        tokens.AddLast(new LinkedListNode<Token>(token));
                    }
                }
                currentTokenString = nextTokenString;
                nextTokenString = currentTokenString?.Next;
                previousTokenString = currentTokenString?.Previous;

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
                if ((EnumTokenType) currentToken.Value.Type == UNKNOWNN)
                {
                    if (previousToken != null)
                    {
                        if ((EnumKeywordType) previousToken?.Value.Type == VAR
                            || (EnumKeywordType) previousToken?.Value.Type == PROGRAM
                            || (EnumKeywordType) previousToken?.Value.Type == FUN
                            || (EnumKeywordType) previousToken?.Value.Type == SUB
                            || (EnumKeywordType) previousToken?.Value.Type == GOTO
                            || (EnumKeywordType) previousToken?.Value.Type == CALL)
                        {
                            currentToken.Value.Type = IDENTIFIER;
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
                        output.Text += currentToken.Value + "\r";
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
