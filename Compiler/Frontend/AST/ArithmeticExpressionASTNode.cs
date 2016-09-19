using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CPU_OS_Simulator.Compiler.Frontend.AST
{
    public class ArithmeticExpressionASTNode : BaseASTNode
    {
        private BaseASTNode pLeftExpreesion;
        private BaseASTNode pRightExpression;
        EnumOperatorType operatorType = EnumOperatorType.UNKNOWN;

        public ArithmeticExpressionASTNode(BaseASTNode pLeftExpression, BaseASTNode pRightExpression,
            EnumOperatorType operatorType, BaseASTNode pNodeData)
        {
            this.pLeftExpreesion = pLeftExpression;
            this.pRightExpression = pRightExpression;
            this.operatorType = operatorType;
            this.PNodeData = pNodeData;
        }

        public override bool Destroy()
        {
            pLeftExpreesion.Destroy();
            pRightExpression.Destroy();
            PNodeData.Destroy();
            return true;
        }

        public BaseASTNode PLeftExpreesion
        {
            get { return pLeftExpreesion; }
            set { pLeftExpreesion = value; }
        }

        public BaseASTNode PRightExpression
        {
            get { return pRightExpression; }
            set { pRightExpression = value; }
        }

        public EnumOperatorType OperatorType
        {
            get { return operatorType; }
            set { operatorType = value; }
        }
    }
}
