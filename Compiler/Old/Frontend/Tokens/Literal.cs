using System;

namespace CPU_OS_Simulator.Compiler.Old.Frontend.Tokens
{
    public abstract class Literal : Token
    {
        /// <summary>
        /// the character that defines the start of the literal 
        /// </summary>
        protected char startChar = '\0';
        /// <summary>
        /// the character that defines the end of the literal
        /// </summary>
        protected char endChar = '\0';
        /// <summary>
        /// This function detects the type of literal
        /// </summary>
        /// <returns> the type of literal</returns>
        public override abstract Enum DetectType();
        /// <summary>
        /// This function identifies the type of data stored in the literal
        /// </summary>
        /// <returns> the type of value in the literal</returns>
        public abstract EnumTypes identfyValueType();
    }
}