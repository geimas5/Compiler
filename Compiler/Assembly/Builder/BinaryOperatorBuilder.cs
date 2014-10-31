namespace Compiler.Assembly.Builder
{
    using System;

    using Compiler.ControlFlowGraph;
    using Compiler.SyntaxTree;

    public sealed class BinaryOperatorBuilder : StatementBuilder<BinaryOperatorStatement>
    {
        protected override void Build()
        {
            if (Statement.Operator == BinaryOperator.Divide 
                || Statement.Operator == BinaryOperator.Add
                || Statement.Operator == BinaryOperator.Equal 
                || Statement.Operator == BinaryOperator.Multiply
                || Statement.Operator == BinaryOperator.Subtract 
                || Statement.Operator == BinaryOperator.Mod
                || Statement.Operator == BinaryOperator.Exponensiation)
            {
                CreateMathInstruction();
            }
            else if (this.Statement.Operator == BinaryOperator.Equal
                || this.Statement.Operator == BinaryOperator.Greater
                || this.Statement.Operator == BinaryOperator.GreaterEqual
                || this.Statement.Operator == BinaryOperator.Less
                || this.Statement.Operator == BinaryOperator.LessEqual
                || this.Statement.Operator == BinaryOperator.NotEqual)
            {
                CreateComparisonInstruction();
            }
            else
            {
                throw new ArgumentOutOfRangeException("statement", "Unsupported operator");    
            }
        }

        private void CreateMathInstruction()
        {
            if (this.Statement.Return.Type.PrimitiveType == PrimitiveType.Double)
            {
                this.CreateFloatingPointMathInstructions();
            }
            else
            {
                this.CreateIntegerMathInstructions();
            }
        }

        private void CreateIntegerMathInstructions()
        {
            Opcode opcode;

            switch (this.Statement.Operator)
            {
                case BinaryOperator.Add:
                    opcode = Opcode.ADD;
                    break;
                case BinaryOperator.Subtract:
                    opcode = Opcode.SUB;
                    break;
                case BinaryOperator.Multiply:
                    opcode = Opcode.IMUL;
                    break;
                case BinaryOperator.Divide:
                    this.CreateDivisionInstruction();
                    return;
                case BinaryOperator.Mod:
                    CreateModuloInstruction();
                    return;
                case BinaryOperator.Exponensiation:
                    CreateExponentiantionInstruction();
                    return;
                default:
                    throw new ArgumentException("operation operation not supported");
            }

            var argument1 = new RegisterOperand(Register.R10);
            Operand argument2 = this.GetRightIntegerOperand(false);

            this.PlaceArgumentInRegister(Statement.Left, Register.R10);

            this.WriteBinaryInstruction(opcode, argument1, argument2);

            this.WriteRegisterToDestination(this.Statement.Return, Register.R10);
        }

        private Operand GetRightIntegerOperand(bool leftIsMemory)
        {
            var variableArgument = Statement.Right as VariableArgument;
            if (variableArgument != null)
            {
                if (variableArgument.Variable.Register.HasValue)
                {
                    return new RegisterOperand(variableArgument.Variable.Register.Value);
                }

                var memoryOperand = this.Procedure.GetVarialeLocation(variableArgument.Variable);
                if (!leftIsMemory) return memoryOperand;

                this.WriteBinaryInstruction(Opcode.MOV, new RegisterOperand(Register.R11), memoryOperand);
                return new RegisterOperand(Register.R11);
            }

            var intConstantArgument = Statement.Right as IntConstantArgument;
            if (intConstantArgument != null)
            {
                if (leftIsMemory)
                {
                    this.PlaceArgumentInRegister(intConstantArgument, Register.R11);
                    return new RegisterOperand(Register.R11);
                }

                return new ConstantOperand(intConstantArgument.Value);
            }

            var globalConstantArgument = Statement.Right as GlobalArgument;
            if (globalConstantArgument != null)
            {
                this.PlaceArgumentInRegister(globalConstantArgument, Register.R11);
                return new RegisterOperand(Register.R11);
            }

            var pointerArgument = Statement.Right as PointerArgument;
            if (pointerArgument != null)
            {
                if (pointerArgument.Variable.Register.HasValue)
                {
                    return new MemoryOperand(pointerArgument.Variable.Register.Value);
                }

                this.PlaceArgumentInRegister(pointerArgument, Register.R11);
                return new RegisterOperand(Register.R11);
            }

            throw new NotImplementedException();
        }

        private void CreateFloatingPointMathInstructions()
        {
            Opcode opcode;

            switch (Statement.Operator)
            {
                case BinaryOperator.Add:
                    opcode = Opcode.ADDSD;
                    break;
                case BinaryOperator.Subtract:
                    opcode = Opcode.SUBSD;
                    break;
                case BinaryOperator.Multiply:
                    opcode = Opcode.MULSD;
                    break;
                case BinaryOperator.Divide:
                    opcode = Opcode.DIVSD;
                    break;
                case BinaryOperator.Exponensiation:
                    CreateExponentiantionInstruction();
                    return;
                default:
                    throw new ArgumentException("operation operation not supported");
            }

            var argument1 = new RegisterOperand(Register.XMM14);
            var argument2 = new RegisterOperand(Register.XMM15);
            PlaceArgumentInRegister(Statement.Left, Register.XMM14);
            PlaceArgumentInRegister(Statement.Right, Register.XMM15);

            this.WriteBinaryInstruction(opcode, argument1, argument2);

            this.WriteBinaryInstruction(Opcode.MOVD, new RegisterOperand(Register.R10), argument1);
            this.WriteRegisterToDestination(this.Statement.Return, Register.R10);
        }

        private void CreateDivisionInstruction()
        {
            // Clear RDX
            this.WriteBinaryInstruction(Opcode.XOR, new RegisterOperand(Register.RDX), new RegisterOperand(Register.RDX));
            this.PlaceArgumentInRegister(Statement.Left, Register.RAX);
            this.PlaceArgumentInRegister(Statement.Right, Register.R10);
            
            this.WriteInstruction(new SingleOpcodeInstruction(SingleArgOpcode.IDIV, new RegisterOperand(Register.R10)));

            this.WriteRegisterToDestination(this.Statement.Return, Register.RAX);
        }

        private void CreateModuloInstruction()
        {
            // Clear RDX
            this.WriteBinaryInstruction(Opcode.XOR, new RegisterOperand(Register.RDX), new RegisterOperand(Register.RDX));

            PlaceArgumentInRegister(Statement.Left, Register.RAX);
            PlaceArgumentInRegister(Statement.Right, Register.R10);

            this.WriteInstruction(new SingleOpcodeInstruction(SingleArgOpcode.IDIV, new RegisterOperand(Register.R10)));

            this.WriteRegisterToDestination(this.Statement.Return, Register.RDX);
        }

        private void CreateExponentiantionInstruction()
        {
            PlaceArgumentInRegister(Statement.Left, Register.XMM14);
            PlaceArgumentInRegister(Statement.Right, Register.XMM15);

            this.WriteBinaryInstruction(Opcode.SUB, new RegisterOperand(Register.RSP), new ConstantOperand(40));
            this.WriteInstruction(new CallInstruction("Power"));
            this.WriteBinaryInstruction(Opcode.ADD, new RegisterOperand(Register.RSP), new ConstantOperand(40));

            this.WriteBinaryInstruction(Opcode.MOVD, new RegisterOperand(Register.R10), new RegisterOperand(Register.XMM14));
            this.WriteRegisterToDestination(this.Statement.Return, Register.R10);
        }

        private void CreateComparisonInstruction()
        {
            PlaceArgumentInRegister(Statement.Left, Register.R10);
            PlaceArgumentInRegister(Statement.Right, Register.R11);

            Opcode opcode;
            switch (Statement.Operator)
            {
                case BinaryOperator.Equal:
                    opcode = Opcode.CMOVE;
                    break;
                case BinaryOperator.NotEqual:
                    opcode = Opcode.CMOVNE;
                    break;
                case BinaryOperator.Less:
                    opcode = Opcode.CMOVL;
                    break;
                case BinaryOperator.LessEqual:
                    opcode = Opcode.CMOVLE;
                    break;
                case BinaryOperator.Greater:
                    opcode = Opcode.CMOVG;
                    break;
                case BinaryOperator.GreaterEqual:
                    opcode = Opcode.CMOVGE;
                    break;
                default:
                    throw new ArgumentOutOfRangeException("statement", "Unsupported operator");
            }

            this.WriteBinaryInstruction(Opcode.XOR, new RegisterOperand(Register.RAX), new RegisterOperand(Register.RAX));
            this.WriteBinaryInstruction(Opcode.CMP, new RegisterOperand(Register.R10), new RegisterOperand(Register.R11));

            PlaceArgumentInRegister(new IntConstantArgument(1), Register.R10);
            this.WriteBinaryInstruction(opcode, new RegisterOperand(Register.RAX), new RegisterOperand(Register.R10));

            this.WriteRegisterToDestination(this.Statement.Return, Register.RAX);
        }
    }
}
