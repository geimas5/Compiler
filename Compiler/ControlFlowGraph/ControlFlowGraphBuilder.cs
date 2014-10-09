namespace Compiler.ControlFlowGraph
{
    using System;

    using Compiler.SymbolTable;
    using Compiler.SyntaxTree;

    using ConstantExpression = Compiler.SyntaxTree.ConstantExpression;
    using Type = Compiler.Type;

    public class ControlFlowGraphBuilder : Visitor<BasicBlock>
    {
        private ControlFlowGraph controlFlowGraph;

        public ControlFlowGraph BuildGraph(ProgramNode rootNode)
        {
            this.controlFlowGraph = new ControlFlowGraph();

            foreach (var node in rootNode.Functions)
            {
                this.controlFlowGraph.Functions.Add(node.Accept(this));
            }

            return this.controlFlowGraph;
        }

        public override BasicBlock Visit(VoidFunctionDecleration node)
        {
            BasicBlock block = null;

            foreach (var statement in node.Statements)
            {
                var subBlock = statement.Accept(this);

                block = block == null ? subBlock : block.Join(subBlock);
            }

            return block;
        }

        public override BasicBlock Visit(ReturningFunctionDecleration node)
        {
            BasicBlock block = null;

            foreach (var statement in node.Statements)
            {
                var subBlock = statement.Accept(this);

                block = block == null ? subBlock : block.Join(subBlock);
            }

            return block;
        }

        public override BasicBlock Visit(ExpressionStatement node)
        {
            return node.Expression.Accept(this);
        }

        public override BasicBlock Visit(ConstantExpression node)
        {
            return node.Constant.Accept(this);
        }

        public override BasicBlock Visit(InitializedVariableDecleration node)
        {
            var initBlock = node.Initialization.Accept(this);
            var argument = new VariableArgument(((ReturningStatement)initBlock.Exit).Return);

            var statemment = new AssignStatement((VariableSymbol)node.Variable.Name.Symbol, argument);

            return initBlock.Append(statemment);
        }

        public override BasicBlock Visit(IntegerConstant node)
        {
            var variable = this.MakeTempVariable(node, new Type(PrimitiveType.Int));

            var assignment = new AssignStatement(variable, new IntConstantArgument(node.Value));
            return new BasicBlock(assignment);
        }

        public override BasicBlock Visit(VariableExpression node)
        {
            var assignment = new AssignStatement(
                (VariableSymbol)node.VariableId.Symbol,
                new VariableArgument((VariableSymbol)node.VariableId.Symbol));
            return new BasicBlock(assignment);
        }

        public override BasicBlock Visit(VoidReturnStatement node)
        {
            return new BasicBlock(new ReturnStatement());
        }

        public override BasicBlock Visit(ReturnExpressionStatement node)
        {
            var block = node.Expression.Accept(this);
            var argument = new VariableArgument(((ReturningStatement)block.Exit).Return);
            var statement = new ReturnStatement(argument);

            return block.Append(statement);
        }

        public override BasicBlock Visit(BinaryOperatorExpression node)
        {
            var leftBlock = node.Left.Accept(this);
            var rightBlock = node.Right.Accept(this);
            var beforeBlock = leftBlock.Join(rightBlock);

            var leftArgument = new VariableArgument(((ReturningStatement)leftBlock.Exit).Return);
            var rightArguumment = new VariableArgument(((ReturningStatement)rightBlock.Exit).Return);

            var tempVariable = this.MakeTempVariable(node, node.ResultingType);

            var statement = new BinaryOperatorStatement(tempVariable, node.Operator, leftArgument, rightArguumment);

            return beforeBlock.Append(statement);
        }

        public override BasicBlock Visit(AssignmentExpression node)
        {
            var beforeBlock = node.RightSide.Accept(this);
            var argument = new VariableArgument(((ReturningStatement)beforeBlock.Exit).Return);

            VariableSymbol symbol;

            if (node.LeftSide is VariableExpression)
            {
                symbol = (VariableSymbol)((VariableExpression)node.LeftSide).VariableId.Symbol;
            }
            else
            {
                throw new NotImplementedException();
            }

            var statement = new AssignStatement(symbol, argument);
            return beforeBlock.Append(statement);
        }

        private TempVariableSymbol MakeTempVariable(Node node, Type type)
        {
            var variable = new TempVariableSymbol(type);
            node.SymbolTable.RegisterSymbol(variable);
            return variable;
        }
    }
}
