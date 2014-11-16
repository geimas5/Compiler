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
    using UnaryExpression = Compiler.SyntaxTree.UnaryExpression;

    public class ControlFlowGraphBuilder : Visitor<BasicBlock>
    {
        private const int dataItemSize = 8; // In bytes.

        private ControlFlowGraph controlFlowGraph;

        private readonly Stack<Statement> loopExits = new Stack<Statement>(); 

        public ControlFlowGraph BuildGraph(ProgramNode rootNode)
        {
            this.controlFlowGraph = new ControlFlowGraph();

            foreach (var node in rootNode.Functions)
            {
                this.controlFlowGraph.Functions[node.Name] = this.SplitBlock(node.Accept(this)).ToList();

                this.controlFlowGraph.FunctionParameters[node.Name] = new List<VariableSymbol>();
                foreach (var param in node.Parameters)
                {
                    this.controlFlowGraph.FunctionParameters[node.Name].Add((VariableSymbol)param.Name.Symbol);
                }
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
            var argument = this.ToArgument(((IReturningStatement)initBlock.Exit).Return);
            var statemment = new AssignStatement(new VariableDestination((VariableSymbol)node.Variable.Name.Symbol), argument);

            return initBlock.Append(statemment);
        }

        public override BasicBlock Visit(IntegerConstant node)
        {
            var variable = this.MakeTempVariable(node, new Type(PrimitiveType.Int));

            var assignment = new AssignStatement(new VariableDestination(variable), new IntConstantArgument(node.Value));
            return new BasicBlock(assignment);
        }

        public override BasicBlock Visit(BooleanConstant node)
        {
            var variable = this.MakeTempVariable(node, new Type(PrimitiveType.Boolean));

            var assignment = new AssignStatement(new VariableDestination(variable), new BooleanConstantArgument(node.Value));
            return new BasicBlock(assignment);
        }

        public override BasicBlock Visit(VariableExpression node)
        {
            var assignment = new AssignStatement(
                new VariableDestination((VariableSymbol)node.VariableId.Symbol),
                new VariableArgument((VariableSymbol)node.VariableId.Symbol));
            return new BasicBlock(assignment);
        }

        public override BasicBlock Visit(VoidReturnStatement node)
        {
            return new BasicBlock(new ReturnStatement());
        }

        public override BasicBlock Visit(StringConstant node)
        {
            string name = "str" + Math.Abs(node.Text.GetHashCode());
            if (!this.controlFlowGraph.Strings.ContainsKey(name))
            {
                this.controlFlowGraph.Strings.Add(name, node.Text);
            }

            var variable = this.MakeTempVariable(node, new Type(PrimitiveType.Int));

            var assignment = new AssignStatement(new VariableDestination(variable), new GlobalArgument(name));
            return new BasicBlock(assignment);
        }

        public override BasicBlock Visit(ArrayCreatorExpression node)
        {
            BasicBlock basicBlock = null;

            var sizes = new List<VariableSymbol>();
            VariableSymbol totalNumberOfElements = null;

            foreach (var size in node.Sizes)
            {
                basicBlock = basicBlock == null ? size.Accept(this) : basicBlock.Join(size.Accept(this));

                sizes.Add(((VariableDestination)((IReturningStatement)basicBlock.Exit).Return).Variable);

                if (totalNumberOfElements == null)
                {
                    totalNumberOfElements = this.MakeTempVariable(node, Type.IntType);
                    basicBlock = basicBlock.Append(new AssignStatement(new VariableDestination(totalNumberOfElements), new VariableArgument(sizes.Last())));
                }
                else
                {
                    var newtotalNumberOfElements = this.MakeTempVariable(node, Type.IntType);
                    basicBlock =
                        basicBlock.Append(
                            new BinaryOperatorStatement(
                                new VariableDestination(newtotalNumberOfElements),
                                BinaryOperator.Multiply,
                                new VariableArgument(sizes.Last()),
                                new VariableArgument(totalNumberOfElements)));

                    totalNumberOfElements = newtotalNumberOfElements;
                }
            }

            var totalElemmentSize = this.MakeTempVariable(node, Type.IntType);
            basicBlock = basicBlock.Append(
                    new BinaryOperatorStatement(
                        new VariableDestination(totalElemmentSize),
                        BinaryOperator.Multiply,
                        new VariableArgument(totalNumberOfElements), 
                        new IntConstantArgument(dataItemSize)));

            var totalSize = this.MakeTempVariable(node, Type.IntType);
            basicBlock = basicBlock.Append(
                    new BinaryOperatorStatement(
                        new VariableDestination(totalSize),
                        BinaryOperator.Add,
                        new VariableArgument(totalElemmentSize), 
                        new IntConstantArgument(node.Sizes.Count * dataItemSize)));


            // Allocate the array, store the pointer in variable2.
            var allocVariable = this.MakeTempVariable(node, node.ResultingType);
            basicBlock = basicBlock.Append(new AllocStatement(new VariableDestination(allocVariable), new VariableArgument(totalSize)));


            // Store variable dimention sizes.
            for (int i = 0; i < sizes.Count; i++)
            {
                var sizeOffsetVariable = this.MakeTempVariable(node, Type.IntType);
                basicBlock =
                    basicBlock.Append(
                        new BinaryOperatorStatement(
                            new VariableDestination(sizeOffsetVariable), 
                            BinaryOperator.Add,
                            new VariableArgument(allocVariable),
                            new IntConstantArgument(i * dataItemSize)));

                basicBlock = basicBlock.Append(new AssignStatement(new PointerDestination(sizeOffsetVariable, Type.IntType), new VariableArgument(sizes[i])));
            }

            // Needed to have the variable at the end of the block to join with next statement.
            var statemment = new AssignStatement(new VariableDestination(allocVariable), new VariableArgument(allocVariable));

            return basicBlock.Append(statemment);
        }

        public override BasicBlock Visit(UnaryExpression node)
        {
            var tempVariable = this.MakeTempVariable(node, node.ResultingType);

            var beforeBlock = node.Expression.Accept(this);
            var argument = ToArgument(((IReturningStatement)beforeBlock.Exit).Return);

            Statement statement;
            switch (node.Operator)
            {
                case UnaryOperator.Not:
                    statement = new UnaryOperatorStatement(new VariableDestination(tempVariable), node.Operator, argument);
                    break;
                case UnaryOperator.Negation:
                    var constantZero = Equals(node.ResultingType, Type.DoubleType)
                                           ? (Argument)new DoubleConstantArgument(0)
                                           : new IntConstantArgument(0);

                    statement = new BinaryOperatorStatement(
                        new VariableDestination(tempVariable),
                        BinaryOperator.Subtract,
                        constantZero,
                        argument);

                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            

            return beforeBlock.Append(statement);
        }

        public override BasicBlock Visit(DoubleConstant node)
        {
            var variable = this.MakeTempVariable(node, new Type(PrimitiveType.Double));

            var assignment = new AssignStatement(new VariableDestination(variable), new DoubleConstantArgument(node.Value));
            return new BasicBlock(assignment);
        }

        public override BasicBlock Visit(ReturnExpressionStatement node)
        {
            var block = node.Expression.Accept(this);
            var argument = ToArgument(((IReturningStatement)block.Exit).Return);
            var statement = new ReturnStatement(argument);

            return block.Append(statement);
        }

        public override BasicBlock Visit(BinaryOperatorExpression node)
        {
            var tempVariable = this.MakeTempVariable(node, node.ResultingType);
            
            if (node.Operator == BinaryOperator.And || node.Operator == BinaryOperator.Or)
            {
                var trueBranch = new BasicBlock(new AssignStatement(new VariableDestination(tempVariable), new BooleanConstantArgument(true)));
                var falseBranch = new BasicBlock(new AssignStatement(new VariableDestination(tempVariable), new BooleanConstantArgument(false)));

                var branch = this.CreateBranchNode(node, trueBranch, falseBranch);

                return branch.Append(new AssignStatement(new VariableDestination(tempVariable), new VariableArgument(tempVariable)));
            }

            var leftBlock = node.Left.Accept(this);
            var rightBlock = node.Right.Accept(this);
            var beforeBlock = leftBlock.Join(rightBlock);

            var leftArgument = ToArgument(((IReturningStatement)leftBlock.Exit).Return);
            var rightArgument = ToArgument(((IReturningStatement)rightBlock.Exit).Return);
          
            if (Equals(rightArgument.Type, Type.DoubleType) && Equals(leftArgument.Type, Type.IntType))
            {
                var leftTempVariable = this.MakeTempVariable(node, Type.DoubleType);
                beforeBlock = beforeBlock.Append(new ConvertToDoubleStatement(new VariableDestination(leftTempVariable), leftArgument));
                leftArgument = ToArgument(((IReturningStatement)beforeBlock.Exit).Return);
            }
            else if (Equals(leftArgument.Type, Type.DoubleType) && Equals(rightArgument.Type, Type.IntType))
            {
                var rightTempVariable = this.MakeTempVariable(node, Type.DoubleType);
                beforeBlock = beforeBlock.Append(new ConvertToDoubleStatement(new VariableDestination(rightTempVariable), rightArgument));
                rightArgument = ToArgument(((IReturningStatement)beforeBlock.Exit).Return);
            }

            var statement = new BinaryOperatorStatement(new VariableDestination(tempVariable), node.Operator, leftArgument, rightArgument);

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
            var branchNode = this.CreateBranchNode(node.Condition, body, afterBranch);

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

            body = body.Join(node.Afterthought.Accept(this));
            var beforeBranch = new NopStatement();
            var jumpStatement = new JumpStatement(beforeBranch);
            body = body.Append(jumpStatement);

            var conditionNode = this.CreateBranchNode(node.Condition, body, new BasicBlock(new JumpStatement(afterStatement.Enter)));

            return initialization.Append(beforeBranch).Join(conditionNode).Join(afterStatement);
        }

        public override BasicBlock Visit(IndexerExpression node)
        {
            var indexer = this.BuildIndexerLocationVariable(node);
            var tempVariable = this.MakeTempVariable(node, node.ResultingType);

            var assignment = new AssignStatement(
               new VariableDestination(tempVariable),
               new PointerArgument(((VariableDestination)((IReturningStatement)indexer.Exit).Return).Variable, tempVariable.Type));
            return indexer.Join(new BasicBlock(assignment));
        }

        public override BasicBlock Visit(FunctionCallExpression node)
        {
            BasicBlock block = null;

            if (node.Arguments.Any())
            {
                var arguments = new List<Argument>();
                foreach (var expressionNode in node.Arguments)
                {
                    var nodeStatements = expressionNode.Accept(this);
                    arguments.Add(this.ToArgument(((IReturningStatement)nodeStatements.Exit).Return));

                    block = block == null ? nodeStatements : block.Join(nodeStatements);
                }

                foreach (var argument in arguments)
                {
                    var param = new ParamStatement(argument);
                    block = block.Append(param);
                }
            }

            Statement callStatement;
            if (node.Symbol is ReturningFunctionSymbol)
            {
                var temp = this.MakeTempVariable(node, ((ReturningFunctionSymbol)node.Symbol).Type);

                callStatement = new ReturningCallStatement((FunctionSymbol)node.Symbol, node.Arguments.Count, new VariableDestination(temp));
            }
            else
            {
                callStatement = new CallStatement((FunctionSymbol)node.Symbol, node.Arguments.Count);      
            }

            block = block == null ? new BasicBlock(callStatement) : block.Append(callStatement);

            return block;
        }

        public override BasicBlock Visit(BreakStatement node)
        {
            return new BasicBlock(new JumpStatement(this.loopExits.Peek()));
        }

        private BasicBlock BuildIfStatement(ExpressionNode expression, BasicBlock trueBranch)
        {
            var afterStatement = new BasicBlock(new NopStatement());

            return this.CreateBranchNode(expression, trueBranch, afterStatement);
        }

        private BasicBlock BuildIfStatement(
            ExpressionNode expression,
            BasicBlock trueBranch,
            BasicBlock elseBranch)
        {
            return this.CreateBranchNode(expression, trueBranch, elseBranch);
        }

        public override BasicBlock Visit(AssignmentExpression node)
        {
            var beforeBlock = node.RightSide.Accept(this);
            var argument = this.ToArgument(((IReturningStatement)beforeBlock.Exit).Return);

            if (Equals(node.ResultingType, Type.DoubleType) && Equals(node.RightSide.ResultingType, Type.IntType))
            {
                var tempVariable = this.MakeTempVariable(node, Type.DoubleType);
                beforeBlock = beforeBlock.Append(new ConvertToDoubleStatement(new VariableDestination(tempVariable), argument));
                argument = ToArgument(((IReturningStatement)beforeBlock.Exit).Return);
            }

            Destination destination;

            if (node.LeftSide is VariableExpression)
            {
                destination = new VariableDestination((VariableSymbol)((VariableExpression)node.LeftSide).VariableId.Symbol);
            }
            else
            {
                beforeBlock = beforeBlock.Join(this.BuildIndexerLocationVariable((IndexerExpression)node.LeftSide));

                destination =
                    new PointerDestination(
                        ((VariableDestination)((IReturningStatement)beforeBlock.Exit).Return).Variable,
                        node.ResultingType);
            }

            var statement = new AssignStatement(destination, argument);
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
                var block = statement.Accept(this);
                if (block == null)
                {
                    continue;
                }

                result = result == null ? block : result.Join(block);
            }

            return result;
        }

        private BasicBlock CreateBranchNode(ExpressionNode expression, BasicBlock trueBranch, BasicBlock falseBranch)
        {
            var afterStatement = new NopStatement();
            var jumpToAfter = new JumpStatement(afterStatement);

            var unaryExpression = expression as UnaryExpression;
            if (unaryExpression != null)
            {
                // If the node is a not node, then all we need to do is to switch the branches. 

                Trace.Assert(unaryExpression.Operator == UnaryOperator.Not);
                return this.CreateBranchNode(unaryExpression.Expression, falseBranch, trueBranch);
            }

            var constantExpression = expression as ConstantExpression;
            if (constantExpression != null && constantExpression.Constant is BooleanConstant)
            {
                var constant = constantExpression.Constant as BooleanConstant;

                return constant.Value ? trueBranch : falseBranch;
            }

            var variableExpression = expression as VariableExpression;
            if (variableExpression != null)
            {
                var variable = (VariableSymbol)variableExpression.VariableId.Symbol;
                var branchStatement = new BranchStatement(
                        true,
                        BinaryOperator.Equal,
                        new VariableArgument(variable), 
                        new BooleanConstantArgument(true), 
                        falseBranch.Enter);

                var temp = new BasicBlock(branchStatement);
                temp = temp.Join(trueBranch);
                temp = temp.Append(jumpToAfter);
                temp = temp.Join(falseBranch);
                temp = temp.Append(afterStatement);

                return temp;
            }

            var binaryExpression = expression as BinaryOperatorExpression;
            if (binaryExpression != null)
            {
                var leftBlock = binaryExpression.Left.Accept(this);
                var leftArgument = ToArgument(((IReturningStatement)leftBlock.Exit).Return);

                if (binaryExpression.Operator != BinaryOperator.And && binaryExpression.Operator != BinaryOperator.Or)
                {
                    var rightBlock = binaryExpression.Right.Accept(this);
                    var rightArgument = ToArgument(((IReturningStatement)rightBlock.Exit).Return);
                    var beforeBlock = leftBlock.Join(rightBlock);

                    if (Equals(rightArgument.Type, Type.DoubleType) && Equals(leftArgument.Type, Type.IntType))
                    {
                        var leftTempVariable = this.MakeTempVariable(expression, Type.DoubleType);
                        beforeBlock = beforeBlock.Append(new ConvertToDoubleStatement(new VariableDestination(leftTempVariable), leftArgument));
                        leftArgument = ToArgument(((IReturningStatement)beforeBlock.Exit).Return);
                    }
                    else if (Equals(leftArgument.Type, Type.DoubleType) && Equals(rightArgument.Type, Type.IntType))
                    {
                        var rightTempVariable = this.MakeTempVariable(expression, Type.DoubleType);
                        beforeBlock = beforeBlock.Append(new ConvertToDoubleStatement(new VariableDestination(rightTempVariable), rightArgument));
                        rightArgument = ToArgument(((IReturningStatement)beforeBlock.Exit).Return);
                    }

                    var branchStatement = new BranchStatement(
                        true,
                        binaryExpression.Operator,
                        leftArgument,
                        rightArgument,
                        falseBranch.Enter);

                    var temp = beforeBlock.Append(branchStatement);
                    temp = temp.Join(trueBranch);
                    temp = temp.Append(jumpToAfter);
                    temp = temp.Join(falseBranch);
                    temp = temp.Append(afterStatement);

                    return temp;
                }

                var block = leftBlock;

                if (binaryExpression.Operator == BinaryOperator.And)
                {
                    var firstBranch = new BranchStatement(
                        true,
                        BinaryOperator.Equal,
                        leftArgument,
                        new BooleanConstantArgument(true),
                        falseBranch.Enter);

                    var rightBlock = binaryExpression.Right.Accept(this);
                    var rightArguumment = ToArgument(((IReturningStatement)rightBlock.Exit).Return);

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
                        false,
                        BinaryOperator.Equal,
                        leftArgument,
                        new BooleanConstantArgument(true),
                        trueBranch.Enter);

                    var rightBlock = binaryExpression.Right.Accept(this);
                    var rightArguumment = ToArgument(((IReturningStatement)rightBlock.Exit).Return);

                    var seccondBranch = new BranchStatement(
                        true,
                        BinaryOperator.Equal,
                        rightArguumment,
                        new BooleanConstantArgument(true),
                        falseBranch.Enter);

                    block = block.Append(firstBranch).Join(rightBlock).Append(seccondBranch);
                }

                return block.Join(trueBranch).Append(jumpToAfter).Join(falseBranch).Append(afterStatement);
            }

            throw new NotSupportedException();
        }

        private IEnumerable<BasicBlock> SplitBlock(BasicBlock block)
        {
            int count = block.Count();

            var leaders = new HashSet<int>();

            leaders.Add(block.Enter.Id);

            foreach (var statement in block)
            {
                var branchStatement = statement as BranchStatement;
                if (branchStatement != null)
                {
                    leaders.Add(branchStatement.BranchTarget.Id);
                }

                var jumpStatement = statement as JumpStatement;
                if (jumpStatement != null)
                {
                    leaders.Add(jumpStatement.Target.Id);
                }

                if ((statement is BranchStatement || statement is JumpStatement || statement is CallStatement)
                    && statement.Next != null)
                {
                    leaders.Add(statement.Next.Id);
                }
            }

            var blocks = new List<BasicBlock>();
            var currentBlock = new List<Statement>();

            foreach (var statement in block.ToArray())
            {
                if (leaders.Contains(statement.Id))
                {
                    if (currentBlock.Any())
                    {
                        blocks.Add(new BasicBlock(currentBlock.First(), currentBlock.Last()));
                        currentBlock = new List<Statement>();
                    }
                }

                currentBlock.Add(statement);
            }

            if (currentBlock.Any())
            {
                blocks.Add(new BasicBlock(currentBlock.First(), currentBlock.Last()));
            }

            Debug.Assert(count == blocks.Sum(m => m.Count()));

            return blocks;
        }

        private Argument ToArgument(Destination destination)
        {
            var variableDestination = destination as VariableDestination;
            if (variableDestination != null)
            {
                return new VariableArgument(variableDestination.Variable);
            }

            var pointerDestination = destination as PointerDestination;
            if (pointerDestination != null)
            {
                return new PointerArgument(pointerDestination.Destination, destination.Type);
            }

            throw new ArgumentException("The destination type can not be converted to a argument", "destination");
        }

        private BasicBlock BuildIndexerLocationVariable(IndexerExpression expression)
        {
            var indexes = new List<Argument>();

            var currentIndexer = expression;

            BasicBlock block = null;
            VariableSymbol arrayVariable = null;

            do
            {
                block = block == null ? currentIndexer.Index.Accept(this) : block.Join(currentIndexer.Index.Accept(this));

                indexes.Add(this.ToArgument(((IReturningStatement)block.Exit).Return));

                if (currentIndexer.Name is VariableExpression)
                {
                    arrayVariable = (VariableSymbol)((VariableExpression)currentIndexer.Name).VariableId.Symbol;
                }

                currentIndexer = currentIndexer.Name as IndexerExpression;
            }
            while (currentIndexer != null);

            var indexBaseVariable = this.MakeTempVariable(expression, Type.IntType);
            block = block.Append(
                    new BinaryOperatorStatement(
                        new VariableDestination(indexBaseVariable),
                        BinaryOperator.Add,
                        new VariableArgument(arrayVariable),
                        new IntConstantArgument(indexes.Count * dataItemSize)));


            VariableSymbol intermediateCalculation = this.MakeTempVariable(expression, Type.IntType);
            block = block.Append(new AssignStatement(new VariableDestination(intermediateCalculation), indexes[0]));

            for (int i = 1; i < indexes.Count; i++)
            {
                var rowItemNumLocation = this.MakeTempVariable(expression, Type.IntType);
                block = block.Append(new BinaryOperatorStatement(
                            new VariableDestination(rowItemNumLocation),
                            BinaryOperator.Add,
                            new VariableArgument(arrayVariable),
                            new IntConstantArgument(i * dataItemSize)));

                var rowElements = this.MakeTempVariable(expression, Type.IntType);
                block = block.Append(new AssignStatement(
                            new VariableDestination(rowElements),
                            new PointerArgument(rowItemNumLocation, rowElements.Type)));

                var rowLocation = this.MakeTempVariable(expression, Type.IntType);

                block = block.Append(
                        new BinaryOperatorStatement(
                            new VariableDestination(rowLocation),
                            BinaryOperator.Multiply,
                            new VariableArgument(intermediateCalculation),
                            new VariableArgument(rowElements)));

                var result = this.MakeTempVariable(expression, Type.IntType);

                block = block.Append(
                        new BinaryOperatorStatement(
                            new VariableDestination(result),
                            BinaryOperator.Add,
                            new VariableArgument(rowLocation),
                            indexes[i]));

                intermediateCalculation = result;
            }

            var withDataFieldSize = this.MakeTempVariable(expression, Type.IntType);
            block = block.Append(
                    new BinaryOperatorStatement(
                        new VariableDestination(withDataFieldSize),
                        BinaryOperator.Multiply,
                        new VariableArgument(intermediateCalculation),
                        new IntConstantArgument(dataItemSize)));


            var resultVariable = this.MakeTempVariable(expression, Type.IntType);
            block = block.Append(
                    new BinaryOperatorStatement(
                        new VariableDestination(resultVariable),
                        BinaryOperator.Add,
                        new VariableArgument(indexBaseVariable),
                        new VariableArgument(withDataFieldSize)));

            return block;
        }
    }
}
