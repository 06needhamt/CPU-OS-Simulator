namespace CPU_OS_Simulator.Compiler.Frontend.AST
{
    public class ValueExpressionASTNode : BaseASTNode
    {
        private EnumTypes nodeDataType = EnumTypes.UNKNOWN;

        public ValueExpressionASTNode(EnumTypes nodeDataType, BaseASTNode pNodeData)
        {
            this.nodeDataType = nodeDataType;
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
    }
}