using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace CPU_OS_Simulator.Compiler.Frontend.AST
{
    public class VariableDeclarationASTNode : BaseASTNode
    {
        private string variableName;
        private string variableType;

        public VariableDeclarationASTNode(string variableName, string variableType, BaseASTNode pNodeData)
        {
            this.variableName = variableName;
            this.variableType = variableName;
            this.PNodeData = pNodeData;
            this.NodeType = EnumASTNodeType.VARIABLE_DECLARATION_NODE;
        }
        public override bool Destroy()
        {
            PNodeData.Destroy();
            return true;
        }

        public string VariableName
        {
            get { return variableName; }
            set { variableName = value; }
        }

        public string VariableType
        {
            get { return variableType; }
            set { variableType = value; }
        }
    }
}
