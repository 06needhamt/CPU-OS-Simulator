using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CPU_OS_Simulator.Compiler.Frontend.Tokens;

namespace CPU_OS_Simulator.Compiler.Frontend.SyntaxTree
{
    public class ASTNode<T>
    {
        protected ASTNode()
        {
            
        } 
        public ASTNode(T value)
        {
            this.value = value;
            this.type = (EnumTypes)Enum.Parse(typeof(EnumTypes), typeof(T).ToString());
        }
        /// <summary>
        /// The value stored in this node
        /// </summary>
        protected T value;
        /// <summary>
        /// The type of value to be returned from this node
        /// </summary>
        protected EnumTypes type = EnumTypes.UNKNOWN;
        /// <summary>
        /// True if a result needs to be returned from this node 
        /// i.e. it holds an expression such as 1 + 1
        /// </summary>
        protected bool isResultRequired;

        /// <summary>
        /// This function is called when the node is being visited by the parser
        /// </summary>
        public virtual void Visit()
        {
            
        }

        /// <summary>
        /// This Function is called when the node is being evaluated by the parser
        /// </summary>
        public virtual void Evaluate()
        {
            
        }

    }
}
