#pragma warning disable 1591
#pragma warning disable 0108

namespace CPU_OS_Simulator.Compiler.Old.Frontend.SyntaxTree
{
    public interface IASTAccessor
    {
        #region Primitive Types
        /// <summary>
        /// This method accesses a string node
        /// </summary>
        /// <param name="node"> the string node to access </param>
        void Access(StringNode node);
        /// <summary>
        /// This method accesses a integer node
        /// </summary>
        /// <param name="node"> the integer node to access</param>
        void Access(IntegerNode node);
        /// <summary>
        /// This method accesses a boolean node
        /// </summary>
        /// <param name="node"> the boolean node to access</param>
        void Access(BooleanNode node);
        /// <summary>
        /// This method accesses a byte node
        /// </summary>
        /// <param name="node"> the byte node to access</param>
        void Access(ByteNode node);
        /// <summary>
        /// This method accesses a object node
        /// </summary>
        /// <param name="node"> the object node to access</param>
        void Access(ObjectNode node);
        #endregion Primitive Types
        #region Primitive Array Types
        /// <summary>
        /// This method accesses a string array node
        /// </summary>
        /// <param name="node"> the string node to access</param>
        void Access(StringArrayNode node);
        /// <summary>
        /// This method accesses a integer array node
        /// </summary>
        /// <param name="node"> the integer array node to access</param>
        void Access(IntegerArrayNode node);
        /// <summary>
        /// This method accesses a boolean array node
        /// </summary>
        /// <param name="node"> the boolean array node to access</param>
        void Access(BooleanArrayNode node);
        /// <summary>
        /// This method accesses a byte array node
        /// </summary>
        /// <param name="node"> the byte array node to access</param>
        void Access(ByteArrayNode node);
        /// <summary>
        /// This method accesses a object array node
        /// </summary>
        /// <param name="node"> the string array node to access</param>
        void Access(ObjectArrayNode node);
        #endregion

        //TODO Add Access Methods for function and subroutine nodes here
    }
}