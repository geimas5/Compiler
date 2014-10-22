namespace Compiler.DataFlowAnalysis
{
    public abstract class DataflowBlock
    {
        public DataflowBlock(VariableRegister variableRegister)
        {
            this.In = new VariableBitset(variableRegister);
            this.Out = new VariableBitset(variableRegister);
        }

        public VariableBitset In { get; set; }

        public VariableBitset Out { get; set; }
    }
}
