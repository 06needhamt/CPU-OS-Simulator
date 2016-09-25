using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CPU_OS_Simulator.Compiler.Frontend.AST
{
    public class ProgramDeclarationASTNode : BaseASTNode
    {
        private AST programBody;
        private string programName;

        public ProgramDeclarationASTNode(string programName)
        {
            this.programName = programName;
            this.programBody = new AST();
        }

        public override bool Destroy()
        {
            programBody.Destroy();
            PNodeData.Destroy();
            return true;
        }

        public AST ProgramBody
        {
            get { return programBody; }
            set { programBody = value; }
        }

        public string ProgramName
        {
            get { return programName; }
            set { programName = value; }
        }
    }
}
