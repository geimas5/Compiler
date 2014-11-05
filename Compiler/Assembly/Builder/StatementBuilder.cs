namespace Compiler.Assembly.Builder
{
    using System;
    using System.Collections.Generic;

    using Compiler.ControlFlowGraph;

    public abstract class StatementBuilder<T> where T : Statement
    {
        public T Statement { get; private set; }

        public Procedure Procedure { get; private set; }

        private IList<Instruction> instructions = new List<Instruction>();

        public IEnumerable<Instruction> Build(T statement, Procedure procedure)
        {
            instructions = new List<Instruction>();

            this.Statement = statement;
            this.Procedure = procedure;
            this.Build();

            return this.instructions;
        }

        protected abstract void Build();

        protected void WriteInstruction(Instruction instruction)
        {
            instructions.Add(instruction);
        }

        protected void WriteBinaryInstruction(Opcode opcode, Operand argument1, Operand argument2)
        {
            this.WriteInstruction(new BinaryOpCodeInstruction(opcode, argument1, argument2));
        }

        protected void WriteTrinaryInstruction(TrinaryOpcode opcode, Operand argument1, Operand argument2, Operand argument3)
        {
            this.WriteInstruction(new TrinaryInstruction(opcode, argument1, argument2, argument3));
        }

        protected void WriteUnaryInstruction(SingleArgOpcode opcode, Operand argument1)
        {
            this.WriteInstruction(new SingleOpcodeInstruction(opcode, argument1));
        }

        public void MoveData(Operand source, Operand destination, Register tempGenRegister, Register tempXmmRegister)
        {
            if (source is RegisterOperand && destination is RegisterOperand)
            {
                this.MoveData((RegisterOperand)source, (RegisterOperand)destination);
            }
            else if (source is RegisterOperand && destination is MemoryOperand)
            {
                this.MoveData((RegisterOperand)source, (MemoryOperand)destination);
            }
            else if (source is MemoryOperand && destination is RegisterOperand)
            {
                this.MoveData((MemoryOperand)source, (RegisterOperand)destination);
            }
            else if (source is MemoryOperand && destination is MemoryOperand)
            {
                this.MoveData((MemoryOperand)source, (MemoryOperand)destination);
            }
            else if (source is ConstantOperand && destination is RegisterOperand)
            {
                this.MoveData((ConstantOperand)source, (RegisterOperand)destination);
            }
            else if (source is ConstantOperand && destination is MemoryOperand)
            {
                this.MoveData((ConstantOperand)source, (MemoryOperand)destination, tempGenRegister);
            }
            else if (source is DoubleConstantOperand && destination is RegisterOperand)
            {
                this.MoveData((DoubleConstantOperand)source, (RegisterOperand)destination);
            }
            else if (source is DoubleConstantOperand && destination is MemoryOperand)
            {
                this.MoveData((DoubleConstantOperand)source, (MemoryOperand)destination, tempGenRegister, tempXmmRegister);
            }
            else if (source is StringConstantOperand && destination is RegisterOperand)
            {
                this.MoveData((StringConstantOperand)source, (RegisterOperand)destination);
            }
            else if (source is StringConstantOperand && destination is MemoryOperand)
            {
                this.MoveData((StringConstantOperand)source, (MemoryOperand)destination, tempGenRegister);
            }
            else
            {
                throw new NotImplementedException("The transfer between the operandtypes specified is not implemented");
            }
        }

        private void MoveData(RegisterOperand source, RegisterOperand destination)
        {
            if (destination.Register == source.Register)
            {
                // Do nothing as the data is already in the same location.
            }
            else if (RegisterUtility.IsXMMOperand(source) && RegisterUtility.IsXMMOperand(destination))
            {
                this.WriteBinaryInstruction(Opcode.MOVSD, destination, source);
            }
            else if (RegisterUtility.IsXMMOperand(source) || RegisterUtility.IsXMMOperand(destination))
            {
                this.WriteBinaryInstruction(Opcode.MOVD, destination, source);
            }
            else
            {
                this.WriteBinaryInstruction(Opcode.MOV, destination, source);
            }
        }

        private void MoveData(RegisterOperand source, MemoryOperand destination)
        {
            if (RegisterUtility.IsXMMOperand(source))
            {
                this.WriteBinaryInstruction(Opcode.MOVD, new RegisterOperand(Register.R11), source);
                this.WriteBinaryInstruction(Opcode.MOV, destination, new RegisterOperand(Register.R11));
            }
            else
            {
                this.WriteBinaryInstruction(Opcode.MOV, destination, source);
            }
        }

        private void MoveData(MemoryOperand source, RegisterOperand destination)
        {
            if (RegisterUtility.IsXMMOperand(destination))
            {
                this.WriteBinaryInstruction(Opcode.MOV, new RegisterOperand(Register.R11), source);
                this.WriteBinaryInstruction(Opcode.MOVD, destination, new RegisterOperand(Register.R11));
            }
            else
            {
                this.WriteBinaryInstruction(Opcode.MOV, destination, source);
            }
        }

        private void MoveData(MemoryOperand source, MemoryOperand destination)
        {
            this.WriteBinaryInstruction(Opcode.MOV, new RegisterOperand(Register.R11), source);
            this.WriteBinaryInstruction(Opcode.MOV, destination, new RegisterOperand(Register.R11));
        }

        private void MoveData(DoubleConstantOperand source, RegisterOperand destination)
        {
            if (RegisterUtility.IsXMMOperand(destination))
            {
                this.WriteBinaryInstruction(Opcode.MOVD, destination, source);
            }
            else
            {
                this.WriteBinaryInstruction(Opcode.MOVD, new RegisterOperand(Register.XMM15), source);
                this.WriteBinaryInstruction(Opcode.MOVSD, destination, new RegisterOperand(Register.XMM15));
            }
        }

        private void MoveData(DoubleConstantOperand source, MemoryOperand destination, Register tempGenRegister, Register tempXmmRegister)
        {
            this.WriteBinaryInstruction(Opcode.MOVD, new RegisterOperand(tempXmmRegister), source);
            this.WriteBinaryInstruction(Opcode.MOVD, new RegisterOperand(tempGenRegister), new RegisterOperand(tempXmmRegister));
            this.WriteBinaryInstruction(Opcode.MOV, destination, new RegisterOperand(tempGenRegister));
        }

        private void MoveData(StringConstantOperand source, RegisterOperand destination)
        {
            this.WriteBinaryInstruction(Opcode.LEA, destination, source);
        }

        private void MoveData(StringConstantOperand source, MemoryOperand destination, Register tempGenRegister)
        {
            this.WriteBinaryInstruction(Opcode.LEA, new RegisterOperand(tempGenRegister), source);
            this.WriteBinaryInstruction(Opcode.MOV, destination, new RegisterOperand(tempGenRegister));
        }

        private void MoveData(ConstantOperand source, RegisterOperand destination)
        {
            this.WriteBinaryInstruction(Opcode.MOV, destination, source);
        }

        private void MoveData(ConstantOperand source, MemoryOperand destination, Register tempGenRegister)
        {
            this.WriteBinaryInstruction(Opcode.MOV, new RegisterOperand(tempGenRegister), source);
            this.WriteBinaryInstruction(Opcode.MOV, destination, new RegisterOperand(tempGenRegister));
        }

        protected Operand ArgumentToOperand(Argument argument, Register tempGenRegister, Register tempXmmRegister)
        {
            var variableArgument = argument as VariableArgument;
            if (variableArgument != null)
            {
                if (variableArgument.Variable.Register.HasValue)
                {
                    return new RegisterOperand(variableArgument.Variable.Register.Value);
                }

                return Procedure.GetVarialeLocation(variableArgument.Variable);
            }

            var doubleConstantArgument = argument as DoubleConstantArgument;
            if (doubleConstantArgument != null)
            {
                var realId = Procedure.AssemblyFile.DataSection.AddOrGetRealId(doubleConstantArgument.Value);
                return new DoubleConstantOperand(realId);
            }

            var globalArgument = argument as GlobalArgument;
            if (globalArgument != null)
            {
                return new StringConstantOperand(globalArgument.Name);
            }

            var integerConstantArgument = argument as IntConstantArgument;
            if (integerConstantArgument != null)
            {
                return new ConstantOperand(integerConstantArgument.Value);
            }

            var booleanConstantArgument = argument as BooleanConstantArgument;
            if (booleanConstantArgument != null)
            {
                return new ConstantOperand(Convert.ToInt32(booleanConstantArgument.Value));
            }

            var pointerArgument = argument as PointerArgument;
            if (pointerArgument != null)
            {
                if (pointerArgument.Variable.Register.HasValue)
                {
                    return new MemoryOperand(pointerArgument.Variable.Register.Value);
                }

                this.MoveData(this.Procedure.GetVarialeLocation(pointerArgument.Variable), new RegisterOperand(tempGenRegister));
                return new MemoryOperand(tempGenRegister);
            }


            throw new NotImplementedException();
        }

        protected Operand DestinationToOperand(Destination destination, Register tempRegister)
        {
            var variableDestination = destination as VariableDestination;
            if (variableDestination != null)
            {
                if (variableDestination.Variable.Register.HasValue)
                {
                    return new RegisterOperand(variableDestination.Variable.Register.Value);
                }

                return Procedure.GetVarialeLocation(variableDestination.Variable);
            }

            var pointerDestination = destination as PointerDestination;
            if (pointerDestination != null)
            {
                if (pointerDestination.Destination.Register.HasValue)
                {
                    return new MemoryOperand(pointerDestination.Destination.Register.Value);
                }

                var variable = Procedure.GetVarialeLocation(pointerDestination.Destination);
                this.MoveData(variable, new RegisterOperand(tempRegister));

                return new MemoryOperand(tempRegister);
            }

            throw new NotImplementedException();
        }
    }
}
