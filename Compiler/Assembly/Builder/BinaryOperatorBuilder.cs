namespace Compiler.Assembly.Builder
{
    using System;
    using System.Collections.Generic;

    using Compiler.ControlFlowGraph;
    using Compiler.SyntaxTree;

    using Type = Compiler.Type;

    internal sealed class BinaryOperatorBuilder
    {
        private readonly BinaryOperatorStatement statement;

        private readonly Procedure procedure;

        private readonly List<Instruction> instructions = new List<Instruction>();

        private BinaryOperatorBuilder(BinaryOperatorStatement binaryOperatorStatement, Procedure procedure)
        {
            this.statement = binaryOperatorStatement;
            this.procedure = procedure;
        }

        public static IEnumerable<Instruction> BuildOperator(
            BinaryOperatorStatement binaryOperatorStatement,
            Procedure procedure)
        {
            var builder = new BinaryOperatorBuilder(binaryOperatorStatement, procedure);

            return builder.Build();
        }

        private IEnumerable<Instruction> Build()
        {
            if (statement.Operator == BinaryOperator.Divide 
                || statement.Operator == BinaryOperator.Add
                || statement.Operator == BinaryOperator.Equal 
                || statement.Operator == BinaryOperator.Multiply
                || statement.Operator == BinaryOperator.Subtract 
                || statement.Operator == BinaryOperator.Mod
                || statement.Operator == BinaryOperator.Exponensiation)
            {
                CreateMathInstruction();
            }
            else if (this.statement.Operator == BinaryOperator.Equal 
                || this.statement.Operator == BinaryOperator.Greater
                || this.statement.Operator == BinaryOperator.GreaterEqual
                || this.statement.Operator == BinaryOperator.Less 
                || this.statement.Operator == BinaryOperator.LessEqual
                || this.statement.Operator == BinaryOperator.NotEqual)
            {
                CreateComparisonInstruction();
            }
            else
            {
                throw new ArgumentOutOfRangeException("statement", "Unsupported operator");    
            }

            return this.instructions;
        }

        private void CreateMathInstruction()
        {
            if (this.statement.Return.Type.PrimitiveType == PrimitiveType.Double)
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

            switch (this.statement.Operator)
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
            Operand argument2;
            instructions.AddRange(BuilderHelper.PlaceArgumentInRegister(Register.R10, statement.Left, procedure));

            if (statement.Right is IntConstantArgument)
            {
                argument2 = new ConstantOperand(((IntConstantArgument)statement.Right).Value);
            }
            else
            {
                argument2 = new RegisterOperand(Register.R11);
                instructions.AddRange(BuilderHelper.PlaceArgumentInRegister(Register.R11, statement.Right, procedure));    
            }

            instructions.Add(new BinaryOpCodeInstruction(opcode, argument1, argument2));

            instructions.AddRange(BuilderHelper.WriteRegisterToDestination(statement.Return, Register.R10, procedure));
        } 

        private void CreateFloatingPointMathInstructions()
        {
            Opcode opcode;

            switch (statement.Operator)
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

            var argument1 = new RegisterOperand(Register.XMM0);
            var argument2 = new RegisterOperand(Register.XMM1);
            instructions.AddRange(BuilderHelper.PlaceArgumentInRegister(Register.XMM0, statement.Left, procedure));
            instructions.AddRange(BuilderHelper.PlaceArgumentInRegister(Register.XMM1, statement.Right, procedure));

            instructions.Add(new BinaryOpCodeInstruction(opcode, argument1, argument2));

            instructions.Add(new BinaryOpCodeInstruction(Opcode.MOVD, new RegisterOperand(Register.R10), argument1));
            instructions.AddRange(BuilderHelper.WriteRegisterToDestination(statement.Return, Register.R10, procedure));
        }

        private void CreateDivisionInstruction()
        {
            // Clear RDX
            instructions.Add(new BinaryOpCodeInstruction(Opcode.XOR, new RegisterOperand(Register.RDX), new RegisterOperand(Register.RDX)));

            foreach (var instruction in BuilderHelper.PlaceArgumentInRegister(Register.RAX, statement.Left, procedure))
            {
                instructions.Add(instruction);
            }

            foreach (var instruction in BuilderHelper.PlaceArgumentInRegister(Register.R10, statement.Right, procedure))
            {
                instructions.Add(instruction);
            }

            instructions.Add(new SingleOpcodeInstruction(SingleArgOpcode.IDIV, new RegisterOperand(Register.R10)));
            instructions.AddRange(BuilderHelper.WriteRegisterToDestination(statement.Return, Register.RAX, procedure));
        }

        private void CreateModuloInstruction()
        {
            // Clear RDX
            instructions.Add(new BinaryOpCodeInstruction(Opcode.XOR, new RegisterOperand(Register.RDX), new RegisterOperand(Register.RDX)));

            instructions.AddRange(BuilderHelper.PlaceArgumentInRegister(Register.RAX, statement.Left, procedure));
            instructions.AddRange(BuilderHelper.PlaceArgumentInRegister(Register.R10, statement.Right, procedure));

            instructions.Add(new SingleOpcodeInstruction(SingleArgOpcode.IDIV, new RegisterOperand(Register.R10)));

            instructions.AddRange(BuilderHelper.WriteRegisterToDestination(statement.Return, Register.RDX, procedure));
        }

        private void CreateExponentiantionInstruction()
        {
            instructions.AddRange(BuilderHelper.PlaceArgumentInRegister(Register.XMM0, statement.Left, procedure));
            instructions.AddRange(BuilderHelper.PlaceArgumentInRegister(Register.XMM1, statement.Right, procedure));

            instructions.Add(new BinaryOpCodeInstruction(Opcode.SUB, new RegisterOperand(Register.RSP), new ConstantOperand(40)));
            instructions.Add(new CallInstruction("Power"));
            instructions.Add(new BinaryOpCodeInstruction(Opcode.ADD, new RegisterOperand(Register.RSP), new ConstantOperand(40)));

            instructions.Add(new BinaryOpCodeInstruction(Opcode.MOVD, new RegisterOperand(Register.R10), new RegisterOperand(Register.XMM0)));
            instructions.AddRange(BuilderHelper.WriteRegisterToDestination(statement.Return, Register.R10, procedure));
        }

        private void CreateComparisonInstruction()
        {
            instructions.AddRange(BuilderHelper.PlaceArgumentInRegister(Register.R10, statement.Left, procedure));
            instructions.AddRange(BuilderHelper.PlaceArgumentInRegister(Register.R11, statement.Right, procedure));

            Opcode opcode;
            switch (statement.Operator)
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

            instructions.Add(new BinaryOpCodeInstruction(Opcode.XOR, new RegisterOperand(Register.RAX), new RegisterOperand(Register.RAX)));
            instructions.Add(new BinaryOpCodeInstruction(Opcode.CMP, new RegisterOperand(Register.R10), new RegisterOperand(Register.R11)));

            instructions.AddRange(BuilderHelper.PlaceArgumentInRegister(Register.R10, new IntConstantArgument(1), procedure));
            instructions.Add(new BinaryOpCodeInstruction(opcode, new RegisterOperand(Register.RAX), new RegisterOperand(Register.R10)));

            instructions.AddRange(BuilderHelper.WriteRegisterToDestination(statement.Return, Register.RAX, procedure));
        }        

        private bool IsDestinationSameAsLeft()
        {
            if (this.statement.Return is VariableDestination && this.statement.Left is VariableArgument)
            {
                // Reference comparison is intended
                return ((VariableDestination)statement.Return).Variable == ((VariableArgument)statement.Left).Variable;
            }

            return false;
        }
    }
}
