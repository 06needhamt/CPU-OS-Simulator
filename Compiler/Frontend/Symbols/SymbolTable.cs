﻿using System.Collections.Generic;
using System.Windows.Controls;
using CPU_OS_Simulator.Compiler.Frontend.Tokens;

namespace CPU_OS_Simulator.Compiler.Frontend.Symbols
{
    public class SymbolTable
    {
        private LinkedList<Symbol> symbols;
        private LinkedListNode<Symbol> currentSymbol;
        private LinkedListNode<Symbol> nextSymbol;
        private LinkedListNode<Symbol> previousSymbol;
        private TextBox output;

        public SymbolTable(TextBox output)
        {
            this.output = output;
            symbols = new LinkedList<Symbol>();
        }

        public LinkedListNode<Symbol> FindSymbol(string name)
        {
            while (currentSymbol?.Next != null)
            {
                if (currentSymbol.Value.SymbolName.Equals(name))
                {
                    return currentSymbol;
                }
                previousSymbol = currentSymbol;
                currentSymbol = nextSymbol;
                nextSymbol = currentSymbol.Next;
            }
            return null;
        }

        public LinkedListNode<Symbol> FindSymbol(EnumTypes type)
        {
            while (currentSymbol?.Next != null)
            {
                if (currentSymbol.Value.SymbolType == type)
                {
                    return currentSymbol;
                }
                previousSymbol = currentSymbol;
                currentSymbol = nextSymbol;
                nextSymbol = currentSymbol.Next;
            }
            return null;
        }

        public LinkedListNode<Symbol> AddSymbol(LinkedListNode<Symbol> symbol)
        {
            symbols.AddLast(symbol);
            return symbol;
        }

        public void PrintSymbols()
        {
            currentSymbol = symbols.First;
            nextSymbol = currentSymbol.Next;
            previousSymbol = currentSymbol.Previous;

            while (currentSymbol != null)
            {
                if (output != null)
                {
                    output.Text += currentSymbol.Value.ToString();
                }
                currentSymbol = nextSymbol;
                nextSymbol = currentSymbol?.Next;
                previousSymbol = currentSymbol?.Previous;
            }
        }
    } 
}
