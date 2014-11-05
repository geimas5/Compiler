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
                || Statement.Operator == BinaryOperator.Multiply
                || Statement.Operator == BinaryOperator.Subtract 
                || Statement.Operator == BinaryOperator.Mod
                || Statement.Operator == BinaryOperator.Exponensiation)
            {
                this.CreateMathInstruction();
            }
            else if (this.Statement.Operator == BinaryOperator.Equal
                || this.Statement.Operator == BinaryOperator.Greater
                || this.Statement.Operator == BinaryOperator.GreaterEqual
                || this.Statement.Operator == BinaryOperator.Less
                || this.Statement.Operator == BinaryOperator.LessEqual
                || this.Statement.Operator == BinaryOperator.NotEqual)
            {
                this.CreateComparisonInstruction();
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

        private void CreateComparisonInstruction()
        {
            if (this.Statement.Left.Type.PrimitiveType == PrimitiveType.Double)
            {
                this.CreateFloatingPointComparisonInstruction();
            }
            else
            {
                this.CreateIntegerComparisonInstruction();
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
                    this.CreateModuloInstruction();
                    return;
                default:
                    throw new ArgumentException("operation operation not supported");
            }

            var argument1 = new RegisterOperand(Register.R10);
            Operand argument2 = this.GetRightIntegerOperand(false);

            this.MoveData(this.ArgumentToOperand(Statement.Left, Register.R10, Register.XMM14), new RegisterOperand(Register.R10), Register.R10, Register.XMM14);

            this.WriteBinaryInstruction(opcode, argument1, argument2);

            this.MoveData(new RegisterOperand(Register.R10), this.DestinationToOperand(this.Statement.Return, Register.R11), Register.R11, Register.XMM14);
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
                    // This should not happen, exponentiantion should have 
                    // been converted to calls to power before here.
                default:
                    throw new ArgumentException("operation operation not supported");
            }

            var rightOperand = this.ArgumentToOperand(Statement.Right, Register.R14, Register.XMM15);
            if (!RegisterUtility.IsXMMOperand(rightOperand))
            {
                this.MoveData(rightOperand, new RegisterOperand(Register.XMM15), Register.R10, Register.XMM15);
                rightOperand = new RegisterOperand(Register.XMM15);
            }

            Operand leftOperand = new RegisterOperand(Register.XMM14);
            this.MoveData(this.ArgumentToOperand(Statement.Left, Register.R10, Register.XMM14), leftOperand, Register.R10, Register.XMM15);

            this.WriteBinaryInstruction(opcode, leftOperand, rightOperand);

            var destinationOperand = this.DestinationToOperand(Statement.Return, Register.R10);
            this.MoveData(leftOperand, destinationOperand, Register.R10, Register.XMM14);
        }

        private void CreateDivisionInstruction()
        {
            // Clear RDX
            this.WriteBinaryInstruction(Opcode.XOR, new RegisterOperand(Register.RDX), new RegisterOperand(Register.RDX));

            this.MoveData(this.ArgumentToOperand(Statement.Left, Register.RAX, Register.XMM14), new RegisterOperand(Register.RAX), Register.RAX, Register.XMM14);

            this.WriteInstruction(new SingleOpcodeInstruction(SingleArgOpcode.IDIV, this.GetRightIntegerOperand(true, false)));

            this.MoveData(new RegisterOperand(Register.RAX), this.DestinationToOperand(this.Statement.Return, Register.R10), Register.R10, Register.XMM14);
        }

        private void CreateModuloInstruction()
        {
            // Clear RDX
            this.WriteBinaryInstruction(Opcode.XOR, new RegisterOperand(Register.RDX), new RegisterOperand(Register.RDX));

            this.MoveData(this.ArgumentToOperand(Statement.Left, Register.RAX, Register.XMM14), new RegisterOperand(Register.RAX), Register.RAX, Register.XMM14);

            this.WriteInstruction(new SingleOpcodeInstruction(SingleArgOpcode.IDIV, this.GetRightIntegerOperand(true, false)));

            this.MoveData(new RegisterOperand(Register.RDX), this.DestinationToOperand(this.Statement.Return, Register.R10), Register.RAX, Register.XMM14);
        }

        private void CreateIntegerComparisonInstruction()
        {
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

            var leftOperand = this.GetLeftBooleanOperand();
            var rightOperand = this.GetRightIntegerOperand(leftOperand is MemoryOperand);

            this.WriteBinaryInstruction(Opcode.CMP, leftOperand, rightOperand);

            this.MoveData(new ConstantOperand(1), new RegisterOperand(Register.R10), Register.R10, Register.XMM14);
            this.WriteBinaryInstruction(Opcode.MOV, new RegisterOperand(Register.R11), new ConstantOperand(0));
            this.WriteBinaryInstruction(opcode, new RegisterOperand(Register.R11), new RegisterOperand(Register.R10));

            var destinationOperand = this.DestinationToOperand(Statement.Return, Register.R10);
            this.MoveData(new RegisterOperand(Register.R11), destinationOperand, Register.R10, Register.XMM14);
        }

        private void CreateFloatingPointComparisonInstruction()
        {
            int comparisonType;

            switch (Statement.Operator)
            {
                case BinaryOperator.Equal:
                    comparisonType = (int)SDComparisonType.Equal;
                    break;
                case BinaryOperator.NotEqual:
                    comparisonType = (int)SDComparisonType.NotEqual;
                    break;
                case BinaryOperator.Less:
                    comparisonType = (int)SDComparisonType.LessThan;
                    break;
                case BinaryOperator.LessEqual:
                    comparisonType = (int)SDComparisonType.LessEqual;
                    break;
                case BinaryOperator.Greater:
                    comparisonType = (int)SDComparisonType.NotLessEqual;
                    break;
                case BinaryOperator.GreaterEqual:
                    comparisonType = (int)SDComparisonType.NotLessThan;
                    break;
                default:
                    throw new ArgumentOutOfRangeException("statement", "Unsupported operator");
            }

            Operand leftOperand = new RegisterOperand(Register.XMM14);
            Operand rightOperand = new RegisterOperand(Register.XMM15);
            this.MoveData(this.ArgumentToOperand(Statement.Left, Register.R10, Register.XMM14), leftOperand, Register.R10, Register.XMM14);
            this.MoveData(this.ArgumentToOperand(Statement.Right, Register.R11, Register.XMM15), rightOperand, Register.R11, Register.XMM15);

            this.WriteTrinaryInstruction(TrinaryOpcode.CMPSD, leftOperand, rightOperand, new ConstantOperand(comparisonType));

            var destination = this.DestinationToOperand(Statement.Return, Register.R10);
            this.MoveData(leftOperand, destination, Register.R10, Register.XMM14);

            this.MoveData(new ConstantOperand(1), new RegisterOperand(Register.R10), Register.R10, Register.XMM14);
            this.WriteBinaryInstruction(Opcode.AND, destination, new RegisterOperand(Register.R10));
        }

        private Operand GetLeftBooleanOperand()
        {
            var leftOperand = this.ArgumentToOperand(Statement.Left, Register.R11, Register.XMM14);

            if (leftOperand is MemoryOperand)
            {
                this.MoveData(leftOperand, new RegisterOperand(Register.R11), Register.R11, Register.XMM14);

                leftOperand = new RegisterOperand(Register.R11);
            }

            if (leftOperand is ConstantOperand)
            {
                this.MoveData(leftOperand, new RegisterOperand(Register.R11), Register.R11, Register.XMM14);

                leftOperand = new RegisterOperand(Register.R11);
            }

            return leftOperand;
        }

        private Operand GetRightIntegerOperand(bool leftIsMemory, bool canBeImmediate = true)
        {
            var rightOperand = this.ArgumentToOperand(Statement.Right, Register.R11, Register.XMM14);

            if (leftIsMemory && rightOperand is MemoryOperand)
            {
                this.MoveData(rightOperand, new RegisterOperand(Register.R11), Register.R11, Register.XMM14);

                rightOperand = new RegisterOperand(Register.R11);
            }

            if (!canBeImmediate && rightOperand is ConstantOperand)
            {
                this.MoveData(rightOperand, new RegisterOperand(Register.R11), Register.R11, Register.XMM14);

                rightOperand = new RegisterOperand(Register.R11);
            }

            return rightOperand;
        }
    }
}
