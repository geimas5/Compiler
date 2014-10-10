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

    public class ControlFlowGraphBuilder : Visitor<BasicBlock>
    {
        private ControlFlowGraph controlFlowGraph;

        private readonly Stack<Statement> loopExits = new Stack<Statement>(); 

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
            return this.BuildBlock(node.Statements);
        }

        public override BasicBlock Visit(ReturningFunctionDecleration node)
        {
            return this.BuildBlock(node.Statements);
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

        public override BasicBlock Visit(BooleanConstant node)
        {
            var variable = this.MakeTempVariable(node, new Type(PrimitiveType.Boolean));

            var assignment = new AssignStatement(variable, new BooleanConstantArgument(node.Value));
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

        public override BasicBlock Visit(StringConstant node)
        {
            return base.Visit(node);
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
            var tempVariable = this.MakeTempVariable(node, node.ResultingType);
            
            if (node.Operator == BinaryOperator.And || node.Operator == BinaryOperator.Or)
            {
                var trueBranch = new BasicBlock(new AssignStatement(tempVariable, new BooleanConstantArgument(true)));
                var falseBranch = new BasicBlock(new AssignStatement(tempVariable, new BooleanConstantArgument(false)));

                var branch = this.CreateBranchNode1(node, trueBranch, falseBranch);

                return branch.Append(new AssignStatement(tempVariable, new VariableArgument(tempVariable)));
            }


            var leftBlock = node.Left.Accept(this);
            var rightBlock = node.Right.Accept(this);
            var beforeBlock = leftBlock.Join(rightBlock);

            var leftArgument = new VariableArgument(((ReturningStatement)leftBlock.Exit).Return);
            var rightArguumment = new VariableArgument(((ReturningStatement)rightBlock.Exit).Return);

            var statement = new BinaryOperatorStatement(tempVariable, node.Operator, leftArgument, rightArguumment);

            return beforeBlock.Append(statement);
        }

        public override BasicBlock Visit(IfStatement node)
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

        public override BasicBlock Visit(WhileStatement node)
        {
            var afterBranch = new BasicBlock(new NopStatement());
            var body = this.BuildBlock(node.Body);
            var branchNode = this.CreateBranchNode1(node.Condition, body, afterBranch);

            loopExits.Push(afterBranch.Enter);
            
            loopExits.Pop();

            body = body.Append(new JumpStatement(branchNode.Enter));

            return branchNode.Join(body).Join(afterBranch);
        }

        public override BasicBlock Visit(ForStatement node)
        {
            var afterStatement = new BasicBlock(new NopStatement());
            var initialization = node.Initialization.Accept(this);
            
            loopExits.Push(afterStatement.Enter);

            var body = this.BuildBlock(node.Body);

            loopExits.Pop();

            var conditionNode = this.CreateBranchNode1(node.Condition, body, afterStatement);

            body = body.Join(node.Afterthought.Accept(this));

            return initialization.Join(conditionNode).Join(body).Join(afterStatement);
        }

        public override BasicBlock Visit(IndexerExpression node)
        {
            return base.Visit(node);
        }

        public override BasicBlock Visit(BreakStatement node)
        {
            return new BasicBlock(new JumpStatement(this.loopExits.Peek()));
        }

        private BasicBlock BuildIfStatement(ExpressionNode expression, BasicBlock trueBranch)
        {
            var afterStatement = new BasicBlock(new NopStatement());

            return this.CreateBranchNode1(expression, trueBranch, afterStatement);
        }

        private BasicBlock BuildIfStatement(
            ExpressionNode expression,
            BasicBlock trueBranch,
            BasicBlock elseBranch)
        {
            return this.CreateBranchNode1(expression, trueBranch, elseBranch);
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

        private BasicBlock BuildBlock(IEnumerable<StatementNode> statements)
        {
            if (!statements.Any())
            {
                return new BasicBlock(new NopStatement());
            }

            BasicBlock result = null;

            foreach (StatementNode statement in statements)
            {
                BasicBlock block = statement.Accept(this);
                if (block == null)
                {
                    continue;
                }

                result = result == null ? block : result.Join(block);
            }

            return result;
        }

        private BasicBlock CreateBranchNode1(ExpressionNode expression, BasicBlock trueBranch, BasicBlock falseBranch)
        {
            var unaryExpression = expression as UnaryExpression;
            if (unaryExpression != null)
            {
                // If the node is a not node, then all we need to do is to switch the branches. 

                Trace.Assert(unaryExpression.Operator == UnaryOperator.Not);
                return this.CreateBranchNode1(unaryExpression.Expression, falseBranch, trueBranch);
            }

            var binaryExpression = expression as BinaryOperatorExpression;

            var leftBlock = binaryExpression.Left.Accept(this);
            var leftArgument = new VariableArgument(((ReturningStatement)leftBlock.Exit).Return);

            if (binaryExpression.Operator != BinaryOperator.And
                && binaryExpression.Operator != BinaryOperator.Or)
            {
                var rightBlock = binaryExpression.Right.Accept(this);
                var rightArguumment = new VariableArgument(((ReturningStatement)rightBlock.Exit).Return);
                var beforeBlock = leftBlock.Join(rightBlock);

                var branchStatement = new BranchStatement(
                    true,
                    binaryExpression.Operator,
                    leftArgument,
                    rightArguumment,
                    falseBranch.Enter);

                return beforeBlock.Append(branchStatement).Join(trueBranch).Join(falseBranch);
            }

            BasicBlock block = leftBlock;

            if (binaryExpression.Operator == BinaryOperator.And)
            {
                var firstBranch = new BranchStatement(
                    true,
                    BinaryOperator.Equal,
                    leftArgument,
                    new BooleanConstantArgument(true),
                    falseBranch.Enter);

                var rightBlock = binaryExpression.Right.Accept(this);
                var rightArguumment = new VariableArgument(((ReturningStatement)rightBlock.Exit).Return);

                var seccondBranch = new BranchStatement(
                    true,
                    BinaryOperator.Equal,
                    rightArguumment,
                    new BooleanConstantArgument(true),
                    falseBranch.Enter);

                block = block.Append(firstBranch).Join(rightBlock).Append(seccondBranch);
            }
            else if (binaryExpression.Operator == BinaryOperator.Or)
            {
                var firstBranch = new BranchStatement(
                   true,
                   BinaryOperator.Equal,
                   leftArgument,
                   new BooleanConstantArgument(true),
                   trueBranch.Enter);

                var rightBlock = binaryExpression.Right.Accept(this);
                var rightArguumment = new VariableArgument(((ReturningStatement)rightBlock.Exit).Return);

                var seccondBranch = new BranchStatement(
                    true,
                    BinaryOperator.Equal,
                    rightArguumment,
                    new BooleanConstantArgument(true),
                    falseBranch.Enter);

                block = block.Append(firstBranch).Join(rightBlock).Append(seccondBranch);
            }

            var afterStatement = new NopStatement();
            var jumpToAfter = new JumpStatement(afterStatement);

            return block.Join(trueBranch).Append(jumpToAfter).Join(falseBranch).Append(afterStatement);
        }
    }
}
