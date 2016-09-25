using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace CPU_OS_Simulator.Compiler.Symbols
{
    public class SymbolRegistry
    {
        private readonly List<Symbol> predefinedSymbols = new Symbol[]
        {

        }.ToList();

        private List<Symbol> registeredSymbols;
        private List<long> registeredIDs;
        private Random r = new Random((int)DateTime.Now.Ticks);

        public SymbolRegistry()
        {
            registeredSymbols = new List<Symbol>();
        }

        public Symbol RegisterSymbol(ref Symbol symbol)
        {
            string name = symbol.Name; // Needed Because of ref parameter
            Symbol temp = predefinedSymbols.Concat(registeredSymbols).FirstOrDefault(x => x.Name.Equals(name));
            if (temp != null)
            {
                MessageBox.Show("Symbol: " + symbol.Name + " Is Already Defined");
                return null;
            }
            else
            {
                int id = r.Next();
                while (registeredSymbols.Any(x => x.Id == id))
                    id = r.Next();
                symbol.Id = id;
                registeredSymbols.Add(symbol);
            }
            return symbol;
        }

        public List<Symbol> PredefinedSymbols
        {
            get { return predefinedSymbols; }
        }

        public List<Symbol> RegisteredSymbols
        {
            get { return registeredSymbols; }
            set { registeredSymbols = value; }
        }
    }
}
