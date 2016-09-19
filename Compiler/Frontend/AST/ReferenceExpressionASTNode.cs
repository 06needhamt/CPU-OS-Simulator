namespace CPU_OS_Simulator.Compiler.Frontend.AST
{
    public class ReferenceExpressionASTNode : BaseASTNode
    {
        private EnumTypes nodeDataType = EnumTypes.UNKNOWN;
        private string identifier;

        public ReferenceExpressionASTNode(EnumTypes nodeDataType, string identifier, BaseASTNode pNodeData)
        {
            this.nodeDataType = nodeDataType;
            this.identifier = identifier;
            this.PNodeData = pNodeData;
        }
        public override bool Destroy()
        {
            PNodeData.Destroy();
            return true;
        }

        public EnumTypes NodeDataType
        {
            get { return nodeDataType; }
            set { nodeDataType = value; }
        }

        public string Identifier
        {
            get { return identifier; }
            set { identifier = value; }
        }
    }
}