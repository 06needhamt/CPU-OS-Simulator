using System;

namespace CPU_OS_Simulator.Compiler.Frontend.AST
{
    #pragma warning disable 1591
    [Flags]
    public enum EnumASTNodeType
    {
       VARIABLE_DECLARATION_NODE = 1 << 0,
       VALUE_EXPRESSION_NODE = 1 << 1,
       LITERAL_EXPRESSION_NODE = 1 << 2,
       ARITHMETIC_EXPRESSION_NODE = 1 << 3,
       REFERENCE_EXPRESSION_NODE = 1 << 4,
       FUNCTION_DECLARATION_NODE = 1 << 5
    }
}