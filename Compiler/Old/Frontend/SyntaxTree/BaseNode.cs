using System;

namespace CPU_OS_Simulator.Compiler.Old.Frontend.SyntaxTree
{
    public abstract class BaseNode : IComparable
    {
        public abstract BaseNode Right { get; set; }

        public abstract BaseNode Left { get; set; }

        public dynamic Data
        {
            get { return data; }
            set { data = value; }
        }

        public abstract BaseNode Parent { get; set; }

        protected dynamic data;
        protected BaseNode right;
        protected BaseNode left;
        protected BaseNode parent;
        /// <summary>
        /// This function is called when the node is being visited by the parser
        /// </summary>
        public abstract void Visit();

        /// <summary>
        /// This Function is called when the node is being evaluated by the parser
        /// </summary>
        public abstract void Evaluate();

        /// <summary>
        /// Compares the current instance with another object of the same type and returns an integer that indicates whether the current instance precedes, follows, or occurs in the same position in the sort order as the other object.
        /// </summary>
        /// <returns>
        /// A value that indicates the relative order of the objects being compared. The return value has these meanings: Value Meaning Less than zero This instance precedes <paramref name="obj"/> in the sort order. Zero This instance occurs in the same position in the sort order as <paramref name="obj"/>. Greater than zero This instance follows <paramref name="obj"/> in the sort order. 
        /// </returns>
        /// <param name="obj">An object to compare with this instance. </param><exception cref="T:System.ArgumentException"><paramref name="obj"/> is not the same type as this instance. </exception>
        public virtual int CompareTo(object obj)
        {
            return 0;
        }
    }
}
