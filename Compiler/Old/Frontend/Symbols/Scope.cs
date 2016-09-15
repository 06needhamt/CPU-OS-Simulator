
namespace CPU_OS_Simulator.Compiler.Old.Frontend.Symbols
{
    public class Scope
    {
        public static readonly Scope GLOBAL_SCOPE = new Scope(null,"Global Scope");

        private Scope parentScope;
        private string name;

        public Scope(Scope parentScope, string name)
        {
            this.parentScope = parentScope;
            this.name = name;
        }

        public Scope ParentScope
        {
            get { return parentScope; }
            set { parentScope = value; }
        }

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

    }
}