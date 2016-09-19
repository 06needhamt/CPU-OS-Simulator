using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace CPU_OS_Simulator.Compiler.Frontend.AST
{
    public class FunctionDeclarationASTNode : BaseASTNode
    {
        EnumTypes functionReturnType = EnumTypes.UNKNOWN;
        private string functionName;
        private AST pfunctionArguments;
        private AST pfunctionBody;

        public FunctionDeclarationASTNode(EnumTypes functionReturnType, string functionName, BaseASTNode pNodeData)
        {
            this.functionReturnType = functionReturnType;
            this.functionName = functionName;
            this.PNodeData = pNodeData;
        }

        public override bool Destroy()
        {
            pfunctionArguments.Destroy();
            pfunctionBody.Destroy();
            PNodeData.Destroy();
            return true;
        }

        public EnumTypes FunctionReturnType
        {
            get { return functionReturnType; }
            set { functionReturnType = value; }
        }

        public string FunctionName
        {
            get { return functionName; }
            set { functionName = value; }
        }

        public AST PfunctionArguments
        {
            get { return pfunctionArguments; }
            set { pfunctionArguments = value; }
        }

        public AST PfunctionBody
        {
            get { return pfunctionBody; }
            set { pfunctionBody = value; }
        }
    }
}
