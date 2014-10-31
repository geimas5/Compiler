namespace Compiler.Assembly.Builder
{
    using System;

    using Compiler.ControlFlowGraph;

    public sealed class AssignStatementBuilder : StatementBuilder<AssignStatement>
    {
        protected override void Build()
        {
            var leftOperand = this.GetLeftOperand();
            var rightOperand = this.GetRightOperand(leftOperand is MemoryOperand);

            if (leftOperand is RegisterOperand && RegisterUtility.IsXMM(((RegisterOperand)leftOperand).Register)
                && rightOperand is MemoryOperand)
            {
                this.WriteBinaryInstruction(Opcode.MOV, new RegisterOperand(Register.R10), rightOperand);
                this.WriteBinaryInstruction(Opcode.MOVD, leftOperand, new RegisterOperand(Register.R10));
            }
            else
            {
                this.WriteBinaryInstruction(Opcode.MOV, leftOperand, rightOperand);    
            }
        }

        private Operand GetRightOperand(bool leftIsMemory)
        {
            var variableArgument = Statement.Argument as VariableArgument;
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

            var intConstantArgument = Statement.Argument as IntConstantArgument;
            if (intConstantArgument != null)
            {
                if (leftIsMemory)
                {
                    this.PlaceArgumentInRegister(intConstantArgument, Register.R11);
                    return new RegisterOperand(Register.R11);
                }

                return new ConstantOperand(intConstantArgument.Value);
            }

            var globalConstantArgument = Statement.Argument as GlobalArgument;
            if (globalConstantArgument != null)
            {
                this.PlaceArgumentInRegister(globalConstantArgument, Register.R11);
                return new RegisterOperand(Register.R11);
            }

            var doubleConstantArgument = Statement.Argument as DoubleConstantArgument;
            if (doubleConstantArgument != null)
            {
                this.PlaceArgumentInRegister(doubleConstantArgument, Register.R11);
                return new RegisterOperand(Register.R11);
            }

            var pointerArgument = Statement.Argument as PointerArgument;
            if (pointerArgument != null)
            {
                this.PlaceArgumentInRegister(pointerArgument, Register.R11);
                return new RegisterOperand(Register.R11);
            }

            throw new NotImplementedException();
        }

        private Operand GetLeftOperand()
        {
            var variableDestination = Statement.Return as VariableDestination;
            if (variableDestination != null)
            {
                if (variableDestination.Variable.Register.HasValue)
                {
                    return new RegisterOperand(variableDestination.Variable.Register.Value);    
                }

                return this.Procedure.GetVarialeLocation(variableDestination.Variable);
            }

            var pointerDestination = Statement.Return as PointerDestination;
            if (pointerDestination != null)
            {
                Register memoryLocationRegister;

                if (pointerDestination.Destination.Register.HasValue)
                {
                    memoryLocationRegister = pointerDestination.Destination.Register.Value;
                }
                else
                {
                    this.PlaceArgumentInRegister(new VariableArgument(pointerDestination.Destination), Register.R10);
                    memoryLocationRegister = Register.R10;
                }


                return new MemoryOperand(memoryLocationRegister);
            }

            throw new NotSupportedException();
        }
    }
}
