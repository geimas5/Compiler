namespace Compiler.ControlFlowGraph
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;

    using Compiler.SymbolTable;
    using Compiler.SyntaxTree;

    using ConstantExpression = Compiler.SyntaxTree.ConstantExpression;
    using Type = Compiler.Type;

    public class ControlFlowGraphBuilder : Visitor<IEnumerable<BasicBlock>>
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

        public override IEnumerable<BasicBlock> Visit(VoidFunctionDecleration node)
        {
            return this.BuildBlock(node.Statements);
        }

        public override IEnumerable<BasicBlock> Visit(ReturningFunctionDecleration node)
        {
            return this.BuildBlock(node.Statements);
        }

        public override IEnumerable<BasicBlock> Visit(ExpressionStatement node)
        {
            return node.Expression.Accept(this);
        }

        public override IEnumerable<BasicBlock> Visit(ConstantExpression node)
        {
            return node.Constant.Accept(this);
        }

        public override IEnumerable<BasicBlock> Visit(InitializedVariableDecleration node)
        {
            var initBlock = node.Initialization.Accept(this).Single();
            var argument = new VariableArgument(((ReturningStatement)initBlock.Exit).Return);

            var statemment = new AssignStatement((VariableSymbol)node.Variable.Name.Symbol, argument);

            yield return initBlock.Append(statemment);
        }

        public override IEnumerable<BasicBlock> Visit(IntegerConstant node)
        {
            var variable = this.MakeTempVariable(node, new Type(PrimitiveType.Int));

            var assignment = new AssignStatement(variable, new IntConstantArgument(node.Value));
            yield return new BasicBlock(assignment);
        }

        public override IEnumerable<BasicBlock> Visit(VariableExpression node)
        {
            var assignment = new AssignStatement(
                (VariableSymbol)node.VariableId.Symbol,
                new VariableArgument((VariableSymbol)node.VariableId.Symbol));
            yield return new BasicBlock(assignment);
        }

        public override IEnumerable<BasicBlock> Visit(VoidReturnStatement node)
        {
            yield return new BasicBlock(new ReturnStatement());
        }

        public override IEnumerable<BasicBlock> Visit(ReturnExpressionStatement node)
        {
            var block = node.Expression.Accept(this).Single();
            var argument = new VariableArgument(((ReturningStatement)block.Exit).Return);
            var statement = new ReturnStatement(argument);

            yield return block.Append(statement);
        }

        public override IEnumerable<BasicBlock> Visit(BinaryOperatorExpression node)
        {
            var leftBlock = node.Left.Accept(this).Single();
            var rightBlock = node.Right.Accept(this).Single();
            var beforeBlock = leftBlock.Join(rightBlock);

            var leftArgument = new VariableArgument(((ReturningStatement)leftBlock.Exit).Return);
            var rightArguumment = new VariableArgument(((ReturningStatement)rightBlock.Exit).Return);

            var tempVariable = this.MakeTempVariable(node, node.ResultingType);

            var statement = new BinaryOperatorStatement(tempVariable, node.Operator, leftArgument, rightArguumment);

            yield return beforeBlock.Append(statement);
        }

        public override IEnumerable<BasicBlock> Visit(IfStatement node)
        {
            if (!node.ElseStatements.Any())
            {
                return this.BuildIfStatement(node.Condition, this.BuildBlock(node.Body));
            }
            
            return this.BuildIfStatement(
                node.Condition,
                this.BuildBlock(node.Body),
                this.BuildBlock(node.ElseStatements));
        }

        private IEnumerable<BasicBlock> BuildIfStatement(ExpressionNode expression, IEnumerable<BasicBlock> trueBranch)
        {
            var afterStatement = new NopStatement();
            var afterBranch = new BasicBlock(afterStatement);

            var ret = this.UnionBasicBlockLists(
                    this.CreateBranchNode(expression, afterStatement),
                    trueBranch);

            ret = this.UnionBasicBlockLists(ret, new[] { afterBranch });
            
            return ret;
        }

        private IEnumerable<BasicBlock> BuildIfStatement(
            ExpressionNode expression,
            IEnumerable<BasicBlock> trueBranch,
            IEnumerable<BasicBlock> elseBranch)
        {
            var afterStatement = new NopStatement();
            var afterBranch = new BasicBlock(afterStatement);

            var elseBranchList = elseBranch.ToArray();

            var condition = this.CreateBranchNode(expression, elseBranchList.First().Enter);

            var ret = this.UnionBasicBlockLists(condition, trueBranch);
            ret = this.UnionBasicBlockLists(ret, new[] { new BasicBlock(new JumpStatement(afterStatement)), });
            ret = this.UnionBasicBlockLists(ret, elseBranchList);
            ret = this.UnionBasicBlockLists(ret, new[] { afterBranch });

            return ret;
        }

        public override IEnumerable<BasicBlock> Visit(AssignmentExpression node)
        {
            var beforeBlock = node.RightSide.Accept(this).Single();
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
            yield return beforeBlock.Append(statement);
        }

        private TempVariableSymbol MakeTempVariable(Node node, Type type)
        {
            var variable = new TempVariableSymbol(type);
            node.SymbolTable.RegisterSymbol(variable);
            return variable;
        }

        private IEnumerable<BasicBlock> BuildBlock(IEnumerable<StatementNode> statements)
        {
            var blocks = new List<BasicBlock>();

            foreach (var statement in statements)
            {
                var subBlocks = statement.Accept(this).ToArray();
                if (!blocks.Any() || blocks.Count > 1)
                {
                    blocks = this.UnionBasicBlockLists(blocks, subBlocks).ToList();
                }
                else
                {
                    blocks[blocks.Count - 1] = blocks[blocks.Count - 1].Join(subBlocks.Single());
                }
            }

            return blocks;
        }

        private IEnumerable<BasicBlock> CreateBranchNode(ExpressionNode expression, Statement after)
        {
            var unaryExpression = expression as UnaryExpression;
            if (unaryExpression != null)
            {
                Trace.Assert(unaryExpression.Operator == UnaryOperator.Not);
                return this.CreateBranchNode(unaryExpression.Expression, after);
            }

            var binaryOperatorExpression = expression as BinaryOperatorExpression;
            Trace.Assert(binaryOperatorExpression != null);

            if (binaryOperatorExpression.Operator != BinaryOperator.And
                && binaryOperatorExpression.Operator != BinaryOperator.Or)
            {
                var leftBlock = binaryOperatorExpression.Left.Accept(this).Single();
                var rightBlock = binaryOperatorExpression.Right.Accept(this).Single();
                var beforeBlock = leftBlock.Join(rightBlock);

                var leftArgument = new VariableArgument(((ReturningStatement)leftBlock.Exit).Return);
                var rightArguumment = new VariableArgument(((ReturningStatement)rightBlock.Exit).Return);

                var branchStatement = new BranchStatement(
                    true,
                    binaryOperatorExpression.Operator,
                    leftArgument,
                    rightArguumment,
                    after);

                return new[] { beforeBlock.Append(branchStatement) };
            }

            throw new NotImplementedException("&& or || operator in if is not implemented");
        }

        private IEnumerable<BasicBlock> UnionBasicBlockLists(
            IEnumerable<BasicBlock> blocks1,
            IEnumerable<BasicBlock> blocks2)
        {
            var b1arr = blocks1.ToArray();
            if (b1arr.Length == 0)
            {
                return blocks2;
            }

            var b2arr = blocks2.ToArray();

            b1arr.Last().Exit.Next = b2arr.First().Enter;

            return b1arr.Union(b2arr);
        }
    }
}
