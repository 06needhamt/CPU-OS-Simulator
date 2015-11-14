using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web.Script.Serialization;
using System.Windows;

namespace CPU_OS_Simulator.CPU
{
    /// <summary>
    /// This class represents an instruction witch can be executed by the virtual CPU
    /// </summary>
    [Serializable]
    public class Instruction
    {
        #region Global Variables

        /// <summary>
        /// The opcode for the instruction
        /// </summary>
        private Int32 opcode;

        /// <summary>
        /// The catagory in which the instruction will be displayed within the interface
        /// </summary>
        private string category;

        /// <summary>
        /// The function that will be executed when the instruction is executed
        /// </summary>
        [ScriptIgnore]
        private Func<int> execute;

        /// <summary>
        /// The first operand for the instruction
        /// </summary>
        private Operand operand1;

        /// <summary>
        /// The second operand for the instruction
        /// </summary>
        private Operand operand2;

        /// <summary>
        /// The Logical address of this instruction within a program
        /// </summary>
        private Int32 address;

        /// <summary>
        /// The size of the instruction within the program
        /// </summary>
        private Int32 size;

        /// <summary>
        /// The result of the instruction once executed e.g. the result of ADD 1,1 would be 2
        /// </summary>
        private Int32 result;

        /// <summary>
        /// The string representation of this instruction e.g. ADD R01,10
        /// </summary>
        private string instructionString;

        [ScriptIgnore]
        private ExecutionUnit unit;
        #endregion Global Variables

        #region Constructors

        /// <summary>
        /// Default constructor for instruction used when deserialising an instruction
        /// NOTE Do not use in code
        /// </summary>
        public Instruction()
        {
        }

        /// <summary>
        /// Constructor for an instruction that takes no operands
        /// </summary>
        /// <param name="opcode"> the opcode for the instruction</param>
        /// <param name="size"> the size of the instruction </param>
        public Instruction(Int32 opcode, Int32 size)
        {
            this.opcode = opcode;
            this.operand1 = null;
            this.operand2 = null;
            this.size = size;
            instructionString = this.ToString();
            BindDelegate();
        }

        /// <summary>
        /// Constructor for an instruction that takes one operand
        /// </summary>
        /// <param name="opcode"> the opcode for the instruction</param>
        /// <param name="op1"> the first operand of the instruction</param>
        /// <param name="size"> the size of the instruction </param>
        public Instruction(Int32 opcode, Operand op1, Int32 size)
        {
            this.opcode = opcode;
            this.operand1 = op1;
            this.operand2 = null;
            this.size = size;
            instructionString = this.ToString();
            BindDelegate();
        }

        /// <summary>
        /// Constructor for an instruction that takes two operands
        /// </summary>
        /// <param name="opcode"> the opcode for the instruction</param>
        /// <param name="op1"> the first operand of the instruction</param>
        /// <param name="op2"> the second operand of the instruction</param>
        /// <param name="size"> the size of the instruction </param>
        public Instruction(Int32 opcode, Operand op1, Operand op2, Int32 size)
        {
            this.opcode = opcode;
            this.operand1 = op1;
            this.operand2 = op2;
            this.size = size;
            instructionString = this.ToString();
            BindDelegate();
        }

        #endregion Constructors

        #region Properties

        public int Opcode
        {
            get
            {
                return opcode;
            }

            set
            {
                opcode = value;
            }
        }

        public string Category
        {
            get
            {
                return category;
            }

            set
            {
                category = value;
            }
        }

        public Operand Operand1
        {
            get
            {
                return operand1;
            }

            set
            {
                operand1 = value;
            }
        }

        public Operand Operand2
        {
            get
            {
                return operand2;
            }

            set
            {
                operand2 = value;
            }
        }

        [ScriptIgnore]
        public Func<int> Execute
        {
            get
            {
                return execute;
            }

            set
            {
                execute = value;
            }
        }

        public int Result
        {
            get
            {
                return result;
            }

            set
            {
                result = value;
            }
        }

        public int Size
        {
            get
            {
                return size;
            }

            set
            {
                size = value;
            }
        }

        public int Address
        {
            get
            {
                return address;
            }

            set
            {
                address = value;
            }
        }

