
namespace Compiler.Optimization
{
    using System.Collections.Generic;
    using System.Linq;

    using Compiler.ControlFlowGraph;
    using Compiler.Optimization.AlgeabraicRules;

    public class AlgebraicOptimization : OptimizerBase
    {
        private static readonly List<IAlgeabraicRule> rules = new List<IAlgeabraicRule>();

        public AlgebraicOptimization()
        {
            if (!rules.Any())
            {
                rules.Add(new IntBinaryOperatorConstantRule());
                rules.Add(new IntAddZeroRule());
                rules.Add(new IntMultiplyZeroRule());
                rules.Add(new IntMultiplyOneRule());
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
            foreach (var algeabraicRule in rules)
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
