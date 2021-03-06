﻿#pragma warning disable 1591
#pragma warning disable 0108
namespace CPU_OS_Simulator.Compiler.Old.Frontend.SyntaxTree
{
    public abstract class ArrayNode : BaseNode
    {
        public abstract override BaseNode Right { get; set; }

        public abstract override BaseNode Left { get; set; }

        /// <summary>
        /// This function is called when the node is being visited by the parser
        /// </summary>
        public abstract override void Visit();

        /// <summary>
        /// This Function is called when the node is being evaluated by the parser
        /// </summary>
        public abstract override void Evaluate();

        public abstract int numberOfElements();


    }
}
