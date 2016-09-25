using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CPU_OS_Simulator.Compiler.Frontend.Symbols;
using CPU_OS_Simulator.CPU;

namespace CPU_OS_Simulator.Compiler.Symbols
{
    public class Symbol
    {
        private EnumSymbolType type = EnumSymbolType.UNKNOWN;
        private string name;
        private long id;
        private Symbol parent;
        private Register register;
        private int memoryAddress;

        public Symbol(string name, EnumSymbolType type, Symbol parent = null)
        {
            this.name = name;
            this.type = type;
        }

        public virtual bool IsDefinedInScope(string name, Symbol scope) { throw new NotSupportedException("Function Must Be Overridden");}
        public virtual bool IsDefined(string name) { throw new NotSupportedException("Function Must Be Overridden"); }

        public int AllocateMemoryAddress()
        {
            return int.MaxValue;
        }

        public Register AllocateRegister()
        {
            int num = RegisterAllocationTable.allocated.IndexOf(RegisterAllocationTable.allocated.FirstOrDefault(x => x == true));
            string name = "R";
            if (num < 10)
                name += "0" + num;
            else
                name += num;
            return CPU.Register.FindRegister(name);
        }

        public bool IsExecutable()
        {
            return (type & (EnumSymbolType.FUNCTION | EnumSymbolType.SUBROUTINE | EnumSymbolType.PROGRAM)) > 0;
        }

        public EnumSymbolType Type
        {
            get { return type; }
            set { type = value; }
        }

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public long Id
        {
            get { return id; }
            set { id = value; }
        }

        public Symbol Parent
        {
            get { return parent; }
            set { parent = value; }
        }

        public Register Register
        {
            get { return register; }
            set { register = value; }
        }

        public int MemoryAddress
        {
            get { return memoryAddress; }
            set { memoryAddress = value; }
        }
    }
}
