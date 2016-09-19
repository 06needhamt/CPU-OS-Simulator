namespace CPU_OS_Simulator.Compiler.Frontend.AST
{
    public class LiteralExpressionASTNode : BaseASTNode
    {
        private string expressionValue;

        public LiteralExpressionASTNode(string expressionValue, BaseASTNode pNodeData)
        {
            this.expressionValue = expressionValue;
            this.PNodeData = pNodeData;
        }

        public override bool Destroy()
        {
            PNodeData.Destroy();
            return true;
        }

        public string ExpressionValue
        {
            get { return expressionValue; }
            set { expressionValue = value; }
        }
    }
}