namespace CPU_OS_Simulator.Compiler.Frontend.SyntaxTree
{
    public interface IASTEvaluator
    {
        #region Primitive Types
        /// <summary>
        /// This method Evaluates a string node
        /// </summary>
        /// <param name="node"> the string node to Evaluate </param>
        void Evaluate(StringNode node);
        /// <summary>
        /// This method Evaluates a integer node
        /// </summary>
        /// <param name="node"> the integer node to Evaluate</param>
        void Evaluate(IntegerNode node);
        /// <summary>
        /// This method Evaluates a boolean node
        /// </summary>
        /// <param name="node"> the boolean node to Evaluate</param>
        void Evaluate(BooleanNode node);
        /// <summary>
        /// This method Evaluates a byte node
        /// </summary>
        /// <param name="node"> the byte node to Evaluate</param>
        void Evaluate(ByteNode node);
        /// <summary>
        /// This method Evaluates a object node
        /// </summary>
        /// <param name="node"> the object node to Evaluate</param>
        void Evaluate(ObjectNode node);
        #endregion Primitive Types
        #region Primitive Array Types
        /// <summary>
        /// This method Evaluates a string array node
        /// </summary>
        /// <param name="node"> the string node to Evaluate</param>
        void Evaluate(StringArrayNode node);
        /// <summary>
        /// This method Evaluates a integer array node
        /// </summary>
        /// <param name="node"> the integer array node to Evaluate</param>
        void Evaluate(IntegerArrayNode node);
        /// <summary>
        /// This method Evaluates a boolean array node
        /// </summary>
        /// <param name="node"> the boolean array node to Evaluate</param>
        void Evaluate(BooleanArrayNode node);
        /// <summary>
        /// This method Evaluates a byte array node
        /// </summary>
        /// <param name="node"> the byte array node to Evaluate</param>
        void Evaluate(ByteArrayNode node);
        /// <summary>
        /// This method Evaluates a object array node
        /// </summary>
        /// <param name="node"> the string array node to Evaluate</param>
        void Evaluate(ObjectArrayNode node);
        #endregion

        //TODO Add Evaluator Methods for function and subroutine nodes here
    }
}