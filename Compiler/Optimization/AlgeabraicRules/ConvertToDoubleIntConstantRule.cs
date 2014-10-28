namespace Compiler.Optimization.AlgeabraicRules
{
    using Compiler.ControlFlowGraph;

    public class ConvertToDoubleIntConstantRule : IAlgeabraicRule
    {
        public bool ProcessStatement(Statement statement)
        {
            var convertToDoubleStatement = statement as ConvertToDoubleStatement;
            if (convertToDoubleStatement == null) return false;

            var intConstantArgument = convertToDoubleStatement.Argument as IntConstantArgument;
            if (intConstantArgument == null) return false;

            var newStatement = new AssignStatement(
                convertToDoubleStatement.Return,
                new DoubleConstantArgument(intConstantArgument.Value));

            CFGUtilities.ReplaceStatement(statement, newStatement);
            
            return true;
        }
    }
}
