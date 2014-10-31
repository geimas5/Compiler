namespace Compiler.Optimization
{
    using System.Collections.Generic;
    using System.Linq;

    using Compiler.ControlFlowGraph;
    using Compiler.Optimization.AlgeabraicRules;

    public class AlgebraicOptimization : OptimizerBase
    {
        private static readonly List<IAlgeabraicRule> Rules = new List<IAlgeabraicRule>();

        public AlgebraicOptimization()
        {
            if (!Rules.Any())
            {
                Rules.Add(new IntBinaryOperatorConstantRule());
                Rules.Add(new IntAddZeroRule());
                Rules.Add(new IntMultiplyZeroRule());
                Rules.Add(new IntMultiplyOneRule());
                Rules.Add(new DoubleBinaryOperatorConstantRule());
                Rules.Add(new ConvertToDoubleIntConstantRule());
            }
        }

        public override void VisitBlock(BasicBlock block)
        {
            foreach (var statement in block.ToArray())
            {
                this.ProcessStatement(statement);
            }
        }

        private void ProcessStatement(Statement statement)
        {
            foreach (var algeabraicRule in Rules)
            {
                if (algeabraicRule.ProcessStatement(statement))
                {
                    this.SetSomethingChanged();
                    return;
                }
            }
        }
    }
}
