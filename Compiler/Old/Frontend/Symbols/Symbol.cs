#pragma warning disable 1591
using System;
using CPU_OS_Simulator.Compiler.Old.Frontend.Tokens;

namespace CPU_OS_Simulator.Compiler.Old.Frontend.Symbols
{
    public class Symbol : Token
    {
        protected string name = String.Empty;
        protected readonly EnumTypes symbolType = EnumTypes.UNKNOWN;
        protected new string value;
        protected bool issub = false;
        protected bool isfun = false;
        protected Scope symbolScope;

        public bool IsSub
        {
             get { return issub; }
        }

        public bool IsFun
        {
             get { return isfun; }
        }

        public string SymbolValue
        {
            get { return value; }
            set { this.value = value; }
        }

        public string SymbolName
        {
            get { return name; }
            
        }

        public EnumTypes SymbolType
        {
            get { return symbolType; }
        }

        public Scope SymbolScope
        {
            get { return symbolScope; }
            set { symbolScope = value; }
        }

        protected Symbol()
        {
            
        }

        public Symbol(string name, EnumTypes type, string value, Scope scope, bool isSub, bool isFun)
        {
            this.name = name;
            this.type = type;
            this.value = value;
            symbolScope = scope;
            issub = isSub;
            isfun = isFun;
        }

        public override Enum DetectType()
        {
            return EnumTokenType.IDENTIFIER;
        }

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>
        /// A string that represents the current object.
        /// </returns>
        public override string ToString()
        {
            return name + " " + type.ToString() + " " + value + " " + isfun + " " + issub + " " + symbolScope.Name + "\r";
        }
    }
}