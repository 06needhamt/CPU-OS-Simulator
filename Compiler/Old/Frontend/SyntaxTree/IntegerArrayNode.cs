#pragma warning disable 1591
#pragma warning disable 0108

namespace CPU_OS_Simulator.Compiler.Old.Frontend.SyntaxTree
{
    public class IntegerArrayNode : ArrayNode
    {
        private new int[] data;
        private BaseNode parent;

        public override BaseNode Right
        {
            get { return right; }
            set { right = value; }
        }

        public override BaseNode Left
        {
            get { return left; }
            set { left = value; }
        }

        public int[] Data
        {
            get { return data; }
            set { data = value; }
        }

        public override BaseNode Parent
        {
            get { return parent; }
            set { parent = value; }
        }


        /// <summary>
        /// This function is called when the node is being visited by the parser
        /// </summary>
        public override void Visit()
        {
        }

        /// <summary>
        /// This Function is called when the node is being evaluated by the parser
        /// </summary>
        public override void Evaluate()
        {
        }

        public override int numberOfElements()
        {
            return data.Length;
        }
    }
}
