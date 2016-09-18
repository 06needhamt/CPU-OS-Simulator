#pragma warning disable 1591
using System;

namespace CPU_OS_Simulator.Compiler.Old.Frontend.Tokens
{
    public abstract class Token
    {
        protected Enum type = EnumOperatorType.UNKNOWN;
        protected string value;
        private bool isoperator;
        private bool iskeyword;
        private bool istype;

        public bool Isoperator
        {
             get { return isoperator; }
        }

        public bool Iskeyword
        {
            get { return iskeyword; }
        }

        public bool Istype
        {
            get { return istype; }
        }

        public Enum Type
        {
            get { return type; }
            set { type = value; }
        }

        public string Value
        {
            get { return value; }
            set { this.value = value; }
        }

        public abstract Enum DetectType();

        public bool isOperator()
        {
            Operator op = new Operator(value);
            isoperator = ((EnumOperatorType) op.DetectType() != EnumOperatorType.UNKNOWN);
            return isoperator;
        }

        public bool isKeyword()
        {
            Keyword keyword = new Keyword(value);
            iskeyword = ((EnumKeywordType) keyword.DetectType() != EnumKeywordType.UNKNOWN);
            return iskeyword;
        }

        public bool isType()
        {
            Typename type = new Typename(value);
            istype = ((EnumTypes) type.DetectType() != EnumTypes.UNKNOWN);
            return istype;
        }

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>
        /// A string that represents the current object.
        /// </returns>
        public override string ToString()
        {
            if (isType())
            {
                return value + " " + (((EnumTypes) type));
            }
            if (isKeyword())
            {
                return value + " " + (((EnumKeywordType)type));
            }
            if (isOperator())
            {
                return value + " " + (((EnumOperatorType)type));
            }
            return value + " " + (((EnumTokenType)type));
        }
    }
}
