using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CPU_OS_Simulator.Compiler.Frontend.Tokens;

namespace CPU_OS_Simulator.Compiler.Frontend.SyntaxTree
{
    class ASTNodeArray<T> : ASTNode<T>
    {
        private T[] value;

        public ASTNodeArray(T[] value)
        {
            this.value = value;
            this.type = (EnumTypes) Enum.Parse(typeof (EnumTypes), typeof (T).ToString());
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
    }
}
