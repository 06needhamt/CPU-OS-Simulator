using System;

namespace CPU_OS_Simulator.Compiler.Frontend.Tokens
{
    public class Keyword : Token
    {
        public Keyword(string value)
        {
            this.value = value;
        }
        public override Enum DetectType()
        {
            switch (value)
            {
                case "program":
                    return EnumKeywordType.PROGRAM;
                case "end":
                    return EnumKeywordType.END;
                case "while":
                    return EnumKeywordType.WHILE;
                case "wend":
                    return EnumKeywordType.WEND;
                case "for":
                    return EnumKeywordType.FOR;
                case "next":
                    return EnumKeywordType.NEXT;
                case "if":
                    return EnumKeywordType.IF;
                case "else":
                    return EnumKeywordType.ELSE;
                case "else if":
                    return EnumKeywordType.ELSE_IF;
                case "then":
                    return EnumKeywordType.THEN;
                case "end if":
                    return EnumKeywordType.END_IF;
                case "select":
                    return EnumKeywordType.SELECT;
                case "case":
                    return EnumKeywordType.CASE;
                case "default":
                    return EnumKeywordType.DEFAULT;
                case "end select":
                    return EnumKeywordType.END_SELECT;
                case "thread":
                    return EnumKeywordType.THREAD;
                case "synchronise":
                    return EnumKeywordType.SYNCHRONISE;
                case "enter":
                    return EnumKeywordType.ENTER;
                case "leave":
                    return EnumKeywordType.LEAVE;
                case "continue":
                    return EnumKeywordType.CONTINUE;
                case "break":
                    return EnumKeywordType.BREAK;
                case "true":
                    return EnumKeywordType.TRUE;
                case "false":
                    return EnumKeywordType.FALSE;
                case "resource":
                    return EnumKeywordType.RESOURCE;
                case "sub":
                    return EnumKeywordType.SUB;
                case "end sub":
                    return EnumKeywordType.END_SUB;
                case "fun":
                    return EnumKeywordType.FUN;
                case "do":
                    return EnumKeywordType.DO;
                case "loop":
                    return EnumKeywordType.LOOP;
                case "read":
                    return EnumKeywordType.READ;
                case "write":
                    return EnumKeywordType.WRITE;
                case "var":
                    return EnumKeywordType.VAR;
                case "ret":
                    return EnumKeywordType.RET;
                case "call":
                    return EnumKeywordType.CALL;
                case "goto":
                    return EnumKeywordType.GOTO;
                case "as":
                    return EnumKeywordType.END;
                default:
                    return EnumKeywordType.UNKNOWN;
            }
        }

        public EnumKeywordType GetKeywordType()
        {
            return (EnumKeywordType) type;
        }
    }
}