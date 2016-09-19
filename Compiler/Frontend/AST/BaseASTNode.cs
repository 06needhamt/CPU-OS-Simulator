using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CPU_OS_Simulator.Compiler.Frontend.AST
{
    public abstract class BaseASTNode
    {
        private BaseASTNode pNodeData;
        EnumASTNodeType nodeType;

        public abstract bool Destroy();

        public BaseASTNode PNodeData
        {
            get { return pNodeData; }
            set { pNodeData = value; }
        }

        public EnumASTNodeType NodeType
        {
            get { return nodeType; }
            set { nodeType = value; }
        }
    }
}
