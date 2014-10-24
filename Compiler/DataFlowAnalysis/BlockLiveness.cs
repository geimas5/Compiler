namespace Compiler.DataFlowAnalysis
{
    using System.Collections.Generic;
    using System.Linq;

    using Compiler.ControlFlowGraph;
    using Compiler.SymbolTable;

    public class BlockLiveness : DataflowBlock
    {
        private readonly BasicBlock block;

        public VariableBitset Use { get; private set; }

        public VariableBitset Def { get; private set; }

        public BlockLiveness(BasicBlock block, LivenessAnalysis analysis)
            : base(analysis.VariableRegister)
        {
            this.Use = new VariableBitset(analysis.VariableRegister);
            this.Def = new VariableBitset(analysis.VariableRegister);

            this.block = block;
            this.CalculateGetDef();
        }

        public IEnumerable<Statement> FindDeadStatements()
        {
            var liveVariables = this.Out.Clone();

            foreach (var statement in this.block.Reverse())
            {
                var returningStatement = statement as IReturningStatement;
                if (returningStatement != null && returningStatement.Return is VariableDestination
                    && !liveVariables.Get(((VariableDestination)returningStatement.Return).Variable))
                {
                    yield return statement;

                    liveVariables.Clear(((VariableDestination)returningStatement.Return).Variable);
                }

                foreach (var variable in StatementHelper.GetStatementVariableUsages(statement))
                {
                    liveVariables.Set(variable);
                }
            }
        }  

        private void CalculateGetDef()
        {
            foreach (var statement in this.block.Reverse())
            {
                VariableSymbol definition = null;

                var returningStatement = statement as IReturningStatement;
                if (returningStatement != null && returningStatement.Return is VariableDestination)
                {
                    definition = ((VariableDestination)returningStatement.Return).Variable;
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
