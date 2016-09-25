using System;
using System.Linq;
using System.Windows;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using CPU_OS_Simulator.Compiler.Frontend.AST;
using CPU_OS_Simulator.Compiler.Symbols;

namespace CPU_OS_Simulator.Compiler.Frontend
{
    public class Parser
    {
        private TokenRegistry tokens;
        private SymbolRegistry symbols;
        private int index = 0;
        private AST.AST ast;
        private BaseASTNode pCurrentASTNode;
        private EnumErrorCodes error = EnumErrorCodes.NO_ERROR;

        public Parser(TokenRegistry tokens)
        {
            this.tokens = tokens;
            this.symbols = new SymbolRegistry();
            this.ast = new AST.AST();
        }

        public bool ParseTokens()
        {
            while (index < tokens.RegisteredTokens.Count)
            {
                if (tokens.RegisteredTokens[index].Type == EnumTokenType.END_OF_FILE_TOKEN)
                    return true;
                if (!ParseProgramDefinition(out pCurrentASTNode) && pCurrentASTNode == null 
                    && symbols.RegisteredSymbols.All(x => x.Type != EnumSymbolType.PROGRAM))
                    return false;
                if (!ParseVariableDeclaration(out pCurrentASTNode) && pCurrentASTNode == null)
                    return false;
                AdvanceWhitespace();
            }
            return true;
        }

        private bool ParseProgramDefinition(out BaseASTNode pCurrentASTNode)
        {
            ProgramDeclarationASTNode node = null;
            Token name = null;
            pCurrentASTNode = null;
            if (tokens.RegisteredTokens[index].Type == EnumTokenType.KEYWORD_TOKEN &&
                tokens.RegisteredTokens[index].Value.Equals("program"))
            {
                Advance();
                AdvanceWhitespace();
                if (tokens.RegisteredTokens[index].Type == EnumTokenType.IDENTIFIER_TOKEN)
                {
                    name = tokens.RegisteredTokens[index];
                    node = new ProgramDeclarationASTNode(name.Value);
                    pCurrentASTNode = (BaseASTNode) node;
                    Symbol sym = new Symbol(name.Value, EnumSymbolType.PROGRAM, null);
                    symbols.RegisterSymbol(ref sym);
                    Advance();
                    AdvanceWhitespace();
                    return true;
                }
            }
            pCurrentASTNode = null;
            return false;
        }

        private Token AdvanceWhitespace()
        {
            while (tokens.RegisteredTokens[index].Type == EnumTokenType.WHITESPACE_TOKEN)
                Advance();
            return tokens.RegisteredTokens[index];
        }

        private bool ParseVariableDeclaration(out BaseASTNode pCurrentASTNode)
        {
            VariableDeclarationASTNode node = null;
            Token name = null;
            Token type = null;

            pCurrentASTNode = null;
            if (tokens.RegisteredTokens[index].Type != EnumTokenType.KEYWORD_TOKEN &&
                !tokens.RegisteredTokens[index].Value.Equals("var"))
                return false;
            Advance();
            AdvanceWhitespace();
            if (tokens.RegisteredTokens[index].Type != EnumTokenType.IDENTIFIER_TOKEN)
                return false;
            name = tokens.RegisteredTokens[index];
            Advance();
            AdvanceWhitespace();
            if (tokens.RegisteredTokens[index].Type == EnumTokenType.OPERATOR_TOKEN)
            {
                if (!ParseValueExpression(out pCurrentASTNode))
                    return false;
            }

            if (tokens.RegisteredTokens[index].Type != EnumTokenType.TYPE_TOKEN)
                return false;
            type = tokens.RegisteredTokens[index];
            Advance();
            AdvanceWhitespace();
            node = new VariableDeclarationASTNode(name.Value, type.Value, pCurrentASTNode);
            pCurrentASTNode = (BaseASTNode) node;
            
            return true;
        }

        private bool ParseValueExpression(out BaseASTNode pCurrentAstNode)
        {
            Token @operator = tokens.RegisteredTokens[index];
            Token value = null;
            Token type = null;
            EnumTypes outT;
            ValueExpressionASTNode node = null;
            pCurrentAstNode = null;
            Advance();
            AdvanceWhitespace();
            switch (@operator.Value)
            {
                case "=":
                {
                    if (tokens.RegisteredTokens[index].Type == EnumTokenType.IDENTIFIER_TOKEN)
                    {
                        Advance(-2);
                        if (!ParseReferenceExpression(out pCurrentAstNode))
                            return true;
                    }
                    else if ((tokens.RegisteredTokens[index].Type &
                                    (EnumTokenType.INTEGER_LITERAL | EnumTokenType.STRING_LITERAL)) > 0)
                    {
                        value = tokens.RegisteredTokens[index];
                    }
                    Advance();
                    AdvanceWhitespace();
                    if (tokens.RegisteredTokens[index].Type == EnumTokenType.OPERATOR_TOKEN)
                    {
                        //TODO Implement Chained Expressions
                        throw new NotImplementedException("Chained Expressions Are Not Implemented Yet");

                    }
                    else if (tokens.RegisteredTokens[index].Type == EnumTokenType.TYPE_TOKEN)
                    {
                        type = tokens.RegisteredTokens[index];

                    }
                    if (Enum.TryParse<EnumTypes>(type.Value, true, out outT))
                    {
                        string ty = outT.ToString().ToLower();
                        if (ty.Equals(type.Value))
                        {
                            node = new ValueExpressionASTNode(outT, value.Value, pCurrentAstNode);
                        }
                        else
                        {
                            ThrowError(EnumErrorCodes.INCORRECT_TYPE, "Expected: " + type.Value + " Found: " + ty);
                            return false;
                        }
                    }
                    pCurrentAstNode = (BaseASTNode) node;
                    break;
                }
            }
            return true;
        }

        private void ThrowError(EnumErrorCodes errorCode, string errorMessage)
        {
            MessageBox.Show("Compile Error: " + errorCode.ToString() + " " + errorMessage);
        }

        private bool ParseReferenceExpression(out BaseASTNode pCurrentAstNode)
        {
            throw new NotImplementedException();
        }

        public Token Advance()
        {
            return Advance(1);
        }

        public Token Advance(int amount)
        {
            index += amount;
            if (index >= tokens.RegisteredTokens.Count)
            {
                MessageBox.Show("Reached End Of File");
                return null;
            }
            return tokens.RegisteredTokens[index];
        }
    }
}