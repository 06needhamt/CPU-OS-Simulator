using System;
using System.Linq;
using System.Windows;
using System.Windows.Navigation;
using CPU_OS_Simulator.Compiler.Frontend.AST;

namespace CPU_OS_Simulator.Compiler.Frontend
{
    public class Parser
    {
        private TokenRegistry tokens;
        private int index = 0;
        private AST.AST ast;
        private BaseASTNode pCurrentASTNode;
        private EnumErrorCodes error = EnumErrorCodes.NO_ERROR;

        public Parser(TokenRegistry tokens)
        {
            this.tokens = tokens;
            this.ast = new AST.AST();
        }

        public bool ParseTokens()
        {
            while (index < tokens.RegisteredTokens.Count)
            {
                if (tokens.RegisteredTokens[index].Type == EnumTokenType.END_OF_FILE_TOKEN)
                    return true;
                else if (!ParseProgramDefinition(out pCurrentASTNode))
                    return false;
                else if (!ParseVariableDeclaration(out pCurrentASTNode) && pCurrentASTNode == null)
                    return false;
                Advance();
            }
            return true;
        }

        private bool ParseProgramDefinition(out BaseASTNode baseASTNode)
        {
            throw new NotImplementedException();
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

            if (tokens.RegisteredTokens[index].Type != EnumTokenType.IDENTIFIER_TOKEN)
                return false;
            name = tokens.RegisteredTokens[index];
            Advance();

            if (tokens.RegisteredTokens[index].Type == EnumTokenType.OPERATOR_TOKEN)
            {
                Advance(-1);
                if (!ParseValueExpression(out pCurrentASTNode))
                    return false;
            }

            if (tokens.RegisteredTokens[index].Type != EnumTokenType.TYPE_TOKEN)
                return false;
            type = tokens.RegisteredTokens[index];
            Advance();
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
            switch (@operator.Value)
            {
                case "=":
                {
                    if (tokens.RegisteredTokens[index].Type == EnumTokenType.IDENTIFIER_TOKEN)
                    {
                        Advance(-1);
                        if (!ParseReferenceExpression(out pCurrentAstNode))
                            return true;
                    }
                    else if ((tokens.RegisteredTokens[index].Type &
                                    (EnumTokenType.INTEGER_LITERAL | EnumTokenType.STRING_LITERAL)) > 0)
                    {
                        value = tokens.RegisteredTokens[index];
                    }
                    Advance();
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
                        if (ty.Substring(0, ty.IndexOf('_')).Equals(type.Value))
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