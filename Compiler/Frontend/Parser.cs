namespace CPU_OS_Simulator.Compiler.Frontend
{
    public class Parser
    {
        private TokenRegistry tokens;
        private int index = 0;

        public Parser(TokenRegistry tokens)
        {
            this.tokens = tokens;
        }
    }
}