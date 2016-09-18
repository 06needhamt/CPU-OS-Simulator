#pragma warning disable 1591
using System.Collections.Generic;
using CPU_OS_Simulator.Compiler.Old.Frontend.SyntaxTree;
using CPU_OS_Simulator.Compiler.Old.Frontend.Tokens;

namespace CPU_OS_Simulator.Compiler.Old.Frontend
{
    class Parser
    {
        private LinkedList<Token> tokens;
        private AST ast;
        public Parser(LinkedList<Token> tokens)
        {
            this.tokens = tokens;
            ast = new AST(delegate(BaseNode node, BaseNode baseNode)
            {
                return node.Data = baseNode.Data;
            });
            //TODO STUCK HERE.
        }
    }
}
