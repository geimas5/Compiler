namespace Compiler.DataFlowAnalysis
{
    using System.Linq;

    using Compiler.ControlFlowGraph;
    using Compiler.SymbolTable;

    public class BlockLiveness : DataflowBlock
    {
        private readonly BasicBlock block;

        private readonly LivenessAnalysis analysis;

        public VariableBitset Use { get; private set; }

        public VariableBitset Def { get; private set; }


        public BlockLiveness(BasicBlock block, LivenessAnalysis analysis)
            : base(analysis.VariableRegister)
        {
            this.Use = new VariableBitset(analysis.VariableRegister);
            this.Def = new VariableBitset(analysis.VariableRegister);

            this.block = block;
            this.analysis = analysis;
            this.CalculateGetDef();
        }

        public void CalculateGetDef()
        {
            foreach (var statement in this.block.Reverse())
            {
                VariableSymbol definition = null;

                var returningStatement = statement as IReturningStatement;
                if (returningStatement != null)
                {
                    definition = returningStatement.Return;
                }

                if (definition != null)
                {
                    this.Use.Clear(definition);
                }

                foreach (var variable in StatementHelper.GetStatementVariableUsages(statement).Where(variable => !this.Def.Get(variable)))
                {
                    this.Use.Set(variable);
                }
                
                if (definition != null)
                {
                    this.Def.Set(definition);
                }
            }
        }
    }
}
