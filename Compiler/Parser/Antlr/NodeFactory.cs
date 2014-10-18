namespace Compiler.Parser.Antlr
{
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;

    using Antlr4.Runtime;

    using Compiler.SyntaxTree;

    public class NodeFactory
    {
        private readonly Logger logger;

        public NodeFactory(Logger logger)
        {
            this.logger = logger;
        }

        public ProgramNode CreateProgramNode(ParserRuleContext context)
        {
            return new ProgramNode(CreateLocation(context));
        }

        public InitializedVariableDecleration CreateInitializedVariableDecleration(
            ParserRuleContext context,
            VariableNode variable,
            ExpressionNode expression)
        {
            return new InitializedVariableDecleration(CreateLocation(context), variable, expression);
        }

        public UnInitializedVariableDecleration CreateUnInitializedVariableDecleration(
            ParserRuleContext context,
            VariableNode variable)
        {
            return new UnInitializedVariableDecleration(CreateLocation(context), variable);
        }

        public VoidFunctionDecleration CreateVoidFunctionDecleration(
            ParserRuleContext context,
            string name,
            IEnumerable<VariableNode> parameters,
            IEnumerable<StatementNode> statements)
        {
            return new VoidFunctionDecleration(
                CreateLocation(context),
                name,
                parameters.Reverse(),
                statements.Where(m => m != null));
        }

        public ReturningFunctionDecleration CreateReturningFunctionDecleration(
            ParserRuleContext context,
            string name,
            IEnumerable<VariableNode> parameters,
            IEnumerable<StatementNode> statements,
            TypeNode type)
        {
            return new ReturningFunctionDecleration(
                CreateLocation(context),
                name,
                parameters.Reverse(),
                statements.Where(m => m != null),
                type);
        }

        public VariableNode CreateVariableNode(
            ParserRuleContext context,
            TypeNode node,
            VariableIdNode variableId)
        {
            return new VariableNode(CreateLocation(context), node, variableId);
        }

        public VariableIdNode CreateVariableIdNode(ParserRuleContext context, string name)
        {
            return new VariableIdNode(CreateLocation(context), name);
        }

        public TypeNode CreateTypeNode(ParserRuleContext context, PrimitiveType primitiveType)
        {
            return new TypeNode(CreateLocation(context), new Type(primitiveType));
        }

        public TypeNode CreateTypeNodeArrayOfType(ParserRuleContext context, Type type)
        {
            return new TypeNode(CreateLocation(context), new Type(type.PrimitiveType, type.Dimensions + 1));
        }

        public ExpressionStatement CreateExpressionStatement(
            ParserRuleContext context,
            ExpressionNode expression)
        {
            if (expression == null)
            {
                expression = new NopExpression(CreateLocation(context));
            }

            return new ExpressionStatement(CreateLocation(context), expression);
        }

        public IfStatement CreateIfStatement(
            ParserRuleContext context,
            ExpressionNode condition,
            IEnumerable<StatementNode> body)
        {
            return new IfStatement(CreateLocation(context), condition, body.Where(m => m != null));
        }

        public IfStatement CreateIfStatement(
            ParserRuleContext context,
            ExpressionNode condition,
            IEnumerable<StatementNode> body,
            IEnumerable<StatementNode> els)
        {
            return new IfStatement(
                CreateLocation(context),
                condition,
                body.Where(m => m != null),
                els.Where(m => m != null));
        }

        public WhileStatement CreateWhileStatement(
            ParserRuleContext context,
            ExpressionNode condition,
            IEnumerable<StatementNode> body)
        {
            return new WhileStatement(CreateLocation(context), condition, body.Where(m => m != null));
        }

        public ForStatement CreateForStatement(
            ParserRuleContext context,
            ExpressionNode initialization,
            ExpressionNode condition,
            ExpressionNode afterthought,
            IEnumerable<StatementNode> body)
        {
            return new ForStatement(
                CreateLocation(context),
                initialization,
                condition,
                afterthought,
                body.Where(m => m != null));
        }

        public ReturnExpressionStatement CreateReturnStatement(
            ParserRuleContext context,
            ExpressionNode expression)
        {
            return new ReturnExpressionStatement(CreateLocation(context), expression);
        }

        public VoidReturnStatement CreateReturnStatement(ParserRuleContext context)
        {
            return new VoidReturnStatement(CreateLocation(context));
        }

        public BreakStatement CreateBreakStatement(ParserRuleContext context)
        {
            return new BreakStatement(CreateLocation(context));
        }

        public IndexerExpression CreateIndexerExpression(
            ParserRuleContext context,
            ExpressionNode name,
            ExpressionNode index)
        {
            return new IndexerExpression(CreateLocation(context), name, index);
        }

        public BinaryOperatorExpression CreateBinaryOperatorExpression(
            ParserRuleContext context,
            ExpressionNode left,
            ExpressionNode right,
            BinaryOperator op)
        {
            if (left == null) left = new NopExpression(CreateLocation(context));
            if (right == null) right = new NopExpression(CreateLocation(context));

            return new BinaryOperatorExpression(CreateLocation(context), left, right, op);
        }

        public UnaryExpression CreateUnaryExpression(
            ParserRuleContext context,
            UnaryOperator op,
            ExpressionNode expression)
        {
            if (expression == null)
            {
                expression = new NopExpression(CreateLocation(context));
            }

            return new UnaryExpression(CreateLocation(context), op, expression);
        }

        public AssignmentExpression CreateAssignmentExpression(
            ParserRuleContext context,
            ExpressionNode leftSide,
            ExpressionNode rightSide)
        {
            return new AssignmentExpression(CreateLocation(context), leftSide, rightSide);
        }

        public ConstantExpression CreateConstantExpression(ParserRuleContext context, ConstantNode constant)
        {
            return new ConstantExpression(CreateLocation(context), constant);
        }

        public VariableExpression CreateVariableExpression(ParserRuleContext context, VariableIdNode variableId)
        {
            return new VariableExpression(CreateLocation(context), variableId);
        }

        public ArrayCreatorExpression CreateArrayCreatorExpression(
            ParserRuleContext context,
            PrimitiveType type,
            IEnumerable<ExpressionNode> sizes)
        {
            return new ArrayCreatorExpression(
                CreateLocation(context),
                type,
                sizes.Select(m => m ?? new NopExpression(CreateLocation(context))));
        }

        public FunctionCallExpression CreateFunctionCallExpression(
            ParserRuleContext context,
            string name,
            IEnumerable<ExpressionNode> arguments)
        {
            return new FunctionCallExpression(CreateLocation(context), name, arguments.Where(m => m != null));
        }

        public IntegerConstant CreateIntegerConstant(ParserRuleContext context, string value)
        {
            long intValue;
            if (!long.TryParse(value, out intValue))
            {
                this.logger.LogError(CreateLocation(context), "The integer value is either too large or too small.");
            }

            return new IntegerConstant(CreateLocation(context), intValue);
        }

        public DoubleConstant CreateDoubleConstant(ParserRuleContext context, string value)
        {
            double doubleValue;
            if (!double.TryParse(value, NumberStyles.Float, CultureInfo.InvariantCulture, out doubleValue))
            {
                this.logger.LogError(CreateLocation(context), "The double value is either too large or too small.");
            }

            return new DoubleConstant(CreateLocation(context), doubleValue);
        }

        public StringConstant CreateStringConstant(ParserRuleContext context, string value)
        {
            return new StringConstant(CreateLocation(context), value.Substring(1, value.Length - 2));
        }

        public BooleanConstant CreateBooleanConstant(ParserRuleContext context, bool value)
        {
            return new BooleanConstant(CreateLocation(context), value);
        }

        private static Location CreateLocation(ParserRuleContext context)
        {
            return new Location(context.Start.Column, context.Start.Line);
        }
    }
}