        public string InstructionString
        {
            get
            {
                return instructionString;
            }

            set
            {
                instructionString = value;
            }
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// this function binds a delegate to an instruction.
        /// The delegate bound here will be called when the instruction is executed
        /// </summary>
        public void BindDelegate()
        {
            switch (opcode)
            {
                case 0:
                    {
                        this.execute = () => MOV(operand1, operand2); // save the function in memory to call later
                        break;
                    }
                case 1:
                    {
                        this.execute = () => MVS(operand1, operand2); // save the function in memory to call later
                        break;
                    }
                case 2:
                    {
                        this.execute = () => CVS(operand1, operand2); // save the function in memory to call later
                        break;
                    }
                case 3:
                    {
                        this.execute = () => CVI(operand1, operand2); // save the function in memory to call later
                        break;
                    }
                case 4:
                    {
                        this.execute = () => LDB(operand1, operand2); // save the function in memory to call later
                        break;
                    }
                case 5:
                    {
                        this.execute = () => LDW(operand1, operand2); // save the function in memory to call later
                        break;
                    }
                case 6:
                    {
                        this.execute = () => LNS(operand1, operand2); // save the function in memory to call later
                        break;
                    }
                case 7:
                    {
                        this.execute = () => LDBI(operand1, operand2); // save the function in memory to call later
                        break;
                    }
                case 8:
                    {
                        this.execute = () => LDWI(operand1, operand2); // save the function in memory to call later
                        break;
                    }
                case 9:
                    {
                        this.execute = () => TAS(operand1, operand2); // save the function in memory to call later
                        break;
                    }
                case 10:
                    {
                        this.execute = () => STB(operand1, operand2); // save the function in memory to call later
                        break;
                    }
                case 11:
                    {
                        this.execute = () => STW(operand1, operand2); // save the function in memory to call later
                        break;
                    }
                case 12:
                    {
                        this.execute = () => STBI(operand1, operand2); // save the function in memory to call later
                        break;
                    }
                case 13:
                    {
                        this.execute = () => STWI(operand1, operand2); // save the function in memory to call later
                        break;
                    }
                case 14:
                    {
                        this.execute = () => PUSH(operand1, operand2); // save the function in memory to call later
                        break;
                    }
                case 15:
                    {
                        this.execute = () => POP(operand1, operand2); // save the function in memory to call later
                        break;
                    }
                case 16:
                    {
                        this.execute = () => SWP(operand1, operand2); // save the function in memory to call later
                        break;
                    }
                case 17:
                    {
                        this.execute = () => AND(operand1, operand2); // save the function in memory to call later
                        break;
                    }
                case 18:
                    {
                        this.execute = () => OR(operand1, operand2); // save the function in memory to call later
                        break;
                    }
                case 19:
                    {
                        this.execute = () => NOT(operand1, operand2); // save the function in memory to call later
                        break;
                    }
                case 20:
                    {
                        this.execute = () => SHL(operand1, operand2); // save the function in memory to call later
                        break;
                    }
                case 21:
                    {
                        this.execute = () => SHR(operand1, operand2); // save the function in memory to call later
                        break;
                    }
                case 22:
                    {
                        this.execute = () => ADD(operand1, operand2); // save the function in memory to call later
                        break;
                    }
                case 23:
                    {
                        this.execute = () => SUB(operand1, operand2); // save the function in memory to call later
                        break;
                    }
                case 24:
                    {
                        this.execute = () => SUBU(operand1, operand2); // save the function in memory to call later
                        break;
                    }
                case 25:
                    {
                        this.execute = () => MUL(operand1, operand2); // save the function in memory to call later
                        break;
                    }
                case 26:
                    {
                        this.execute = () => DIV(operand1, operand2); // save the function in memory to call later
                        break;
                    }
                case 27:
                    {
                        this.execute = () => INC(operand1, operand2); // save the function in memory to call later
                        break;
                    }
                case 28:
                    {
                        this.execute = () => DEC(operand1, operand2); // save the function in memory to call later
                        break;
                    }
                case 29:
                    {
                        this.execute = () => JMP(operand1, operand2); // save the function in memory to call later
                        break;
                    }
                default:
                    {
                        break;
                    }
            }
        }

        /// <summary>
        /// Returns a string that represents this instruction
        /// </summary>
        /// <returns> a string that represents this instruction</returns>
        public override string ToString()
        {
            string parsedOpcode = Enum.GetName(typeof(EnumOpcodes), opcode);
            string op1 = "";
            string op2 = "";
            if (operand1 != null)
            {
                if (operand1.IsRegister)
                {
                    op1 = Operand1.Register.Name;
                }
                else
                {
                    op1 = Operand1.Value.ToString();
                }
            }
            if (operand2 != null)
            {
                if (Operand2.IsRegister)
                {
                    op2 = Operand2.Register.Name;
                }
                else
                {
                    op2 = Operand2.Value.ToString();
                }
            }
            return parsedOpcode.ToUpper() + " " + op1.ToUpper() + "," + op2.ToUpper();
        }

        #endregion Methods

        #region Instruction Execution Functions

        //TODO Allow for memory operands

        #region Data Transfer

        /// <summary>
        /// This fuction is called whenever a MOV instruction is executed
        /// </summary>
        /// <param name="lhs"> The left hand operand of the instruction </param>
        /// <param name="rhs"> The right hand operand of the instruction </param>
        /// <returns> the result of the instruction or int.MINVALUE if no result is returned </returns>
        private int MOV(Operand lhs, Operand rhs)
        {
            result = rhs.Value;
            if (lhs.IsRegister)
            {
                lhs.Register.Value = result;
                Register.FindRegister(lhs.Register.Name).setRegisterValue(result, EnumOperandType.VALUE);
            }
            return result;
        }

        /// <summary>
        /// This fuction is called whenever a MVS instruction is executed
        /// </summary>
        /// <param name="lhs"> The left hand operand of the instruction </param>
        /// <param name="rhs"> The right hand operand of the instruction </param>
        /// <returns> the result of the instruction or int.MINVALUE if no result is returned </returns>
        private int MVS(Operand lhs, Operand rhs)
        {
            MessageBox.Show("MVS Instruction is not currently implemented", "", MessageBoxButton.OK, MessageBoxImage.Information);
            return 0;
        }

        /// <summary>
        /// This fuction is called whenever a CVS instruction is executed
        /// </summary>
        /// <param name="lhs"> The left hand operand of the instruction </param>
        /// <param name="rhs"> The right hand operand of the instruction </param>
        /// <returns> the result of the instruction or int.MINVALUE if no result is returned </returns>
        private int CVS(Operand lhs, Operand rhs)
        {
            MessageBox.Show("CVS Instruction is not currently used", "", MessageBoxButton.OK, MessageBoxImage.Information);
            return 0;
        }

        /// <summary>
        /// This fuction is called whenever a CVI instruction is executed
        /// </summary>
        /// <param name="lhs"> The left hand operand of the instruction </param>
        /// <param name="rhs"> The right hand operand of the instruction </param>
        /// <returns> the result of the instruction or int.MINVALUE if no result is returned </returns>
        private int CVI(Operand lhs, Operand rhs)
        {
            MessageBox.Show("CVI Instruction is not currently used", "", MessageBoxButton.OK, MessageBoxImage.Information);
            return 0;
        }

        /// <summary>
        /// This fuction is called whenever a LDB instruction is executed
        /// </summary>
        /// <param name="lhs"> The left hand operand of the instruction </param>
        /// <param name="rhs"> The right hand operand of the instruction </param>
        /// <returns> the result of the instruction or int.MINVALUE if no result is returned </returns>
        private int LDB(Operand lhs, Operand rhs)
        {
            MessageBox.Show("LDB Instruction is not currently implemented", "", MessageBoxButton.OK, MessageBoxImage.Information);
            return 0;
        }

        /// <summary>
        /// This fuction is called whenever a LDW instruction is executed
        /// </summary>
        /// <param name="lhs"> The left hand operand of the instruction </param>
        /// <param name="rhs"> The right hand operand of the instruction </param>
        /// <returns> the result of the instruction or int.MINVALUE if no result is returned </returns>
        private int LDW(Operand lhs, Operand rhs)
        {
            MessageBox.Show("LDW Instruction is not currently implemented", "", MessageBoxButton.OK, MessageBoxImage.Information);
            return 0;
        }

        /// <summary>
        /// This fuction is called whenever a LNS instruction is executed
        /// </summary>
        /// <param name="lhs"> The left hand operand of the instruction </param>
        /// <param name="rhs"> The right hand operand of the instruction </param>
        /// <returns> the result of the instruction or int.MINVALUE if no result is returned </returns>
        private int LNS(Operand lhs, Operand rhs)
        {
            MessageBox.Show("LNS Instruction is not currently implemented", "", MessageBoxButton.OK, MessageBoxImage.Information);
            return 0;
        }

        /// <summary>
        /// This fuction is called whenever a LDBI instruction is executed
        /// </summary>
        /// <param name="lhs"> The left hand operand of the instruction </param>
        /// <param name="rhs"> The right hand operand of the instruction </param>
        /// <returns> the result of the instruction or int.MINVALUE if no result is returned </returns>
        private int LDBI(Operand lhs, Operand rhs)
        {
            MessageBox.Show("LDBI Instruction is not currently implemented", "", MessageBoxButton.OK, MessageBoxImage.Information);
            return 0;
        }

        /// <summary>
        /// This fuction is called whenever a LDWI instruction is executed
        /// </summary>
        /// <param name="lhs"> The left hand operand of the instruction </param>
        /// <param name="rhs"> The right hand operand of the instruction </param>
        /// <returns> the result of the instruction or int.MINVALUE if no result is returned </returns>
        private int LDWI(Operand lhs, Operand rhs)
        {
            MessageBox.Show("LDWI Instruction is not currently implemented", "", MessageBoxButton.OK, MessageBoxImage.Information);
            return 0;
        }

        /// <summary>
        /// This fuction is called whenever a TAS instruction is executed
        /// </summary>
        /// <param name="lhs"> The left hand operand of the instruction </param>
        /// <param name="rhs"> The right hand operand of the instruction </param>
        /// <returns> the result of the instruction or int.MINVALUE if no result is returned </returns>
        private int TAS(Operand lhs, Operand rhs)
        {
            //TODO Implement TAS
            MessageBox.Show("TAS Instruction is not currently implemented", "", MessageBoxButton.OK, MessageBoxImage.Information);
            return 0;
        }

        /// <summary>
        /// This fuction is called whenever a STB instruction is executed
        /// </summary>
        /// <param name="lhs"> The left hand operand of the instruction </param>
        /// <param name="rhs"> The right hand operand of the instruction </param>
        /// <returns> the result of the instruction or int.MINVALUE if no result is returned </returns>
        private int STB(Operand lhs, Operand rhs)
        {
            MessageBox.Show("STB Instruction is not currently implemented", "", MessageBoxButton.OK, MessageBoxImage.Information);
            return 0;
        }

        /// <summary>
        /// This fuction is called whenever a STW instruction is executed
        /// </summary>
        /// <param name="lhs"> The left hand operand of the instruction </param>
        /// <param name="rhs"> The right hand operand of the instruction </param>
        /// <returns> the result of the instruction or int.MINVALUE if no result is returned </returns>
        private int STW(Operand lhs, Operand rhs)
        {
            MessageBox.Show("STW Instruction is not currently implemented", "", MessageBoxButton.OK, MessageBoxImage.Information);
            return 0;
        }

        /// <summary>
        /// This fuction is called whenever a STBI instruction is executed
        /// </summary>
        /// <param name="lhs"> The left hand operand of the instruction </param>
        /// <param name="rhs"> The right hand operand of the instruction </param>
        /// <returns> the result of the instruction or int.MINVALUE if no result is returned </returns>
        private int STBI(Operand lhs, Operand rhs)
        {
            MessageBox.Show("STBI Instruction is not currently implemented", "", MessageBoxButton.OK, MessageBoxImage.Information);
            return 0;
        }

        /// <summary>
        /// This fuction is called whenever a STWI instruction is executed
        /// </summary>
        /// <param name="lhs"> The left hand operand of the instruction </param>
        /// <param name="rhs"> The right hand operand of the instruction </param>
        /// <returns> the result of the instruction or int.MINVALUE if no result is returned </returns>
        private int STWI(Operand lhs, Operand rhs)
        {
            MessageBox.Show("STWI Instruction is not currently implemented", "", MessageBoxButton.OK, MessageBoxImage.Information);
            return 0;
        }

        /// <summary>
        /// This fuction is called whenever a PUSH instruction is executed
        /// </summary>
        /// <param name="lhs"> The left hand operand of the instruction </param>
        /// <param name="rhs"> The right hand operand of the instruction </param>
        /// <returns> the result of the instruction or int.MINVALUE if no result is returned </returns>
        private int PUSH(Operand lhs, Operand rhs)
        {
            SimulatorProgram p = GetCurrentProgram();
            if (lhs.IsRegister)
            {
                lhs.Register.Value = Register.FindRegister(lhs.Register.Name).Value;
                p.Stack.pushItem(new StackItem(lhs.Register.Value));
            }
            else
            {
                p.Stack.pushItem(new StackItem(lhs.Value));
            }
            //MessageBox.Show("PUSH Instruction is not currently implemented", "", MessageBoxButton.OK, MessageBoxImage.Information);
            return 0;
        }

        /// <summary>
        /// This fuction is called whenever a POP instruction is executed
        /// </summary>
        /// <param name="lhs"> The left hand operand of the instruction </param>
        /// <param name="rhs"> The right hand operand of the instruction </param>
        /// <returns> the result of the instruction or int.MINVALUE if no result is returned </returns>
        private int POP(Operand lhs, Operand rhs)
        {
            SimulatorProgram p = GetCurrentProgram();
            if (!lhs.IsRegister)
            {
                MessageBox.Show("Operand for POP instruction must be a register", "", MessageBoxButton.OK, MessageBoxImage.Information);
                return int.MinValue;
            }
            lhs.Register.Value = p.Stack.popItem();
            result = lhs.Register.Value;
            Register.FindRegister(lhs.Register.Name).setRegisterValue(result, EnumOperandType.VALUE);
            //MessageBox.Show("POP Instruction is not currently implemented", "", MessageBoxButton.OK, MessageBoxImage.Information);
            return result;
        }

        /// <summary>
        /// This fuction is called whenever a SWP instruction is executed
        /// </summary>
        /// <param name="lhs"> The left hand operand of the instruction </param>
        /// <param name="rhs"> The right hand operand of the instruction </param>
        /// <returns> the result of the instruction or int.MINVALUE if no result is returned </returns>
        private int SWP(Operand lhs, Operand rhs)
        {
            if (!lhs.IsRegister || !rhs.IsRegister)
            {
                MessageBox.Show("ERROR SWP Both operands must be a register", "", MessageBoxButton.OK, MessageBoxImage.Information);
                return int.MinValue;
            }
            else
            {
                int leftvalue = lhs.Register.Value;
                int rightvalue = rhs.Register.Value;
                lhs.Register.Value = rightvalue;
                rhs.Register.Value = leftvalue;
                result = lhs.Register.Value;
                return result;
            }
        }
        #endregion Data Transfer
        #region Logical
        
        /// <summary>
        /// This fuction is called whenever a AND instruction is executed
        /// </summary>
        /// <param name="lhs"> The left hand operand of the instruction </param>
        /// <param name="rhs"> The right hand operand of the instruction </param>
        /// <returns> the result of the instruction or int.MINVALUE if no result is returned </returns>
        private int AND(Operand lhs, Operand rhs)
        {
            if (lhs.IsRegister)
            {
                lhs.Register.Value = Register.FindRegister(lhs.Register.Name).Value;
                result = lhs.Register.Value;
            }
            else
            {
                MessageBox.Show("First operand of AND instruction must be a register", "", MessageBoxButton.OK, MessageBoxImage.Information);
                return int.MinValue;
            }
            if (rhs.IsRegister)
            {
                lhs.Register.Value &= Register.FindRegister(rhs.Register.Name).Value; 
            }
            else
            {
                lhs.Register.Value &= rhs.Value;
            }
            result = lhs.Register.Value;
            Register.FindRegister(lhs.Register.Name).setRegisterValue(result, EnumOperandType.VALUE);
            return result;
        }

        /// <summary>
        /// This fuction is called whenever a OR instruction is executed
        /// </summary>
        /// <param name="lhs"> The left hand operand of the instruction </param>
        /// <param name="rhs"> The right hand operand of the instruction </param>
        /// <returns> the result of the instruction or int.MINVALUE if no result is returned </returns>
        private int OR(Operand lhs, Operand rhs)
        {
            if (lhs.IsRegister)
            {
                lhs.Register.Value = Register.FindRegister(lhs.Register.Name).Value;
                result = lhs.Register.Value;
            }
            else
            {
                MessageBox.Show("First operand of OR instruction must be a register", "", MessageBoxButton.OK, MessageBoxImage.Information);
                return int.MinValue;
            }
            if (rhs.IsRegister)
            {
                lhs.Register.Value |= Register.FindRegister(rhs.Register.Name).Value;
            }
            else
            {
                lhs.Register.Value |= rhs.Value;
            }
            result = lhs.Register.Value;
            Register.FindRegister(lhs.Register.Name).setRegisterValue(result, EnumOperandType.VALUE);
            return result;
        }
        /// <summary>
        /// This fuction is called whenever a NOT instruction is executed
        /// </summary>
        /// <param name="lhs"> The left hand operand of the instruction </param>
        /// <param name="rhs"> The right hand operand of the instruction </param>
        /// <returns> the result of the instruction or int.MINVALUE if no result is returned </returns>
        private int NOT(Operand lhs, Operand rhs)
        {
            if (lhs.IsRegister)
            {
                lhs.Register.Value = Register.FindRegister(lhs.Register.Name).Value;
                result = lhs.Register.Value;
            }
            else
            {
                MessageBox.Show("First operand of NOT instruction must be a register", "", MessageBoxButton.OK, MessageBoxImage.Information);
                return int.MinValue;
            }
            if (rhs.IsRegister)
            {
                lhs.Register.Value = ~Register.FindRegister(rhs.Register.Name).Value;
            }
            else
            {
                lhs.Register.Value = ~rhs.Value;
            }
            result = lhs.Register.Value;
            Register.FindRegister(lhs.Register.Name).setRegisterValue(result, EnumOperandType.VALUE);
            return result;
        }
        /// <summary>
        /// This fuction is called whenever a SHL instruction is executed
        /// </summary>
        /// <param name="lhs"> The left hand operand of the instruction </param>
        /// <param name="rhs"> The right hand operand of the instruction </param>
        /// <returns> the result of the instruction or int.MINVALUE if no result is returned </returns>
        private int SHL(Operand lhs, Operand rhs)
        {
            if (lhs.IsRegister)
            {
                lhs.Register.Value = Register.FindRegister(lhs.Register.Name).Value;
                result = lhs.Register.Value;
            }
            else
            {
                MessageBox.Show("First operand of SHL instruction must be a register", "", MessageBoxButton.OK, MessageBoxImage.Information);
                return int.MinValue;
            }
            if (rhs.IsRegister)
            {
                lhs.Register.Value <<= Register.FindRegister(rhs.Register.Name).Value;
            }
            else
            {
                lhs.Register.Value <<= rhs.Value;
            }
            result = lhs.Register.Value;
            Register.FindRegister(lhs.Register.Name).setRegisterValue(result, EnumOperandType.VALUE);
            return result;
        }
        /// <summary>
        /// This fuction is called whenever a SHR instruction is executed
        /// </summary>
        /// <param name="lhs"> The left hand operand of the instruction </param>
        /// <param name="rhs"> The right hand operand of the instruction </param>
        /// <returns> the result of the instruction or int.MINVALUE if no result is returned </returns>
        private int SHR(Operand lhs, Operand rhs)
        {
            if (lhs.IsRegister)
            {
                lhs.Register.Value = Register.FindRegister(lhs.Register.Name).Value;
                result = lhs.Register.Value;
            }
            else
            {
                MessageBox.Show("First operand of SHR instruction must be a register", "", MessageBoxButton.OK, MessageBoxImage.Information);
                return int.MinValue;
            }
            if (rhs.IsRegister)
            {
                lhs.Register.Value >>= Register.FindRegister(rhs.Register.Name).Value;
            }
            else
            {
                lhs.Register.Value >>= rhs.Value;
            }
            result = lhs.Register.Value;
            Register.FindRegister(lhs.Register.Name).setRegisterValue(result, EnumOperandType.VALUE);
            return result;
        }
        #endregion Logical

        #region Arithmetic
        /// <summary>
        /// This fuction is called whenever a ADD instruction is executed
        /// </summary>
        /// <param name="lhs"> The left hand operand of the instruction </param>
        /// <param name="rhs"> The right hand operand of the instruction </param>
        /// <returns> the result of the instruction or int.MINVALUE if no result is returned </returns>
        private int ADD(Operand lhs, Operand rhs)
        {
            if (lhs.IsRegister)
            {
                lhs.Register.Value = Register.FindRegister(lhs.Register.Name).Value;
                result = lhs.Register.Value;
                //Register.FindRegister(lhs.Register.Name).setRegisterValue(result, EnumOperandType.VALUE);
            }
            else
            {
                MessageBox.Show("First operand of ADD instruction must be a register", "", MessageBoxButton.OK, MessageBoxImage.Information);
                return int.MinValue;
            }
            if (rhs.IsRegister)
            {
                lhs.Register.Value += Register.FindRegister(rhs.Register.Name).Value;
            }
            else
            {
                lhs.Register.Value += rhs.Value;
            }
            result = lhs.Register.Value;
            Register.FindRegister(lhs.Register.Name).setRegisterValue(result, EnumOperandType.VALUE);
            return result;
        }
        /// <summary>
        /// This fuction is called whenever a SUB instruction is executed
        /// </summary>
        /// <param name="lhs"> The left hand operand of the instruction </param>
        /// <param name="rhs"> The right hand operand of the instruction </param>
        /// <returns> the result of the instruction or int.MINVALUE if no result is returned </returns>
        private int SUB(Operand lhs, Operand rhs)
        {
            if (lhs.IsRegister)
            {
                lhs.Register.Value = Register.FindRegister(lhs.Register.Name).Value;
                result = lhs.Register.Value;
                //Register.FindRegister(lhs.Register.Name).setRegisterValue(result, EnumOperandType.VALUE);
            }
            else
            {
                MessageBox.Show("First operand of SUB instruction must be a register", "", MessageBoxButton.OK, MessageBoxImage.Information);
                return int.MinValue;
            }
            if (rhs.IsRegister)
            {
                lhs.Register.Value -= Register.FindRegister(rhs.Register.Name).Value;
            }
            else
            {
                lhs.Register.Value -= rhs.Value;
            }
            result = lhs.Register.Value;
            Register.FindRegister(lhs.Register.Name).setRegisterValue(result, EnumOperandType.VALUE);
            return result;
        }
        /// <summary>
        /// This fuction is called whenever a SUBU instruction is executed
        /// </summary>
        /// <param name="lhs"> The left hand operand of the instruction </param>
        /// <param name="rhs"> The right hand operand of the instruction </param>
        /// <returns> the result of the instruction or int.MINVALUE if no result is returned </returns>
        private int SUBU(Operand lhs, Operand rhs)
        {
            if (lhs.IsRegister)
            {
                lhs.Register.Value = Math.Abs(Register.FindRegister(lhs.Register.Name).Value);
                result = lhs.Register.Value;
                //Register.FindRegister(lhs.Register.Name).setRegisterValue(result, EnumOperandType.VALUE);
            }
            else
            {
                MessageBox.Show("First operand of SUBU instruction must be a register", "", MessageBoxButton.OK, MessageBoxImage.Information);
                return int.MinValue;
            }
            if (rhs.IsRegister)
            {
                lhs.Register.Value = Math.Abs(lhs.Register.Value - Register.FindRegister(rhs.Register.Name).Value);
            }
            else
            {
                lhs.Register.Value = Math.Abs(lhs.Register.Value - rhs.Value);
            }
            result = lhs.Register.Value;
            Register.FindRegister(lhs.Register.Name).setRegisterValue(result, EnumOperandType.VALUE);
            return result;
        }
        /// <summary>
        /// This fuction is called whenever a MUL instruction is executed
        /// </summary>
        /// <param name="lhs"> The left hand operand of the instruction </param>
        /// <param name="rhs"> The right hand operand of the instruction </param>
        /// <returns> the result of the instruction or int.MINVALUE if no result is returned </returns>
        private int MUL(Operand lhs, Operand rhs)
        {
            if (lhs.IsRegister)
            {
                lhs.Register.Value = Register.FindRegister(lhs.Register.Name).Value;
                result = lhs.Register.Value;
                //Register.FindRegister(lhs.Register.Name).setRegisterValue(result, EnumOperandType.VALUE);
            }
            else
            {
                MessageBox.Show("First operand of MUL instruction must be a register", "", MessageBoxButton.OK, MessageBoxImage.Information);
                return int.MinValue;
            }
            if (rhs.IsRegister)
            {
                lhs.Register.Value *= Register.FindRegister(rhs.Register.Name).Value;
            }
            else
            {
                lhs.Register.Value *= rhs.Value;
            }
            result = lhs.Register.Value;
            Register.FindRegister(lhs.Register.Name).setRegisterValue(result, EnumOperandType.VALUE);
            return result;
        }

        /// <summary>
        /// This fuction is called whenever a DIV instruction is executed
        /// </summary>
        /// <param name="lhs"> The left hand operand of the instruction </param>
        /// <param name="rhs"> The right hand operand of the instruction </param>
        /// <returns> the result of the instruction or int.MINVALUE if no result is returned </returns>
        private int DIV(Operand lhs, Operand rhs)
        {
            if (lhs.IsRegister)
            {
                lhs.Register.Value = Register.FindRegister(lhs.Register.Name).Value;
                result = lhs.Register.Value;
                //Register.FindRegister(lhs.Register.Name).setRegisterValue(result, EnumOperandType.VALUE);
            }
            else
            {
                MessageBox.Show("First operand of DIV instruction must be a register", "", MessageBoxButton.OK, MessageBoxImage.Information);
                return int.MinValue;
            }
            if (rhs.IsRegister)
            {
                if (Register.FindRegister(rhs.Register.Name).Value == 0)
                {
                    MessageBox.Show("Cannot Divide by ZERO", "", MessageBoxButton.OK, MessageBoxImage.Information);
                    lhs.Register.Value = 0;
                    result = int.MinValue;
                    return result;
                }
                lhs.Register.Value /= Register.FindRegister(rhs.Register.Name).Value;
            }
            else
            {
                if(rhs.Value == 0)
                {
                    MessageBox.Show("Cannot Divide by ZERO", "", MessageBoxButton.OK, MessageBoxImage.Information);
                    lhs.Register.Value = 0;
                    result = int.MinValue;
                    return result;
                }
                lhs.Register.Value /= rhs.Value;
            }
            result = lhs.Register.Value;
            Register.FindRegister(lhs.Register.Name).setRegisterValue(result, EnumOperandType.VALUE);
            return result;
        }
        /// <summary>
        /// This fuction is called whenever a INC instruction is executed
        /// </summary>
        /// <param name="lhs"> The left hand operand of the instruction </param>
        /// <param name="rhs"> The right hand operand of the instruction </param>
        /// <returns> the result of the instruction or int.MINVALUE if no result is returned </returns>
        private int INC(Operand lhs, Operand rhs)
        {
            if (lhs.IsRegister)
            {
                lhs.Register.Value = Register.FindRegister(lhs.Register.Name).Value;
                lhs.Register.Value++;
                result = lhs.Register.Value;
                Register.FindRegister(lhs.Register.Name).setRegisterValue(result, EnumOperandType.VALUE);
                return result;
            }
            else
            {
                MessageBox.Show("Operand of INC instruction must be a register", "", MessageBoxButton.OK, MessageBoxImage.Information);
                return int.MinValue;
            }
        }

        /// <summary>
        /// This fuction is called whenever a DEC instruction is executed
        /// </summary>
        /// <param name="lhs"> The left hand operand of the instruction </param>
        /// <param name="rhs"> The right hand operand of the instruction </param>
        /// <returns> the result of the instruction or int.MINVALUE if no result is returned </returns>
        private int DEC(Operand lhs, Operand rhs)
        {
            if (lhs.IsRegister)
            {
                lhs.Register.Value = Register.FindRegister(lhs.Register.Name).Value;
                lhs.Register.Value--;
                result = lhs.Register.Value;
                Register.FindRegister(lhs.Register.Name).setRegisterValue(result, EnumOperandType.VALUE);
                return result;
            }
            else
            {
                MessageBox.Show("Operand of DEC instruction must be a register", "", MessageBoxButton.OK, MessageBoxImage.Information);
                return int.MinValue;
            }
        }
        #endregion Arithmetic

        #region Control Transfer
        private int JMP(Operand lhs, Operand rhs)
        {
            dynamic window = GetMainWindowInstance();
            ExecutionUnit unit = window.ActiveUnit;
            unit.LogicalAddress = lhs.Value;
            unit.CurrentIndex = lhs.Value / 4;
            result = lhs.Value;
            unit.Done = false;
            unit.Stop = false;
            //window.lst_InstructionsList.SelectedIndex = 0;
            return result;
        }
        #endregion Control Transfer
        #endregion Instruction Execution Functions

        #region Window Accessor Methods

        /// <summary>
        /// This function gets the main window instance from the window bridge
        /// </summary>
        /// <returns> the active instance of main window </returns>
        private dynamic GetMainWindowInstance()
        {
            Assembly windowBridge = Assembly.LoadFrom("CPU_OS_Simulator.WindowBridge.dll"); // Load the window bridge module
            Console.WriteLine(windowBridge.GetExportedTypes()[0]);
            Type WindowType = windowBridge.GetType(windowBridge.GetExportedTypes()[0].ToString()); // get the name of the type that contains the window inatances
            dynamic window = WindowType.GetField("MainWindowInstance").GetValue(null); // get the value of the static MainWindowInstance field
            return window;
        }

        /// <summary>
        /// This Function gets the program to be executed by the CPU from the main window
        /// </summary>
        /// <returns>the program to be executed by the CPU</returns>
        private SimulatorProgram GetCurrentProgram()
        {
            dynamic window = GetMainWindowInstance();
            string programName = window.currentProgram; // get the name of the program that is currently loaded
            List<SimulatorProgram> programs = window.ProgramList; // get a copy of the program list
            SimulatorProgram prog = programs.Where(x => x.Name.Equals(programName)).FirstOrDefault(); // find the current program in the list
            return prog; // return the current program
        }

        /// <summary>
        /// This method returns all of the active window instances of a specific type
        /// </summary>
        /// <param name="WindowType"> the type of window to return</param>
        /// <returns> a list containing all active window instances</returns>
        [Obsolete("GetActiveWindow is depreciated please use GetMainWindowInstance instead", true)]
        private List<Window> GetActiveWindow(Type WindowType)
        {
            List<Window> windows = new List<Window>();
            foreach (Window window in Application.Current.Windows)
            {
                if (window.GetType() == WindowType)
                    windows.Add(window);
                else
                    continue;
            }
            return windows;
        }

        #endregion Window Accessor Methods
    }
}