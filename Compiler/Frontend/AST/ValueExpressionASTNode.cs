namespace CPU_OS_Simulator.Compiler.Frontend.AST
{
    public class ValueExpressionASTNode : BaseASTNode
    {
        private EnumTypes nodeDataType = EnumTypes.UNKNOWN;
        private string value;

        public ValueExpressionASTNode(EnumTypes nodeDataType, string value, BaseASTNode pNodeData)
        {
            this.nodeDataType = nodeDataType;
            this.value = value;
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

        public string Value
        {
            get { return value; }
            set { this.value = value; }
        }
    }
}